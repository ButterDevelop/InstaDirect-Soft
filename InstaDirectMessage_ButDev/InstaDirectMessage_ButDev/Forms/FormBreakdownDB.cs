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
    public partial class FormBreakdownDB : Form
    {
        public static string SourceFilePath = "", ResultPath = "";
        public static List<string> SourceFile = new List<string>();
        public static bool stop = true;
        public static int Divide = 0;

        public FormBreakdownDB()
        {
            InitializeComponent();

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            this.FormClosing += FormInstagramChecker_FormClosing;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            SourceFile.Clear();
            labelIs.Text = "0";
            if (SourceFilePath == "") { MessageBox.Show(Translate.Tr("Исходный файл не указан!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (!int.TryParse(textBoxDivide.Text, out Divide) || Divide <= 0) { MessageBox.Show(Translate.Tr("Число частей для разбивки файла не указано или указано некорректно!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            CheckLicense.GetRemainingTime();
            if (CheckLicense.remaining < 0) { MessageBox.Show(Translate.Tr("Лицензия истекла!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

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

            Progress<int> progress = new Progress<int>(p =>
            {
                labelIs.Text = (int.Parse(labelIs.Text) + 1).ToString();
            });

            new Thread(() => Work(progress)).Start();
            Thread.Sleep(100);
            new Thread(() => WaitForEnd()).Start();
            Thread.Sleep(100);
        }

        private void Work(IProgress<int> progress)
        {
            var lists = new List<string>[Divide + 1];
            for (int i = 0; i < Divide; i++)
            {
                lists[i] = new List<string>();
                lists[i].AddRange(SourceFile.Skip((SourceFile.Count / Divide) * i).Take((SourceFile.Count / Divide)).ToArray());
                File.WriteAllLines(Path.Combine(ResultPath, Path.GetFileNameWithoutExtension(SourceFilePath) + "_" + (i+1).ToString() + ".txt"), lists[i]);
                progress.Report(1);
            }
            if (SourceFile.Count % Divide != 0)
            {
                lists[Divide - 1] = new List<string>();
                lists[Divide-1].AddRange(SourceFile.Skip(SourceFile.Count - (SourceFile.Count % Divide)).Take(SourceFile.Count % Divide).ToArray());
                File.AppendAllLines(Path.Combine(ResultPath, Path.GetFileNameWithoutExtension(SourceFilePath) + "_" + Divide.ToString() + ".txt"), lists[Divide-1]);
            }

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
            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("Разбивка файла на части"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
            FolderBrowserDialog DirectoryDialog = new FolderBrowserDialog();
            if (DirectoryDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxResultFile.Text = DirectoryDialog.SelectedPath;
                ResultPath = DirectoryDialog.SelectedPath;
            }
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
