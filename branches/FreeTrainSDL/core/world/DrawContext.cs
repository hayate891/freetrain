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
using System.Drawing;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.World
{
    /// <summary>
    /// Voxels will access GDI DC or DirectDraw surface
    /// via this object.
    /// 
    /// This object minimizes the number of "context switch"
    /// between GDI DC and DirectDraw.
    /// </summary>
    public class DrawContext : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        public DrawContext(Surface surface) 
        { 
            this.surface = surface;
        }

        private Surface surface;
 
        /// <summary>
        /// 
        /// </summary>
        public Surface Surface
        {
            get
            {
                return surface;
            }
        }

        /// <summary>
        /// Only the owner of the DrawContext class can
        /// call this method.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// MapOverlay can use this property to pass parameters among
        /// various callbacks.
        /// </summary>
        private object tag;

        /// <summary>
        /// 
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
    }
}
