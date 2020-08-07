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
    public partial class FormMassFollowing : Form
    {
        public static string ProxyPath = "", AccountsPath = "", ResultPath = "", IDPath = "", PROXYTYPE = "", time = "", ApiProxyUrl = "";
        public static int ThreadsCount = 0, Final = 0, finalresult = 0, HowManyID = 0, ApiProxyTimeout = 600, Delay = 0;
        public static List<string> Proxy = new List<string>();
        public static List<string> Accounts = new List<string>();
        public static List<string> ID = new List<string>();
        public static bool stop = true, isApiProxy = false;

        public FormMassFollowing()
        {
            InitializeComponent();

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            richTextBoxLog.AutoScrollOffset = new Point();
            this.FormClosing += FormInstagramChecker_FormClosing;
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
            Final = 0;
            Proxy.Clear(); Accounts.Clear();
            labelGood.Text = "0"; labelBad.Text = "0"; labelDone.Text = "0"; labelAll.Text = "0";
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
            if (ResultPath == "" && isFileNameValid(ResultPath)) { MessageBox.Show(Translate.Tr("Файл, куда нужно сохранять результат, не указан или указан некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            buttonStart.Visible = false;
            buttonStop.Visible = true;
            stop = false;
            InstagramMassFollowing.stop = false;

            time = DateTime.Now.Date.ToString();
            time = time.Remove(10);
            string path = Path.Combine(Environment.CurrentDirectory, time);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (!Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"))) Directory.CreateDirectory(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"));
            path = Path.Combine(Environment.CurrentDirectory, time);
            InstagramMassFollowing.path = path;
            InstagramMassFollowing.ResultPath = ResultPath;
            InstagramMassFollowing.proxytype = comboBoxTypeProxy.Text;
            InstagramMassFollowing.Delay = Delay;

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
            InstagramMassFollowing.Proxy = Proxy;

            //Load Accs
            using (StreamReader sr = new StreamReader(AccountsPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Accounts.Add(line);
                }
            }

            //Load ID
            using (StreamReader sr = new StreamReader(IDPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    ID.Add(line);
                }
            }
            InstagramMassFollowing.ID = ID;

            //Load User Agents
            var UserAgents = Properties.Resources.UserAgentsWebAndroid.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            InstagramMassFollowing.UserAgents = UserAgents;

            labelAll.Text = Accounts.Count.ToString();

            Progress<int> progress = new Progress<int>(p =>
            {
                labelGood.Text = (int.Parse(labelGood.Text) + p).ToString();
                labelBad.Text = (int.Parse(labelBad.Text) + ID.Count - p).ToString();

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

            new Thread(() => WaitForEnd(btnVisibility)).Start();
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
                            InstagramMassFollowing.Proxy = Proxy;
                        }
                }
            }
            catch (Exception e) { /*MessageBox.Show("WaitForEnd: " + e.ToString());*/ }

            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("Постинг фото (web)"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
            /*if (stop) this.Hide(); else MessageBox.Show("Нельзя закрыть окно, пока оно работает!", "Смена аватарок: Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }*/
            e.Cancel = true;
            this.Hide();
        }

        private void buttonSeeLog_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstagramMassFollowing.log"))) MessageBox.Show(Translate.Tr("Файла с логом пока нет!"), Translate.Tr("Постинг фото (web): Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            else Process.Start(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstagramMassFollowing.log"));
        }

        private async void Work(int L, int R, int tt, IProgress<int> progress, IProgress<string> log)
        {
            await Task.Run(() => MainThread(L, R, tt, (Proxy.Count / (ThreadsCount + 1)) * tt, (Accounts.Count / (ThreadsCount + 1)) * tt, progress, log));
        }
        private void MainThread(int L, int R, int tt, int indx, int indexAcc, IProgress<int> progress, IProgress<string> log)
        {
            for (int i = L; i < R; i++)
            {
                try
                {
                    if (stop) return;
                    if (i >= Accounts.Count) break;

                    InstagramMassFollowing worker = new InstagramMassFollowing(Accounts[i], indx, tt);
                    progress.Report(worker.GoodCount);

                    log.Report(worker.log);

                    Thread.Sleep(1000);
                }
                catch (Exception ex) { /*MessageBox.Show(ex.ToString());*/ progress.Report(0); }
            }
            Final++;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stop = true;
            InstagramMassFollowing.stop = true;
            buttonStop.Visible = false;
            Thread.Sleep(ThreadsCount * 500);
            buttonStart.Visible = true;
            MessageBox.Show(Translate.Tr("Вы нажали кнопку \"Стоп\"."), Translate.Tr("Смена аватарок"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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

        private void buttonOpenResult_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxResult.Text = openFileDialog.FileName;
            ResultPath = openFileDialog.FileName;
        }

        private void buttonOpenFollowIDs_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxFollowIDs.Text = openFileDialog.FileName;
            IDPath = openFileDialog.FileName;
        }
    }
}
