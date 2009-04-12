using System;
using System.Drawing;

#if window
#else
public class RECT
{
#warning STUB
	public int Left;
	public int Top;
	public int Right;
	public int Bottom;
};
#endif

namespace org.kohsuke.directdraw
{
	/// <summary>
	/// Color mask.
	/// </summary>
	public enum ColorMask { R,G,B };

	/// <summary>
	/// Wraps DirectDraw surface object.
	/// 
	/// This is the core object of DirectDraw.
	/// The code is a wrapper around Visual BASIC binding of DirectDraw.
	/// 
	/// Since I couldn't figure out how to create a CLR binding for
	/// clipper, this class implements a clipping support by itself.
	/// </summary>
	public class AgateSurface : IDisposable
	{
        /// <summary>
        /// Surface object that is being wrapped up
        /// </summary>
        private AgateLib.DisplayLib.Surface m_Surface;

#if windows
		private DirectDrawSurface7 surface;
		private static AlphaBlender alpha = new AlphaBlenderClass();
#else
#warning STUB
#endif

		/// <summary> Bit-width. </summary>
		private readonly byte widthR,widthB,widthG;

		/// <summary>
		/// Clipping rect. Even if the client doesn't set any clipping,
		/// this is initialized to (0,0)-(size)
		/// </summary>
		private RECT clip;

		/// <summary>
		/// Gets the size of this surface.
		/// </summary>
		public readonly Size size;

		private bool hasSourceColorKey = false;
		

		/// <summary>
		/// Obtain the wrapped DirectDraw interface.
		/// For advanced use only.
		/// </summary>
#if windows
		public DirectDrawSurface7 handle { get { return surface; } }
#else
#warning STUB
#endif

        public AgateLib.DisplayLib.Surface Surface
        {
            get { return m_Surface; }
            set { m_Surface = value; }
        }

        /// <summary>
        /// Draws the surface
        /// </summary>
        public void Draw(Rectangle inRectangle)
        {
           AgateLib.DisplayLib.DisplayWindow.CreateWindowed("", inRectangle.Width,inRectangle.Height);
            //while( !AgateLib.DisplayLib.Display.CurrentWindow.IsClosed )
            {
                AgateLib.DisplayLib.Display.BeginFrame();
                AgateLib.DisplayLib.Display.Clear( AgateLib.Geometry.Color.Red);
                Surface.Draw( new AgateLib.Geometry.Rectangle( inRectangle ) );
                AgateLib.DisplayLib.Display.EndFrame();
               //AgateLib.Core.KeepAlive();
            }
        }

        public void DrawBoxNew( Rectangle inRectangle )
        {
            Surface.Draw( new AgateLib.Geometry.Rectangle( inRectangle ) );
        }

		public AgateSurface(AgateLib.DisplayLib.Surface inSurface) 
        {
			this.Surface = inSurface;
 /*
			// compute the size of this surface
			DDSURFACEDESC2 desc = new DDSURFACEDESC2();
			surface.GetSurfaceDesc( ref desc );
			this.size = new Size( desc.lWidth, desc.lHeight );
			resetClipRect();

			// compute the bit shift width for color fill
			DDPIXELFORMAT pixelFormat = new DDPIXELFORMAT();
			surface.GetPixelFormat( ref pixelFormat );
			widthR = countBitWidth(pixelFormat.lRBitMask);
			widthG = countBitWidth(pixelFormat.lGBitMask);
			widthB = countBitWidth(pixelFormat.lBBitMask);
            */
            
		}

		public AgateSurface() 
        {
            
#warning STUB
		}


		public string displayModeName {
			get {
				return "mode"+widthR.ToString()+widthG.ToString()+widthB.ToString();
			}
		}

		/// <summary>
		/// Counts the width of bits.
		/// </summary>
		private byte countBitWidth( int _i ) {
			uint i = (uint)_i;

			while((i&1)==0)	i>>=1;

			byte w=0;
			while(i!=0) {
				i >>= 1;
				w++;
			}
			return w;
		}

