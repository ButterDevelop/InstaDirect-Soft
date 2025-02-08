namespace InstaDirectMessage_ButDev
{
    partial class FormStatsChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStatsChart));
            this.buttonRegistrarStats = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.panelBrowser = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // buttonRegistrarStats
            // 
            this.buttonRegistrarStats.BackColor = System.Drawing.Color.Transparent;
            this.buttonRegistrarStats.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonRegistrarStats.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            resources.ApplyResources(this.buttonRegistrarStats, "buttonRegistrarStats");
            this.buttonRegistrarStats.Name = "buttonRegistrarStats";
            this.buttonRegistrarStats.UseVisualStyleBackColor = false;
            this.buttonRegistrarStats.Click += new System.EventHandler(this.buttonRegistrarStats_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.BackColor = System.Drawing.Color.Transparent;
            this.buttonRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            resources.ApplyResources(this.buttonRefresh, "buttonRefresh");
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.UseVisualStyleBackColor = false;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // panelBrowser
            // 
            this.panelBrowser.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.panelBrowser, "panelBrowser");
            this.panelBrowser.Name = "panelBrowser";
            // 
            // FormStatsChart
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBrowser);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.buttonRegistrarStats);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormStatsChart";
            this.ShowIcon = false;
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonRegistrarStats;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Panel panelBrowser;
    }
}