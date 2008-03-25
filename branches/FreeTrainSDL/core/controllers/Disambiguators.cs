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
using FreeTrain.World;
using FreeTrain.World.Rail;

namespace FreeTrain.Controllers
{
    /// <summary>
    /// LocationDisambiguator implementation that prefers
    /// a location with a railroad.
    /// </summary>
    public class RailRoadDisambiguator : LocationDisambiguator
    {
        // the singleton instance
        /// <summary>
        /// 
        /// </summary>
        public static LocationDisambiguator theInstance = new RailRoadDisambiguator();
        private RailRoadDisambiguator() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool isSelectable(Location loc)
        {
            // if there's any rail roads, fine
            if (RailRoad.get(loc) != null) return true;

            // or if we hit the ground
            if (WorldDefinition.World.getGroundLevel(loc) >= loc.z) return true;

            return false;
        }
    }

    /// <summary>
    /// LocationDisambiguator that prefers the surface level.
    /// </summary>
    public class GroundDisambiguator : LocationDisambiguator
    {
        // the singleton instance
        /// <summary>
        /// 
        /// </summary>
        public static LocationDisambiguator theInstance = new GroundDisambiguator();
        private GroundDisambiguator() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool isSelectable(Location loc)
        {
            return loc.z == WorldDefinition.World.getGroundLevel(loc);
        }
    }

    /// <summary>
    /// LocationDisambiguator that only allows locations in the same level
    /// </summary>
    public class SameLevelDisambiguator : LocationDisambiguator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        public SameLevelDisambiguator(int height) { this.height = height; }
        private readonly int height;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool isSelectable(Location loc)
        {
            return loc.z == height;
        }
    }

    // TODO: other disambiguators
}
