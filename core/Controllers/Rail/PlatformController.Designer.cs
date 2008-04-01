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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Rail;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Util;
using FreeTrain.World;
using FreeTrain.World.Rail;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.Controls;


namespace FreeTrain.Controllers.Rail
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PlatformController : AbstractControllerImpl, IMapOverlay, ILocationDisambiguator
    {
       #region Designer generated code
        private System.Windows.Forms.TabPage stationPage;
        private System.Windows.Forms.ComboBox stationType;
        private System.Windows.Forms.PictureBox stationPicture;
        private System.Windows.Forms.TabPage platformPage;
        private System.Windows.Forms.PictureBox dirS;
        private System.Windows.Forms.PictureBox dirW;
        private System.Windows.Forms.PictureBox dirE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown lengthBox;
        private System.Windows.Forms.PictureBox dirN;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.CheckBox checkSlim;
        private System.Windows.Forms.RadioButton buttonRemove;
        private System.Windows.Forms.RadioButton buttonPlace;
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Designer サポートに必要なメソッドです。コード エディタで
        /// このメソッドのコンテンツを変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonRemove = new System.Windows.Forms.RadioButton();
            this.buttonPlace = new System.Windows.Forms.RadioButton();
            this.stationPage = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.indexSelector = new FreeTrain.Controls.IndexSelector();
            this.stationType = new System.Windows.Forms.ComboBox();
            this.stationPicture = new System.Windows.Forms.PictureBox();
            this.indexSelector1 = new FreeTrain.Controls.IndexSelector();
            this.label4 = new System.Windows.Forms.Label();
            this.indexSelector2 = new FreeTrain.Controls.IndexSelector();
            this.label5 = new System.Windows.Forms.Label();
            this.platformPage = new System.Windows.Forms.TabPage();
            this.checkSlim = new System.Windows.Forms.CheckBox();
            this.dirS = new System.Windows.Forms.PictureBox();
            this.dirW = new System.Windows.Forms.PictureBox();
            this.dirE = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lengthBox = new System.Windows.Forms.NumericUpDown();
            this.dirN = new System.Windows.Forms.PictureBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.stationPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stationPicture)).BeginInit();
            this.platformPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dirS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lengthBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirN)).BeginInit();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(236)))));
            this.buttonRemove.Location = new System.Drawing.Point(109, 208);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(76, 26);
            this.buttonRemove.TabIndex = 1;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonRemove.UseVisualStyleBackColor = false;
            // 
            // buttonPlace
            // 
            this.buttonPlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonPlace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(236)))));
            this.buttonPlace.Checked = true;
            this.buttonPlace.Location = new System.Drawing.Point(9, 208);
            this.buttonPlace.Name = "buttonPlace";
            this.buttonPlace.Size = new System.Drawing.Size(85, 26);
            this.buttonPlace.TabIndex = 0;
            this.buttonPlace.TabStop = true;
            this.buttonPlace.Text = "Build";
            this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonPlace.UseVisualStyleBackColor = false;
            // 
            // stationPage
            // 
            this.stationPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(236)))));
            this.stationPage.Controls.Add(this.label3);
            this.stationPage.Controls.Add(this.label2);
            this.stationPage.Controls.Add(this.listView1);
            this.stationPage.Controls.Add(this.indexSelector);
            this.stationPage.Controls.Add(this.stationType);
            this.stationPage.Controls.Add(this.stationPicture);
            this.stationPage.Controls.Add(this.indexSelector1);
            this.stationPage.Controls.Add(this.label4);
            this.stationPage.Controls.Add(this.indexSelector2);
            this.stationPage.Controls.Add(this.label5);
            this.stationPage.Location = new System.Drawing.Point(4, 21);
            this.stationPage.Margin = new System.Windows.Forms.Padding(0);
            this.stationPage.Name = "stationPage";
            this.stationPage.Size = new System.Drawing.Size(178, 155);
            this.stationPage.TabIndex = 1;
            this.stationPage.Text = "Station";
            this.stationPage.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Design";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(279, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView1.Location = new System.Drawing.Point(191, 9);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(103, 156);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Scale";
            this.columnHeader2.Width = 54;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Material";
            this.columnHeader3.Width = 54;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Size";
            this.columnHeader4.Width = 64;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Cost";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Maintenance";
            // 
            // indexSelector
            // 
            this.indexSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.indexSelector.count = 10;
            this.indexSelector.current = 0;
            this.indexSelector.dataSource = null;
            this.indexSelector.Location = new System.Drawing.Point(70, 35);
            this.indexSelector.Name = "indexSelector";
            this.indexSelector.Size = new System.Drawing.Size(103, 17);
            this.indexSelector.TabIndex = 3;
            this.indexSelector.indexChanged += new System.EventHandler(this.onStationChanged);
            // 
            // stationType
            // 
            this.stationType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.stationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stationType.ItemHeight = 13;
            this.stationType.Location = new System.Drawing.Point(3, 9);
            this.stationType.Name = "stationType";
            this.stationType.Size = new System.Drawing.Size(170, 21);
            this.stationType.Sorted = true;
            this.stationType.TabIndex = 2;
            this.stationType.SelectedIndexChanged += new System.EventHandler(this.onGroupChanged);
            // 
            // stationPicture
            // 
            this.stationPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.stationPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.stationPicture.Location = new System.Drawing.Point(3, 54);
            this.stationPicture.Name = "stationPicture";
            this.stationPicture.Size = new System.Drawing.Size(170, 91);
            this.stationPicture.TabIndex = 1;
            this.stationPicture.TabStop = false;
            // 
            // indexSelector1
            // 
            this.indexSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.indexSelector1.count = 10;
            this.indexSelector1.current = 0;
            this.indexSelector1.dataSource = null;
            this.indexSelector1.Location = new System.Drawing.Point(235, 85);
            this.indexSelector1.Name = "indexSelector1";
            this.indexSelector1.Size = new System.Drawing.Size(96, 17);
            this.indexSelector1.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(245, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Direction:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // indexSelector2
            // 
            this.indexSelector2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.indexSelector2.count = 10;
            this.indexSelector2.current = 0;
            this.indexSelector2.dataSource = null;
            this.indexSelector2.Location = new System.Drawing.Point(235, 102);
            this.indexSelector2.Name = "indexSelector2";
            this.indexSelector2.Size = new System.Drawing.Size(96, 17);
            this.indexSelector2.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(245, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Colour:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // platformPage
            // 
            this.platformPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(236)))));
            this.platformPage.Controls.Add(this.checkSlim);
            this.platformPage.Controls.Add(this.dirS);
            this.platformPage.Controls.Add(this.dirW);
            this.platformPage.Controls.Add(this.dirE);
            this.platformPage.Controls.Add(this.label1);
            this.platformPage.Controls.Add(this.lengthBox);
            this.platformPage.Controls.Add(this.dirN);
            this.platformPage.Location = new System.Drawing.Point(4, 21);
            this.platformPage.Name = "platformPage";
            this.platformPage.Size = new System.Drawing.Size(178, 155);
            this.platformPage.TabIndex = 0;
            this.platformPage.Text = "Platform";
            this.platformPage.UseVisualStyleBackColor = true;
            // 
            // checkSlim
            // 
            this.checkSlim.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkSlim.Location = new System.Drawing.Point(8, 109);
            this.checkSlim.Name = "checkSlim";
            this.checkSlim.Size = new System.Drawing.Size(164, 17);
            this.checkSlim.TabIndex = 7;
            this.checkSlim.Text = "Slim platform";
            this.checkSlim.CheckedChanged += new System.EventHandler(this.onModeChanged);
            // 
            // dirS
            // 
            this.dirS.BackColor = System.Drawing.Color.White;
            this.dirS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dirS.Location = new System.Drawing.Point(64, 69);
            this.dirS.Name = "dirS";
            this.dirS.Size = new System.Drawing.Size(48, 52);
            this.dirS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dirS.TabIndex = 6;
            this.dirS.TabStop = false;
            this.dirS.Click += new System.EventHandler(this.onDirChange);
            // 
            // dirW
            // 
            this.dirW.BackColor = System.Drawing.Color.White;
            this.dirW.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dirW.Location = new System.Drawing.Point(8, 69);
            this.dirW.Name = "dirW";
            this.dirW.Size = new System.Drawing.Size(48, 52);
            this.dirW.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dirW.TabIndex = 5;
            this.dirW.TabStop = false;
            this.dirW.Click += new System.EventHandler(this.onDirChange);
            // 
            // dirE
            // 
            this.dirE.BackColor = System.Drawing.Color.White;
            this.dirE.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dirE.Location = new System.Drawing.Point(64, 9);
            this.dirE.Name = "dirE";
            this.dirE.Size = new System.Drawing.Size(48, 52);
            this.dirE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dirE.TabIndex = 4;
            this.dirE.TabStop = false;
            this.dirE.Click += new System.EventHandler(this.onDirChange);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(3, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Length:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lengthBox
            // 
            this.lengthBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lengthBox.Location = new System.Drawing.Point(64, 128);
            this.lengthBox.Name = "lengthBox";
            this.lengthBox.Size = new System.Drawing.Size(108, 20);
            this.lengthBox.TabIndex = 3;
            this.lengthBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lengthBox.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.lengthBox.Validating += new System.ComponentModel.CancelEventHandler(this.validateLength);
            this.lengthBox.TextChanged += new System.EventHandler(this.onLengthChanged);
            // 
            // dirN
            // 
            this.dirN.BackColor = System.Drawing.Color.White;
            this.dirN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dirN.Location = new System.Drawing.Point(8, 9);
            this.dirN.Name = "dirN";
            this.dirN.Size = new System.Drawing.Size(48, 52);
            this.dirN.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dirN.TabIndex = 1;
            this.dirN.TabStop = false;
            this.dirN.Click += new System.EventHandler(this.onDirChange);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.stationPage);
            this.tabControl.Controls.Add(this.platformPage);
            this.tabControl.ItemSize = new System.Drawing.Size(42, 17);
            this.tabControl.Location = new System.Drawing.Point(5, 26);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(186, 180);
            this.tabControl.TabIndex = 0;
            this.tabControl.Click += new System.EventHandler(this.updateAfterResize);
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.onModeChanged);
            // 
            // PlatformController
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(197, 240);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonPlace);
            this.Controls.Add(this.buttonRemove);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PlatformController";
            this.Text = "Station Construction";
            this.Resize += new System.EventHandler(this.updateAfterResize);
            this.stationPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stationPicture)).EndInit();
            this.platformPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dirS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lengthBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirN)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}

