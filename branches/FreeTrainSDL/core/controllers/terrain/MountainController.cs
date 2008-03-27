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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FreeTrain.Views.Map;
using FreeTrain.Util;
using FreeTrain.World;
using FreeTrain.World.Terrain;
using FreeTrain.Framework;

namespace FreeTrain.Controllers.Terrain
{
    /// <summary>
    /// Manipulates mountains
    /// </summary>
    public partial class MountainController : AbstractControllerImpl
    {
        #region Singleton instance management
        /// <summary>
        /// Creates a new controller window, or active the existing one.
        /// </summary>
        public static void create()
        {
            if (theInstance == null)
                theInstance = new MountainController();
            theInstance.Show();
            theInstance.Activate();
        }

        private Label label1;
        private GroupBox groupBox1;
        private FreeTrain.Controls.IndexSelector selSize;

        public static MountainController theInstance;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            theInstance = null;
        }
        #endregion
        private Bitmap previewBitmap;
        /// <summary>
        /// 
        /// </summary>
        public MountainController()
        {
            InitializeComponent();
            previewBitmap = ResourceUtil.loadSystemBitmap("Terrain.bmp");
            preview.Image = previewBitmap;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }
        /// <summary>
        /// 
        /// </summary>
        public override LocationDisambiguator Disambiguator { get { return GroundDisambiguator.theInstance; } }

