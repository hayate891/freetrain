using System;
using System.Diagnostics;
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

using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Structs;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Util;
using FreeTrain.World;
using FreeTrain.World.Subsidiaries;

namespace FreeTrain.World.Structs.HalfVoxelStructure
{
    /// <summary>
    /// The half divided voxel structure.
    /// Consist of Back part and Fore part.
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public class HVStructure : Structure, ISubsidiaryEntity
    {
        enum Orientation : int { XAxis, YAxis };
        /// <summary>
        /// The sprite to draw.
        /// </summary>
        public HVStructure(ContributionReference type, Location loc)
        {
            this.baseLocation = loc;
            if (type.placeSide == PlaceSide.Back)
                this.back = type;
            else
                this.fore = type;

            // build voxels
            new HalfDividedVoxel(this, loc);
            subsidiary = new SubsidiaryCompany(this, false);
            if (type.population != null)
                stationListener = new StationListenerImpl(type.population, loc);
        }

        #region add or remove half voxel.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool add(ContributionReference type)
        {
            if (type.placeSide == PlaceSide.Back)
            {
                if (back != null) return false; // already occupied!

                if (type.frontface.isParallelToX)
                {
                    if (fore.frontface.isParallelToX)
                    {
                        back = type;
                        return true;
                    }
                }
                else // parallel to Y
                {
                    if (fore.frontface.isParallelToY)
                    {
                        back = type;
                        return true;
                    }
                }
            }
            else // PlaceSide.Fore
            {
                if (fore != null) return false; // already occupied!

                if (type.frontface.isParallelToX)
                {
                    if (back.frontface.isParallelToX)
                    {
                        fore = type;
                        return true;
                    }
                }
                else // parallel to Y
                {
                    if (back.frontface.isParallelToY)
                    {
                        fore = type;
                        return true;
                    }
                }
            }
            WorldDefinition.World.OnVoxelUpdated(baseLocation);
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public bool remove(PlaceSide side)
        {
            if (side == PlaceSide.Back)
            {
                if (back == null) return false;
                back = null;
                if (fore == null)
                    remove();
            }
            else // PlaceSide.Fore
            {
                if (fore == null) return false;
                fore = null;
                if (back == null)
                    remove();
            }
            WorldDefinition.World.OnVoxelUpdated(baseLocation);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected ContributionReference fore;
        /// <summary>
        /// 
        /// </summary>
        protected ContributionReference back;

        internal ContributionReference foreside { get { return fore; } }
        internal ContributionReference backside { get { return back; } }
        #endregion

        #region population related methods
        /// <summary>
        /// Station to which this structure sends population to.
        /// </summary>
        private readonly StationListenerImpl stationListener;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public override object queryInterface(Type aspect)
        {
            // if type.population is null, we don't have any population
            if (aspect == typeof(Rail.IStationListener))
                return stationListener;
            else
                return base.queryInterface(aspect);
        }
        #endregion

        /// <summary>
        /// north-west bottom corner of this structure.
        /// </summary>
        [CLSCompliant(false)]
        public readonly Location baseLocation;

        /// <summary>
        /// Obtains the color that will be used to draw when in the height-cut mode.
        /// </summary>
        internal protected Color heightCutColor { get { return hcColor; } }
        private static Color hcColor = Color.FromArgb(146, 94, 42);
        /// <summary>
        /// Gets the distance to this location from the base location of this structure.
        /// </summary>
        [CLSCompliant(false)]
        protected int distanceTo(Location loc)
        {
            return baseLocation.distanceTo(loc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool onClick()
        {
            return false;	// no associated action
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
            // just remove the voxels
            WorldDefinition world = WorldDefinition.World;
            world.remove(baseLocation);

            if (onEntityRemoved != null)
                onEntityRemoved(this, null);

            if (stationListener != null)
                stationListener.onRemoved();
        }

        //		public static new bool canBeBuilt( Location loc, Distance size ) 
        //		{
        //			if(!Structure.canBeBuilt(loc,size))
        //				return false;
        //
        //			// make sure all the voxels are on the ground.
        //			for( int y=0; y<size.y; y++ )
        //				for( int x=0; x<size.x; x++ )
        //					if( World.world.getGroundLevel(loc.x+x,loc.y+y)!=loc.z )
        //						return false;
        //			return true;
        //		}

        #region Entity implementation
        /// <summary>
        /// 
        /// </summary>
        public override bool isSilentlyReclaimable { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public override bool isOwned { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public override int entityValue { get { return (int)subsidiary.currentMarketPrice; } }
        #endregion

        #region SubsideryEntity implementation
        /// <summary>
        /// 
        /// </summary>
        protected readonly SubsidiaryCompany subsidiary;
        /// <summary>
        /// 
        /// </summary>
        public override string name
        {
            get
            {
                if (fore == null) return back.name;
                if (back == null) return fore.name;
                if (fore.name.Equals(back.name))
                    return fore.name;
                return fore.name + "/" + back.name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long structurePrice
        {
            get
            {
                long p = 0;
                if (fore != null) p += fore.price;
                if (back != null) p += back.price;
                return p;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public long totalLandPrice
        { get { return WorldDefinition.World.landValue[baseLocation]; } }

        /// <summary>
        /// 
        /// </summary>
        public Location locationClue
        { get { return baseLocation; } }
        #endregion

    }



    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public class HalfDividedVoxel : AbstractVoxelImpl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_owner"></param>
        /// <param name="_loc"></param>
        [CLSCompliant(false)]
        public HalfDividedVoxel(HVStructure _owner, Location _loc)
            : base(_loc)
        {
            this.owner = _owner;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_owner"></param>
        /// <param name="wloc"></param>
        [CLSCompliant(false)]
        internal protected HalfDividedVoxel(HVStructure _owner, WorldLocator wloc)
            : base(wloc)
        {
            this.owner = _owner;
        }

        /// <summary>
        /// The structure object to which this voxel belongs.
        /// </summary>
        public readonly HVStructure owner;

        /// <summary>
        /// 
        /// </summary>
        public override IEntity entity { get { return owner; } }

        /// <summary>
        /// onClick event is delegated to the parent.
        /// </summary>
        public override bool onClick()
        {
            return owner.onClick();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="heightCutDiff"></param>
        [CLSCompliant(false)]
        public override void draw(DrawContext display, Point pt, int heightCutDiff)
        {
            if (heightCutDiff >= 0)
                ResourceUtil.emptyChip.drawShape(display.Surface, pt, owner.heightCutColor);
            else
                ResourceUtil.emptyChip.draw(display.Surface, pt);
            // above line is needed when my(=477) patch is applied.

            if (owner.backside != null)
                if (heightCutDiff < 0 || owner.backside.height < heightCutDiff)
                {
                    owner.backside.getSprite().draw(display.Surface, pt);
                    ISprite hls = owner.backside.getHighlightSprite();
                    if (hls != null) hls.draw(display.Surface, pt);
                }
            if (owner.foreside != null)
                if (heightCutDiff < 0 || owner.foreside.height < heightCutDiff)
                {
                    owner.foreside.getSprite().draw(display.Surface, pt);
                    ISprite hls = owner.foreside.getHighlightSprite();
                    if (hls != null) hls.draw(display.Surface, pt);
                }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool hasSpace
        {
            get { return (owner.backside == null || owner.foreside == null); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ContributionReference[] getReferences()
        {
            ContributionReference[] arr;
            if (hasSpace)
            {
                arr = new ContributionReference[1];
                if (owner.backside == null)
                    arr[0] = owner.foreside;
                else
                    arr[0] = owner.backside;
            }
            else
            {
                arr = new ContributionReference[2];
                arr[0] = owner.backside;
                arr[1] = owner.foreside;
            }
            return arr;
        }
    }
}
