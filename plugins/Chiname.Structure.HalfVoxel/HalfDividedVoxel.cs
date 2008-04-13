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
    /// 
    /// </summary>
    [Serializable]
    public class HalfDividedVoxel : AbstractVoxelImpl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="loc"></param>
        public HalfDividedVoxel(HVStructure owner, Location loc)
            : base(loc)
        {
            this.owner = owner;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="wloc"></param>
        internal protected HalfDividedVoxel(HVStructure owner, WorldLocator wloc)
            : base(wloc)
        {
            this.owner = owner;
        }

        /// <summary>
        /// The structure object to which this voxel belongs.
        /// </summary>
        public readonly HVStructure owner;

        /// <summary>
        /// 
        /// </summary>
        public override IEntity Entity { get { return owner; } }

        /// <summary>
        /// onClick event is delegated to the parent.
        /// </summary>
        public override bool OnClick()
        {
            return owner.onClick();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="heightCutDiff"></param>
        public override void Draw(DrawContext display, Point pt, int heightCutDiff)
        {
            if (heightCutDiff >= 0)
                ResourceUtil.EmptyChip.DrawShape(display.Surface, pt, owner.heightCutColor);
            else
                ResourceUtil.EmptyChip.Draw(display.Surface, pt);
            // above line is needed when my(=477) patch is applied.

            if (owner.backside != null)
                if (heightCutDiff < 0 || owner.backside.Height < heightCutDiff)
                {
                    owner.backside.GetSprite().Draw(display.Surface, pt);
                    ISprite hls = owner.backside.GetHighlightSprite();
                    if (hls != null) hls.Draw(display.Surface, pt);
                }
            if (owner.foreside != null)
                if (heightCutDiff < 0 || owner.foreside.Height < heightCutDiff)
                {
                    owner.foreside.GetSprite().Draw(display.Surface, pt);
                    ISprite hls = owner.foreside.GetHighlightSprite();
                    if (hls != null) hls.Draw(display.Surface, pt);
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
