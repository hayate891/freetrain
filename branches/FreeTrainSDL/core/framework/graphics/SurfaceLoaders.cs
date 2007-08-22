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
	public interface SurfaceLoader {
		/// <summary>
		/// Fill the surface by the image and return the mask color.
		/// If the surface is null, the callee needs to allocate a new surface
		/// </summary>
		void load(ref Surface s);
        string fileName { get; }
	}

	/// <summary>
	/// Loads a surface from a bitmap
	/// </summary>
	public class BitmapSurfaceLoader : SurfaceLoader
	{
		/// <summary> File name of the bitmap. </summary>
		private readonly string _fileName;
        protected Surface daySurface;

        public string fileName { get { return _fileName; } }
		
		public BitmapSurfaceLoader( string _fileName) {
			this._fileName = _fileName;
            
		}

		public void load(ref Surface surface) {
            if (this.daySurface == null) this.daySurface = new Surface(fileName);
			if(surface!=null) surface.Dispose();
            surface = daySurface;
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
		//private readonly SurfaceLoader coreLoader;
        private Surface nightSurface;

        public string fileName { get { return _fileName;} }

        private string _fileName;

        public NightSurfaceLoader(string _fileName)
        {
			//Debug.Assert(_core!=null);
			//this.coreLoader = _core;
            this._fileName = _fileName;

		}

		//[DllImport("DirectDraw.AlphaBlend.dll")]
		//private static extern int buildNightImage( DxVBLib.DirectDrawSurface7 surface);

		public virtual void load(ref Surface surface) {
            if (this.nightSurface == null)
            {
                this.nightSurface = new Surface(_fileName);
                this.nightSurface.buildNightImage();
            }
            if (surface != null) surface.Dispose();
            surface = nightSurface;
		}
	}
}
