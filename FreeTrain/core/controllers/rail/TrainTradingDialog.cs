using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.contributions.train;
using freetrain.framework;
using freetrain.framework.graphics;
using freetrain.framework.plugin;
using freetrain.views;
using freetrain.world.accounting;

namespace freetrain.world.rail
{
	/// <summary>
	/// Dialog box to buy trains
	/// </summary>
	public class TrainTradingDialog : Form
	{
		public TrainTradingDialog() {
			InitializeComponent();
			//handler = new OptionChangedHandler(updatePreview);
			World.world.viewOptions.OnViewOptionChanged+=new OptionChangedHandler(updatePreview);
			Bitmap bmp = ResourceUtil.loadSystemBitmap("DayNight.bmp");
			buttonImages.TransparentColor = bmp.GetPixel(0,0);
			buttonImages.Images.AddStrip(bmp);
			
			tbDay.Pushed=(World.world.viewOptions.nightSpriteMode==NightSpriteMode.AlwaysDay);
			tbNight.Pushed=(World.world.viewOptions.nightSpriteMode==NightSpriteMode.AlwaysNight);

			// organize trains into a tree
			IDictionary types = new SortedList();
			foreach( TrainContribution tc in Core.plugins.trains ) {
				IDictionary company = (IDictionary)types[tc.companyName];
				if( company==null )
					types[tc.companyName] = company = new SortedList();

				IDictionary type = (IDictionary)company[tc.typeName];
				if( type==null )
					company[tc.typeName] = type = new SortedList();

				type.Add( tc.nickName, tc );
			}

			// build a tree
			foreach( DictionaryEntry company in types ) {
				TreeNode cn = new TreeNode((string)company.Key);
				typeTree.Nodes.Add(cn);

				foreach( DictionaryEntry type in (IDictionary)company.Value ) {
					IDictionary t = (IDictionary)type.Value;
					if(t.Count==1) {
						addTrains( cn, t );
					} else {
						TreeNode tn = new TreeNode((string)type.Key);
						cn.Nodes.Add(tn);

						addTrains( tn, t );
					}
				}
			}

			onTypeChanged(null,null);
		}

