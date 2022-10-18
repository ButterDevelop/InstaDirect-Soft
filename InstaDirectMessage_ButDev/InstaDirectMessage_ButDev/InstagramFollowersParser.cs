using InstaDirectMessage_ButDev.JSONClasses;
using InstaDirectMessage_ButDev.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace InstaDirectMessage_ButDev
{
    public class InstagramFollowersParser
    {
        public int index = 0, AccIndex = 0, ThreadNumber = 0, good = 0, tempcount = 0;
        public string cookiesGlobal = "", UserAgent = "", csrfGlobal = "", log = "", Rival = "", end_cursor = "", Login = "", Password = "";
        public static List<string> Proxy = new List<string>();
        public static List<string> Accounts = new List<string>();
        public static string[] UserAgents;
        public static string path = "", proxytype = "", GoodPath = "";
        public const string ConstString = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_";
        public HashSet<string> SetOfGoods = new HashSet<string>();
        public static int HowManyID = 0, resulttype = 0;
        public static bool stop = false;
        public void DoResult(string s)
        {
            if (resulttype != 0)
            {
                if (resulttype != 1) s = "https://www.instagram.com/" + s + "/";
            }

            if (SetOfGoods.Contains(s)) return;

            SetOfGoods.Add(s);
            tempcount++;
            File.AppendAllText(Path.Combine(GoodPath, Rival + ".txt"), s + Environment.NewLine, Encoding.UTF8);
            good = 1;
        }

        public void Log(string x)
        {
            try
            {
                try
                {
                    File.AppendAllText(Path.Combine(path, "InstagramFollowersParser.log"), "", Encoding.UTF8);
                }
                catch { }
                string res = "[" + DateTime.Now.ToString() + ", поток " + ThreadNumber + "] " + x + Environment.NewLine;
                log += res;
                File.AppendAllText(Path.Combine(path, "InstagramFollowersParser.log"), res, Encoding.UTF8);
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ }
        }

        public InstagramFollowersParser(string rival, int indx, int indexAcc, int tt, IProgress<int> setGood)
        {
            index = indx % Proxy.Count;
            AccIndex = indexAcc % Accounts.Count;
            ThreadNumber = tt;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            UserAgent = UserAgents[rnd.Next(UserAgents.Length)];

            Rival = rival;

            int attempts = 0;
            string cookie = "";
            while (attempts < 5)
            try
            {
                Log(Translate.Tr("Парсим куки..."));
                cookie = GetCookie();
                    cookiesGlobal = cookie;
                if (cookie == "-1") { attempts++; SwapProxy(); SwapAccount(); continue; }
                Log(Translate.Tr("Получили куки..."));

                Log(Translate.Tr("Начинаем парсинг..."));

                    string UserAgent = "", device_id = "", phone_id = "", localcookie = "", adid = "", guid = "";
                    if (Accounts[AccIndex].Contains("||"))
                    {
                        // Login:Password||DeviceId;PhoneId;ADID;GUID|Cookie||
                        if (Accounts[AccIndex].Substring(Accounts[AccIndex].Length - 2) != "\\") Accounts[AccIndex] += "\\";
                        if (new Regex("(.*):(.*)\\|\\|(.*)\\|(.*)\\|\\|").IsMatch(Accounts[AccIndex]))
                        {
                            Regex regex = new Regex("(.*):(.*)\\|\\|(.*)\\|(.*)\\|\\|");
                            var mathes = regex.Match(Accounts[AccIndex]);

                            Login = mathes.Groups[1].Value;
                            Password = mathes.Groups[2].Value;
                            localcookie = mathes.Groups[4].Value;
                            localcookie = localcookie.Replace(";", "; ");

                            Regex regex2 = new Regex("(.*);(.*);(.*);(.*)");
                            var mathes_2 = regex2.Match(mathes.Groups[3].Value);
                            device_id = mathes_2.Groups[1].Value;
                            phone_id = mathes_2.Groups[2].Value;
                            guid = mathes_2.Groups[3].Value;
                            adid = mathes_2.Groups[4].Value;
                        }
                        else
                        {
                            Regex regex = new Regex("(.*):(.*)\\|(.*)\\|(.*)\\|(.*)\\|\\|");
                            var mathes = regex.Match(Accounts[AccIndex]);

                            Login = mathes.Groups[1].Value;
                            Password = mathes.Groups[2].Value;
                            UserAgent = mathes.Groups[3].Value;
                            localcookie = mathes.Groups[5].Value;
                            localcookie = localcookie.Replace(";", "; ");

                            Regex regex2 = new Regex("(.*);(.*);(.*);(.*)");
                            var mathes_2 = regex2.Match(mathes.Groups[4].Value);
                            device_id = mathes_2.Groups[1].Value;
                            phone_id = mathes_2.Groups[2].Value;
                            guid = mathes_2.Groups[3].Value;
                            adid = mathes_2.Groups[4].Value;
                        }
                    }
                    else
                    {
                        Regex regex = new Regex("(.*):(.*)");
                        var mathes = regex.Match(Accounts[AccIndex]);
                        Login = mathes.Groups[1].Value;
                        Password = mathes.Groups[2].Value;
                    }

                int result = -1;
                string logincookie = localcookie;

                if (logincookie != "")
                {
                    tempcount = 0;
                    result = GetFirstData(logincookie);
                    setGood.Report(tempcount);
                    if (result == 0) logincookie = DoLogin(cookie);
                } else
                {
                    logincookie = DoLogin(cookie);
                }

                if (logincookie == "-1") { SwapProxy(); attempts++; continue; }
                if (logincookie == "checkpoint") { SwapAccount(); continue; }

                tempcount = 0;
                result = GetFirstData(logincookie);
                setGood.Report(tempcount);
                while (attempts < 10)
                {
                    while (result != 2 && result != 0 && SetOfGoods.Count < HowManyID)
                    {
                        if (stop) return;
                        tempcount = 0;
                        result = GetData(logincookie);
                        setGood.Report(tempcount);
                        Log(Translate.Tr("Спарсили часть..."));
                    }

                    if (result == 2 || SetOfGoods.Count >= HowManyID) { Log(Translate.Tr("Парсинг конкурента \"") + Rival + Translate.Tr("\" закончен!")); return; }

                    SwapProxy();
                    SwapAccount();
                    attempts++;
                    Log(Translate.Tr("Что-то не так, пытаемся спарсить снова."));
                    Thread.Sleep(5000);
                }
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ SwapProxy(); SwapAccount(); attempts++; }
            if (attempts >= 3) { good = 0; return; }
        }

        private bool ServerCertificateValidationCallbackInstagram(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            if (certificate.GetRawCertDataString() == "3082066A30820552A00302010202100264F6B53C60714AD562A8B2E4718CF5300D06092A864886F70D01010B05003070310B300906035504061302555331153013060355040A130C446967694365727420496E6331193017060355040B13107777772E64696769636572742E636F6D312F302D06035504031326446967694365727420534841322048696768204173737572616E636520536572766572204341301E170D3230303330353030303030305A170D3230303630333132303030305A306C310B3009060355040613025553311330110603550408130A43616C69666F726E6961311330110603550407130A4D656E6C6F205061726B31173015060355040A130E46616365626F6F6B2C20496E632E311A301806035504030C112A2E692E696E7374616772616D2E636F6D30820122300D06092A864886F70D01010105000382010F003082010A0282010100B0DA2FF97882ED42D480D4977CB25DC30CC4D47B2F8F1911E2AE06594B432CAE67149BD1995B1C6F441228875A043233F7CA1F8B7A7B42FE6C1A87633D4DD4312F9582A7F9D746060EA2B78A9B6C0DF30DAA7DCE42CED632D0CF4A0FC0F66D56A37D3A61E2DA73710C3365524A4FBCA5969C087CA6255AAAF7AC8CDCD85C7D8A3F3F507DF4CCD487BF9E4534588CDF8A7C057404CAE7C3FA347C7FD883BAC76F86C91DDEBFD185F49B7A3F101C762FC9178CF0555710EC6E8F1D3DB85C382214D8A6896DF45D08AC1EA94A9CA84B04467F92A33A5524543C18DC64748BD317971C7B8E886632F3B8EB04FEAF70453FD06969CD28AF02C1F48ECE49B378ECFF990203010001A3820302308202FE301F0603551D230418301680145168FF90AF0207753CCCD9656462A212B859723B301D0603551D0E041604147389B1960FF0B0BA526ECB1968B34C49F3411D12302D0603551D110426302482112A2E692E696E7374616772616D2E636F6D820F692E696E7374616772616D2E636F6D300E0603551D0F0101FF0404030205A0301D0603551D250416301406082B0601050507030106082B0601050507030230750603551D1F046E306C3034A032A030862E687474703A2F2F63726C332E64696769636572742E636F6D2F736861322D68612D7365727665722D67362E63726C3034A032A030862E687474703A2F2F63726C342E64696769636572742E636F6D2F736861322D68612D7365727665722D67362E63726C304C0603551D2004453043303706096086480186FD6C0101302A302806082B06010505070201161C68747470733A2F2F7777772E64696769636572742E636F6D2F4350533008060667810C01020230818306082B0601050507010104773075302406082B060105050730018618687474703A2F2F6F6373702E64696769636572742E636F6D304D06082B060105050730028641687474703A2F2F636163657274732E64696769636572742E636F6D2F446967694365727453484132486967684173737572616E636553657276657243412E637274300C0603551D130101FF0402300030820103060A2B06010401D6790204020481F40481F100EF007500B21E05CC8BA2CD8A204E8766F92BB98A2520676BDAFA70E7B249532DEF8B905E00000170ACFC3E3E0000040300463044022058C561D025C0751823EF717DC6F60E008BDFC0FB1B69695012538F3B184988740220681463A5B16F42EB733142F43ADE9B3411171C9771ED1BCB6B1DE86E2E2F3348007600F095A459F200D18240102D2F93888EAD4BFE1D47E399E1D034A6B0A8AA8EB27300000170ACFC3E840000040300473045022100966D5B3DFC025B79948F2C2D54E538452AB164503D223F0ADFE368E8A48A39E3022001B7AEBF0B967B8D4ABD15605B7A96C251AA6A6A76033BAE25817D854227F72A300D06092A864886F70D01010B0500038201010043AF20ED648128FA2EA462A6D3F4E5173CA498457BC7CFA316B42804A792D07EEB9905B045C0F2218E267C6CDFFE4B99E9A54E02CC918C1FCE6FB8D0F4EB74FE3E929AA56C05AE766121FA69B3E08B1CB6DDF54A5CD76A8C288F6FB2C218EAC5E695D0D8D75F77A9E03F92D4F43A45F8B002BC226C5B79450AF8FF337C62E9FBFBD45685D1E68BEC1B316FCB2146286B3D1D118FD56F4ABDC2BB7507AABA3970235C826A40C3D2CF1056AB581A7F97EDD043E4CF7015E1C19B4CAEFBE0A1560C3EAF8B0E15BEB987E4C8F1EC25062B1D87168C8579F82821905127B0748093EDA651E6A235AB625A360F0824DCAF5339F273CCA07BE9A6F28CAF2D821A5EFF53") return true; else { Log("[ERROR] Не удаётся связаться с Instagram по защищённому соединению."); return false; }
        }
        private void SwapProxy()
        {
            index++;
            if (index >= Proxy.Count) index = 0;
            Log(Translate.Tr("[DEBUG] Сменили прокси."));
        }

        private void SwapAccount()
        {
            AccIndex++;
            if (AccIndex >= Accounts.Count) AccIndex = 0;
            Log(Translate.Tr("[DEBUG] Сменили аккаунт."));
        }

        public string GetCookie()
        {
            try
            {
                string ig_did = "", csrftoken = "", mid = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    xNet.HttpResponse response = request.Get("https://www.instagram.com");

                    try
                    {
                        var c = response.Cookies;
                        foreach (var cooke in c)
                        {
                            if (cooke.Key == "ig_did") { ig_did = cooke.Value; }
                            else
                            if (cooke.Key == "csrftoken") { csrftoken = cooke.Value; }
                            else
                            if (cooke.Key == "mid") { mid = cooke.Value; }
                        }
                    }
                    catch (Exception e) { Log("HTML: " + e.ToString()); }
                }

                if (csrftoken == "" || mid == "") { Log(Translate.Tr("[DEBUG] Не удалось спарсить Cookie. Плохие прокси?")); SwapProxy(); return "-1"; }
                csrfGlobal = csrftoken;
                string cookie = $"csrftoken={csrftoken}; ig_did={ig_did}; mid={mid};";
                Log(Translate.Tr("Спарсили куки"));
                return cookie;
            } catch(Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return "-1";
            }
        }

        public string DoLogin(string cookie)
        {
            try
            {
                string html = "", ds_user_id = "", sessionid = "", rur = "", csrftok = "";
                using (var request = new xNet.HttpRequest())
                {
                    //request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;
                    var reqParams = new RequestParams();
                    reqParams["username"] = Login;
                    reqParams["enc_password"] = $"#PWD_INSTAGRAM_BROWSER:0:{DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString()}:{Password}";

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    request.Referer = "https://www.instagram.com/accounts/login/?source=auth_switcher";
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);

                    request.AddHeader("Origin", "https://www.instagram.com");
                    request.AddHeader("X-Instagram-AJAX", "1");
                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-IG-App-ID", "936619743392459");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Cookie", cookie);

                    xNet.HttpResponse response = request.Post("https://www.instagram.com/accounts/login/ajax/", reqParams);

                    try
                    {
                        html = response.ToString();
                        Console.WriteLine("LoginHTML: " + html);
                        var c = response.Cookies;
                        foreach (var cooke in c)
                        {
                            if (cooke.Key == "sessionid") { sessionid = cooke.Value; }
                            else
                            if (cooke.Key == "rur") { rur = cooke.Value; }
                            else
                            if (cooke.Key == "csrftoken") { csrftok = cooke.Value; }
                            else
                            if (cooke.Key == "ds_user_id") { ds_user_id = cooke.Value; }
                        }

                        string X_IG_WWW_Claim = response["x-ig-set-www-claim"];
                        //MessageBox.Show(X_IG_WWW_Claim);
                    }
                    catch { }
                }

                cookie = cookie.Remove(cookie.IndexOf("rur=") + 4);
                cookie += rur + "; csrftoken=" + csrftok + ";";
                string logincookie = $"{cookie} ds_user_id={ds_user_id}; sessionid={sessionid};";
                Console.WriteLine(logincookie);

                if (html.Contains("challenge")) Log(Translate.Tr("Чекпойнт - ") + Login + ":" + Password);

                if (!html.Contains("\"status\":\"ok\"")) return "checkpoint";
                return logincookie;
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return "-1";
            }
        }

        public int GetFirstData(string cookie)
        {
            try
            {
                string html = "";
                using (var request = new xNet.HttpRequest())
                {
                    var reqParams = new RequestParams();
                    reqParams["query_hash"] = "c76146de99bb02f6415203be841dd25a";
                    reqParams["variables"] = $"{{\"id\":\"{Rival}\",\"include_reel\":false,\"fetch_mutual\":true,\"first\":50}}";

                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;

                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-IG-App-ID", "936619743392459");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Cookie", cookie);

                    try
                    {
                        var response = request.Get("https://www.instagram.com/graphql/query/", reqParams);
                        html = response.ToString();
                    }
                    catch { }

                    Console.WriteLine(html);

                    Json_Followers_Data.RootObject json = JsonConvert.DeserializeObject<Json_Followers_Data.RootObject>(html);

                    if (json.data.user.edge_followed_by.page_info.has_next_page) end_cursor = json.data.user.edge_followed_by.page_info.end_cursor;

                    foreach(var a in json.data.user.edge_followed_by.edges)
                    {
                        string x = a.node.id;
                        if (resulttype != 0) x = a.node.username;
                        DoResult(x);
                    }

                    if (json.data.user.edge_followed_by.page_info.has_next_page) return 1; else return 2;
                }
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return 0;
            }
        }

        public int GetData(string cookie)
        {
            try
            {
                string html = "";
                using (var request = new xNet.HttpRequest())
                {
                    var reqParams = new RequestParams();
                    reqParams["query_hash"] = "c76146de99bb02f6415203be841dd25a";
                    reqParams["variables"] = $"{{\"id\":\"{Rival}\",\"include_reel\":false,\"fetch_mutual\":true,\"first\":50,\"after\":\"{end_cursor}\"}}";

                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;

                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-IG-App-ID", "936619743392459");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Cookie", cookie);

                    try
                    {
                        var response = request.Get("https://www.instagram.com/graphql/query/", reqParams);
                        html = response.ToString();
                    }
                    catch { }

                    try
                    {
                        if (html.Contains("rate limited")) Log(Translate.Tr("Достигли лимита, меняем аккаунт"));
                        Console.WriteLine(html);
                    } catch { }

                    Json_Followers_Data.RootObject json = JsonConvert.DeserializeObject<Json_Followers_Data.RootObject>(html);
                    
                    if (json.data.user.edge_followed_by.page_info.has_next_page) end_cursor = json.data.user.edge_followed_by.page_info.end_cursor;

                    foreach (var a in json.data.user.edge_followed_by.edges)
                    {
                        string x = a.node.id;
                        if (resulttype != 0) x = a.node.username;
                        DoResult(x);
                    }

                    if (json.data.user.edge_followed_by.page_info.has_next_page) return 1; else return 2;
                }
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return 0;
            }
        }
    }
}
