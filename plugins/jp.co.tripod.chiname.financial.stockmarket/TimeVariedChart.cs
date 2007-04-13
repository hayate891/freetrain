using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace freetrain.finance.stock
{
	/// <summary>
	/// TimeVariedChart �̊T�v�̐����ł��B
	/// </summary>
	public class TimeVariedChart : System.Windows.Forms.UserControl
	{		
		private Panel panel1;
		private Label label1;
		private ChartControl timeChart;
		public ChartControl chart { get { return timeChart; } }
		private XAxisStyle[] scales;
		private int scale = 0;
		private System.Windows.Forms.Button btnMinus;
		private System.Windows.Forms.Button btnPlus;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TimeVariedChart()
		{
			// ���̌Ăяo���́AWindows.Forms �t�H�[�� �f�U�C�i�ŕK�v�ł��B
			InitializeComponent();
			setScaleArray(null);
			writeLabel();
		}

		public void setScaleArray( XAxisStyle[] array )
		{
			if( array == null || array.Length < 1)
				scales = new XAxisStyle[]{ XAxisStyle.DAILY };
			else
				scales = array;
			chart.ScaleTypeX = scales[0];
			bool b =( scales.Length > 1 );
			btnMinus.Visible = b;
			btnPlus.Visible = b;
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.timeChart = new freetrain.finance.stock.ChartControl();
			this.btnMinus = new System.Windows.Forms.Button();
			this.btnPlus = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.timeChart,
																				 this.btnMinus,
																				 this.btnPlus,
																				 this.label1});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(128, 160);
			this.panel1.TabIndex = 7;
			// 
			// timeChart
			// 
			this.timeChart.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.timeChart.Name = "timeChart";
			this.timeChart.Size = new System.Drawing.Size(124, 138);
			this.timeChart.TabIndex = 1;
			// 
			// btnMinus
			// 
			this.btnMinus.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnMinus.Location = new System.Drawing.Point(0, 140);
			this.btnMinus.Name = "btnMinus";
			this.btnMinus.Size = new System.Drawing.Size(16, 16);
			this.btnMinus.TabIndex = 3;
			this.btnMinus.Text = "�|";
			this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
			// 
			// btnPlus
			// 
			this.btnPlus.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnPlus.Location = new System.Drawing.Point(108, 140);
			this.btnPlus.Name = "btnPlus";
			this.btnPlus.Size = new System.Drawing.Size(16, 16);
			this.btnPlus.TabIndex = 3;
			this.btnPlus.Text = "�{";
			this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.label1.Location = new System.Drawing.Point(18, 140);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// TimeVariedChart
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1});
			this.Name = "TimeVariedChart";
			this.Size = new System.Drawing.Size(128, 160);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnPlus_Click(object sender, System.EventArgs e) {
			if( scale < scales.Length-1 ) {
				scale++;
				writeLabel();
			}
		}

		private void btnMinus_Click(object sender, System.EventArgs e) {
			if( scale > 0 ) {
				scale--;		
				writeLabel();
			}		
		}

		private void writeLabel() {
			label1.Text = scales[scale].ToString();
			chart.ScaleTypeX = scales[scale];
			chart.Invalidate();
		}
	}
}
