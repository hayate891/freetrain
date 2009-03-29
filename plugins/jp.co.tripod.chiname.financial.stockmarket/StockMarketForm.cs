using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.framework;
using freetrain.util;
using freetrain.world;
using freetrain.world.accounting;

namespace freetrain.finance.stock
{
	/// <summary>
	/// StockMarketForm の概要の説明です。
	/// </summary>
	public class StockMarketForm : Form
	{
		#region generated by form designer
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnBuy;
		private System.Windows.Forms.Label tl_totStockBuy;
		private freetrain.finance.stock.NumberEditEx numberBuy;
		private System.Windows.Forms.Label tl_totCapital;
		private System.Windows.Forms.Label tl_totExpense;
		private System.Windows.Forms.Label tl_commitionBuy;
		private freetrain.finance.stock.NumberEditEx numberSell;
		private System.Windows.Forms.Button btnSell;
		private System.Windows.Forms.Label tl_totStockSell;
		private System.Windows.Forms.Label tl_priceBought;
		private System.Windows.Forms.Label tl_commitionSell;
		private System.Windows.Forms.ListView listview;
		private System.Windows.Forms.Label tl_benefit;
		private System.Windows.Forms.Label tl_totIncom;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private freetrain.finance.stock.TimeVariedChart chart;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		#endregion
		private System.ComponentModel.Container components = null;
		static private StockCompanyModule market { get { return StockCompanyModule.theInstance; } }
		static private AccountManager manager { get { return World.world.account; } }

		private Company current = Company.Null;
		private NumberChangeListener forBuyEdit;
		private NumberChangeListener forSellEdit;
		private AccountListener listener;
		static private Rectangle bounds = new Rectangle();
		static private StockMarketForm theInstance = null;

		static public void ShowForm()
		{
			if( theInstance == null )
				theInstance = new StockMarketForm();
			theInstance.ShowDialog(MainWindow.mainWindow);
		}

		public StockMarketForm()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();
			if( StockMarketConfig.debug )
				this.ClientSize = new Size(850, 381);
			forBuyEdit = new NumberChangeListener(onBuyEditChanged);
			numberBuy.onNumberChanged += forBuyEdit;
			forSellEdit = new NumberChangeListener(onSellEditChanged);
			numberSell.onNumberChanged += forSellEdit;
			numberSell.numberMax = 0;
			StocksListHelper.buildStockMarketList( listview );
			listener = new AccountListener(onAccountChange);
			AccountManager.onAccountChange+=listener;
			onAccountChange();

