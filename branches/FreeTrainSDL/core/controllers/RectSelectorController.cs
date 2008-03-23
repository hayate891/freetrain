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
using FreeTrain.World;
using FreeTrain.Views.Map;

namespace FreeTrain.Controllers
{
    /// <summary>
    /// ModalController that selects the rectangular region
    /// and do something with it.
    /// </summary>
    public abstract class RectSelectorController : ModalController, LocationDisambiguator
    {
        /// <summary>Constant</summary>
        protected static readonly Location UNPLACED = World.Location.UNPLACED;
        /// <summary>
        /// 
        /// </summary>
        protected Location anchor = UNPLACED;
        /// <summary>
        /// 
        /// </summary>
        protected Location currentLoc = UNPLACED;
        /// <summary>
        /// 
        /// </summary>
        protected readonly IControllerSite site;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_site"></param>
        public RectSelectorController(IControllerSite _site)
        {
            this.site = _site;
        }

        //
        // methods that can/should be overrided by derived classes
        //

        /// <summary>
        /// Called when the selection is completed.
        /// </summary>
        protected abstract void onRectSelected(Location loc1, Location loc2);

        /// <summary>
        /// Called when the selection is changed.
        /// </summary>
        protected virtual void onRectUpdated(Location loc1, Location loc2) { }

        /// <summary>
        /// Called when the user wants to cancel the modal controller.
        /// </summary>
        protected virtual void onCanceled()
        {
            site.close();
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual string name { get { return site.name; } }

        // can be overrided by a derived class to return another object.
        /// <summary>
        /// 
        /// </summary>
        public virtual MapOverlay overlay
        {
            get
            {
                // return this object if it implements MapOverlay by itself.
                return this as MapOverlay;
            }
        }


        //
        // convenience methods
        /// <summary>
        /// North-west corner of the selected region.
        /// </summary>
        protected Location location1
        {
            get
            {
                Debug.Assert(currentLoc != UNPLACED);
                return new Location(
                    Math.Min(currentLoc.x, anchor.x),
                    Math.Min(currentLoc.y, anchor.y),
                    anchor.z);
            }
        }

        /// <summary>
        /// South-east corner of the selected region.
        /// </summary>
        protected Location location2
        {
            get
            {
                Debug.Assert(currentLoc != UNPLACED);
                return new Location(
                    Math.Max(currentLoc.x, anchor.x),
                    Math.Max(currentLoc.y, anchor.y),
                    anchor.z);
            }
        }


        //
        // internal logic
        //

        /// <summary>
        /// 
        /// </summary>
        public LocationDisambiguator Disambiguator { get { return this; } }

        /// <summary> LocationDisambiguator implementation </summary>
        public bool isSelectable(Location loc)
        {
            if (anchor != UNPLACED)
                return loc.z == anchor.z;
            else
                // lands can be placed only on the ground
                return GroundDisambiguator.theInstance.isSelectable(loc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void onClick(MapViewWindow view, Location loc, Point ab)
        {
            if (anchor == UNPLACED)
            {
                anchor = loc;
            }
            else
            {
                onRectSelected(this.location1, this.location2);
                anchor = UNPLACED;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void onRightClick(MapViewWindow source, Location loc, Point ab)
        {
            if (anchor == UNPLACED)
                onCanceled();
            else
            {
                // cancel the anchor
                WorldDefinition.world.onAllVoxelUpdated();
                anchor = UNPLACED;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void onMouseMove(MapViewWindow view, Location loc, Point ab)
        {
            WorldDefinition w = WorldDefinition.world;

            if (anchor != UNPLACED && currentLoc != loc)
            {
                // the current location is moved.
                // update the screen
                currentLoc = loc;
                onRectUpdated(this.location1, this.location2);
                w.onAllVoxelUpdated();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void onAttached() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void onDetached()
        {
            // redraw the entire surface to erase any left-over from this controller
            WorldDefinition.world.onAllVoxelUpdated();
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void close()
        {
            onCanceled();
        }
    }
}
