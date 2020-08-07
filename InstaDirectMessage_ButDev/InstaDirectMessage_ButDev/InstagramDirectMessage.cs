using InstaDirectMessage_ButDev.JSONClasses;
using InstaDirectMessage_ButDev.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using xNet;

namespace InstaDirectMessage_ButDev
{
    public class InstagramDirectMessage
    {
        public static List<string> Proxy = new List<string>();
        public static List<string> BlackList = new List<string>();
        public static string[] UserAgentsMob, UserAgents;
        public static string BlacklistPath = "";
        public string path = "", UserAgent = "", UserAgentMob = "", Mail = "", Name = "", Password = "", logincookie = "", X_IG_WWW_Claim = "", csrfGlobal = "", 
                        ID = "", proxytype = "", TEXT = "", device_id = "", adid = Guid.NewGuid().ToString(), guid = Guid.NewGuid().ToString(), phone_id = Guid.NewGuid().ToString(), log = "";
        public int index = 0, ProxyPort = 0, tempattempts = 0, ThreadNumber = 0;
        public const string ConstString = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_";
        public int good = 0;

        public void Log(string x)
        {
            try
            {
                try
                {
                    File.AppendAllText(Path.Combine(path, "InstaDirectMessage.log"), "", Encoding.UTF8);
                } catch { }

                string res = "[" + DateTime.Now.ToString() + Translate.Tr(", поток ") + ThreadNumber + "] " + x + Environment.NewLine;
                log += res;

                File.AppendAllText(Path.Combine(path, "InstaDirectMessage.log"), res, Encoding.UTF8);
            }
            catch(Exception e) { /*MessageBox.Show(e.ToString());*/ }
        }
        private void Bad(string s)
        {
            try
            {
                File.AppendAllText(Path.Combine(path, "badAccounts_DirectMailing.txt"), s + Environment.NewLine, Encoding.UTF8);
            }
            catch { }
        }
        private void Result()
        {
            good = 1;
        }
        private bool ServerCertificateValidationCallbackInstagram(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            if (certificate.GetRawCertDataString() == "3082066A30820552A00302010202100264F6B53C60714AD562A8B2E4718CF5300D06092A864886F70D01010B05003070310B300906035504061302555331153013060355040A130C446967694365727420496E6331193017060355040B13107777772E64696769636572742E636F6D312F302D06035504031326446967694365727420534841322048696768204173737572616E636520536572766572204341301E170D3230303330353030303030305A170D3230303630333132303030305A306C310B3009060355040613025553311330110603550408130A43616C69666F726E6961311330110603550407130A4D656E6C6F205061726B31173015060355040A130E46616365626F6F6B2C20496E632E311A301806035504030C112A2E692E696E7374616772616D2E636F6D30820122300D06092A864886F70D01010105000382010F003082010A0282010100B0DA2FF97882ED42D480D4977CB25DC30CC4D47B2F8F1911E2AE06594B432CAE67149BD1995B1C6F441228875A043233F7CA1F8B7A7B42FE6C1A87633D4DD4312F9582A7F9D746060EA2B78A9B6C0DF30DAA7DCE42CED632D0CF4A0FC0F66D56A37D3A61E2DA73710C3365524A4FBCA5969C087CA6255AAAF7AC8CDCD85C7D8A3F3F507DF4CCD487BF9E4534588CDF8A7C057404CAE7C3FA347C7FD883BAC76F86C91DDEBFD185F49B7A3F101C762FC9178CF0555710EC6E8F1D3DB85C382214D8A6896DF45D08AC1EA94A9CA84B04467F92A33A5524543C18DC64748BD317971C7B8E886632F3B8EB04FEAF70453FD06969CD28AF02C1F48ECE49B378ECFF990203010001A3820302308202FE301F0603551D230418301680145168FF90AF0207753CCCD9656462A212B859723B301D0603551D0E041604147389B1960FF0B0BA526ECB1968B34C49F3411D12302D0603551D110426302482112A2E692E696E7374616772616D2E636F6D820F692E696E7374616772616D2E636F6D300E0603551D0F0101FF0404030205A0301D0603551D250416301406082B0601050507030106082B0601050507030230750603551D1F046E306C3034A032A030862E687474703A2F2F63726C332E64696769636572742E636F6D2F736861322D68612D7365727665722D67362E63726C3034A032A030862E687474703A2F2F63726C342E64696769636572742E636F6D2F736861322D68612D7365727665722D67362E63726C304C0603551D2004453043303706096086480186FD6C0101302A302806082B06010505070201161C68747470733A2F2F7777772E64696769636572742E636F6D2F4350533008060667810C01020230818306082B0601050507010104773075302406082B060105050730018618687474703A2F2F6F6373702E64696769636572742E636F6D304D06082B060105050730028641687474703A2F2F636163657274732E64696769636572742E636F6D2F446967694365727453484132486967684173737572616E636553657276657243412E637274300C0603551D130101FF0402300030820103060A2B06010401D6790204020481F40481F100EF007500B21E05CC8BA2CD8A204E8766F92BB98A2520676BDAFA70E7B249532DEF8B905E00000170ACFC3E3E0000040300463044022058C561D025C0751823EF717DC6F60E008BDFC0FB1B69695012538F3B184988740220681463A5B16F42EB733142F43ADE9B3411171C9771ED1BCB6B1DE86E2E2F3348007600F095A459F200D18240102D2F93888EAD4BFE1D47E399E1D034A6B0A8AA8EB27300000170ACFC3E840000040300473045022100966D5B3DFC025B79948F2C2D54E538452AB164503D223F0ADFE368E8A48A39E3022001B7AEBF0B967B8D4ABD15605B7A96C251AA6A6A76033BAE25817D854227F72A300D06092A864886F70D01010B0500038201010043AF20ED648128FA2EA462A6D3F4E5173CA498457BC7CFA316B42804A792D07EEB9905B045C0F2218E267C6CDFFE4B99E9A54E02CC918C1FCE6FB8D0F4EB74FE3E929AA56C05AE766121FA69B3E08B1CB6DDF54A5CD76A8C288F6FB2C218EAC5E695D0D8D75F77A9E03F92D4F43A45F8B002BC226C5B79450AF8FF337C62E9FBFBD45685D1E68BEC1B316FCB2146286B3D1D118FD56F4ABDC2BB7507AABA3970235C826A40C3D2CF1056AB581A7F97EDD043E4CF7015E1C19B4CAEFBE0A1560C3EAF8B0E15BEB987E4C8F1EC25062B1D87168C8579F82821905127B0748093EDA651E6A235AB625A360F0824DCAF5339F273CCA07BE9A6F28CAF2D821A5EFF53") return true; else { Log("[ERROR] Не удаётся связаться с Instagram по защищённому соединению."); return false; }
        }

