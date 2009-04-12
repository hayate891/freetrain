using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AgateLib;
using System.Diagnostics;

namespace org.kohsuke.directdraw
{
	public enum DDSurfaceAllocation { Auto, ForceVideoMem, ForceSystemMem };
	/// <summary>
	/// DirectDraw root class.
	/// </summary>
	public class DirectDraw : IDisposable
	{
#if windows
		private static CONST_DDSURFACECAPSFLAGS memoryPlace = CONST_DDSURFACECAPSFLAGS.DDSCAPS_SYSTEMMEMORY;
#else
#warning STUB
#endif
		public static DDSurfaceAllocation SurfeceAllocation
		{
			get {
#if windows
				if((memoryPlace&CONST_DDSURFACECAPSFLAGS.DDSCAPS_VIDEOMEMORY)!=0)
					return DDSurfaceAllocation.ForceVideoMem;
				else if((memoryPlace&CONST_DDSURFACECAPSFLAGS.DDSCAPS_SYSTEMMEMORY)!=0)
					return DDSurfaceAllocation.ForceSystemMem;
				else
					return DDSurfaceAllocation.Auto;
#else
#warning STUB
				return DDSurfaceAllocation.Auto;
#endif
			}
			set {
#if windows
				switch(value)
				{
					case DDSurfaceAllocation.ForceSystemMem:
						memoryPlace = CONST_DDSURFACECAPSFLAGS.DDSCAPS_SYSTEMMEMORY;
						break;
					case DDSurfaceAllocation.ForceVideoMem:
						memoryPlace = CONST_DDSURFACECAPSFLAGS.DDSCAPS_VIDEOMEMORY;
						break;
					default:
						memoryPlace &= CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;
						break;
				}
#else
#warning STUB
#endif
			}
		}

#if windows
		protected DirectDraw7 handle;
#else
#warning STUB
#endif

		public DirectDraw() {
#if windows
			// initialize DirectDraw
			DirectX7 dxc = new DirectX7Class();
			handle = dxc.DirectDrawCreate("");

			handle.SetCooperativeLevel( 0, CONST_DDSCLFLAGS.DDSCL_NORMAL );	// window mode
#else
#warning STUB
#endif
		}

		public int totalVideoMemory {
			get {
#if windows
				DDSCAPS2 ddcaps = new DDSCAPS2();
				ddcaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_VIDEOMEMORY;
				return handle.GetAvailableTotalMem(ref ddcaps);
#else
#warning STUB
				return 0;
#endif
			}
		}

		public int availableVideoMemory {
			get {
#if windows
				DDSCAPS2 ddcaps = new DDSCAPS2();
				ddcaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_VIDEOMEMORY;
				return handle.GetFreeMem(ref ddcaps);
#else
#warning STUB
				return 0;
#endif
			}
		}

		public string displayModeName {
			get {
#if windows
				return GetDisplayModeName();
#else
#warning STUB
				return "";
#endif
			}
		}

#if windows
		[DllImport("DirectDraw.AlphaBlend.dll")]
		private static extern string GetDisplayModeName();
#else
#warning STUB
#endif


		public virtual void Dispose() {
#if windows
			handle=null;
#else
#warning STUB
#endif
		}


		/// <summary>
		/// Creates a blank off-screen surface with the specified size.
		/// </summary>
		public AgateSurface createOffscreenSurface( int width, int height ) {
#if windows
			DDSURFACEDESC2 sd = new DDSURFACEDESC2();
			sd.lSize = Marshal.SizeOf(sd);
			sd.lFlags =	CONST_DDSURFACEDESCFLAGS.DDSD_CAPS |
						CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH |
						CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT;

			sd.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN|memoryPlace;
			sd.lHeight	= height;
			sd.lWidth	= width;
			try
			{
				return new Surface(handle.CreateSurface( ref sd ));
			}
			catch(Exception e)
			{
				//for safe
				Debug.WriteLine(string.Format("{0}:({1}x{2})",e.Message,width,height));
				sd.ddsCaps.lCaps |= CONST_DDSURFACECAPSFLAGS.DDSCAPS_SYSTEMMEMORY;
				return new Surface(handle.CreateSurface( ref sd ));
			}
#else
#warning STUB
            // TODO: fixme
            return new AgateSurface();
            //return new AgateSurface( new AgateLib.DisplayLib.Surface( width, height ) );
#endif
		}

		public AgateSurface createOffscreenSurface( Size sz ) 
        {
			return createOffscreenSurface( sz.Width, sz.Height );
		}

		/// <summary>
		/// Creates an off-screen surface from an image.
		/// </summary>
		public AgateSurface createFromImage( Image img ) {
#if windows
			Surface s = createOffscreenSurface( img.Size );
			using(GDIGraphics g=new GDIGraphics(s)) {
				// without the size parameter, it doesn't work well with non-standard DPIs.
				g.graphics.DrawImage( img, new Rectangle( new Point(0,0), img.Size ) );
			}
			return s;
#else
#warning STUB
			AgateSurface s = createOffscreenSurface( img.Size );
			return s;
#endif
		}

		/// <summary>
		/// Creates an off-screen surface from an image
		/// and set the source key color to the color of the
		/// top-left corner of the image.
		/// </summary>
		public AgateSurface createSprite( Bitmap img ) {
			AgateSurface surface = createFromImage(img);
			surface.sourceColorKey = img.GetPixel(0,0);
			return surface;
		}



		/// <summary>
		/// Returns true if the given exception is thrown because of
		/// a lost surface.
		/// </summary>
		public static bool isSurfaceLostException( COMException e ) {
			return (uint)e.ErrorCode == (uint)0x887601C2;
		}
	}

	/// <summary>
	/// DirectDraw with the primary surface to the window of
	/// the specified control.
	/// </summary>
	public class WindowedDirectDraw : DirectDraw {
		private AgateSurface primary;
		public AgateSurface primarySurface { get { return primary; } }

		public WindowedDirectDraw( Control control ) {
			primary = createPrimarySurface();
#if windows
			// attach window clipper
			DirectDrawClipper cp = handle.CreateClipper(0);
			cp.SetHWnd( control.Handle.ToInt32() );
			primary.handle.SetClipper(cp);
			primary.clipRect = new Rectangle( int.MinValue/2, int.MinValue/2, int.MaxValue, int.MaxValue );
#else
#warning STUB
#endif
		}

		/// <summary>
		/// Creates the primary surface.
		/// </summary>
		private AgateSurface createPrimarySurface() {
#if windows
			DDSURFACEDESC2 sd= new DDSURFACEDESC2();


			sd.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
			sd.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_PRIMARYSURFACE;

			return new Surface(handle.CreateSurface(ref sd ));
#else
#warning STUB
			return new AgateSurface();
#endif
		}

		public override void Dispose() {
			primary=null;
		}
	}

}
