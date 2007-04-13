using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using DxVBLib;
using nft.framework;
using nft.framework.drawing;

namespace nft.drawing.ddraw7 
{
	/// <summary>
	/// DirectDraw root class.
	/// </summary>
	/// <remarks>Some part of these methods are originaly implemented by K.Kawaguchi</remarks>
	public class DD7GraphicManager : IDisposable, IGraphicManager
	{
		static protected DD7GraphicManager theInstance;
		static internal protected DD7GraphicManager TheInstance { get { return theInstance; } }
		
		private WindowedDDraw7 ddraw;
		private readonly string name;
		private readonly string description;

		static protected CONST_DDSURFACECAPSFLAGS ToDDConstant(SurfaceAlloc place){
			CONST_DDSURFACECAPSFLAGS memoryPlace;
			switch(place) {
				case SurfaceAlloc.SystemMem:
					memoryPlace = CONST_DDSURFACECAPSFLAGS.DDSCAPS_SYSTEMMEMORY;
					break;
				case SurfaceAlloc.VideoMem:
					memoryPlace = CONST_DDSURFACECAPSFLAGS.DDSCAPS_VIDEOMEMORY;
					break;
				default:
					memoryPlace = 0;
					break;
			}
			return memoryPlace;
		}

		public DD7GraphicManager( XmlNode node ){
			XmlAttribute a = node.Attributes["name"];
			if(a!=null)
				name = a.Value;
			else
				name = "DirectDraw7 Graphics";
			XmlNode n = node.SelectSingleNode("description");
			if(n!=null)
				description = n.InnerText;
			else
				description = "Microsoft DirectDraw(DirectX7) Graphic Manager";
			ddraw = new WindowedDDraw7(new UserControl());
			if(theInstance!=null)
				theInstance.Dispose();
			theInstance = this;
		}

		/// <summary>
		/// System.Drawing.Imaging.PixelFormatに適した値をDDSURFACEDESC2.ddpfPixelFormatに設定
		/// </summary>
		/// <param name="ddsd"></param>
		/// <param name="format"></param>
		/// <returns>formatが未対応の場合false</returns>
		internal protected static bool SetPixelFormat(ref DDSURFACEDESC2 ddsd, PixelColorMode format){
			ddsd.ddpfPixelFormat.lSize = Marshal.SizeOf(ddsd.ddpfPixelFormat);
			ddsd.ddpfPixelFormat.lFlags = CONST_DDPIXELFORMATFLAGS.DDPF_RGB;

			switch(format){
				case PixelColorMode.RGB32Bit:
					ddsd.ddpfPixelFormat.lRGBBitCount	= 32;
					ddsd.ddpfPixelFormat.lRBitMask		= 0x00FF0000;
					ddsd.ddpfPixelFormat.lGBitMask		= 0x0000FF00;
					ddsd.ddpfPixelFormat.lBBitMask		= 0x000000FF;
					break;
				case PixelColorMode.RGB24Bit:
					ddsd.ddpfPixelFormat.lRGBBitCount	= 24;
					ddsd.ddpfPixelFormat.lRBitMask		= 0x00FF0000;
					ddsd.ddpfPixelFormat.lGBitMask		= 0x0000FF00;
					ddsd.ddpfPixelFormat.lBBitMask		= 0x000000FF;
					break;
				case PixelColorMode.RGB16Bit565:
					ddsd.ddpfPixelFormat.lRGBBitCount	= 16;
					ddsd.ddpfPixelFormat.lRBitMask		= 0x0000F800;
					ddsd.ddpfPixelFormat.lGBitMask		= 0x000007E0;
					ddsd.ddpfPixelFormat.lBBitMask		= 0x0000001F;
					break;
				case PixelColorMode.RGB16Bit555:
					ddsd.ddpfPixelFormat.lRGBBitCount	= 16;
					ddsd.ddpfPixelFormat.lRBitMask		= 0x00007C00;
					ddsd.ddpfPixelFormat.lGBitMask		= 0x000003E0;
					ddsd.ddpfPixelFormat.lBBitMask		= 0x0000001F;
					break;
				default:
					// format not supported.
					return false;
			}
			return true;
		}

