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
using FreeTrain.Util;

namespace FreeTrain.World.Development
{
    /// <summary>
    /// ULV stands for "Unused Land Value."
    /// ULV is a property of a cube on the ground.
    /// 
    /// It consits of two parts; sum of the land values in the cube,
    /// and sum of the entity values in the cube.
    /// 
    /// This is an index of how well a space is utilized.
    /// </summary>
    [Serializable]
    public class ULV
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly int landValue;
        /// <summary>
        /// 
        /// </summary>
        public readonly int entityValue;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="landValue"></param>
        /// <param name="entityValue"></param>
        public ULV(int landValue, int entityValue)
        {
            this.landValue = landValue;
            this.entityValue = entityValue;
        }

        /// <summary>
        /// Creates a ULV of the specified surface.
        /// </summary>
        /// <param name="cube">
        /// a surface specified as a cube. The height of the cube must be zero.
        /// </param>
        /// <returns>
        /// null if unable to to compute ULV or if any voxel in the cube is owned by
        /// the user.
        /// </returns>
        public static ULV create(Cube cube)
        {
            Debug.Assert(cube.sz == 0);

            int mx = cube.x2;
            int my = cube.y2;
            int z = cube.z1;

            int landValue = 0, entityValue = 0;
            WorldDefinition world = WorldDefinition.World;
            Set s = new Set();

            if (z < world.waterLevel) return null; // underwater

            for (int x = cube.x1; x < mx; x++)
            {
                for (int y = cube.y1; y < my; y++)
                {

                    if (world.getGroundLevel(x, y) != z)
                        return null;	// not on the ground

                    Voxel v = world[x, y, z];
                    if (v != null)
                    {
                        IEntity e = v.entity;
                        if (e.isOwned)	// cannot reclaim this voxel.
                            return null;

                        if (!s.contains(e))
                        { // new entity
                            s.add(e);
                            entityValue += e.entityValue;
                        }
                    }

                    landValue += world.landValue[new Location(x, y, z)];
                }
            }

            return new ULV(landValue, entityValue);
        }
    }
}