        private bool isRaising
        {
            get
            {
                return buttonUp.Checked ^ (SdlDotNet.Input.Keyboard.IsKeyPressed(SdlDotNet.Input.Key.LeftControl) || SdlDotNet.Input.Keyboard.IsKeyPressed(SdlDotNet.Input.Key.RightControl));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void OnMouseMove(MapViewWindow view, Location loc, Point ab)
        {
            if (SdlDotNet.Input.Keyboard.IsKeyPressed(SdlDotNet.Input.Key.LeftShift) || SdlDotNet.Input.Keyboard.IsKeyPressed(SdlDotNet.Input.Key.RightShift))
            {
                loc = selectVoxel(view, loc, ab);
                raiseLowerLand(loc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void OnClick(MapViewWindow view, Location loc, Point ab)
        {
            loc = selectVoxel(view, loc, ab);
            raiseLowerLand(loc);
        }

        private void raiseLowerLand(Location loc)
        {
            //int origLoc = loc.y;
            //for (int sizeX = 0; sizeX < (selSize.current + 1); sizeX++)
            //{
            //	for (int sizeY = 0; sizeY < (selSize.current + 1); sizeY++)
            //	{
            //		loc.y = origLoc + sizeY;
            if (isRaising) raise(loc);
            else lower(loc);
            //	}
            //	loc.x++;
            //}
        }

        /// <summary>
        /// Selects the south-western voxel of the point selected by the mouse.
        /// The loc parameter and the ab parameter should point to the same location.
        /// </summary>
        private Location selectVoxel(MapViewWindow view, Location loc, Point ab)
        {
            // top-left corner of the selected location
            Point vxl = WorldDefinition.World.fromXYZToAB(loc);

            Point offset = new Point(ab.X - vxl.X, ab.Y - vxl.Y);

            if (offset.X < 8) loc.x--;
            else
                if (offset.X >= 24) loc.y++;
                else
                {
                    MountainVoxel mv = MountainVoxel.get(loc);
                    int h0 = (mv != null) ? (int)mv.getHeight(Direction.NORTHEAST) : 0;
                    int h2 = (mv != null) ? (int)mv.getHeight(Direction.SOUTHWEST) : 0;

                    if (offset.Y >= (16 - (h0 + h2) * 4) / 2)
                    {
                        loc.x--; loc.y++;
                    }
                }

            return loc;
        }

        /// <summary>
        /// Checks the height agreement of the four corners adjacent to
        /// the north-eastern corner of the given location.
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        private bool isFourAdjacentCornersMatched(Location loc)
        {
            Direction d = Direction.NORTH;

            for (int i = 0; i < 4; i++)
            {
                if (!MountainVoxel.isCornerMatched(loc, d.left))
                    return false;

                loc += d;
                d = d.right90;
            }

            return true;
        }

        // clean it up by using MountainVoxel.isCornerMatched

        /// <summary>
        /// Return true iff the north-eastern corner of the given location
        /// can be raised by a quarter height unit.
        /// </summary>
        private bool canBeRaised(Location loc)
        {
            WorldDefinition w = WorldDefinition.World;

            if (!isFourAdjacentCornersMatched(loc)) return false;


            Voxel baseVoxel = w[loc];
            int glevel = w.getGroundLevel(loc);

            if (loc.z != glevel) return false;	//mountain can be placed only at the ground level

            // true if this ground level is too close to the roof.
            bool nearRoof = (glevel == WorldDefinition.World.Size.z - 1);

            for (int x = 0; x <= 1; x++)
            {
                for (int y = -1; y <= 0; y++)
                {
                    Location l = new Location(loc.x + x, loc.y + y, loc.z);
                    Direction d = Direction.get(1 - x * 2, -y * 2 - 1);	// corner to modify

                    if (w.isOutsideWorld(l))
                        continue;	// it's OK if it's beyond the border

                    Voxel v = w[l];

                    if (glevel != w.getGroundLevel(l))
                        return false;	// different ground level

                    if (v == null)
                        continue;	// this voxel is unoccupied.

                    if (v is MountainVoxel)
                    {
                        int h = ((MountainVoxel)v).getHeight(d);
                        if (h == 4)
                            return false;	// corner saturated.
                        if (nearRoof && h == 3)
                            return false;	// too close to the roof

                        continue;	// otherwise OK
                    }

                    return false;	// occupied for another purpose
                }
            }

            if (WorldDefinition.World.isOutsideWorld(loc))
                return false;

            return true;
        }

        /// <summary>
        /// Raises the north-eastern corner of the specified voxel
        /// </summary>
        /// <returns>false if the operation was unsuccessful.</returns>
        private bool raise(Location loc)
        {
            WorldDefinition w = WorldDefinition.World;

            // make sure that four surrounding voxels can be raised,
            // and the ground levels of them are the same
            if (!canBeRaised(loc))
                return false;

            // then actually change the terrain
            for (int x = 0; x <= 1; x++)
            {
                for (int y = -1; y <= 0; y++)
                {
                    Location l = new Location(loc.x + x, loc.y + y, loc.z);

                    Voxel vx = w[l];
                    if (vx is WorldDefinition.OutOfWorldVoxel)
                        continue;	// this is beyond the border

                    MountainVoxel v = vx as MountainVoxel;

                    Direction d = Direction.get(1 - x * 2, -y * 2 - 1);	// corner to modify

                    if (v == null)
                        v = new MountainVoxel(l, 0, 0, 0, 0);

                    // raise the corner
                    v.setHeight(d, v.getHeight(d) + 1);

                    if (v.isSaturated)
                    {
                        // if the voxel is saturated, raise the ground level
                        w.raiseGround(l);
                        w.remove(l);	// remove this voxel
                    }
                }
            }

            return true;
        }


        // clean it up by using MountainVoxel.isCornerMatched
        private bool canBeLowered(ref Location loc)
        {
            WorldDefinition world = WorldDefinition.World;

            if (!isFourAdjacentCornersMatched(loc)) return false;

            MountainVoxel mvBase = MountainVoxel.get(loc);
            if (mvBase != null)
            {
                if (mvBase.getHeight(Direction.NORTHEAST) == 0)
                    return false;	// other corners need to be lowered first.
            }
            else
            {
                int glevel = world.getGroundLevel(loc);
                if (glevel != loc.z && glevel != loc.z - 1)
                    return false;
                if (loc.z == 0)
                    return false;	// can't dig deeper
                loc.z--;
            }

            // check other voxels
            for (int x = 0; x <= 1; x++)
            {
                for (int y = -1; y <= 0; y++)
                {
                    Location l = new Location(loc.x + x, loc.y + y, loc.z);

                    if (MountainVoxel.get(l) != null)
                        continue;	// if it's mountain, OK.

                    // otherwise, make sure that nothing is on it.
                    if (WorldDefinition.World[l.x, l.y, l.z + 1] != null)
                        return false;
                    // and nothing is under it
                    if (WorldDefinition.World[l.x, l.y, l.z] != null)
                        return false;
                }
            }

            if (WorldDefinition.World.isOutsideWorld(loc))
                return false;

            return true;
        }

        /// <summary>
        /// Lowers the north-eastern corner of the specified voxel.
        /// </summary>
        /// <returns>false if the operation was unsuccessful.</returns>
        private bool lower(Location loc)
        {

            WorldDefinition world = WorldDefinition.World;

            if (!canBeLowered(ref loc)) return false;


            // then actually change the terrain
            for (int x = 0; x <= 1; x++)
            {
                for (int y = -1; y <= 0; y++)
                {
                    Location l = new Location(loc.x + x, loc.y + y, loc.z);
                    Direction d = Direction.get(1 - x * 2, -y * 2 - 1);	// corner to modify

                    MountainVoxel mv = MountainVoxel.get(l);
                    if (mv == null)
                    {
                        WorldDefinition.World.lowerGround(l);
                        mv = new MountainVoxel(l, 4, 4, 4, 4);
                    }

                    mv.setHeight(d, mv.getHeight(d) - 1);

                    if (mv.isFlattened)	// completely flattened
                        world.remove(mv);
                }
            }

            return true;
        }
    }
}
