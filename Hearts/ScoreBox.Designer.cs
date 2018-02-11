namespace Hearts
{
    partial class ScoreBox
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
            this.lblUserH = new System.Windows.Forms.Label();
            this.lblComp1H = new System.Windows.Forms.Label();
            this.lblComp2H = new System.Windows.Forms.Label();
            this.lblComp3H = new System.Windows.Forms.Label();
            this.lblUserScore = new System.Windows.Forms.ListBox();
            this.lblComp1 = new System.Windows.Forms.ListBox();
            this.lblComp2 = new System.Windows.Forms.ListBox();
            this.lblComp3 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblUserH
            // 
            this.lblUserH.AutoSize = true;
            this.lblUserH.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserH.Location = new System.Drawing.Point(31, 27);
            this.lblUserH.Name = "lblUserH";
            this.lblUserH.Size = new System.Drawing.Size(43, 15);
            this.lblUserH.TabIndex = 0;
            this.lblUserH.Text = "Player";
            // 
            // lblComp1H
            // 
            this.lblComp1H.AutoSize = true;
            this.lblComp1H.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComp1H.Location = new System.Drawing.Point(131, 27);
            this.lblComp1H.Name = "lblComp1H";
            this.lblComp1H.Size = new System.Drawing.Size(32, 15);
            this.lblComp1H.TabIndex = 1;
            this.lblComp1H.Text = "Tom";
            // 
            // lblComp2H
            // 
            this.lblComp2H.AutoSize = true;
            this.lblComp2H.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComp2H.Location = new System.Drawing.Point(252, 27);
            this.lblComp2H.Name = "lblComp2H";
            this.lblComp2H.Size = new System.Drawing.Size(32, 15);
            this.lblComp2H.TabIndex = 2;
            this.lblComp2H.Text = "Dick";
            // 
            // lblComp3H
            // 
            this.lblComp3H.AutoSize = true;
            this.lblComp3H.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComp3H.Location = new System.Drawing.Point(370, 27);
            this.lblComp3H.Name = "lblComp3H";
            this.lblComp3H.Size = new System.Drawing.Size(38, 15);
            this.lblComp3H.TabIndex = 3;
            this.lblComp3H.Text = "Harry";
            // 
            // lblUserScore
            // 
            this.lblUserScore.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblUserScore.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblUserScore.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lblUserScore.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserScore.ItemHeight = 16;
            this.lblUserScore.Location = new System.Drawing.Point(34, 51);
            this.lblUserScore.Name = "lblUserScore";
            this.lblUserScore.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lblUserScore.Size = new System.Drawing.Size(34, 208);
            this.lblUserScore.TabIndex = 4;
            this.lblUserScore.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lblUserScore_DrawItem);
            // 
            // lblComp1
            // 
            this.lblComp1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblComp1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblComp1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lblComp1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComp1.ItemHeight = 16;
            this.lblComp1.Location = new System.Drawing.Point(129, 51);
            this.lblComp1.Name = "lblComp1";
            this.lblComp1.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lblComp1.Size = new System.Drawing.Size(34, 208);
            this.lblComp1.TabIndex = 5;
            this.lblComp1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lblComp1_DrawItem);
            // 
            // lblComp2
            // 
            this.lblComp2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblComp2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblComp2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lblComp2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComp2.ItemHeight = 16;
            this.lblComp2.Location = new System.Drawing.Point(250, 51);
            this.lblComp2.Name = "lblComp2";
            this.lblComp2.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lblComp2.Size = new System.Drawing.Size(34, 208);
            this.lblComp2.TabIndex = 6;
            this.lblComp2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lblComp2_DrawItem);
            // 
            // lblComp3
            // 
            this.lblComp3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblComp3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblComp3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lblComp3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComp3.ItemHeight = 16;
            this.lblComp3.Location = new System.Drawing.Point(373, 51);
            this.lblComp3.Name = "lblComp3";
            this.lblComp3.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lblComp3.Size = new System.Drawing.Size(34, 208);
            this.lblComp3.TabIndex = 7;
            this.lblComp3.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lblComp3_DrawItem);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(373, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Deal";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ScoreBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(460, 304);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblComp3);
            this.Controls.Add(this.lblComp2);
            this.Controls.Add(this.lblComp1);
            this.Controls.Add(this.lblUserScore);
            this.Controls.Add(this.lblComp3H);
            this.Controls.Add(this.lblComp2H);
            this.Controls.Add(this.lblComp1H);
            this.Controls.Add(this.lblUserH);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ScoreBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Scores";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUserH;
        private System.Windows.Forms.Label lblComp1H;
        private System.Windows.Forms.Label lblComp2H;
        private System.Windows.Forms.Label lblComp3H;
        private System.Windows.Forms.ListBox lblUserScore;
        private System.Windows.Forms.ListBox lblComp1;
        private System.Windows.Forms.ListBox lblComp2;
        private System.Windows.Forms.ListBox lblComp3;
        private System.Windows.Forms.Button button1;
    }
}