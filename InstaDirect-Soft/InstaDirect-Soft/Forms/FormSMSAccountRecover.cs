﻿using InstaDirectMessage_ButDev.Tools;
using Newtonsoft.Json;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace InstaDirectMessage_ButDev
{
    public partial class FormSMSAccountRecover : Form
    {
        public static string ProxyPath = "", AccountsPath = "", UAsPath = "", ResultFilePath = "", PROXYTYPE = "", time = "", ApiProxyUrl = "";
        public static int[] tries = new int[10000];
        public static int ThreadsCount = 0, Final = 0, finalresult = 0, ApiProxyTimeout = 0, Delay = 0;
        public static List<string> Proxy = new List<string>();
        public static List<string> Accounts = new List<string>();
        public static List<string> Locales = new List<string>();
        public static bool stop = true, isApiProxy = false;

        public FormSMSAccountRecover()
        {
            InitializeComponent();

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            richTextBoxLog.AutoScrollOffset = new Point();
            this.FormClosing += FormWebRegistrator_FormClosing;

            List<string> key = new List<string>();
            List<string> value = new List<string>();
            var data = Properties.Settings.Default.FormSMSAccountRecover;
            foreach (string k in data)
            {
                if (k == "") continue;
                var dict = k.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                key.Add(dict[0]);
                value.Add(Encoding.UTF8.GetString(Convert.FromBase64String(dict[1])));
            }
            foreach (Control a in this.Controls)
            {
                if (key.Contains(a.Name)) a.Text = value[key.FindIndex(x => x == a.Name)];
            }

            ProxyPath = textBoxProxy.Text;
            AccountsPath = textBoxAccounts.Text;
            UAsPath = textBoxUserAgents.Text;
            ResultFilePath = textBoxResultFile.Text;
        }

        private bool isFileNameValid(string fileName)
        {
            if ((fileName == null) || (fileName.IndexOfAny(Path.GetInvalidPathChars()) != -1)) return false;
            /*try
            {
                var tempFileInfo = new FileInfo(fileName);
                return true;
            }
            catch (NotSupportedException)
            {
                return false;
            }*/
            return true;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Final = 0; Delay = 5; InstagramAccountRecover.stop = false;
            for (int i = 0; i < 10000; i++) tries[i] = 0;
            Accounts.Clear(); Proxy.Clear(); 
            if (!int.TryParse(textBoxThreadsCount.Text, out ThreadsCount) || ThreadsCount <= 0) { MessageBox.Show(Translate.Tr("Количество потоков указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (!int.TryParse(textBoxDelay.Text, out Delay) || Delay <= 0) { MessageBox.Show(Translate.Tr("Число секунд в задержке указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (isApiProxy)
            {
                if (!(textBoxProxy.Text.Contains("http://") || textBoxProxy.Text.Contains("https://"))) { MessageBox.Show(Translate.Tr("Ссылка для API Proxy указана некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!int.TryParse(textBoxApiProxyUpdateInterval.Text, out ApiProxyTimeout) || ApiProxyTimeout <= 0) { MessageBox.Show(Translate.Tr("Число секунд в интервале обновления указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                ApiProxyUrl = textBoxProxy.Text;
                if (!GetApiProxy()) { MessageBox.Show(Translate.Tr("Не удалось получить прокси по ссылке!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }
            else
                if (ProxyPath == "") { MessageBox.Show(Translate.Tr("Файл с прокси не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (AccountsPath == "") { MessageBox.Show(Translate.Tr("Файл с аккаунтами не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (UAsPath == "") { MessageBox.Show(Translate.Tr("Файл с Юзер-Агентами не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (ResultFilePath == "" && isFileNameValid(ResultFilePath)) { MessageBox.Show(Translate.Tr("Файл, куда нужно сохранять результат, не указан или указан некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (textBoxApiKeyRuCaptcha.Text == "" || textBoxAPIKey.Text == "") { MessageBox.Show(Translate.Tr("Не указан API ключ!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            var data = Properties.Settings.Default.FormSMSAccountRecover;
            data.Clear();
            foreach (Control a in this.Controls)
            {
                if (a.GetType().ToString() == "System.Windows.Forms.TextBox") data.Add(a.Name + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(a.Text)));
            }
            Properties.Settings.Default.FormSMSAccountRecover = data;
            Properties.Settings.Default.Save();

            InstagramAccountRecover.ResultPath = ResultFilePath;
            InstagramAccountRecover.SmsApiKey = textBoxAPIKey.Text;
            InstagramAccountRecover.CountryCode = comboBoxPhoneRegion.Text.Split(new char[1] {' '})[0];
            InstagramAccountRecover.ApiKeyRuCaptcha = textBoxApiKeyRuCaptcha.Text;

            if (comboBoxService.Text == "https://sms-activate.ru/") InstagramAccountRecover.Host = "https://sms-activate.ru/";
            else
                if (comboBoxService.Text == "sms.kopeechka.store") InstagramAccountRecover.Host = "http://213.166.69.162/";
            else
                if (comboBoxService.Text == "http://smsvk.net/") InstagramAccountRecover.Host = "http://smsvk.net/";
            else
                if (comboBoxService.Text == "sms-online.pro") InstagramAccountRecover.Host = "https://sms-online.pro/"; else InstagramAccountRecover.Host = "http://194.87.238.180/";

            buttonStart.Visible = false;
            buttonStop.Visible = true;
            stop = false;

            time = DateTime.Now.Date.ToString("dd.MM.yyyy");
            //time = time.Remove(10);
            string path = Path.Combine(Environment.CurrentDirectory, time);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (!Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"))) Directory.CreateDirectory(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"));
            path = Path.Combine(Environment.CurrentDirectory, time);
            InstagramAccountRecover.path = path;

            InstagramAccountRecover.proxytype = comboBoxTypeProxy.Text;

            if (!isApiProxy)
            {
                //Load Proxies
                using (StreamReader sr = new StreamReader(ProxyPath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Proxy.Add(line);
                    }
                }
            }
            InstagramAccountRecover.Proxy = Proxy;

            //Load Accounts
            using (StreamReader sr = new StreamReader(AccountsPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Accounts.Add(line);
                }
            }

            //Load User Agents
            //var UserAgents = Properties.Resources.UserAgents.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var UserAgents = File.ReadAllLines(UAsPath);
            InstagramAccountRecover.UserAgents = UserAgents;

            Progress<int> progress = new Progress<int>(p =>
            {
                if (p == 1)
                {
                    labelGood.Text = (int.Parse(labelGood.Text) + 1).ToString();
                } else
                if (p == 0)
                {
                    labelBad.Text = (int.Parse(labelBad.Text) + 1).ToString();
                }
            });

            Progress<string> log = new Progress<string>(p =>
            {
                if (richTextBoxLog.TextLength > 10000) richTextBoxLog.Clear();
                richTextBoxLog.AppendText(p);
            });

            Progress<int> btnVisibility = new Progress<int>(p =>
            {
                buttonStart.Visible = true;
                buttonStop.Visible = false;
            });

            if (ThreadsCount > Accounts.Count) ThreadsCount = Accounts.Count;
            int L = 0; int R = Accounts.Count / ThreadsCount;
            for (int potok = 0; potok < ThreadsCount; potok++)
            {
                new Thread(() => Work(L, R, potok, progress, log)).Start();
                Thread.Sleep(300);
                L = R;
                R += Accounts.Count / ThreadsCount;
            }
            if (Accounts.Count % ThreadsCount != 0) { finalresult = ThreadsCount + 1; new Thread(() => Work(R, Accounts.Count, ThreadsCount, progress, log)).Start(); } else finalresult = ThreadsCount;

            //MessageBox.Show(ThreadsCount.ToString() + " " + finalresult.ToString());

            new Thread(() => WaitForEnd(btnVisibility)).Start();
        }

        private void checkBoxIsApiProxy_CheckedChanged(object sender, EventArgs e)
        {
            isApiProxy = checkBoxIsApiProxy.Checked;
            if (isApiProxy)
            {
                labelApiProxyLink.Visible = true;
                textBoxProxy.ReadOnly = false;
                buttonOpenProxy.Visible = false;
                textBoxProxy.Text = "https://example.com/proxies.txt";

                textBoxApiProxyUpdateInterval.Visible = true;
                labelApiProxyUpdateInterval.Visible = true;
            }
            else
            {
                labelApiProxyLink.Visible = false;
                textBoxProxy.ReadOnly = true;
                buttonOpenProxy.Visible = true;
                textBoxProxy.Clear();

                textBoxApiProxyUpdateInterval.Visible = false;
                labelApiProxyUpdateInterval.Visible = false;
            }
        }

        private bool GetApiProxy()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiProxyUrl);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string html = new StreamReader(response.GetResponseStream()).ReadToEnd();

                Proxy = html.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void linkLabelSmsService_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://sms-activate.ru/");
        }

        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (certificate.GetRawCertDataString() == "3082064B30820533A003020102021203F4AA827856B1930459D17AC57FF3A4F273300D06092A864886F70D01010B05003032310B300906035504061302555331163014060355040A130D4C6574277320456E6372797074310B3009060355040313025233301E170D3231303133313131303933375A170D3231303530313131303933375A3020311E301C06035504031315696E7374616D61696C636865636B65722E7369746530820222300D06092A864886F70D01010105000382020F003082020A0282020100DE1CBB12AE9A36C965EF5A3F0ED9964878D849A4C9570E7DEBA052C3F0023667A0E80EC9EC470B5F19E074840956A083A719B370B02D132030B10C95E0AC036C8C69BE4841DFE749DBF5CFC36EE689FF4C267A7900B72703742A6C2539AE04DE3E08E368DF0804FF8CB32916641CBDBE54A14AA4EFC668E323C29637112D4863B4C41F36AA04A99A422D704FE5393DD0D7C17338B84857484E65A67D812AB5C9D6EEC5E3100D9A3E3203C57B67C8539D333C67F492DE45BCA6253785E9501D23B7D31374CA68D61EB5CED76DAC72BACBA00143A7A38C0F39C818AD145913A3C1D67799D1FDF26FE4FC629F03650FC9BF3C8EE567CD7C9134C5B7108A9FBB189F1D472EC4DC79D6BC9A6C861853978EAF1978AB452D4D980379514C9A2DDC2482D5D9A29BAFED56E19CBD84F19C791B402783D3A2290E9B446A770A16AB4C43BAD28CEC9E64BEF7C184C3CCEEA62DAA2953B3A13FF9DCA912115974BD182B7E5C6035099AC1021BA2730B5058D893A4E0126FE804C75858F6041BCED58F542473F2D748EFF2369DBC41D52C70D462D83E8CC503DED8739EE7132DEE5B2476B50C8C7369E25E5FD09095D3D303CEDD01A6550109C7CE3366C0A988A59789A0B919B847DB7835FD9BBE98D36DF9B6F20CD4E60C47A821B9DAB141680D687F5EC278A847118FAAA56DEB09807309E698D9646E43D45066CDCC35B456476F41AB6CEB0203010001A382026B30820267300E0603551D0F0101FF0404030205A0301D0603551D250416301406082B0601050507030106082B06010505070302300C0603551D130101FF04023000301D0603551D0E04160414297CCDE45513CFB3C2CE2F5619FB084D1C66E5A4301F0603551D23041830168014142EB317B75856CBAE500940E61FAF9D8B14C2C6305506082B0601050507010104493047302106082B060105050730018615687474703A2F2F72332E6F2E6C656E63722E6F7267302206082B060105050730028616687474703A2F2F72332E692E6C656E63722E6F72672F303B0603551D11043430328215696E7374616D61696C636865636B65722E7369746582197777772E696E7374616D61696C636865636B65722E73697465304C0603551D20044530433008060667810C0102013037060B2B0601040182DF130101013028302606082B06010505070201161A687474703A2F2F6370732E6C657473656E63727970742E6F726730820104060A2B06010401D6790204020481F50481F200F00076009420BC1E8ED58D6C88731F828B222C0DD1DA4D5E6C4F943D61DB4E2F584DA2C200000177585934490000040300473045022075573493F73DAF3BAA11FD70564A11EF2FD0D7A61A4B828D642E4C0FFF8A9A91022100B0D5ADC09FFAD9FFF457816D00013C06081AB6DDB10ABEA801B4E2EDF52E42010076007D3EF2F88FFF88556824C2C0CA9E5289792BC50E78097F2E6A9768997E22F0D7000001775859348F00000403004730450220617D007BE1F9A8CA43D99229377B201D6E2E96D874312C4F6805ABF892578C80022100DC945A8D40314EFCF10B070EFD159B8B10AA7B3345D91D572B7B18605E236411300D06092A864886F70D01010B050003820101004687A95CC57BDACCFF56CB13A2F5D7B6B0D79A13A6414B9C8B3E66639F85D0C2EBE4E533147CE14E8B8DCFF585CFD1AEB2E1C15911DBED06B07F48F93FBC3242034932ED48C2EE29A4EA6CCF15834B0EE2CCF417A044D202AE4BCB11342D2B2027E51A9C856E9B9EDA51557F64237E05C6678D3A2770361FC698A8555AD6B2C6DA0B651F6B5C639D6759BBD3179359F46649C536A79BC9EE72D463E0B3F888DBE83A76F726C5A46615F9712E7172B6B179B83D211D248BA7F35828A58FA5C4013149A3C1B883972D5CA1E11056AB42D5C951E28B583A1A2F1D80FF5037267999EBB997913C74F46ACAF5EC73B6D89B78223B25FBA9D2E8D512B1AC6A0DC548D7") return true; else return false;
        }
        public static BigInteger fun(BigInteger p, BigInteger a, BigInteger b)
        {
            BigInteger s = BigInteger.One;
            for (int i = 1; i <= b.IntValue; i++)
            {
                s = s.Multiply(a).Mod(p);
            }
            return s;
        }
        private void Stats()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                Random myrandom = new Random(DateTime.Now.Millisecond * DateTime.Now.Second * DateTime.Now.Year);
                BigInteger p = BigInteger.ValueOf(myrandom.Next(1000, 10000)), g, A, a, B, k;
                p = p.NextProbablePrime();
                g = BigInteger.ValueOf(FirstLevelRoot.generator(p.IntValue));
                a = BigInteger.ValueOf(myrandom.Next(p.IntValue));//A
                A = fun(p, g, a);

                string url = B64X.Encrypt("http://a116901.hostde27.fornex.host/insta/license/InstaDirectMessageTime.php");
                HttpWebRequest requesttime = (HttpWebRequest)WebRequest.Create(B64X.Decrypt(url));
                requesttime.Method = "GET";
                requesttime.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                HttpWebResponse responz = (HttpWebResponse)requesttime.GetResponse();
                string htmltime = B64X.Encrypt(new StreamReader(responz.GetResponseStream()).ReadToEnd());
                string hash = B64X.Encrypt(MCrypt.Decrypt(new string(B64X.Decrypt(htmltime).Reverse().ToArray())));
                hash = B64X.Encrypt(new string(B64X.Decrypt(hash).Reverse().ToArray()));

                url = B64X.Encrypt("http://a116901.hostde27.fornex.host/insta/license/InstaDirectMessageTime.php?q=" + MCrypt.Encrypt(B64X.Decrypt(hash)));
                HttpWebRequest requesttimereal = (HttpWebRequest)WebRequest.Create(B64X.Decrypt(url));
                requesttimereal.Method = "GET";
                requesttimereal.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                requesttimereal.Headers.Add("Cookie", responz.Headers["Set-Cookie"].Remove(responz.Headers["Set-Cookie"].IndexOf(';')));
                HttpWebResponse responze = (HttpWebResponse)requesttimereal.GetResponse();
                string TIME = B64X.Encrypt(MCrypt.Decrypt(new StreamReader(responze.GetResponseStream()).ReadToEnd()));


                HttpWebRequest reqt = (HttpWebRequest)WebRequest.Create("http://a116901.hostde27.fornex.host/insta/license/InstaDirectMessageDH.php?q=" + MCrypt.Encrypt("p=" + p.ToString() + "&g=" + g.ToString() + "&A=" + A.ToString() + "&time=" + System.Web.HttpUtility.UrlEncode(B64X.Decrypt(TIME))));
                reqt.Method = "GET";
                reqt.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                HttpWebResponse resp = (HttpWebResponse)reqt.GetResponse();
                string htmlB = B64X.Encrypt(new StreamReader(resp.GetResponseStream()).ReadToEnd());
                B = new BigInteger(MCrypt.Decrypt(B64X.Decrypt(htmlB)));

                string ConstString = B64X.Encrypt("abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_");
                Random random = new Random(DateTime.Now.Second);
                string salt = B64X.Encrypt(new string(Enumerable.Repeat(B64X.Decrypt(ConstString), 10).Select(s => s[random.Next(s.Length)]).ToArray()));
                var outStr = new Dictionary<string, string>
                {
                    {"time",       B64X.Decrypt(TIME)},
                    {"id",         ID.NewIDNumber},
                    {"salt", B64X.Decrypt(salt)},
                    {"good", labelGood.Text}
                };

                string req = B64X.Encrypt(JsonConvert.SerializeObject(outStr));

                string res = "";
                for (int i = 0; i < B64X.Decrypt(req).Length; i++)
                {
                    k = BigInteger.ValueOf(B64X.Decrypt(req)[i]);

                    k = k.Multiply(fun(p, B, a)).Mod(p);
                    res = B64X.Encrypt(B64X.Decrypt(res) + k.ToString() + ".");
                }

                url = B64X.Encrypt("http://a116901.hostde27.fornex.host/insta/license/RegistrarStats.php?q=" + MCrypt.Encrypt(B64X.Decrypt(res)));
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(B64X.Decrypt(url));
                request.Method = "GET";
                request.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                request.Headers.Add("Cookie", resp.Headers["Set-Cookie"].Remove(resp.Headers["Set-Cookie"].IndexOf(';')));
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string html = B64X.Encrypt(new StreamReader(response.GetResponseStream()).ReadToEnd());
                //MessageBox.Show(B64X.Decrypt(html));

                string result = "";
                foreach (string value in MCrypt.Decrypt(B64X.Decrypt(html)).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    k = BigInteger.ValueOf(long.Parse(value));

                    k = k.Multiply(fun(p, B, p.Subtract(BigInteger.One.Add(a)))).Mod(p);
                    result = B64X.Encrypt(B64X.Decrypt(result) + (char)k.IntValue);
                }
                CheckLicense.jsonDes account = JsonConvert.DeserializeObject<CheckLicense.jsonDes>(B64X.Decrypt(result));
                if (MCrypt.Decrypt(new string(MCrypt.Decrypt(B64X.Decrypt(account.salt)).Reverse().ToArray())) != B64X.Decrypt(salt) || B64X.Decrypt(account.result) != "OK") throw new Exception();
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ }
        }

        private void WaitForEnd(IProgress<int> vis)
        {
            try
            {
                int i = 0;
                while (true)
                {
                    if (Final == finalresult)
                    {
                        Thread.Sleep(ThreadsCount * 800);
                        foreach (var process in Process.GetProcessesByName("chromedriver"))
                        {
                            process.Kill();
                        }
                        break;
                    }
                    i++;
                    if (stop)
                    {
                        foreach (var process in Process.GetProcessesByName("chromedriver"))
                        {
                            process.Kill();
                        }
                        return;
                    }
                    Thread.Sleep(3000);
                    //if (i == 60) { Stats(); i = 0; }

                    if (isApiProxy)
                        if (i * 3 >= ApiProxyTimeout)
                        {
                            i = 0;
                            int attempts = 0;
                            while (!GetApiProxy() && attempts < 3) { Thread.Sleep(3000); attempts++; }
                            InstagramChecker.Proxy = Proxy;
                        }
                }
            }
            catch (Exception e) { /*MessageBox.Show("WaitForEnd: " + e.ToString());*/ }

            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("СМС восстановитель аккаунтов"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            vis.Report(999);
            stop = true;
            InstagramAccountRecover.stop = true;
        }

        private void FormWebRegistrator_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                /*if (MessageBox.Show("Вы уверены что хотите закрыть программу?", "Выйти?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Exit();
                }*/
                /*if (stop) this.Hide(); else MessageBox.Show("Нельзя закрыть окно, пока оно работает!", "Веб-регистратор: Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
            e.Cancel = true;
            this.Hide();
        }

        private void buttonSeeLog_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstaSMSAccountRecover.log"))) MessageBox.Show(Translate.Tr("Файла с логом пока нет!"), Translate.Tr("СМС восстановитель аккаунтов: Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            else Process.Start(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstaSMSAccountRecover.log"));
        }

        private async void Work(int L, int R, int tt, IProgress<int> progress, IProgress<string> log)
        {
            await Task.Run(() => MainThread(L, R, tt, (Proxy.Count / (ThreadsCount + 1)) * tt, progress, log));
        }
        private void MainThread(int L, int R, int tt, int indx, IProgress<int> progress, IProgress<string> log)
        {
            //MessageBox.Show("tt: " + tt.ToString());
            string phone = "", id = "";
            DateTime time = DateTime.Now.AddMinutes(1);
            for (int i = L; i < R; i++)
            {
                try
                {
                    while (!stop)
                    {
                        Random rnd = new Random(Guid.NewGuid().GetHashCode());
                        InstagramAccountRecover reg = new InstagramAccountRecover(phone, id, Accounts[i], indx + tries[tt], tt);
                        log.Report(reg.log);
                        if (InstagramAccountRecover.Host != "https://sms-activate.ru/") { reg.PhoneNumber = ""; phone = ""; id = ""; }
                        if (DateTime.Now > time.AddMinutes(21)) { tries[tt]++; reg.PhoneNumber = ""; phone = ""; id = ""; time = DateTime.Now.AddMinutes(1); }

                        if (reg.good == 1)
                        {
                            progress.Report(1);
                            phone = reg.PhoneNumber;
                            id = reg.id;
                            tries[tt] = 0;
                            break;
                        }
                        else
                        if (reg.good == 3 || reg.good == 4) { progress.Report(0); tries[tt]++; reg.PhoneNumber = ""; phone = ""; id = ""; }
                        else { progress.Report(reg.good); if (reg.good != 0) tries[tt]++; }

                        Thread.Sleep(Delay * 1000);

                        if (reg.PhoneNumber != "")
                        {
                            phone = reg.PhoneNumber;
                            id = reg.id;
                        }

                        /*if (reg.good != 0)
                        {
                            steps++;
                            if (steps == 15) { steps = 0; phone = ""; id = ""; }
                        } else 
                        {
                            steps--;
                        }*/
                        Console.WriteLine(phone + " " + id);
                    }
                }
                catch (Exception ex) { /*MessageBox.Show(ex.ToString());*/ progress.Report(0); }
            }
            Final++;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= ThreadsCount; i++) tries[i] = 0;
            stop = true;
            buttonStop.Visible = false;
            Thread.Sleep(ThreadsCount * 500);
            buttonStart.Visible = true;
            MessageBox.Show(Translate.Tr("Вы нажали кнопку \"Стоп\"."), Translate.Tr("СМС восстановитель аккаунтов"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void buttonOpenResultFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxResultFile.Text = openFileDialog.FileName;
            ResultFilePath = openFileDialog.FileName;
        }

        private void buttonOpenUserAgents_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxUserAgents.Text = openFileDialog.FileName;
            UAsPath = openFileDialog.FileName;
        }
        private void buttonOpenProxy_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxProxy.Text = openFileDialog.FileName;
            ProxyPath = openFileDialog.FileName;
        }

        private void buttonOpenAccounts_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxAccounts.Text = openFileDialog.FileName;
            AccountsPath = openFileDialog.FileName;
        }
    }
}
