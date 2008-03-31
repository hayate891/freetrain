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
using FreeTrain.Contributions.Land;
using FreeTrain.Framework.Plugin;
using FreeTrain.World.Rail;
using FreeTrain.World.Structs;

namespace FreeTrain.World.Land
{
    /// <summary>
    /// Land voxel with a fixed graphics and its population.
    /// </summary>
    [Serializable]
    public class StaticLandVoxel : LandVoxel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="contrib"></param>
        public StaticLandVoxel(Location loc, StaticLandBuilder contrib)
            : base(loc)
        {
            this.contrib = contrib;

            if (contrib.population != null)
                this.stationListener = new StationListenerImpl(contrib.population, loc);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void onRemoved()
        {
            if (stationListener != null)
                stationListener.onRemoved();
        }
        /// <summary>
        /// 
        /// </summary>
        public override int entityValue { get { return contrib.price; } }

        private readonly StationListenerImpl stationListener;

        private readonly StaticLandBuilder contrib;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        /// <param name="heightCutDiff"></param>
        public override void draw(DrawContext surface, Point pt, int heightCutDiff)
        {
            // always draw it regardless of the height cut
            contrib.sprite.draw(surface.Surface, pt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public override object queryInterface(Type aspect)
        {
            // if type.population is null, we don't have any population
            if (aspect == typeof(StationListener))
                return stationListener;
            else
                return base.queryInterface(aspect);
        }
    }
}