		internal protected static PixelColorMode GetPixelColorMode(DirectDrawSurface7 surface) { 
			DDSURFACEDESC2	ddsd = new DDSURFACEDESC2();
			surface.GetSurfaceDesc(ref ddsd);
			int bitcount = ddsd.ddpfPixelFormat.lRGBBitCount;
			switch( bitcount ){
				case 32:
					return PixelColorMode.RGB32Bit;
				case 24:
					return PixelColorMode.RGB24Bit;
				case 16:
					if ( ddsd.ddpfPixelFormat.lRBitMask == ( 31 << 11 ) &&
						ddsd.ddpfPixelFormat.lGBitMask == ( 63 << 5 ) &&
						ddsd.ddpfPixelFormat.lBBitMask == 31 ) {
						return PixelColorMode.RGB16Bit565;
					}

					// If we are in 555 mode ...
					if ( ddsd.ddpfPixelFormat.lRBitMask == ( 31 << 10 ) &&
						ddsd.ddpfPixelFormat.lGBitMask == ( 31 << 5 ) &&
						ddsd.ddpfPixelFormat.lBBitMask == 31 ) {
						return PixelColorMode.RGB16Bit555;
					}
					break;
			}
			return PixelColorMode.Unknown;
		}

		#region IGraphicManager メンバ
		public PixelColorMode CurrentColorMode { 
			get{ 
				return GetPixelColorMode(ddraw.PrimarySurface.NativeSurface);
			}
		}

		public int TotalVideoMemory {
			get {
				DDSCAPS2 ddcaps = new DDSCAPS2();
				ddcaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_VIDEOMEMORY;
				return ddraw.Handle.GetAvailableTotalMem(ref ddcaps);
			}
		}

		public int AvailableVideoMemory {
			get {
				DDSCAPS2 ddcaps = new DDSCAPS2();
				ddcaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_VIDEOMEMORY;
				return ddraw.Handle.GetFreeMem(ref ddcaps);
			}
		}
		
		public ISprite CreateSprite( ITexture texture ) {
			return new DD7Sprite(texture);
		}

		public ISimpleTexture CreateSimpleTexture(string filepath, Rectangle region, Point offset){
			Bitmap bmp = null;
			try{
				bmp = new Bitmap(filepath);
			}catch(Exception ex){
				throw new ArgumentException("cannot load image:"+filepath, ex );
			}
			ISurface s = CreateSurfaceFromBitmap(bmp, region, PixelColorMode.Default, SurfaceAlloc.SystemMem);
			return new DD7Texture(s,new Rectangle(0,0,region.Width,region.Height),offset);
		}
		
		public ISimpleTexture CreateSimpleTexture(ISurface source, Rectangle region, Point offset){
			return new DD7Texture(source,region,offset);
		}

		public ISurface CreateOffscreenSurface(Size size, PixelColorMode mode, SurfaceAlloc alloc) {
			//Debug.Assert(size.Height>0 && size.Width>0,"invalid size");
			DDSURFACEDESC2 sd = new DDSURFACEDESC2();
			sd.lSize = Marshal.SizeOf(sd);
			sd.lFlags =	CONST_DDSURFACEDESCFLAGS.DDSD_CAPS |
				CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH |
				CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT;
			if(SetPixelFormat(ref sd, mode ))
				sd.lFlags |= CONST_DDSURFACEDESCFLAGS.DDSD_PIXELFORMAT;
			sd.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN|ToDDConstant(alloc);
			sd.lHeight	= size.Height;
			sd.lWidth	= size.Width;
			try {
				return new DD7Surface(ddraw.Handle.CreateSurface( ref sd ));
			}
			catch(Exception e) {
				//for safe
				Debug.WriteLine(string.Format("{0}:({1}x{2})",e.Message,size.Width,size.Height));
				sd.ddsCaps.lCaps |= CONST_DDSURFACECAPSFLAGS.DDSCAPS_SYSTEMMEMORY;
				return new DD7Surface(ddraw.Handle.CreateSurface( ref sd ));
			}
		}

