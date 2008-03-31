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
using FreeTrain.Controllers.Structs;
using FreeTrain.Framework;
using FreeTrain.Framework.Plugin;
using FreeTrain.Views.Map;
using FreeTrain.World;
using FreeTrain.World.Rail;

namespace FreeTrain.Controllers.Rail
{
    /// <summary>
    /// StationaryStructPlacementController の概要の説明です。
    /// </summary>
    public class StationaryStructPlacementController : FixedSizeStructController
    {
        #region Singleton instance management
        /// <summary>
        /// Creates a new controller window, or active the existing one.
        /// </summary>
        public static void create()
        {
            if (theInstance == null)
                theInstance = new StationaryStructPlacementController();
            theInstance.Show();
            theInstance.Activate();
        }

        private static StationaryStructPlacementController theInstance;
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

        private StationaryStructPlacementController() : base(Core.plugins.railStationaryGroup) { }

        /// <summary> LocationDisambiguator implementation </summary>
        public override bool IsSelectable(Location loc)
        {
            if (isPlacing)
            {
                // structures can be placed only on the ground
                return GroundDisambiguator.theInstance.IsSelectable(loc);
            }
            else
            {
                return RailStationaryStructure.get(loc) != null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        public override void remove(MapViewWindow view, Location loc)
        {
            RailStationaryStructure s = RailStationaryStructure.get(loc);
            if (s != null)
            {
                s.remove();
            }
        }
    }
}
