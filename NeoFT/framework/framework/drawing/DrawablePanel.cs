using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace nft.framework.drawing
{
	/// <summary>
	/// DrawablePanel �̊T�v�̐����ł��B
	/// </summary>
	public class DrawablePanel : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		private DrawableControl canvas;

		public DrawablePanel()
		{
			// ���̌Ăяo���́AWindows.Forms �t�H�[�� �f�U�C�i�ŕK�v�ł��B
			InitializeComponent();
			InitializeCanvas();
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
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

		#region �R���|�[�l���g �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
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
