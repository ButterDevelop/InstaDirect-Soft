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
    public partial class FormDeleteCookie : Form
    {
        public static string SourceFilePath = "", ResultPath = "";
        public static List<string> SourceFile = new List<string>();
        public static bool stop = true;

        public FormDeleteCookie()
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
            foreach (string a in SourceFile)
            {
                string Login = "", Password = "", UserAgent = "", device_id = "", phone_id = "", cookie = "", adid = "", guid = "";
                if (a.Contains("||"))
                {
                    // Login:Password||DeviceId;PhoneId;ADID;GUID|Cookie||
                    if (new Regex("(.*):(.*)\\|\\|(.*)\\|(.*)\\|\\|").IsMatch(a))
                    {
                        Regex regex = new Regex("(.*):(.*)\\|\\|(.*)\\|(.*)\\|\\|");
                        var mathes = regex.Match(a);

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
                        var mathes = regex.Match(a);

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
                }
                else
                {
                    Regex regex = new Regex("(.*):(.*)");
                    var mathes = regex.Match(a);
                    Login = mathes.Groups[1].Value;
                    Password = mathes.Groups[2].Value;
                }
                File.AppendAllText(ResultPath, Login + ":" + Password + Environment.NewLine);
                progress.Report(1);
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
            MessageBox.Show(Translate.Tr("Завершено!"), Translate.Tr("Удаление cookie"), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
