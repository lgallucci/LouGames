namespace FreeCell
{
    partial class Options
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
            this.cbxRightClick = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblPercentage = new System.Windows.Forms.Label();
            this.lblLost = new System.Windows.Forms.Label();
            this.lblForfiet = new System.Windows.Forms.Label();
            this.lblWon = new System.Windows.Forms.Label();
            this.lblPlayed = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxRightClick
            // 
            this.cbxRightClick.AutoSize = true;
            this.cbxRightClick.Location = new System.Drawing.Point(77, 116);
            this.cbxRightClick.Name = "cbxRightClick";
            this.cbxRightClick.Size = new System.Drawing.Size(105, 17);
            this.cbxRightClick.TabIndex = 0;
            this.cbxRightClick.Text = "Allow Right Click";
            this.cbxRightClick.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(19, 139);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(162, 139);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblPercentage);
            this.groupBox1.Controls.Add(this.lblLost);
            this.groupBox1.Controls.Add(this.lblForfiet);
            this.groupBox1.Controls.Add(this.lblWon);
            this.groupBox1.Controls.Add(this.lblPlayed);
            this.groupBox1.Location = new System.Drawing.Point(19, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 98);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Statistics";
            // 
            // lblPercentage
            // 
            this.lblPercentage.AutoSize = true;
            this.lblPercentage.Location = new System.Drawing.Point(55, 72);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(72, 13);
            this.lblPercentage.TabIndex = 4;
            this.lblPercentage.Text = "lblPercentage";
            // 
            // lblLost
            // 
            this.lblLost.AutoSize = true;
            this.lblLost.Location = new System.Drawing.Point(106, 20);
            this.lblLost.Name = "lblLost";
            this.lblLost.Size = new System.Drawing.Size(37, 13);
            this.lblLost.TabIndex = 3;
            this.lblLost.Text = "lblLost";
            // 
            // lblForfiet
            // 
            this.lblForfiet.AutoSize = true;
            this.lblForfiet.Location = new System.Drawing.Point(106, 44);
            this.lblForfiet.Name = "lblForfiet";
            this.lblForfiet.Size = new System.Drawing.Size(46, 13);
            this.lblForfiet.TabIndex = 2;
            this.lblForfiet.Text = "lblForfiet";
            // 
            // lblWon
            // 
            this.lblWon.AutoSize = true;
            this.lblWon.Location = new System.Drawing.Point(6, 20);
            this.lblWon.Name = "lblWon";
            this.lblWon.Size = new System.Drawing.Size(40, 13);
            this.lblWon.TabIndex = 1;
            this.lblWon.Text = "lblWon";
            // 
            // lblPlayed
            // 
            this.lblPlayed.AutoSize = true;
            this.lblPlayed.Location = new System.Drawing.Point(6, 44);
            this.lblPlayed.Name = "lblPlayed";
            this.lblPlayed.Size = new System.Drawing.Size(49, 13);
            this.lblPlayed.TabIndex = 0;
            this.lblPlayed.Text = "lblPlayed";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 205);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cbxRightClick);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbxRightClick;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblLost;
        private System.Windows.Forms.Label lblForfiet;
        private System.Windows.Forms.Label lblWon;
        private System.Windows.Forms.Label lblPlayed;
        private System.Windows.Forms.Label lblPercentage;
    }
}