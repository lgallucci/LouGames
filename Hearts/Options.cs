using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GameOptions;

namespace Hearts
{
    internal partial class Options : Form
    {
        internal string player, comp1, comp2, comp3;

        /* Constructor */
        internal Options()
        {
            InitializeComponent();
        }

        /* Show Options Method */
        internal DialogResult ShowForm(bool onlyPlayer, string playerName)
        {
            this.CenterToScreen();
            LoadNames();

            txtPlayer.Text = playerName;
            txtComp1.Text = comp1;
            txtComp2.Text = comp2;
            txtComp3.Text = comp3;

            if (onlyPlayer)
            {
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                txtComp1.Visible = false;
                txtComp2.Visible = false;
                txtComp3.Visible = false;
                this.Height = 100;
                btnOk.Location = new Point(15, 41);
                this.Text = "Change Name";
            }
            else
            {
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                txtComp1.Visible = true;
                txtComp2.Visible = true;
                txtComp3.Visible = true;
                this.Height = 182;
                btnOk.Location = new Point(15, 116);
                this.Text = "Change Names";
            }

            if (this.ShowDialog() == DialogResult.OK)
            {
                player = txtPlayer.Text;
                comp1 = txtComp1.Text;
                comp2 = txtComp2.Text;
                comp3 = txtComp3.Text;
                SaveNames();
                return DialogResult.OK;
            }
            return DialogResult.Cancel;
        }

        internal void LoadNames()
        {
            FileProcessing.LoadHearts(ref player, ref comp1, ref comp2, ref comp3);
        }

        internal void SaveNames()
        {
            FileProcessing.SaveHearts(comp1, comp2, comp3);
        }
    }
}