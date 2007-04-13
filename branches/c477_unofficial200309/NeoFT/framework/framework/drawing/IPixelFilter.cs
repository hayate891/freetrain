using System;
using System.Drawing;

namespace nft.framework.drawing
{
	/// <summary>
	/// ITexture �̊T�v�̐����ł��B
	/// </summary>
	public interface IPixelFilter
	{
		/// <summary>
		/// �t�B���^�[��ƊJ�n���O�ɌĂ΂��
		/// </summary>
		/// <param name="mode">�J���[���[�h</param>
		/// <param name="colorKey">�����F</param>
		void Begin(PixelColorMode mode, Int32 colorKey);

		/// <summary>
		/// �^����ꂽ�J���[��ϊ�����
		/// 16bit���[�h�ł́A��x��2pixel���̃f�[�^���^������
		/// </summary>
		/// <param name="source">�ϊ��O�̐F�l</param>
		/// <returns></returns>
		Int32 Convert(Int32 dest, Int32 source);

		/// <summary>
		/// �t�B���^�[��ƏI�����ɌĂ΂��
		/// </summary>
		void End();
	}
}
