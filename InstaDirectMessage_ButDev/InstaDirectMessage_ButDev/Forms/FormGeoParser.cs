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
    public partial class FormGeoParser : Form
    {
        public static string ProxyPath = "", GeolocationsPath = "", ResultPath = "", ResultPathStatic = "", PROXYTYPE = "", time = "", ApiProxyUrl = "";
        public static int ThreadsCount = 0, Final = 0, finalresult = 0, HowManyID = 0, ApiProxyTimeout = 0;
        public static List<string> Proxy = new List<string>();
        public static List<string> Geolocations = new List<string>();
        public static bool stop = true, isApiProxy = false;

        public FormGeoParser()
        {
            InitializeComponent();

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            comboBoxResultFormat.SelectedIndex = 0;
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

        private void buttonOpenResult_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog DirectoryDialog = new FolderBrowserDialog();
            if (DirectoryDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxResultPath.Text = DirectoryDialog.SelectedPath;
                ResultPath = DirectoryDialog.SelectedPath;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Final = 0;
            Proxy.Clear(); Geolocations.Clear();
            labelGood.Text = "0"; labelBad.Text = "0"; labelDone.Text = "0"; labelAll.Text = "0";
            if (!int.TryParse(textBoxThreadsCount.Text, out ThreadsCount) || ThreadsCount <= 0) { MessageBox.Show(Translate.Tr("Количество потоков указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (!int.TryParse(textBoxHowManyID.Text, out HowManyID) || HowManyID < 0) { MessageBox.Show(Translate.Tr("Количество ID для парсинга указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (GeolocationsPath == "") { MessageBox.Show(Translate.Tr("Файл с геолокациями не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (isApiProxy)
            {
                if (!(textBoxProxy.Text.Contains("http://") || textBoxProxy.Text.Contains("https://"))) { MessageBox.Show(Translate.Tr("Ссылка для API Proxy указана некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!int.TryParse(textBoxApiProxyUpdateInterval.Text, out ApiProxyTimeout) || ApiProxyTimeout <= 0) { MessageBox.Show(Translate.Tr("Число секунд в интервале обновления указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                ApiProxyUrl = textBoxProxy.Text;
                if (!GetApiProxy()) { MessageBox.Show(Translate.Tr("Не удалось получить прокси по ссылке!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }
            else
                if (ProxyPath == "") { MessageBox.Show(Translate.Tr("Файл с прокси не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (ResultPath == "" && isFileNameValid(ResultPath)) { MessageBox.Show(Translate.Tr("Файл, куда нужно сохранять результат, не указан или указан некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            buttonStart.Visible = false;
            buttonStop.Visible = true;
            stop = false;
            InstagramGeoParser.stop = false;

            time = DateTime.Now.Date.ToString();
            time = time.Remove(10);
            string path = Path.Combine(Environment.CurrentDirectory, time);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (!Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"))) Directory.CreateDirectory(Path.Combine(Path.Combine(Environment.CurrentDirectory, time), "Logs"));
            path = Path.Combine(Environment.CurrentDirectory, time);
            InstagramGeoParser.path = path;

            ResultPathStatic = ResultPath;
            InstagramGeoParser.GoodPath = ResultPathStatic;
            if (HowManyID == 0) InstagramGeoParser.HowManyID = int.MaxValue; else InstagramGeoParser.HowManyID = HowManyID;
            InstagramGeoParser.proxytype = comboBoxTypeProxy.Text;
            InstagramGeoParser.resulttype = comboBoxResultFormat.SelectedIndex;

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
            InstagramGeoParser.Proxy = Proxy;

            //Load Geolocations
            using (StreamReader sr = new StreamReader(GeolocationsPath, true))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Geolocations.Add(line);
                }
            }

            //Load User Agents
            var UserAgents = Properties.Resources.UserAgents.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            InstagramGeoParser.UserAgents = UserAgents;

            labelAll.Text = Geolocations.Count.ToString();

            Progress<int> setGood = new Progress<int>(p =>
            {
                labelGood.Text = (int.Parse(labelGood.Text) + p).ToString();
            });

            Progress<int> progress = new Progress<int>(p =>
            {
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

            if (ThreadsCount > Geolocations.Count) ThreadsCount = Geolocations.Count;
            int L = 0; int R = Geolocations.Count / ThreadsCount;
            for (int potok = 0; potok < ThreadsCount; potok++)
            {
                new Thread(() => Work(L, R, potok, progress, log, setGood)).Start();
                Thread.Sleep(300);
                L = R;
                R += Geolocations.Count / ThreadsCount;
            }
            if (Geolocations.Count % ThreadsCount != 0) { finalresult = ThreadsCount + 1; new Thread(() => Work(R, Geolocations.Count, ThreadsCount, progress, log, setGood)).Start(); } else finalresult = ThreadsCount;

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
            }
            catch (Exception e)
            {
                return false;
            }
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
                            InstagramGeoParser.Proxy = Proxy;
                        }
                }
            }
            catch (Exception e) { /*MessageBox.Show("WaitForEnd: " + e.ToString());*/ }

            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("Парсер геолокаций"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
                /*if (stop) this.Hide(); else MessageBox.Show("Нельзя закрыть окно, пока оно работает!", "Парсер геолокаций: Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
            e.Cancel = true;
            this.Hide();
        }

        private void buttonSeeLog_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstagramGeoParser.log"))) MessageBox.Show("Файла с логом пока нет!", "Парсер геолокаций: Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else Process.Start(Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, time)), "InstagramGeoParser.log"));
        }

        private async void Work(int L, int R, int tt, IProgress<int> progress, IProgress<string> log, IProgress<int> setGood)
        {
            await Task.Run(() => MainThread(L, R, tt, (Proxy.Count / (ThreadsCount + 1)) * tt, progress, log, setGood));
        }
        private void MainThread(int L, int R, int tt, int indx, IProgress<int> progress, IProgress<string> log, IProgress<int> setGood)
        {
            for (int i = L; i < R; i++)
            {
                try
                {
                    if (stop) return;
                    if (i >= Geolocations.Count) break;

                    InstagramGeoParser parser = new InstagramGeoParser(Geolocations[i], indx, tt, setGood);
                    progress.Report(parser.good);

                    log.Report(parser.log);
                     
                    Thread.Sleep(1000);
                }
                catch (Exception ex) { /*MessageBox.Show(ex.ToString());*/ progress.Report(0); }
            }
            Final++;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stop = true;
            InstagramGeoParser.stop = true;
            buttonStop.Visible = false;
            Thread.Sleep(ThreadsCount * 500);
            buttonStart.Visible = true;
            MessageBox.Show(Translate.Tr("Вы нажали кнопку \"Стоп\"."), Translate.Tr("Парсер геолокаций"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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

        private void buttonOpenGeolocations_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxGeolocations.Text = openFileDialog.FileName;
            GeolocationsPath = openFileDialog.FileName;
        }
    }
}
