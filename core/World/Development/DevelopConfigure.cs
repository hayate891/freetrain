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
using FreeTrain.Framework;

namespace FreeTrain.World.Development
{
    /// <summary>
    /// 
    /// </summary>
    public class DevelopConfigure : FreeTrain.Controllers.AbstractControllerImpl
    {
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbLandPliceScale;
        private System.Windows.Forms.TextBox tbMaxPricePower;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbStrDiffuse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbReplacePriceFactor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbPopAmpScale;
        private System.Windows.Forms.TextBox tbPopAmpPower;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbQAlpha;
        private System.Windows.Forms.TextBox tbAddedQScale;
        private System.Windows.Forms.TextBox tbLandValuePower;
        private System.Windows.Forms.TextBox tbQDiffuse;
        private System.Windows.Forms.TextBox tbBaseRho;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox a;
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// 
        /// </summary>
        public DevelopConfigure()
            : base()
        {

            // この呼び出しは Windows フォーム デザイナで必要です。
            InitializeComponent();

            tbLandPliceScale.Text = string.Format("{0:0.00}", SearchPlan.F_LandPriceScale.ToString());
            tbMaxPricePower.Text = string.Format("{0:0.00}", SearchPlan.F_MaxPricePower);
            tbStrDiffuse.Text = string.Format("{0:0.00}", SearchPlan.F_StrDiffuse);
            tbReplacePriceFactor.Text = string.Format("{0:0.00}", SearchPlan.F_ReplacePriceFactor);
            tbPopAmpScale.Text = string.Format("{0:0.00}", SearchPlan.F_PopAmpScale);
            tbPopAmpPower.Text = string.Format("{0:0.00}", SearchPlan.F_PopAmpPower);

            tbQAlpha.Text = string.Format("{0:0.000}", LandValue.ALPHA);
            tbAddedQScale.Text = string.Format("{0}", LandValue.UPDATE_FREQUENCY);
            tbLandValuePower.Text = string.Format("{0:0.00}", LandValue.LAND_VAL_POWER);
            tbQDiffuse.Text = string.Format("{0:0.000}", LandValue.DIFF);
            tbBaseRho.Text = string.Format("{0:0.00}", LandValue.RHO_BARE_LAND);

        }

