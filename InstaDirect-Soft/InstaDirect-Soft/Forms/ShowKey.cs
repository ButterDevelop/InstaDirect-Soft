using InstaDirectMessage_ButDev.Tools;
using System;
using System.Windows.Forms;

namespace InstaDirectMessage_ButDev
{
    public partial class ShowKey : Form
    {
        public ShowKey()
        {
            InitializeComponent();
            FormClosing += ShowKey_FormClosing;
            textBoxID.Text = ID.IDNumber;
            textBoxNewID.Text = ID.NewIDNumber;
        }

        private void ShowKey_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
