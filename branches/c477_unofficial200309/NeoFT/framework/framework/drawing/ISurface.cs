using System;
using System.Drawing;

namespace nft.framework.drawing
{
	/// <summary>
	/// ISurface ‚ÍIDrawable‚ð•`‰æ‚·‚éƒLƒƒƒ“ƒpƒX
	/// </summary>
	public interface ISurface : IDrawable
	{
		PixelColorMode PixelColorMode {get;}
		Color ColorKey { get; set; }
		Size Size { get; }
		/// <summary>
		/// Clear the surface region with a specified color.
		/// </summary>
		/// <param name="region"></param>
		/// <param name="fill"></param>
		void Clear(Rectangle region, Color fill);
		void Clear(Color fill);
		void BitBlt(Point destpos, ISurface source, Rectangle region, int zoom);
		void BitBlt(Point destpos, ISurface source, Rectangle region, int zoom, IPixelFilter filter);
		void BitBlt(Point destpos, ISurface source, Rectangle region, int zoom, ISurfaceDrawer drawer);
		bool IsOpaque(int x, int y );
		Bitmap CreateBitmap(PixelColorMode mode);
	}

}