		public ISurface CreateSurfaceFromBitmap(Bitmap source, PixelColorMode mode, SurfaceAlloc alloc) {
			Rectangle r = new Rectangle(0,0,source.Width,source.Height);
			return CreateSurfaceFromBitmap(source, r, mode, alloc);
		}

		public ISurface CreateSurfaceFromBitmap(Bitmap source, Rectangle region, PixelColorMode mode, SurfaceAlloc alloc) {
//			if(mode==PixelColorMode.Default) {
//				mode = PixelFormatUtil.ToPixelColorMode(source.PixelFormat);
//				if(mode==PixelColorMode.Unknown)
//					mode = PixelColorMode.Default;
//			}
			DD7Surface s = (DD7Surface)CreateOffscreenSurface(region.Size, mode, alloc);
			using(GDIGraphics g=new GDIGraphics(s.NativeSurface)) {
				g.graphics.FillRectangle(new SolidBrush(Color.Black), region);
				// without the size parameter, it doesn't work well with non-standard DPIs.
				Rectangle dst = new Rectangle(0,0,region.Width,region.Height);
				g.graphics.DrawImage( source, dst, region, GraphicsUnit.Pixel);
			}
			return s;
		}

		public DrawableControl CreateDrawableControl() {			
			return new DD7Control();
		}
		#endregion

		#region ISurfaceDrawer factories
		private IntersectDrawer drawerIntersect = new IntersectDrawer();
		private HalfAlphaDrawer drawerAlpha = new HalfAlphaDrawer();
		private SelectBrighterDrawer drawerBrighter = new SelectBrighterDrawer();
		/// <summary>
		/// create intersect drawer
		/// draws only when both source and destination pixel is not transpalent
		/// this drawer must be necessary for drawing perspective scenes
		/// </summary>
		/// <returns></returns>
		public ISurfaceDrawer GetIntersectDrawer(){
			return drawerIntersect;
		}
		/// <summary>
		/// create 50% alpha blend drawer
		/// </summary>
		/// <returns></returns>
		public ISurfaceDrawer GetHalfAlphaDrawer(){
			return drawerAlpha; 
		}
		/// <summary>
		/// create brighter drawer
		/// draws only when source pixel is brighter than destination
		/// this drawer is used for red sunset and morning haze
		/// </summary>
		/// <returns></returns>
		public ISurfaceDrawer GetBrighterDrawer(){
			return drawerBrighter; 
		}
		/// <summary>
		/// create color burn drawer
		/// this drawer is used for red sunset and morning haze
		/// </summary>
		/// <param name="c"></param>
		/// <param name="apply"></param>
		/// <returns></returns>
		public ISurfaceDrawer GetColorBurnDrawer(Color c, float apply){
			return new ColorBurnDrawer(c,apply); 
		}
		/// <summary>
		/// create mono color drawer
		/// draws gray-scaled image with specified color
		/// this drawer is used for setting highlihgt some sprites.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public ISurfaceDrawer GetMonoColorDrawer(Color c){
			return new MonoColorDrawer(c);
		}
		#endregion

		#region IDisposable メンバ

		public void Dispose() {
			ddraw.Dispose();
		}

		#endregion

		#region IGlobalModule メンバ

		public string Description {	get { return description; } }

		public string Name { get { return name; } }

		public Type RegistType {
			get {
				Type t = this.GetType().GetInterface("IGraphicManager");
				Debug.Assert((t!=null)&&(null!=this as IGraphicManager));
				return t;
			}
		}

		#endregion
	}

}
