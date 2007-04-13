using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace freetrain.world.rail.tattc
{
	/// <summary>
	/// Configuration Dialog of JunctionController
	/// </summary>
	internal class JunctionConfigDialog : Form
	{
		internal JunctionConfigDialog( Junction jc ) {
			this.junction = jc;
			InitializeComponent();

			updateDirectionButton();
		}

		/// <summary>
		/// The junction controller which we are configuring.
		/// </summary>
		private readonly Junction junction;


		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private System.Windows.Forms.Button buttonDirection;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonAdvanced;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(JunctionConfigDialog));
			this.buttonDirection = new System.Windows.Forms.Button();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.buttonOk = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonAdvanced = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonDirection
			// 
			this.buttonDirection.Image = ((System.Drawing.Bitmap)(resources.GetObject("buttonDirection.Image")));
			this.buttonDirection.ImageIndex = 0;
			this.buttonDirection.ImageList = this.imageList;
			this.buttonDirection.Location = new System.Drawing.Point(24, 32);
			this.buttonDirection.Name = "buttonDirection";
			this.buttonDirection.Size = new System.Drawing.Size(72, 72);
			this.buttonDirection.TabIndex = 1;
			this.buttonDirection.Click += new System.EventHandler(this.buttonDirection_Click);
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList.ImageSize = new System.Drawing.Size(48, 48);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "進行方向(&D)：";
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOk.Location = new System.Drawing.Point(128, 40);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(72, 24);
			this.buttonOk.TabIndex = 3;
			this.buttonOk.Text = "&OK";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(120, -8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(2, 120);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			// 
			// buttonAdvanced
			// 
			this.buttonAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonAdvanced.Location = new System.Drawing.Point(128, 8);
			this.buttonAdvanced.Name = "buttonAdvanced";
			this.buttonAdvanced.Size = new System.Drawing.Size(72, 24);
			this.buttonAdvanced.TabIndex = 2;
			this.buttonAdvanced.Text = "詳細(&A)...";
			this.buttonAdvanced.Click += new System.EventHandler(this.buttonAdvanced_Click);
			// 
			// JunctionConfigDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(206, 112);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonAdvanced,
																		  this.groupBox1,
																		  this.buttonOk,
																		  this.label1,
																		  this.buttonDirection});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "JunctionConfigDialog";
			this.Text = "ポイントの設定";
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonDirection_Click(object sender, System.EventArgs e) {
			junction.defaultRoute =
				(junction.defaultRoute==JunctionRoute.Curve)
				? JunctionRoute.Straight : JunctionRoute.Curve;
			updateDirectionButton();
			World.world.onVoxelUpdated( junction.location );	// upte the map window
		}

		/// <summary>
		/// Update the image of the direction button.
		/// </summary>
		private void updateDirectionButton() {
			if(junction.defaultRoute==JunctionRoute.Curve)
				buttonDirection.ImageIndex = 1;
			else
				buttonDirection.ImageIndex = 0;
		}

		private void buttonAdvanced_Click(object sender, System.EventArgs e) {
			new JunctionAdvancedDialog(junction).ShowDialog(this);
		}

		protected override void OnClosed(EventArgs e) {
			// redraw the voxel
			World.world.onVoxelUpdated(junction.location);
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			// move the dialog to the cursor
			Point pt = Cursor.Position;
			pt.X -= Width/2;
			pt.Y -= Height/2;
			this.Location = pt;
		}
	}
}
