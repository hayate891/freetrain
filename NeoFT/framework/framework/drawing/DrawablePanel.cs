using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace nft.framework.drawing
{
	/// <summary>
	/// DrawablePanel の概要の説明です。
	/// </summary>
	public class DrawablePanel : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		private DrawableControl canvas;

		public DrawablePanel()
		{
			// この呼び出しは、Windows.Forms フォーム デザイナで必要です。
			InitializeComponent();
			InitializeCanvas();
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
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

		public DrawableControl Canvas { get { return canvas; } }

		public ISurface Surface { get { return canvas.Surface; } }

		#region コンポーネント デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// DrawablePanel
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Name = "DrawablePanel";
			this.Size = new System.Drawing.Size(88, 88);

		}
		#endregion
		private void InitializeCanvas() {
			this.SuspendLayout();
			try{
				this.canvas = GlobalModules.GraphicManager.CreateDrawableControl();
				// 
				// canvas
				// 
				this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
				this.canvas.Location = new System.Drawing.Point(0, 0);
				this.canvas.Name = "canvas";
				this.canvas.TabIndex = 0;
				// 
				// DrawablePanel
				// 
				this.Controls.Add(this.canvas);
			}catch {
			}
			this.ResumeLayout(false);
		}
	}
}
