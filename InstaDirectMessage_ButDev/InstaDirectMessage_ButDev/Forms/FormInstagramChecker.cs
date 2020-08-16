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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstaDirectMessage_ButDev
{
    public partial class FormInstagramChecker : Form
    {
        public static string ProxyPath = "", NicknamesPath = "", ResultFilePath = "", ResultPathStatic = "", PROXYTYPE = "", time = "", ApiProxyUrl = "";
        public static int ThreadsCount = 0, Final = 0, finalresult = 0, Delay = 0, ApiProxyTimeout = 600;
        public static List<string> Proxy = new List<string>();
        public static List<string> Nicknames = new List<string>();
        public static bool stop = true, isApiProxy = false;

        public FormInstagramChecker()
        {
            InitializeComponent();

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            richTextBoxLog.AutoScrollOffset = new Point();
            this.FormClosing += FormInstagramChecker_FormClosing;

            List<string> key = new List<string>();
            List<string> value = new List<string>();
            var data = Properties.Settings.Default.FormInstagramChecker;
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
            NicknamesPath = textBoxNicknames.Text;
            ResultFilePath = textBoxResultFile.Text;
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
            } else
            {
                labelApiProxyLink.Visible = false;
                textBoxProxy.ReadOnly = true;
                buttonOpenProxy.Visible = true;
                textBoxProxy.Clear();

                textBoxApiProxyUpdateInterval.Visible = false;
                labelApiProxyUpdateInterval.Visible = false;
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

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Final = 0; labelGood.Text = labelDone.Text = labelGood.Text = labelBad.Text = "0";
            Proxy.Clear(); Nicknames.Clear(); richTextBoxLog.Clear();
            if (!int.TryParse(textBoxThreadsCount.Text, out ThreadsCount) || ThreadsCount <= 0) { MessageBox.Show(Translate.Tr("Количество потоков указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (!int.TryParse(textBoxDelay.Text, out Delay) || Delay <= 0) { MessageBox.Show(Translate.Tr("Число секунд в задержке указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (isApiProxy)
            {
                if (!(textBoxProxy.Text.Contains("http://") || textBoxProxy.Text.Contains("https://"))) { MessageBox.Show(Translate.Tr("Ссылка для API Proxy указана некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!int.TryParse(textBoxApiProxyUpdateInterval.Text, out ApiProxyTimeout) || ApiProxyTimeout <= 0) { MessageBox.Show(Translate.Tr("Число секунд в интервале обновления указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                ApiProxyUrl = textBoxProxy.Text;
                if (!GetApiProxy()) { MessageBox.Show(Translate.Tr("Не удалось получить прокси по ссылке!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            } else 
                if (ProxyPath == "") { MessageBox.Show(Translate.Tr("Файл с прокси не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (NicknamesPath == "") { MessageBox.Show(Translate.Tr("Файл с никнеймами не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (ResultFilePath == "" && isFileNameValid(ResultFilePath)) { MessageBox.Show(Translate.Tr("Файл, куда нужно сохранять результат, не указан или указан некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            var data = Properties.Settings.Default.FormInstagramChecker;
            data.Clear();
            foreach (Control a in this.Controls)
            {
                if (a.GetType().ToString() == "System.Windows.Forms.TextBox") data.Add(a.Name + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(a.Text)));
            }
            Properties.Settings.Default.FormInstagramChecker = data;
            Properties.Settings.Default.Save();

            buttonStart.Visible = false;
            buttonStop.Visible = true;
            stop = false;

            time = DateTime.Now.Date.ToString();
            time = time.Remove(10);
            string path = Path.Combine(Environment.CurrentDirectory, time);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (!Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"))) Directory.CreateDirectory(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"));
            path = Path.Combine(Environment.CurrentDirectory, time);
            InstagramChecker.path = path;

            ResultPathStatic = ResultFilePath;
            InstagramChecker.GoodPath = ResultPathStatic;

            InstagramChecker.proxytype = comboBoxTypeProxy.Text;

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
            InstagramChecker.Proxy = Proxy;

            //Load Nicknames
            using (StreamReader sr = new StreamReader(NicknamesPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Nicknames.Add(line);
                }
            }
            List<string> distinct = Nicknames.Distinct().ToList();
            Nicknames = distinct;

            //Load User Agents
            var UserAgents = Properties.Resources.UserAgents.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            InstagramChecker.UserAgents = UserAgents;

            labelAll.Text = Nicknames.Count.ToString();

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
                labelDone.Text = (int.Parse(labelDone.Text) + 1).ToString();
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

            if (ThreadsCount > Nicknames.Count) ThreadsCount = Nicknames.Count;
            int L = 0; int R = Nicknames.Count / ThreadsCount;
            for (int i = 0; i < ThreadsCount; i++)
            {
                new Thread(() => Work(L, R, i, Nicknames.Count / ThreadsCount, progress, log)).Start();
                Thread.Sleep(100);
                L = R;
                R += Nicknames.Count / ThreadsCount;
            }
            if (Nicknames.Count % ThreadsCount != 0) { finalresult = ThreadsCount + 1; new Thread(() => Work(R, Nicknames.Count, ThreadsCount, Nicknames.Count % ThreadsCount, progress, log)).Start(); } else finalresult = ThreadsCount;

            new Thread(() => WaitForEnd(btnVisibility)).Start();
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
            } catch(Exception e)
            {
                return false;
            }
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
                            InstagramChecker.Proxy = Proxy;
                        }
                }
            }
            catch (Exception e) { /*MessageBox.Show("WaitForEnd: " + e.ToString());*/ }

            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("Чекер аккаунтов"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
                /*if (stop) this.Hide(); else MessageBox.Show("Нельзя закрыть окно, пока оно работает!", "Чекер аккаунтов: Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
            e.Cancel = true;
            this.Hide();
        }

        private void buttonSeeLog_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstagramChecker.log"))) MessageBox.Show(Translate.Tr("Файла с логом пока нет!"), Translate.Tr("Чекер аккаунтов: Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            else Process.Start(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstagramChecker.log"));
        }

        private async void Work(int L, int R, int tt, int count, IProgress<int> progress, IProgress<string> log)
        {
            await Task.Run(() => MainThread(L, R, tt, (Proxy.Count / (ThreadsCount + 1)) * tt, count, progress, log));
        }
        private void MainThread(int L, int R, int tt, int indx, int count, IProgress<int> progress, IProgress<string> log)
        {
            for (int i = L; i < R; i++)
            {
                try
                {
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    InstagramChecker check = new InstagramChecker(Nicknames[i], indx, tt);
                    progress.Report(check.good);

                    log.Report(check.log);
                     
                    Thread.Sleep(Delay * 1000);
                }
                catch (Exception ex) { /*MessageBox.Show(ex.ToString());*/ progress.Report(0); }
            }
            Final++;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stop = true;
            buttonStop.Visible = false;
            Thread.Sleep(ThreadsCount * 500);
            buttonStart.Visible = true;
            MessageBox.Show(Translate.Tr("Вы нажали кнопку \"Стоп\"."), Translate.Tr("Чекер аккаунтов"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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

        private void buttonOpenNicknames_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxNicknames.Text = openFileDialog.FileName;
            NicknamesPath = openFileDialog.FileName;
        }

    }
}
