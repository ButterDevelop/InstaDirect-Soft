using InstaDirectMessage_ButDev.Tools;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Newtonsoft.Json;
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
    public class InstagramApiRegistrator
    {
        public int index = 0, ThreadNumber = 0, good = 0;
        public string UserAgent = "", csrfGlobal = "", log = "", Login = "", Password = "", accept_lang = "", Mail = "", IMAPPassword = "";
        public static List<string> Proxy = new List<string>();
        public static List<string> Names = new List<string>();
        public static List<string> Surnames = new List<string>();
        public static List<string> Mails = new List<string>();
        public static List<string> UserAgentsMob = new List<string>();
        public static List<string> IMAPKey = new List<string>();
        public static List<string> IMAPValue = new List<string>();
        public static string path = "", proxytype = "", ResultPath = "", ResultUAPath = "", CheckpointPath = "", AcceptLang = "", CurrentMobileAppVersion = "138.0.0.28.117", ResultCheckpointPath = "";
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
                    File.AppendAllText(Path.Combine(path, "InstaApiRegistrator.log"), "", Encoding.UTF8);
                }
                catch { }
                string res = "[" + DateTime.Now.ToString() + Translate.Tr(", поток ") + ThreadNumber + "] " + x + Environment.NewLine;
                log += res;
                File.AppendAllText(Path.Combine(path, "InstaApiRegistrator.log"), res, Encoding.UTF8);
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ }
        }

        public InstagramApiRegistrator(string mail, string imappassword, string name, string surname, int indx, int tt)
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
                } else
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
                /*UserAgent = UserAgent.Remove(0, UserAgent.IndexOf(" ") + 1);
                UserAgent = UserAgent.Remove(0, UserAgent.IndexOf(" ") + 1);
                UserAgent = "Instagram " + CurrentMobileAppVersion + " " + UserAgent;*/
                //UserAgent = Regex.Replace(UserAgent, "Instagram (.*) Android \\(.*\\)", CurrentMobileAppVersion);


                string cookie = GetCookie();
                if (cookie == "-1")
                {
                    cookie = GetCookie();
                    if (cookie == "-1") return;
                }

                bool result = Register(name, surname, cookie);
                if (result)
                    if (ConfirmIMAP())
                    {
                        return;
                    } else
                    {
                        Log(Translate.Tr("Не удалось подтвердить почту! - ") + Mail + ":" + IMAPPassword);
                        good = 0;
                        return;
                    }
            } catch(Exception e) { /*MessageBox.Show(e.ToString());*/ }
        }

        private bool ConfirmIMAP()
        {
            Log(Translate.Tr("Пробуем зайти на почту."));
            try
            {
                if (IMAPKey.BinarySearch(Mail.Remove(0, Mail.IndexOf('@') + 1)) < 0) return false;
                string imapserver = IMAPValue[IMAPKey.BinarySearch(Mail.Remove(0, Mail.IndexOf('@') + 1))];
                var dict = imapserver.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                string imaphost = dict[0];
                int imapport = int.Parse(dict[1]);

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
                    //imapclient.Connect(imaphost, imapport, true);
                    imapclient.Authenticate(Mail, IMAPPassword);

                    var inbox = imapclient.Inbox; //SpecialFolder.All
                                                  //var inbox = imapclient.GetFolder(SpecialFolder.All);
                    inbox.Open(FolderAccess.ReadWrite);

                    var query = SearchQuery.All.And(SearchQuery.DeliveredAfter(DateTime.Today.AddDays(-1)));
                    foreach (var uid in inbox.Search(query))
                    {
                        var message = inbox.GetMessage(uid);
                        if (message.HtmlBody.Contains("<a href=\"https://www.instagram.com/_n/confirm_email_deeplink"))
                        {
                            MessageBox.Show(message.Subject);
                            string link = "", html = "";
                            Regex regex = new Regex("<a href=\"https://www.instagram.com/_n/confirm_email_deeplink\\?(.*)\" style=\"color:#3b5998;text-decoration:none;display:block;width:370px;\"><table border=\"0\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" style=\"border-collapse:collapse;\"><tr><td style=\"border-collapse:collapse;border-radius:3px;text-align:center;display:block;border:solid 1px #009fdf;padding:10px 16px 14px 16px;margin:0 2px 0 auto;min-width:80px;background-color:#47A2EA;\"><a href=\"https://www.instagram.com/_n/confirm_email_deeplink?.*\" style=\"color:#3b5998;text-decoration:none;display:block;\"><center><font size=\"3\"><span style=\"font-family:Helvetica Neue,Helvetica,Roboto,Arial,sans-serif;white-space:nowrap;font-weight:bold;vertical-align:middle;color:#fdfdfd;font-size:16px;line-height:16px;\">.*</span></font></center></a></td></tr></table></a></td></tr><tr style=\"\"><td height=\"30\" style=\"line-height:30px;\" colspan=\"3\">&nbsp;</td></tr><tr><td style=\"border-top:solid 1px #DBDBDB;\"></td></tr></table></td><td width=\"30\" style=\"display:block;width:30px;\">&nbsp;&nbsp;&nbsp;</td></tr></table></td></tr><tr style=\"\"><td height=\"25\" style=\"line-height:25px;\" colspan=\"3\">&nbsp;</td></tr></table></td><td width=\"15\" style=\"display:block;width:15px;\">&nbsp;&nbsp;&nbsp;</td></tr><tr style=\"\"><td width=\"15\" style=\"display:block;width:15px;\">&nbsp;&nbsp;&nbsp;</td><td style=\"\"><table border=\"0\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" style=\"border-collapse:collapse;\"><tr><td style=\"\"><img src=\"https://static.xx.fbcdn.net/rsrc.php/v3/yd/r/1vEc4R7__Ok.png\" width=\"430\" style=\"border:0;width:430px;\" /></td></tr><tr style=\"\">");
                            var reg = regex.Match(message.HtmlBody);
                            link = "https://www.instagram.com/_n/confirm_email_deeplink?" + reg.Groups[1].Value;

                            inbox.AddFlags(uid, MessageFlags.Deleted, true);
                            using (var request = new xNet.HttpRequest())
                            {
                                request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                                request.IgnoreProtocolErrors = true;
                                request.UserAgent = UserAgent;
                                request.KeepAlive = true;
                                if (proxytype == "HTTP") request.Proxy = HttpProxyClient.Parse(Proxy[index % Proxy.Count]); else request.Proxy = Socks5ProxyClient.Parse(Proxy[index % Proxy.Count]);
                                request.Proxy.ConnectTimeout = Properties.Settings.Default.ProxyTimeout * 1000;

                                xNet.HttpResponse response = request.Get(link);

                                try
                                {
                                    html = response.ToString();
                                }
                                catch (Exception e) { Log("HTML: " + e.ToString()); }
                            }
                        }
                    }
                    imapclient.Disconnect(true);
                    Thread.Sleep(3000);
                    Log(Translate.Tr("Ни один код не подошёл! ") + Mail + ":" + IMAPPassword);
                    return false;
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
            if (certificate.GetRawCertDataString() == "3082066A30820552A00302010202100264F6B53C60714AD562A8B2E4718CF5300D06092A864886F70D01010B05003070310B300906035504061302555331153013060355040A130C446967694365727420496E6331193017060355040B13107777772E64696769636572742E636F6D312F302D06035504031326446967694365727420534841322048696768204173737572616E636520536572766572204341301E170D3230303330353030303030305A170D3230303630333132303030305A306C310B3009060355040613025553311330110603550408130A43616C69666F726E6961311330110603550407130A4D656E6C6F205061726B31173015060355040A130E46616365626F6F6B2C20496E632E311A301806035504030C112A2E692E696E7374616772616D2E636F6D30820122300D06092A864886F70D01010105000382010F003082010A0282010100B0DA2FF97882ED42D480D4977CB25DC30CC4D47B2F8F1911E2AE06594B432CAE67149BD1995B1C6F441228875A043233F7CA1F8B7A7B42FE6C1A87633D4DD4312F9582A7F9D746060EA2B78A9B6C0DF30DAA7DCE42CED632D0CF4A0FC0F66D56A37D3A61E2DA73710C3365524A4FBCA5969C087CA6255AAAF7AC8CDCD85C7D8A3F3F507DF4CCD487BF9E4534588CDF8A7C057404CAE7C3FA347C7FD883BAC76F86C91DDEBFD185F49B7A3F101C762FC9178CF0555710EC6E8F1D3DB85C382214D8A6896DF45D08AC1EA94A9CA84B04467F92A33A5524543C18DC64748BD317971C7B8E886632F3B8EB04FEAF70453FD06969CD28AF02C1F48ECE49B378ECFF990203010001A3820302308202FE301F0603551D230418301680145168FF90AF0207753CCCD9656462A212B859723B301D0603551D0E041604147389B1960FF0B0BA526ECB1968B34C49F3411D12302D0603551D110426302482112A2E692E696E7374616772616D2E636F6D820F692E696E7374616772616D2E636F6D300E0603551D0F0101FF0404030205A0301D0603551D250416301406082B0601050507030106082B0601050507030230750603551D1F046E306C3034A032A030862E687474703A2F2F63726C332E64696769636572742E636F6D2F736861322D68612D7365727665722D67362E63726C3034A032A030862E687474703A2F2F63726C342E64696769636572742E636F6D2F736861322D68612D7365727665722D67362E63726C304C0603551D2004453043303706096086480186FD6C0101302A302806082B06010505070201161C68747470733A2F2F7777772E64696769636572742E636F6D2F4350533008060667810C01020230818306082B0601050507010104773075302406082B060105050730018618687474703A2F2F6F6373702E64696769636572742E636F6D304D06082B060105050730028641687474703A2F2F636163657274732E64696769636572742E636F6D2F446967694365727453484132486967684173737572616E636553657276657243412E637274300C0603551D130101FF0402300030820103060A2B06010401D6790204020481F40481F100EF007500B21E05CC8BA2CD8A204E8766F92BB98A2520676BDAFA70E7B249532DEF8B905E00000170ACFC3E3E0000040300463044022058C561D025C0751823EF717DC6F60E008BDFC0FB1B69695012538F3B184988740220681463A5B16F42EB733142F43ADE9B3411171C9771ED1BCB6B1DE86E2E2F3348007600F095A459F200D18240102D2F93888EAD4BFE1D47E399E1D034A6B0A8AA8EB27300000170ACFC3E840000040300473045022100966D5B3DFC025B79948F2C2D54E538452AB164503D223F0ADFE368E8A48A39E3022001B7AEBF0B967B8D4ABD15605B7A96C251AA6A6A76033BAE25817D854227F72A300D06092A864886F70D01010B0500038201010043AF20ED648128FA2EA462A6D3F4E5173CA498457BC7CFA316B42804A792D07EEB9905B045C0F2218E267C6CDFFE4B99E9A54E02CC918C1FCE6FB8D0F4EB74FE3E929AA56C05AE766121FA69B3E08B1CB6DDF54A5CD76A8C288F6FB2C218EAC5E695D0D8D75F77A9E03F92D4F43A45F8B002BC226C5B79450AF8FF337C62E9FBFBD45685D1E68BEC1B316FCB2146286B3D1D118FD56F4ABDC2BB7507AABA3970235C826A40C3D2CF1056AB581A7F97EDD043E4CF7015E1C19B4CAEFBE0A1560C3EAF8B0E15BEB987E4C8F1EC25062B1D87168C8579F82821905127B0748093EDA651E6A235AB625A360F0824DCAF5339F273CCA07BE9A6F28CAF2D821A5EFF53") return true; else { Log("[ERROR] Не удаётся связаться с Instagram по защищённому соединению."); return false; }
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

        public string GetCookie()
        {
            string rur = "", csrftoken = "", mid = "";
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

            if (csrftoken == "" || mid == "") { Log(Translate.Tr("[DEBUG] Не удалось спарсить Cookie. Плохие прокси?")); SwapProxy(); return "-1"; }
            csrfGlobal = csrftoken;
            string cookie = $"csrftoken={csrftoken}; rur={rur}; mid={mid};";
            Log(Translate.Tr("Спарсили куки"));
            return cookie;
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
                string html = "", cookies = "";
                using (var request = new xNet.HttpRequest())
                {
                    request.SslCertificateValidatorCallback += ServerCertificateValidationCallbackInstagram;
                    request.IgnoreProtocolErrors = true;

                    Login = Name + "_" + Surname + rnd.Next(100, 999);
                    Password = GeneratePassword(20);

                    var outStr = new Dictionary<string, string>
                    {
                        {"tos_version",               "row"},
                        //{"allow_contacts_sync",       "true"},
                        {"suggestedUsername",         ""},
                        //{"sn_result",                 "API_ERROR:+null"},
                        {"sn_result" ,                "eyJhbGciOiJSUzI1NiIsIng1YyI6WyJNSUlGa3pDQ0JIdWdBd0lCQWdJUkFOY1NramRzNW42K0NBQUFBQUFwYTBjd0RRWUpLb1pJaHZjTkFRRUxCUUF3UWpFTE1Ba0dBMVVFQmhNQ1ZWTXhIakFjQmdOVkJBb1RGVWR2YjJkc1pTQlVjblZ6ZENCVFpYSjJhV05sY3pFVE1CRUdBMVVFQXhNS1IxUlRJRU5CSURGUE1UQWVGdzB5TURBeE1UTXhNVFF4TkRsYUZ3MHlNVEF4TVRFeE1UUXhORGxhTUd3eEN6QUpCZ05WQkFZVEFsVlRNUk13RVFZRFZRUUlFd3BEWVd4cFptOXlibWxoTVJZd0ZBWURWUVFIRXcxTmIzVnVkR0ZwYmlCV2FXVjNNUk13RVFZRFZRUUtFd3BIYjI5bmJHVWdURXhETVJzd0dRWURWUVFERXhKaGRIUmxjM1F1WVc1a2NtOXBaQzVqYjIwd2dnRWlNQTBHQ1NxR1NJYjNEUUVCQVFVQUE0SUJEd0F3Z2dFS0FvSUJBUUNXRXJCUVRHWkdOMWlaYk45ZWhSZ2lmV0J4cWkyUGRneHcwM1A3VHlKWmZNeGpwNUw3ajFHTmVQSzVIemRyVW9JZDF5Q0l5Qk15eHFnYXpxZ3RwWDVXcHNYVzRWZk1oSmJOMVkwOXF6cXA2SkQrMlBaZG9UVTFrRlJBTVdmTC9VdVp0azdwbVJYZ0dtNWpLRHJaOU54ZTA0dk1ZUXI4OE5xd1cva2ZaMWdUT05JVVQwV3NMVC80NTIyQlJXeGZ3eGMzUUUxK1RLV2tMQ3J2ZWs2V2xJcXlhQzUyVzdNRFI4TXBGZWJ5bVNLVHZ3Zk1Sd3lLUUxUMDNVTDR2dDQ4eUVjOHNwN3dUQUhNL1dEZzhRb3RhcmY4T0JIa25vWjkyWGl2aWFWNnRRcWhST0hDZmdtbkNYaXhmVzB3RVhDdnFpTFRiUXRVYkxzUy84SVJ0ZFhrcFFCOUFnTUJBQUdqZ2dKWU1JSUNWREFPQmdOVkhROEJBZjhFQkFNQ0JhQXdFd1lEVlIwbEJBd3dDZ1lJS3dZQkJRVUhBd0V3REFZRFZSMFRBUUgvQkFJd0FEQWRCZ05WSFE0RUZnUVU2REhCd3NBdmI1M2cvQzA3cHJUdnZ3TlFRTFl3SHdZRFZSMGpCQmd3Rm9BVW1OSDRiaERyejV2c1lKOFlrQnVnNjMwSi9Tc3daQVlJS3dZQkJRVUhBUUVFV0RCV01DY0dDQ3NHQVFVRkJ6QUJoaHRvZEhSd09pOHZiMk56Y0M1d2Eya3VaMjl2Wnk5bmRITXhiekV3S3dZSUt3WUJCUVVITUFLR0gyaDBkSEE2THk5d2Eya3VaMjl2Wnk5bmMzSXlMMGRVVXpGUE1TNWpjblF3SFFZRFZSMFJCQll3RklJU1lYUjBaWE4wTG1GdVpISnZhV1F1WTI5dE1DRUdBMVVkSUFRYU1CZ3dDQVlHWjRFTUFRSUNNQXdHQ2lzR0FRUUIxbmtDQlFNd0x3WURWUjBmQkNnd0pqQWtvQ0tnSUlZZWFIUjBjRG92TDJOeWJDNXdhMmt1WjI5dlp5OUhWRk14VHpFdVkzSnNNSUlCQkFZS0t3WUJCQUhXZVFJRUFnU0I5UVNCOGdEd0FIY0E5bHlVTDlGM01DSVVWQmdJTUpSV2p1Tk5FeGt6djk4TUx5QUx6RTd4Wk9NQUFBRnZudXkwWndBQUJBTUFTREJHQWlFQTdlLzBZUnUzd0FGbVdIMjdNMnZiVmNaL21ycCs0cmZZYy81SVBKMjlGNmdDSVFDbktDQ0FhY1ZOZVlaOENDZllkR3BCMkdzSHh1TU9Ia2EvTzQxaldlRit6Z0IxQUVTVVpTNnc3czZ2eEVBSDJLaitLTURhNW9LKzJNc3h0VC9UTTVhMXRvR29BQUFCYjU3c3RKTUFBQVFEQUVZd1JBSWdFWGJpb1BiSnA5cUMwRGoyNThERkdTUk1BVStaQjFFaVZFYmJiLzRVdk5FQ0lCaEhrQnQxOHZSbjl6RHZ5cmZ4eXVkY0hUT1NsM2dUYVlBLzd5VC9CaUg0TUEwR0NTcUdTSWIzRFFFQkN3VUFBNElCQVFESUFjUUJsbWQ4TUVnTGRycnJNYkJUQ3ZwTVhzdDUrd3gyRGxmYWpKTkpVUDRqWUZqWVVROUIzWDRFMnpmNDluWDNBeXVaRnhBcU9SbmJqLzVqa1k3YThxTUowajE5ekZPQitxZXJ4ZWMwbmhtOGdZbExiUW02c0tZN1AwZXhmcjdIdUszTWtQMXBlYzE0d0ZFVWFHcUR3VWJHZ2wvb2l6MzhGWENFK0NXOEUxUUFFVWZ2YlFQVFliS3hZait0Q05sc3MwYlRTb0wyWjJkL2ozQnBMM01GdzB5eFNLL1VUcXlrTHIyQS9NZGhKUW14aStHK01LUlNzUXI2MkFuWmF1OXE2WUZvaSs5QUVIK0E0OFh0SXlzaEx5Q1RVM0h0K2FLb2hHbnhBNXVsMVhSbXFwOEh2Y0F0MzlQOTVGWkdGSmUwdXZseWpPd0F6WHVNdTdNK1BXUmMiLCJNSUlFU2pDQ0F6S2dBd0lCQWdJTkFlTzBtcUdOaXFtQkpXbFF1REFOQmdrcWhraUc5dzBCQVFzRkFEQk1NU0F3SGdZRFZRUUxFeGRIYkc5aVlXeFRhV2R1SUZKdmIzUWdRMEVnTFNCU01qRVRNQkVHQTFVRUNoTUtSMnh2WW1Gc1UybG5iakVUTUJFR0ExVUVBeE1LUjJ4dlltRnNVMmxuYmpBZUZ3MHhOekEyTVRVd01EQXdOREphRncweU1URXlNVFV3TURBd05ESmFNRUl4Q3pBSkJnTlZCQVlUQWxWVE1SNHdIQVlEVlFRS0V4VkhiMjluYkdVZ1ZISjFjM1FnVTJWeWRtbGpaWE14RXpBUkJnTlZCQU1UQ2tkVVV5QkRRU0F4VHpFd2dnRWlNQTBHQ1NxR1NJYjNEUUVCQVFVQUE0SUJEd0F3Z2dFS0FvSUJBUURRR005RjFJdk4wNXprUU85K3ROMXBJUnZKenp5T1RIVzVEekVaaEQyZVBDbnZVQTBRazI4RmdJQ2ZLcUM5RWtzQzRUMmZXQllrL2pDZkMzUjNWWk1kUy9kTjRaS0NFUFpSckF6RHNpS1VEelJybUJCSjV3dWRnem5kSU1ZY0xlL1JHR0ZsNXlPRElLZ2pFdi9TSkgvVUwrZEVhbHROMTFCbXNLK2VRbU1GKytBY3hHTmhyNTlxTS85aWw3MUkyZE44RkdmY2Rkd3VhZWo0YlhocDBMY1FCYmp4TWNJN0pQMGFNM1Q0SStEc2F4bUtGc2JqemFUTkM5dXpwRmxnT0lnN3JSMjV4b3luVXh2OHZObWtxN3pkUEdIWGt4V1k3b0c5aitKa1J5QkFCazdYckpmb3VjQlpFcUZKSlNQazdYQTBMS1cwWTN6NW96MkQwYzF0Skt3SEFnTUJBQUdqZ2dFek1JSUJMekFPQmdOVkhROEJBZjhFQkFNQ0FZWXdIUVlEVlIwbEJCWXdGQVlJS3dZQkJRVUhBd0VHQ0NzR0FRVUZCd01DTUJJR0ExVWRFd0VCL3dRSU1BWUJBZjhDQVFBd0hRWURWUjBPQkJZRUZKalIrRzRRNjgrYjdHQ2ZHSkFib090OUNmMHJNQjhHQTFVZEl3UVlNQmFBRkp2aUIxZG5IQjdBYWdiZVdiU2FMZC9jR1lZdU1EVUdDQ3NHQVFVRkJ3RUJCQ2t3SnpBbEJnZ3JCZ0VGQlFjd0FZWVphSFIwY0RvdkwyOWpjM0F1Y0d0cExtZHZiMmN2WjNOeU1qQXlCZ05WSFI4RUt6QXBNQ2VnSmFBamhpRm9kSFJ3T2k4dlkzSnNMbkJyYVM1bmIyOW5MMmR6Y2pJdlozTnlNaTVqY213d1B3WURWUjBnQkRnd05qQTBCZ1puZ1F3QkFnSXdLakFvQmdnckJnRUZCUWNDQVJZY2FIUjBjSE02THk5d2Eya3VaMjl2Wnk5eVpYQnZjMmwwYjNKNUx6QU5CZ2txaGtpRzl3MEJBUXNGQUFPQ0FRRUFHb0ErTm5uNzh5NnBSamQ5WGxRV05hN0hUZ2laL3IzUk5Ha21VbVlIUFFxNlNjdGk5UEVhanZ3UlQyaVdUSFFyMDJmZXNxT3FCWTJFVFV3Z1pRK2xsdG9ORnZoc085dHZCQ09JYXpwc3dXQzlhSjl4anU0dFdEUUg4TlZVNllaWi9YdGVEU0dVOVl6SnFQalk4cTNNRHhyem1xZXBCQ2Y1bzhtdy93SjRhMkc2eHpVcjZGYjZUOE1jRE8yMlBMUkw2dTNNNFR6czNBMk0xajZieWtKWWk4d1dJUmRBdktMV1p1L2F4QlZielltcW13a201ekxTRFc1bklBSmJFTENRQ1p3TUg1NnQyRHZxb2Z4czZCQmNDRklaVVNweHU2eDZ0ZDBWN1N2SkNDb3NpclNtSWF0ai85ZFNTVkRRaWJldDhxLzdVSzR2NFpVTjgwYXRuWnoxeWc9PSJdfcOiYcKyFcKYGEp X8O/w45oIsKHPUTCkwTCvT3Cpm9Ec2tmw4vDjnIBKRsnwrsWK8O3w5BvWx56wrTCuMOmwr/DrMOtw7NvwpbDmsKEZi1XMMKKAzbDvBXCrMK ClJjf8OSw5dSw6AAwq/ChDJmT8KlJyE3VMOCQVoHw5zDlMObwqZqw7HDoMK8wq/CjBcrwoXCicOocApuOsK2BCLDqcKwwrQXw5ZLwoLCisKtw6EUwrDCq8OFw6DCg8KtT8KPJcK9ZMO5EMOPwrHCqcKmCF8hECzDuMKwwpvCmMKWGsOpT8O8woDDncOMw7zCvsOjw5jDn8OfwoPCscOAJBnCjcOab2TCncO0cyVDw6pcwoM="},
                        {"phone_id",                  Guid.NewGuid().ToString()},
                        {"_csrftoken",                csrfGlobal},
                        {"username",                  Login},
                        {"first_name",                Name + " " + Surname},
                        {"adid",                      Guid.NewGuid().ToString()},
                        {"guid",                      Guid.NewGuid().ToString()},
                        {"device_id",                 GenerateRandomDeviceId()},
                        {"email",                     Mail},
                        //{"sn_nonce",                  ""},
                        {"sn_nonce",                  "bjEudHNocWpmemNAZ21haWwuY29tfDE1OTM4NTUxODV8acO5w4BNGcOtS8ObQcOIWcK2"},
                        {"force_sign_up_code",        ""},
                        {"waterfall_id",              Guid.NewGuid().ToString()},
                        {"qs_stamp",                  ""},
                        {"password",                  Password},
                        {"one_tap_opt_in",            "true"},
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
                        var response = request.Post("https://i.instagram.com/api/v1/accounts/create/", reqParams);
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

                    if (html.Contains("challenge_required"))
                    {
                        DoCheckpoint(Login + ":" + Password);
                        Log(Translate.Tr("Чекпойнт - ") + Login + ":" + Password + ":" + Proxy[index % Proxy.Count] + ":" + UserAgent + ":" + Mail + ":" + IMAPPassword + ":" + accept_lang + ":" + CurrentMobileAppVersion);
                    }
                    else
                    if (!html.Contains("\"account_created\": true"))
                    {
                        DoBad();
                        Log(Translate.Tr("Ошибка при регистрации - ") + Login + ":" + Password + ":" + Proxy[index % Proxy.Count] + ":" + UserAgent + ":" + Mail + ":" + IMAPPassword + ":" + accept_lang + ":" + CurrentMobileAppVersion);
                    }
                    else
                    {
                        DoResult(Login + ":" + Password/* + "||" + outStr["device_id"] + ":" + outStr["guid"] + ":" + outStr["phone_id"] +"|" + cookies + "||"*/);
                        Log(Translate.Tr("Регистрация успешна! - ") + Login + ":" + Password + ":" + Proxy[index % Proxy.Count] + ":" + UserAgent + ":" + Mail + ":" + IMAPPassword + ":" + accept_lang + ":" + CurrentMobileAppVersion);
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
