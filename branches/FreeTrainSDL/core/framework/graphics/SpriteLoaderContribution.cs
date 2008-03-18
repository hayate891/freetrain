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
using System.Xml;
using FreeTrain.Framework.plugin;

namespace FreeTrain.Framework.graphics
{
    /// <summary>
    /// SpriteLoaderContribution encapsulates the details of how a set of sprites
    /// are produced from a Picture.
    /// 
    /// This is a contribution so it can be implemented by plug-ins.
    /// 
    /// This class has many methods but one doesn't need
    /// to implement all of them.
    /// throw NotImplementedException in case of any error.
    /// 
    /// This default implementation just implements all the methods
    /// by returning an error.
    /// </summary>
    public abstract class SpriteLoaderContribution : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public SpriteLoaderContribution(XmlElement e) : base(e) { }

        /// <summary>
        /// Load a single sprite from the given manifest.
        /// </summary>
        public virtual Sprite load0D(XmlElement sprite)
        {
            throw new NotImplementedException(this.GetType().FullName + ".load0D");
        }

        /// <summary>
        /// Load a set of sprites of size (x) from the given manifest.
        /// </summary>
        public virtual Sprite[] load1D(XmlElement sprite, int x)
        {
            throw new NotImplementedException(this.GetType().FullName + ".load1D");
        }

        /// <summary>
        /// Load a set of sprites of size (x,y) from the given manifest.
        /// </summary>
        public virtual Sprite[,] load2D(XmlElement sprite, int x, int y, int height)
        {
            throw new NotImplementedException(this.GetType().FullName + ".load2D");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="sz"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Sprite[,] load2D(XmlElement sprite, Size sz, int height)
        {
            return load2D(sprite, sz.Width, sz.Height, height);
        }

        /// <summary>
        /// Load a set of sprites of size (x,y,z) from the given manifest.
        /// </summary>
        public virtual Sprite[, ,] load3D(XmlElement sprite, int x, int y, int z)
        {
            throw new NotImplementedException(this.GetType().FullName + ".load3D");
        }
    }
}
