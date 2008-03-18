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
using FreeTrain.Framework.graphics;
using FreeTrain.Framework.plugin;

namespace FreeTrain.Contributions.road
{
    /// <summary>
    /// RoadContribution for "org.kohsuke.freetrain.road.pc-9801fa" plug-in
    /// TODO: move to its own DLL.
    /// </summary>
    [Serializable]
    public class A3RoadContribution : AbstractRoadContributionImpl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public A3RoadContribution(XmlElement e)
            : base(e)
        {
            // load resource, but don't dispose it as sprites will still refer to this surface.
            Picture picture = getPicture(e);

            sprites = new Sprite[3];
            for (int i = 0; i < 3; i++)
                sprites[i] = new SimpleSprite(picture, new Point(0, 16), new Point(i * 32, 0), new Size(32, 32));
        }

        /// <summary>
        /// three sprites (0:E-W, 1:N-S, 2:cross)
        /// </summary>
        private readonly Sprite[] sprites;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        protected internal override Sprite getSprite(byte idx)
        {
            switch (idx)
            {
                case 2:
                case 8:
                case 10:
                    return sprites[0];
                case 1:
                case 4:
                case 5:
                    return sprites[1];
                default:
                    return sprites[2];
            }
        }
    }
}
