using System;
using System.Drawing;
using System.Collections;
using nft.framework.drawing;
using DxVBLib;

namespace nft.drawing.ddraw7 
{
	internal class Util
	{
		public const int OPAQUE_KEY = 0x01000000;

		internal static RECT toRECT( Rectangle srcRect ) 
		{
			RECT r = new RECT();
			r.Left	= srcRect.Left;
			r.Top	= srcRect.Top;
			r.Right	= srcRect.Right;
			r.Bottom= srcRect.Bottom;
			return r;
		}
		internal static RECT toRECT( int x1, int y1, int x2, int y2 ) {
			RECT r = new RECT();
			r.Left	= x1;
			r.Top	= y1;
			r.Right	= x2;
			r.Bottom= y2;
			return r;
		}
		internal static RECT toRECT( Point pt, Size sz ) {
			RECT r = new RECT();
			r.Left	= pt.X;
			r.Top	= pt.Y;
			r.Right	= pt.X+sz.Width;
			r.Bottom= pt.Y+sz.Height;
			return r;
		}
		internal static Rectangle toRectangle( RECT r ) {
			return new Rectangle( r.Left, r.Top, r.Right-r.Left, r.Bottom-r.Top );
		}

		/// <summary>
		/// Compute the intersection of two RECTs.
		/// </summary>
		internal static RECT intersect( RECT a, RECT b ) {
			RECT r = new RECT();
			r.Left   = Math.Max( a.Left,   b.Left   );
			r.Top    = Math.Max( a.Top,    b.Top    );
			r.Right	 = Math.Min( a.Right,  b.Right  );
			r.Bottom = Math.Min( a.Bottom, b.Bottom );
			return r;
		}

		/// <summary>
		/// Clip two rectangles by the clipping region.
		/// 
		/// </summary>
		internal static void clip( ref RECT dst, ref RECT src, RECT clip ) {
			int t;

			// compute new dst.Left
			t = Math.Max( dst.Left,   clip.Left );
			src.Left   += (t-dst.Left);
			dst.Left   =  t;		// dst.Left += (t-dst.Left)

			t = Math.Max( dst.Top,    clip.Top );
			src.Top    += (t-dst.Top);
			dst.Top    =  t;

			t = Math.Min( dst.Right,  clip.Right );
			src.Right  += (t-dst.Right);
			dst.Right  =  t;

			t = Math.Min( dst.Bottom, clip.Bottom );
			src.Bottom += (t-dst.Bottom);
			dst.Bottom =  t;
		}

		/// <summary>
		/// Clipping in a vflip mode.
		/// 
		/// In this mode, clipping is done as:
		/// 
		/// ###---     ------
		/// ###---  => ###---
		/// ------     ###---
		/// </summary>
		/// <param name="dst"></param>
		/// <param name="src"></param>
		/// <param name="clip"></param>
		internal static void clipVflip( ref RECT dst, ref RECT src, RECT clip ) {
			int t;

			// compute new dst.Left
			t = Math.Max( dst.Left,   clip.Left );
			src.Left   += (t-dst.Left);
			dst.Left   =  t;		// dst.Left += (t-dst.Left)

			t = Math.Max( dst.Top,    clip.Top );
			src.Bottom -= (t-dst.Top);		// different than the clip method
			dst.Top    =  t;

			t = Math.Min( dst.Right,  clip.Right );
			src.Right  += (t-dst.Right);
			dst.Right  =  t;

			t = Math.Min( dst.Bottom, clip.Bottom );
			src.Top    -= (t-dst.Bottom);	// different than the clip method
			dst.Bottom =  t;
		}

		internal static void PrepareDesc(ref DrawingDesc desc, DD7Surface dstSurf, ref DDSURFACEDESC2 dstSD, ref RECT dstRegion,
			DD7Surface srcSurf, ref DDSURFACEDESC2 srcSD, ref RECT srcRegion, int zoomCount ) {
			
			int skip = 0;
			if( zoomCount >= 0 ) {
				desc.Stretch = zoomCount;
				desc.ColumnSteps = srcRegion.Right-srcRegion.Left;
				desc.RowSteps = srcRegion.Bottom-srcRegion.Top;
				//dstRegion.Width *= 1+zoomCount;
				//dstRegion.Height *= 1+zoomCount;
			}
			else {
				desc.Stretch = 0;
				skip = -zoomCount;
				//dstRegion.Width /= 1-zoomCount;
				//dstRegion.Height /= 1-zoomCount;
				desc.ColumnSteps = dstRegion.Right-dstRegion.Left;
				desc.RowSteps = dstRegion.Bottom-dstRegion.Top;
			}
			PreparePixelBuffer(ref desc.Dest, dstSurf, dstSD, ref dstRegion, 0 );
			PreparePixelBuffer(ref desc.Src, srcSurf, srcSD, ref srcRegion, skip );
		}

		internal static void PreparePixelBuffer(ref PixelBuffer pb, DD7Surface surface, DDSURFACEDESC2 ddsd, ref RECT region, int skip ) {
			if(surface.ColorKey == Color.Empty)
				pb.ColorKey = OPAQUE_KEY;
			else
				pb.ColorKey = (int)surface.colorKeyValue;
			pb.Mode = surface.PixelColorMode;
			pb.RowPitch = ddsd.lPitch;
			pb.PixelPitch = ddsd.ddpfPixelFormat.lRGBBitCount>>3;
			pb.PixelMask = ddsd.ddpfPixelFormat.lRBitMask|ddsd.ddpfPixelFormat.lGBitMask|ddsd.ddpfPixelFormat.lBBitMask;
			pb.lpOffset = ddsd.lpSurface + region.Left*pb.PixelPitch + region.Top*pb.RowPitch;
			if(skip>0) {
				pb.PixelPitch *= (1+skip);
				pb.RowPitch *= (1+skip);
			}
			//pb.LowPadding = ?;
		}
	}
}
