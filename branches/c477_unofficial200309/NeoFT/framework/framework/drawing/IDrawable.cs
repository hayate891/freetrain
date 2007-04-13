using System;
using System.Drawing;

namespace nft.framework.drawing
{
	/// <summary>
	/// IDrawable はISurfaceに描画可能なオブジェクト
	/// </summary>
	public interface IDrawable : IDisposable
	{
		/// <summary>
		/// サーフェスに描画
		/// </summary>
		/// <param name="dest">描画先のサーフェス</param>
		/// <param name="location">描画先の位置（左上）</param>
		/// <param name="frame">フレーム番号（アニメーション用）</param>
		void Draw(ISurface dest, Point location, int zoom, int frame );
		void DrawEx(ISurface dest, Point location, int zoom, IPixelFilter filter, int frame );
		void DrawEx(ISurface dest, Point location, int zoom, ISurfaceDrawer drawer, int frame );
	}
}
