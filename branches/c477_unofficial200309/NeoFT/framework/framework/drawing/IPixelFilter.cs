using System;
using System.Drawing;

namespace nft.framework.drawing
{
	/// <summary>
	/// ITexture の概要の説明です。
	/// </summary>
	public interface IPixelFilter
	{
		/// <summary>
		/// フィルター作業開始直前に呼ばれる
		/// </summary>
		/// <param name="mode">カラーモード</param>
		/// <param name="colorKey">透明色</param>
		void Begin(PixelColorMode mode, Int32 colorKey);

		/// <summary>
		/// 与えられたカラーを変換する
		/// 16bitモードでは、一度に2pixel分のデータが与えられる
		/// </summary>
		/// <param name="source">変換前の色値</param>
		/// <returns></returns>
		Int32 Convert(Int32 dest, Int32 source);

		/// <summary>
		/// フィルター作業終了事に呼ばれる
		/// </summary>
		void End();
	}
}
