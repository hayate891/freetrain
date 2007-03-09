using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace freetrain.framework
{
	/// <summary>
	/// Splash screen that reports the progress of initialization.
	/// </summary>
	public class Splash : Form
	{
		public Splash() {
			InitializeComponent();
		}

		public void updateMessage( string msg ) {
			status.Text = msg;
			Application.DoEvents();
		}

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label status;
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Splash));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.status = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(320, 240);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// status
			// 
			this.status.Location = new System.Drawing.Point(0, 240);
			this.status.Name = "status";
			this.status.Size = new System.Drawing.Size(320, 32);
			this.status.TabIndex = 1;
			// 
			// Splash
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(320, 272);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.status,
																		  this.pictureBox1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Splash";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Splash";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
