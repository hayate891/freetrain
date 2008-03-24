#region LICENSE
/*
 * Copyright (C) 2007 - 2008 FreeTrain Team (http://freetrain.sourceforge.net)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.Framework.Graphics
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
        void Load(ref Surface s);
        /// <summary>
        /// 
        /// </summary>
        string FileName { get; }
    }

    /// <summary>
    /// Loads a surface from a bitmap
    /// </summary>
    public class BitmapSurfaceLoader : SurfaceLoader
    {
        /// <summary> File name of the bitmap. </summary>
        private readonly string _fileName;
        /// <summary>
        /// 
        /// </summary>
        protected Surface DaySurface;
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get { return _fileName; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_fileName"></param>
        public BitmapSurfaceLoader(string _fileName)
        {
            this._fileName = _fileName;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        public void Load(ref Surface surface)
        {
            if (this.DaySurface == null) this.DaySurface = new Surface(FileName);
            if (surface != null) surface.Dispose();
            surface = DaySurface;
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
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get { return _fileName; } }

        private string _fileName;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_fileName"></param>
        public NightSurfaceLoader(string _fileName)
        {
            //Debug.Assert(_core!=null);
            //this.coreLoader = _core;
            this._fileName = _fileName;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        public virtual void Load(ref Surface surface)
        {
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
