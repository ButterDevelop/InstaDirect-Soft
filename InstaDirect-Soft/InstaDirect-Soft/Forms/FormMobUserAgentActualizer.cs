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
    public partial class FormMobUserAgentActualizer : Form
    {
        public static string OldUAPath = "", ResultPath = "", AcceptLanguage = "";
        public static int HowManyAsResult = 0;
        public static List<string> OldUA = new List<string>();
        public static bool stop = true;

        public FormMobUserAgentActualizer()
        {
            InitializeComponent();

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            this.FormClosing += FormInstagramChecker_FormClosing;

            List<string> key = new List<string>();
            List<string> value = new List<string>();
            var data = Properties.Settings.Default.FormMobUserAgentActualizer;
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

            OldUAPath = textBoxOldUA.Text;
            ResultPath = textBoxResultFile.Text;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            OldUA.Clear();
            labelGood.Text = "0";
            if (!int.TryParse(textBoxHowManyAsResult.Text, out HowManyAsResult) || HowManyAsResult < 0) { MessageBox.Show(Translate.Tr("Количество ЮзерАгентов в результате указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (OldUAPath == "") { MessageBox.Show(Translate.Tr("Файл со старыми ЮзерАгентами не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            var data = Properties.Settings.Default.FormMobUserAgentActualizer;
            data.Clear();
            foreach (Control a in this.Controls)
            {
                if (a.GetType().ToString() == "System.Windows.Forms.TextBox") data.Add(a.Name + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(a.Text)));
            }
            Properties.Settings.Default.FormMobUserAgentActualizer = data;
            Properties.Settings.Default.Save();

            buttonStart.Visible = false;
            buttonStop.Visible = true;
            stop = false;

            //Load Old UserAgents
            using (StreamReader sr = new StreamReader(OldUAPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    OldUA.Add(line);
                }
            }

            /*string abc = "mozilla/5.0 (linux; Android 8.0.0; Moto G (4) Build/NPJS25.93-14-4; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/67.0.3396.87 Mobile Safari/537.36 [FB_IAB/FB4A;FBAV/186.0.0.48.81;]";
            string bac = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.16 Safari/537.36 Edg/79.0.309.7";*/

            Progress<int> progress = new Progress<int>(p =>
            {
                labelGood.Text = (int.Parse(labelGood.Text) + 1).ToString();
            });

            new Thread(() => Work(progress)).Start();
            Thread.Sleep(100);
            new Thread(() => WaitForEnd()).Start();
            Thread.Sleep(100);
        }

        private void Work(IProgress<int> progress)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            string[] androidVersions = new string[] { "18/4.3", "19/4.4.2", "21/5.0", "22/5.1", "22/5.1.1", "23/6.0", "23/6.0.1", "23/6.1", "24/7.0", "25/7.1", "25/7.1.1", "25/7.1.2", "26/8.0.0", "26/8.1" };
            HashSet<string> results = new HashSet<string>();
            List<string> dpi = new List<string>();
            List<string> resolution = new List<string>();
            List<string> model = new List<string>();
            string instVersion = textBoxInstVersion.Text, region = textBoxRegion.Text;

            foreach (string str in OldUA)
            {
                Regex regex = new Regex("Instagram .* Android \\(.*; (.*dpi); (.*x.*); (.*) ..-..; .*\\)");
                if (regex.IsMatch(str) == false) continue;
                var m = regex.Match(str);
                string DPI = m.Groups[1].Value;
                string RESOLUTION = m.Groups[2].Value;
                string MODEL = m.Groups[3].Value;

                if (!dpi.Contains(DPI)) dpi.Add(DPI);
                if (!resolution.Contains(RESOLUTION)) resolution.Add(RESOLUTION);
                if (!model.Contains(MODEL)) model.Add(MODEL);
            }

            if (HowManyAsResult == 0) HowManyAsResult = OldUA.Count;
            for (int i = 0; i < HowManyAsResult; i++)
            {
                string a = "";
                do
                {
                    if (stop) return;

                    Random rnd = new Random(int.Parse(DateTime.Now.Ticks.ToString().Substring(10)));
                    a = $"Instagram {instVersion} Android ({androidVersions[rnd.Next(androidVersions.Length)]}; {dpi[rnd.Next(dpi.Count)]}; {resolution[rnd.Next(resolution.Count)]}; {model[rnd.Next(model.Count)]} {region}; 2{rnd.Next(10000000, 90000000).ToString()})";
                } while (results.Contains(a));

                File.AppendAllText(ResultPath, a + Environment.NewLine);
                results.Add(a);
                progress.Report(1);
            }

            //Instagram 179.0.0.28.132 Android (23/6.0; 380dpi; 720x1280; Nokia; 1 Plus; tissot_sprout; hi3650; ru-RU; 220561401)
            /*if (HowManyAsResult == 0)
            {
                for (int i = 0; i < OldUA.Count; i++)
                {
                    string a = OldUA[i];
                    do
                    {
                        if (stop) return; 
                        Regex regex = new Regex("Instagram .* Android \\(23/6.0; 380dpi; 720x1280; Nokia; 1 Plus; tissot_sprout; hi3650; (..-..); 220561401\\)");

                        var match = regex.Match(a);
                        string num = match.Groups[1].Value;
                        string android = match.Groups[2].Value;
                        string other = match.Groups[3].Value;
                        string accept_lang = match.Groups[4].Value;

                        if (AcceptLanguage != "-1") accept_lang = AcceptLanguage;

                        a = $"{random.Next(19, 29)}/{androidVersions[random.Next(androidVersions.Length)]}; {other}; {accept_lang}";
                    } while (results.Contains(a));
                    File.AppendAllText(ResultPath, a + Environment.NewLine);
                    results.Add(a);
                    progress.Report(1);
                }
            }

            if (HowManyAsResult >= OldUA.Count)
                for (int i = 0; i < (HowManyAsResult / OldUA.Count); i++)
                    foreach (string str in OldUA)
                    {
                        string a = OldUA[i];
                        do
                        {
                            if (stop) return;
                            Regex regex = new Regex("(.{1,3})\\/(.{1,6}); (.*); (.._..)");

                            var match = regex.Match(a);
                            string num = match.Groups[1].Value;
                            string android = match.Groups[2].Value;
                            string other = match.Groups[3].Value;
                            string accept_lang = match.Groups[4].Value;

                            if (AcceptLanguage != "-1") accept_lang = AcceptLanguage;

                            a = $"{random.Next(19, 29)}/{androidVersions[random.Next(androidVersions.Length)]}; {other}; {accept_lang}";
                        } while (results.Contains(a));
                        File.AppendAllText(ResultPath, a + Environment.NewLine);
                        results.Add(a);
                        progress.Report(1);
                    }

            for (int i = 0; i < (HowManyAsResult % OldUA.Count); i++)
            {
                string a = OldUA[i];
                do
                {
                    if (stop) return;
                    Regex regex = new Regex("(.{1,3})\\/(.{1,6}); (.*); (.._..)");

                    var match = regex.Match(a);
                    string num = match.Groups[1].Value;
                    string android = match.Groups[2].Value;
                    string other = match.Groups[3].Value;
                    string accept_lang = match.Groups[4].Value;

                    if (AcceptLanguage != "-1") accept_lang = AcceptLanguage;

                    a = $"{random.Next(19, 29)}/{androidVersions[random.Next(androidVersions.Length)]}; {other}; {accept_lang}";
                } while (results.Contains(a));
                File.AppendAllText(ResultPath, a + Environment.NewLine);
                results.Add(a);
                progress.Report(1);
            }*/

            stop = true;
        }

        private void WaitForEnd()
        {
            try
            {
                while (true)
                {
                    if (stop) break;
                    Thread.Sleep(3000);
                }
            }
            catch (Exception e) { /*MessageBox.Show("WaitForEnd: " + e.ToString());*/ }

            buttonStart.Invoke((MethodInvoker)delegate
            {
                buttonStart.Visible = true;
            });
            buttonStop.Invoke((MethodInvoker)delegate
            {
                buttonStop.Visible = false;
            });
            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("Актуализатор ЮзерАгентов"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
                /*if (stop) this.Hide(); else MessageBox.Show("Нельзя закрыть окно, пока оно работает!", "Парсер хэштегов: Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
            e.Cancel = true;
            this.Hide();
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
            ResultPath = openFileDialog.FileName;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stop = true;
            buttonStop.Visible = false;
        }

        private void buttonOpenOldUA_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBoxOldUA.Text = openFileDialog.FileName;
            OldUAPath = openFileDialog.FileName;
        }
    }
}
