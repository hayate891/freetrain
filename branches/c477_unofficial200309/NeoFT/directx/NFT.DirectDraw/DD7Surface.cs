using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using DxVBLib;
using DirectDrawAlphaBlendLib;
using nft.framework.drawing;

namespace nft.drawing.ddraw7
{
	/// <summary>
	/// DD7Surface の概要の説明です。
	/// </summary>
	/// <remarks>this class is originaly implemented by K.Kawaguchi</remarks>
	public class DD7Surface : ISurface
	{
		protected DirectDrawSurface7 surface;
		protected Color colorKey;
		internal protected UInt32 colorKeyValue;
		protected PixelColorMode colorMode;
		protected Size surfaceSize;

		/// <summary>
		/// Clipping rect. Even if the client doesn't set any clipping,
		/// this is initialized to (0,0)-(size)
		/// </summary>
		protected RECT clip;

		public DD7Surface(DirectDrawSurface7 surface)
		{
			this.surface = surface;
			this.colorKey = Color.Empty;
			this.colorKeyValue = 0;

			// compute the size of this surface
			DDSURFACEDESC2 desc = new DDSURFACEDESC2();
			surface.GetSurfaceDesc( ref desc );
			this.surfaceSize = new Size( desc.lWidth, desc.lHeight );
			this.colorMode = DD7GraphicManager.GetPixelColorMode(surface);
			ResetClipRect();

		}

		#region クラス独自メソッド
		static internal protected DirectDrawSurface7 ToNativeSurface(ISurface surface){
			DirectDrawSurface7 s;
			try{
				s = ((DD7Surface)surface).NativeSurface;
			}catch(InvalidCastException){
				DD7GraphicManager gm = DD7GraphicManager.TheInstance;
				PixelColorMode mode = surface.PixelColorMode;
				Bitmap bmp = surface.CreateBitmap(mode);
				DD7Surface temp = (DD7Surface)gm.CreateSurfaceFromBitmap(bmp,mode,SurfaceAlloc.SystemMem);
				s = temp.NativeSurface;
			}
			return s;
		}

		/// <summary>
		/// Returns true if the given exception is thrown because of
		/// a lost surface.
		/// </summary>
		public static bool isSurfaceLostException( COMException e ) {
			return (uint)e.ErrorCode == (uint)0x887601C2;
		}

		public DirectDrawSurface7 NativeSurface { get { return surface; } }

		public virtual Rectangle ClipRect {
			get {
				return Util.toRectangle(clip);
			}
			set {
				// clipping rectangle must also clip things to fit inside the surface.
				// otherwise blitting won't work.
				value.Intersect( new Rectangle( 0,0, surfaceSize.Width, surfaceSize.Height ) );
				clip = Util.toRECT(value);
			}
		}

		public virtual RECT ClipRECT {
			get {
				return clip;
			}
			set {
				ClipRect = Util.toRectangle(value);
			}
		}

		/// <summary>
		/// Removes the clipping rect by re-initializing it
		/// to the default size.
		/// </summary>
		public virtual void ResetClipRect() {
			clip = Util.toRECT( new Point(0,0), surfaceSize );
		}

		public virtual void DxBlt(Rectangle dstRegion, DirectDrawSurface7 source, Rectangle srcRegion, CONST_DDBLTFLAGS addFlags){
			CONST_DDBLTFLAGS flag;
			flag = CONST_DDBLTFLAGS.DDBLT_WAIT|addFlags;
			
			RECT dst = Util.toRECT(dstRegion);
			RECT src = Util.toRECT(srcRegion);
			//Util.clip( ref dst, ref src, clip );
		
			surface.Blt( ref dst, source, ref src, flag );
		}

		public Color ColorKey {
			set {
				if(value == Color.Empty )
					RemoveColorKey();
				else
					SetColorKey(value);
			}
			get { return colorKey; }
		}

		protected void SetColorKey(Color c){
			colorKey = c;
			colorKeyValue = PixelFormatUtil.ValueOf(c,colorMode);
			DDCOLORKEY key = new DDCOLORKEY();
			key.high = key.low = (int)colorKeyValue;
			surface.SetColorKey( CONST_DDCKEYFLAGS.DDCKEY_SRCBLT, ref key );
		}

