using System;
using System.Drawing;

namespace nft.framework.drawing
{
	/// <summary>
	/// ISprite の概要の説明です。
	/// </summary>
	public interface ISprite : IDrawable
	{
		ITexture Texture { get; set; }

		/// <summary>
		/// 描画時に使用するフィルタ
		/// </summary>
		IPixelFilter Filter { get; set; }

		/// <summary>
		/// スプライトの表示位置
		/// </summary>
		Point Location { get; set; }

		/// <summary>
		/// 拡大（縮小）倍率
		/// ０以上は[倍率=1+Zoom]負の値は[倍率=1/(1-Zoom)]
		/// </summary>
		int Zoom { get; set; }

		/// <summary>
		/// 指定の座標が描画ピクセルならtrueを返す
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		bool HitTest(Point location);
	}
}
