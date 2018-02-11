using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Hearts
{
    internal partial class ScoreBox : Form
    {
        List<int> maxIndexes, minIndexes;
        int min, max;

        /* Constructor */
        internal ScoreBox()
        {
            InitializeComponent();

            minIndexes = new List<int>();
            maxIndexes = new List<int>();

            this.CenterToScreen();
            button1.Top = this.Height - 75;
        }

        /* Form Display Info Methods */
        internal void SetNames(string name1, string name2, string name3, string name4, bool endOfRound)
        {

            lblUserH.Text = name1; lblComp1H.Text = name2;
            lblComp2H.Text = name3; lblComp3H.Text = name4;

            if (!endOfRound)
            {
                button1.Text = "Ok";
            }
        }

        internal void SetScores(ReadOnlyCollection<int> score1, ReadOnlyCollection<int> score2, ReadOnlyCollection<int> score3,
                              ReadOnlyCollection<int> score4, string name1, string name2, string name3, string name4, bool endOfRound)
        {
            button1.Text = "Deal";
            maxIndexes.Clear();
            minIndexes.Clear();

            lblUserH.Text = name1; lblComp1H.Text = name2;
            lblComp2H.Text = name3; lblComp3H.Text = name4;

            List<int> temp = new List<int>();

            lblUserScore.Items.Clear();
            lblComp1.Items.Clear();
            lblComp2.Items.Clear();
            lblComp3.Items.Clear();


            lblUserH.ForeColor = Color.Black;
            lblComp1H.ForeColor = Color.Black;
            lblComp2H.ForeColor = Color.Black;
            lblComp3H.ForeColor = Color.Black;

            temp.Add(score1[score1.Count - 1]);
            temp.Add(score2[score2.Count - 1]);
            temp.Add(score3[score3.Count - 1]);
            temp.Add(score4[score4.Count - 1]);

            min = temp[0];
            max = temp[0];

            for (int i = 0; i < 4; i++)
            {
                if (temp[i] < min)
                {
                    min = temp[i];
                    minIndexes.Clear();
                    minIndexes.Add(i);
                }
                else if (temp[i] == min)
                {
                    minIndexes.Add(i);
                }

                if (temp[i] > max)
                {
                    max = temp[i];
                    maxIndexes.Clear();
                    maxIndexes.Add(i);
                }
                else if (temp[i] == max)
                {
                    maxIndexes.Add(i);
                }
            }

            if (minIndexes.Contains(0))
                lblUserH.ForeColor = Color.Green;
            if (minIndexes.Contains(1))
                lblComp1H.ForeColor = Color.Green;
            if (minIndexes.Contains(2))
                lblComp2H.ForeColor = Color.Green;
            if (minIndexes.Contains(3))
                lblComp3H.ForeColor = Color.Green;

            if (max >= 100)
            {
                if (maxIndexes.Contains(0))
                    lblUserH.ForeColor = Color.Red;
                if (maxIndexes.Contains(1))
                    lblComp1H.ForeColor = Color.Red;
                if (maxIndexes.Contains(2))
                    lblComp2H.ForeColor = Color.Red;
                if (maxIndexes.Contains(3))
                    lblComp3H.ForeColor = Color.Red;
            }

            if (max >= 100)
            {
                button1.Text = "New Game";
            }
            for (int i = 0; i < score1.Count; i++)
            {
                if (i < score1.Count - 1)
                {
                    lblUserScore.Items.Add(score1[i].ToString(CultureInfo.CurrentCulture));
                    lblComp1.Items.Add(score2[i].ToString(CultureInfo.CurrentCulture));
                    lblComp2.Items.Add(score3[i].ToString(CultureInfo.CurrentCulture));
                    lblComp3.Items.Add(score4[i].ToString(CultureInfo.CurrentCulture));

                }
                else
                {
                    lblUserScore.Items.Add(score1[i].ToString(CultureInfo.CurrentCulture));
                    lblComp1.Items.Add(score2[i].ToString(CultureInfo.CurrentCulture));
                    lblComp2.Items.Add(score3[i].ToString(CultureInfo.CurrentCulture));
                    lblComp3.Items.Add(score4[i].ToString(CultureInfo.CurrentCulture));
                }
            }
            this.Height = lblUserScore.Items.Count * 15 + 200;
            button1.Top = this.Height - 75;
            this.CenterToScreen();

            if (!endOfRound)
                button1.Text = "Ok";
        }

        /* Score Drawing Methods */
        private void lblUserScore_DrawItem(object sender, DrawItemEventArgs e)
        {
            string str = lblUserScore.Items[e.Index].ToString();

            Rectangle rc = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            Brush brush;
            Font font;

            if (minIndexes.Contains(0))
                brush = new SolidBrush(Color.Green);
            else if (maxIndexes.Contains(0) && max >= 100)
                brush = new SolidBrush(Color.Red);
            else
                brush = new SolidBrush(Color.Black);

            if (e.Index != lblUserScore.Items.Count - 1)
                font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Strikeout | FontStyle.Bold);
            else
                font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular | FontStyle.Bold);

            e.Graphics.DrawString(str, font, brush, rc, sf);
        }

        private void lblComp1_DrawItem(object sender, DrawItemEventArgs e)
        {
            string str = lblComp1.Items[e.Index].ToString();

            Rectangle rc = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            Brush brush;
            Font font;

            if (minIndexes.Contains(1))
                brush = new SolidBrush(Color.Green);
            else if (maxIndexes.Contains(1) && max >= 100)
                brush = new SolidBrush(Color.Red);
            else
                brush = new SolidBrush(Color.Black);

            if (e.Index != lblUserScore.Items.Count - 1)
                font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Strikeout | FontStyle.Bold);
            else
                font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular | FontStyle.Bold);

            e.Graphics.DrawString(str, font, brush, rc, sf);
        }

        private void lblComp2_DrawItem(object sender, DrawItemEventArgs e)
        {
            string str = lblComp2.Items[e.Index].ToString();

            Rectangle rc = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            Brush brush;
            Font font;

            if (minIndexes.Contains(2))
                brush = new SolidBrush(Color.Green);
            else if (maxIndexes.Contains(2) && max >= 100)
                brush = new SolidBrush(Color.Red);
            else
                brush = new SolidBrush(Color.Black);

            if (e.Index != lblUserScore.Items.Count - 1)
                font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Strikeout | FontStyle.Bold);
            else
                font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular | FontStyle.Bold);

            e.Graphics.DrawString(str, font, brush, rc, sf);
        }

        private void lblComp3_DrawItem(object sender, DrawItemEventArgs e)
        {
            string str = lblComp3.Items[e.Index].ToString();

            Rectangle rc = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            Brush brush;
            Font font;

            if (minIndexes.Contains(3))
                brush = new SolidBrush(Color.Green);
            else if (maxIndexes.Contains(3) && max >= 100)
                brush = new SolidBrush(Color.Red);
            else
                brush = new SolidBrush(Color.Black);

            if (e.Index != lblUserScore.Items.Count - 1)
                font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Strikeout | FontStyle.Bold);
            else
                font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular | FontStyle.Bold);

            e.Graphics.DrawString(str, font, brush, rc, sf);
        }

        internal void ShowForm(IWin32Window engine)
        {
            this.CenterToScreen();
            this.ShowDialog(engine);
        }
    }
}