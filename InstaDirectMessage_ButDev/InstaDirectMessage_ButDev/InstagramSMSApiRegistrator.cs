using InstaDirectMessage_ButDev.Tools;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace InstaDirectMessage_ButDev
{
    public class InstagramSMSApiRegistrator
    {
        public int index = 0, ThreadNumber = 0, good = 0;
        public string UserAgent = "", csrfGlobal = "", midGlobal = "", log = "", Login = "", Password = "", PhoneNumber = "", SMSCode = "", id = "", accept_lang = "", phone_id = "", waterfall_id = "", guid = "", device_id = "";
        public static List<string> Proxy = new List<string>();
        public static List<string> Names = new List<string>();
        public static List<string> Surnames = new List<string>();
        public static List<string> Mails = new List<string>();
        public static List<string> UserAgentsMob = new List<string>();
        public static string path = "", proxytype = "", ResultPath = "", ResultUAPath = "", APIKey = "", CountryCode = "", Host = "", AcceptLang = "", CurrentMobileAppVersion = "138.0.0.28.117", ResultCheckpointPath = "";
        public const string ConstString = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_";

        public void DoBad()
        {
            good = 0;
        }
        public void DoCheckpoint(string s)
        {
            File.AppendAllText(ResultCheckpointPath, s + Environment.NewLine, Encoding.UTF8);
            good = 2;
        }
        public void DoResult(string s)
        {
            File.AppendAllText(ResultPath, s + Environment.NewLine, Encoding.UTF8);
            File.AppendAllText(ResultUAPath, UserAgent + Environment.NewLine, Encoding.UTF8);
            good = 1;
        }

        public void Log(string x)
        {
            try
            {
                try
                {
                    File.AppendAllText(Path.Combine(path, "InstaSMSApiRegistrator.log"), "", Encoding.UTF8);
                }
                catch { }
                string res = "[" + DateTime.Now.ToString() + Translate.Tr(", поток ") + ThreadNumber + "] " + x + Environment.NewLine;
                log += res;
                File.AppendAllText(Path.Combine(path, "InstaSMSApiRegistrator.log"), res, Encoding.UTF8);
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ }
        }

        public InstagramSMSApiRegistrator(string name, string surname, string phone, string DealID, int indx, int tt)
        {
            if (AcceptLang == "autodetect") AutoDetectAcceptLang();
            ThreadNumber = tt;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            UserAgent = UserAgentsMob[rnd.Next(UserAgentsMob.Count)];

            if (UserAgent.Contains("Instagram"))
            {
                Regex regex = new Regex("Instagram (.*) Android \\((.{1,3})\\/(.{1,6}); (.*); (.._..); (.*)\\)");
                var match = regex.Match(UserAgent);
                string version = match.Groups[1].Value;
                string num = match.Groups[2].Value;
                string android = match.Groups[3].Value;
                string other = match.Groups[4].Value;
                string num2 = match.Groups[5].Value;
                UserAgent = $"Instagram {CurrentMobileAppVersion} Android ({num}/{android}; {other}; {accept_lang}; {num2})";
            }
            else
            if (!UserAgent.ToLower().Contains("mozilla"))
            {
                Regex regex = new Regex("(.{1,3})\\/(.{1,6}); (.*); (.._..)");
                var match = regex.Match(UserAgent);
                string num = match.Groups[1].Value;
                string android = match.Groups[2].Value;
                string other = match.Groups[3].Value;
                string accept_lang = match.Groups[4].Value;
                UserAgent = $"Instagram {CurrentMobileAppVersion} Android ({num}/{android}; {other}; {accept_lang})";
            }
            try
            {
                index = indx % Proxy.Count;
                if (AcceptLang == "autodetect") AutoDetectAcceptLang();
                ThreadNumber = tt;

                string cookie = GetCookie();
                if (cookie == "-1")
                {
                    cookie = GetCookie();
                    if (cookie == "-1") return;
                }

                if (phone == "")
                {
                    if (!GetPhoneNumber()) return;
                }
                else
                {
                    id = DealID;
                    PhoneNumber = phone;
                }
                if (!SendSMS(cookie)) { Log(Translate.Tr("Не удалось отправить смс на номер, просим ещё одно!")); CancelPhone(); return; }
                Log(Translate.Tr("Запросили смс у Instagram."));

                int attempts = 0;
                while (attempts < 3)
                {
                    Thread.Sleep(20000);
                    attempts++;
                    SMSCode = GetSMS();
                    if (good == 4) { Log(Translate.Tr("Номер отменён!")); return; }
                    //MessageBox.Show(SMSCode);
                    if (SMSCode != "-1") break;
                }

                if (attempts == 3)
                {
                    if (!SendSMS(cookie)) { Log(Translate.Tr("Не удалось отправить смс на номер!")); CancelPhone(); return; }
                    Log(Translate.Tr("Запросили ещё одно смс у Instagram."));
                    attempts = 0;
                    while (attempts < 3)
                    {
                        Thread.Sleep(20000);
                        attempts++;
                        SMSCode = GetSMS();
                        //MessageBox.Show(SMSCode);
                        if (SMSCode != "-1") break;
                    }
                    if (attempts == 3) { Log(Translate.Tr("Не удалось получить смс от Instagram!")); CancelPhone(); return; }
                }

                Log(Translate.Tr("Начинаем регистрацию..."));
                bool result = Register(name, surname, cookie);
                AllowPhone();
            } catch(Exception e) { /*MessageBox.Show(e.ToString());*/ }
        }

        private void AutoDetectAcceptLang()
        {
            string html = "";
            using (var request = new xNet.HttpRequest())
            {
                request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                request.IgnoreProtocolErrors = true;

                request.AddHeader("Accept", "*/*");

                request.UserAgent = UserAgent;
                request.KeepAlive = true;
                //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                xNet.HttpResponse response = request.Get("https://api.myip.com/");

                try
                {
                    html = response.ToString();

                    Regex regex = new Regex("\"cc\":\"(.{2})\"}");
                    var match = regex.Match(html);
                    string countrycode = match.Groups[1].Value;

                    accept_lang = GetLanguageByCountryCode.Get(countrycode) + "-" + countrycode;
                    Log(Translate.Tr("Определили регион прокси - ") + accept_lang);
                }
                catch (Exception e) { Log("HTML: " + e.ToString()); }
            }
        }

        private bool ServerCertificateValidationCallbackInstagram(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            //MessageBox.Show(certificate.GetRawCertDataString());
            if (certificate.GetRawCertDataString() == "308206723082055AA00302010202100FB9ACB3933850231FD789B39FAAB837300D06092A864886F70D01010B05003070310B300906035504061302555331153013060355040A130C446967694365727420496E6331193017060355040B13107777772E64696769636572742E636F6D312F302D06035504031326446967694365727420534841322048696768204173737572616E636520536572766572204341301E170D3230303432383030303030305A170D3230303732373132303030305A306E310B3009060355040613025553311330110603550408130A43616C69666F726E6961311330110603550407130A4D656E6C6F205061726B31173015060355040A130E46616365626F6F6B2C20496E632E311C301A06035504030C132A2E7777772E696E7374616772616D2E636F6D30820122300D06092A864886F70D01010105000382010F003082010A0282010100CFB712A7F7558D49C9B34A939EDDE296BF0D1A9BF3767C93E00A14FF5BCCE5D55037ACB33F965DE95ED07A153086E29F696D50163C0AEF06A943BE7CE1AC0351DA8C3F22306157182EDDCE594B9DB3074C4C6ADFEEAEDA7E41C7F13C7552C040190D3B17ACACF79F11FE5E8D3C49C77651FB7EB2F45728F312DAFF4DE6B36BA958D261D30AC639ECB6E174E061D12C33EB884C80532C54A45E9C2771F0B201FC98D524D32B550A297B6C41746AF98BC28B5935F659504E0039D588C95585A1B30ABFDC2EA1F0EDD79BEB7E40C48F4ED18A1ED257FA4EC1870FE4C34C843B91BF366B950FA1C890B1766048EDF5B3BCE9E17AFD9EF11DE7B868CDB76280FD33590203010001A382030830820304301F0603551D230418301680145168FF90AF0207753CCCD9656462A212B859723B301D0603551D0E04160414F6AC7BCE63D13CD498136D175D9306E58BD728DF30310603551D11042A302882132A2E7777772E696E7374616772616D2E636F6D82117777772E696E7374616772616D2E636F6D300E0603551D0F0101FF0404030205A0301D0603551D250416301406082B0601050507030106082B0601050507030230750603551D1F046E306C3034A032A030862E687474703A2F2F63726C332E64696769636572742E636F6D2F736861322D68612D7365727665722D67362E63726C3034A032A030862E687474703A2F2F63726C342E64696769636572742E636F6D2F736861322D68612D7365727665722D67362E63726C304C0603551D2004453043303706096086480186FD6C0101302A302806082B06010505070201161C68747470733A2F2F7777772E64696769636572742E636F6D2F4350533008060667810C01020230818306082B0601050507010104773075302406082B060105050730018618687474703A2F2F6F6373702E64696769636572742E636F6D304D06082B060105050730028641687474703A2F2F636163657274732E64696769636572742E636F6D2F446967694365727453484132486967684173737572616E636553657276657243412E637274300C0603551D130101FF0402300030820105060A2B06010401D6790204020481F60481F300F1007600B21E05CC8BA2CD8A204E8766F92BB98A2520676BDAFA70E7B249532DEF8B905E00000171C148E43F000004030047304502204C374582FFF1B48BB66C781D6AEAA5FAC73F2DF05CDC79A3CD2E14BEDD3FF0FA02210099AD056302AA1598492ACF2E1A03760123EB37C5A1DEB350777758FDF3F56C90007700F095A459F200D18240102D2F93888EAD4BFE1D47E399E1D034A6B0A8AA8EB27300000171C148E46D0000040300483046022100A79A9F054B7B137D8CEA90BB44A920FAA9401F1910AE7034DBE1544174C3332502210085A92BE30353C86FDA4210552C8E7B398ED8AE1DFB49A4A03044BE093C74A5A8300D06092A864886F70D01010B050003820101007EF0B0B3836D5BB1749AFA5E2D52FE4DAD83E3E5BBFF293C821A8C82F79D4A2B59630226441B3EA441AAE9949E617B8A9FB7172D78086714F5F91FADCA93896F4D9C70E6179595CF6A2D7E2636E817912E5DE3620F3A8611961B96A5F8612B25FCC9DDEEB78EBD659337608677C3452043FC2B0355B63F7FB1D7D2C0353E9319351754ECBDB9BB903CDCF3B2DCA5083B6D343B4B2EDB65471A5805C6DC552AD87A1A752C9BF51E6833C134828BA7D32F9AE8610BBCF7FB25B3B7AAA47D455BB7EFF8C15408C13D1EB254A4BF024C0A7184CEA47E00827269AC09BF887000D4CE7A363362D1C87688D5D07EC7F007A625881F3C9234ED38E57DA74A0753F0B82A") return true; else { Log("[ERROR] Не удаётся связаться с Instagram по защищённому соединению."); return false; }
        }
        private void SwapProxy()
        {
            index++;
            if (index >= Proxy.Count) index = 0;
            if (AcceptLang == "autodetect") AutoDetectAcceptLang();
            Log(Translate.Tr("[DEBUG] Сменили прокси."));
        }
        private string Translit(string s)
        {
            s = s.ToUpper();

            string ret = "";
            string[] rus = {"А","Б","В","Г","Д","Е","Ё","Ж", "З","И","Й","К","Л","М","Н","О","П","Р","С","Т","У","Ф","Х", "Ц", "Ч", "Ш", "Щ",   "Ъ", "Ы","Ь", "Э","Ю", "Я" };
            string[] eng = {"A","B","V","G","D","E","E","ZH","Z","I","Y","K","L","M","N","O","P","R","S","T","U","F","KH","TS","CH","SH","SHCH",null,"Y",null,"E","YU","YA"};

            for (int j = 0; j < s.Length; j++)
                for (int i = 0; i < rus.Length; i++)
                    if (s.Substring(j, 1) == rus[i]) ret += eng[i];

            return ret.ToLower();
        }

        private string GeneratePassword(int length)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            return new string(Enumerable.Repeat(ConstString, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public bool GetPhoneNumber()
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
                    reqParams["api_key"] = APIKey;
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
                    }
                    catch (Exception e) { Log("HTML: " + e.ToString()); }

                    //MessageBox.Show("GetPhoneNumber: " + html);
                }

                if (html.Contains("ACCESS_NUMBER"))
                {
                    var s = html.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    id = s[1];
                    PhoneNumber = "+" + s[2];
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
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
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
                    reqParams["api_key"] = APIKey;
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
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
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
                    reqParams["api_key"] = APIKey;
                    reqParams["action"] = "setStatus";
                    if (Host == "http://213.166.69.162/") reqParams["status"] = "6"; else reqParams["status"] = "3";
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
                //if (html.Contains("ACCESS_ACTIVATION"))
                {
                    //Log(Translate.Tr("Приняли номер телефона."));
                    Log(Translate.Tr("Ожидаем ещё смс на этот номер."));
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public string GetCookieRequest()
        {
            string rur = "", csrftoken = "", mid = "", ig_did = "";
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            var settings = new ChromeOptions();
            settings.AddArgument("headless");
            settings.AddArgument("-incognito");

            Proxy proxy = new Proxy();
            proxy.Kind = ProxyKind.Manual;
            proxy.IsAutoDetect = false;
            if (proxytype == "HTTP")
            {
                var a = Proxy[index % Proxy.Count].Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (a.Length > 2) settings.AddArguments($"--proxy-server=http://{a[2]}:{a[3]}@{a[0]}:{a[1]}");
                else
                {
                    proxy.HttpProxy = proxy.SslProxy = Proxy[index % Proxy.Count];
                    settings.Proxy = proxy;
                }
            }
            else
            {
                var a = Proxy[index % Proxy.Count].Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                proxy.SocksProxy = a[0] + ":" + a[1];
                if (a.Length > 2)
                {
                    proxy.SocksUserName = a[2];
                    proxy.SocksPassword = a[3];
                }
                settings.Proxy = proxy;
            }

            IWebDriver webBrowser = new ChromeDriver(service, settings);
            try
            {
                webBrowser.Navigate().GoToUrl("https://www.instagram.com");

                try
                {
                    csrftoken = webBrowser.Manage().Cookies.GetCookieNamed("csrftoken").Value;
                    mid = webBrowser.Manage().Cookies.GetCookieNamed("mid").Value;
                    ig_did = webBrowser.Manage().Cookies.GetCookieNamed("ig_did").Value;
                    rur = webBrowser.Manage().Cookies.GetCookieNamed("rur").Value;
                }
                catch { }

                if (csrftoken == "" || mid == "") { Log(Translate.Tr("[DEBUG] Не удалось спарсить Cookie. Плохие прокси?")); SwapProxy(); webBrowser.Close(); return "-1"; }

                csrfGlobal = csrftoken;
                string cookie = $"ig_did={ig_did}; csrftoken={csrftoken}; rur={rur}; mid={mid};";
                webBrowser.Close();
                return cookie;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Log(Translate.Tr("[DEBUG] Не удалось спарсить Cookie. Плохие прокси?"));
                SwapProxy();
                webBrowser.Close();
                return "-1";
            }
        }

        public string GetCookie()
        {
            string rur = "", csrftoken = "", mid = "", ig_did = "", html = "";
            using (var request = new xNet.HttpRequest())
            {
                request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                request.IgnoreProtocolErrors = true;

                request.AddHeader("Accept", "*/*");

                request.UserAgent = UserAgent;
                request.KeepAlive = true;
                //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                xNet.HttpResponse response = request.Get("https://www.instagram.com");

                try
                {
                    var c = response.Cookies;
                    html = response.ToString();
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
                    //Log("Html: " + response.ToString());
                    //foreach (var a in response.Cookies) Console.WriteLine(a.Key + "=" + a.Value);
                }
                catch (Exception e) { Log("HTML: " + e.ToString()); }
            }
            using (var request = new xNet.HttpRequest())
            {
                request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                request.IgnoreProtocolErrors = true;

                request.UserAgent = UserAgent;
                request.KeepAlive = true;
                if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                xNet.HttpResponse response = request.Get("https://www.instagram.com/static/bundles/es6/Vendor.js/c911f5848b78.js");
            }
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
                        if (cooke.Key == "rur") { rur = cooke.Value; }
                        else
                        if (cooke.Key == "csrftoken") { csrftoken = cooke.Value; }
                        else
                        if (cooke.Key == "mid") { mid = cooke.Value; }
                        else
                        if (cooke.Key == "ig_did") { ig_did = cooke.Value; }
                    }
                    //Log("Html: " + response.ToString());
                    //foreach (var a in response.Cookies) MessageBox.Show(a.Key + "=" + a.Value);
                }
                catch (Exception e) { Log("HTML: " + e.ToString()); }
            }

            //($"csrftoken={csrftoken}; rur={rur}; mid={mid};");

            string cookie = $"csrftoken={csrftoken}; rur={rur}; mid={mid};";
            midGlobal = mid;

            if (csrftoken == "" || mid == "") { Log(Translate.Tr("[DEBUG] Не удалось спарсить Cookie. Плохие прокси?")); SwapProxy(); return "-1"; }
            csrfGlobal = csrftoken;
            Log(Translate.Tr("Спарсили куки"));
            return cookie;
        }

        public string GetSMS()
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
                    reqParams["api_key"] = APIKey;
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
                    }
                    catch (Exception e) { Log("HTML: " + e.ToString()); }
                }

                //MessageBox.Show("GetSMS: " + html);

                if (html.Contains("STATUS_OK") || html.Contains("STATUS_WAIT_RETRY"))
                {
                    var s = html.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    Log(Translate.Tr("Получили код ") + s[1] + Translate.Tr(" на номер ") + PhoneNumber + ", id: " + id);
                    return s[1];
                }
                else
                {
                    if (html.Contains("STATUS_CANCEL")) good = 4;
                    return "-1";
                }
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return "-1";
            }
        }

        public bool SendSMS(string cookie)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            string html = "";
            try
            {
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    phone_id = Guid.NewGuid().ToString();
                    guid = Guid.NewGuid().ToString();
                    device_id = GenerateRandomDeviceId();
                    waterfall_id = Guid.NewGuid().ToString();

                    var outStr = new Dictionary<string, string>
                    {
                        {"phone_id",                  phone_id},
                        {"phone_number",              PhoneNumber.Replace("+","")},
                        {"_csrftoken",                csrfGlobal},
                        {"guid",                      guid},
                        {"device_id",                 device_id},
                        {"waterfall_id",              waterfall_id},
                    };
                    string info = JsonConvert.SerializeObject(outStr);
                    string body = $"{HMAC_Crypt(info)}.{info}";

                    var reqParams = new RequestParams();
                    reqParams["signed_body"] = body;
                    reqParams["ig_sig_key_version"] = "4";

                    request.AddHeader("Accept-Language", accept_lang);
                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-Instagram-AJAX", "0f01031b87a8");
                    request.AddHeader("X-IG-App-ID", "936619743392459");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Cookie", cookie);

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;
                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    xNet.HttpResponse response = request.Post("https://i.instagram.com/api/v1/accounts/send_signup_sms_code/", reqParams);

                    try
                    {
                        var c = response.Cookies;
                        html = response.ToString();
                    }
                    catch (Exception e) { Log("HTML: " + e.ToString()); }

                    //MessageBox.Show("SendSMS: " + html);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
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
        public bool Register(string Name, string Surname, string cookie)
        {
            try
            {
                Random rnd = new Random(DateTime.Now.Millisecond * 3);
                string Mail = Name + Surname + rnd.Next(10000, 99999) + "@gmail.com";
                string html = "", cookies = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    Login = Name + "_" + Surname + rnd.Next(100, 999);
                    Password = GeneratePassword(20);

                    var outStr = new Dictionary<string, string>
                    {
                        {"allow_contacts_sync",       "true"},
                        {"verification_code",         SMSCode},
                        //{"sn_result",                 "API_ERROR:+null"},
                        {"sn_result" ,                "eyJhbGciOiJSUzI1NiIsIng1YyI6WyJNSUlGa3pDQ0JIdWdBd0lCQWdJUkFOY1NramRzNW42K0NBQUFBQUFwYTBjd0RRWUpLb1pJaHZjTkFRRUxCUUF3UWpFTE1Ba0dBMVVFQmhNQ1ZWTXhIakFjQmdOVkJBb1RGVWR2YjJkc1pTQlVjblZ6ZENCVFpYSjJhV05sY3pFVE1CRUdBMVVFQXhNS1IxUlRJRU5CSURGUE1UQWVGdzB5TURBeE1UTXhNVFF4TkRsYUZ3MHlNVEF4TVRFeE1UUXhORGxhTUd3eEN6QUpCZ05WQkFZVEFsVlRNUk13RVFZRFZRUUlFd3BEWVd4cFptOXlibWxoTVJZd0ZBWURWUVFIRXcxTmIzVnVkR0ZwYmlCV2FXVjNNUk13RVFZRFZRUUtFd3BIYjI5bmJHVWdURXhETVJzd0dRWURWUVFERXhKaGRIUmxjM1F1WVc1a2NtOXBaQzVqYjIwd2dnRWlNQTBHQ1NxR1NJYjNEUUVCQVFVQUE0SUJEd0F3Z2dFS0FvSUJBUUNXRXJCUVRHWkdOMWlaYk45ZWhSZ2lmV0J4cWkyUGRneHcwM1A3VHlKWmZNeGpwNUw3ajFHTmVQSzVIemRyVW9JZDF5Q0l5Qk15eHFnYXpxZ3RwWDVXcHNYVzRWZk1oSmJOMVkwOXF6cXA2SkQrMlBaZG9UVTFrRlJBTVdmTC9VdVp0azdwbVJYZ0dtNWpLRHJaOU54ZTA0dk1ZUXI4OE5xd1cva2ZaMWdUT05JVVQwV3NMVC80NTIyQlJXeGZ3eGMzUUUxK1RLV2tMQ3J2ZWs2V2xJcXlhQzUyVzdNRFI4TXBGZWJ5bVNLVHZ3Zk1Sd3lLUUxUMDNVTDR2dDQ4eUVjOHNwN3dUQUhNL1dEZzhRb3RhcmY4T0JIa25vWjkyWGl2aWFWNnRRcWhST0hDZmdtbkNYaXhmVzB3RVhDdnFpTFRiUXRVYkxzUy84SVJ0ZFhrcFFCOUFnTUJBQUdqZ2dKWU1JSUNWREFPQmdOVkhROEJBZjhFQkFNQ0JhQXdFd1lEVlIwbEJBd3dDZ1lJS3dZQkJRVUhBd0V3REFZRFZSMFRBUUgvQkFJd0FEQWRCZ05WSFE0RUZnUVU2REhCd3NBdmI1M2cvQzA3cHJUdnZ3TlFRTFl3SHdZRFZSMGpCQmd3Rm9BVW1OSDRiaERyejV2c1lKOFlrQnVnNjMwSi9Tc3daQVlJS3dZQkJRVUhBUUVFV0RCV01DY0dDQ3NHQVFVRkJ6QUJoaHRvZEhSd09pOHZiMk56Y0M1d2Eya3VaMjl2Wnk5bmRITXhiekV3S3dZSUt3WUJCUVVITUFLR0gyaDBkSEE2THk5d2Eya3VaMjl2Wnk5bmMzSXlMMGRVVXpGUE1TNWpjblF3SFFZRFZSMFJCQll3RklJU1lYUjBaWE4wTG1GdVpISnZhV1F1WTI5dE1DRUdBMVVkSUFRYU1CZ3dDQVlHWjRFTUFRSUNNQXdHQ2lzR0FRUUIxbmtDQlFNd0x3WURWUjBmQkNnd0pqQWtvQ0tnSUlZZWFIUjBjRG92TDJOeWJDNXdhMmt1WjI5dlp5OUhWRk14VHpFdVkzSnNNSUlCQkFZS0t3WUJCQUhXZVFJRUFnU0I5UVNCOGdEd0FIY0E5bHlVTDlGM01DSVVWQmdJTUpSV2p1Tk5FeGt6djk4TUx5QUx6RTd4Wk9NQUFBRnZudXkwWndBQUJBTUFTREJHQWlFQTdlLzBZUnUzd0FGbVdIMjdNMnZiVmNaL21ycCs0cmZZYy81SVBKMjlGNmdDSVFDbktDQ0FhY1ZOZVlaOENDZllkR3BCMkdzSHh1TU9Ia2EvTzQxaldlRit6Z0IxQUVTVVpTNnc3czZ2eEVBSDJLaitLTURhNW9LKzJNc3h0VC9UTTVhMXRvR29BQUFCYjU3c3RKTUFBQVFEQUVZd1JBSWdFWGJpb1BiSnA5cUMwRGoyNThERkdTUk1BVStaQjFFaVZFYmJiLzRVdk5FQ0lCaEhrQnQxOHZSbjl6RHZ5cmZ4eXVkY0hUT1NsM2dUYVlBLzd5VC9CaUg0TUEwR0NTcUdTSWIzRFFFQkN3VUFBNElCQVFESUFjUUJsbWQ4TUVnTGRycnJNYkJUQ3ZwTVhzdDUrd3gyRGxmYWpKTkpVUDRqWUZqWVVROUIzWDRFMnpmNDluWDNBeXVaRnhBcU9SbmJqLzVqa1k3YThxTUowajE5ekZPQitxZXJ4ZWMwbmhtOGdZbExiUW02c0tZN1AwZXhmcjdIdUszTWtQMXBlYzE0d0ZFVWFHcUR3VWJHZ2wvb2l6MzhGWENFK0NXOEUxUUFFVWZ2YlFQVFliS3hZait0Q05sc3MwYlRTb0wyWjJkL2ozQnBMM01GdzB5eFNLL1VUcXlrTHIyQS9NZGhKUW14aStHK01LUlNzUXI2MkFuWmF1OXE2WUZvaSs5QUVIK0E0OFh0SXlzaEx5Q1RVM0h0K2FLb2hHbnhBNXVsMVhSbXFwOEh2Y0F0MzlQOTVGWkdGSmUwdXZseWpPd0F6WHVNdTdNK1BXUmMiLCJNSUlFU2pDQ0F6S2dBd0lCQWdJTkFlTzBtcUdOaXFtQkpXbFF1REFOQmdrcWhraUc5dzBCQVFzRkFEQk1NU0F3SGdZRFZRUUxFeGRIYkc5aVlXeFRhV2R1SUZKdmIzUWdRMEVnTFNCU01qRVRNQkVHQTFVRUNoTUtSMnh2WW1Gc1UybG5iakVUTUJFR0ExVUVBeE1LUjJ4dlltRnNVMmxuYmpBZUZ3MHhOekEyTVRVd01EQXdOREphRncweU1URXlNVFV3TURBd05ESmFNRUl4Q3pBSkJnTlZCQVlUQWxWVE1SNHdIQVlEVlFRS0V4VkhiMjluYkdVZ1ZISjFjM1FnVTJWeWRtbGpaWE14RXpBUkJnTlZCQU1UQ2tkVVV5QkRRU0F4VHpFd2dnRWlNQTBHQ1NxR1NJYjNEUUVCQVFVQUE0SUJEd0F3Z2dFS0FvSUJBUURRR005RjFJdk4wNXprUU85K3ROMXBJUnZKenp5T1RIVzVEekVaaEQyZVBDbnZVQTBRazI4RmdJQ2ZLcUM5RWtzQzRUMmZXQllrL2pDZkMzUjNWWk1kUy9kTjRaS0NFUFpSckF6RHNpS1VEelJybUJCSjV3dWRnem5kSU1ZY0xlL1JHR0ZsNXlPRElLZ2pFdi9TSkgvVUwrZEVhbHROMTFCbXNLK2VRbU1GKytBY3hHTmhyNTlxTS85aWw3MUkyZE44RkdmY2Rkd3VhZWo0YlhocDBMY1FCYmp4TWNJN0pQMGFNM1Q0SStEc2F4bUtGc2JqemFUTkM5dXpwRmxnT0lnN3JSMjV4b3luVXh2OHZObWtxN3pkUEdIWGt4V1k3b0c5aitKa1J5QkFCazdYckpmb3VjQlpFcUZKSlNQazdYQTBMS1cwWTN6NW96MkQwYzF0Skt3SEFnTUJBQUdqZ2dFek1JSUJMekFPQmdOVkhROEJBZjhFQkFNQ0FZWXdIUVlEVlIwbEJCWXdGQVlJS3dZQkJRVUhBd0VHQ0NzR0FRVUZCd01DTUJJR0ExVWRFd0VCL3dRSU1BWUJBZjhDQVFBd0hRWURWUjBPQkJZRUZKalIrRzRRNjgrYjdHQ2ZHSkFib090OUNmMHJNQjhHQTFVZEl3UVlNQmFBRkp2aUIxZG5IQjdBYWdiZVdiU2FMZC9jR1lZdU1EVUdDQ3NHQVFVRkJ3RUJCQ2t3SnpBbEJnZ3JCZ0VGQlFjd0FZWVphSFIwY0RvdkwyOWpjM0F1Y0d0cExtZHZiMmN2WjNOeU1qQXlCZ05WSFI4RUt6QXBNQ2VnSmFBamhpRm9kSFJ3T2k4dlkzSnNMbkJyYVM1bmIyOW5MMmR6Y2pJdlozTnlNaTVqY213d1B3WURWUjBnQkRnd05qQTBCZ1puZ1F3QkFnSXdLakFvQmdnckJnRUZCUWNDQVJZY2FIUjBjSE02THk5d2Eya3VaMjl2Wnk5eVpYQnZjMmwwYjNKNUx6QU5CZ2txaGtpRzl3MEJBUXNGQUFPQ0FRRUFHb0ErTm5uNzh5NnBSamQ5WGxRV05hN0hUZ2laL3IzUk5Ha21VbVlIUFFxNlNjdGk5UEVhanZ3UlQyaVdUSFFyMDJmZXNxT3FCWTJFVFV3Z1pRK2xsdG9ORnZoc085dHZCQ09JYXpwc3dXQzlhSjl4anU0dFdEUUg4TlZVNllaWi9YdGVEU0dVOVl6SnFQalk4cTNNRHhyem1xZXBCQ2Y1bzhtdy93SjRhMkc2eHpVcjZGYjZUOE1jRE8yMlBMUkw2dTNNNFR6czNBMk0xajZieWtKWWk4d1dJUmRBdktMV1p1L2F4QlZielltcW13a201ekxTRFc1bklBSmJFTENRQ1p3TUg1NnQyRHZxb2Z4czZCQmNDRklaVVNweHU2eDZ0ZDBWN1N2SkNDb3NpclNtSWF0ai85ZFNTVkRRaWJldDhxLzdVSzR2NFpVTjgwYXRuWnoxeWc9PSJdfcOiYcKyFcKYGEp X8O/w45oIsKHPUTCkwTCvT3Cpm9Ec2tmw4vDjnIBKRsnwrsWK8O3w5BvWx56wrTCuMOmwr/DrMOtw7NvwpbDmsKEZi1XMMKKAzbDvBXCrMK ClJjf8OSw5dSw6AAwq/ChDJmT8KlJyE3VMOCQVoHw5zDlMObwqZqw7HDoMK8wq/CjBcrwoXCicOocApuOsK2BCLDqcKwwrQXw5ZLwoLCisKtw6EUwrDCq8OFw6DCg8KtT8KPJcK9ZMO5EMOPwrHCqcKmCF8hECzDuMKwwpvCmMKWGsOpT8O8woDDncOMw7zCvsOjw5jDn8OfwoPCscOAJBnCjcOab2TCncO0cyVDw6pcwoM="},
                        {"phone_id",                  phone_id},
                        {"phone_number",              PhoneNumber.Replace("+","")},
                        {"_csrftoken",                csrfGlobal},
                        {"username",                  Login},
                        {"first_name",                Name + " " + Surname},
                        {"adid",                      Guid.NewGuid().ToString()},
                        {"guid",                      guid},
                        {"device_id",                 device_id},
                        //{"sn_nonce",                  ""},
                        {"sn_nonce",                  "bjEudHNocWpmemNAZ21haWwuY29tfDE1OTM4NTUxODV8acO5w4BNGcOtS8ObQcOIWcK2"},
                        {"force_sign_up_code",        ""},
                        {"waterfall_id",              waterfall_id},
                        {"qs_stamp",                  ""},
                        {"password",                  Password},
                        {"has_sms_consent",           "true"}
                    };
                    string info = JsonConvert.SerializeObject(outStr);
                    string body = $"{HMAC_Crypt(info)}.{info}";

                    var reqParams = new RequestParams();
                    reqParams["signed_body"] = body;
                    reqParams["ig_sig_key_version"] = "4";

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;

                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Language", accept_lang);
                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-IG-App-ID", "3brTvw==");
                    request.AddHeader("X-Requested-With", "WIFI");
                    request.AddHeader("X-IG-App-ID", "567067343352427");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    //request.AddHeader("Cookie", cookie);

                    try
                    {
                        var response = request.Post("https://i.instagram.com/api/v1/accounts/create_validated/", reqParams);
                        var cookiesPairValue = response.Cookies.ToList();
                        foreach (var coo in cookiesPairValue)
                        {
                            cookies += coo.Key + "=" + coo.Value + ";";
                        }
                        accept_lang = response["Accept-Language"].ToString();
                        //MessageBox.Show(accept_lang);
                        html = response.ToString();
                    }
                    catch { }

                    //MessageBox.Show("Register Result: " + html);
                    Console.WriteLine(html);

                    if (html.Contains("challenge_required"))
                    {
                        DoCheckpoint(Login + ":" + Password);
                        Log(Translate.Tr("Чекпойнт - ") + Login + ":" + Password + ":" + Proxy[index % Proxy.Count] + ":" + UserAgent + ":" + Mail + ":" + accept_lang + ":" + CurrentMobileAppVersion);
                    }
                    else
                    if (!html.Contains("\"account_created\": true"))
                    {
                        DoBad();
                        Log(Translate.Tr("Ошибка при регистрации - ") + Login + ":" + Password + ":" + Proxy[index % Proxy.Count] + ":" + UserAgent + ":" + Mail + ":" + accept_lang + ":" + CurrentMobileAppVersion);
                    }
                    else
                    {
                        DoResult(Login + ":" + Password + "||" + outStr["device_id"] + ":" + outStr["guid"] + ":" + outStr["phone_id"] + "|" + cookies + "||");
                        Log(Translate.Tr("Регистрация успешна! - ") + Login + ":" + Password + ":" + Proxy[index % Proxy.Count] + ":" + UserAgent + ":" + Mail + ":" + accept_lang + ":" + CurrentMobileAppVersion);
                    }

                    return html.Contains("\"account_created\": true");
                }
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }
    }
}