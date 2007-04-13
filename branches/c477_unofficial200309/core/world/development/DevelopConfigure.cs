using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using freetrain.framework;

namespace freetrain.world.development
{
	public class DevelopConfigure : freetrain.controllers.AbstractControllerForm
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

		public DevelopConfigure() :base()
		{			
			
			// ���̌Ăяo���� Windows �t�H�[�� �f�U�C�i�ŕK�v�ł��B
			InitializeComponent();
			
			tbLandPliceScale.Text = string.Format("{0:0.00}",SearchPlan.F_LandPriceScale.ToString());
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
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region �f�U�C�i�Ő������ꂽ�R�[�h
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
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
			this.tbLandPliceScale.Location = new System.Drawing.Point(216, 24);
			this.tbLandPliceScale.Name = "tbLandPliceScale";
			this.tbLandPliceScale.Size = new System.Drawing.Size(72, 19);
			this.tbLandPliceScale.TabIndex = 0;
			this.tbLandPliceScale.Text = "";
			this.toolTip1.SetToolTip(this.tbLandPliceScale, "�n���ɑ΂��ď悶�āA�����̍Œቿ�i�ɂ���");
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(8, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(192, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "�����������i�E�Βn���W�� (>=0.0):";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label2, "�n���ɑ΂��ď悶�āA�����̍Œቿ�i�ɂ���");
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(0, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(200, 16);
			this.label3.TabIndex = 1;
			this.label3.Text = "����������i�E�΍~�ԋq�w�� (> 0.0):";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label3, "�ώZ�~�ԋq���̎w���Ƃ��ēK�p���A�����̉��i�ő�l�����߂�");
			// 
			// tbMaxPricePower
			// 
			this.tbMaxPricePower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMaxPricePower.Location = new System.Drawing.Point(216, 64);
			this.tbMaxPricePower.Name = "tbMaxPricePower";
			this.tbMaxPricePower.Size = new System.Drawing.Size(72, 19);
			this.tbMaxPricePower.TabIndex = 0;
			this.tbMaxPricePower.Text = "";
			this.toolTip1.SetToolTip(this.tbMaxPricePower, "�ώZ�~�ԋq���̎w���Ƃ��ēK�p���A�����̉��i�ő�l�����߂�");
			// 
			// btnApply
			// 
			this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnApply.Location = new System.Drawing.Point(408, 296);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(80, 23);
			this.btnApply.TabIndex = 2;
			this.btnApply.Text = "�K�p";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(504, 296);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "��ݾ�";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(32, 96);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(176, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "1-���W�x������ (0.0�`1.0):";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label1, "���W�X�R�A�ɖ��T�悶��W��");
			// 
			// tbStrDiffuse
			// 
			this.tbStrDiffuse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbStrDiffuse.Location = new System.Drawing.Point(216, 96);
			this.tbStrDiffuse.Name = "tbStrDiffuse";
			this.tbStrDiffuse.Size = new System.Drawing.Size(72, 19);
			this.tbStrDiffuse.TabIndex = 0;
			this.tbStrDiffuse.Text = "";
			this.toolTip1.SetToolTip(this.tbStrDiffuse, "���W�X�R�A�ɖ��T�悶��W��");
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(24, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(176, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "���đւ����i�W�� (>=0.0):";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label4, "�n���ɂ��̒l���悶���l���A���i���Ⴂ�����͌��đւ��\");
			// 
			// tbReplacePriceFactor
			// 
			this.tbReplacePriceFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbReplacePriceFactor.Location = new System.Drawing.Point(216, 104);
			this.tbReplacePriceFactor.Name = "tbReplacePriceFactor";
			this.tbReplacePriceFactor.Size = new System.Drawing.Size(72, 19);
			this.tbReplacePriceFactor.TabIndex = 0;
			this.tbReplacePriceFactor.Text = "";
			this.toolTip1.SetToolTip(this.tbReplacePriceFactor, "�n���ɂ��̒l���悶���l���A���i���Ⴂ�����͌��đւ��\");
			// 
			// tbPopAmpScale
			// 
			this.tbPopAmpScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPopAmpScale.Location = new System.Drawing.Point(216, 56);
			this.tbPopAmpScale.Name = "tbPopAmpScale";
			this.tbPopAmpScale.Size = new System.Drawing.Size(72, 19);
			this.tbPopAmpScale.TabIndex = 0;
			this.tbPopAmpScale.Text = "";
			this.toolTip1.SetToolTip(this.tbPopAmpScale, "�w�̔��W�͈͂����肷�邽�ߒn���ɏ悸��l");
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.Location = new System.Drawing.Point(0, 56);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(208, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "�w�e���͈́E�Βn���搔 (>= 0.0):";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label7, "�w�̔��W�͈͂����肷�邽�ߒn���ɏ悸��l");
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.BackColor = System.Drawing.Color.Transparent;
			this.label8.Location = new System.Drawing.Point(32, 16);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(176, 16);
			this.label8.TabIndex = 1;
			this.label8.Text = "�w�e���͈́E�Βn���w�� (> 0.0):";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label8, "�w�̔��W�͈͂����肷��n���ɑ΂���w��");
			// 
			// tbPopAmpPower
			// 
			this.tbPopAmpPower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPopAmpPower.Location = new System.Drawing.Point(216, 16);
			this.tbPopAmpPower.Name = "tbPopAmpPower";
			this.tbPopAmpPower.Size = new System.Drawing.Size(72, 19);
			this.tbPopAmpPower.TabIndex = 0;
			this.tbPopAmpPower.Text = "";
			this.toolTip1.SetToolTip(this.tbPopAmpPower, "�w�̔��W�͈͂����肷��n���ɑ΂���w��");
			// 
			// tbQDiffuse
			// 
			this.tbQDiffuse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbQDiffuse.Location = new System.Drawing.Point(216, 56);
			this.tbQDiffuse.Name = "tbQDiffuse";
			this.tbQDiffuse.Size = new System.Drawing.Size(72, 19);
			this.tbQDiffuse.TabIndex = 0;
			this.tbQDiffuse.Text = "";
			this.toolTip1.SetToolTip(this.tbQDiffuse, "���񌻍ݒn���ɏ悸��W��");
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.BackColor = System.Drawing.Color.Transparent;
			this.label11.Location = new System.Drawing.Point(24, 56);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(176, 16);
			this.label11.TabIndex = 1;
			this.label11.Text = "1-�n�����U�� (0.0�`0.999):";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label11, "���񌻍ݒn���ɏ悸��W��");
			// 
			// label12
			// 
			this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label12.BackColor = System.Drawing.Color.Transparent;
			this.label12.Location = new System.Drawing.Point(-8, 96);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(208, 16);
			this.label12.TabIndex = 1;
			this.label12.Text = "�n���`���� (0�`0.25):";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label12, "�ׂ̃{�N�Z������`���n�������ɏ悸��W��");
			// 
			// tbQAlpha
			// 
			this.tbQAlpha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbQAlpha.Location = new System.Drawing.Point(216, 96);
			this.tbQAlpha.Name = "tbQAlpha";
			this.tbQAlpha.Size = new System.Drawing.Size(72, 19);
			this.tbQAlpha.TabIndex = 0;
			this.tbQAlpha.Text = "";
			this.toolTip1.SetToolTip(this.tbQAlpha, "�ׂ̃{�N�Z������`���n�������ɏ悸��W��");
			// 
			// label13
			// 
			this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label13.BackColor = System.Drawing.Color.Transparent;
			this.label13.Location = new System.Drawing.Point(24, 16);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(176, 16);
			this.label13.TabIndex = 1;
			this.label13.Text = "�W���n���`�d���x (0.4�`0.999):";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label13, "�n�����ׂ̃{�N�Z���ɓ`�d���銄��");
			// 
			// tbBaseRho
			// 
			this.tbBaseRho.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbBaseRho.Location = new System.Drawing.Point(216, 16);
			this.tbBaseRho.Name = "tbBaseRho";
			this.tbBaseRho.Size = new System.Drawing.Size(72, 19);
			this.tbBaseRho.TabIndex = 0;
			this.tbBaseRho.Text = "";
			this.toolTip1.SetToolTip(this.tbBaseRho, "�n�����ׂ̃{�N�Z���ɓ`�d���銄��");
			// 
			// tbAddedQScale
			// 
			this.tbAddedQScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbAddedQScale.Location = new System.Drawing.Point(216, 136);
			this.tbAddedQScale.Name = "tbAddedQScale";
			this.tbAddedQScale.Size = new System.Drawing.Size(72, 19);
			this.tbAddedQScale.TabIndex = 0;
			this.tbAddedQScale.Text = "";
			this.toolTip1.SetToolTip(this.tbAddedQScale, "�w�~�Ԏ��̒n�����Z�l�ɏ悸��W��");
			// 
			// label14
			// 
			this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label14.BackColor = System.Drawing.Color.Transparent;
			this.label14.Location = new System.Drawing.Point(-8, 136);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(208, 16);
			this.label14.TabIndex = 1;
			this.label14.Text = "�n���㏸�␳�W�� (>= 0, ����):";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label14, "�w�~�Ԏ��̒n�����Z�l�ɏ悸��W��");
			// 
			// label15
			// 
			this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label15.BackColor = System.Drawing.Color.Transparent;
			this.label15.Location = new System.Drawing.Point(24, 176);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(176, 16);
			this.label15.TabIndex = 1;
			this.label15.Text = "�ŏI�n���␳�w�� (> 0.0):";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label15, "�����n���f�[�^�����ۂ̒n���Ɋ��Z���邽�߂̕␳�w��");
			// 
			// tbLandValuePower
			// 
			this.tbLandValuePower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbLandValuePower.Location = new System.Drawing.Point(216, 176);
			this.tbLandValuePower.Name = "tbLandValuePower";
			this.tbLandValuePower.Size = new System.Drawing.Size(72, 19);
			this.tbLandValuePower.TabIndex = 0;
			this.tbLandValuePower.Text = "";
			this.toolTip1.SetToolTip(this.tbLandValuePower, "�����n���f�[�^�����ۂ̒n���Ɋ��Z���邽�߂̕␳�w��");
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.BackColor = System.Drawing.SystemColors.Highlight;
			this.label10.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label10.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label10.Location = new System.Drawing.Point(8, 32);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(208, 16);
			this.label10.TabIndex = 1;
			this.label10.Text = "�傫������ƍL�͈͂ɒn�����L����܂�";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label10, "�n�����ׂ̃{�N�Z���ɓ`�d���銄��");
			// 
			// label16
			// 
			this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label16.BackColor = System.Drawing.SystemColors.Highlight;
			this.label16.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label16.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label16.Location = new System.Drawing.Point(8, 72);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(208, 16);
			this.label16.TabIndex = 1;
			this.label16.Text = "�傫������ƍL�͈͂ɒn�����L����܂�";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label16, "���񌻍ݒn���ɏ悸��W��");
			// 
			// label17
			// 
			this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label17.BackColor = System.Drawing.SystemColors.Highlight;
			this.label17.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label17.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label17.Location = new System.Drawing.Point(8, 112);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(208, 16);
			this.label17.TabIndex = 1;
			this.label17.Text = "�傫������ƒn���㏸�������L����܂�";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label17, "�ׂ̃{�N�Z������`���n�������ɏ悸��W��");
			// 
			// label18
			// 
			this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label18.BackColor = System.Drawing.SystemColors.Highlight;
			this.label18.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label18.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label18.Location = new System.Drawing.Point(8, 152);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(208, 16);
			this.label18.TabIndex = 1;
			this.label18.Text = "�傫������Ə��Ȃ��A���Œn�����㏸���܂�";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label18, "�w�~�Ԏ��̒n�����Z�l�ɏ悸��W��");
			// 
			// label19
			// 
			this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label19.BackColor = System.Drawing.SystemColors.Highlight;
			this.label19.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label19.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label19.Location = new System.Drawing.Point(8, 192);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(208, 16);
			this.label19.TabIndex = 1;
			this.label19.Text = "�傫������Ə��Ȃ��A���Œn�����㏸���܂�";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label19, "�w�~�Ԏ��̒n�����Z�l�ɏ悸��W��");
			// 
			// label20
			// 
			this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label20.BackColor = System.Drawing.SystemColors.Highlight;
			this.label20.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label20.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label20.Location = new System.Drawing.Point(8, 32);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(208, 16);
			this.label20.TabIndex = 1;
			this.label20.Text = "�傫������Ɖw�̔��W�͈͂��L���Ȃ�܂�";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label20, "�w�̔��W�͈͂����肷��n���ɑ΂���w��");
			// 
			// label21
			// 
			this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label21.BackColor = System.Drawing.SystemColors.Highlight;
			this.label21.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label21.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label21.Location = new System.Drawing.Point(8, 72);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(208, 16);
			this.label21.TabIndex = 1;
			this.label21.Text = "�傫������Ɖw�̔��W�͈͂��L���Ȃ�܂�";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label21, "�w�̔��W�͈͂����肷�邽�ߒn���ɏ悸��l");
			// 
			// label22
			// 
			this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label22.BackColor = System.Drawing.SystemColors.Highlight;
			this.label22.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label22.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label22.Location = new System.Drawing.Point(8, 112);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(208, 16);
			this.label22.TabIndex = 1;
			this.label22.Text = "�傫������ƍ����Ȍ��z�������₷���Ȃ�܂�";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label22, "�w�̔��W�͈͂����肷�邽�ߒn���ɏ悸��l");
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label9.BackColor = System.Drawing.SystemColors.Highlight;
			this.label9.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label9.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label9.Location = new System.Drawing.Point(8, 32);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(208, 16);
			this.label9.TabIndex = 1;
			this.label9.Text = "�傫������ƈ����Ȍ��z�������ɂ����Ȃ�܂�";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label9, "�n���ɑ΂��ď悶�āA�����̍Œቿ�i�ɂ���");
			// 
			// label23
			// 
			this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label23.BackColor = System.Drawing.SystemColors.Highlight;
			this.label23.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label23.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label23.Location = new System.Drawing.Point(8, 72);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(208, 16);
			this.label23.TabIndex = 1;
			this.label23.Text = "�傫������ƍ����Ȍ��z�������₷���Ȃ�܂�";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label23, "�ώZ�~�ԋq���̎w���Ƃ��ēK�p���A�����̉��i�ő�l�����߂�");
			// 
			// label24
			// 
			this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label24.BackColor = System.Drawing.SystemColors.Highlight;
			this.label24.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.label24.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label24.Location = new System.Drawing.Point(8, 112);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(208, 16);
			this.label24.TabIndex = 1;
			this.label24.Text = "�傫������ƌ��đւ����N����ɂ����Ȃ�܂�";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.label24, "�n���ɂ��̒l���悶���l���A���i���Ⴂ�����͌��đւ��\");
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label5.Location = new System.Drawing.Point(8, 288);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(584, 2);
			this.label5.TabIndex = 3;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.Location = new System.Drawing.Point(16, 296);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(80, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "�ݒ�ۑ�";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnLoad.Location = new System.Drawing.Point(112, 296);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(80, 23);
			this.btnLoad.TabIndex = 2;
			this.btnLoad.Text = "�ݒ�Ǎ�";
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
			this.groupBox1.Location = new System.Drawing.Point(0, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(296, 216);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "�n��";
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
			this.groupBox2.Location = new System.Drawing.Point(304, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(296, 136);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "���W�x";
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
			this.groupBox3.Location = new System.Drawing.Point(304, 144);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(296, 136);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "�������i";
			// 
			// a
			// 
			this.a.BackColor = System.Drawing.SystemColors.Control;
			this.a.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.a.ForeColor = System.Drawing.SystemColors.InfoText;
			this.a.Location = new System.Drawing.Point(24, 224);
			this.a.Multiline = true;
			this.a.Name = "a";
			this.a.ReadOnly = true;
			this.a.Size = new System.Drawing.Size(272, 64);
			this.a.TabIndex = 7;
			this.a.Text = "�ݒ�̓Q�[���f�[�^�ł͂Ȃ�FreeTrain�̊��ݒ�t�@�C��\r\n�ɕۑ�����܂��B�������ċN�����Ă��u�ݒ�Ǎ��v�{�^����\r\n�����܂ł́A�f�t�H���g�l�ɂȂ��Ă��܂�" +
				"�B\r\n\r\n�����ݒ�l����������A���񍐂����肢���܂��B(c477)";
			// 
			// DevelopConfigure
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(602, 323);
			this.Controls.Add(this.a);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnLoad);
			this.Name = "DevelopConfigure";
			this.Text = "���W�A���S���Y���F�p�����[�^";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e) {
			Dispose();
		}

		private void btnApply_Click(object sender, System.EventArgs e) {
			try{
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
			}finally{
				Dispose();
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e) {
			int i=0;
			Core.options.devParams[i++] = double.Parse(tbLandPliceScale.Text);
			Core.options.devParams[i++] = double.Parse(tbMaxPricePower.Text);
			Core.options.devParams[i++] = double.Parse(tbStrDiffuse.Text);
			Core.options.devParams[i++] = double.Parse(tbReplacePriceFactor.Text);
			Core.options.devParams[i++] = double.Parse(tbPopAmpScale.Text);
			Core.options.devParams[i++] = double.Parse(tbPopAmpPower.Text);

			Core.options.devParams[i++] = float.Parse(tbQAlpha.Text);
			Core.options.devParams[i++] = int.Parse(tbAddedQScale.Text);
			Core.options.devParams[i++] = double.Parse(tbLandValuePower.Text);
			Core.options.devParams[i++] = float.Parse(tbQDiffuse.Text);
			Core.options.devParams[i++] = float.Parse(tbBaseRho.Text);		
		}

		private void btnLoad_Click(object sender, System.EventArgs e) {
			int i=0;
			tbLandPliceScale.Text =  string.Format("{0:0.00}",Core.options.devParams[i++]);
			tbMaxPricePower.Text = string.Format("{0:0.00}",Core.options.devParams[i++]);
			tbStrDiffuse.Text = string.Format("{0:0.00}",Core.options.devParams[i++]);
			tbReplacePriceFactor.Text = string.Format("{0:0.00}",Core.options.devParams[i++]);
			tbPopAmpScale.Text = string.Format("{0:0.00}",Core.options.devParams[i++]);
			tbPopAmpPower.Text = string.Format("{0:0.00}",Core.options.devParams[i++]);

			tbQAlpha.Text = string.Format("{0:0.000}",Core.options.devParams[i++]);
			tbAddedQScale.Text = string.Format("{0}",(int)Core.options.devParams[i++]);
			tbLandValuePower.Text = string.Format("{0:0.00}",Core.options.devParams[i++]);
			tbQDiffuse.Text = string.Format("{0:0.000}",Core.options.devParams[i++]);
			tbBaseRho.Text = string.Format("{0:0.00}",Core.options.devParams[i++]);
		}
	}
}

