namespace InstaDirectMessage_ButDev
{
    partial class FormSMSAccountRecover
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSMSAccountRecover));
            this.textBoxProxy = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxThreadsCount = new System.Windows.Forms.TextBox();
            this.buttonOpenProxy = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOpenAccs = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxTypeProxy = new System.Windows.Forms.ComboBox();
            this.labelBad = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonSeeLog = new System.Windows.Forms.Button();
            this.labelGood = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxUserAgents = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonOpenUserAgents = new System.Windows.Forms.Button();
            this.textBoxResultFile = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonOpenResultFile = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.textBoxAccounts = new System.Windows.Forms.TextBox();
            this.textBoxApiProxyUpdateInterval = new System.Windows.Forms.TextBox();
            this.labelApiProxyUpdateInterval = new System.Windows.Forms.Label();
            this.labelApiProxyLink = new System.Windows.Forms.Label();
            this.checkBoxIsApiProxy = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.linkLabelSmsService = new System.Windows.Forms.LinkLabel();
            this.textBoxAPIKey = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.comboBoxPhoneRegion = new System.Windows.Forms.ComboBox();
            this.comboBoxService = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxApiKeyRuCaptcha = new System.Windows.Forms.TextBox();
            this.textBoxDelay = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxProxy
            // 
            resources.ApplyResources(this.textBoxProxy, "textBoxProxy");
            this.textBoxProxy.Name = "textBoxProxy";
            this.textBoxProxy.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // textBoxThreadsCount
            // 
            resources.ApplyResources(this.textBoxThreadsCount, "textBoxThreadsCount");
            this.textBoxThreadsCount.Name = "textBoxThreadsCount";
            // 
            // buttonOpenProxy
            // 
            resources.ApplyResources(this.buttonOpenProxy, "buttonOpenProxy");
            this.buttonOpenProxy.Name = "buttonOpenProxy";
            this.buttonOpenProxy.UseVisualStyleBackColor = true;
            this.buttonOpenProxy.Click += new System.EventHandler(this.buttonOpenProxy_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // buttonOpenAccs
            // 
            resources.ApplyResources(this.buttonOpenAccs, "buttonOpenAccs");
            this.buttonOpenAccs.Name = "buttonOpenAccs";
            this.buttonOpenAccs.UseVisualStyleBackColor = true;
            this.buttonOpenAccs.Click += new System.EventHandler(this.buttonOpenAccounts_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.Transparent;
            this.buttonStart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(221)))), ((int)(((byte)(253)))));
            this.buttonStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(221)))), ((int)(((byte)(253)))));
            resources.ApplyResources(this.buttonStart, "buttonStart");
            this.buttonStart.ForeColor = System.Drawing.Color.Black;
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.Color.Transparent;
            this.buttonStop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(221)))), ((int)(((byte)(253)))));
            this.buttonStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(221)))), ((int)(((byte)(253)))));
            resources.ApplyResources(this.buttonStop, "buttonStop");
            this.buttonStop.ForeColor = System.Drawing.Color.Black;
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Name = "label8";
            // 
            // comboBoxTypeProxy
            // 
            this.comboBoxTypeProxy.FormattingEnabled = true;
            this.comboBoxTypeProxy.Items.AddRange(new object[] {
            resources.GetString("comboBoxTypeProxy.Items"),
            resources.GetString("comboBoxTypeProxy.Items1")});
            resources.ApplyResources(this.comboBoxTypeProxy, "comboBoxTypeProxy");
            this.comboBoxTypeProxy.Name = "comboBoxTypeProxy";
            // 
            // labelBad
            // 
            resources.ApplyResources(this.labelBad, "labelBad");
            this.labelBad.BackColor = System.Drawing.Color.Transparent;
            this.labelBad.ForeColor = System.Drawing.Color.Red;
            this.labelBad.Name = "labelBad";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Name = "label11";
            // 
            // buttonSeeLog
            // 
            this.buttonSeeLog.BackColor = System.Drawing.Color.Transparent;
            this.buttonSeeLog.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonSeeLog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(221)))), ((int)(((byte)(253)))));
            this.buttonSeeLog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(221)))), ((int)(((byte)(253)))));
            resources.ApplyResources(this.buttonSeeLog, "buttonSeeLog");
            this.buttonSeeLog.ForeColor = System.Drawing.Color.Black;
            this.buttonSeeLog.Name = "buttonSeeLog";
            this.buttonSeeLog.UseVisualStyleBackColor = false;
            this.buttonSeeLog.Click += new System.EventHandler(this.buttonSeeLog_Click);
            // 
            // labelGood
            // 
            resources.ApplyResources(this.labelGood, "labelGood");
            this.labelGood.BackColor = System.Drawing.Color.Transparent;
            this.labelGood.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.labelGood.Name = "labelGood";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Name = "label10";
            // 
            // textBoxUserAgents
            // 
            resources.ApplyResources(this.textBoxUserAgents, "textBoxUserAgents");
            this.textBoxUserAgents.Name = "textBoxUserAgents";
            this.textBoxUserAgents.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Name = "label6";
            // 
            // buttonOpenUserAgents
            // 
            resources.ApplyResources(this.buttonOpenUserAgents, "buttonOpenUserAgents");
            this.buttonOpenUserAgents.Name = "buttonOpenUserAgents";
            this.buttonOpenUserAgents.UseVisualStyleBackColor = true;
            this.buttonOpenUserAgents.Click += new System.EventHandler(this.buttonOpenUserAgents_Click);
            // 
            // textBoxResultFile
            // 
            resources.ApplyResources(this.textBoxResultFile, "textBoxResultFile");
            this.textBoxResultFile.Name = "textBoxResultFile";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Name = "label9";
            // 
            // buttonOpenResultFile
            // 
            resources.ApplyResources(this.buttonOpenResultFile, "buttonOpenResultFile");
            this.buttonOpenResultFile.Name = "buttonOpenResultFile";
            this.buttonOpenResultFile.UseVisualStyleBackColor = true;
            this.buttonOpenResultFile.Click += new System.EventHandler(this.buttonOpenResultFile_Click);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Name = "label12";
            // 
            // richTextBoxLog
            // 
            resources.ApplyResources(this.richTextBoxLog, "richTextBoxLog");
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            // 
            // textBoxAccounts
            // 
            resources.ApplyResources(this.textBoxAccounts, "textBoxAccounts");
            this.textBoxAccounts.Name = "textBoxAccounts";
            this.textBoxAccounts.ReadOnly = true;
            // 
            // textBoxApiProxyUpdateInterval
            // 
            resources.ApplyResources(this.textBoxApiProxyUpdateInterval, "textBoxApiProxyUpdateInterval");
            this.textBoxApiProxyUpdateInterval.Name = "textBoxApiProxyUpdateInterval";
            // 
            // labelApiProxyUpdateInterval
            // 
            resources.ApplyResources(this.labelApiProxyUpdateInterval, "labelApiProxyUpdateInterval");
            this.labelApiProxyUpdateInterval.BackColor = System.Drawing.Color.Transparent;
            this.labelApiProxyUpdateInterval.Name = "labelApiProxyUpdateInterval";
            // 
            // labelApiProxyLink
            // 
            resources.ApplyResources(this.labelApiProxyLink, "labelApiProxyLink");
            this.labelApiProxyLink.BackColor = System.Drawing.Color.Transparent;
            this.labelApiProxyLink.ForeColor = System.Drawing.Color.Black;
            this.labelApiProxyLink.Name = "labelApiProxyLink";
            // 
            // checkBoxIsApiProxy
            // 
            resources.ApplyResources(this.checkBoxIsApiProxy, "checkBoxIsApiProxy");
            this.checkBoxIsApiProxy.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxIsApiProxy.Name = "checkBoxIsApiProxy";
            this.checkBoxIsApiProxy.UseVisualStyleBackColor = false;
            this.checkBoxIsApiProxy.CheckedChanged += new System.EventHandler(this.checkBoxIsApiProxy_CheckedChanged);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Name = "label16";
            // 
            // linkLabelSmsService
            // 
            resources.ApplyResources(this.linkLabelSmsService, "linkLabelSmsService");
            this.linkLabelSmsService.BackColor = System.Drawing.Color.Transparent;
            this.linkLabelSmsService.Name = "linkLabelSmsService";
            this.linkLabelSmsService.TabStop = true;
            this.linkLabelSmsService.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSmsService_LinkClicked);
            // 
            // textBoxAPIKey
            // 
            resources.ApplyResources(this.textBoxAPIKey, "textBoxAPIKey");
            this.textBoxAPIKey.Name = "textBoxAPIKey";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Name = "label17";
            // 
            // comboBoxPhoneRegion
            // 
            this.comboBoxPhoneRegion.FormattingEnabled = true;
            this.comboBoxPhoneRegion.Items.AddRange(new object[] {
            resources.GetString("comboBoxPhoneRegion.Items"),
            resources.GetString("comboBoxPhoneRegion.Items1"),
            resources.GetString("comboBoxPhoneRegion.Items2"),
            resources.GetString("comboBoxPhoneRegion.Items3"),
            resources.GetString("comboBoxPhoneRegion.Items4"),
            resources.GetString("comboBoxPhoneRegion.Items5"),
            resources.GetString("comboBoxPhoneRegion.Items6"),
            resources.GetString("comboBoxPhoneRegion.Items7"),
            resources.GetString("comboBoxPhoneRegion.Items8"),
            resources.GetString("comboBoxPhoneRegion.Items9"),
            resources.GetString("comboBoxPhoneRegion.Items10"),
            resources.GetString("comboBoxPhoneRegion.Items11"),
            resources.GetString("comboBoxPhoneRegion.Items12"),
            resources.GetString("comboBoxPhoneRegion.Items13"),
            resources.GetString("comboBoxPhoneRegion.Items14"),
            resources.GetString("comboBoxPhoneRegion.Items15"),
            resources.GetString("comboBoxPhoneRegion.Items16"),
            resources.GetString("comboBoxPhoneRegion.Items17"),
            resources.GetString("comboBoxPhoneRegion.Items18"),
            resources.GetString("comboBoxPhoneRegion.Items19"),
            resources.GetString("comboBoxPhoneRegion.Items20"),
            resources.GetString("comboBoxPhoneRegion.Items21"),
            resources.GetString("comboBoxPhoneRegion.Items22"),
            resources.GetString("comboBoxPhoneRegion.Items23"),
            resources.GetString("comboBoxPhoneRegion.Items24"),
            resources.GetString("comboBoxPhoneRegion.Items25"),
            resources.GetString("comboBoxPhoneRegion.Items26"),
            resources.GetString("comboBoxPhoneRegion.Items27"),
            resources.GetString("comboBoxPhoneRegion.Items28"),
            resources.GetString("comboBoxPhoneRegion.Items29"),
            resources.GetString("comboBoxPhoneRegion.Items30"),
            resources.GetString("comboBoxPhoneRegion.Items31"),
            resources.GetString("comboBoxPhoneRegion.Items32"),
            resources.GetString("comboBoxPhoneRegion.Items33"),
            resources.GetString("comboBoxPhoneRegion.Items34"),
            resources.GetString("comboBoxPhoneRegion.Items35"),
            resources.GetString("comboBoxPhoneRegion.Items36"),
            resources.GetString("comboBoxPhoneRegion.Items37"),
            resources.GetString("comboBoxPhoneRegion.Items38"),
            resources.GetString("comboBoxPhoneRegion.Items39"),
            resources.GetString("comboBoxPhoneRegion.Items40"),
            resources.GetString("comboBoxPhoneRegion.Items41"),
            resources.GetString("comboBoxPhoneRegion.Items42"),
            resources.GetString("comboBoxPhoneRegion.Items43"),
            resources.GetString("comboBoxPhoneRegion.Items44"),
            resources.GetString("comboBoxPhoneRegion.Items45"),
            resources.GetString("comboBoxPhoneRegion.Items46"),
            resources.GetString("comboBoxPhoneRegion.Items47"),
            resources.GetString("comboBoxPhoneRegion.Items48"),
            resources.GetString("comboBoxPhoneRegion.Items49"),
            resources.GetString("comboBoxPhoneRegion.Items50"),
            resources.GetString("comboBoxPhoneRegion.Items51"),
            resources.GetString("comboBoxPhoneRegion.Items52"),
            resources.GetString("comboBoxPhoneRegion.Items53"),
            resources.GetString("comboBoxPhoneRegion.Items54"),
            resources.GetString("comboBoxPhoneRegion.Items55"),
            resources.GetString("comboBoxPhoneRegion.Items56"),
            resources.GetString("comboBoxPhoneRegion.Items57"),
            resources.GetString("comboBoxPhoneRegion.Items58"),
            resources.GetString("comboBoxPhoneRegion.Items59"),
            resources.GetString("comboBoxPhoneRegion.Items60"),
            resources.GetString("comboBoxPhoneRegion.Items61"),
            resources.GetString("comboBoxPhoneRegion.Items62"),
            resources.GetString("comboBoxPhoneRegion.Items63"),
            resources.GetString("comboBoxPhoneRegion.Items64"),
            resources.GetString("comboBoxPhoneRegion.Items65"),
            resources.GetString("comboBoxPhoneRegion.Items66"),
            resources.GetString("comboBoxPhoneRegion.Items67"),
            resources.GetString("comboBoxPhoneRegion.Items68"),
            resources.GetString("comboBoxPhoneRegion.Items69"),
            resources.GetString("comboBoxPhoneRegion.Items70"),
            resources.GetString("comboBoxPhoneRegion.Items71"),
            resources.GetString("comboBoxPhoneRegion.Items72"),
            resources.GetString("comboBoxPhoneRegion.Items73"),
            resources.GetString("comboBoxPhoneRegion.Items74"),
            resources.GetString("comboBoxPhoneRegion.Items75"),
            resources.GetString("comboBoxPhoneRegion.Items76"),
            resources.GetString("comboBoxPhoneRegion.Items77"),
            resources.GetString("comboBoxPhoneRegion.Items78"),
            resources.GetString("comboBoxPhoneRegion.Items79"),
            resources.GetString("comboBoxPhoneRegion.Items80"),
            resources.GetString("comboBoxPhoneRegion.Items81"),
            resources.GetString("comboBoxPhoneRegion.Items82"),
            resources.GetString("comboBoxPhoneRegion.Items83"),
            resources.GetString("comboBoxPhoneRegion.Items84"),
            resources.GetString("comboBoxPhoneRegion.Items85"),
            resources.GetString("comboBoxPhoneRegion.Items86"),
            resources.GetString("comboBoxPhoneRegion.Items87"),
            resources.GetString("comboBoxPhoneRegion.Items88"),
            resources.GetString("comboBoxPhoneRegion.Items89"),
            resources.GetString("comboBoxPhoneRegion.Items90"),
            resources.GetString("comboBoxPhoneRegion.Items91"),
            resources.GetString("comboBoxPhoneRegion.Items92"),
            resources.GetString("comboBoxPhoneRegion.Items93"),
            resources.GetString("comboBoxPhoneRegion.Items94"),
            resources.GetString("comboBoxPhoneRegion.Items95"),
            resources.GetString("comboBoxPhoneRegion.Items96"),
            resources.GetString("comboBoxPhoneRegion.Items97")});
            resources.ApplyResources(this.comboBoxPhoneRegion, "comboBoxPhoneRegion");
            this.comboBoxPhoneRegion.Name = "comboBoxPhoneRegion";
            // 
            // comboBoxService
            // 
            this.comboBoxService.FormattingEnabled = true;
            this.comboBoxService.Items.AddRange(new object[] {
            resources.GetString("comboBoxService.Items"),
            resources.GetString("comboBoxService.Items1"),
            resources.GetString("comboBoxService.Items2")});
            resources.ApplyResources(this.comboBoxService, "comboBoxService");
            this.comboBoxService.Name = "comboBoxService";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // textBoxApiKeyRuCaptcha
            // 
            resources.ApplyResources(this.textBoxApiKeyRuCaptcha, "textBoxApiKeyRuCaptcha");
            this.textBoxApiKeyRuCaptcha.Name = "textBoxApiKeyRuCaptcha";
            // 
            // textBoxDelay
            // 
            resources.ApplyResources(this.textBoxDelay, "textBoxDelay");
            this.textBoxDelay.Name = "textBoxDelay";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Name = "label13";
            // 
            // FormSMSAccountRecover
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxDelay);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBoxApiKeyRuCaptcha);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxService);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.comboBoxPhoneRegion);
            this.Controls.Add(this.textBoxAPIKey);
            this.Controls.Add(this.linkLabelSmsService);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.textBoxApiProxyUpdateInterval);
            this.Controls.Add(this.labelApiProxyUpdateInterval);
            this.Controls.Add(this.labelApiProxyLink);
            this.Controls.Add(this.checkBoxIsApiProxy);
            this.Controls.Add(this.textBoxAccounts);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxResultFile);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.buttonOpenResultFile);
            this.Controls.Add(this.textBoxUserAgents);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonOpenUserAgents);
            this.Controls.Add(this.labelGood);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.buttonSeeLog);
            this.Controls.Add(this.labelBad);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBoxTypeProxy);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.textBoxProxy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxThreadsCount);
            this.Controls.Add(this.buttonOpenProxy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonOpenAccs);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormSMSAccountRecover";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxProxy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxThreadsCount;
        private System.Windows.Forms.Button buttonOpenProxy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOpenAccs;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxTypeProxy;
        private System.Windows.Forms.Label labelBad;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonSeeLog;
        private System.Windows.Forms.Label labelGood;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxUserAgents;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonOpenUserAgents;
        private System.Windows.Forms.TextBox textBoxResultFile;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonOpenResultFile;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.TextBox textBoxAccounts;
        private System.Windows.Forms.TextBox textBoxApiProxyUpdateInterval;
        private System.Windows.Forms.Label labelApiProxyUpdateInterval;
        private System.Windows.Forms.Label labelApiProxyLink;
        private System.Windows.Forms.CheckBox checkBoxIsApiProxy;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.LinkLabel linkLabelSmsService;
        private System.Windows.Forms.TextBox textBoxAPIKey;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox comboBoxPhoneRegion;
        private System.Windows.Forms.ComboBox comboBoxService;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxApiKeyRuCaptcha;
        private System.Windows.Forms.TextBox textBoxDelay;
        private System.Windows.Forms.Label label13;
    }
}