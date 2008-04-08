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

namespace FreeTrain.Framework.Graphics
{
    /// <summary>
    /// Sprite that directly draws from a surface.
    /// </summary>
    public class DirectSprite : ISprite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_surface"></param>
        /// <param name="_offset"></param>
        /// <param name="_origin"></param>
        /// <param name="_size"></param>
        public DirectSprite(Surface _surface, Point _offset, Point _origin, Size _size)
        {
            this.surface = _surface;
            this.offset = _offset;
            this.origin = _origin;
            this.size = _size;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_surface"></param>
        /// <param name="_offset"></param>
        public DirectSprite(Surface _surface, Point _offset)
            : this(_surface, _offset, new Point(0, 0), _surface.Size) { }


        /// <summary>
        /// Surface that contains the image.
        /// </summary>
        private Surface surface;

        protected Surface Surface
        {
            get { return surface; }
            set { surface = value; }
        }

        /// <summary>
        /// The point in the image that will be aligned to
        /// the left-top corner of a voxel.
        Point offset;

        /// <summary>
        /// 
        /// </summary>
        public Point Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        Point origin;

        /// <summary>
        /// 
        /// </summary>
        public Point Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        Size size;

        /// <summary>
        /// 
        /// </summary>
        public Size Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        public virtual void Draw(Surface surface, Point pt)
        {
            pt.X -= offset.X;
            pt.Y -= offset.Y;
            surface.blt(pt, this.surface, origin, size);
        }

        /// <summary>
        /// Draws the shape of this sprite in the specified color.
        /// </summary>
        public virtual void DrawShape(Surface surface, Point pt, Color color)
        {
            pt.X -= offset.X;
            pt.Y -= offset.Y;
            surface.bltShape(pt, this.surface, origin, size, color);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        public virtual void DrawAlpha(Surface surface, Point pt)
        {
            pt.X -= offset.X;
            pt.Y -= offset.Y;
            surface.bltAlpha(pt, this.surface, origin, size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool HitTest(int x, int y)
        {
            return surface.HitTest(x, y);
        }
    }
}
