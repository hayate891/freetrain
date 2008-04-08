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
using FreeTrain.Controllers;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework;
using FreeTrain.Views;
using FreeTrain.World;
using FreeTrain.World.Rail;
using FreeTrain.World.Terrain;
using FreeTrain.World.Land;

namespace FreeTrain.Contributions.Land
{
    /// <summary>
    /// Removes any land voxel in the region.
    /// </summary>
    public class Bulldozer : LandBuilderContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public Bulldozer(XmlElement e)
            : base(e)
        {
        }

        /// <summary>
        /// Gets the land that should be used to fill (x,y) within [x1,y1]-[x2,y2] (inclusive).
        /// </summary>
        public override void create(int x1, int y1, int x2, int y2, int z, bool owned)
        {
            bulldoze(new Location(x1, y1, z), new Location(x2, y2, z));
            WorldDefinition.World.OnVoxelUpdated(new Cube(x1, y1, z, x2 - x1 + 1, y2 - y1 + 1, 1));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc1"></param>
        /// <param name="loc2"></param>
        public static void bulldoze(Location loc1, Location loc2)
        {
            int z = loc1.z;
            for (int x = loc1.x; x <= loc2.x; x++)
            {
                for (int y = loc1.y; y <= loc2.y; y++)
                {
                    // edited by 477 (04/02/14)
                    //if( World.world.isReusable(x,y,z) && World.world[x,y,z]!=null ) 
                    Voxel v = WorldDefinition.World[x, y, z];
                    if (v != null)
                    {
                        if (v is MountainVoxel)
                        {
                            MountainVoxel mv = v as MountainVoxel;
                            if (mv.isFlattened)
                                WorldDefinition.World.remove(x, y, z);
                            else
                                mv.removeTrees();
                        }
                        else if (v.entity != null)
                        {
                            v.entity.remove();
                        }
                        else
                        {
                            WorldDefinition.World.remove(x, y, z);
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Creates the preview image of the land builder.
        /// </summary>
        public override PreviewDrawer CreatePreview(Size pixelSize)
        {
            return new PreviewDrawer(pixelSize, new Size(10, 10), 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateBuilder(IControllerSite site)
        {
            return CreateRemover(site);
        }

    }
}
