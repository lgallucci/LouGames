using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper
{
    internal partial class UserName : Form
    {
        internal string userName = "Anonymous";
        internal UserName()
        {
            InitializeComponent();
        }

        internal DialogResult ShowForm()
        {
            txtUserName.Text = userName;
            txtUserName.SelectAll();

            return this.ShowDialog();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
                userName = "Anonymous";
            else
                userName = txtUserName.Text;
        }

        private void UserName_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }
    }
}