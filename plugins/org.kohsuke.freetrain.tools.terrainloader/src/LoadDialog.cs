using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.util.command;
using freetrain.framework;
using freetrain.world;

namespace freetrain.tools.terrainloader
{
	public class LoadDialog : System.Windows.Forms.Form
	{
		public LoadDialog() {
			InitializeComponent();

			commands = new CommandManager();

			new Command( commands )
				.addUpdateHandler( new CommandHandler(updateOKButton) )
				.commandInstances.AddAll( buttonOK );
		}

		private CommandManager commands;


		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox fileName;
		private System.Windows.Forms.Button buttonSelectFile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox sizeX;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox sizeY;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox sizeZ;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox water;
		private System.Windows.Forms.PictureBox previewBox;
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.fileName = new System.Windows.Forms.TextBox();
			this.buttonSelectFile = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.previewBox = new System.Windows.Forms.PictureBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.water = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.sizeZ = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.sizeY = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.sizeX = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "bmp";
			this.openFileDialog.Filter = "全ての画像 (*.bmp;*.gif;*.png;*.jpg)|*.bmp;*.gif;*.png;*.jpg";
			this.openFileDialog.RestoreDirectory = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "ファイル名(&F)：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// fileName
			// 
			this.fileName.Location = new System.Drawing.Point(88, 8);
			this.fileName.Name = "fileName";
			this.fileName.Size = new System.Drawing.Size(200, 19);
			this.fileName.TabIndex = 1;
			this.fileName.Text = "";
			this.fileName.TextChanged += new System.EventHandler(this.onFileNameChanged);
			// 
			// buttonSelectFile
			// 
			this.buttonSelectFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonSelectFile.Location = new System.Drawing.Point(296, 8);
			this.buttonSelectFile.Name = "buttonSelectFile";
			this.buttonSelectFile.Size = new System.Drawing.Size(64, 20);
			this.buttonSelectFile.TabIndex = 2;
			this.buttonSelectFile.Text = "選択(&S)...";
			this.buttonSelectFile.Click += new System.EventHandler(this.onSelectFile);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "プレビュー：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// previewBox
			// 
			this.previewBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.previewBox.Location = new System.Drawing.Point(8, 64);
			this.previewBox.Name = "previewBox";
			this.previewBox.Size = new System.Drawing.Size(176, 128);
			this.previewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.previewBox.TabIndex = 4;
			this.previewBox.TabStop = false;
			// 
			// buttonOK
			// 
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(192, 200);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(80, 24);
			this.buttonOK.TabIndex = 5;
			this.buttonOK.Text = "&OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(280, 200);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(80, 24);
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "ｷｬﾝｾﾙ(&C)";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.water,
																					this.label6,
																					this.sizeZ,
																					this.label5,
																					this.sizeY,
																					this.label4,
																					this.sizeX,
																					this.label3});
			this.groupBox1.Location = new System.Drawing.Point(192, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(168, 152);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "大きさ";
			// 
			// water
			// 
			this.water.Location = new System.Drawing.Point(72, 120);
			this.water.Name = "water";
			this.water.Size = new System.Drawing.Size(88, 19);
			this.water.TabIndex = 7;
			this.water.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 16);
			this.label6.TabIndex = 6;
			this.label6.Text = "水面高(&A)：";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// sizeZ
			// 
			this.sizeZ.Location = new System.Drawing.Point(72, 88);
			this.sizeZ.Name = "sizeZ";
			this.sizeZ.Size = new System.Drawing.Size(88, 19);
			this.sizeZ.TabIndex = 5;
			this.sizeZ.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "高さ(&H)：";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// sizeY
			// 
			this.sizeY.Location = new System.Drawing.Point(72, 56);
			this.sizeY.Name = "sizeY";
			this.sizeY.Size = new System.Drawing.Size(88, 19);
			this.sizeY.TabIndex = 3;
			this.sizeY.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 16);
			this.label4.TabIndex = 2;
			this.label4.Text = "奥行(&D)：";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// sizeX
			// 
			this.sizeX.Location = new System.Drawing.Point(72, 24);
			this.sizeX.Name = "sizeX";
			this.sizeX.Size = new System.Drawing.Size(88, 19);
			this.sizeX.TabIndex = 1;
			this.sizeX.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "幅(&W)：";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// LoadDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(368, 229);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox1,
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.previewBox,
																		  this.label2,
																		  this.buttonSelectFile,
																		  this.fileName,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoadDialog";
			this.ShowInTaskbar = false;
			this.Text = "地形の読み込み";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void onSelectFile(object sender, System.EventArgs e) {
			if( openFileDialog.ShowDialog(this) == DialogResult.OK )
				fileName.Text = openFileDialog.FileName;
		}

		private void onFileNameChanged(object sender, System.EventArgs e) {
			if( previewBox.Image!=null ) {
				Image img = previewBox.Image;
				previewBox.Image = null;
				img.Dispose();
			}

			try {
				previewBox.Image = new Bitmap(fileName.Text);
			} catch( Exception ) {
				previewBox.Image = null;
			}
		}

		private void updateOKButton( Command cmd ) {
			try {
				cmd.Enabled = (previewBox.Image!=null)
					&& int.Parse(sizeX.Text)>0
					&& int.Parse(sizeY.Text)>0
					&& int.Parse(sizeZ.Text)>0
					&& int.Parse(water.Text)>=0
					&& int.Parse(sizeZ.Text) > int.Parse(water.Text);
			} catch( Exception ) {
				cmd.Enabled = false;
			}
		}

		public World createWorld() {
			return TerrainLoader.loadWorld(
				(Bitmap)previewBox.Image, 
				new Size( int.Parse(sizeX.Text), int.Parse(sizeY.Text) ),
				int.Parse(sizeZ.Text), int.Parse(water.Text) );
		}

		private void buttonOK_Click(object sender, System.EventArgs e) {
			DialogResult = DialogResult.OK;
			Close();
		}

	}
}
