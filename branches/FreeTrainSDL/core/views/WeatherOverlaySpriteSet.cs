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
using freetrain.framework.graphics;

namespace freetrain.views
{
    /// <summary>
    /// sprite images.
    /// </summary>
    public class WeatherOverlaySpriteSet
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly Sprite[] overlayImages;
        /// <summary>
        /// 
        /// </summary>
        public readonly Size imageSize;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="frameLength"></param>
        /// <param name="sz"></param>
        public WeatherOverlaySpriteSet(string pictureId, int frameLength, Size sz)
        {
            Picture pic = PictureManager.get(pictureId);
            imageSize = sz;
            overlayImages = new Sprite[frameLength];
            for (int i = 0; i < frameLength; i++)
                overlayImages[i] = new SimpleSprite(pic, new Point(0, 0), new Point(sz.Width * i, 0), sz);
        }
    }
}
