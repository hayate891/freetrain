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
using freetrain.world;
using SDL.net;

namespace freetrain.framework.graphics
{
    /// <summary>
    /// Draw an image in the picture as-is.
    /// </summary>
    [Serializable]
    public class SimpleSprite : Sprite
    {
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
        protected readonly Point _offset;

        /// <summary>
        /// The area of the image to be drawn.
        /// </summary>
        protected readonly Point origin;
        protected readonly Size _size;

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

        public virtual void drawAlpha(Surface surface, Point pt)
        {
            pt.X -= _offset.X;
            pt.Y -= _offset.Y;
            surface.bltAlpha(pt, picture.surface, origin, _size);
        }

        public Size size { get { return _size; } }
        public Point offset { get { return _offset; } }
        public bool HitTest(int x, int y) { return picture.surface.HitTest(x, y); }
    }


    /// <summary>
    /// SpriteFactory for SimpleSprite.
    /// </summary>
    public class SimpleSpriteFactory : SpriteFactory
    {
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