		/// <summary>
		/// Converts a Color into a fill value.
		/// </summary>
		private uint colorToFill( Color c ) {
			uint x = 0;
			x |= ((uint)c.R)>>(8-widthR);
			x <<= widthG;
			x |= ((uint)c.G)>>(8-widthG);
			x <<= widthB;
			x |= ((uint)c.B)>>(8-widthB);
			return x;
		}

		public Rectangle clipRect {
			get {
				return Util.toRectangle(clip);
			}
			set {
				// clipping rectangle must also clip things to fit inside the surface.
				// otherwise blitting won't work.
				value.Intersect( new Rectangle( 0,0, size.Width, size.Height ) );
				clip = Util.toRECT(value);
			}
		}
		/// <summary>
		/// Removes the clipping rect by re-initializing it
		/// to the default size.
		/// </summary>
		public void resetClipRect() {
			clip = Util.toRECT( new Point(0,0), size );
		}


		public void Dispose() {
#if windows
			// explicitly release a reference
			if(surface!=null)
				System.Runtime.InteropServices.Marshal.ReleaseComObject(surface);
			surface=null;
#else
#warning STUB
#endif
		}

		/// <summary>
		/// Performs fast bit blitting.
		/// 
		/// This can't be used with a surface with a clipper.
		/// </summary>
		/// 
		public void bltFast( int destX, int destY, AgateSurface source, Rectangle srcRect ) {
#if windows
			RECT srect = Util.toRECT(srcRect);
			// TODO: clip

			surface.BltFast( destX, destY, source.handle, ref srect,
				CONST_DDBLTFASTFLAGS.DDBLTFAST_WAIT |
				CONST_DDBLTFASTFLAGS.DDBLTFAST_SRCCOLORKEY );
#else
#warning STUB
#endif
		}

		/// <summary>
		/// Copies an image from another surface.
		/// </summary>
		public void blt( int dstX1, int dstY1, int dstX2, int dstY2, AgateSurface source,
						 int srcX1, int srcY1, int srcX2, int srcY2 ) {
			
			RECT drect = Util.toRECT(dstX1,dstY1,dstX2,dstY2);
			RECT srect = Util.toRECT(srcX1,srcY1,srcX2,srcY2);

			blt( drect, source, srect );
		}

		public void blt( Point dst, AgateSurface source, Rectangle src ) {
			blt( Util.toRECT( dst, src.Size ), source, Util.toRECT(src) );
		}

		private void blt( RECT dst, AgateSurface source, RECT src ) {
#if windows
			CONST_DDBLTFLAGS flag;
			flag = CONST_DDBLTFLAGS.DDBLT_WAIT;
			if( source.hasSourceColorKey )
				flag |= CONST_DDBLTFLAGS.DDBLT_KEYSRC;

			Util.clip( ref dst, ref src, clip );

			surface.Blt( ref dst, source.handle, ref src, flag );
#else
#warning STUB
#endif
		}

		public void bltAlpha( Point dstPos, AgateSurface source, Point srcPos, Size sz ) {
#if windows
			RECT dst = Util.toRECT( dstPos, sz );
			RECT src = Util.toRECT( srcPos, sz );
			Util.clip( ref dst, ref src, clip );
			alpha.bltAlphaFast( surface, source.surface,
				dst.Left, dst.Top,
				src.Left, src.Top, src.Right, src.Bottom,
				source.colorKey );
#else
#warning STUB
#endif
		}

		public void bltAlpha( Point dstPos, AgateSurface source ) {
			bltAlpha( dstPos, source, new Point(0,0), source.size );
		}

		public void bltShape( Point dstPos, AgateSurface source, Point srcPos, Size sz, Color fill ) {
#if windows
			RECT dst = Util.toRECT( dstPos, sz );
			RECT src = Util.toRECT( srcPos, sz );
			Util.clip( ref dst, ref src, clip );

			alpha.bltShape( surface, source.surface,
				dst.Left, dst.Top,
				src.Left, src.Top, src.Right, src.Bottom,
				(int)colorToFill(fill),
				source.colorKey );
#else
#warning STUB
#endif
		}

