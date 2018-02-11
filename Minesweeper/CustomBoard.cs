using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Minesweeper
{
    internal partial class CustomBoard : Form
    {
        /* Properties */
        private int bWidth = 9, bHeight = 9, bMines = 10;

        internal int BWidth
        {
            get { return bWidth; }
        }

        internal int BHeight
        {
            get { return bHeight; }
        }

        internal int BMines
        {
            get { return bMines; }
        }

        /* Constructor */
        internal CustomBoard(int width, int height, int mines)
        {
            InitializeComponent();
            bWidth = width;
            bHeight = height;
            bMines = mines;
        }

        /* Form Event Methods */
        private void CustomBoard_Load(object sender, EventArgs e)
        {
            txtHeight.Text = bHeight.ToString(CultureInfo.CurrentCulture);
            txtWidth.Text = bWidth.ToString(CultureInfo.CurrentCulture);
            txtMines.Text = bMines.ToString(CultureInfo.CurrentCulture);
            this.CenterToParent();
        }

        private void txtWidth_Leave(object sender, EventArgs e)
        {
            int result;
            bool temp = Int32.TryParse(txtWidth.Text, out result);

            if (!temp)
                return;

            if (result == 0)
                btnOK.Enabled = false;
            else
            {
                btnOK.Enabled = true;
                bWidth = result;
            }
        }

        private void txtHeight_Leave(object sender, EventArgs e)
        {
            int result;
            bool temp = Int32.TryParse(txtWidth.Text, out result);

            if (!temp)
                return;

            if (result == 0)
                btnOK.Enabled = false;
            else
            {
                btnOK.Enabled = true;
                bHeight = result;
            }
        }

        private void txtMines_Leave(object sender, EventArgs e)
        {
            int result;
            bool temp = Int32.TryParse(txtMines.Text, out result);

            if (!temp)
                return;

            if (result == 0)
                btnOK.Enabled = false;
            else
            {
                btnOK.Enabled = true;
                bMines = result;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (bWidth < 9)
                bWidth = 9;
            else if (bWidth > 30)
                bWidth = 30;
            else if (bWidth > (bHeight * 2))
                bWidth = (bHeight * 2) - 1;

            if (bHeight < 9)
                bHeight = 9;
            else if (bHeight > 30)
                bHeight = 30;
            else if (bHeight > bWidth)
                bHeight = bWidth;

            int maxMines = (int)(bHeight * bWidth / 4.84);

            if (bMines < 10)
                bMines = 10;
            else if (bMines > maxMines)
                bMines = maxMines;

            this.Close();
        }
    }
}