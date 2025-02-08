using InstaDirectMessage_ButDev.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstaDirectMessage_ButDev
{
    public partial class MainForm : Form
    {
        public static bool sendLink = false, sendPost = false, stop = true, isApiProxy = false;
        public static int Final = 0, ThreadsCount = 50, Laps = 1, Delay = 5, finalresult = 0, ApiProxyTimeout = 0;
        public static string ProxyPath = "", OldEmailsPath = "", IDPath = "", time = "", TYPE = "", TEXT = "", link = "", postLink = "", ApiProxyUrl = "";
        public static List<string> OldMail = new List<string>();
        public static List<string> Proxy = new List<string>();
        public static List<string> Blacklist = new List<string>();
        public static List<string> ID = new List<string>();
        DataGridViewCellEventArgs mouseLocation;
        FormWebRegistrator formWebRegistrator = new FormWebRegistrator();
        FormInstagramChecker formInstagramChecker = new FormInstagramChecker();
        FormHashtagsParser formInstagramHashtagsParser = new FormHashtagsParser();
        FormFollowersParser formInstagramFollowersParser = new FormFollowersParser();
        FormGeoParser formInstagramGeoParser = new FormGeoParser();
        FormApiRegistrator formApiRegistrator = new FormApiRegistrator();
        FormUserAgentActualizer formUserAgentActualizer = new FormUserAgentActualizer();
        //formStatsChart formStatsChart = new FormStatsChart();
        FormAvatarChanger formAvatarChanger = new FormAvatarChanger();
        FormMobUserAgentActualizer formMobUserAgentActualizer = new FormMobUserAgentActualizer();
        FormSMSWebRegistrator formSMSWebRegistrator = new FormSMSWebRegistrator();
        FormSMSApiRegistrator formSMSApiRegistrator = new FormSMSApiRegistrator();
        FormSMSAccountRecover formSMSAccountRecover = new FormSMSAccountRecover();
        FormDeleteDuplicate formDeleteDuplicate = new FormDeleteDuplicate();
        FormDeleteCookie formDeleteCookie = new FormDeleteCookie();
        FormBreakdownDB formBreakdownDB = new FormBreakdownDB();
        FormPhotoWebPosting formPhotoWebPosting = new FormPhotoWebPosting();
        FormMassFollowing formMassFollowing = new FormMassFollowing();
        FormFollowersGreeting formFollowersGreeting = new FormFollowersGreeting();
        public MainForm()
        {
            InitializeComponent();
            formWebRegistrator.Hide();
            formInstagramChecker.Hide();
            formInstagramHashtagsParser.Hide();
            formInstagramGeoParser.Hide();
            formApiRegistrator.Hide();
            //formStatsChart.Hide();
            formAvatarChanger.Hide();
            formMobUserAgentActualizer.Hide();
            formSMSWebRegistrator.Hide();
            formSMSApiRegistrator.Hide();
            formSMSAccountRecover.Hide();
            formDeleteDuplicate.Hide();
            formDeleteCookie.Hide();
            formBreakdownDB.Hide();
            formPhotoWebPosting.Hide();
            formMassFollowing.Hide();
            formFollowersGreeting.Hide();

            dataGridView.CellMouseEnter += dataGridView_CellMouseEnter;
            dataGridView.CellMouseMove += dataGridView_CellMouseMove;

            dataGridView.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView.CellMouseDown += dataGridView_CellMouseMove;
            dataGridView.RowHeadersVisible = false;

            buttonOpenAccounts.MouseEnter += buttonOpenAccounts_MouseEnter;
            buttonOpenAccounts.MouseLeave += buttonOpenAccounts_MouseLeave;
            buttonMailingSwitch.MouseEnter += buttonTasksSwitch_MouseEnter;
            buttonMailingSwitch.MouseLeave += buttonTasksSwitch_MouseLeave;
            buttonAccountRecover.MouseEnter += buttonSeeLog_MouseEnter;
            buttonAccountRecover.MouseLeave += buttonSeeLog_MouseLeave;
            buttonOpenSettings.MouseEnter += buttonOpenSettings_MouseEnter;
            buttonOpenSettings.MouseLeave += buttonOpenSettings_MouseLeave;
            buttonWebRegistrator.MouseEnter += buttonWebRegistrator_MouseEnter;
            buttonWebRegistrator.MouseLeave += buttonWebRegistrator_MouseLeave;
            buttonHashtagParser.MouseEnter += buttonHashtagParser_MouseEnter;
            buttonHashtagParser.MouseLeave += buttonHashtagParser_MouseLeave;
            buttonUtilities.MouseEnter += buttonUtilities_MouseEnter;
            buttonUtilities.MouseLeave += buttonUtilities_MouseLeave;
            buttonPosting.MouseEnter += buttonPosting_MouseEnter;
            buttonPosting.MouseLeave += buttonPosting_MouseLeave;

            panelDirectMailing.Location = new Point(204, 0);
            panelSettings.Location = new Point(204, 0);
            panelAccounts.Location = new Point(210, 0);
            panelTasks.Location = new Point(220, 12);
            panelUtilities.Location = new Point(204, 306);
            panelPosting.Location = new Point(204, 416);

            if (Properties.Settings.Default.StopIfCheckpoint) comboBoxCheckpointBehavior.Text = "Останавливаться"; else comboBoxCheckpointBehavior.Text = "Брать другой аккаунт (если имеется)";
            if (Properties.Settings.Default.StopIfTempBlocked) comboBoxTempBlock.SelectedIndex = 0; else comboBoxTempBlock.SelectedIndex = 1;
            textBoxTimeoutIfBlocked.Text = Properties.Settings.Default.TimeoutIfBlocked.ToString();
            textBoxProxyTimeout.Text = Properties.Settings.Default.ProxyTimeout.ToString();
            comboBoxLanguage.Text = Properties.Settings.Default.Language;

            textBoxTimeoutIfBlocked.KeyPress += Number_KeyPress;
            textBoxProxyTimeout.KeyPress += Number_KeyPress;

            comboBoxParser.SelectedIndex = 0;

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            labelRemaining.Text = CheckLicense.remaining.ToString();

            List<string> key = new List<string>();
            List<string> value = new List<string>();
            var data = Properties.Settings.Default.MainForm;

            //Check parameters from old version
            if (data.Count == 0)
            {
                // %userprofile%\appdata\local
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "appdata\\local\\ButterDevelop");
                if (Directory.Exists(path))
                {
                    foreach (var a in Directory.GetDirectories(path))
                    {
                        if (a.Contains("InstaDirect-Soft.exe"))
                        {
                            path = a;
                            break;
                        }
                    }
                    if (Directory.Exists(Path.Combine(path, Properties.Resources.PreviousVersion)))
                    {
                        Directory.Move(Path.Combine(path, Properties.Resources.PreviousVersion), Path.Combine(path, Properties.Resources.Version));
                        Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }

            foreach (string k in data)
            {
                if (k == "") continue;
                var dict = k.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                key.Add(dict[0]);
                value.Add(Encoding.UTF8.GetString(Convert.FromBase64String(dict[1])));
            }
            foreach (Control a in panelDirectMailing.Controls)
            {
                if (key.Contains(a.Name)) a.Text = value[key.FindIndex(x => x == a.Name)];
            }
            foreach (Control a in panelAccounts.Controls)
            {
                if (key.Contains(a.Name)) a.Text = value[key.FindIndex(x => x == a.Name)];
            }

            ProxyPath = textBoxProxy.Text;
            OldEmailsPath = textBoxOldEmails.Text;
            IDPath = textBoxID.Text;
            TEXT = textBoxMessage.Text;
            postLink = textBoxPost.Text;
            link = textBoxLink.Text;
        }

        private void toolStripMenuItemCopyLogin_Click(object sender, EventArgs e)
        {
            // dataGridView.SelectedCells[0].RowIndex

            if (mouseLocation == null || mouseLocation.RowIndex < 0 || mouseLocation.ColumnIndex < 0) return;
            Clipboard.SetText(dataGridView.Rows[mouseLocation.RowIndex].Cells[0].Value.ToString());
        }
        private void toolStripMenuItemCopyPass_Click(object sender, EventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex < 0 || mouseLocation.ColumnIndex < 0) return;
            Clipboard.SetText(dataGridView.Rows[mouseLocation.RowIndex].Cells[1].Value.ToString());
        }
        private void скопироватьЛогинпарольToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex < 0 || mouseLocation.ColumnIndex < 0) return;
            Clipboard.SetText(dataGridView.Rows[mouseLocation.RowIndex].Cells[0].Value.ToString() + ":" + dataGridView.Rows[mouseLocation.RowIndex].Cells[1].Value.ToString());
        }
        private void скопироватьКукиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex < 0 || mouseLocation.ColumnIndex < 0) return;
            Clipboard.SetText(dataGridView.Rows[mouseLocation.RowIndex].Cells[4].Value.ToString());
        }
        private void посмотретьЛогАккаунтаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex < 0 || mouseLocation.ColumnIndex < 0) return;

            var path = Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"), dataGridView.Rows[mouseLocation.RowIndex].Cells[0].Value.ToString() + ".log");
            File.AppendAllText(path, dataGridView.Rows[mouseLocation.RowIndex].Cells[5].Value.ToString());
            Process.Start("notepad.exe", path);
        }
        private void снятьВыделениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView.ClearSelection();
        }

        private void dataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
            //dataGridView.ClearSelection();
        }
        private void dataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs location)
        {
            //dataGridView.ClearSelection();
        }

        void AddElement(string x)
        {
            string Login = "", Password = "", device_id = "", phone_id = "", cookie = "", adid = "", guid = "";
            if (x.Contains("||"))
            {
                // Login:Password||DeviceId;PhoneId;ADID;GUID|Cookie||
                //Regex regex = new Regex("(.*):(.*)\\|\\|(android\\-.{16});(.{36});(.{36});(.{36})\\|(.*)\\|\\|");
                if (x.Substring(x.Length - 2) != "\\") x += "\\";
                if (new Regex("(.*):(.*)\\|\\|(.*)\\|(.*)\\|\\|").IsMatch(x))
                {
                    Regex regex = new Regex("(.*):(.*)\\|\\|(.*)\\|(.*)\\|\\|");
                    var mathes = regex.Match(x);

                    Login = mathes.Groups[1].Value;
                    Password = mathes.Groups[2].Value;
                    cookie = mathes.Groups[4].Value;
                    cookie = cookie.Replace(";", "; ");

                    Regex regex2 = new Regex("(.*);(.*);(.*);(.*)");
                    var mathes_2 = regex2.Match(mathes.Groups[3].Value);
                    device_id = mathes_2.Groups[1].Value;
                    phone_id = mathes_2.Groups[2].Value;
                    guid = mathes_2.Groups[3].Value;
                    adid = mathes_2.Groups[4].Value;
                } else
                {
                    Regex regex = new Regex("(.*):(.*)\\|(.*)\\|(.*)\\|(.*)\\|\\|");
                    var mathes = regex.Match(x);

                    Login = mathes.Groups[1].Value;
                    Password = mathes.Groups[2].Value;
                    cookie = mathes.Groups[5].Value;
                    cookie = cookie.Replace(";", "; ");

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
                var mathes = regex.Match(x);
                Login = mathes.Groups[1].Value;
                Password = mathes.Groups[2].Value;
            }

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView);

            string[] temp = { Login, Password, "0", Translate.Tr("Ожидание..."), cookie, "", "0" };
            row.SetValues(temp);
            dataGridView.Rows.Add(row);
        }
        void GetDataTable()
        {
            for (int i = 0; i < OldMail.Count; i++)
            {
                AddElement(OldMail[i]);
            }
            dataGridView.ClearSelection();
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

        private void buttonStart_Click(object sender, EventArgs e)
        {
            sendLink = checkBoxLink.Checked;
            sendPost = checkBoxPost.Checked;
            link = textBoxLink.Text;
            postLink = textBoxPost.Text;
            labelGood.Text = "0"; labelBad.Text = "0";
            time = "";
            labelDone.Text = "0"; labelAll.Text = "0";
            progressBar.Value = 0;
            Proxy.Clear(); ID.Clear(); InstagramDirectMessage.BlackList.Clear(); Blacklist.Clear();
            Final = 0;
            ThreadsCount = 50; Laps = 1; Delay = 5;
            if (OldMail.Count == 0) { MessageBox.Show(Translate.Tr("Аккаунты не указаны!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (textBoxMessage.Text == "") { MessageBox.Show(Translate.Tr("Форма с сообщением для рассылки не указана!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (IDPath == "") { MessageBox.Show(Translate.Tr("Файл с ID пользователей для расссылки не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (isApiProxy)
            {
                if (!(textBoxProxy.Text.Contains("http://") || textBoxProxy.Text.Contains("https://"))) { MessageBox.Show(Translate.Tr("Ссылка для API Proxy указана некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!int.TryParse(textBoxApiProxyUpdateInterval.Text, out ApiProxyTimeout) || ApiProxyTimeout <= 0) { MessageBox.Show(Translate.Tr("Число секунд в интервале обновления указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                ApiProxyUrl = textBoxProxy.Text;
                if (!GetApiProxy()) { MessageBox.Show(Translate.Tr("Не удалось получить прокси по ссылке!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }
            else
                if (ProxyPath == "") { MessageBox.Show(Translate.Tr("Файл с прокси не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (sendLink && !(link.Contains("http://") || link.Contains("https://"))) { MessageBox.Show(Translate.Tr("Ссылка указана некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (sendPost && !(postLink.Contains("http://") || postLink.Contains("https://"))) { MessageBox.Show(Translate.Tr("Ссылка указана некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (!int.TryParse(textBoxThreadsCount.Text, out ThreadsCount) || ThreadsCount <= 0) { MessageBox.Show(Translate.Tr("Количество потоков указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (!int.TryParse(textBoxLaps.Text, out Laps) || Laps <= 0) { MessageBox.Show(Translate.Tr("Количество кругов указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (!int.TryParse(textBoxDelay.Text, out Delay) || Delay <= 0) { MessageBox.Show(Translate.Tr("Число секунд в задержке указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            labelRemaining.Text = CheckLicense.remaining.ToString();

            var data = Properties.Settings.Default.MainForm;
            data.Clear();
            foreach (Control a in panelDirectMailing.Controls)
            {
                if (a.GetType().ToString() == "System.Windows.Forms.TextBox") data.Add(a.Name + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(a.Text)));
            }
            foreach (Control a in panelAccounts.Controls)
            {
                if (a.GetType().ToString() == "System.Windows.Forms.TextBox") data.Add(a.Name + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(a.Text)));
            }
            Properties.Settings.Default.MainForm = data;
            Properties.Settings.Default.Save();

            stop = false;
            buttonStart.Visible = false;
            buttonStop.Visible = true;
            time = DateTime.Now.Date.ToString();
            time = time.Remove(10);
            if (!Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"))) Directory.CreateDirectory(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"));
            TYPE = comboBoxTypeProxy.Text;
            TEXT = textBoxMessage.Text;
            if (!sendLink) link = "";
            if (!sendPost) postLink = "";

            string BlacklistPath = Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstaDirectMessage_BlackList.log");
            InstagramDirectMessage.BlacklistPath = BlacklistPath;
            if (File.Exists(BlacklistPath))
            {
                //Load Blacklist
                using (StreamReader sr = new StreamReader(BlacklistPath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Blacklist.Add(line);
                    }
                }
                InstagramDirectMessage.BlackList = Blacklist;
            }

            //Load User Agents
            var UserAgents = Properties.Resources.UserAgents.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            InstagramDirectMessage.UserAgents = UserAgents;

            if (!isApiProxy)
            {
                //Load Proxy
                using (StreamReader sr = new StreamReader(ProxyPath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Proxy.Add(line);
                    }
                }
            }
            InstagramDirectMessage.Proxy = Proxy;

            //Load ID
            using (StreamReader sr = new StreamReader(IDPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    ID.Add(line);
                }
            }

            List<string> distinct = ID.Distinct().ToList();
            ID = distinct;
            labelAll.Text = ID.Count.ToString();

            //Load Mobile UserAgent from Resources
            InstagramDirectMessage.UserAgentsMob = Properties.Resources.UserAgentMob.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            progressBar.Maximum = ID.Count;
            Progress<int> progress = new Progress<int>(p =>
            {
                if (p == 1) labelGood.Text = (int.Parse(labelGood.Text) + 1).ToString(); else labelBad.Text = (int.Parse(labelBad.Text) + 1).ToString();  
                if (progressBar.Value != progressBar.Maximum)
                {
                    progressBar.Value++;
                    labelDone.Text = progressBar.Value.ToString();
                }
            });
            Progress<int> progressSetMax = new Progress<int>(p =>
            {
                progressBar.Value = progressBar.Maximum;
                //labelDone.Text = labelAll.Text;
            });

            //Если потоков больше, чем работы, то распределяем по единице работы на меньшее количество потоков
            if (ID.Count < ThreadsCount) ThreadsCount = ID.Count;
            //Делим всю работу на равное количество потоков
            int L = 0; int R = ID.Count / ThreadsCount;
            for (int i = 0; i < ThreadsCount; i++)
            {
                new Thread(() => Work(L, R, i, progress)).Start();
                Thread.Sleep(100);
                L = R;
                R += ID.Count / ThreadsCount;
            }
            //Если нацело количество id на потоки не делится, то остаток мы добавляем в отдельный поток
            if (ID.Count % ThreadsCount != 0) { finalresult = ThreadsCount + 1; new Thread(() => Work(R, ID.Count, ThreadsCount, progress)).Start(); } else finalresult = ThreadsCount;

            dataGridView.Sort(dataGridView.Columns["ColumnStatus"], ListSortDirection.Ascending);
            new Thread(() => WaitForEnd(progressSetMax)).Start();
        }
        private void Stats()
        {
            try
            {
                string url = B64X.Encrypt("http://a116901.hostde27.fornex.host/insta/license/InstaDirectMessageStats.php?q=" + MCrypt.Encrypt("done=" + labelGood.Text + "&id=" + Tools.ID.NewIDNumber));
                HttpWebRequest requesttimereal = (HttpWebRequest)WebRequest.Create(B64X.Decrypt(url));
                requesttimereal.Method = "GET";
                requesttimereal.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                HttpWebResponse responze = (HttpWebResponse)requesttimereal.GetResponse();
                string html = B64X.Encrypt(MCrypt.Decrypt(new StreamReader(responze.GetResponseStream()).ReadToEnd()));
            } catch { }
        }

        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (certificate.GetRawCertDataString() == "3082064B30820533A003020102021203F4AA827856B1930459D17AC57FF3A4F273300D06092A864886F70D01010B05003032310B300906035504061302555331163014060355040A130D4C6574277320456E6372797074310B3009060355040313025233301E170D3231303133313131303933375A170D3231303530313131303933375A3020311E301C06035504031315696E7374616D61696C636865636B65722E7369746530820222300D06092A864886F70D01010105000382020F003082020A0282020100DE1CBB12AE9A36C965EF5A3F0ED9964878D849A4C9570E7DEBA052C3F0023667A0E80EC9EC470B5F19E074840956A083A719B370B02D132030B10C95E0AC036C8C69BE4841DFE749DBF5CFC36EE689FF4C267A7900B72703742A6C2539AE04DE3E08E368DF0804FF8CB32916641CBDBE54A14AA4EFC668E323C29637112D4863B4C41F36AA04A99A422D704FE5393DD0D7C17338B84857484E65A67D812AB5C9D6EEC5E3100D9A3E3203C57B67C8539D333C67F492DE45BCA6253785E9501D23B7D31374CA68D61EB5CED76DAC72BACBA00143A7A38C0F39C818AD145913A3C1D67799D1FDF26FE4FC629F03650FC9BF3C8EE567CD7C9134C5B7108A9FBB189F1D472EC4DC79D6BC9A6C861853978EAF1978AB452D4D980379514C9A2DDC2482D5D9A29BAFED56E19CBD84F19C791B402783D3A2290E9B446A770A16AB4C43BAD28CEC9E64BEF7C184C3CCEEA62DAA2953B3A13FF9DCA912115974BD182B7E5C6035099AC1021BA2730B5058D893A4E0126FE804C75858F6041BCED58F542473F2D748EFF2369DBC41D52C70D462D83E8CC503DED8739EE7132DEE5B2476B50C8C7369E25E5FD09095D3D303CEDD01A6550109C7CE3366C0A988A59789A0B919B847DB7835FD9BBE98D36DF9B6F20CD4E60C47A821B9DAB141680D687F5EC278A847118FAAA56DEB09807309E698D9646E43D45066CDCC35B456476F41AB6CEB0203010001A382026B30820267300E0603551D0F0101FF0404030205A0301D0603551D250416301406082B0601050507030106082B06010505070302300C0603551D130101FF04023000301D0603551D0E04160414297CCDE45513CFB3C2CE2F5619FB084D1C66E5A4301F0603551D23041830168014142EB317B75856CBAE500940E61FAF9D8B14C2C6305506082B0601050507010104493047302106082B060105050730018615687474703A2F2F72332E6F2E6C656E63722E6F7267302206082B060105050730028616687474703A2F2F72332E692E6C656E63722E6F72672F303B0603551D11043430328215696E7374616D61696C636865636B65722E7369746582197777772E696E7374616D61696C636865636B65722E73697465304C0603551D20044530433008060667810C0102013037060B2B0601040182DF130101013028302606082B06010505070201161A687474703A2F2F6370732E6C657473656E63727970742E6F726730820104060A2B06010401D6790204020481F50481F200F00076009420BC1E8ED58D6C88731F828B222C0DD1DA4D5E6C4F943D61DB4E2F584DA2C200000177585934490000040300473045022075573493F73DAF3BAA11FD70564A11EF2FD0D7A61A4B828D642E4C0FFF8A9A91022100B0D5ADC09FFAD9FFF457816D00013C06081AB6DDB10ABEA801B4E2EDF52E42010076007D3EF2F88FFF88556824C2C0CA9E5289792BC50E78097F2E6A9768997E22F0D7000001775859348F00000403004730450220617D007BE1F9A8CA43D99229377B201D6E2E96D874312C4F6805ABF892578C80022100DC945A8D40314EFCF10B070EFD159B8B10AA7B3345D91D572B7B18605E236411300D06092A864886F70D01010B050003820101004687A95CC57BDACCFF56CB13A2F5D7B6B0D79A13A6414B9C8B3E66639F85D0C2EBE4E533147CE14E8B8DCFF585CFD1AEB2E1C15911DBED06B07F48F93FBC3242034932ED48C2EE29A4EA6CCF15834B0EE2CCF417A044D202AE4BCB11342D2B2027E51A9C856E9B9EDA51557F64237E05C6678D3A2770361FC698A8555AD6B2C6DA0B651F6B5C639D6759BBD3179359F46649C536A79BC9EE72D463E0B3F888DBE83A76F726C5A46615F9712E7172B6B179B83D211D248BA7F35828A58FA5C4013149A3C1B883972D5CA1E11056AB42D5C951E28B583A1A2F1D80FF5037267999EBB997913C74F46ACAF5EC73B6D89B78223B25FBA9D2E8D512B1AC6A0DC548D7") return true; else return false;
        }
        private void WaitForEnd(IProgress<int> progressSetMax)
        {
            try
            {
                int i = 0;
                while (true)
                {
                    if (i == 10) { Stats(); i = 0; }
                    if (Final == finalresult)
                    {
                        Stats();
                        Thread.Sleep(ThreadsCount * 500);
                        //while (progressBar.Value < progressBar.Maximum - 1) Thread.Sleep(3000);
                        progressSetMax.Report(999);
                        break;
                    }
                    i++;
                    if (stop) return;
                    Thread.Sleep(3000);

                    if (isApiProxy)
                        if (i * 3 >= ApiProxyTimeout)
                        {
                            i = 0;
                            int attempts = 0;
                            while (!GetApiProxy() && attempts < 3) { Thread.Sleep(3000); attempts++; }
                            InstagramDirectMessage.Proxy = Proxy;
                        }
                }
            } catch(Exception e) { /*MessageBox.Show("WaitForEnd: " + e.ToString());*/ }

            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("Рассыльщик сообщений"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            buttonStart.Invoke((MethodInvoker)delegate
            {
                buttonStart.Visible = true;
            });
            buttonStop.Invoke((MethodInvoker)delegate
            {
                buttonStop.Visible = false;
            });
            stop = true;
        }

        private void buttonOpenAccountLog_Click(object sender, EventArgs e)
        {
            dataGridView.Visible = true;
            this.Size = new Size(1300, 720);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stop = true;
            buttonStart.Visible = true;
            buttonStop.Visible = false;
            MessageBox.Show(Translate.Tr("Вы нажали кнопку \"Стоп\"."), Translate.Tr("Остановлено!"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        void buttonSeeLog_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxSeeLogButton.BackColor = Color.Transparent;
        }
        void buttonSeeLog_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxSeeLogButton.BackColor = Color.Indigo;
        }

        void buttonTasksSwitch_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxTasksButton.BackColor = Color.Transparent;
        }
        void buttonTasksSwitch_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxTasksButton.BackColor = Color.Indigo;
        }

        void buttonOpenAccounts_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxAccountsButton.BackColor = Color.Transparent;
        }
        void buttonOpenAccounts_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxAccountsButton.BackColor = Color.Indigo;
        }
        void buttonOpenSettings_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxSettings.BackColor = Color.Transparent;
        }
        void buttonOpenSettings_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxSettings.BackColor = Color.Indigo;
        }
        void buttonWebRegistrator_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxWebRegistrator.BackColor = Color.Transparent;
        }
        void buttonWebRegistrator_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxWebRegistrator.BackColor = Color.Indigo;
        }
        void buttonHashtagParser_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxHashtagParser.BackColor = Color.Transparent;
        }
        void buttonHashtagParser_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxHashtagParser.BackColor = Color.Indigo;
        }
        void buttonUtilities_MouseLeave(object sender, EventArgs e)
        {
            if (panelUtilities.Visible)
            {
                buttonUtilities.BackColor = Color.Indigo;
                return;
            }
            pictureBoxUtilities.BackColor = Color.Transparent;
        }
        void buttonUtilities_MouseEnter(object sender, EventArgs e)
        {
            if (panelUtilities.Visible)
            {
                buttonUtilities.BackColor = Color.Transparent;
                return;
            }
            pictureBoxUtilities.BackColor = Color.Indigo;
        }
        void buttonPosting_MouseLeave(object sender, EventArgs e)
        {
            if (panelPosting.Visible)
            {
                buttonPosting.BackColor = Color.Indigo;
                return;
            }
            pictureBoxAvatarChanger.BackColor = Color.Transparent;
        }
        void buttonPosting_MouseEnter(object sender, EventArgs e)
        {
            if (panelPosting.Visible)
            {
                buttonPosting.BackColor = Color.Transparent;
                return;
            }
            pictureBoxAvatarChanger.BackColor = Color.Indigo;
        }

        private void buttonAccountsSwitch_Click(object sender, EventArgs e)
        {
            panelDirectMailing.Visible = false;
            panelTasks.Visible = false;
            panelSettings.Visible = false;
            this.Size = new Size(1050, 750);
            panelAccounts.Visible = true;
        }
        private void buttonOpenAccountList_Mailing_Click(object sender, EventArgs e)
        {
            panelDirectMailing.Visible = false;
            panelTasks.Visible = false;
            panelSettings.Visible = false;
            this.Size = new Size(1050, 750);
            panelAccounts.Visible = true;
        }
        private void buttonOpenAccounts_Click(object sender, EventArgs e)
        {
            //FormBorderStyle = FormBorderStyle.Sizable;
            panelDirectMailing.Visible = false;
            panelTasks.Visible = false;
            panelSettings.Visible = false;
            this.MinimumSize = new Size(1050, 750);
            this.Size = new Size(1050, 750);
            panelAccounts.Visible = true;
        }
        private void buttonOpenSettings_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.StopIfCheckpoint) comboBoxCheckpointBehavior.Text = "Останавливаться"; else comboBoxCheckpointBehavior.Text = "Брать другой аккаунт (если имеется)";
            if (Properties.Settings.Default.StopIfTempBlocked) comboBoxTempBlock.Text = "Оставить аккаунт"; else comboBoxTempBlock.Text = "Ждать указанное время";
            textBoxTimeoutIfBlocked.Text = Properties.Settings.Default.TimeoutIfBlocked.ToString();
            textBoxProxyTimeout.Text = Properties.Settings.Default.ProxyTimeout.ToString();

            panelDirectMailing.Visible = false;
            panelTasks.Visible = false;
            panelAccounts.Visible = false;
            this.MinimumSize = new Size(550, 750);
            this.Size = new Size(550, 750);
            panelSettings.Visible = true;
        }
        private void buttonTasksSwitch_Click(object sender, EventArgs e)
        {
            /*panelAccounts.Visible = false;
            panelDirectMailing.Visible = true;
            panelTasks.Visible = true;
            this.Size = new Size(670, 720);*/
            buttonOpenDirectMailing_Click(null, null);
        }
        private void buttonOpenDirectMailing_Click(object sender, EventArgs e)
        {
            panelAccounts.Visible = false;
            panelTasks.Visible = false;
            panelSettings.Visible = false;
            panelDirectMailing.Visible = true;
            this.MinimumSize = new Size(670, 750);
            this.Size = new Size(670, 750);
            FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void buttonBackTo_Mailing_Click(object sender, EventArgs e)
        {
            if (!stop) { MessageBox.Show(Translate.Tr("Остановите программу, чтобы сбросить таблицу!"), Translate.Tr("Нажмите \"Стоп\" для этого действия!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                dataGridView.Rows[i].Cells[2].Value = "0";
                dataGridView.Rows[i].Cells[3].Value = Translate.Tr("Ожидание...");
                dataGridView.Rows[i].Cells[6].Value = "0";
            }
        }
        private void buttonWebRegistrator_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Translate.Tr("Данная функция временно отключена! Просим прощения за неудобства!"), Translate.Tr("Внимание!"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;

            if (comboBoxRegistrar.Text == "Web") formWebRegistrator.Show();
                else
                if (comboBoxRegistrar.Text == "API") formApiRegistrator.Show();
                else
                    if (comboBoxRegistrar.Text == "SMS Web") formSMSWebRegistrator.Show(); else formSMSApiRegistrator.Show();
        }
        private void buttonInstagramCheckerSwitch_Click(object sender, EventArgs e)
        {
            formInstagramChecker.Show();
        }
        private void buttonHashtagParser_Click(object sender, EventArgs e)
        {
            if (comboBoxParser.SelectedIndex == 0) formInstagramHashtagsParser.Show();
            else
                if (comboBoxParser.SelectedIndex == 1) formInstagramFollowersParser.Show(); else formInstagramGeoParser.Show();
        }
        private void buttonUtilities_Click(object sender, EventArgs e)
        {
            if (!panelUtilities.Visible)
            {
                panelUtilities.Visible = true;
            } else
            {
                panelUtilities.Visible = false;
            }
            panelUtilities.BringToFront();
        }
        private void buttonAvatarChanger_Click(object sender, EventArgs e)
        {
            if (!panelPosting.Visible)
            {
                panelPosting.Visible = true;
            }
            else
            {
                panelPosting.Visible = false;
            }
            panelPosting.BringToFront();
            //formAvatarChanger.Show();
        }
        private void buttonAccountRecover_Click(object sender, EventArgs e)
        {
            formSMSAccountRecover.Show();
        }
        private void buttonClearAutocomplete_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FormApiRegistrator.Clear();
            Properties.Settings.Default.FormAvatarChanger.Clear();
            Properties.Settings.Default.FormBreakdownDB.Clear();
            Properties.Settings.Default.FormDeleteCookie.Clear();
            Properties.Settings.Default.FormDeleteDuplicate.Clear();
            Properties.Settings.Default.FormFollowersParser.Clear();
            Properties.Settings.Default.FormGeoParser.Clear();
            Properties.Settings.Default.FormHashtagsParser.Clear();
            Properties.Settings.Default.FormInstagramChecker.Clear();
            Properties.Settings.Default.FormMassFollowing.Clear();
            Properties.Settings.Default.FormMobUserAgentActualizer.Clear();
            Properties.Settings.Default.FormPhotoWebPosting.Clear();
            Properties.Settings.Default.FormSMSAccountRecover.Clear();
            Properties.Settings.Default.FormSMSApiRegistrator.Clear();
            Properties.Settings.Default.FormSMSWebRegistrator.Clear();
            Properties.Settings.Default.FormUserAgentActualizer.Clear();
            Properties.Settings.Default.FormWebRegistrator.Clear();
            Properties.Settings.Default.MainForm.Clear();
            Properties.Settings.Default.Save();
            MessageBox.Show(Translate.Tr("Данные автозаполнения успешно очищены!"), Translate.Tr("Успех!"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void buttonLoadAccounts_Click(object sender, EventArgs e)
        {
            if (OldEmailsPath == "") { MessageBox.Show(Translate.Tr("Файл с аккаунтами не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            //Load OldMail
            using (StreamReader sr = new StreamReader(OldEmailsPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!OldMail.Exists(z => z == line))
                    {
                        OldMail.Add(line);
                        AddElement(line);
                    }
                }
            }
            dataGridView.ClearSelection();
        }

        private void buttonClearDataGrid_Click(object sender, EventArgs e)
        {
            if (!stop) { MessageBox.Show(Translate.Tr("Остановите программу, чтобы очистить таблицу!"), Translate.Tr("Нажмите \"Стоп\" для этого действия!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            OldMail.Clear();
            dataGridView.Rows.Clear();
            dataGridView.ClearSelection();
        }

        private void linkLabelTechSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://tele.gg/instadirect_support");
        }

        private void buttonSeeLog_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstaDirectMessage.log"))) MessageBox.Show(Translate.Tr("Файла с логом пока нет!"), "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else Process.Start(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstaDirectMessage.log"));
        }

        private void toolStripActualizerUA_Web_Click(object sender, EventArgs e)
        {
            buttonUtilities.BackColor = Color.Transparent;
            panelUtilities.Visible = false;
            buttonUtilities_MouseLeave(null, null);
            formUserAgentActualizer.Show();
        }

        private void toolStripActualizerUA_API_Click(object sender, EventArgs e)
        {
            buttonUtilities.BackColor = Color.Transparent;
            panelUtilities.Visible = false;
            buttonUtilities_MouseLeave(null, null);
            formMobUserAgentActualizer.Show();
        }

        private void toolStripDeleteDublicate_Click(object sender, EventArgs e)
        {
            buttonUtilities.BackColor = Color.Transparent;
            panelUtilities.Visible = false;
            buttonUtilities_MouseLeave(null, null);
            formDeleteDuplicate.Show();
        }

        private void toolStripDeleteCookie_Click(object sender, EventArgs e)
        {
            buttonUtilities.BackColor = Color.Transparent;
            panelUtilities.Visible = false;
            buttonUtilities_MouseLeave(null, null);
            formDeleteCookie.Show();
        }

        private void toolStripBreakdownFile_Click(object sender, EventArgs e)
        {
            buttonUtilities.BackColor = Color.Transparent;
            panelUtilities.Visible = false;
            buttonUtilities_MouseLeave(null, null);
            formBreakdownDB.Show();
        }

        private void toolStripAvatarChanger_Click(object sender, EventArgs e)
        {
            buttonPosting.BackColor = Color.Transparent;
            panelPosting.Visible = false;
            buttonPosting_MouseLeave(null, null);
            formAvatarChanger.Show();
        }

        private void toolStripPhotoPosingWeb_Click(object sender, EventArgs e)
        {
            buttonPosting.BackColor = Color.Transparent;
            panelPosting.Visible = false;
            buttonPosting_MouseLeave(null, null);
            formPhotoWebPosting.Show();
        }

        private void toolStripAccountChecker_Click(object sender, EventArgs e)
        {
            buttonUtilities.BackColor = Color.Transparent;
            panelUtilities.Visible = false;
            buttonUtilities_MouseLeave(null, null);
            formInstagramChecker.Show();
        }

        private void toolStripMassFollowing_Click(object sender, EventArgs e)
        {
            buttonPosting.BackColor = Color.Transparent;
            panelPosting.Visible = false;
            buttonPosting_MouseLeave(null, null);
            formMassFollowing.Show();
        }

        private void toolStripFollowersGreeting_Click(object sender, EventArgs e)
        {
            buttonPosting.BackColor = Color.Transparent;
            panelPosting.Visible = false;
            buttonPosting_MouseLeave(null, null);
            formFollowersGreeting.Show();
        }

        private void buttonOpenBlackList_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstaDirectMessage_BlackList.log"))) MessageBox.Show(Translate.Tr("Файла с чёрным списком пока нет!"), "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else Process.Start(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstaDirectMessage_BlackList.log"));
        }

        private void buttonClearBlackList_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstaDirectMessage_BlackList.log"))) MessageBox.Show(Translate.Tr("Файла с чёрным списком пока нет!"), "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                DialogResult dialogResult = MessageBox.Show(Translate.Tr("Вы действительно хотите очистить чёрный список рассылки?"), Translate.Tr("Точно?"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    File.Delete(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstaDirectMessage_BlackList.log"));
                    MessageBox.Show(Translate.Tr("Файл с чёрным списком очищен!"), "Готово!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void linkLabelTelegram_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://tele.gg/instadirect_aks");
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

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            if (!stop) MessageBox.Show(Translate.Tr("Остановите программу, чтобы изменить настройки!"), Translate.Tr("Нажмите \"Стоп\" для этого действия!"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (comboBoxCheckpointBehavior.Text == "Останавливаться") Properties.Settings.Default.StopIfCheckpoint = true; else Properties.Settings.Default.StopIfCheckpoint = false;
            if (comboBoxTempBlock.SelectedIndex == 0) Properties.Settings.Default.StopIfTempBlocked = true; else Properties.Settings.Default.StopIfTempBlocked = false;
            if (textBoxTimeoutIfBlocked.TextLength != 0) Properties.Settings.Default.TimeoutIfBlocked = int.Parse(textBoxTimeoutIfBlocked.Text); else MessageBox.Show(Translate.Tr("Неверное количество секунд!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (textBoxProxyTimeout.TextLength != 0) Properties.Settings.Default.ProxyTimeout = int.Parse(textBoxProxyTimeout.Text); else MessageBox.Show(Translate.Tr("Неверное количество секунд!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (Properties.Settings.Default.Language != comboBoxLanguage.Text)
            {
                if (comboBoxLanguage.Text == "Русский")
                {
                    DialogResult dialogResult = MessageBox.Show("Чтобы изменить язык, программе нужно закрыться. Вы согласны?", "Нужна перезагрузка", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Properties.Settings.Default.Language = comboBoxLanguage.Text;
                        Properties.Settings.Default.Save();
                        Environment.Exit(0);
                    }
                } else 
                if (comboBoxLanguage.Text == "English")
                {
                    DialogResult dialogResult = MessageBox.Show("To change the language, the program needs to close. Do you agree?", "I need a reboot!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Properties.Settings.Default.Language = comboBoxLanguage.Text;
                        Properties.Settings.Default.Save();
                        Environment.Exit(0);
                    }
                }
            }
            Properties.Settings.Default.Save();
            MessageBox.Show(Translate.Tr("Настройки программы были обновлены."), Translate.Tr("Сохранено!"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkBoxPost_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPost.Checked)
            {
                labelPost.Visible = true;
                textBoxPost.Visible = true;

                textBoxMessage.Enabled = false;
                labelMailingText.Enabled = false;
                checkBoxLink.Enabled = false;
                labelLink.Enabled = false;
                textBoxLink.Enabled = false;

                checkBoxLink.Checked = false;

                if (checkBoxLink.Checked)
                {
                    labelLink.Visible = true;
                    textBoxLink.Visible = true;
                }
                else
                {
                    labelLink.Visible = false;
                    textBoxLink.Visible = false;
                }
            }
            else
            {
                labelPost.Visible = false;
                textBoxPost.Visible = false;

                textBoxMessage.Enabled = true;
                labelMailingText.Enabled = true;
                checkBoxLink.Enabled = true;
                labelLink.Enabled = true;
                textBoxLink.Enabled = true;
                if (checkBoxLink.Checked)
                {
                    labelLink.Visible = true;
                    textBoxLink.Visible = true;
                }
                else
                {
                    labelLink.Visible = false;
                    textBoxLink.Visible = false;
                }
            }
        }

        private void checkBoxLink_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLink.Checked)
            {
                labelLink.Visible = true;
                textBoxLink.Visible = true;
            } else
            {
                labelLink.Visible = false;
                textBoxLink.Visible = false;
            }
        }

        private async void Work(int left, int right, int tt, IProgress<int> progress)
        {
            await Task.Run(() => MainThread(left, right, tt, progress));
            //MainThread(left, right, LeftNM, LeftSP, tt, progress);
        }

        [STAThread]
        private void MainThread(int left, int right, int tt, IProgress<int> progress)
        {
            //MessageBox.Show(stop.ToString() + " Check this out");

            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int indx = left;
            //Если идентификаторов больше, чем прокси, ты выбираем случайно, чтобы ускорить программу и снизить нагрузку на определённый прокси
            if (OldMail.Count > Proxy.Count) indx = rnd.Next(Proxy.Count);
            
            for (int potok = 0; potok < Laps; potok++)
            for (int i = left; i < right; i++)
            try
            {
                    Console.WriteLine(tt + " " + Final + " " + i.ToString());
                if (stop) return;
                if (i >= ID.Count) break;
                int j = i % OldMail.Count;
                     //MessageBox.Show(tt.ToString() + " " + i.ToString() + " " + OldMail.Count.ToString());
                string Login = "", Password = "", UserAgent = "", device_id = "", phone_id = "", cookie = "", adid = "", guid = "";
                if (OldMail[j].Contains("||"))
                {
                    // Login:Password||DeviceId;PhoneId;ADID;GUID|Cookie||
                    if (OldMail[j].Substring(OldMail[j].Length - 2) != "\\") OldMail[j] += "\\";
                    if (new Regex("(.*):(.*)\\|\\|(.*)\\|(.*)\\|\\|").IsMatch(OldMail[j]))
                    {
                        Regex regex = new Regex("(.*):(.*)\\|\\|(.*)\\|(.*)\\|\\|");
                        var mathes = regex.Match(OldMail[j]);

                        Login = mathes.Groups[1].Value;
                        Password = mathes.Groups[2].Value;
                        cookie = mathes.Groups[4].Value;
                        cookie = cookie.Replace(";", "; ");

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
                        var mathes = regex.Match(OldMail[j]);

                        Login = mathes.Groups[1].Value;
                        Password = mathes.Groups[2].Value;
                        UserAgent = mathes.Groups[3].Value;
                        cookie = mathes.Groups[5].Value;
                        cookie = cookie.Replace(";", "; ");

                        Regex regex2 = new Regex("(.*);(.*);(.*);(.*)");
                        var mathes_2 = regex2.Match(mathes.Groups[4].Value);
                        device_id = mathes_2.Groups[1].Value;
                        phone_id = mathes_2.Groups[2].Value;
                        guid = mathes_2.Groups[3].Value;
                        adid = mathes_2.Groups[4].Value;
                    }
                } else
                {
                    Regex regex = new Regex("(.*):(.*)");
                    var mathes = regex.Match(OldMail[j]);
                    Login = mathes.Groups[1].Value;
                    Password = mathes.Groups[2].Value;
                }

                                Thread.Sleep(1000);

                int search = Search(Login);
                if (search == -1) break;
                if (int.Parse(dataGridView.Rows[search].Cells[2].Value.ToString()) >= Laps) { dataGridView.Rows[search].Cells[3].Value = Translate.Tr("Закончил."); continue; }
                if (dataGridView.Rows[search].Cells[3].Value.ToString().Contains(Translate.Tr("Чекпойнт"))) { continue; }
                if (dataGridView.Rows[search].Cells[3].Value.ToString() == Translate.Tr("Лимит!") && Properties.Settings.Default.StopIfTempBlocked) continue;

                dataGridView.Rows[search].Cells[3].Value = Translate.Tr("Запуск...");

                int attempts = 0;
                while (attempts < 3)
                {
                    if (stop) return;
                    if (int.Parse(dataGridView.Rows[search].Cells[2].Value.ToString()) >= Laps) { dataGridView.Rows[search].Cells[3].Value = Translate.Tr("Закончил."); break; }
                    if (dataGridView.Rows[search].Cells[3].Value.ToString().Contains(Translate.Tr("Чекпойнт"))) { break; }
                    if (dataGridView.Rows[search].Cells[3].Value.ToString() == Translate.Tr("Лимит!") && Properties.Settings.Default.StopIfTempBlocked) break;

                    InstagramDirectMessage ins = new InstagramDirectMessage(Login, Password, ID[i], cookie, device_id, adid, guid, phone_id, indx, time, tt, TYPE, TEXT, Delay, link, postLink, UserAgent);
                    Thread.Sleep(500);

                    search = Search(Login);
                    if (search == -1) break;

                    if (labelAll.Text == labelDone.Text) { break; }

                    if (ins.good == 1)
                    {
                        if (int.Parse(dataGridView.Rows[search].Cells[2].Value.ToString()) >= Laps) { dataGridView.Rows[search].Cells[3].Value = Translate.Tr("Закончил."); break; }
                        progress.Report(ins.good);
                        dataGridView.Rows[search].Cells[2].Value = (int.Parse(dataGridView.Rows[search].Cells[2].Value.ToString()) + 1).ToString();
                        dataGridView.Rows[search].Cells[3].Value = Translate.Tr("Сообщение отправлено.");
                        dataGridView.Rows[search].Cells[6].Value = "0";
                        dataGridView.Rows[search].Cells[5].Value += ins.log;
                        break;
                    }
                    else
                    if (ins.good == 0)
                    {
                        dataGridView.Rows[search].Cells[6].Value = (int.Parse(dataGridView.Rows[search].Cells[6].Value.ToString()) + 1).ToString();
                        dataGridView.Rows[search].Cells[3].Value = Translate.Tr("Переотправляем x") + dataGridView.Rows[search].Cells[6].Value.ToString();
                        dataGridView.Rows[search].Cells[5].Value += ins.log;
                        //Thread.Sleep(Properties.Settings.Default.TimeoutIfBlocked * 1000);
                        Thread.Sleep(Delay * 1000);
                        indx++;
                        attempts++;
                    } else
                    if (ins.good == 2)
                    {
                        dataGridView.Rows[search].Cells[3].Value = Translate.Tr("Чекпойнт!");
                        dataGridView.Rows[search].Cells[5].Value += ins.log;
                        //if (Properties.Settings.Default.StopIfCheckpoint) progress.Report(0);
                        break;
                    } else 
                    if (ins.good == 3)
                    {
                        dataGridView.Rows[search].Cells[6].Value = (int.Parse(dataGridView.Rows[search].Cells[6].Value.ToString()) + 1).ToString();
                        dataGridView.Rows[search].Cells[3].Value = Translate.Tr("Лимит!");
                        dataGridView.Rows[search].Cells[5].Value += ins.log;
                        if (Properties.Settings.Default.StopIfTempBlocked) break; else Thread.Sleep(Properties.Settings.Default.TimeoutIfBlocked * 1000);
                    } else 
                    if (ins.good == 4)
                    {
                        dataGridView.Rows[search].Cells[3].Value = Translate.Tr("Нет такого ID!");
                        dataGridView.Rows[search].Cells[5].Value += ins.log;
                        progress.Report(0);
                        break;
                    } else
                    if (ins.good == 5)
                    {
                        dataGridView.Rows[search].Cells[3].Value = Translate.Tr("ID в чёрном списке!");
                        dataGridView.Rows[search].Cells[5].Value += ins.log;
                        progress.Report(0);
                        break;
                    }
                    if (attempts == 3) { progress.Report(0); dataGridView.Rows[search].Cells[3].Value = Translate.Tr("Ошибка!"); dataGridView.Rows[search].Cells[6].Value = "0"; }
                }
                
                Console.WriteLine("Success. " + tt);
                //MessageBox.Show(tt + " " + k + " " + Final);
            } catch(Exception e) { /*MessageBox.Show(e.ToString());*/ }
            Final += 1;
        }

        private void Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            /*if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }*/
        }


        int Search(string login)
        {
            for (int i = 0; i < OldMail.Count; i++)
            {
                if (dataGridView.Rows[i].Cells[0].Value.ToString() == login) return i;
            }
            return -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
        private void buttonOpenID_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxID.Text = openFileDialog.FileName;
            IDPath = openFileDialog.FileName;
        }

        private void buttonOpenOldEmails_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxOldEmails.Text = openFileDialog.FileName;
            OldEmailsPath = openFileDialog.FileName;
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
    }
}
