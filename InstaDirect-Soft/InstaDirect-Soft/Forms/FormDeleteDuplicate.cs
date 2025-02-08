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
    public partial class FormDeleteDuplicate : Form
    {
        public static string SourceFilePath = "", ResultPath = "";
        public static List<string> SourceFile = new List<string>();
        public static bool stop = true, NeedSort = false;

        public FormDeleteDuplicate()
        {
            InitializeComponent();

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            this.FormClosing += FormInstagramChecker_FormClosing;

            List<string> key = new List<string>();
            List<string> value = new List<string>();
            var data = Properties.Settings.Default.FormDeleteDuplicate;
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

            SourceFilePath = textBoxSourceFile.Text;
            ResultPath = textBoxResultFile.Text;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            SourceFile.Clear();
            labelWas.Text = "0";
            labelIs.Text = "0";
            if (SourceFilePath == "") { MessageBox.Show(Translate.Tr("Исходный файл не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            NeedSort = checkBoxNeedSort.Checked;

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            var data = Properties.Settings.Default.FormDeleteDuplicate;
            data.Clear();
            foreach (Control a in this.Controls)
            {
                if (a.GetType().ToString() == "System.Windows.Forms.TextBox") data.Add(a.Name + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(a.Text)));
            }
            Properties.Settings.Default.FormDeleteDuplicate = data;
            Properties.Settings.Default.Save();

            buttonStart.Visible = false;
            buttonStop.Visible = true;
            stop = false;

            //Load source file
            using (StreamReader sr = new StreamReader(SourceFilePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    SourceFile.Add(line);
                }
            }
            labelWas.Text = SourceFile.Count.ToString();

            /*string abc = "mozilla/5.0 (linux; Android 8.0.0; Moto G (4) Build/NPJS25.93-14-4; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/67.0.3396.87 Mobile Safari/537.36 [FB_IAB/FB4A;FBAV/186.0.0.48.81;]";
            string bac = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.16 Safari/537.36 Edg/79.0.309.7";*/

            Progress<int> progress = new Progress<int>(p =>
            {
                labelIs.Text = p.ToString();
            });

            new Thread(() => Work(progress)).Start();
            Thread.Sleep(100);
            new Thread(() => WaitForEnd()).Start();
            Thread.Sleep(100);
        }

        private void Work(IProgress<int> progress)
        {
            List<string> distinct = SourceFile.Distinct().ToList();
            SourceFile = distinct;
            if (NeedSort) SourceFile.Sort();
            File.WriteAllLines(ResultPath, SourceFile);
            progress.Report(SourceFile.Count);

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
            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("Удаление дубликатов"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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

            textBoxSourceFile.Text = openFileDialog.FileName;
            SourceFilePath = openFileDialog.FileName;
        }
    }
}