		protected void RemoveColorKey(){
			unsafe {
				colorKeyValue = 0;
				colorKey = Color.Empty;
				surface.SetColorKey( CONST_DDCKEYFLAGS.DDCKEY_DESTBLT, ref *(DDCOLORKEY*)new IntPtr(0));
			}
		}		

		internal protected int GetPixel(int x, int y) {
			DDSURFACEDESC2  ddsd = new DDSURFACEDESC2();
			
			//  ロックしてポインタを得ます
			ddsd.lSize = Marshal.SizeOf(ddsd);
			RECT rect = new RECT();
			rect.Left = rect.Right = x;
			rect.Top = rect.Bottom =y;
			CONST_DDLOCKFLAGS flag = 
				CONST_DDLOCKFLAGS.DDLOCK_WAIT|
				CONST_DDLOCKFLAGS.DDLOCK_NOSYSLOCK|
				CONST_DDLOCKFLAGS.DDLOCK_READONLY;
			surface.Lock(ref rect, ref ddsd, flag, 0);
			int c = surface.GetLockedPixel(x,y);
			surface.Unlock(ref rect);
			
			return c;
		}

		#endregion

		#region ISurface メンバ
		public Size Size { get { return surfaceSize; } } 

		public PixelColorMode PixelColorMode { get { return colorMode; } }

		public void BitBlt(Point destpos, ISurface source, Rectangle region, int zoom, IPixelFilter filter) {
			DirectDrawSurface7 ssrc = ToNativeSurface(source);
			Size sz = ZoomUtil.Scale(region.Size,zoom);
			Rectangle dst = new Rectangle(destpos,sz);
			// TODO: how to apply filter?
			CONST_DDBLTFLAGS f = (source.ColorKey==Color.Empty)?0:CONST_DDBLTFLAGS.DDBLT_KEYSRC;
			
			DxBlt(dst,ssrc,region,f);
		}

		private static AlphaBlender alpha = new AlphaBlenderClass();

//		public void BitBltAlpha(Point destpos, ISurface source, Rectangle region, int zoom) {
//			DirectDrawSurface7 ssrc = ToNativeSurface(source);
//			Size sz = ZoomUtil.Scale(region.Size,zoom);
//			Rectangle dst = new Rectangle(destpos,sz);
			//CONST_DDBLTFLAGS f = (source.ColorKey==Color.Empty)?0:CONST_DDBLTFLAGS.DDBLT_KEYSRC;
//			RECT src = Util.toRECT(region);
//			alpha.bltAlphaFast( surface, ssrc,
//				dst.Left, dst.Top,
//				src.Left, src.Top, src.Right, src.Bottom,
//				(int)PixelFormatUtil.ValueOf(source.ColorKey,source.PixelColorMode) );
//		}

		public void BitBlt(Point destpos, ISurface source, Rectangle region, int zoom) {
			DirectDrawSurface7 ssrc = ToNativeSurface(source);
			Size sz = ZoomUtil.Scale(region.Size,zoom);
			Rectangle dst = new Rectangle(destpos,sz);
			CONST_DDBLTFLAGS f = (source.ColorKey==Color.Empty)?0:CONST_DDBLTFLAGS.DDBLT_KEYSRC;		
			DxBlt(dst,ssrc,region,f);
		}

		public void BitBlt(Point destpos, ISurface source, Rectangle region, int zoom, ISurfaceDrawer drawer){
			if(!(source is DD7Surface))
				BitBlt(destpos,source,region,zoom);
			else {
				DD7Surface srcSurf = source as DD7Surface;
				DDSURFACEDESC2  srcSD = new DDSURFACEDESC2();		
				DDSURFACEDESC2  dstSD = new DDSURFACEDESC2();		
				srcSD.lSize = Marshal.SizeOf(srcSD);
				dstSD.lSize = Marshal.SizeOf(dstSD);
				//  ロックしてポインタを得ます
				CONST_DDLOCKFLAGS write = CONST_DDLOCKFLAGS.DDLOCK_WAIT|CONST_DDLOCKFLAGS.DDLOCK_WRITEONLY;
				CONST_DDLOCKFLAGS read = CONST_DDLOCKFLAGS.DDLOCK_WAIT|CONST_DDLOCKFLAGS.DDLOCK_READONLY|CONST_DDLOCKFLAGS.DDLOCK_NOSYSLOCK;			
				RECT srcRec = Util.toRECT(region);
				RECT dstRec = Util.toRECT(destpos, ZoomUtil.Scale(region.Size,zoom));
				Util.clip(ref dstRec, ref srcRec, this.ClipRECT);
				NativeSurface.Lock(ref dstRec, ref dstSD, write, 0);
				srcSurf.NativeSurface.Lock(ref srcRec, ref srcSD, read, 0);
				DrawingDesc desc = new DrawingDesc();
				Util.PrepareDesc( ref desc, this, ref dstSD, ref dstRec, srcSurf, ref srcSD, ref srcRec, zoom );
				drawer.Blt(ref desc);
				NativeSurface.Unlock(ref srcRec);
				srcSurf.NativeSurface.Unlock(ref dstRec);
			}
		}

