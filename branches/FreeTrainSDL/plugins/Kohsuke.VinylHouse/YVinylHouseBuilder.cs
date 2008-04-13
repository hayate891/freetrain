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
using System.Runtime.Serialization;
using System.Xml;
using FreeTrain.Framework.Graphics;
using FreeTrain.Contributions.Land;
using FreeTrain.Contributions.Population;
using FreeTrain.Controllers;
using FreeTrain.Framework.Plugin;
using FreeTrain.Views;

namespace FreeTrain.World.Land.VinylHouse
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class YVinylHouseBuilder : VinylHouseBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public YVinylHouseBuilder(XmlElement e) : base(e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        protected override int GetSpriteIndex(int x, int y, int x1, int y1, int x2, int y2)
        {
            if (y == y2) return 2;
            if (y == y1) return 0;
            return 1;
        }
    }
}
