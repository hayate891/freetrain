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
using System.Windows.Forms;
using FreeTrain.Framework;
using FreeTrain.Framework.Plugin;
using FreeTrain.Views.Map;
using FreeTrain.World;
using FreeTrain.World.Structs;

namespace FreeTrain.Controllers.Structs
{
    /// <summary>
    /// CommercialStructPlacementController の概要の説明です。
    /// </summary>
    public class CommercialStructPlacementController : FixedSizeStructController
    {
        private CommercialStructPlacementController() : base(PluginManager.CommercialStructureGroup) { }

        /// <summary> LocationDisambiguator implementation </summary>
        public override bool IsSelectable(Location loc)
        {
            if (IsPlacing)
            {
                // structures can be placed only on the ground
                return GroundDisambiguator.theInstance.IsSelectable(loc);
            }
            else
            {
                return Commercial.get(loc) != null;
            }
        }

        /// <summary>
        /// Removes the structure from given location, if any.
        /// </summary>
        public override void Remove(MapViewWindow view, Location loc)
        {
            Commercial c = Commercial.get(loc);
            if (c != null)
            {
                if (c.isOwned)
                    c.remove();
                else
                    MessageBox.Show("Can not remove");
                //! MessageBox.Show("撤去できません");
            }
        }

    }
}
