using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace freetrain.world.rail
{
	/// <summary>
	/// Property dialog of a station
	/// </summary>
	public class StationPropertyDialog : Form
	{
		#region Windows Form Designer generated code
		
		private System.Windows.Forms.Button remove;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button OKbutton;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Label label1;
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
			this.remove = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.OKbutton = new System.Windows.Forms.Button();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// remove
			// 
			this.remove.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.remove.Location = new System.Drawing.Point(8, 64);
			this.remove.Name = "remove";
			this.remove.Size = new System.Drawing.Size(88, 24);
			this.remove.TabIndex = 3;
			this.remove.Text = "撤去(&E)";
			this.remove.Click += new System.EventHandler(this.remove_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(200, 64);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(88, 24);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "キャンセル(&C)";
			// 
			// OKbutton
			// 
			this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKbutton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OKbutton.Location = new System.Drawing.Point(104, 64);
			this.OKbutton.Name = "OKbutton";
			this.OKbutton.Size = new System.Drawing.Size(88, 24);
			this.OKbutton.TabIndex = 4;
			this.OKbutton.Text = "&OK";
			this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
			// 
			// nameBox
			// 
			this.nameBox.Location = new System.Drawing.Point(72, 8);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(216, 19);
			this.nameBox.TabIndex = 2;
			this.nameBox.Text = "";
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
			// StationPropertyDialog
			// 
			this.AcceptButton = this.OKbutton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(296, 95);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.remove,
																		  this.cancelButton,
																		  this.OKbutton,
																		  this.nameBox,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StationPropertyDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "駅のプロパティ";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion

		public StationPropertyDialog( Station st ) {
			this.station = st;

			InitializeComponent();

			// initialize the dialog
			nameBox.Text = station.name;
		}

		/// <summary> Station object to which this dialog is opened for. </summary>
		private readonly Station station;

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}


		private void remove_Click(object sender, EventArgs e) {
			if(MessageBox.Show(this,"この駅舎を撤去しますか？","駅舎の撤去",
					MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.Yes)
				return;

			// destroy the station and close the dialog
			station.remove();
			Close();
		}

		private void OKbutton_Click(object sender, EventArgs e) {
			station.setName(nameBox.Text);
			Close();
		}
	}
}