		public void bltShape( Point dstPos, AgateSurface source, Color fill ) {
			bltShape( dstPos, source, new Point(0,0), source.size, fill );
		}

		public void blt( Point dstPos, AgateSurface source, Point srcPos, Size sz ) {
			RECT drect = Util.toRECT(dstPos,sz);
			RECT srect = Util.toRECT(srcPos,sz);
			blt( drect, source, srect );
		}

		public void blt( Point dstPos, AgateSurface source ) {
			RECT drect = Util.toRECT(dstPos, source.size );
			RECT srect = Util.toRECT(new Point(0,0),source.size);	// use the mpety rect
			blt( drect, source, srect );
		}

		public void bltColorTransform( Point dstPos, AgateSurface source,
			Point srcPos, Size sz,
			Color[] _srcColors, Color[] _dstColors, bool vflip ) {

#if windows
			RECT dst = Util.toRECT( dstPos, sz );
			RECT src = Util.toRECT( srcPos, sz );

			if( vflip )
				// in VFLIP mode, clipping works in a different way.
				Util.clipVflip( ref dst, ref src, clip );
			else
				Util.clip( ref dst, ref src, clip );

			int[] srcColors = new int[_srcColors.Length];
			int[] dstColors = new int[_srcColors.Length];
			for( int i=_srcColors.Length-1; i>=0; i-- ) {
				srcColors[i] = (int)colorToFill(_srcColors[i]);
				dstColors[i] = (int)colorToFill(_dstColors[i]);
			}

			alpha.bltColorTransform(
				surface, source.surface,
				dst.Left, dst.Top,
				src.Left, src.Top, src.Right, src.Bottom,
				srcColors,
				dstColors,
				srcColors.Length,
				source.colorKey,
				vflip?-1:0 );
#else
#warning STUB
#endif
		}

		public void bltHueTransform( Point dstPos, AgateSurface source, Point srcPos, Size sz,
			Color R_dest, Color G_dest, Color B_dest ) {
#if windows
			RECT dst = Util.toRECT( dstPos, sz );
			RECT src = Util.toRECT( srcPos, sz );
			Util.clip( ref dst, ref src, clip );

			//Debug.WriteLine(""+R_dest.ToArgb()+","+G_dest.ToArgb()+","+B_dest.ToArgb());
			bltHueTransform( surface, source.surface,
				dst.Left, dst.Top,
				src.Left, src.Top, src.Right, src.Bottom,				
				R_dest.ToArgb(), G_dest.ToArgb(), B_dest.ToArgb(),
				source.colorKey );
#else
#warning STUB
#endif
		}



		/// <summary>
		/// Fills the surface.
		/// </summary>
		public void fill( Color c ) {
#if windows
			surface.BltColorFill( ref clip, (int)colorToFill(c) );
#else
#warning STUB
#endif
		}

		public void fill( Rectangle rect, Color c ) {
#if windows
			RECT r = Util.intersect( Util.toRECT(rect), clip );
			surface.BltColorFill( ref r, (int)colorToFill(c) );
#else
#warning STUB
#endif
		}


		private int colorKey;

		/// <summary>
		/// Source color key. A mask color that will not be copied to other plains.
		/// </summary>
		public Color sourceColorKey 
        {
			set {
#if windows
				DDCOLORKEY key = new DDCOLORKEY();
				// TODO: how shall I convert Color to this structure?
				key.high = key.low = colorKey = (int)colorToFill(value);
				surface.SetColorKey( CONST_DDCKEYFLAGS.DDCKEY_SRCBLT, ref key );
				hasSourceColorKey = true;

				// TODO: how to remove color key?
#else
#warning STUB
#endif
                colorKey = (int)colorToFill( value );
                hasSourceColorKey = true;
               
                // TODO: fixme... what's a source color key...
                
			}
		}

