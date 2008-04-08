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
using System.Xml;
using FreeTrain.World;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.Framework.Graphics
{
    /// <summary>
    /// Draw an image in the picture as-is.
    /// </summary>
    [Serializable]
    public class SimpleSprite : ISprite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_picture"></param>
        /// <param name="_offset"></param>
        /// <param name="_origin"></param>
        /// <param name="_size"></param>
        public SimpleSprite(Picture _picture, Point _offset, Point _origin, Size _size)
        {
            this.picture = _picture;
            this.offset = _offset;
            this.origin = _origin;
            Debug.Assert(_size.Height != 0 && _size.Width != 0);
            this.size = _size;
        }


        /// <summary>
        /// Surface that contains the image.
        /// </summary>
        private Picture picture;

        /// <summary>
        /// 
        /// </summary>
        public Picture Picture
        {
            get { return picture; }
            set { picture = value; }
        }

        Size size;
        Point offset;

        /// <summary>
        /// The area of the image to be drawn.
        /// </summary>
        Point origin;

        /// <summary>
        /// 
        /// </summary>
        public Point Origin
        {
            get { return origin; }
            set { origin = value; }
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
            surface.blt(pt, picture.surface, origin, Size);
        }

        /// <summary>
        /// Draws the shape of this sprite in the specified color.
        /// </summary>
        public virtual void DrawShape(Surface surface, Point pt, Color color)
        {
            pt.X -= offset.X;
            pt.Y -= offset.Y;
            surface.bltShape(pt, picture.surface, origin, size, color);
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
            surface.bltAlpha(pt, picture.surface, origin, size);
        }

        /// <summary>
        /// 
        /// </summary>
        public Size Size { get { return size; } }
        /// <summary>
        /// 
        /// </summary>
        public Point Offset { get { return offset; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool HitTest(int x, int y) { return picture.surface.HitTest(x, y); }
    }


    /// <summary>
    /// SpriteFactory for SimpleSprite.
    /// </summary>
    public class SimpleSpriteFactory : SpriteFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override ISprite CreateSprite(Picture picture, Point offset, Point origin, Size size)
        {
            return new SimpleSprite(picture, offset, origin, size);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SimpleSpriteFactoryContributionImpl : SpriteFactoryContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public SimpleSpriteFactoryContributionImpl(XmlElement e) : base(e) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override SpriteFactory CreateSpriteFactory(XmlElement e)
        {
            return new SimpleSpriteFactory();
        }
    }
}
