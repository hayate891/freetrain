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
using FreeTrain.Framework.Graphics;

namespace FreeTrain.World
{
    /// <summary>
    /// A block in the game world.
    /// 
    /// The voxel is the unit of the game world. The game world consists of a cube of
    /// voxels, and this is the base class of such voxels.
    /// </summary>
    [Serializable]
    public abstract class Voxel
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract Location location { get; }
        /// <summary>
        /// 
        /// </summary>
        protected bool showFence = true;
        /// <summary>
        /// 
        /// </summary>
        public virtual bool transparent { get { return false; } }

        /// <summary>
        /// Draws this voxel
        /// </summary>
        /// <param name="heightCutDiff">
        /// heightCut - Z.
        ///	0 if this voxel is located to the "edge" of the height cut.
        ///	negative value if the view is not in the height cut mode.
        ///	positive values if this voxel is located below the cut height
        ///	(the value will be the difference between the height of
        ///	this voxel and the cut height.)
        /// </param>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        public abstract void draw(DrawContext display, Point pt, int heightCutDiff);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="heightCutDiff"></param>
        public void drawVoxel(DrawContext display, Point pt, int heightCutDiff)
        {
            if (showFence)
            {
                // draw behind fence 
                drawBehindFence(display, pt);
                draw(display, pt, heightCutDiff);
                drawFrontFence(display, pt);
                //draw front fence 
            }
            else
                draw(display, pt, heightCutDiff);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Color getColorOfTile();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="f"></param>
        public abstract void setFence(Direction d, Fence f);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public abstract Fence getFence(Direction d);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        protected abstract void drawFrontFence(DrawContext display, Point pt);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        protected abstract void drawBehindFence(DrawContext display, Point pt);

        /// <summary>
        /// Processes a mouse click.
        /// </summary>
        /// <returns>true if a mouse click event is "consumed"</returns>
        public virtual bool onClick() { return false; }

        /// <summary>
        /// Query this voxel to return some "aspect" of it.
        /// 
        /// Aspect is usually a tear-off interface that allows
        /// voxels to be extended through compositions.
        /// 
        /// The queryInterface method of voxels should delegate to
        /// the queryInterface method of entities.
        /// </summary>
        /// <returns>null if the given aspect is not supported.</returns>
        public virtual object queryInterface(Type aspect) { return null; }

        /// <summary>
        /// Calls immediately after the voxel is removed from the world.
        /// </summary>
        public virtual void onRemoved() { }
        // TODO: is this method necessary


        /// <summary>
        /// Short-cut to call the getLandPrice method of the World class.
        /// </summary>
        public int landPrice
        {
            get
            {
                return (int)WorldDefinition.World.landValue[location];
            }
        }

        /// <summary>
        /// Obtains a reference to the entity that includes this voxel.
        /// </summary>
        public abstract Entity entity { get; }

    }

    /// <summary>
    /// Partial implementation for most of the voxel.
    /// </summary>
    [Serializable]
    public abstract class AbstractVoxelImpl : Voxel
    {
        /// <summary>
        /// 
        /// </summary>
        protected Fence[] fence = new Fence[4];
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        protected AbstractVoxelImpl(int x, int y, int z)
            : this(new Location(x, y, z))
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_loc"></param>
        protected AbstractVoxelImpl(Location _loc)
        {
            this.loc = _loc;
            WorldDefinition.World[loc] = this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wloc"></param>
        protected AbstractVoxelImpl(WorldLocator wloc)
        {
            this.loc = wloc.location;
            wloc.world[loc] = this;
        }

        private readonly Location loc;
        /// <summary>
        /// 
        /// </summary>
        public override Location location { get { return loc; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        protected override void drawFrontFence(DrawContext display, Point pt)
        {
            Fence f;
            f = fence[(Direction.SOUTH).index / 2];
            if (f != null)
                f.drawFence(display.Surface, pt, Direction.SOUTH);
            f = fence[(Direction.WEST).index / 2];
            if (f != null)
                f.drawFence(display.Surface, pt, Direction.WEST);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        protected override void drawBehindFence(DrawContext display, Point pt)
        {
            Fence f;
            f = fence[(Direction.NORTH).index / 2];
            if (f != null)
                f.drawFence(display.Surface, pt, Direction.NORTH);
            f = fence[(Direction.EAST).index / 2];
            if (f != null)
                f.drawFence(display.Surface, pt, Direction.EAST);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="f"></param>
        public override void setFence(Direction d, Fence f)
        {
            fence[d.index / 2] = f;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public override Fence getFence(Direction d)
        {
            return fence[d.index / 2];

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Color getColorOfTile()
        {
            //return Color.Beige;   
            if (this.GetType().Name == "VoxelImpl") return Color.AliceBlue;
            else return Color.Green;
        }

    }


    /// <summary>
    /// Voxel can additionally implement this interface to
    /// control the painting of the ground surface.
    /// 
    /// The drawing routine queries this interface for voxels
    /// that are directly above and below the surface.
    /// </summary>
    public interface HoleVoxel
    {
        /// <summary>
        /// Returns false to prevent the ground surface to be drawn.
        /// </summary>
        /// <param name="above">
        /// True if the callee is located directly above the ground,
        /// false if directly below the ground.
        /// </param>
        bool drawGround(bool above);
    }

    /// <summary>
    /// The interface called when the fence should be drawn.
    /// </summary>
    public interface Fence
    {
        /// <summary>
        /// called when the fehce should be drawn.
        /// </summary>
        /// <param name="d">one of the 4 directions (N,E,W,S)</param>
        /// <param name="pt"></param>
        /// <param name="surface"></param>
        void drawFence(Surface surface, Point pt, Direction d);

        /// <summary>
        /// 
        /// </summary>
        string fence_id { get; }
    }
}
