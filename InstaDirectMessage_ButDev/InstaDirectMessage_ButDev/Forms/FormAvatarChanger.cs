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
    public partial class FormAvatarChanger : Form
    {
        public static string ProxyPath = "", AccountsPath = "", ResultPath = "", ImagesPath = "", PROXYTYPE = "", time = "", ApiProxyUrl = "";
        public static int ThreadsCount = 0, Final = 0, finalresult = 0, HowManyID = 0, ApiProxyTimeout = 600;
        public static List<string> Proxy = new List<string>();
        public static List<string> Accounts = new List<string>();
        public static List<Image> Images = new List<Image>();
        public static bool stop = true, isApiProxy = false, ShouldUniquePhoto = true;

        public FormAvatarChanger()
        {
            InitializeComponent();

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            richTextBoxLog.AutoScrollOffset = new Point();
            this.FormClosing += FormInstagramChecker_FormClosing;

            List<string> key = new List<string>();
            List<string> value = new List<string>();
            var data = Properties.Settings.Default.FormAvatarChanger;
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
            ResultPath = textBoxResult.Text;
            ImagesPath = textBoxResultPath.Text;
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
            if (ImagesPath == "" && isFileNameValid(ImagesPath)) { MessageBox.Show(Translate.Tr("Папка с картинками не указан или указан некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            var data = Properties.Settings.Default.FormAvatarChanger;
            data.Clear();
            foreach (Control a in this.Controls)
            {
                if (a.GetType().ToString() == "System.Windows.Forms.TextBox") data.Add(a.Name + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(a.Text)));
            }
            Properties.Settings.Default.FormAvatarChanger = data;
            Properties.Settings.Default.Save();

            buttonStart.Visible = false;
            buttonStop.Visible = true;
            stop = false;
            InstagramAvatarChanger.stop = false;
            ShouldUniquePhoto = checkBoxShouldPhotoUnique.Checked;

            time = DateTime.Now.Date.ToString();
            time = time.Remove(10);
            string path = Path.Combine(Environment.CurrentDirectory, time);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (!Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"))) Directory.CreateDirectory(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"));
            path = Path.Combine(Environment.CurrentDirectory, time);
            InstagramAvatarChanger.path = path;
            InstagramAvatarChanger.ResultPath = ResultPath;
            InstagramAvatarChanger.proxytype = comboBoxTypeProxy.Text;

            var files = Directory.GetFiles(ImagesPath, "*.*", SearchOption.TopDirectoryOnly).ToList();
            foreach (string filename in files)
            {
                if (Regex.IsMatch(filename, @".jpg|.jpeg|.tiff|.bmp|.svg|.png|.gif$")) Images.Add(Image.FromFile(filename));
            }
            if (Images.Count == 0) { MessageBox.Show(Translate.Tr("В выбранной папке нет картинок!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

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
            InstagramAvatarChanger.Proxy = Proxy;

            //Load Accs
            using (StreamReader sr = new StreamReader(AccountsPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Accounts.Add(line);
                }
            }
            InstagramAvatarChanger.Accounts = Accounts;

            //Load User Agents
            var UserAgents = Properties.Resources.UserAgents.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            InstagramAvatarChanger.UserAgents = UserAgents;

            labelAll.Text = Accounts.Count.ToString();

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
                            InstagramAvatarChanger.Proxy = Proxy;
                        }
                }
            }
            catch (Exception e) { /*MessageBox.Show("WaitForEnd: " + e.ToString());*/ }

            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("Смена аватарок"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
            if (!File.Exists(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstagramAvatarChanger.log"))) MessageBox.Show(Translate.Tr("Файла с логом пока нет!"), Translate.Tr("Смена аватарок: Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            else Process.Start(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstagramAvatarChanger.log"));
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

                    Random random = new Random(Guid.NewGuid().GetHashCode());
                    Image ava = Images[random.Next(Images.Count)];
                    if (ShouldUniquePhoto) ava = ImageUnique.ChangeImage(ava);
                    InstagramAvatarChanger worker = new InstagramAvatarChanger(Accounts[i], indx, tt, ava);
                    progress.Report(worker.good);

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
            InstagramAvatarChanger.stop = true;
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

        private void buttonOpenImages_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog DirectoryDialog = new FolderBrowserDialog();
            if (DirectoryDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxResultPath.Text = DirectoryDialog.SelectedPath;
                ImagesPath = DirectoryDialog.SelectedPath;
            }
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
    }
}
