
using System;
using System.Drawing;

namespace nft.framework.drawing
{
	/// <summary>
	/// ITexture の概要の説明です。
	/// </summary>
	public interface ITexture : IDrawable
	{
		Rectangle Boundary { get; }
		bool HitTest( Point pos );
	}

	public interface ISimpleTexture : ITexture {
		Color ColorKey { get; set; }
		Point DrawOffset { get; }
		Rectangle SourceRegion { get; }
		ISurface SourceSurface { get; }
		void PickColorKeyFromSource( int x, int y );
	}
}