        public string GetCookie()
        {
            string rur = "", csrftoken = "", mid = "", ig_did = "";
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
                        if (cooke.Key == "rur") { rur = cooke.Value; }
                        else
                        if (cooke.Key == "csrftoken") { csrftoken = cooke.Value; } 
                        else
                        if (cooke.Key == "mid") { mid = cooke.Value; }
                    }
                }
                catch(Exception e) { Log("HTML: " + e.ToString()); }
            }

            if (csrftoken == "" || mid == "") { Log(Translate.Tr("[DEBUG] Не удалось спарсить Cookie. Плохие прокси?")); SwapProxy(); return "-1"; }
            csrfGlobal = csrftoken;
            string cookie = $"csrftoken={csrftoken}; rur={rur}; mid={mid}; ig_did={ig_did}";
            Log(Translate.Tr("Спарсили куки"));
            return cookie;
        }

        string RandPattern(string text)
        {
            string[] Hello = { "Привет!", "Здравствуйте!", "Конишива!", "Вiтаю!", "Hello!", "Hi!", "Доброго времени суток!", "Здоровеньки були!", "Хай!", "Хелло!" };
            string[] Bye = { "До свидания!", "Пока!", "Goodbye!", "Bye!", "Да пабачэння!", "Гудбай!" };
            string[] ЕстьМинутка = { "Спешу уведомить.", "Я надолго не задержу.", "Есть минутка?", "Есть немного времени?" };
            string[] Спасибо = { "Спасибо!", "Благодарю!", "Спасибки!", "Thank you!", "Моя благодарность!" };
            Random rnd = new Random();
            text = text.Replace("{$Привет$}", Hello[rnd.Next(Hello.Length)]);
            text = text.Replace("{$Пока$}", Bye[rnd.Next(Bye.Length)]);
            text = text.Replace("{$ЕстьМинутка?$}", ЕстьМинутка[rnd.Next(ЕстьМинутка.Length)]);
            text = text.Replace("{$Спасибо$}", Спасибо[rnd.Next(Спасибо.Length)]);

            Regex regex = new Regex("\\{(.*)\\}");
            foreach (Match match in regex.Matches(text))
            {
                string m = match.Value;
                m = m.Remove(m.Length - 1);
                m = m.Remove(0, 1);
                string[] variants = m.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                text = text.Replace(match.Value, variants[rnd.Next(variants.Length)]);
            }

            return text;
        }

        public InstagramDirectMessage(string Mail, string password, string id, string cookies, string DEVICEID, string ADID, string GUID, string PHONE_ID, int Index, string Time, int tt, string type, string text, int delay, string link, string postlink, string ua)
        {
            try
            {
                Random rnd = new Random();
                ID = id;
                TEXT = RandPattern(text);
                proxytype = type;
                Password = password;
                ThreadNumber = tt;
                index = Index % Proxy.Count;
                if (index >= Proxy.Count) return;
                path = Path.Combine(Environment.CurrentDirectory, Time);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                if (DEVICEID != "") device_id = DEVICEID; else device_id = GenerateRandomDeviceId();
                if (ADID != "") adid = ADID;
                if (GUID != "") guid = GUID;
                if (PHONE_ID != "") phone_id = PHONE_ID;

                UserAgent = UserAgents[rnd.Next(UserAgents.Length)];
                if (ua == "")
                {
                    UserAgentMob = UserAgentsMob[rnd.Next(UserAgentMob.Length)];
                    UserAgentMob = UserAgentMob.Remove(UserAgentMob.Length - 1);
                }
                else UserAgentMob = ua;
            } catch(Exception e) { /*MessageBox.Show("InstagramExc: " + e.ToString());*/ return; }


            int attempts = 0;
            while (attempts < 2)
            {
                try
                {
                    attempts++;
                    Log("[DEBUG] Start...");

                    string cookie = "";
                    if (cookies != "")
                    {
                        Log(Translate.Tr("Берём заданные куки..."));
                        logincookie = cookies;
                        Regex reg = new Regex(".*csrftoken=(.*);.*");
                        var a = reg.Match(logincookie);
                        csrfGlobal = a.Groups[1].Value;
                    }
                    else
                    {
                        Log(Translate.Tr("Парсим куки..."));
                        cookie = GetCookie();
                        if (cookie == "-1") { attempts--; continue; }
                        if (!DoLogin(Mail, GetProxy(Proxy[index]), UserAgentMob, cookie)) continue;
                    }

                    bool res = DoWork(link, postlink, GetProxy(Proxy[index]), UserAgentMob, logincookie);

                    if (res)
                    {
                        Thread.Sleep(delay * 1000);
                        Log(Translate.Tr("Сообщение отправлено."));
                        Result();
                        return;
                    }
                    else
                    {
                        if (good != 0 && good != 1) return;

                        Log(Translate.Tr("[Ошибка] Что-то не так. Переделываем."));
                        cookie = GetCookie();
                        if (cookie == "-1") { attempts--; continue; }
                        if (!DoLogin(Mail, GetProxy(Proxy[index]), UserAgentMob, cookie)) continue;
                        res = DoWork(link, postlink, GetProxy(Proxy[index]), UserAgentMob, logincookie);
                        if (res) { Thread.Sleep(delay * 1000); Log(Translate.Tr("Сообщение отправлено.")); Result(); return; } else if (good != 0 && good != 1) return;
                    }
                    Thread.Sleep(delay * 1000);
                }
                catch (Exception e) { Bad(Mail + ":" + Password); Log("[DEBUG, ERROR] " + e.ToString()); SwapProxy(); Thread.Sleep(delay * 1000); continue; }
            }
        }

        public bool DoLogin(string email, WebProxy proxy, string UserAgent, string cookie)
        {
            try
            {
                var outStr = new Dictionary<string, string>
                {
                    {"phone_id",                  phone_id},
                    {"username",                  email},
                    {"adid",                      adid},
                    {"guid",                      guid},
                    {"device_id",                 device_id},
                    {"email",                     Mail},
                    {"sn_nonce",                  ""},
                    {"force_sign_up_code",        ""},
                    {"_uuid",                     Guid.NewGuid().ToString()},
                    {"google_tokens",            "[]"},
                    {"password",                 Password},
                    {"login_attempt_count",       "1"}
                };

                Random random = new Random();
                string info = JsonConvert.SerializeObject(outStr);
                string body = $"{HMAC_Crypt(info)}.{info}";

                string html = "", ds_user_id = "", sessionid = "", rur = "", csrftok = "", ds_user = "", ig_did = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;
                    var reqParams = new RequestParams();
                    reqParams["signed_body"] = body;
                    reqParams["ig_sig_key_version"] = "4";

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;

                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    request.AddHeader("X-IG-Capabilities", "3brTvw==");
                    request.AddHeader("X-IG-Connection-Type", "WIFI");
                    request.AddHeader("X-IG-App-ID", "567067343352427");
                    request.AddHeader("Cookie", cookie);

                    xNet.HttpResponse response = request.Post("https://i.instagram.com/api/v1/accounts/login/", reqParams);

                    try
                    {
                        html = response.ToString();
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
                            else
                            if (cooke.Key == "ds_user") { ds_user = cooke.Value; }
                            else
                            if (cooke.Key == "ig_did") { ig_did = cooke.Value; }
                        }
                    }
                    catch { }
                }
                logincookie = $"ig_did={ig_did}; csrftoken={csrftok}; rur={rur}; ds_user={ds_user}; ds_user_id={ds_user_id}; sessionid={sessionid};";

                //MessageBox.Show("1: " + html);

                if (html.Contains("\"status\": \"ok\""))
                {
                    Log(Translate.Tr("Вошли в аккаунт."));
                    return true;
                }
                else
                {
                    if (html.Contains("checkpoint")) { good = 2; Log(Translate.Tr("Наткнулись на чекпойнт!")); }  else Log(Translate.Tr("Войти в аккаунт не удалось!"));
                    return false;
                }
            } catch(Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public bool DoWork(string link, string postlink, WebProxy proxy, string UserAgent, string cookie)
        {
            if (BlackList.Contains(ID)) { good = 5; Console.WriteLine(ThreadNumber + " - чёрный список!"); Log(ID + Translate.Tr(" - это ID в чёрном списке.")); return false; }
            if (link != "") return DoWorkLink(link, proxy, UserAgent, cookie);
            else
                if (postlink != "") return DoWorkPost(postlink, proxy, UserAgent, cookie); else return DoWorkText(proxy, UserAgent, cookie); 
        }
        public bool DoWorkText(WebProxy proxy, string UserAgent, string cookie)
        {
            try
            {
                string html = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;
                    var reqParams = new RequestParams();
                    reqParams["text"] = TEXT;
                    reqParams["recipient_users"] = "[[" + ID + "]]";

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;

                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    request.AddHeader("X-IG-Capabilities", "3brTvw==");
                    request.AddHeader("X-IG-Connection-Type", "WIFI");
                    request.AddHeader("X-IG-App-ID", "567067343352427");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("Cookie", cookie);

                    try
                    {
                        var response = request.Post("https://i.instagram.com/api/v1/direct_v2/threads/broadcast/text/", reqParams);
                        html = response.ToString();
                    }
                    catch { }

                    //MessageBox.Show("2: " + html);
                    if (html.Contains("\"spam\": true")) { good = 3; Log(Translate.Tr("Лимит сообщений.")); return false; }
                    if (html.Contains("Unloadable")) { good = 4; Log(Translate.Tr("Не нашли данного ID!")); return false; }

                    if (html.Contains("\"status\": \"ok\""))
                    {
                        BlackList.Add(ID);
                        File.AppendAllText(BlacklistPath, ID + Environment.NewLine);
                    }
                    return html.Contains("\"status\": \"ok\"");
                }
            }
            catch(Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }
        public bool DoWorkLink(string link, WebProxy proxy, string UserAgent, string cookie)
        {
            try
            {
                string html = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;
                    var reqParams = new RequestParams();
                    reqParams["link_text"] = TEXT;
                    reqParams["link_urls"] = "[\"" + link + "\"]";
                    reqParams["action"] = "send_item";
                    reqParams["client_context"] = Guid.NewGuid().ToString();
                    reqParams["_csrftoken"] = csrfGlobal;
                    reqParams["_uuid"] = Guid.NewGuid().ToString();
                    reqParams["recipient_users"] = "[[" + ID + "]]";

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;

                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    request.AddHeader("X-IG-Capabilities", "3brTvw==");
                    request.AddHeader("X-IG-Connection-Type", "WIFI");
                    request.AddHeader("X-IG-App-ID", "567067343352427");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("Cookie", cookie);

                    try
                    {
                        var response = request.Post("https://i.instagram.com/api/v1/direct_v2/threads/broadcast/link/", reqParams);
                        html = response.ToString();
                    }
                    catch { }

                    //MessageBox.Show("3: " + html);
                    if (html.Contains("\"spam\": true")) { good = 3; Log(Translate.Tr("Лимит сообщений.")); return false; }
                    if (html.Contains("Unloadable")) { good = 4; Log(Translate.Tr("Не нашли данного ID!")); return false; }

                    if (html.Contains("\"status\": \"ok\""))
                    {
                        BlackList.Add(ID);
                        File.AppendAllText(BlacklistPath, ID + Environment.NewLine);
                    }
                    return html.Contains("\"status\": \"ok\"");
                }
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }
        public bool DoWorkPost(string postlink, WebProxy proxy, string UserAgent, string cookie)
        {
            try
            {
                string answer = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    try
                    {
                        xNet.HttpResponse response = request.Get(postlink + "?__a=1");
                        answer = response.ToString();
                        //Console.WriteLine(answer);
                    }
                    catch (Exception e) { Log("HTML: " + e.ToString()); }
                }

                Json_PostMediaInfo.Root json = JsonConvert.DeserializeObject<Json_PostMediaInfo.Root>(answer);
                if (json.graphql.shortcode_media.id == null) { Log(Translate.Tr("Не удалось загрузить пост! Пробуем ещё раз.")); return false; }

                string html = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;
                    var reqParams = new RequestParams();
                    reqParams["action"] = "send_item";
                    reqParams["client_context"] = Guid.NewGuid().ToString();
                    reqParams["_csrftoken"] = csrfGlobal;
                    reqParams["_uuid"] = Guid.NewGuid().ToString();
                    reqParams["recipient_users"] = "[[" + ID + "]]";
                    reqParams["media_id"] = json.graphql.shortcode_media.id;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;

                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    request.AddHeader("X-IG-Capabilities", "3brTvw==");
                    request.AddHeader("X-IG-Connection-Type", "WIFI");
                    request.AddHeader("X-IG-App-ID", "567067343352427");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("Cookie", cookie);

                    try
                    {
                        var response = request.Post("https://i.instagram.com/api/v1/direct_v2/threads/broadcast/media_share/", reqParams);
                        html = response.ToString();
                    }
                    catch { }

                    //MessageBox.Show("3: " + html);
                    if (html.Contains("\"spam\": true")) { good = 3; Log(Translate.Tr("Лимит сообщений.")); return false; }
                    if (html.Contains("Unloadable")) { good = 4; Log(Translate.Tr("Не нашли данного ID!")); return false; }

                    if (html.Contains("\"status\": \"ok\""))
                    {
                        BlackList.Add(ID);
                        File.AppendAllText(BlacklistPath, ID + Environment.NewLine);
                    }
                    return html.Contains("\"status\": \"ok\"");
                }
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        private WebProxy GetProxy(string s)
        {
            try
            {
                if (index >= Proxy.Count) index = 0;
                string[] dict = s.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                WebProxy webProxy = new WebProxy(dict[0] + ":" + dict[1]);
                if (dict.Length == 4) webProxy.Credentials = new NetworkCredential(dict[2], dict[3]);
                return webProxy;
            } catch { return null; }
        }

        private void SwapProxy()
        {
            index++;
            if (index >= Proxy.Count) index = 0;
            Log(Translate.Tr("[DEBUG] Сменили прокси."));
        }

        private string HMAC_Crypt(string x)
        {
            string key = "c36436a942ea1dbb40d7f2d7d45280a620d991ce8c62fb4ce600f0a048c32c11";
            var hmac = new HMACSHA256(Encoding.Default.GetBytes(key));
            byte[] array = Encoding.Default.GetBytes(x);
            return BitConverter.ToString(hmac.ComputeHash(array)).Replace("-", string.Empty).ToLower();
        }

        private string GenerateRandomDeviceId()
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            string result = BitConverter.ToString(checkSum).Replace("-", string.Empty);
            return "android-" + result.Substring(0, 16).ToLower();
        }
 
    }
}