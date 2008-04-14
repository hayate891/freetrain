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
using FreeTrain.World.Land;
using FreeTrain.World.Rail;
using FreeTrain.World.Structs;

namespace FreeTrain.World.Land.VinylHouse
{
    /// <summary>
    /// Land voxel with a fixed graphics and its population.
    /// </summary>
    [Serializable]
    public class VinylHouseVoxel : LandVoxel, IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="contrib"></param>
        /// <param name="index"></param>
        public VinylHouseVoxel(Location loc, VinylHouseBuilder contrib, int index)
            : base(loc)
        {
            this.contrib = contrib;
            this.index = index;

            this.stationListener = new StationListenerImpl(contrib.Population, loc);
        }

        /// <summary>
        /// 
        /// </summary>
        public override IEntity Entity { get { return this; } }

        /// <summary>
        /// 
        /// </summary>
        public override int EntityValue { get { return contrib.Price; } }

        /// <summary>
        /// 
        /// </summary>
        public override void OnRemoved()
        {
            stationListener.OnRemoved();
        }

        private readonly StationListenerImpl stationListener;

        private readonly VinylHouseBuilder contrib;

        private readonly int index;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        /// <param name="heightCutDiff"></param>
        public override void Draw(DrawContext surface, Point pt, int heightCutDiff)
        {
            // always draw it regardless of the height cut
            contrib.Sprites[index].Draw(surface.Surface, pt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public override object QueryInterface(Type aspect)
        {
            // if type.population is null, we don't have any population
            if (aspect == typeof(IStationListener))
                return stationListener;
            else
                return base.QueryInterface(aspect);
        }

    }
}
