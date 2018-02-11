using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper
{
    internal partial class HighScores : Form
    {
        internal string[] names;
        internal int[] times;

        internal HighScores()
        {
            InitializeComponent();
            names = new string[3];
            times = new int[3];
        }

        internal void HighScores_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        internal void showForm(IWin32Window win, bool showReset)
        {
            lblBeginner.Text = names[0] + " - " + times[0] + " seconds.";
            lblIntermediate.Text = names[1] + " - " + times[1] + " seconds.";
            lblExpert.Text = names[2] + " - " + times[2] + " seconds.";

            if (showReset)
            {
                btnReset.Visible = true;
            }
            else
            {
                btnReset.Visible = false;
            }

            this.ShowDialog(win);
        }

        internal void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the scores?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                names[0] = "Anonymous"; names[1] = "Anonymous"; names[2] = "Anonymous";
                times[0] = 9999; times[1] = 9999; times[2] = 9999;

                lblBeginner.Text = names[0] + " - " + times[0] + " seconds.";
                lblIntermediate.Text = names[1] + " - " + times[1] + " seconds.";
                lblExpert.Text = names[2] + " - " + times[2] + " seconds.";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}