        /// <summary>
        /// 使用されているリソースに後処理を実行します。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region デザイナで生成されたコード
        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevelopConfigure));
            this.tbLandPliceScale = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbMaxPricePower = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.tbStrDiffuse = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbReplacePriceFactor = new System.Windows.Forms.TextBox();
            this.tbPopAmpScale = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbPopAmpPower = new System.Windows.Forms.TextBox();
            this.tbQDiffuse = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbQAlpha = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbBaseRho = new System.Windows.Forms.TextBox();
            this.tbAddedQScale = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tbLandValuePower = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.a = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLandPliceScale
            // 
            this.tbLandPliceScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLandPliceScale.Location = new System.Drawing.Point(252, 16);
            this.tbLandPliceScale.Name = "tbLandPliceScale";
            this.tbLandPliceScale.Size = new System.Drawing.Size(72, 20);
            this.tbLandPliceScale.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbLandPliceScale, "Multiplier for land price to calculate lower limit of price of structure to be bu" +
                    "ilt");
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(248, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Coeff. structure minimum price (>=0.0):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label2, "Multiplier for land price to calculate lower limit of price of structure to be bu" +
                    "ilt");
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(6, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(248, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Coeff. structure maximum price (> 0.0):";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label3, "Exponential for passengers to caluculate upper limit of price of structure to be " +
                    "built");
            // 
            // tbMaxPricePower
            // 
            this.tbMaxPricePower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMaxPricePower.Location = new System.Drawing.Point(252, 60);
            this.tbMaxPricePower.Name = "tbMaxPricePower";
            this.tbMaxPricePower.Size = new System.Drawing.Size(72, 20);
            this.tbMaxPricePower.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbMaxPricePower, "Exponential for passengers to calucurate upper limit of price of structure to be " +
                    "built");
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(489, 315);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(80, 25);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(585, 315);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(6, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "1-[weekly effect reduce ratio] (0.0-1.0):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label1, "Weekly multiplier for reducing intensity of development");
            // 
            // tbStrDiffuse
            // 
            this.tbStrDiffuse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStrDiffuse.Location = new System.Drawing.Point(252, 102);
            this.tbStrDiffuse.Name = "tbStrDiffuse";
            this.tbStrDiffuse.Size = new System.Drawing.Size(72, 20);
            this.tbStrDiffuse.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbStrDiffuse, "Weekly multiplier for reducing intensity of development");
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(6, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(248, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Coeff. replacement threshold (>=0.0):";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label4, "Replacement occurs when existing structure price is lower than this coeff. mutipl" +
                    "ied with the land price.");
            // 
            // tbReplacePriceFactor
            // 
            this.tbReplacePriceFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReplacePriceFactor.Location = new System.Drawing.Point(252, 103);
            this.tbReplacePriceFactor.Name = "tbReplacePriceFactor";
            this.tbReplacePriceFactor.Size = new System.Drawing.Size(72, 20);
            this.tbReplacePriceFactor.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbReplacePriceFactor, "Replacement occurs when existing structure price is lower than this coeff. mutipl" +
                    "ied with the land price.");
            // 
            // tbPopAmpScale
            // 
            this.tbPopAmpScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPopAmpScale.Location = new System.Drawing.Point(252, 59);
            this.tbPopAmpScale.Name = "tbPopAmpScale";
            this.tbPopAmpScale.Size = new System.Drawing.Size(72, 20);
            this.tbPopAmpScale.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbPopAmpScale, "Multiplier for station effective area (>= 0.0):");
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(6, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(248, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "Multiplier for station effective area (>= 0.0):";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label7, "Multiplier for land price to calculate effective area");
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(6, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(248, 18);
            this.label8.TabIndex = 1;
            this.label8.Text = "Coeff. station effective area (> 0.0):";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label8, "Coefficient for station effective area against land price");
            // 
            // tbPopAmpPower
            // 
            this.tbPopAmpPower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPopAmpPower.Location = new System.Drawing.Point(252, 15);
            this.tbPopAmpPower.Name = "tbPopAmpPower";
            this.tbPopAmpPower.Size = new System.Drawing.Size(72, 20);
            this.tbPopAmpPower.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbPopAmpPower, "Coefficient for station effective area against land price");
            // 
            // tbQDiffuse
            // 
            this.tbQDiffuse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbQDiffuse.Location = new System.Drawing.Point(246, 61);
            this.tbQDiffuse.Name = "tbQDiffuse";
            this.tbQDiffuse.Size = new System.Drawing.Size(72, 20);
            this.tbQDiffuse.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbQDiffuse, "Multiplied every phase to reduce land price");
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(8, 61);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(238, 17);
            this.label11.TabIndex = 1;
            this.label11.Text = "1-[land price emit ratio] (0.0-0.999):";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label11, "Multiplied every phase to reduce land price");
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(8, 104);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(238, 17);
            this.label12.TabIndex = 1;
            this.label12.Text = "Land price conductivity (0-0.25):";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label12, "The ratio of influence on land price by neighboring voxel");
            // 
            // tbQAlpha
            // 
            this.tbQAlpha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbQAlpha.Location = new System.Drawing.Point(246, 104);
            this.tbQAlpha.Name = "tbQAlpha";
            this.tbQAlpha.Size = new System.Drawing.Size(72, 20);
            this.tbQAlpha.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbQAlpha, "隣のボクセルから伝わる地価落差に乗ずる係数");
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Location = new System.Drawing.Point(8, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(238, 19);
            this.label13.TabIndex = 1;
            this.label13.Text = "Land price diffusivity (0.4-0.999):";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label13, "The ratio of land price diffusion for neighboring voxels");
            // 
            // tbBaseRho
            // 
            this.tbBaseRho.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBaseRho.Location = new System.Drawing.Point(246, 17);
            this.tbBaseRho.Name = "tbBaseRho";
            this.tbBaseRho.Size = new System.Drawing.Size(72, 20);
            this.tbBaseRho.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbBaseRho, "The ratio of land price diffusion for neighboring voxels");
            // 
            // tbAddedQScale
            // 
            this.tbAddedQScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAddedQScale.Location = new System.Drawing.Point(246, 147);
            this.tbAddedQScale.Name = "tbAddedQScale";
            this.tbAddedQScale.Size = new System.Drawing.Size(72, 20);
            this.tbAddedQScale.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbAddedQScale, "Multiplier for land price increment when passengers arrives");
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Location = new System.Drawing.Point(8, 147);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(238, 18);
            this.label14.TabIndex = 1;
            this.label14.Text = "Multiplier for land price increase (>=0, int):";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label14, "Multiplier for land price increment when passengers arrives");
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(8, 191);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(238, 17);
            this.label15.TabIndex = 1;
            this.label15.Text = "Land price modifier (> 0.0):";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label15, "Multiplier from converted internal land value to land price");
            // 
            // tbLandValuePower
            // 
            this.tbLandValuePower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLandValuePower.Location = new System.Drawing.Point(246, 191);
            this.tbLandValuePower.Name = "tbLandValuePower";
            this.tbLandValuePower.Size = new System.Drawing.Size(72, 20);
            this.tbLandValuePower.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbLandValuePower, "Multiplier from converted internal land value to land price");
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.BackColor = System.Drawing.SystemColors.Highlight;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label10.Location = new System.Drawing.Point(8, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(238, 17);
            this.label10.TabIndex = 1;
            this.label10.Text = "The larger the more widely is land price raised";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label10, "The ratio of land price diffusion for neighboring voxels");
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.BackColor = System.Drawing.SystemColors.Highlight;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label16.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label16.Location = new System.Drawing.Point(8, 78);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(238, 17);
            this.label16.TabIndex = 1;
            this.label16.Text = "The larger the more widely is land price raised";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label16, "Multiplied every phase to reduce land price");
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.BackColor = System.Drawing.SystemColors.Highlight;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label17.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label17.Location = new System.Drawing.Point(8, 121);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(238, 18);
            this.label17.TabIndex = 1;
            this.label17.Text = "The larger the more quickly land price rises";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label17, "The ratio in which land price affects neighboring voxels");
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.BackColor = System.Drawing.SystemColors.Highlight;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label18.Location = new System.Drawing.Point(8, 165);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(238, 17);
            this.label18.TabIndex = 1;
            this.label18.Text = "Enhance land price increment for transportation";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label18, "Multiplier for land price increment when passengers arrives");
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.BackColor = System.Drawing.SystemColors.Highlight;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label19.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label19.Location = new System.Drawing.Point(8, 208);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(238, 17);
            this.label19.TabIndex = 1;
            this.label19.Text = "Enhance land price increment for transportation";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label19, "Multiplier for land price increment when passengers arrives");
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.BackColor = System.Drawing.SystemColors.Highlight;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label20.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label20.Location = new System.Drawing.Point(6, 35);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(248, 17);
            this.label20.TabIndex = 1;
            this.label20.Text = "The larger the wider area around stations develops";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label20, "Coefficient for station effective area against land price");
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.BackColor = System.Drawing.SystemColors.Highlight;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label21.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label21.Location = new System.Drawing.Point(6, 78);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(248, 17);
            this.label21.TabIndex = 1;
            this.label21.Text = "The larger the wider area around stations develops";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label21, "Multiplier for land price to calculate effective area");
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label22.BackColor = System.Drawing.SystemColors.Highlight;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label22.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label22.Location = new System.Drawing.Point(6, 121);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(248, 18);
            this.label22.TabIndex = 1;
            this.label22.Text = "The larger the more expensive structures are built";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label22, "Weekly multiplier to reduce intensity of development");
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.BackColor = System.Drawing.SystemColors.Highlight;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label9.Location = new System.Drawing.Point(6, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(248, 17);
            this.label9.TabIndex = 1;
            this.label9.Text = "The larger the cheaper structures are built";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label9, "Multiplier for land price to calculate lower limit of the price of structures to " +
                    "be built");
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.BackColor = System.Drawing.SystemColors.Highlight;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label23.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label23.Location = new System.Drawing.Point(6, 78);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(248, 17);
            this.label23.TabIndex = 1;
            this.label23.Text = "The larger the more expensive structures are built";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label23, "Exponential for passengers to caluculate upper limit of the price of structures t" +
                    "o be built");
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.BackColor = System.Drawing.SystemColors.Highlight;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label24.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label24.Location = new System.Drawing.Point(6, 121);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(248, 18);
            this.label24.TabIndex = 1;
            this.label24.Text = "The smaller the more replacement occurs";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label24, "Replacement occurs when existing structure price is lower than the land valuemult" +
                    "iplied with this coeff.");
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(8, 306);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(665, 2);
            this.label5.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(16, 315);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 25);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save Settings";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Location = new System.Drawing.Point(112, 315);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(80, 25);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load Settings";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.tbQDiffuse);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.tbLandValuePower);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.tbAddedQScale);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.tbBaseRho);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.tbQAlpha);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 234);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Land price";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.tbPopAmpScale);
            this.groupBox2.Controls.Add(this.tbStrDiffuse);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbPopAmpPower);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Location = new System.Drawing.Point(332, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(332, 147);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Area development rate";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.tbReplacePriceFactor);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.tbMaxPricePower);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.tbLandPliceScale);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.label24);
            this.groupBox3.Location = new System.Drawing.Point(332, 150);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(332, 147);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Structure selection";
            // 
            // a
            // 
            this.a.BackColor = System.Drawing.SystemColors.Control;
            this.a.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.a.ForeColor = System.Drawing.SystemColors.InfoText;
            this.a.Location = new System.Drawing.Point(6, 243);
            this.a.Multiline = true;
            this.a.Name = "a";
            this.a.ReadOnly = true;
            this.a.Size = new System.Drawing.Size(312, 69);
            this.a.TabIndex = 7;
            this.a.Text = resources.GetString("a.Text");
            // 
            // DevelopConfigure
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(683, 344);
            this.Controls.Add(this.a);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DevelopConfigure";
            this.Text = "Development Algorithm: Parameters";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Dispose();
        }

        private void btnApply_Click(object sender, System.EventArgs e)
        {
            try
            {
                SearchPlan.F_LandPriceScale = double.Parse(tbLandPliceScale.Text);
                SearchPlan.F_MaxPricePower = double.Parse(tbMaxPricePower.Text);
                SearchPlan.F_StrDiffuse = double.Parse(tbStrDiffuse.Text);
                SearchPlan.F_ReplacePriceFactor = double.Parse(tbReplacePriceFactor.Text);
                SearchPlan.F_PopAmpScale = double.Parse(tbPopAmpScale.Text);
                SearchPlan.F_PopAmpPower = double.Parse(tbPopAmpPower.Text);

                LandValue.ALPHA = float.Parse(tbQAlpha.Text);
                LandValue.UPDATE_FREQUENCY = int.Parse(tbAddedQScale.Text);
                LandValue.LAND_VAL_POWER = double.Parse(tbLandValuePower.Text);
                LandValue.DIFF = float.Parse(tbQDiffuse.Text);
                LandValue.RHO_BARE_LAND = float.Parse(tbBaseRho.Text);
            }
            finally
            {
                Dispose();
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            int i = 0;
            Core.Options.devParams[i++] = double.Parse(tbLandPliceScale.Text);
            Core.Options.devParams[i++] = double.Parse(tbMaxPricePower.Text);
            Core.Options.devParams[i++] = double.Parse(tbStrDiffuse.Text);
            Core.Options.devParams[i++] = double.Parse(tbReplacePriceFactor.Text);
            Core.Options.devParams[i++] = double.Parse(tbPopAmpScale.Text);
            Core.Options.devParams[i++] = double.Parse(tbPopAmpPower.Text);

            Core.Options.devParams[i++] = float.Parse(tbQAlpha.Text);
            Core.Options.devParams[i++] = int.Parse(tbAddedQScale.Text);
            Core.Options.devParams[i++] = double.Parse(tbLandValuePower.Text);
            Core.Options.devParams[i++] = float.Parse(tbQDiffuse.Text);
            Core.Options.devParams[i++] = float.Parse(tbBaseRho.Text);
        }

        private void btnLoad_Click(object sender, System.EventArgs e)
        {
            int i = 0;
            tbLandPliceScale.Text = string.Format("{0:0.00}", Core.Options.devParams[i++]);
            tbMaxPricePower.Text = string.Format("{0:0.00}", Core.Options.devParams[i++]);
            tbStrDiffuse.Text = string.Format("{0:0.00}", Core.Options.devParams[i++]);
            tbReplacePriceFactor.Text = string.Format("{0:0.00}", Core.Options.devParams[i++]);
            tbPopAmpScale.Text = string.Format("{0:0.00}", Core.Options.devParams[i++]);
            tbPopAmpPower.Text = string.Format("{0:0.00}", Core.Options.devParams[i++]);

            tbQAlpha.Text = string.Format("{0:0.000}", Core.Options.devParams[i++]);
            tbAddedQScale.Text = string.Format("{0}", (int)Core.Options.devParams[i++]);
            tbLandValuePower.Text = string.Format("{0:0.00}", Core.Options.devParams[i++]);
            tbQDiffuse.Text = string.Format("{0:0.000}", Core.Options.devParams[i++]);
            tbBaseRho.Text = string.Format("{0:0.00}", Core.Options.devParams[i++]);
        }
    }
}

