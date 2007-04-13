using System;
using System.Drawing;

namespace nft.framework.drawing
{
	/// <summary>
	/// ISprite �̊T�v�̐����ł��B
	/// </summary>
	public interface ISprite : IDrawable
	{
		ITexture Texture { get; set; }

		/// <summary>
		/// �`�掞�Ɏg�p����t�B���^
		/// </summary>
		IPixelFilter Filter { get; set; }

		/// <summary>
		/// �X�v���C�g�̕\���ʒu
		/// </summary>
		Point Location { get; set; }

		/// <summary>
		/// �g��i�k���j�{��
		/// �O�ȏ��[�{��=1+Zoom]���̒l��[�{��=1/(1-Zoom)]
		/// </summary>
		int Zoom { get; set; }

		/// <summary>
		/// �w��̍��W���`��s�N�Z���Ȃ�true��Ԃ�
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		bool HitTest(Point location);
	}
}
