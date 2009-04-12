using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using org.kohsuke.directdraw;

namespace freetrain.framework.graphics
{
	/// <summary>
	/// Function object that can load a picture into the given surface.
	/// </summary>
	public interface SurfaceLoader
    {
		/// <summary>
		/// Fill the surface by the image and return the mask color.
		/// If the surface is null, the callee needs to allocate a new surface
		/// </summary>
		Color load(ref AgateSurface inAgateSurface);
	}

	/// <summary>
	/// Loads a surface from a bitmap
	/// </summary>
	public class BitmapSurfaceLoader : SurfaceLoader
	{
		/// <summary> File name of the bitmap. </summary>
		private readonly string m_FileName;
		
		public BitmapSurfaceLoader( string inFileName) 
        {
			this.m_FileName = inFileName;
		}

		public Color load(ref AgateSurface inAgateSurface)
        {
			using( Bitmap bmp = new Bitmap(m_FileName) ) 
            {
               
				if(inAgateSurface==null) 
                {
					//inAgateSurface = ResourceUtil.directDraw.createOffscreenSurface( bmp.Size );
                    inAgateSurface = new AgateSurface( new AgateLib.DisplayLib.Surface(m_FileName) );
				}

                inAgateSurface.Draw( new Rectangle( new Point( 0, 0 ),bmp.Size ));
 
				return bmp.GetPixel(0,0);
			}
		}
	}

	/// <summary>
	/// Surface Loader that builds a night image in an automatic way.
	/// This surface loader uses another surface loader to load the surface,
	/// then change the picture on the surface.
	/// </summary>
	public class NightSurfaceLoader : SurfaceLoader
	{
		/// <summary>
		/// Base surface loader.
		/// </summary>
		private readonly SurfaceLoader coreLoader;

		public NightSurfaceLoader( SurfaceLoader _core ) 
        {
			Debug.Assert(_core!=null);
			this.coreLoader = _core;
		}

#if windows
		[DllImport("DirectDraw.AlphaBlend.dll")]
		private static extern int buildNightImage( DxVBLib.DirectDrawSurface7 surface);
#else
		[DllImport("DirectDraw.AlphaBlend.dll")]
		private static extern int buildNightImage( );
#warning STUB
#endif
		
		public virtual Color load(ref AgateSurface surface) 
        {
            /*
			Color c = coreLoader.load(ref surface);

			buildNightImage(surface.handle);

			buildNightImage();
#warning STUB
			return ColorMap.getNightColor(c);
             *
             */
            return new Color();
		}
	}
}
