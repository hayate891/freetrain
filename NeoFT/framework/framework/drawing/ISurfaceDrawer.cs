using System;

namespace nft.framework.drawing
{
	/// <summary>
	/// ISurfaceDrawer ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public interface ISurfaceDrawer
	{
		void Blt(ref DrawingDesc dd );
	}

	public struct PixelBuffer {
		public int lpOffset;
		public int RowPitch;
		//public int RowPadding;
		public int PixelPitch;
		public int PixelMask;
		public PixelColorMode Mode;
		public int ColorKey;
	}

	public struct DrawingDesc {
		public PixelBuffer Src;
		public PixelBuffer Dest;
		public int RowSteps;
		public int ColumnSteps;
		public int Stretch;
	}
}
