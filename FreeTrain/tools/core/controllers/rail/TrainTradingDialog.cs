using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.contributions.train;
using freetrain.framework;
using freetrain.framework.graphics;
using freetrain.framework.plugin;
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
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
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
			((System.ComponentModel.ISupportInitialize)(this.length)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.count)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(176, 216);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "&Length";
			//! this.label2.Text = "車両数(&L)：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// length
			// 
			this.length.Location = new System.Drawing.Point(248, 216);
			this.length.Minimum = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   0});
			this.length.Name = "length";
			this.length.Size = new System.Drawing.Size(48, 19);
			this.length.TabIndex = 4;
			this.length.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.length.Value = new System.Decimal(new int[] {
																 3,
																 0,
																 0,
																 0});
			this.length.ValueChanged += new System.EventHandler(this.onAmountChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(320, 216);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(16, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "×";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// count
			// 
			this.count.Location = new System.Drawing.Point(344, 216);
			this.count.Name = "count";
			this.count.Size = new System.Drawing.Size(48, 19);
			this.count.TabIndex = 6;
			this.count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.count.Value = new System.Decimal(new int[] {
																1,
																0,
																0,
																0});
			this.count.ValueChanged += new System.EventHandler(this.onAmountChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(176, 240);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(256, 4);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(392, 216);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(32, 20);
			this.label4.TabIndex = 7;
			this.label4.Text = "Formation";
			//! this.label4.Text = "編成";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// buttonOK
			// 
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(264, 312);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(80, 24);
			this.buttonOK.TabIndex = 8;
			this.buttonOK.Text = "Buy (&O)";
			//! this.buttonOK.Text = "購入(&O)";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(352, 312);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(80, 24);
			this.buttonCancel.TabIndex = 9;
			this.buttonCancel.Text = "&Close";
			//! this.buttonCancel.Text = "閉じる(&C)";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(176, 192);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 10;
			this.label5.Text = "Speed:";
			//! this.label5.Text = "速度：";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// speed
			// 
			this.speed.Location = new System.Drawing.Point(248, 192);
			this.speed.Name = "speed";
			this.speed.Size = new System.Drawing.Size(184, 16);
			this.speed.TabIndex = 11;
			this.speed.Text = "Rapid";
			//! this.speed.Text = "高速";
			this.speed.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// totalPrice
			// 
			this.totalPrice.Location = new System.Drawing.Point(248, 256);
			this.totalPrice.Name = "totalPrice";
			this.totalPrice.Size = new System.Drawing.Size(184, 16);
			this.totalPrice.TabIndex = 14;
			this.totalPrice.Text = "100,000";
			this.totalPrice.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(176, 256);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(64, 16);
			this.label8.TabIndex = 15;
			this.label8.Text = "Total cost:";
			//! this.label8.Text = "総費用：";
			this.label8.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// passenger
			// 
			this.passenger.Location = new System.Drawing.Point(248, 280);
			this.passenger.Name = "passenger";
			this.passenger.Size = new System.Drawing.Size(184, 16);
			this.passenger.TabIndex = 17;
			this.passenger.Text = "100";
			this.passenger.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(176, 280);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(64, 16);
			this.label9.TabIndex = 16;
			this.label9.Text = "Capacity:";
			//! this.label9.Text = "定員：";
			this.label9.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// typeTree
			// 
			this.typeTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.typeTree.ImageIndex = -1;
			this.typeTree.Name = "typeTree";
			this.typeTree.SelectedImageIndex = -1;
			this.typeTree.Size = new System.Drawing.Size(168, 342);
			this.typeTree.TabIndex = 18;
			this.typeTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.onTypeChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(176, 112);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 19;
			this.label1.Text = "Author:";
			//! this.label1.Text = "作者：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(176, 136);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 16);
			this.label6.TabIndex = 20;
			this.label6.Text = "Description:";
			//! this.label6.Text = "説明：";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// description
			// 
			this.description.BackColor = System.Drawing.SystemColors.Control;
			this.description.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.description.Location = new System.Drawing.Point(248, 136);
			this.description.Multiline = true;
			this.description.Name = "description";
			this.description.ReadOnly = true;
			this.description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.description.Size = new System.Drawing.Size(184, 48);
			this.description.TabIndex = 21;
			this.description.Text = "";
			// 
			// author
			// 
			this.author.Location = new System.Drawing.Point(248, 112);
			this.author.Name = "author";
			this.author.Size = new System.Drawing.Size(184, 16);
			this.author.TabIndex = 22;
			this.author.Text = "477-san";
			//! this.author.Text = "477さん";
			this.author.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(296, 216);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(16, 20);
			this.label7.TabIndex = 23;
			this.label7.Text = "Car";
			//! this.label7.Text = "両";
			this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(176, 8);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 16);
			this.label10.TabIndex = 24;
			this.label10.Text = "Name:";
			//! this.label10.Text = "名称：";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// name
			// 
			this.name.Location = new System.Drawing.Point(248, 8);
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(184, 16);
			this.name.TabIndex = 25;
			this.name.Text = "123 system ABCDEF";
			//! this.name.Text = "123系 ABCDEF";
			this.name.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// preview
			// 
			this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.preview.Location = new System.Drawing.Point(248, 32);
			this.preview.Name = "preview";
			this.preview.Size = new System.Drawing.Size(184, 72);
			this.preview.TabIndex = 26;
			this.preview.TabStop = false;
			// 
			// TrainTradingDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(442, 342);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.preview,
																		  this.name,
																		  this.label10,
																		  this.label7,
																		  this.author,
																		  this.description,
																		  this.label6,
																		  this.label1,
																		  this.typeTree,
																		  this.passenger,
																		  this.label9,
																		  this.label8,
																		  this.totalPrice,
																		  this.speed,
																		  this.label5,
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.label4,
																		  this.groupBox1,
																		  this.count,
																		  this.label3,
																		  this.length,
																		  this.label2});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TrainTradingDialog";
			this.Text = "Buy trains";
			//! this.Text = "車両の購入";
			((System.ComponentModel.ISupportInitialize)(this.length)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.count)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

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



	}
}
