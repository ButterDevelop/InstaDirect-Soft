namespace InstaDirectMessage_ButDev
{
    partial class FormBreakdownDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBreakdownDB));
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBoxSourceFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOpenOldUA = new System.Windows.Forms.Button();
            this.textBoxResultFile = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonOpenResultFile = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.labelIs = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDivide = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
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
            // textBoxSourceFile
            // 
            resources.ApplyResources(this.textBoxSourceFile, "textBoxSourceFile");
            this.textBoxSourceFile.Name = "textBoxSourceFile";
            this.textBoxSourceFile.ReadOnly = true;
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
            // labelIs
            // 
            resources.ApplyResources(this.labelIs, "labelIs");
            this.labelIs.BackColor = System.Drawing.Color.Transparent;
            this.labelIs.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.labelIs.Name = "labelIs";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // textBoxDivide
            // 
            resources.ApplyResources(this.textBoxDivide, "textBoxDivide");
            this.textBoxDivide.Name = "textBoxDivide";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // FormBreakdownDB
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxDivide);
            this.Controls.Add(this.labelIs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.textBoxResultFile);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.buttonOpenResultFile);
            this.Controls.Add(this.textBoxSourceFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOpenOldUA);
            this.Controls.Add(this.buttonStart);
            this.MaximizeBox = false;
            this.Name = "FormBreakdownDB";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textBoxSourceFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOpenOldUA;
        private System.Windows.Forms.TextBox textBoxResultFile;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonOpenResultFile;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label labelIs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDivide;
        private System.Windows.Forms.Label label2;
    }
}