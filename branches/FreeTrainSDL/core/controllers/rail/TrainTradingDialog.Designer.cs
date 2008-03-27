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

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FreeTrain.Contributions.Train;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Views;
using FreeTrain.World.Accounting;

namespace FreeTrain.World.Rail
{
    /// <summary>
    /// Dialog box to buy trains
    /// </summary>
    public partial class TrainTradingDialog : Form
    {
        #region Windows Form Designer generated code

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown length;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label speed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label totalPrice;
        private System.Windows.Forms.NumericUpDown count;
        private System.Windows.Forms.Label passenger;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TreeView typeTree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label author;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.PictureBox preview;
        private System.Windows.Forms.ToolBarButton tbDay;
        private System.Windows.Forms.ToolBarButton tbNight;
        private System.Windows.Forms.ImageList buttonImages;
        private System.Windows.Forms.ToolBar toolBarDayNight;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.length = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.count = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.speed = new System.Windows.Forms.Label();
            this.totalPrice = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.passenger = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.typeTree = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.TextBox();
            this.author = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.Label();
            this.preview = new System.Windows.Forms.PictureBox();
            this.buttonImages = new System.Windows.Forms.ImageList(this.components);
            this.toolBarDayNight = new System.Windows.Forms.ToolBar();
            this.tbDay = new System.Windows.Forms.ToolBarButton();
            this.tbNight = new System.Windows.Forms.ToolBarButton();
            ((System.ComponentModel.ISupportInitialize)(this.length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.count)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.preview)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(263, 266);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Length:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // length
            // 
            this.length.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.length.Location = new System.Drawing.Point(331, 267);
            this.length.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.length.Name = "length";
            this.length.Size = new System.Drawing.Size(64, 20);
            this.length.TabIndex = 4;
            this.length.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.length.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.length.ValueChanged += new System.EventHandler(this.onAmountChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(436, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "x";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // count
            // 
            this.count.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.count.Location = new System.Drawing.Point(457, 267);
            this.count.Name = "count";
            this.count.Size = new System.Drawing.Size(64, 20);
            this.count.TabIndex = 6;
            this.count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.count.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.count.ValueChanged += new System.EventHandler(this.onAmountChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(150, 297);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 4);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(526, 266);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "set(s)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(392, 348);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(80, 26);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "&Buy";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.Location = new System.Drawing.Point(478, 348);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 26);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "&Close";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(263, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Speed:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // speed
            // 
            this.speed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.speed.Location = new System.Drawing.Point(333, 240);
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(220, 17);
            this.speed.TabIndex = 11;
            this.speed.Text = "Rapid";
            this.speed.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // totalPrice
            // 
            this.totalPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.totalPrice.Location = new System.Drawing.Point(331, 304);
            this.totalPrice.Name = "totalPrice";
            this.totalPrice.Size = new System.Drawing.Size(227, 18);
            this.totalPrice.TabIndex = 14;
            this.totalPrice.Text = "100,000";
            this.totalPrice.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(263, 304);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 18);
            this.label8.TabIndex = 15;
            this.label8.Text = "Total cost:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // passenger
            // 
            this.passenger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.passenger.Location = new System.Drawing.Point(331, 322);
            this.passenger.Name = "passenger";
            this.passenger.Size = new System.Drawing.Size(222, 18);
            this.passenger.TabIndex = 17;
            this.passenger.Text = "100";
            this.passenger.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(263, 322);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 18);
            this.label9.TabIndex = 16;
            this.label9.Text = "Capacity:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // typeTree
            // 
            this.typeTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.typeTree.Location = new System.Drawing.Point(0, 0);
            this.typeTree.Name = "typeTree";
            this.typeTree.Size = new System.Drawing.Size(257, 380);
            this.typeTree.TabIndex = 18;
            this.typeTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.onTypeChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(263, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 18);
            this.label1.TabIndex = 19;
            this.label1.Text = "Author:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(263, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 18);
            this.label6.TabIndex = 20;
            this.label6.Text = "Description:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.description.BackColor = System.Drawing.SystemColors.Control;
            this.description.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.description.Location = new System.Drawing.Point(333, 147);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.description.Size = new System.Drawing.Size(220, 90);
            this.description.TabIndex = 21;
            this.description.Text = "tadasdffas";
            // 
            // author
            // 
            this.author.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.author.Location = new System.Drawing.Point(331, 121);
            this.author.Name = "author";
            this.author.Size = new System.Drawing.Size(222, 18);
            this.author.TabIndex = 22;
            this.author.Text = "477";
            this.author.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(403, 266);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 20);
            this.label7.TabIndex = 23;
            this.label7.Text = "car(s)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(263, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 17);
            this.label10.TabIndex = 24;
            this.label10.Text = "Name:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // name
            // 
            this.name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.name.Location = new System.Drawing.Point(333, 9);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(220, 17);
            this.name.TabIndex = 25;
            this.name.Text = "123 Series ABCDEF";
            this.name.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // preview
            // 
            this.preview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.preview.Location = new System.Drawing.Point(331, 35);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(222, 78);
            this.preview.TabIndex = 26;
            this.preview.TabStop = false;
            // 
            // buttonImages
            // 
            this.buttonImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.buttonImages.ImageSize = new System.Drawing.Size(16, 15);
            this.buttonImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolBarDayNight
            // 
            this.toolBarDayNight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolBarDayNight.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbDay,
            this.tbNight});
            this.toolBarDayNight.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBarDayNight.DropDownArrows = true;
            this.toolBarDayNight.ImageList = this.buttonImages;
            this.toolBarDayNight.Location = new System.Drawing.Point(287, 35);
            this.toolBarDayNight.Name = "toolBarDayNight";
            this.toolBarDayNight.ShowToolTips = true;
            this.toolBarDayNight.Size = new System.Drawing.Size(38, 48);
            this.toolBarDayNight.TabIndex = 27;
            this.toolBarDayNight.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // tbDay
            // 
            this.tbDay.ImageIndex = 1;
            this.tbDay.Name = "tbDay";
            this.tbDay.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbDay.Tag = FreeTrain.Views.NightSpriteMode.AlwaysDay;
            // 
            // tbNight
            // 
            this.tbNight.ImageIndex = 2;
            this.tbNight.Name = "tbNight";
            this.tbNight.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbNight.Tag = FreeTrain.Views.NightSpriteMode.AlwaysNight;
            // 
            // TrainTradingDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(570, 380);
            this.Controls.Add(this.toolBarDayNight);
            this.Controls.Add(this.preview);
            this.Controls.Add(this.name);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.author);
            this.Controls.Add(this.description);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.typeTree);
            this.Controls.Add(this.passenger);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.totalPrice);
            this.Controls.Add(this.speed);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.count);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.length);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrainTradingDialog";
            this.Text = "Train Trading";
            this.Load += new System.EventHandler(this.TrainTradingDialogLoad);
            ((System.ComponentModel.ISupportInitialize)(this.length)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.count)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
