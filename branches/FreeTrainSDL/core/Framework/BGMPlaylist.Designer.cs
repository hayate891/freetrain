#region LICENSE
/*
 * Copyright (C) 2007 - 2008 FreeTrain Team (http://freetrain.sourceforge.net)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

namespace FreeTrain.Framework
{
    partial class BGMPlaylist
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
            this.lstBGMs = new System.Windows.Forms.ListBox();
            this.lstCurrentPlaylist = new System.Windows.Forms.ListBox();
            this.cmdUp = new System.Windows.Forms.Button();
            this.cmdDown = new System.Windows.Forms.Button();
            this.cmdPlay = new System.Windows.Forms.Button();
            this.cmdNext = new System.Windows.Forms.Button();
            this.cmdPrev = new System.Windows.Forms.Button();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstBGMs
            // 
            this.lstBGMs.FormattingEnabled = true;
            this.lstBGMs.Location = new System.Drawing.Point(283, 25);
            this.lstBGMs.Name = "lstBGMs";
            this.lstBGMs.Size = new System.Drawing.Size(227, 134);
            this.lstBGMs.TabIndex = 0;
            this.lstBGMs.DoubleClick += new System.EventHandler(this.lstBGMs_DoubleClick);
            // 
            // lstCurrentPlaylist
            // 
            this.lstCurrentPlaylist.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lstCurrentPlaylist.FormattingEnabled = true;
            this.lstCurrentPlaylist.Location = new System.Drawing.Point(12, 25);
            this.lstCurrentPlaylist.Name = "lstCurrentPlaylist";
            this.lstCurrentPlaylist.Size = new System.Drawing.Size(228, 134);
            this.lstCurrentPlaylist.TabIndex = 1;
            this.lstCurrentPlaylist.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstCurrentPlaylist_DrawItem);
            this.lstCurrentPlaylist.DoubleClick += new System.EventHandler(this.lstCurrentPlaylist_DoubleClick);
            this.lstCurrentPlaylist.SelectedIndexChanged += new System.EventHandler(this.lstCurrentPlaylist_SelectedIndexChanged);
            // 
            // cmdUp
            // 
            this.cmdUp.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.cmdUp.Location = new System.Drawing.Point(246, 36);
            this.cmdUp.Name = "cmdUp";
            this.cmdUp.Size = new System.Drawing.Size(31, 23);
            this.cmdUp.TabIndex = 2;
            this.cmdUp.Text = "á";
            this.cmdUp.UseVisualStyleBackColor = true;
            this.cmdUp.Click += new System.EventHandler(this.cmdUp_Click);
            // 
            // cmdDown
            // 
            this.cmdDown.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.cmdDown.Location = new System.Drawing.Point(246, 123);
            this.cmdDown.Name = "cmdDown";
            this.cmdDown.Size = new System.Drawing.Size(31, 23);
            this.cmdDown.TabIndex = 3;
            this.cmdDown.Text = "â";
            this.cmdDown.UseVisualStyleBackColor = true;
            this.cmdDown.Click += new System.EventHandler(this.cmdDown_Click);
            // 
            // cmdPlay
            // 
            this.cmdPlay.Location = new System.Drawing.Point(99, 165);
            this.cmdPlay.Name = "cmdPlay";
            this.cmdPlay.Size = new System.Drawing.Size(48, 29);
            this.cmdPlay.TabIndex = 4;
            this.cmdPlay.Text = "Play";
            this.cmdPlay.UseVisualStyleBackColor = true;
            this.cmdPlay.Click += new System.EventHandler(this.cmdPlayPause_Click);
            // 
            // cmdNext
            // 
            this.cmdNext.Font = new System.Drawing.Font("Wingdings 3", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.cmdNext.Location = new System.Drawing.Point(179, 165);
            this.cmdNext.Name = "cmdNext";
            this.cmdNext.Size = new System.Drawing.Size(61, 29);
            this.cmdNext.TabIndex = 5;
            this.cmdNext.Text = "*";
            this.cmdNext.UseVisualStyleBackColor = true;
            this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
            // 
            // cmdPrev
            // 
            this.cmdPrev.Font = new System.Drawing.Font("Wingdings 3", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.cmdPrev.Location = new System.Drawing.Point(12, 165);
            this.cmdPrev.Margin = new System.Windows.Forms.Padding(0);
            this.cmdPrev.Name = "cmdPrev";
            this.cmdPrev.Size = new System.Drawing.Size(61, 29);
            this.cmdPrev.TabIndex = 6;
            this.cmdPrev.Text = ")";
            this.cmdPrev.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cmdPrev.UseVisualStyleBackColor = true;
            this.cmdPrev.Click += new System.EventHandler(this.cmdPrev_Click);
            // 
            // cmdAdd
            // 
            this.cmdAdd.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.cmdAdd.Location = new System.Drawing.Point(246, 65);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(31, 23);
            this.cmdAdd.TabIndex = 7;
            this.cmdAdd.Text = "ß";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.cmdDelete.Location = new System.Drawing.Point(246, 94);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(31, 23);
            this.cmdDelete.TabIndex = 8;
            this.cmdDelete.Text = "à";
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Location = new System.Drawing.Point(9, 9);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(79, 13);
            this.lblCurrent.TabIndex = 9;
            this.lblCurrent.Text = "Current Playlist:";
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Location = new System.Drawing.Point(280, 9);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(89, 13);
            this.lblAvailable.TabIndex = 10;
            this.lblAvailable.Text = "Available Tracks:";
            // 
            // BGMPlaylist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 201);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.cmdPrev);
            this.Controls.Add(this.cmdNext);
            this.Controls.Add(this.cmdPlay);
            this.Controls.Add(this.cmdDown);
            this.Controls.Add(this.cmdUp);
            this.Controls.Add(this.lstCurrentPlaylist);
            this.Controls.Add(this.lstBGMs);
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.lblCurrent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "BGMPlaylist";
            this.Text = "BGMPlaylist";
            this.Load += new System.EventHandler(this.BGMPlaylist_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstBGMs;
        private System.Windows.Forms.ListBox lstCurrentPlaylist;
        private System.Windows.Forms.Button cmdUp;
        private System.Windows.Forms.Button cmdDown;
        private System.Windows.Forms.Button cmdPlay;
        private System.Windows.Forms.Button cmdNext;
        private System.Windows.Forms.Button cmdPrev;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.Label lblAvailable;
    }
}