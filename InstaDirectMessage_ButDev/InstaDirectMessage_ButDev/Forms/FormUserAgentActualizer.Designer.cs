namespace InstaDirectMessage_ButDev
{
    partial class FormUserAgentActualizer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserAgentActualizer));
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelGood = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxOldUA = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOpenOldUA = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxHowManyAsResult = new System.Windows.Forms.TextBox();
            this.textBoxResultFile = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonOpenResultFile = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            resources.ApplyResources(this.buttonStart, "buttonStart");
            this.buttonStart.BackColor = System.Drawing.Color.Transparent;
            this.buttonStart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(221)))), ((int)(((byte)(253)))));
            this.buttonStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(221)))), ((int)(((byte)(253)))));
            this.buttonStart.ForeColor = System.Drawing.Color.Black;
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
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
            // textBoxOldUA
            // 
            resources.ApplyResources(this.textBoxOldUA, "textBoxOldUA");
            this.textBoxOldUA.Name = "textBoxOldUA";
            this.textBoxOldUA.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // buttonOpenOldUA
            // 
            resources.ApplyResources(this.buttonOpenOldUA, "buttonOpenOldUA");
            this.buttonOpenOldUA.Name = "buttonOpenOldUA";
            this.buttonOpenOldUA.UseVisualStyleBackColor = true;
            this.buttonOpenOldUA.Click += new System.EventHandler(this.buttonOpenOldUA_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // textBoxHowManyAsResult
            // 
            resources.ApplyResources(this.textBoxHowManyAsResult, "textBoxHowManyAsResult");
            this.textBoxHowManyAsResult.Name = "textBoxHowManyAsResult";
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
            // buttonStop
            // 
            resources.ApplyResources(this.buttonStop, "buttonStop");
            this.buttonStop.BackColor = System.Drawing.Color.Transparent;
            this.buttonStop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(221)))), ((int)(((byte)(253)))));
            this.buttonStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(221)))), ((int)(((byte)(253)))));
            this.buttonStop.ForeColor = System.Drawing.Color.Black;
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.UseVisualStyleBackColor = false;
            // 
            // FormUserAgentActualizer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.textBoxResultFile);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.buttonOpenResultFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxHowManyAsResult);
            this.Controls.Add(this.textBoxOldUA);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOpenOldUA);
            this.Controls.Add(this.labelGood);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.buttonStart);
            this.MaximizeBox = false;
            this.Name = "FormUserAgentActualizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelGood;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxOldUA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOpenOldUA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxHowManyAsResult;
        private System.Windows.Forms.TextBox textBoxResultFile;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonOpenResultFile;
        private System.Windows.Forms.Button buttonStop;
    }
}