		private void addTrains( TreeNode parent, IDictionary list ) {
			foreach( DictionaryEntry trainEntry in list ) {
				TrainContribution t = (TrainContribution)trainEntry.Value;

				TreeNode trainNode = new TreeNode(t.name);
				trainNode.Tag = t;
				parent.Nodes.Add(trainNode);
			}
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

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
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.length)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.preview)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.count)).BeginInit();
			this.SuspendLayout();
			
			//translations.
			//! this.label10.Text = "名称：";
			//! this.label7.Text = "両";
			//! this.author.Text = "477さん";
			//! this.label6.Text = "説明：";
			//! this.label1.Text = "作者：";
			//! this.label8.Text = "総費用：";
			//! this.buttonOK.Text = "購入(&O)";
			//! this.buttonCancel.Text = "閉じる(&C)";
			//! this.label5.Text = "速度：";
			// this.speed.Text = "高速";
			//! this.label4.Text = "編成";
			
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(9, 273);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 22);
			this.label2.TabIndex = 28;
			this.label2.Text = "&Length:";
			//! this.label2.Text = "車両数(&L)：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// typeTree
			// 
			this.typeTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.typeTree.Location = new System.Drawing.Point(0, 0);
			this.typeTree.Name = "typeTree";
			this.typeTree.Size = new System.Drawing.Size(203, 387);
			this.typeTree.TabIndex = 18;
			this.typeTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.onTypeChanged);
			// 
			// buttonImages
			// 
			this.buttonImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.buttonImages.ImageSize = new System.Drawing.Size(16, 15);
			this.buttonImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.length);
			this.groupBox1.Controls.Add(this.toolBarDayNight);
			this.groupBox1.Controls.Add(this.preview);
			this.groupBox1.Controls.Add(this.name);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.author);
			this.groupBox1.Controls.Add(this.description);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.passenger);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.totalPrice);
			this.groupBox1.Controls.Add(this.speed);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.buttonCancel);
			this.groupBox1.Controls.Add(this.buttonOK);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.count);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(203, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(398, 387);
			this.groupBox1.TabIndex = 19;
			this.groupBox1.TabStop = false;
			// 
			// length
			// 
			this.length.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.length.Location = new System.Drawing.Point(75, 275);
			this.length.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.length.Name = "length";
			this.length.Size = new System.Drawing.Size(64, 20);
			this.length.TabIndex = 50;
			this.length.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.length.Value = new decimal(new int[] {
									3,
									0,
									0,
									0});
			// 
			// toolBarDayNight
			// 
			this.toolBarDayNight.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
									this.tbDay,
									this.tbNight});
			this.toolBarDayNight.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBarDayNight.DropDownArrows = true;
			this.toolBarDayNight.ImageList = this.buttonImages;
			this.toolBarDayNight.Location = new System.Drawing.Point(33, 35);
			this.toolBarDayNight.Name = "toolBarDayNight";
			this.toolBarDayNight.ShowToolTips = true;
			this.toolBarDayNight.Size = new System.Drawing.Size(38, 48);
			this.toolBarDayNight.TabIndex = 49;
			// 
			// tbDay
			// 
			this.tbDay.ImageIndex = 1;
			this.tbDay.Name = "tbDay";
			this.tbDay.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbDay.Tag = freetrain.views.NightSpriteMode.AlwaysDay;
			// 
			// tbNight
			// 
			this.tbNight.ImageIndex = 2;
			this.tbNight.Name = "tbNight";
			this.tbNight.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbNight.Tag = freetrain.views.NightSpriteMode.AlwaysNight;
			// 
			// preview
			// 
			this.preview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.preview.Location = new System.Drawing.Point(77, 35);
			this.preview.Name = "preview";
			this.preview.Size = new System.Drawing.Size(302, 78);
			this.preview.TabIndex = 48;
			this.preview.TabStop = false;
			// 
			// name
			// 
			this.name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.name.Location = new System.Drawing.Point(79, 9);
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(300, 17);
			this.name.TabIndex = 47;
			this.name.Text = "123 series ABCDEF";
			//! this.name.Text = "123系 ABCDEF";
			this.name.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(9, 9);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 17);
			this.label10.TabIndex = 46;
			this.label10.Text = "Name:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label7.Location = new System.Drawing.Point(149, 273);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(27, 20);
			this.label7.TabIndex = 45;
			this.label7.Text = "Car";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// author
			// 
			this.author.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.author.Location = new System.Drawing.Point(77, 121);
			this.author.Name = "author";
			this.author.Size = new System.Drawing.Size(302, 18);
			this.author.TabIndex = 44;
			this.author.Text = "477";
			this.author.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// description
			// 
			this.description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.description.BackColor = System.Drawing.SystemColors.Control;
			this.description.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.description.Location = new System.Drawing.Point(79, 147);
			this.description.Multiline = true;
			this.description.Name = "description";
			this.description.ReadOnly = true;
			this.description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.description.Size = new System.Drawing.Size(300, 97);
			this.description.TabIndex = 43;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(9, 143);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 18);
			this.label6.TabIndex = 42;
			this.label6.Text = "Description:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(9, 121);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 18);
			this.label1.TabIndex = 41;
			this.label1.Text = "Author:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// passenger
			// 
			this.passenger.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.passenger.Location = new System.Drawing.Point(75, 329);
			this.passenger.Name = "passenger";
			this.passenger.Size = new System.Drawing.Size(304, 18);
			this.passenger.TabIndex = 40;
			this.passenger.Text = "100";
			this.passenger.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label9.Location = new System.Drawing.Point(7, 329);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(64, 18);
			this.label9.TabIndex = 39;
			this.label9.Text = "Capacity:";
			//! this.label9.Text = "定員：";
			this.label9.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label8.Location = new System.Drawing.Point(7, 311);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(64, 18);
			this.label8.TabIndex = 38;
			this.label8.Text = "Total cost:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// totalPrice
			// 
			this.totalPrice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.totalPrice.Location = new System.Drawing.Point(75, 311);
			this.totalPrice.Name = "totalPrice";
			this.totalPrice.Size = new System.Drawing.Size(309, 18);
			this.totalPrice.TabIndex = 37;
			this.totalPrice.Text = "100,000";
			this.totalPrice.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// speed
			// 
			this.speed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.speed.Location = new System.Drawing.Point(79, 247);
			this.speed.Name = "speed";
			this.speed.Size = new System.Drawing.Size(300, 17);
			this.speed.TabIndex = 36;
			this.speed.Text = "Rapid";
			this.speed.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.Location = new System.Drawing.Point(9, 247);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 17);
			this.label5.TabIndex = 35;
			this.label5.Text = "Speed:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(304, 355);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(80, 26);
			this.buttonCancel.TabIndex = 34;
			this.buttonCancel.Text = "&Close";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(218, 355);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(80, 26);
			this.buttonOK.TabIndex = 33;
			this.buttonOK.Text = "&Buy";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(272, 273);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(27, 20);
			this.label4.TabIndex = 32;
			this.label4.Text = "Set";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(-104, 304);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(557, 4);
			this.groupBox2.TabIndex = 29;
			this.groupBox2.TabStop = false;
			// 
			// count
			// 
			this.count.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.count.Location = new System.Drawing.Point(201, 275);
			this.count.Name = "count";
			this.count.Size = new System.Drawing.Size(63, 20);
			this.count.TabIndex = 31;
			this.count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.count.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(182, 274);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(25, 19);
			this.label3.TabIndex = 30;
			this.label3.Text = "x";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TrainTradingDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(601, 387);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.typeTree);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TrainTradingDialog";
			this.Text = "Buy trains";
			//! this.Text = "車両の購入";
			this.Resize += new System.EventHandler(this.updateAfterResize);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.length)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.preview)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.count)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.GroupBox groupBox2;
		#endregion

		protected virtual void updateAfterResize(object sender, System.EventArgs e){
			this.typeTree.Width = this.Width / 3;
			this.groupBox1.Width = this.ClientSize.Width - this.typeTree.Width;
			this.groupBox1.Left = this.typeTree.Width;
			updatePreview();
		}
		
		private TrainContribution selectedTrain {
			get {
				TreeNode n = typeTree.SelectedNode;
				if(n==null)	return null;
				return (TrainContribution)n.Tag;
			}
		}

		private long getTotalPrice() {
			return (long)(selectedTrain.price(1) * length.Value * count.Value);
		}

		private void onTypeChanged(object sender, System.Windows.Forms.TreeViewEventArgs e) {
			updatePreview();
		}

		public void updatePreview() 
		{
			length.Enabled = count.Enabled = buttonOK.Enabled = (selectedTrain!=null);

			Image im = preview.Image;
			if(im!=null) {
				preview.Image = null;
				im.Dispose();
			}

			if( selectedTrain!=null ) {
				name.Text = selectedTrain.name;
				author.Text = selectedTrain.author;
				description.Text = selectedTrain.description;
				speed.Text = selectedTrain.speedDisplayName;
				using( PreviewDrawer pd = selectedTrain.createPreview( preview.ClientSize ) ) {
					preview.Image = pd.createBitmap();
				}

				if( count.Value==0 )
					// if the user changes the type, s/he is going to buy another train.
					// thus change the value to 1.
					count.Value=1;

				onAmountChanged(null,null);
			} else {
				name.Text = author.Text = description.Text = speed.Text = "";
			}
		}

		private void onAmountChanged(object sender, EventArgs e) {
			if( count.Value!=0 && selectedTrain!=null ) {
				TrainCarContribution[] cars = selectedTrain.create((int)length.Value);
				if( cars!=null ) {
					buttonOK.Enabled = true;

					// TODO: non-linear price support
					totalPrice.Text = getTotalPrice().ToString();

					int p=0;
					foreach( TrainCarContribution car in cars )
						p += car.capacity;

					passenger.Text = p.ToString()+" Passenger/Formation";
					//! passenger.Text = p.ToString()+" 人/編成";
					return;
				}
			}

			buttonOK.Enabled = false;
			totalPrice.Text = "---";
			passenger.Text = "---";

		}

		private void buttonOK_Click(object sender, EventArgs e) {
			// buy trains
			for( int i=0; i<(int)count.Value; i++ )
				new Train( World.world.rootTrainGroup,
					(int)length.Value, selectedTrain );

			freetrain.framework.sound.SoundEffectManager
				.PlaySynchronousSound( ResourceUtil.findSystemResource("vehiclePurchase.wav") );
			
			AccountManager.theInstance.spend( getTotalPrice(), AccountGenre.RAIL_SERVICE );

			// set count to 0 to avoid accidental purchase
			count.Value=0;
		}

		private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			foreach( ToolBarButton tb in toolBarDayNight.Buttons)
			{
				if(e.Button == tb)
				{
					if(tb.Pushed)
						World.world.viewOptions.nightSpriteMode = (NightSpriteMode)tb.Tag;
					else
						World.world.viewOptions.nightSpriteMode = NightSpriteMode.AlignClock;
				}
				else
				{
					tb.Pushed = false;
				}
			}
		}

		private void TrainTradingDialog_Closed(object sender, System.EventArgs e)
		{
			World.world.viewOptions.OnViewOptionChanged-=new OptionChangedHandler(updatePreview);
		}

	}
}
