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
        public abstract Location Location { get; }
        /// <summary>
        /// 
        /// </summary>
        private bool showFence = true;

        /// <summary>
        /// 
        /// </summary>
        protected bool ShowFence
        {
            get { return showFence; }
            set { showFence = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual bool Transparent { get { return false; } }

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
        public abstract void Draw(DrawContext display, Point pt, int heightCutDiff);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="heightCutDiff"></param>
        public void DrawVoxel(DrawContext display, Point pt, int heightCutDiff)
        {
            if (showFence)
            {
                // draw behind fence 
                DrawBehindFence(display, pt);
                Draw(display, pt, heightCutDiff);
                DrawFrontFence(display, pt);
                //draw front fence 
            }
            else
                Draw(display, pt, heightCutDiff);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Color GetColorOfTile();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="f"></param>
        public abstract void SetFence(Direction d, IFence f);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public abstract IFence GetFence(Direction d);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        protected abstract void DrawFrontFence(DrawContext display, Point pt);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        protected abstract void DrawBehindFence(DrawContext display, Point pt);

        /// <summary>
        /// Processes a mouse click.
        /// </summary>
        /// <returns>true if a mouse click event is "consumed"</returns>
        public virtual bool OnClick() { return false; }

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
        public virtual object QueryInterface(Type aspect) { return null; }

        /// <summary>
        /// Calls immediately after the voxel is removed from the world.
        /// </summary>
        public virtual void OnRemoved() { }
        // TODO: is this method necessary


        /// <summary>
        /// Short-cut to call the getLandPrice method of the World class.
        /// </summary>
        public int LandPrice
        {
            get
            {
                return (int)WorldDefinition.World.LandValue[Location];
            }
        }

        /// <summary>
        /// Obtains a reference to the entity that includes this voxel.
        /// </summary>
        public abstract IEntity Entity { get; }

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
        protected IFence[] fence = new IFence[4];
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
        /// <param name="loc"></param>
        protected AbstractVoxelImpl(Location loc)
        {
            this.loc = loc;
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
        public override Location Location { get { return loc; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        protected override void DrawFrontFence(DrawContext display, Point pt)
        {
            IFence f;
            f = fence[(Direction.SOUTH).index / 2];
            if (f != null)
                f.DrawFence(display.Surface, pt, Direction.SOUTH);
            f = fence[(Direction.WEST).index / 2];
            if (f != null)
                f.DrawFence(display.Surface, pt, Direction.WEST);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        protected override void DrawBehindFence(DrawContext display, Point pt)
        {
            IFence f;
            f = fence[(Direction.NORTH).index / 2];
            if (f != null)
                f.DrawFence(display.Surface, pt, Direction.NORTH);
            f = fence[(Direction.EAST).index / 2];
            if (f != null)
                f.DrawFence(display.Surface, pt, Direction.EAST);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="f"></param>
        public override void SetFence(Direction d, IFence f)
        {
            fence[d.index / 2] = f;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public override IFence GetFence(Direction d)
        {
            return fence[d.index / 2];

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Color GetColorOfTile()
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
    public interface IHoleVoxel
    {
        /// <summary>
        /// Returns false to prevent the ground surface to be drawn.
        /// </summary>
        /// <param name="above">
        /// True if the callee is located directly above the ground,
        /// false if directly below the ground.
        /// </param>
        bool DrawGround(bool above);
    }

    /// <summary>
    /// The interface called when the fence should be drawn.
    /// </summary>
    public interface IFence
    {
        /// <summary>
        /// called when the fehce should be drawn.
        /// </summary>
        /// <param name="d">one of the 4 directions (N,E,W,S)</param>
        /// <param name="pt"></param>
        /// <param name="surface"></param>
        void DrawFence(Surface surface, Point pt, Direction d);

        /// <summary>
        /// 
        /// </summary>
        string FenceId { get; }
    }
}