		public void Clear(Color fill) {
			surface.BltColorFill( ref clip, (int)PixelFormatUtil.ValueOf(fill,colorMode) );
		}

		public void Clear(Rectangle region, Color fill) {
			RECT r = Util.intersect( Util.toRECT(region), clip );
			surface.BltColorFill( ref r, (int)PixelFormatUtil.ValueOf(fill,colorMode) );
		}

		public bool IsOpaque(int x, int y) {			
			return GetPixel(x,y)==colorKeyValue;
		}

		/// <summary>
		/// Makes the bitmap of this surface.
		/// The caller needs to dispose the bitmap.
		/// </summary>
		public Bitmap CreateBitmap(PixelColorMode mode) {
			PixelFormat pf = PixelFormatUtil.ToPixelFormat(mode);
			Bitmap bmp;
			if(pf==PixelFormat.Undefined)
				bmp = new Bitmap( surfaceSize.Width, surfaceSize.Height);
			else
				bmp = new Bitmap( surfaceSize.Width, surfaceSize.Height, pf );
			using( GDIGraphics src = new GDIGraphics(surface) ) {
				using( Graphics dst = Graphics.FromImage(bmp) ) {
					IntPtr dstHDC = dst.GetHdc();
					IntPtr srcHDC = src.graphics.GetHdc();
					BitBlt( dstHDC, 0, 0, surfaceSize.Width, surfaceSize.Height, srcHDC, 0, 0, 0x00CC0020 );
					dst.ReleaseHdc(dstHDC);
					src.graphics.ReleaseHdc(srcHDC);
				}
			}
			return bmp;			
		}

		#endregion

		#region IDrawable メンバ
		public void DrawEx(ISurface dest, Point pos, int zoom, IPixelFilter filter, int frame){
			Rectangle r = new Rectangle(0,0,Size.Width,Size.Height);
			dest.BitBlt(pos, this, r, zoom, filter);
		}

		public void DrawEx(ISurface dest, Point pos, int zoom, ISurfaceDrawer drawer, int frame) {
			Rectangle r = new Rectangle(0,0,Size.Width,Size.Height);
			dest.BitBlt(pos, this, r, zoom, drawer);
		}

		public void Draw(ISurface dest, Point pos, int zoom, int frame) {
			Rectangle r = new Rectangle(0,0,Size.Width,Size.Height);
			dest.BitBlt(pos, this, r, zoom);
		}

		#endregion

		#region IDisposable メンバ

		public virtual void Dispose() {
			if(surface!=null)
				System.Runtime.InteropServices.Marshal.ReleaseComObject(surface);
			surface=null;
		}

		#endregion

		[DllImport("gdi32.dll")]
		protected static extern bool BitBlt(
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

	}

	#region GDIGraphics class
	/// <summary>
	/// Wraps a Surface object and provides GDI+ functionality
	/// via the graphics property.
	/// </summary>
	/// <remarks>this class is originaly implemented by K.Kawaguchi</remarks>
	public sealed class GDIGraphics : IDisposable {
		public readonly Graphics graphics;

		private readonly DirectDrawSurface7 surface;
		private readonly int hdc;

		public GDIGraphics( DirectDrawSurface7 _surface ) {
			this.surface = _surface;
			this.hdc = surface.GetDC();
			graphics = Graphics.FromHdc( new IntPtr(hdc) );
		}

		public void Dispose() {
			graphics.Dispose();
			surface.ReleaseDC(hdc);
		}
	}
	#endregion
}
