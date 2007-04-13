using System;
using System.Drawing;

namespace nft.framework.drawing
{
	/// <summary>
	/// IDrawable ��ISurface�ɕ`��\�ȃI�u�W�F�N�g
	/// </summary>
	public interface IDrawable : IDisposable
	{
		/// <summary>
		/// �T�[�t�F�X�ɕ`��
		/// </summary>
		/// <param name="dest">�`���̃T�[�t�F�X</param>
		/// <param name="location">�`���̈ʒu�i����j</param>
		/// <param name="frame">�t���[���ԍ��i�A�j���[�V�����p�j</param>
		void Draw(ISurface dest, Point location, int zoom, int frame );
		void DrawEx(ISurface dest, Point location, int zoom, IPixelFilter filter, int frame );
		void DrawEx(ISurface dest, Point location, int zoom, ISurfaceDrawer drawer, int frame );
	}
}
