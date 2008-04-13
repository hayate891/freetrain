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
using FreeTrain.Contributions.Common;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Util;

namespace FreeTrain.World.Structs
{
    /// <summary>
    /// Pseudo three-dimensional structure.
    /// 
    /// Sprites are only defined for the ground-level voxels,
    /// and other higher voxels will be occupied by invisible ones.
    /// </summary>
    [Serializable]
    public abstract class PThreeDimStructure : Structure
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="wloc"></param>
        public PThreeDimStructure(FixedSizeStructureContribution type, WorldLocator wloc)
        {
            this.baseLocation = wloc.location;
            this.type = type;

            // build voxels
            for (int z = 0; z < type.Size.z; z++)
                for (int y = 0; y < type.Size.y; y++)
                    for (int x = 0; x < type.Size.x; x++)
                        CreateVoxel(new WorldLocator(wloc.world, baseLocation + new Distance(x, y, z)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        protected virtual StructureVoxel CreateVoxel(WorldLocator loc)
        {
            return new VoxelImpl(this, loc);
        }
        /// <summary>
        /// 
        /// </summary>
        public readonly FixedSizeStructureContribution type;

        /// <summary>
        /// north-west bottom corner of this structure.
        /// </summary>
        public readonly Location baseLocation;
        /// <summary>
        /// 
        /// </summary>
        public override int EntityValue { get { return type.Price; } }
        /// <summary>
        /// 
        /// </summary>
        public Cube cube { get { return Cube.CreateExclusive(baseLocation, type.Size); } }


        /// <summary>
        /// Obtains the color that will be used to draw when in the height-cut mode.
        /// </summary>
        internal protected abstract Color heightCutColor { get; }

        /// <summary>
        /// Gets the distance to this location from the base location of this structure.
        /// </summary>
        protected int distanceTo(Location loc)
        {
            return baseLocation.distanceTo(loc);
        }

        /// <summary>
        /// 
        /// </summary>
        public override string name { get { return type.Name; } }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        protected class VoxelImpl : StructureVoxel, IDeserializationCallback
        {
            internal VoxelImpl(PThreeDimStructure _owner, WorldLocator wloc)
                : base(_owner, wloc)
            {
                setSprite();
            }
            //public override bool transparent { get { return true; } }

            private new PThreeDimStructure owner
            {
                get
                {
                    return (PThreeDimStructure)base.owner;
                }
            }

            /// <summary>
            /// The sprite to draw, or null if the voxel
            /// is invisible.
            /// </summary>
            [NonSerialized]	// programatically recreatable.
            private ISprite sprite;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            public void OnDeserialization(object sender)
            {
                setSprite();
            }

            private void setSprite()
            {
                PThreeDimStructure o = owner;
                sprite = o.type.GetSprite(Location - o.baseLocation);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="display"></param>
            /// <param name="pt"></param>
            /// <param name="heightCutDiff"></param>
            public override void Draw(DrawContext display, Point pt, int heightCutDiff)
            {
                PThreeDimStructure o = owner;

                int zdiff = o.type.Size.z - (this.Location.z - o.baseLocation.z);

                if (heightCutDiff < 0 || zdiff < heightCutDiff)
                {
                    // draw in a normal mode
                    sprite.Draw(display.Surface, pt);
                }
                else
                {
                    // drawing in the height cut mode
                    if (this.Location.z == o.baseLocation.z)
                        ResourceUtil.EmptyChip.DrawShape(display.Surface, pt, o.heightCutColor);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override event EventHandler onEntityRemoved;
        /// <summary>
        /// 
        /// </summary>
        public override void remove()
        {
            // just remove all the voxels
            WorldDefinition world = WorldDefinition.World;
            foreach (Voxel v in this.cube.Voxels)
                world.remove(v);

            if (onEntityRemoved != null)
                onEntityRemoved(this, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="size"></param>
        /// <param name="cm"></param>
        /// <returns></returns>
        public static new bool canBeBuilt(Location loc, Distance size, ControlMode cm)
        {
            if (!Structure.canBeBuilt(loc, size, cm))
                return false;

            // make sure all the voxels are on the ground.
            for (int y = 0; y < size.y; y++)
                for (int x = 0; x < size.x; x++)
                    if (WorldDefinition.World.GetGroundLevel(loc.x + x, loc.y + y) != loc.z)
                        return false;
            return true;
        }
    }
}
