using InstaDirectMessage_ButDev.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstaDirectMessage_ButDev
{
    public partial class FormFollowersGreeting : Form
    {
        public static string ProxyPath = "", PROXYTYPE = "", time = "", ApiProxyUrl = "", Account = "", link = "", postLink = "", TYPE = "", TEXT = "", UserName = "";
        public static int ThreadsCount = 0, Interval = 0, ApiProxyTimeout = 600;
        public static List<string> Proxy = new List<string>();
        public static bool stop = true, isApiProxy = false, sendLink = false, sendPost = false;

        public FormFollowersGreeting()
        {
            InitializeComponent();

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            richTextBoxLog.AutoScrollOffset = new Point();
            this.FormClosing += FormInstagramChecker_FormClosing;

            List<string> key = new List<string>();
            List<string> value = new List<string>();
            var data = Properties.Settings.Default.FormFollowersParser;
            foreach (string k in data)
            {
                if (k == "") continue;
                var dict = k.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                key.Add(dict[0]);
                value.Add(Encoding.UTF8.GetString(Convert.FromBase64String(dict[1])));
            }
            foreach (Control a in this.Controls)
            {
                if (a.GetType().ToString() != "System.Windows.Forms.CheckBox") continue;
                if (key.Contains(a.Name)) ((CheckBox)a).Checked = value[key.FindIndex(x => x == a.Name)] == "1" ? true : false;
            }
            foreach (Control a in this.Controls)
            {
                if (a.GetType().ToString() == "System.Windows.Forms.CheckBox") continue;
                if (key.Contains(a.Name)) a.Text = value[key.FindIndex(x => x == a.Name)];
            }

            ProxyPath = textBoxProxy.Text;
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
            }
            else
            {
                labelLink.Visible = false;
                textBoxLink.Visible = false;
            }
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

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Proxy.Clear();
            labelGood.Text = "0"; labelBad.Text = "0";
            sendLink = checkBoxLink.Checked;
            sendPost = checkBoxPost.Checked;
            link = textBoxLink.Text;
            postLink = textBoxPost.Text;
            if (!int.TryParse(textBoxInterval.Text, out Interval) || Interval < 0) { MessageBox.Show(Translate.Tr("Интервал указан некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (isApiProxy)
            {
                if (!(textBoxProxy.Text.Contains("http://") || textBoxProxy.Text.Contains("https://"))) { MessageBox.Show(Translate.Tr("Ссылка для API Proxy указана некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!int.TryParse(textBoxApiProxyUpdateInterval.Text, out ApiProxyTimeout) || ApiProxyTimeout <= 0) { MessageBox.Show(Translate.Tr("Число секунд в интервале обновления указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                ApiProxyUrl = textBoxProxy.Text;
                if (!GetApiProxy()) { MessageBox.Show(Translate.Tr("Не удалось получить прокси по ссылке!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }
            else
                if (ProxyPath == "") { MessageBox.Show(Translate.Tr("Файл с прокси не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (textBoxMessage.Text == "") { MessageBox.Show(Translate.Tr("Форма с сообщением для рассылки не указана!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (textBoxAccounts.Text == "") { MessageBox.Show(Translate.Tr("Аккаунт не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (sendLink && !(link.Contains("http://") || link.Contains("https://"))) { MessageBox.Show(Translate.Tr("Ссылка указана некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (sendPost && !(postLink.Contains("http://") || postLink.Contains("https://"))) { MessageBox.Show(Translate.Tr("Ссылка указана некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            Account = textBoxAccounts.Text;

            var data = Properties.Settings.Default.FormFollowersParser;
            data.Clear();
            foreach (Control a in this.Controls)
            {
                if (a.GetType().ToString() != "System.Windows.Forms.CheckBox") continue;
                data.Add(a.Name + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(((CheckBox)a).Checked ? "1" : "0")));
            }
            foreach (Control a in this.Controls)
            {
                if (a.GetType().ToString() == "System.Windows.Forms.TextBox") data.Add(a.Name + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(a.Text)));
            }
            Properties.Settings.Default.FormFollowersParser = data;
            Properties.Settings.Default.Save();

            UserName = Account;
            UserName = UserName.Remove(UserName.IndexOf(":"));
            if (InstagramFollowersGreeting.UserName != UserName)
            {
                InstagramFollowersGreeting.logincookie = "";
                InstagramFollowersGreeting.logincookieMob = "";
            }
            InstagramFollowersGreeting.UserName = UserName;
            InstagramFollowersGreeting.Last = "";

            buttonStart.Visible = false;
            buttonStop.Visible = true;
            stop = false;
            InstagramFollowersGreeting.stop = false;

            time = DateTime.Now.Date.ToString();
            time = time.Remove(10);
            string path = Path.Combine(Environment.CurrentDirectory, time);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (!Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"))) Directory.CreateDirectory(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"));
            path = Path.Combine(Environment.CurrentDirectory, time);
            InstagramFollowersGreeting.path = path;

            TYPE = comboBoxTypeProxy.Text;
            TEXT = textBoxMessage.Text;
            if (!sendLink) link = "";
            if (!sendPost) postLink = "";
            InstagramFollowersGreeting.TYPE = TYPE;
            InstagramFollowersGreeting.TEXT = TEXT;
            InstagramFollowersGreeting.link = link;
            InstagramFollowersGreeting.postlink = postLink;

            InstagramFollowersGreeting.proxytype = comboBoxTypeProxy.Text;
            InstagramFollowersGreeting.Interval = Interval;

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
            InstagramFollowersGreeting.Proxy = Proxy;

            //Load User Agents
            var UserAgents = Properties.Resources.UserAgents.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            InstagramFollowersGreeting.UserAgents = UserAgents;
            //Load User Agents Mob
            var UserAgentsMob = Properties.Resources.UserAgentMob.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            InstagramFollowersGreeting.UserAgentsMob = UserAgentsMob;

            Progress<int> setGood = new Progress<int>(p =>
            {
                labelGood.Text = (int.Parse(labelGood.Text) + p).ToString();
            });

            Progress<KeyValuePair<int, int>> progress = new Progress<KeyValuePair<int, int>>(p =>
            {
                if (p.Key != -1)
                {
                    labelGood.Text = (int.Parse(labelGood.Text) + p.Key).ToString();
                    labelBad.Text = (int.Parse(labelBad.Text) + (p.Value - p.Key)).ToString();
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

            new Thread(() => Work(Account, progress, log, setGood)).Start();
            Thread.Sleep(300);

            new Thread(() => WaitForEnd(btnVisibility)).Start();
        }

        private void WaitForEnd(IProgress<int> vis)
        {
            try
            {
                int i = 0;
                while (true)
                {
                    i++;
                    if (stop) return;
                    Thread.Sleep(3000);

                    if (isApiProxy)
                        if (i * 3 >= ApiProxyTimeout)
                        {
                            i = 0;
                            int attempts = 0;
                            while (!GetApiProxy() && attempts < 3) { Thread.Sleep(3000); attempts++; }
                            InstagramFollowersGreeting.Proxy = Proxy;
                        }
                }
            }
            catch (Exception e) { /*MessageBox.Show("WaitForEnd: " + e.ToString());*/ }

            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("Приветствие новых подписчиков"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            vis.Report(999);
            stop = true;
        }

        private void FormInstagramChecker_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                /*if (MessageBox.Show("Вы уверены что хотите закрыть программу?", "Выйти?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Exit();
                }*/
            /*if (stop) this.Hide(); else MessageBox.Show("Нельзя закрыть окно, пока оно работает!", "Парсер подписчиков: Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }*/
            e.Cancel = true;
            this.Hide();
        }

        private void buttonSeeLog_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), $"InstagramFollowersGreeting_{UserName}.log"))) MessageBox.Show(Translate.Tr("Файла с логом пока нет!"), Translate.Tr("Приветствие новых подписчиков: Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            else Process.Start(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), $"InstagramFollowersGreeting_{UserName}.log"));
        }

        private async void Work(string acc, IProgress<KeyValuePair<int, int>> progress, IProgress<string> log, IProgress<int> setGood)
        {
            await Task.Run(() => MainThread(acc, progress, log, setGood));
        }
        private void MainThread(string acc, IProgress<KeyValuePair<int, int>> progress, IProgress<string> log, IProgress<int> setGood)
        {
            while (true)
            {
                try
                {
                    if (stop) return;

                    InstagramFollowersGreeting worker = new InstagramFollowersGreeting(Account, setGood);
                    progress.Report(worker.good);

                    log.Report(worker.log);

                    Thread.Sleep(Interval * 1000);
                }
                catch (Exception ex) { /*MessageBox.Show(ex.ToString());*/ progress.Report(new KeyValuePair<int, int>(0,0)); }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stop = true;
            InstagramFollowersGreeting.stop = true;
            buttonStop.Visible = false;
            Thread.Sleep(ThreadsCount * 500);
            buttonStart.Visible = true;
            MessageBox.Show(Translate.Tr("Вы нажали кнопку \"Стоп\"."), Translate.Tr("Парсер подписчиков"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
