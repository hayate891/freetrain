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
using System.Windows.Forms;
using FreeTrain.Contributions.Structs;
using FreeTrain.Framework;
using FreeTrain.Framework.plugin;
using FreeTrain.Util;
using FreeTrain.world;
using FreeTrain.world.Subsidiaries;

namespace FreeTrain.world.Structs
{
    // TODO: value should be unified to use long, not int.

    /// <summary>
    /// Commercial structure.
    /// </summary>
    [Serializable]
    public class Commercial : PopulatedStructure, SubsidiaryEntity
    {
        private readonly new CommercialStructureContribution type;

        private readonly SubsidiaryCompany subsidiary;

        /// <summary>
        /// Creates a new commercial structurewith its left-top corner at
        /// the specified location.
        /// </summary>
        /// <param name="_type">
        /// Type of the structure to be built.
        /// </param>
        /// <param name="initiallyOwned"></param>
        /// <param name="wloc"></param>
        public Commercial(CommercialStructureContribution _type, WorldLocator wloc, bool initiallyOwned)
            : base(_type, wloc)
        {

            this.type = _type;
            if (wloc.world == World.world)
            {
                this.subsidiary = new SubsidiaryCompany(this, initiallyOwned);
            }
        }

        #region Entity implementation
        /// <summary>
        /// 
        /// </summary>
        public override bool isSilentlyReclaimable { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public override bool isOwned { get { return subsidiary.isOwned; } }
        /// <summary>
        /// 
        /// </summary>
        public override int entityValue { get { return (int)subsidiary.currentMarketPrice; } }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public long structurePrice
        {
            get
            {
                return type.price;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public long totalLandPrice
        {
            get
            {
                return World.world.landValue[baseLocation + type.size / 2] * type.size.x * type.size.y;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Location locationClue
        {
            get
            {
                return base.baseLocation + type.size / 2;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="sz"></param>
        /// <param name="cm"></param>
        /// <returns></returns>
        public new static bool canBeBuilt(Location loc, Distance sz, ControlMode cm)
        {
            return Structure.canBeBuilt(loc, sz, cm) && Structure.isOnTheGround(loc, sz);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool onClick()
        {
            return false;	// no associated action
            // TODO: do something if it's owned
        }
        /// <summary>
        /// 
        /// </summary>
        public override string name { get { return type.name; } }
        /// <summary>
        /// 
        /// </summary>
        internal protected override Color heightCutColor { get { return hcColor; } }
        private static Color hcColor = Color.FromArgb(179, 115, 51);

        /// <summary>
        /// Gets the station object if one is in the specified location.
        /// </summary>
        public static Commercial get(Location loc)
        {
            return World.world.getEntityAt(loc) as Commercial;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Commercial get(int x, int y, int z) { return get(new Location(x, y, z)); }
    }
}
