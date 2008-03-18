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
using FreeTrain.world;
using SDL.net;

namespace FreeTrain.Framework.graphics
{
    /// <summary>
    /// Draw an image in the picture as-is.
    /// </summary>
    [Serializable]
    public class SimpleSprite : Sprite
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
            this._offset = _offset;
            this.origin = _origin;
            Debug.Assert(_size.Height != 0 && _size.Width != 0);
            this._size = _size;
        }


        /// <summary>
        /// Surface that contains the image.
        /// </summary>
        public Picture picture;

        /// <summary>
        /// The point in the image that will be aligned to
        /// the left-top corner of a voxel.
        /// </summary>
        [CLSCompliant(false)]
        protected readonly Point _offset;

        /// <summary>
        /// The area of the image to be drawn.
        /// </summary>
        protected readonly Point origin;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected readonly Size _size;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        public virtual void draw(Surface surface, Point pt)
        {
            pt.X -= _offset.X;
            pt.Y -= _offset.Y;
            surface.blt(pt, picture.surface, origin, size);
        }

        /// <summary>
        /// Draws the shape of this sprite in the specified color.
        /// </summary>
        public virtual void drawShape(Surface surface, Point pt, Color color)
        {
            pt.X -= _offset.X;
            pt.Y -= _offset.Y;
            surface.bltShape(pt, picture.surface, origin, _size, color);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        public virtual void drawAlpha(Surface surface, Point pt)
        {
            pt.X -= _offset.X;
            pt.Y -= _offset.Y;
            surface.bltAlpha(pt, picture.surface, origin, _size);
        }

        /// <summary>
        /// 
        /// </summary>
        public Size size { get { return _size; } }
        /// <summary>
        /// 
        /// </summary>
        public Point offset { get { return _offset; } }
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
        public override Sprite createSprite(Picture picture, Point offset, Point origin, Size size)
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
        public override SpriteFactory createSpriteFactory(XmlElement e)
        {
            return new SimpleSpriteFactory();
        }
    }
}
