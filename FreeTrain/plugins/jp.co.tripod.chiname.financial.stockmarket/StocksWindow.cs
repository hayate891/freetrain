using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace freetrain.finance.stock
{
	/// <summary>
	/// StocksWindow �̊T�v�̐����ł��B
	/// </summary>
	public class StocksWindow : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView listview;
		private System.Windows.Forms.Button btn_counter;
		private System.Windows.Forms.Label tl_status;
		private freetrain.finance.stock.TimeVariedChart chart;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;
		static private StockCompanyModule market { get { return StockCompanyModule.theInstance; } }
		private MarketBusinessHourListener marketListener;

		public StocksWindow()
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();
			StocksListHelper.buildRetainingList( listview );
			onMarketStatusChanged();
			
			marketListener =  new MarketBusinessHourListener(onMarketStatusChanged);
			market.onBusinesStatusChanging += marketListener;
			chart.chart.ScaleTypeX = XAxisStyle.DAILY;
			chart.chart.ScaleTypeY = YAxisStyle.AUTOSCALE;
			chart.chart.area.setYRange(0,15000);
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			market.onBusinesStatusChanging -= marketListener;
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.listview = new System.Windows.Forms.ListView();
			this.btn_counter = new System.Windows.Forms.Button();
			this.tl_status = new System.Windows.Forms.Label();
			this.chart = new freetrain.finance.stock.TimeVariedChart();
			this.SuspendLayout();
			// 
			// listview
			// 
			this.listview.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.listview.FullRowSelect = true;
			this.listview.GridLines = true;
			this.listview.HideSelection = false;
			this.listview.Location = new System.Drawing.Point(2, 0);
			this.listview.MultiSelect = false;
			this.listview.Name = "listview";
			this.listview.Size = new System.Drawing.Size(470, 200);
			this.listview.TabIndex = 3;
			this.listview.View = System.Windows.Forms.View.Details;
			this.listview.SelectedIndexChanged += new System.EventHandler(this.listview_SelectedIndexChanged);
			// 
			// btn_counter
			// 
			this.btn_counter.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btn_counter.Location = new System.Drawing.Point(544, 3);
			this.btn_counter.Name = "btn_counter";
			this.btn_counter.Size = new System.Drawing.Size(72, 23);
			this.btn_counter.TabIndex = 5;
			this.btn_counter.Text = "�������";
			this.btn_counter.Click += new System.EventHandler(this.btn_counter_Click);
			// 
			// tl_status
			// 
			this.tl_status.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.tl_status.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_status.Location = new System.Drawing.Point(480, 3);
			this.tl_status.Name = "tl_status";
			this.tl_status.Size = new System.Drawing.Size(56, 23);
			this.tl_status.TabIndex = 6;
			this.tl_status.Text = "label2";
			this.tl_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chart
			// 
			this.chart.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.chart.Location = new System.Drawing.Point(480, 32);
			this.chart.Name = "chart";
			this.chart.Size = new System.Drawing.Size(136, 168);
			this.chart.TabIndex = 7;
			// 
			// StocksWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(624, 205);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.chart,
																		  this.tl_status,
																		  this.btn_counter,
																		  this.listview});
			this.MinimumSize = new System.Drawing.Size(336, 232);
			this.Name = "StocksWindow";
			this.Text = "���L�����ꗗ";
			this.ResumeLayout(false);

		}
		#endregion

		private void btn_counter_Click(object sender, System.EventArgs e)
		{
			StockMarketForm.ShowForm();		
		}

		private void listview_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			chart.chart.removeDataSourceAt(0);
			if( listview.SelectedItems.Count != 0 )
			{
				ListViewItem item = listview.SelectedItems[0];
				Company com = (Company)item.Tag;
				chart.chart.addDataSource(com.stockData,Color.Blue);
			}
			chart.chart.calcRange();
			chart.chart.Invalidate();
		}

		private void onMarketStatusChanged()
		{
			switch( StockCompanyModule.theInstance.status ) 
			{
				case MarketStatus.HOLIDAY:
					tl_status.Text = "�x��";
					break;
				case MarketStatus.OPEN:
					tl_status.Text = "�c�ƒ�";
					break;
				case MarketStatus.CLOSE:
					tl_status.Text = "���ԊO";
					break;
			}
		}
	}
}
