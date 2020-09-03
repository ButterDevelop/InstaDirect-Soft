using InstaDirectMessage_ButDev.JSONClasses;
using InstaDirectMessage_ButDev.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
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
    public class InstagramAccountRecover
    {
        public bool IsLogin = false, isCaptchaPassed = false;
        public int index = 0, ThreadNumber = 0, good = 0, tempcount = 0;
        public string UserAgent = "", csrfGlobal = "", log = "", Login = "", Password = "", X_IG_WWW_Claim = "", CheckPointUrl = "", RepeatCheckPointUrl = "", SiteKeyCaptcha = "", DealID = "", PhoneNumber = "", id = "", SMSCode = "", Captcha = "-1";
        public static List<string> Proxy = new List<string>();
        public static string[] UserAgents;
        public static string path = "", proxytype = "", ResultPath = "", ApiKeyRuCaptcha = "", SmsApiKey = "", CountryCode = "", Host = "";
        public const string ConstString = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_";
        public static bool stop = false;
        public void DoResult(string s)
        {
            File.AppendAllText(ResultPath, s + Environment.NewLine, Encoding.UTF8);
            good = 1;
        }

        public void Log(string x)
        {
            try
            {
                try
                {
                    File.AppendAllText(Path.Combine(path, "InstaSMSAccountRecover.log"), "", Encoding.UTF8);
                }
                catch { }
                string res = "[" + DateTime.Now.ToString() + ", поток " + ThreadNumber + "] " + x + Environment.NewLine;
                log += res;
                File.AppendAllText(Path.Combine(path, "InstaSMSAccountRecover.log"), res, Encoding.UTF8);
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ }
        }

        public InstagramAccountRecover(string phone, string smsserviceid, string Account, int indx, int tt)
        {
            index = indx % Proxy.Count;
            ThreadNumber = tt;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            UserAgent = UserAgents[rnd.Next(UserAgents.Length)];

            Log(Translate.Tr("Взяты прокси - ") + Proxy[index % Proxy.Count]);
            Log(Translate.Tr("Взят UserAgent - ") + UserAgent);

            PhoneNumber = phone;
            id = smsserviceid;

            int attempts = 0;
            string cookie = "";
            while (attempts < 3)
            try
            {
                Captcha = "-1";
                Log(Translate.Tr("Парсим куки..."));
                cookie = GetCookie();
                if (cookie == "-1") { attempts++; SwapProxy(); continue; }
                Log(Translate.Tr("Получили куки..."));

                Log(Translate.Tr("Начинаем работу...") + " - " + Account);

                string UserAgent = "", device_id = "", phone_id = "", localcookie = "", adid = "", guid = "";
                if (Account.Contains("||"))
                {
                    // Login:Password||DeviceId;PhoneId;ADID;GUID|Cookie||
                    if (Account.Substring(Account.Length - 2) != "\\") Account += "\\";
                    if (new Regex("(.*):(.*)\\|\\|(.*)\\|(.*)\\|\\|").IsMatch(Account))
                    {
                        Regex regex = new Regex("(.*):(.*)\\|\\|(.*)\\|(.*)\\|\\|");
                        var mathes = regex.Match(Account);

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
                        var mathes = regex.Match(Account);

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
                    var mathes = regex.Match(Account);
                    Login = mathes.Groups[1].Value;
                    Password = mathes.Groups[2].Value;
                }

                string logincookie = localcookie;

                if (logincookie == "")
                {
                    logincookie = DoLogin(cookie);
                    logincookie = cookie;
                } else
                {
                    Regex reg = new Regex(".*csrftoken=(.*);.*");
                    var a = reg.Match(logincookie);
                    csrfGlobal = a.Groups[1].Value;
                }

                if (GetChallengeHTML(logincookie))
                    if (SendCaptchaToService())
                    {
                         int a = 0;
                         while (a < 5 && Captcha == "-1")
                         {
                                if (stop) return;
                             Thread.Sleep(20000);
                             Captcha = GetCaptchaFromService();
                             a++;
                         }
                                if (stop) return;
                         if (SendCaptcha(logincookie))
                         {
                             if (good == 2)
                             {
                                 good = 1;
                                 Log(Translate.Tr("Восстановили аккаунт!"));
                                 return;
                             }
                             if (GetPhoneNumber())
                                 if (SendPhoneNumber(logincookie, false))
                                 {
                                      a = 0; bool code = false;
                                      while (a < 3 && code == false)
                                      {
                                          Thread.Sleep(20000);
                                          code = GetSMS();
                                          a++;
                                      }
                                      if (!code)
                                          if (SendPhoneNumber(logincookie, true))
                                          {
                                              while (a < 3 && code == false)
                                              {
                                                  Thread.Sleep(20000);
                                                  code = GetSMS();
                                                  a++;
                                              }
                                          }

                                        if (code)
                                        {
                                            if (SendSMSCode(logincookie))
                                            {
                                                DoLogin(cookie);
                                                if (IsLogin)
                                                {
                                                    DoResult(Account);
                                                    Log(Translate.Tr("Восстановили аккаунт!"));
                                                    return;
                                                }
                                                else
                                                {
                                                    good = 0;
                                                    Log(Translate.Tr("Наткнулись на 24-х часовую проверку!"));
                                                    return;
                                                }
                                            }
                                        }
                                        else CancelPhone();
                                 }
                         }
                    }

                if (stop) return;
                Log(Translate.Tr("Что-то пошло не так, пытаемся восстановить аккаунт ещё раз."));
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ SwapProxy(); attempts++; }
            if (attempts >= 3) { good = 0; Log(Translate.Tr("Не удалось восстановить аккаунт!")); return; }
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
            Log(Translate.Tr("Взяты прокси - ") + Proxy[index % Proxy.Count]);
        }

        public string GetCookie()
        {
            try
            {
                string rur = "", csrftoken = "", mid = "", ig_did = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    xNet.HttpResponse response = request.Get("https://www.instagram.com");

                    try
                    {
                        var c = response.Cookies;
                        foreach (var cooke in c)
                        {
                            if (cooke.Key == "rur") { rur = cooke.Value; }
                            else
                            if (cooke.Key == "csrftoken") { csrftoken = cooke.Value; }
                            else
                            if (cooke.Key == "mid") { mid = cooke.Value; }
                            else
                            if (cooke.Key == "ig_did") { ig_did = cooke.Value; }
                        }
                    }
                    catch (Exception e) { Log("HTML: " + e.ToString()); }
                }

                if (csrftoken == "" || mid == "") { Log(Translate.Tr("[DEBUG] Не удалось спарсить Cookie. Плохие прокси?")); SwapProxy(); return "-1"; }
                csrfGlobal = csrftoken;
                string cookie = $"csrftoken={csrftoken}; rur={rur}; mid={mid}; ig_did={ig_did}";
                Log(Translate.Tr("Спарсили куки"));
                return cookie;
            } catch(Exception ex)
            {
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                return "-1";
            }
        }

        public string DoLogin(string cookie)
        {
            try
            {
                string html = "", mid = "", csrftok = "", ig_did = "";
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
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
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
                        //MessageBox.Show("DoLogin: " + html);

                        if (!html.Contains("checkpoint_required")) IsLogin = true; else IsLogin = false;

                        Regex regex = new Regex("\"message\": \"checkpoint_required\", \"checkpoint_url\": \"(.*)\", \"lock\": .*, \"status\": \"fail\"");
                        var a = regex.Match(html);
                        CheckPointUrl = "https://www.instagram.com" + a.Groups[1].Value;
                        RepeatCheckPointUrl = "https://www.instagram.com/challenge/replay" + a.Groups[1].Value.Remove(0, 10);

                        Console.WriteLine("LoginHTML: " + html);
                        var c = response.Cookies;
                        foreach (var cooke in c)
                        {
                            if (cooke.Key == "mid") { mid = cooke.Value; }
                            else
                            if (cooke.Key == "csrftoken") { csrftok = cooke.Value; }
                            else
                            if (cooke.Key == "ig_did") { ig_did = cooke.Value; }
                        }

                        X_IG_WWW_Claim = response["x-ig-set-www-claim"];
                        //MessageBox.Show(X_IG_WWW_Claim);
                    }
                    catch { }
                }

                csrfGlobal = csrftok;
                string logcookie = $"csrftoken={csrftok}; ig_did={ig_did}; mid={mid}";
                Console.WriteLine(logcookie);

                return logcookie;
            }
            catch (Exception ex)
            {
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                return "-1";
            }
        }

        public bool GetChallengeHTML(string cookie)
        {
            try
            {
                string html = "", answer = "";
                using (var request = new xNet.HttpRequest())
                {
                    //request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    request.Referer = $"https://www.instagram.com/";
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);

                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.AddHeader("Cookie", cookie);

                    xNet.HttpResponse response = request.Get(CheckPointUrl);

                    try
                    {
                        html = response.ToString();
                        //MessageBox.Show("GetChallengeHTML: " + html);

                        /*Regex regex = new Regex(",\"sitekey\":\"(.*)\",\"code_whitelisted\"");
                        var a = regex.Match(html);
                        SiteKeyCaptcha = a.Groups[1].Value;*/
                    }
                    catch { }
                }

                using (var request = new xNet.HttpRequest())
                {
                    //request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    RequestParams reqParams = new RequestParams();
                    reqParams["compact"] = "0";
                    reqParams["referer"] = "https://www.instagram.com";
                    reqParams["locale"] = "ru_RU";

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);

                    request.AddHeader("Accept", "*/*");

                    xNet.HttpResponse response = request.Get("https://www.fbsbx.com/captcha/recaptcha/iframe/", reqParams);

                    try
                    {
                        answer = response.ToString();
                        //MessageBox.Show("GetCaptchaFBHTML: " + answer);

                        Regex regex = new Regex("<div class=\"g-recaptcha\" data-callback=\"successCallback\" data-sitekey=\"(.*)\" data-theme=\"light\" data-size=\"normal\"></div>");
                        var a = regex.Match(answer);
                        SiteKeyCaptcha = a.Groups[1].Value;
                        //MessageBox.Show(SiteKeyCaptcha);
                    }
                    catch { }
                }

                return (SiteKeyCaptcha != "");
            }
            catch (Exception ex)
            {
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                return false;
            }
        }

        public bool SendCaptchaToService()
        {
            try
            {
                if (isCaptchaPassed) { Captcha = "passed"; return true; }

                string html = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.IgnoreProtocolErrors = true;

                    RequestParams reqParams = new RequestParams();
                    reqParams["key"] = ApiKeyRuCaptcha;
                    reqParams["method"] = "userrecaptcha";
                    reqParams["googlekey"] = SiteKeyCaptcha;
                    reqParams["pageurl"] = "https://www.fbsbx.com/captcha/recaptcha/iframe/";//CheckPointUrl;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    //if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);

                    request.AddHeader("Accept", "*/*");

                    xNet.HttpResponse response = request.Post("https://rucaptcha.com/in.php", reqParams);

                    try
                    {
                        html = response.ToString();
                        //MessageBox.Show("SendCaptchaToService: " + html);

                        var a = html.Split(new char[1] { '|' });
                        DealID = a[1];
                    }
                    catch { }
                    //MessageBox.Show(html);
                }

                if (html.Contains("OK")) Log(Translate.Tr("Отправили капчу на решение"));
                return html.Contains("OK");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public string GetCaptchaFromService()
        {
            try
            {
                string html = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.IgnoreProtocolErrors = true;

                    RequestParams reqParams = new RequestParams();
                    reqParams["key"] = ApiKeyRuCaptcha;
                    reqParams["action"] = "get";
                    reqParams["id"] = DealID;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    //if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);

                    request.AddHeader("Accept", "*/*");

                    xNet.HttpResponse response = request.Post("https://rucaptcha.com/res.php", reqParams);

                    try
                    {
                        html = response.ToString();
                        //MessageBox.Show("GetCaptchaFromService: " + html);
                    }
                    catch { }
                    //MessageBox.Show(html);
                }

                if (html.Contains("CAPCHA_NOT_READY")) return "-1";

                var a = html.Split(new char[1] { '|' });
                html = a[1];

                Log(Translate.Tr("Получили решение капчи"));
                return html;
            }
            catch (Exception ex)
            {
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                return "-1";
            }
        }

        public bool SendCaptcha(string cookie)
        {
            try
            {
                if (isCaptchaPassed) return true;

                string html = "";
                using (var request = new xNet.HttpRequest())
                {
                    //request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    RequestParams reqParams = new RequestParams();
                    reqParams["g-recaptcha-response"] = Captcha;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    request.Referer = CheckPointUrl;
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);

                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-IG-App-ID", "936619743392459");
                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-Instagram-AJAX", "9fc13b1a222e");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Cookie", cookie);

                    xNet.HttpResponse response = request.Post(CheckPointUrl, reqParams);

                    try
                    {
                        html = response.ToString();
                        //MessageBox.Show("SendCaptcha: " + html);
                    }
                    catch { }
                    //MessageBox.Show(html);
                }
                if (html.Contains("{\"location\": \"https://www.instagram.com/\", \"type\": \"CHALLENGE_REDIRECTION\", \"status\": \"ok\"}")) good = 2;

                if (html.Contains("\"status\": \"ok\"")) isCaptchaPassed = true;
                return html.Contains("\"status\": \"ok\"") || html.Contains("\"challengeType\": \"SubmitPhoneNumberForm\"");
            }
            catch (Exception ex)
            {
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                return false;
            }
        }

        public bool GetPhoneNumber()
        {
            if (PhoneNumber != "") return true;

            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            string html = "";
            try
            {
                using (var request = new xNet.HttpRequest())
                {
                    //request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    var reqParams = new RequestParams();
                    reqParams["api_key"] = SmsApiKey;
                    reqParams["action"] = "getNumber";
                    reqParams["service"] = "ig";
                    reqParams["operator"] = "any";
                    reqParams["country"] = CountryCode;

                    request.AddHeader("Accept", "*/*");

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    //if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    //request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    xNet.HttpResponse response = request.Get(Host + "stubs/handler_api.php", reqParams);

                    try
                    {
                        var c = response.Cookies;
                        html = response.ToString();
                        //MessageBox.Show("GetPhoneNumber: " + html);
                    }
                    catch (Exception e) { Log("HTML: " + e.ToString()); }
                }

                if (html.Contains("ACCESS_NUMBER"))
                {
                    var s = html.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    id = s[1];
                    PhoneNumber = s[2];
                    Log(Translate.Tr("Взяли в сервисе номер - ") + PhoneNumber + ", id: " + id);
                    return true;
                }
                else
                if (html == "NO_NUMBERS")
                {
                    Log(Translate.Tr("В смс сервисе нет свободных номеров!"));
                    return false;
                }
                else
                if (html == "NO_BALANCE")
                {
                    Log(Translate.Tr("В смс сервисе закончились Ваши средства!"));
                    return false;
                }

                Log(Translate.Tr("Не удалось получить номер в смс сервисе!"));
                return false;
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public bool SendPhoneNumber(string cookie, bool repeat)
        {
            try
            {
                string html = "";
                using (var request = new xNet.HttpRequest())
                {
                    //request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    RequestParams reqParams = new RequestParams();
                    reqParams["phone_number"] = PhoneNumber;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    request.Referer = $"https://www.instagram.com/";
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);

                    request.AddHeader("X-IG-App-ID", "936619743392459");
                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-Instagram-AJAX", "9fc13b1a222e");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Cookie", cookie);

                    string url = "";
                    if (repeat) url = RepeatCheckPointUrl; else url = CheckPointUrl;
                    xNet.HttpResponse response = request.Post(url, reqParams);

                    try
                    {
                        html = response.ToString();
                        //MessageBox.Show("SendPhoneNumber, repeat - " + repeat.ToString() + ": " + html);
                    }
                    catch { }
                    //MessageBox.Show(html);
                }

                if (html.Contains("\"status\": \"ok\"")) Log(Translate.Tr("Ждём смс на номер ") + PhoneNumber);
                return html.Contains("\"status\": \"ok\"");
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public bool GetSMS()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            string html = "";
            try
            {
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    var reqParams = new RequestParams();
                    reqParams["api_key"] = SmsApiKey;
                    reqParams["action"] = "getStatus";
                    reqParams["id"] = id;

                    request.AddHeader("Accept", "*/*");

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    //if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    //request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    xNet.HttpResponse response = request.Get(Host + "stubs/handler_api.php", reqParams);

                    try
                    {
                        var c = response.Cookies;
                        html = response.ToString();
                        //MessageBox.Show("GetSMS: " + html);
                    }
                    catch (Exception e) { Log("HTML: " + e.ToString()); }
                }

                if (html.Contains("STATUS_OK") || html.Contains("STATUS_WAIT_RETRY"))
                {
                    var s = html.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    Log(Translate.Tr("Получили код ") + s[1] + Translate.Tr(" на номер ") + PhoneNumber + ", id: " + id);
                    SMSCode = s[1];
                    AllowPhone();
                    return true;
                }
                else
                {
                    if (html.Contains("STATUS_CANCEL")) good = 4;
                    Log(Translate.Tr("СМС ещё не пришло."));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public bool AllowPhone()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            string html = "";
            try
            {
                using (var request = new xNet.HttpRequest())
                {
                    //request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    var reqParams = new RequestParams();
                    reqParams["api_key"] = SmsApiKey;
                    reqParams["action"] = "setStatus";
                    if (Host != "https://sms-activate.ru/") reqParams["status"] = "6"; else reqParams["status"] = "3";
                    reqParams["id"] = id;

                    request.AddHeader("Accept", "*/*");

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    //if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    //request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    xNet.HttpResponse response = request.Get(Host + "stubs/handler_api.php", reqParams);

                    try
                    {
                        var c = response.Cookies;
                        html = response.ToString();
                    }
                    catch (Exception e) { Log("HTML: " + e.ToString()); }
                }

                //MessageBox.Show("AllowPhone: " + html);

                if (html.Contains("ACCESS_RETRY_GET"))
                {
                    Log(Translate.Tr("Ожидаем ещё смс на этот номер."));
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public bool CancelPhone()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            string html = "";
            try
            {
                using (var request = new xNet.HttpRequest())
                {
                    //request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    var reqParams = new RequestParams();
                    reqParams["api_key"] = SmsApiKey;
                    reqParams["action"] = "setStatus";
                    reqParams["status"] = "8";
                    reqParams["id"] = id;

                    request.AddHeader("Accept", "*/*");

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    //if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    //request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    xNet.HttpResponse response = request.Get(Host + "stubs/handler_api.php", reqParams);

                    try
                    {
                        var c = response.Cookies;
                        html = response.ToString();
                    }
                    catch (Exception e) { Log("HTML: " + e.ToString()); }
                }

                //MessageBox.Show("CancelPhone: " + html);

                if (html.Contains("ACCESS_CANCEL"))
                {
                    Log(Translate.Tr("Отменили номер. Деньги возвращены."));
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public bool SendSMSCode(string cookie)
        {
            try
            {
                string html = "";
                using (var request = new xNet.HttpRequest())
                {
                    //request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    RequestParams reqParams = new RequestParams();
                    reqParams["security_code"] = SMSCode;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    request.Referer = $"https://www.instagram.com/";
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);

                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-IG-App-ID", "936619743392459");
                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-Instagram-AJAX", "9fc13b1a222e");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Cookie", cookie);

                    xNet.HttpResponse response = request.Post(CheckPointUrl, reqParams);

                    try
                    {
                        html = response.ToString();
                        //MessageBox.Show("SendSMSCode: " + html);
                    }
                    catch { }
                    //MessageBox.Show(html);
                }

                return html.Contains("\"status\": \"ok\"");
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("Ошибка. Плохие прокси?"));
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }
    }
}