		// retruns true if the color at the specified pixel is valid (opaque).
		public bool HitTest( Point p )
		{
			return HitTest(p.X, p.Y);
		}

		// retruns true if the color at the specified pixel is valid (opaque).
		public bool HitTest( int x, int y )
		{
			if(x<0 || x>size.Width || y<0 || y>size.Height )
				return false;
			return ((getColorAt(x,y)&0xffffff) == colorKey);
		}

		// returns color at specified point.
		// the return value suited for current pixel format.
		// outrange point will raise an error.
		int getColorAt( int x, int y )
		{
#if windows
			RECT r = new RECT();
			r.Left = x;
			r.Top = y;
			r.Bottom = r.Top+1;
			r.Right = r.Left+1;
			DDSURFACEDESC2 desc = new DDSURFACEDESC2();
			surface.GetSurfaceDesc( ref desc );
			surface.Lock(ref r,ref desc,CONST_DDLOCKFLAGS.DDLOCK_WAIT|CONST_DDLOCKFLAGS.DDLOCK_READONLY,0);
			int c = surface.GetLockedPixel(x,y);
			surface.Unlock(ref r);
			return c;
#else
#warning STUB
			return 0;
#endif
		}


		public void drawPolygon( Point p1, Point p2, Point p3, Point p4 ) {
#if windows
			int hdc = handle.GetDC();
			Polygon( hdc, new Point[]{p1,p2,p3,p4}, 4 );
			handle.ReleaseDC(hdc);
#else
#warning STUB
#endif
		}

		public void drawBox( Rectangle r ) {
#if windows
			handle.DrawBox( r.Left, r.Top, r.Right, r.Bottom );
#else
#warning STUB
#endif
		}

		/// <summary>
		/// Tries to recover a lost surface.
		/// </summary>
		public void restore() {
#if windows
			handle.restore();
#else
#warning STUB
#endif
		}

#if windows
		#region importing external functions
		[DllImport("gdi32.dll")]
		private static extern bool Polygon( int hdc, Point[] pts, int nCount );
		[DllImport("DirectDraw.AlphaBlend.dll")]
		private static extern uint bltHueTransform(
			DirectDrawSurface7 lpDDSDest,
			DirectDrawSurface7 lpDDSSource,
			int iDestX,
			int iDestY,
			int sourceX1, int sourceY1, int sourceX2, int sourceY2,
			int targetR, int targetG, int targetB, int keyCol );
		#endregion
#else
#warning STUB
#endif



//		public void lockTest() {
//			DDSURFACEDESC2 ddsd = new DDSURFACEDESC2();
//			DxVBLib.RECT r = new RECT();
//			r.Left = r.Top = 0;
//			r.Right = size.Width;
//			r.Bottom = size.Height;
//			handle.Lock( ref r, ref ddsd, CONST_DDLOCKFLAGS.DDLOCK_WAIT, 0 );  
//			handle.Unlock( ref r );
//		}

