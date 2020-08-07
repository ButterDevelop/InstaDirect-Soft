using InstaDirectMessage_ButDev.Tools;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace InstaDirectMessage_ButDev
{
    public class InstagramWebRegistrator
    {
        public int index = 0, ThreadNumber = 0, good = 0;
        public string UserAgent = "", csrfGlobal = "", log = "", Login = "", Password = "", accept_lang = "", midGlobal = "", Mail = "", MailCode = "", IMAPPassword = "", cookie = "";
        public static List<string> Proxy = new List<string>();
        public static List<string> Names = new List<string>();
        public static List<string> Surnames = new List<string>();
        public static List<string> IMAPKey = new List<string>();
        public static List<string> IMAPValue = new List<string>();
        public static string[] UserAgents;
        public static string path = "", proxytype = "", ResultPath = "", ResultUAPath = "", CheckpointPath = "", AcceptLang = "", ForceSignupCode = "";
        public const string ConstString = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_";

        public void DoBad()
        {
            good = 0;
        }
        public void DoCheckpoint(string s)
        {
            File.AppendAllText(CheckpointPath, s + Environment.NewLine, Encoding.UTF8);
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
                    File.AppendAllText(Path.Combine(path, "InstaWebRegistrator.log"), "", Encoding.UTF8);
                }
                catch { }
                string res = "[" + DateTime.Now.ToString() + Translate.Tr(", поток ") + ThreadNumber + "] " + x + Environment.NewLine;
                log += res;
                File.AppendAllText(Path.Combine(path, "InstaWebRegistrator.log"), res, Encoding.UTF8);
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ }
        }

        public InstagramWebRegistrator(string mail, string imappassword, string name, string surname, int indx, int tt)
        {
            try
            {
                Mail = mail;
                IMAPPassword = imappassword;
                index = indx % Proxy.Count;
                if (AcceptLang == "autodetect")
                    if (!AutoDetectAcceptLang()) return;
                ThreadNumber = tt;
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                UserAgent = UserAgents[rnd.Next(UserAgents.Length)];

                cookie = GetCookie();
                if (cookie == "-1")
                {
                    cookie = GetCookie();
                    if (cookie == "-1") return;
                }

                Log(Translate.Tr("Начинаем регистрацию..."));

                if (SendMail(cookie))
                {
                    int attempts = 0; bool code = false;
                    while (attempts < 3 && !code)
                    {
                        Thread.Sleep(15000);
                        attempts++;
                        //MessageBox.Show("GetMailCode Start - " + tt.ToString());
                        code = GetMailCodeIMAP();
                        if (good == 3) { Log(Translate.Tr("(Ошибка) Данные от почты не подошли! - ") + Mail + ":" + IMAPPassword); return; }
                        //MessageBox.Show("GetMailCode STOP - " + tt.ToString());
                    }
                    if (code)
                        //if (GetSignupCode(cookie))
                            if (Register(name, surname, cookie))
                            {
                                return;
                            }
                }
            } catch(Exception e) { /*MessageBox.Show(e.ToString());*/ }
        }

        private bool AutoDetectAcceptLang()
        {
            try
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

                    /*try
                    {*/
                    html = response.ToString();

                    Regex regex = new Regex("\"cc\":\"(.{2})\"}");
                    var match = regex.Match(html);
                    string countrycode = match.Groups[1].Value;

                    accept_lang = GetLanguageByCountryCode.Get(countrycode) + "-" + countrycode;
                    Log(Translate.Tr("Определили регион прокси - ") + accept_lang);
                    return true;
                    /*}
                    catch (Exception e) { Log("HTML: " + e.ToString()); }*/
                }
            }
            catch
            {
                Log(Translate.Tr("Не удалось определить регион. Плохие прокси?"));
                return false;
            }
        }

        private bool ServerCertificateValidationCallbackInstagram(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
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
            Log(Translate.Tr("Парсим куки..."));

            string rur = "", csrftoken = "", mid = "", html = "";
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
                    }
                    //Log("Html: " + response.ToString());
                    //foreach (var a in response.Cookies) MessageBox.Show(a.Key + "=" + a.Value);
                }
                catch (Exception e) { Log("HTML: " + e.ToString()); }
            }

            //($"csrftoken={csrftoken}; rur={rur}; mid={mid};");

            string cookie = $"csrftoken={csrftoken}; rur={rur}; mid={mid};";

            if (csrftoken == "" || mid == "") { Log(Translate.Tr("[DEBUG] Не удалось спарсить Cookie. Плохие прокси?")); SwapProxy(); return "-1"; }
            midGlobal = mid;
            csrfGlobal = csrftoken;
            Log(Translate.Tr("Спарсили куки"));
            return cookie;
        }

        public bool SendMail(string cookie)
        {
            Log(Translate.Tr("Пробуем отправить сообщение на почту."));
            try
            {
                string html = "", cookies = "";
                Random rnd = new Random(DateTime.Now.Millisecond * 3);

                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    var reqParams = new RequestParams();

                    reqParams["email"] = Mail;
                    reqParams["device_id"] = midGlobal;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;

                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    request.AddHeader("Accept-Language", accept_lang);
                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-Instagram-AJAX", "0f01031b87a8");
                    request.AddHeader("X-IG-App-ID", "936619743392459");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Cookie", cookie);

                    try
                    {
                        var response = request.Post("https://i.instagram.com/api/v1/accounts/send_verify_email/", reqParams);
                        html = response.ToString();
                    }
                    catch (Exception e) { /*MessageBox.Show(e.ToString());*/ }

                    //MessageBox.Show("SendMail: " + html);
                    Console.WriteLine(html);

                    if (html == "")
                    {
                        Log(Translate.Tr("Ошибка! Невозможно получить данные с сервера! Плохие прокси?"));
                        return false;
                    }

                    if (html.Contains("\"status\": \"ok\""))
                    {
                        Log(Translate.Tr("Instagram отправил письмо на на почту - ") + Mail);
                    } else
                    {
                        Log(Translate.Tr("Instagram НЕ отправил письмо на почту - ") + Mail);
                    }
                    return html.Contains("\"status\": \"ok\"");
                }
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("Ошибка во время отправки письма!"));
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public bool GetMailCodeIMAP()
        {
            Log(Translate.Tr("Пробуем зайти на почту."));
            try
            {
                if (IMAPKey.BinarySearch(Mail.Remove(0, Mail.IndexOf('@') + 1)) < 0) return false;
                string imapserver = IMAPValue[IMAPKey.BinarySearch(Mail.Remove(0, Mail.IndexOf('@') + 1))];
                var dict = imapserver.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                string imaphost = dict[0];
                int imapport = int.Parse(dict[1]);

                /*for (int i = 0; i < 3; i++)
                {*/
                /*using (ImapClient Client = new ImapClient(imaphost, imapport, Mail, IMAPPassword, AuthMethod.Login, true))
                {
                    IEnumerable<uint> uids = Client.Search(
                        SearchCondition.Unseen().And(SearchCondition.SentSince(DateTime.Today.AddDays(-1)))
                    );

                    MessageBox.Show(uids.Count().ToString());

                    IEnumerable<MailMessage> messages = Client.GetMessages(uids);

                    MessageBox.Show(messages.Count().ToString());

                    foreach (var a in messages)
                    {
                        MessageBox.Show(a.Body);
                        if (a.Body.Contains("<td style=\"padding:10px;color:#565a5c;font-size:32px;font-weight:500;text-align:center;padding-bottom:25px;\">"))
                        {
                            Regex regex = new Regex("<td style=\"padding:10px;color:#565a5c;font-size:32px;font-weight:500;text-align:center;padding-bottom:25px;\">(\\d{6})</td>");
                            var reg = regex.Match(a.Body);
                            MailCode = reg.Groups[1].Value;
                            //MessageBox.Show(MailCode);
                            Client.DeleteMessage(uids.ToList()[messages.ToList().IndexOf(a)]);
                            if (GetSignupCode(cookie)) return true;
                        }
                    }
                    return false;
                }*/

                /*System.Net.NetworkCredential objNetworkCredential = null;
                MailKit.Net.Proxy.HttpProxyClient proxyclientimaphttp = null;
                MailKit.Net.Proxy.Socks5Client proxyclientimapsocks = null;
                if (proxytype == "HTTP")
                {
                    var a = Proxy[index % Proxy.Count].Split(new char[1] { ':' });
                    if (a.Length == 4)
                    {
                        objNetworkCredential = new System.Net.NetworkCredential(a[2], a[3]);
                        proxyclientimaphttp = new MailKit.Net.Proxy.HttpProxyClient(a[0], int.Parse(a[1]), objNetworkCredential);
                    }
                    else
                    {
                        proxyclientimaphttp = new MailKit.Net.Proxy.HttpProxyClient(a[0], int.Parse(a[1]));
                    }
                }
                else
                if (proxytype != "HTTP")
                {
                    var a = Proxy[index % Proxy.Count].Split(new char[1] { ':' });
                    if (a.Length == 4)
                    {
                        objNetworkCredential = new System.Net.NetworkCredential(a[2], a[3]);
                        proxyclientimapsocks = new MailKit.Net.Proxy.Socks5Client(a[0], int.Parse(a[1]), objNetworkCredential);
                    }
                    else
                    {
                        proxyclientimapsocks = new MailKit.Net.Proxy.Socks5Client(a[0], int.Parse(a[1]));
                    }
                }*/

                using (var imapclient = new ImapClient())
                {
                    imapclient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    imapclient.ServerCertificateValidationCallback = (mysender, certificate, chain, sslPolicyErrors) => { return true; };
                    //if (proxytype == "HTTP") imapclient.ProxyClient = proxyclientimaphttp; else imapclient.ProxyClient = proxyclientimapsocks;
                    /*imapclient.CheckCertificateRevocation = false;
                    imapclient.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
                    if (imapport == 993) imapclient.Connect(imaphost, imapport, SecureSocketOptions.Auto);
                    else
                    if (imapport == 143) imapclient.Connect(imaphost, imapport, SecureSocketOptions.Auto); else imapclient.Connect(imaphost, imapport, true);*/
                    imapclient.Connect(imaphost, imapport, SecureSocketOptions.Auto);
                    imapclient.Authenticate(Mail, IMAPPassword);

                    var inbox = imapclient.Inbox; //SpecialFolder.All
                                                  //var inbox = imapclient.GetFolder(SpecialFolder.All);
                    inbox.Open(FolderAccess.ReadWrite);

                    var query = SearchQuery.All.And(SearchQuery.DeliveredAfter(DateTime.Today.AddDays(-1)));
                    foreach (var uid in inbox.Search(query))
                    {
                        var message = inbox.GetMessage(uid);
                        //MessageBox.Show(message.Subject);
                        //if (message.HtmlBody.Contains("<td style=\"padding:10px;color:#565a5c;font-size:32px;font-weight:500;text-align:center;padding-bottom:25px;\">"))
                        if (message.Subject.Contains("is your Instagram code"))
                        {
                            Regex regex = new Regex("(\\d{6}) is your Instagram code");
                            var reg = regex.Match(message.Subject);
                            MailCode = reg.Groups[1].Value;
                            //MessageBox.Show(MailCode);
                            inbox.AddFlags(uid, MessageFlags.Deleted, true);
                            if (GetSignupCode(cookie)) { imapclient.Disconnect(true); Thread.Sleep(3000); return true; }
                        }
                    }
                    imapclient.Disconnect(true);
                    Thread.Sleep(3000);
                    Log(Translate.Tr("Ни один код не подошёл! ") + Mail + ":" + IMAPPassword);
                    return false;

                    /*var a = Proxy[index % Proxy.Count].Split(new char[1] { ':' });
                    Chilkat.Imap imap = new Chilkat.Imap();
                    imap.HttpProxyHostname = a[0];
                    imap.HttpProxyPort = int.Parse(a[1]);

                    MessageBox.Show(imaphost);

                    imap.Ssl = true;
                    imap.Port = imapport;
                    bool success = imap.Connect(imaphost);
                    success = imap.Login(Mail, IMAPPassword);
                    if (success != true)
                    {
                        MessageBox.Show(imap.LastErrorText);
                        //return;
                    }
                        MessageBox.Show("1 " + success.ToString());
                    success = imap.SelectMailbox("Inbox");
                        MessageBox.Show("2 " + success.ToString());
                    bool fetchUids = true;
                    Chilkat.MessageSet messageSet = imap.Search("ALL", fetchUids);

                    // Fetch the emails into a bundle object:
                    Chilkat.EmailBundle bundle = imap.FetchBundle(messageSet);

                    int i = 0;
                    int numEmails = bundle.MessageCount;
                    while (i < numEmails)
                    {
                        Chilkat.Email email = bundle.GetEmail(i);
                        MessageBox.Show(email.From);
                        MessageBox.Show(email.Subject);
                        i = i + 1;
                    }
                    success = imap.Disconnect();
                    MessageBox.Show("3 " + success.ToString());
                    return false;*/
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                if (ex.ToString().Contains("InvalidCredentialsException") || ex.ToString().Contains("AuthenticationException")) good = 3;
                Log(Translate.Tr("Что-то не так с IMAP - ") + Mail + ":" + IMAPPassword);
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public bool GetSignupCode(string cookie)
        {
            Log(Translate.Tr("Пробуем код ") + MailCode);
            try
            {
                string html = "";
                Random rnd = new Random(DateTime.Now.Millisecond * 3);

                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    var reqParams = new RequestParams();

                    reqParams["code"] = MailCode;
                    reqParams["email"] = Mail;
                    reqParams["device_id"] = midGlobal;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;

                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    request.AddHeader("Accept-Language", accept_lang);
                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-Instagram-AJAX", "0f01031b87a8");
                    request.AddHeader("X-IG-App-ID", "936619743392459");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Cookie", cookie);

                    try
                    {
                        var response = request.Post("https://i.instagram.com/api/v1/accounts/check_confirmation_code/", reqParams);
                        html = response.ToString();
                    }
                    catch (Exception e) { /*MessageBox.Show(e.ToString());*/ }

                    //MessageBox.Show("SendMail: " + html);
                    Console.WriteLine(html);

                    if (html == "")
                    {
                        Log(Translate.Tr("Ошибка! Невозможно получить данные с сервера! Плохие прокси?"));
                        return false;
                    }

                    if (html.Contains("{\"signup_code\":"))
                    {
                        Regex regex = new Regex("\"signup_code\": \"(.*)\", \"status\": \"ok\"}");
                        var a = regex.Match(html);
                        ForceSignupCode = a.Groups[1].Value;
                        Log(Translate.Tr("Код подошёл! " + MailCode));
                        return true;
                    }
                    else
                    {
                        Log(Translate.Tr("Код не подошёл! ") + MailCode);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("Ошибка во время получения кода!"));
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }

        public bool Register(string Name, string Surname, string cookie)
        {
            try
            {
                string html = "", cookies = "";
                Random rnd = new Random(DateTime.Now.Millisecond * 3);

                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    Login = Name + "_" + Surname + rnd.Next(100, 999);
                    Password = GeneratePassword(20);

                    var reqParams = new RequestParams();

                    reqParams["email"] = Mail;
                    reqParams["enc_password"] = $"#PWD_INSTAGRAM_BROWSER:0:{DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString()}:{Password}";
                    reqParams["username"] = Login;
                    reqParams["first_name"] = Name + " " + Surname;
                    reqParams["month"] = rnd.Next(1, 12).ToString();
                    reqParams["day"] = rnd.Next(1, 28).ToString();
                    reqParams["year"] = rnd.Next(1950, 2001).ToString();
                    reqParams["client_id"] = midGlobal;
                    reqParams["seamless_login_enabled"] = "1";
                    reqParams["tos_version"] = "row";
                    reqParams["force_sign_up_code"] = ForceSignupCode;

                    request.UserAgent = UserAgent;
                    request.KeepAlive = true;

                    //request.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
                    if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                    request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                    request.AddHeader("Accept-Language", accept_lang);
                    request.AddHeader("X-IG-WWW-Claim", "0");
                    request.AddHeader("X-Instagram-AJAX", "0f01031b87a8");
                    request.AddHeader("X-IG-App-ID", "936619743392459");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("X-CSRFToken", csrfGlobal);
                    request.AddHeader("Cookie", cookie);

                    try
                    {
                        var response = request.Post("https://www.instagram.com/accounts/web_create_ajax/", reqParams);
                        var cookiesPairValue = response.Cookies.ToList();
                        foreach (var coo in cookiesPairValue)
                        {
                            cookies += coo.Key + "=" + coo.Value + ";";
                        }
                        //MessageBox.Show(accept_lang);
                        html = response.ToString();
                    }
                    catch (Exception e) { /*MessageBox.Show(e.ToString());*/ }

                    Log(Translate.Tr("Пробуем зарегистрировать аккаунт."));
                    //MessageBox.Show("Register Result: " + html);
                    Console.WriteLine(html);

                    if (html == "")
                    {
                        Log(Translate.Tr("Ошибка! Невозможно получить данные с сервера! Плохие прокси?"));
                        return false;
                    }

                    if (html.Contains("email_is_taken"))
                    {
                        DoBad();
                        Log(Translate.Tr("Ошибка! Email уже занят - ") + Login + ":" + Password + ":" + Proxy[index % Proxy.Count] + ":" + UserAgent + ":" + Mail + ":" + accept_lang);
                        return false;
                    }

                    if (html.Contains("checkpoint"))
                    {
                        DoCheckpoint(Login + ":" + Password);
                        Log(Translate.Tr("Чекпойнт - ") + Login + ":" + Password + ":" + Proxy[index % Proxy.Count] + ":" + UserAgent + ":" + Mail + ":" + accept_lang);
                    }
                    else
                    if (!html.Contains("\"account_created\": true"))
                    {
                        DoBad();
                        Log(Translate.Tr("Ошибка при регистрации - ") + Login + ":" + Password + ":" + Proxy[index % Proxy.Count] + ":" + UserAgent + ":" + Mail + ":" + accept_lang);
                    }
                    else
                    {
                        DoResult(Login + ":" + Password + "|||" + cookies + "||");
                        Log(Translate.Tr("Регистрация успешна! - ") + Login + ":" + Password + ":" + Proxy[index % Proxy.Count] + ":" + UserAgent + ":" + Mail + ":" + IMAPPassword + ":" + accept_lang);
                    }

                    return html.Contains("\"account_created\": true");
                }
            }
            catch (Exception ex)
            {
                Log(Translate.Tr("Ошибка во время процесса регистрации!"));
                //Log(Translate.Tr("(Ошибка) Текст ошибки: ") + ex.ToString());
                return false;
            }
        }
    }
}
