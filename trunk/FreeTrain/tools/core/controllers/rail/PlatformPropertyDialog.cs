using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.contributions.rail;
using freetrain.framework;

namespace freetrain.world.rail
{
	/// <summary>
	/// PlatformPropertyDialog の概要の説明です。
	/// </summary>
	public class PlatformPropertyDialog : Form
	{

		private readonly Platform platform;
		/// <summary>
		/// Indecies of left/right lanes.
		/// </summary>
		private readonly int lIdx,rIdx;

		public PlatformPropertyDialog(Platform platform) {
			this.platform = platform;

			InitializeComponent();

			if(platform.direction.index>=4) {
				lIdx=0; rIdx=1;
			} else {
				lIdx=1; rIdx=0;
			}
			remove.Enabled = platform.canRemove;
			updateDialog();
			nameBox.Text = platform.name;

			groupFat.Visible = ( platform is FatPlatform );

			// fill host list
			foreach( PlatformHost host in platform.listHosts())
				hostList.Items.Add(host);
			hostList.SelectedItem = platform.host;

			// bell sound list
			bell.DataSource = DepartureBellContribution.all;
			bell.SelectedItem = platform.bellSound;
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
		private System.Windows.Forms.ComboBox bell;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupFat;
		private System.Windows.Forms.Button left;
		private System.Windows.Forms.Button right;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button OKbutton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button remove;
		private System.Windows.Forms.PictureBox warning;
		private System.Windows.Forms.ComboBox hostList;
		private System.Windows.Forms.TextBox nameBox;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PlatformPropertyDialog));
			this.left = new System.Windows.Forms.Button();
			this.right = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.groupFat = new System.Windows.Forms.GroupBox();
			this.OKbutton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.hostList = new System.Windows.Forms.ComboBox();
			this.remove = new System.Windows.Forms.Button();
			this.warning = new System.Windows.Forms.PictureBox();
			this.bell = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupFat.SuspendLayout();
			this.SuspendLayout();
			// 
			// left
			// 
			this.left.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.left.Location = new System.Drawing.Point(64, 16);
			this.left.Name = "left";
			this.left.Size = new System.Drawing.Size(96, 24);
			this.left.TabIndex = 7;
			this.left.Text = "左に接続(&L)";
			this.left.Click += new System.EventHandler(this.onLeft);
			// 
			// right
			// 
			this.right.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.right.Location = new System.Drawing.Point(176, 16);
			this.right.Name = "right";
			this.right.Size = new System.Drawing.Size(96, 24);
			this.right.TabIndex = 8;
			this.right.Text = "右に接続(&R)";
			this.right.Click += new System.EventHandler(this.onRight);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "名前(&N):";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// nameBox
			// 
			this.nameBox.Location = new System.Drawing.Point(72, 8);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(216, 19);
			this.nameBox.TabIndex = 2;
			this.nameBox.Text = "";
			// 
			// groupFat
			// 
			this.groupFat.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.left,
																				   this.right});
			this.groupFat.Location = new System.Drawing.Point(8, 104);
			this.groupFat.Name = "groupFat";
			this.groupFat.Size = new System.Drawing.Size(280, 48);
			this.groupFat.TabIndex = 999;
			this.groupFat.TabStop = false;
			this.groupFat.Text = "線路との接続工事";
			// 
			// OKbutton
			// 
			this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKbutton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OKbutton.Location = new System.Drawing.Point(104, 160);
			this.OKbutton.Name = "OKbutton";
			this.OKbutton.Size = new System.Drawing.Size(88, 24);
			this.OKbutton.TabIndex = 10;
			this.OKbutton.Text = "&OK";
			this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(200, 160);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(88, 24);
			this.cancelButton.TabIndex = 11;
			this.cancelButton.Text = "キャンセル(&C)";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "駅(&S):";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// hostList
			// 
			this.hostList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.hostList.Location = new System.Drawing.Point(72, 40);
			this.hostList.Name = "hostList";
			this.hostList.Size = new System.Drawing.Size(216, 20);
			this.hostList.TabIndex = 4;
			// 
			// remove
			// 
			this.remove.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.remove.Location = new System.Drawing.Point(8, 160);
			this.remove.Name = "remove";
			this.remove.Size = new System.Drawing.Size(88, 24);
			this.remove.TabIndex = 9;
			this.remove.Text = "撤去(&E)";
			this.remove.Click += new System.EventHandler(this.onRemove);
			// 
			// warning
			// 
			this.warning.Image = ((System.Drawing.Bitmap)(resources.GetObject("warning.Image")));
			this.warning.Location = new System.Drawing.Point(8, 40);
			this.warning.Name = "warning";
			this.warning.Size = new System.Drawing.Size(16, 16);
			this.warning.TabIndex = 1000;
			this.warning.TabStop = false;
			// 
			// bell
			// 
			this.bell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.bell.Location = new System.Drawing.Point(72, 72);
			this.bell.Name = "bell";
			this.bell.Size = new System.Drawing.Size(216, 20);
			this.bell.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "ベル(&B):";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// PlatformPropertyDialog
			// 
			this.AcceptButton = this.OKbutton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(298, 189);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label3,
																		  this.bell,
																		  this.warning,
																		  this.remove,
																		  this.hostList,
																		  this.label2,
																		  this.cancelButton,
																		  this.OKbutton,
																		  this.groupFat,
																		  this.nameBox,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PlatformPropertyDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ホームのプロパティ";
			this.groupFat.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		private void updateDialog() {
			if( platform is FatPlatform ) {
				FatPlatform fp = (FatPlatform)platform;
				if(!fp.hasLane(lIdx))	left.Text = "左に接続";
				else					left.Text = "左を解放";
				if(!fp.hasLane(rIdx))	right.Text = "右に接続";
				else					right.Text = "右を解放";
			}

			if( platform.host!=null )	warning.Hide();
			else						warning.Show();
		}

		private void onLeft(object sender, EventArgs e) { build(lIdx); }
		private void onRight(object sender, EventArgs e) { build(rIdx); }

		private void build( int index ) {
			Debug.Assert( platform is FatPlatform );
			FatPlatform fp = (FatPlatform)platform;

			if(fp.hasLane(index)) {
				if(fp.canRemoveLane(index))
					fp.removeLane(index);
				else
					MainWindow.showError("障害物があって解放できません");
			} else {
				if(fp.canAddLane(index))
					fp.addLane(index);
				else
					MainWindow.showError("障害物があって接続できません");
			}
			updateDialog();
		}

		private void OKbutton_Click(object sender, System.EventArgs e) {
			platform.name = nameBox.Text;
			platform.host = (PlatformHost)hostList.SelectedItem;
			platform.bellSound = (DepartureBellContribution)bell.SelectedItem;
		}

		private void onRemove(object sender, System.EventArgs e) {
			if(MessageBox.Show(this,"このホームを撤去しますか？","ホームの撤去",MessageBoxButtons.YesNo,MessageBoxIcon.Question)
			== DialogResult.Yes) {
				platform.remove();
				Close();	// close the dialog
			}
		}
	}
}
