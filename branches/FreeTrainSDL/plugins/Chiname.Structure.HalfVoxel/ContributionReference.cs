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
using System.Windows.Forms;
using System.Xml;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Population;
using FreeTrain.Framework;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework.Plugin.Graphics;
using FreeTrain.Framework.Graphics;
using FreeTrain.World;
using FreeTrain.World.Structs;
using FreeTrain.Controllers;
using FreeTrain.Contributions.Structs;

namespace FreeTrain.World.Structs.HalfVoxelStructure
{
    #region ContributionReference
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ContributionReference
    {
        private readonly HalfVoxelContribution contrib;
        private readonly int patternIdx;
        private readonly int colorIdx;
        private readonly int hilightIdx;
        /// <summary>
        /// 
        /// </summary>
        private readonly PlaceSide placeSide;

        /// <summary>
        /// 
        /// </summary>
        public PlaceSide PlaceSide
        {
            get { return placeSide; }
        } 

        /// <summary>
        /// 
        /// </summary>
        readonly Direction frontface;

        /// <summary>
        /// 
        /// </summary>
        public Direction Frontface
        {
            get { return frontface; }
        } 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hvc"></param>
        /// <param name="color"></param>
        /// <param name="hilight"></param>
        /// <param name="front"></param>
        /// <param name="side"></param>
        public ContributionReference(HalfVoxelContribution hvc, int color, int hilight, Direction front, PlaceSide side)
        {
            this.contrib = hvc;
            this.colorIdx = color;
            this.hilightIdx = hilight;
            this.placeSide = side;
            this.frontface = front;
            this.patternIdx = SpriteSet.GetIndexOf(front, side);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ISprite GetSprite()
        {
            return contrib.sprites[colorIdx][patternIdx];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ISprite GetHighlightSprite()
        {
            if (contrib.hilights != null)
                return contrib.hilights[hilightIdx][patternIdx];
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual int Height
        {
            get { return contrib.Height; }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual int Price
        {
            get { return contrib.Price; }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual BasePopulation Population
        {
            get { return contrib.Population; }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Name
        {
            get { return contrib.Subgroup; }
        }
    }

    internal class EmptyReference : ContributionReference
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hvc"></param>
        /// <param name="color"></param>
        /// <param name="hilight"></param>
        /// <param name="front"></param>
        /// <param name="side"></param>
        public EmptyReference(HalfVoxelContribution hvc, int color, int hilight, Direction front, PlaceSide side)
            : base(null, -1, -1, front, side) { }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ISprite GetSprite() { return null; }

        /// <summary>
        /// 
        /// </summary>
        public override int Height { get { return 0; } }
        /// <summary>
        /// 
        /// </summary>
        public override int Price { get { return 0; } }
        /// <summary>
        /// 
        /// </summary>
        public override BasePopulation Population { get { return null; } }
    }
    #endregion
}