        public Bitmap CreateBitmapNew()
        {
			Bitmap theBitmap = new Bitmap( size.Width, size.Height );

            /* TODO: fixme Not sure what this is doing...
			using( GDIGraphics src = new GDIGraphics(this) )
            {
				using( Graphics dst = Graphics.FromImage(theBitmap) ) 
                {
					IntPtr dstHDC = dst.GetHdc();
					IntPtr srcHDC = src.graphics.GetHdc();
					BitBlt( dstHDC, 0, 0, size.Width, size.Height, srcHDC, 0, 0, 0x00CC0020 );
					dst.ReleaseHdc(dstHDC);
					src.graphics.ReleaseHdc(srcHDC);
				}
			}
            */
#warning STUB
            return theBitmap;

        }
		/// <summary>
		/// Makes the bitmap of this surface.
		/// The caller needs to dispose the bitmap.
		/// </summary>
		public Bitmap createBitmap() {
#if windows
			Bitmap bmp = new Bitmap( size.Width, size.Height );
			using( GDIGraphics src = new GDIGraphics(this) ) {
				using( Graphics dst = Graphics.FromImage(bmp) ) {
					IntPtr dstHDC = dst.GetHdc();
					IntPtr srcHDC = src.graphics.GetHdc();
					BitBlt( dstHDC, 0, 0, size.Width, size.Height, srcHDC, 0, 0, 0x00CC0020 );
					dst.ReleaseHdc(dstHDC);
					src.graphics.ReleaseHdc(srcHDC);
				}
			}
			return bmp;
#else
#warning STUB
			// TODO: fixme
            //Bitmap bmp = new Bitmap( size.Width, size.Height );
            Bitmap bmp = new Bitmap( 60,60  );
			return bmp;
#endif
		}

		public void GDICopyBits(Graphics g, Rectangle dst, Rectangle src){
#if windows
			using( GDIGraphics gg = new GDIGraphics(this) ) {
				IntPtr dstHDC = g.GetHdc();
				IntPtr srcHDC = gg.graphics.GetHdc();
				StretchBlt( dstHDC, dst.X,dst.Y,dst.Width,dst.Height, 
					srcHDC, src.X,src.Y,src.Width,src.Height, 0x00CC0020 );
				g.ReleaseHdc(dstHDC);
				gg.graphics.ReleaseHdc(srcHDC);
			}
#else
#warning STUB
#endif
		}

		public void GDICopyBits(Graphics g, Rectangle dst, Point src){
#if windows
			using( GDIGraphics gg = new GDIGraphics(this) ) {
				IntPtr dstHDC = g.GetHdc();
				IntPtr srcHDC = gg.graphics.GetHdc();
				BitBlt( dstHDC, dst.X,dst.Y,dst.Width,dst.Height, srcHDC, src.X,src.Y, 0x00CC0020 );
				g.ReleaseHdc(dstHDC);
				gg.graphics.ReleaseHdc(srcHDC);
			}
#else
#warning STUB
#endif
		}

#if windows
		[DllImport("gdi32.dll")]
		private static extern bool BitBlt(
			IntPtr hdcDest,
			int nXDest,
			int nYDest,
			int nWidth,
			int nHeight,
			IntPtr hdcSrc,
			int nXSrc,
			int nYSrc,
			long dwRop
		);

		[DllImport("gdi32.dll")]
		private static extern bool StretchBlt(
			IntPtr hdcDest,      // コピー先のデバイスコンテキストのハンドル
			int nXOriginDest, // コピー先長方形の左上隅の x 座標
			int nYOriginDest, // コピー先長方形の左上隅の y 座標
			int nWidthDest,   // コピー先長方形の幅
			int nHeightDest,  // コピー先長方形の高さ
			IntPtr hdcSrc,       // コピー元のデバイスコンテキストのハンドル
			int nXOriginSrc,  // コピー元長方形の左上隅の x 座標
			int nYOriginSrc,  // コピー元長方形の左上隅の y 座標
			int nWidthSrc,    // コピー元長方形の幅
			int nHeightSrc,   // コピー元長方形の高さ
			long dwRop       // ラスタオペレーションコード
		);
#else
#warning STUB
#endif

	}

	/// <summary>
	/// Wraps a Surface object and provides GDI+ functionality
	/// via the graphics property.
    /// TODO: fixme Will this go away??
	/// </summary>
	public sealed class GDIGraphics : IDisposable
    {
		public readonly Graphics graphics;

        private readonly AgateSurface Surface;
		private readonly int hdc;


		public GDIGraphics( AgateSurface inSurface ) 
        {
			//
		}

		public void Dispose() 
        {
#if windows
			graphics.Dispose();
			surface.handle.ReleaseDC(hdc);
#else
#warning STUB
#endif
		}
	}
}
