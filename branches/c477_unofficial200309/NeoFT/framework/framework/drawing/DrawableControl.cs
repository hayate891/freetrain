using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace nft.framework.drawing
{
	/// <summary>
	/// DrawableControl �ʒu�t���͒��ۃN���X�ɂ��ׂ������A
	/// ���ۃN���X�ł̓t�H�[���f�U�C�i���g���Ȃ��Ȃ�̂ŁAabstract�͂��Ȃ��B
	/// �������C�u�����͂��̃N���X���p������Surface�t�B�[���h��K�؂Ɏ�������B
	/// </summary>
	public class DrawableControl : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DrawableControl()
		{
			// ���̌Ăяo���́AWindows.Forms �t�H�[�� �f�U�C�i�ŕK�v�ł��B
			InitializeComponent();

			// TODO: InitializeComponent �Ăяo���̌�ɏ�����������ǉ����܂��B

		}

		public virtual ISurface Surface { get{ throw new InvalidOperationException("The field of this class is abstract and does not implemented."); } }

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

		#region �R���|�[�l���g �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
	}
}
