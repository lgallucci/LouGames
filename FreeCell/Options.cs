using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using GameOptions;

namespace FreeCell
{
    internal partial class Options : Form
    {
        /* Properties */
        private bool allowRightClick;

        internal bool AllowRightClick
        {
            get { return allowRightClick; }
        }

        private int gamesPlayed;

        internal int GamesPlayed
        {
            get { return gamesPlayed; }
            set { gamesPlayed = value; }
        }

        private int gamesWon;

        internal int GamesWon
        {
            get { return gamesWon; }
            set { gamesWon = value; }
        }

        private int gamesLost;

        internal int GamesLost
        {
            get { return gamesLost; }
            set { gamesLost = value; }
        }

        private int gamesForfiet;

        internal int GamesForfiet
        {
            get { return gamesForfiet; }
            set { gamesForfiet = value; }
        }
	
        /* Constructor */
        internal Options()
        {
            InitializeComponent();

            LoadOptions();

            cbxRightClick.Checked = allowRightClick;
        }

        /* Form Methods */
        private void btnCancel_Click(object sender, EventArgs e)
        {
            cbxRightClick.Checked = allowRightClick;
            this.Hide();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            allowRightClick = cbxRightClick.Checked;
            
            this.Hide();
        }

        internal void SaveOptions()
        {
            FileProcessing.SaveFreeCell(allowRightClick, gamesPlayed, gamesWon, gamesForfiet, gamesLost);
        }

        internal void LoadOptions()
        {
            FileProcessing.LoadFreeCell(ref allowRightClick, ref  gamesPlayed, ref  gamesWon, ref  gamesForfiet, ref  gamesLost);
        }

        internal void ShowForm(IWin32Window parent)
        {
            lblForfiet.Text = "Games Forfeit: " + gamesForfiet;
            lblLost.Text =    "Games Lost:    " + (gamesLost + gamesForfiet);
            lblWon.Text =     "Games Won:    " + gamesWon;
            lblPlayed.Text =  "Games Played: " + gamesPlayed;
            if ((gamesLost + gamesWon + gamesForfiet) > 0)
                lblPercentage.Text = "Win Percentage: " + Convert.ToInt32(((double)gamesWon / (double)(gamesLost + gamesWon + gamesForfiet)) * 100) + "%";
            else
                lblPercentage.Text = "Win Percentage: 0%";
                
            this.ShowDialog(parent);
        }
    }
}