			chart.chart.ScaleTypeX = XAxisStyle.DAILY;
			chart.setScaleArray( new XAxisStyle[2]{XAxisStyle.DAILY,XAxisStyle.MONTHLY} );
			chart.chart.ScaleTypeY = YAxisStyle.AUTOSCALE;
			chart.chart.area.setYRange(0,15000);
			//chart.Invalidate();
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{			
			AccountManager.onAccountChange-=listener;
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
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockMarketForm));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.btnBuy = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.tl_totStockBuy = new System.Windows.Forms.Label();
			this.numberBuy = new freetrain.finance.stock.NumberEditEx();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.tl_totCapital = new System.Windows.Forms.Label();
			this.tl_totExpense = new System.Windows.Forms.Label();
			this.tl_commitionBuy = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tl_totIncom = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.numberSell = new freetrain.finance.stock.NumberEditEx();
			this.btnSell = new System.Windows.Forms.Button();
			this.tl_totStockSell = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tl_priceBought = new System.Windows.Forms.Label();
			this.tl_benefit = new System.Windows.Forms.Label();
			this.tl_commitionSell = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.listview = new System.Windows.Forms.ListView();
			this.chart = new freetrain.finance.stock.TimeVariedChart();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(418, 104);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(242, 274);
			this.tabControl1.TabIndex = 4;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.btnBuy);
			this.tabPage1.Controls.Add(this.label11);
			this.tabPage1.Controls.Add(this.tl_totStockBuy);
			this.tabPage1.Controls.Add(this.numberBuy);
			this.tabPage1.Controls.Add(this.label13);
			this.tabPage1.Controls.Add(this.label14);
			this.tabPage1.Controls.Add(this.tl_totCapital);
			this.tabPage1.Controls.Add(this.tl_totExpense);
			this.tabPage1.Controls.Add(this.tl_commitionBuy);
			this.tabPage1.Controls.Add(this.label18);
			this.tabPage1.Controls.Add(this.label19);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(234, 248);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Buy";
			//! this.tabPage1.Text = "購入";
			// 
			// btnBuy
			// 
			this.btnBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnBuy.Location = new System.Drawing.Point(148, 210);
			this.btnBuy.Name = "btnBuy";
			this.btnBuy.Size = new System.Drawing.Size(80, 25);
			this.btnBuy.TabIndex = 17;
			this.btnBuy.Text = "Buy";
			//! this.btnBuy.Text = "購入";
			this.btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
			// 
			// label11
			// 
			this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label11.Location = new System.Drawing.Point(4, 85);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(116, 21);
			this.label11.TabIndex = 14;
			this.label11.Text = "Purchase Size (100\'s)";
			//! this.label11.Text = "購入数(百株)：";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tl_totStockBuy
			// 
			this.tl_totStockBuy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_totStockBuy.Location = new System.Drawing.Point(97, 11);
			this.tl_totStockBuy.Name = "tl_totStockBuy";
			this.tl_totStockBuy.Size = new System.Drawing.Size(131, 17);
			this.tl_totStockBuy.TabIndex = 13;
			this.tl_totStockBuy.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numberBuy
			// 
			this.numberBuy.Location = new System.Drawing.Point(7, 63);
			this.numberBuy.Name = "numberBuy";
			this.numberBuy.number = 0;
			this.numberBuy.Size = new System.Drawing.Size(225, 70);
			this.numberBuy.TabIndex = 7;
			// 
			// label13
			// 
			this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label13.Location = new System.Drawing.Point(4, 11);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(183, 17);
			this.label13.TabIndex = 16;
			this.label13.Text = "Shares held:";
			//! this.label13.Text = "保有株数：";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label14
			// 
			this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label14.Location = new System.Drawing.Point(4, 37);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(56, 17);
			this.label14.TabIndex = 15;
			this.label14.Text = "Funds:";
			//! this.label14.Text = "資金：";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tl_totCapital
			// 
			this.tl_totCapital.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_totCapital.Location = new System.Drawing.Point(97, 37);
			this.tl_totCapital.Name = "tl_totCapital";
			this.tl_totCapital.Size = new System.Drawing.Size(131, 17);
			this.tl_totCapital.TabIndex = 9;
			this.tl_totCapital.Text = "0";
			this.tl_totCapital.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// tl_totExpense
			// 
			this.tl_totExpense.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_totExpense.Location = new System.Drawing.Point(97, 184);
			this.tl_totExpense.Name = "tl_totExpense";
			this.tl_totExpense.Size = new System.Drawing.Size(131, 17);
			this.tl_totExpense.TabIndex = 8;
			this.tl_totExpense.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// tl_commitionBuy
			// 
			this.tl_commitionBuy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_commitionBuy.Location = new System.Drawing.Point(97, 158);
			this.tl_commitionBuy.Name = "tl_commitionBuy";
			this.tl_commitionBuy.Size = new System.Drawing.Size(131, 17);
			this.tl_commitionBuy.TabIndex = 12;
			this.tl_commitionBuy.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label18
			// 
			this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label18.Location = new System.Drawing.Point(4, 158);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(143, 17);
			this.label18.TabIndex = 11;
			this.label18.Text = "Commission:";
			//! this.label18.Text = "手数料：";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label19
			// 
			this.label19.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label19.Location = new System.Drawing.Point(4, 184);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(183, 17);
			this.label19.TabIndex = 10;
			this.label19.Text = "Payment:";
			//! this.label19.Text = "支払：";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.tl_priceBought);
			this.tabPage2.Controls.Add(this.tl_totIncom);
			this.tabPage2.Controls.Add(this.label2);
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Controls.Add(this.numberSell);
			this.tabPage2.Controls.Add(this.btnSell);
			this.tabPage2.Controls.Add(this.tl_totStockSell);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.tl_benefit);
			this.tabPage2.Controls.Add(this.tl_commitionSell);
			this.tabPage2.Controls.Add(this.label9);
			this.tabPage2.Controls.Add(this.label10);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(234, 248);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Sell";
			//! this.tabPage2.Text = "売却";
			// 
			// tl_totIncom
			// 
			this.tl_totIncom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_totIncom.Location = new System.Drawing.Point(97, 184);
			this.tl_totIncom.Name = "tl_totIncom";
			this.tl_totIncom.Size = new System.Drawing.Size(131, 17);
			this.tl_totIncom.TabIndex = 18;
			this.tl_totIncom.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label2.Location = new System.Drawing.Point(4, 184);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(83, 17);
			this.label2.TabIndex = 17;
			this.label2.Text = "Income:";
			//! this.label2.Text = "収入：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label6.Location = new System.Drawing.Point(4, 85);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(108, 21);
			this.label6.TabIndex = 16;
			this.label6.Text = "Size of sale (100\'s)";
			//! this.label6.Text = "売却数(百株)：";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numberSell
			// 
			this.numberSell.Location = new System.Drawing.Point(7, 63);
			this.numberSell.Name = "numberSell";
			this.numberSell.number = 0;
			this.numberSell.Size = new System.Drawing.Size(225, 70);
			this.numberSell.TabIndex = 15;
			// 
			// btnSell
			// 
			this.btnSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnSell.Location = new System.Drawing.Point(148, 210);
			this.btnSell.Name = "btnSell";
			this.btnSell.Size = new System.Drawing.Size(80, 25);
			this.btnSell.TabIndex = 6;
			this.btnSell.Text = "Sell";
			//! this.btnSell.Text = "売却";
			this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
			// 
			// tl_totStockSell
			// 
			this.tl_totStockSell.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_totStockSell.Location = new System.Drawing.Point(97, 11);
			this.tl_totStockSell.Name = "tl_totStockSell";
			this.tl_totStockSell.Size = new System.Drawing.Size(131, 17);
			this.tl_totStockSell.TabIndex = 5;
			this.tl_totStockSell.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label3.Location = new System.Drawing.Point(4, 11);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(183, 17);
			this.label3.TabIndex = 5;
			this.label3.Text = "Shares held:";
			//! this.label3.Text = "保有株数：";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label4.Location = new System.Drawing.Point(4, 37);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(133, 17);
			this.label4.TabIndex = 5;
			this.label4.Text = "Price per Share:";
			//! this.label4.Text = "購入額／株：";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tl_priceBought
			// 
			this.tl_priceBought.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_priceBought.Location = new System.Drawing.Point(97, 37);
			this.tl_priceBought.Name = "tl_priceBought";
			this.tl_priceBought.Size = new System.Drawing.Size(131, 17);
			this.tl_priceBought.TabIndex = 5;
			this.tl_priceBought.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// tl_benefit
			// 
			this.tl_benefit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_benefit.Location = new System.Drawing.Point(97, 136);
			this.tl_benefit.Name = "tl_benefit";
			this.tl_benefit.Size = new System.Drawing.Size(132, 17);
			this.tl_benefit.TabIndex = 5;
			this.tl_benefit.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// tl_commitionSell
			// 
			this.tl_commitionSell.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_commitionSell.Location = new System.Drawing.Point(97, 158);
			this.tl_commitionSell.Name = "tl_commitionSell";
			this.tl_commitionSell.Size = new System.Drawing.Size(132, 17);
			this.tl_commitionSell.TabIndex = 5;
			this.tl_commitionSell.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label9
			// 
			this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label9.Location = new System.Drawing.Point(4, 158);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(91, 17);
			this.label9.TabIndex = 5;
			this.label9.Text = "Commission:";
			//! this.label9.Text = "手数料：";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label10.Location = new System.Drawing.Point(4, 136);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(91, 17);
			this.label10.TabIndex = 5;
			this.label10.Text = "Profit per share:";
			//! this.label10.Text = "利益／株：";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// listview
			// 
			this.listview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listview.FullRowSelect = true;
			this.listview.GridLines = true;
			this.listview.HideSelection = false;
			this.listview.Location = new System.Drawing.Point(0, 0);
			this.listview.MultiSelect = false;
			this.listview.Name = "listview";
			this.listview.Size = new System.Drawing.Size(662, 99);
			this.listview.TabIndex = 5;
			this.listview.UseCompatibleStateImageBehavior = false;
			this.listview.View = System.Windows.Forms.View.Details;
			this.listview.SelectedIndexChanged += new System.EventHandler(this.listview_SelectedIndexChanged);
			// 
			// chart
			// 
			this.chart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.chart.BackColor = System.Drawing.SystemColors.Control;
			this.chart.Location = new System.Drawing.Point(0, 104);
			this.chart.Name = "chart";
			this.chart.Size = new System.Drawing.Size(412, 272);
			this.chart.TabIndex = 6;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(2, 380);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(64, 16);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 18;
			this.pictureBox1.TabStop = false;
			// 
			// StockMarketForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(662, 399);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.chart);
			this.Controls.Add(this.listview);
			this.Controls.Add(this.tabControl1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(344, 433);
			this.Name = "StockMarketForm";
			this.ShowInTaskbar = false;
			this.Text = "Stock Market";
			//! this.Text = "株式市場";
			this.VisibleChanged += new System.EventHandler(this.StockMarketForm_VisibleChanged);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region readonly numelic properties
		public long totalBuyPrice 
		{
			get	{
				return ((long)numberBuy.number) * current.stockData.currentPriceU;
			}
		}

		public int commitionToBuy {
			get	{
				return StockCompanyModule.calcCommition(current.stockData.currentPriceU,numberBuy.number);
			}
		}

		public long totalSellPrice {
			get	{
				return ((long)numberSell.number) * current.stockData.currentPriceU;
			}
		}

		public int commitionToSell {
			get	{
				return StockCompanyModule.calcCommition(current.stockData.currentPriceU,numberSell.number);
			}
		}

		public long totalExpense {
			get	{
				return totalBuyPrice+commitionToBuy;
			}
		}

		public long totalIncom {
			get	{
				return totalSellPrice-commitionToSell;
			}
		}
		#endregion

		private void listview_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			chart.chart.removeDataSourceAt(0);
			if( listview.SelectedItems.Count == 0 )
			{
				tl_totStockBuy.Text = "";
				tl_totStockSell.Text = "";
				tl_benefit.Text = "";
				tl_commitionBuy.Text = "";
				tl_commitionSell.Text = "";
				tl_priceBought.Text = "";
				tl_totExpense.Text = "";
				tl_totIncom.Text = "";
				numberBuy.numberMax = 0;
				numberSell.numberMax = 0;
				current = Company.Null;
			}
			else
			{
				ListViewItem item = listview.SelectedItems[0];
				current = (Company)item.Tag;

				int n = current.stockData.numOwn;
				tl_totStockSell.Text = tl_totStockBuy.Text = StocksListHelper.formatNum(n);
				onBuyEditChanged();
				onSellEditChanged();
				tl_benefit.Text = CurrencyUtil.format(current.stockData.benefitPerStock);
				tl_priceBought.Text = CurrencyUtil.format(current.stockData.averageBoughtPrice);
				numberBuy.numberMax = StockCompanyModule.calcBuyableStocks(
									current.stockData, AccountManager.theInstance.liquidAssets);
				numberSell.numberMax = current.stockData.numOwn;

				chart.chart.addDataSource(current.stockData,Color.Blue);

			}
			chart.chart.calcRange();
			chart.chart.Invalidate();
		}

		public void onBuyEditChanged() {
			if( current != Company.Null ) {
				tl_commitionBuy.Text = CurrencyUtil.format(commitionToBuy);
				tl_totExpense.Text = CurrencyUtil.format(totalExpense);
			}
		}
		
		public void onSellEditChanged() {
			if( current != Company.Null ) {
				tl_commitionSell.Text = CurrencyUtil.format(commitionToSell);
				tl_totIncom.Text = CurrencyUtil.format(totalIncom);
			}
		}

		public void onAccountChange() {
			tl_totCapital.Text = CurrencyUtil.format(manager.liquidAssets);
		}

		private void btnBuy_Click(object sender, System.EventArgs e) {
			if( numberBuy.number > 0 ) {
				market.buy( current, numberBuy.number ); 
				listview_SelectedIndexChanged( listview, null );
			}
		}

		private void btnSell_Click(object sender, System.EventArgs e)
		{
			if( numberSell.number > 0 ) {
				market.sell( current, numberSell.number ); 		
				listview_SelectedIndexChanged( listview, null );
			}
		}

		private void StockMarketForm_VisibleChanged(object sender, System.EventArgs e)
		{
			if( Visible )
			{
				if( !bounds.IsEmpty ) 
				{
					Location = bounds.Location;
					Size = bounds.Size;
				}
			}
			else
				bounds=new Rectangle(Location,Size);
		}
	}
}