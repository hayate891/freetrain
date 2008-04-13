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
using FreeTrain.Views;
using FreeTrain.Views.Map;

namespace FreeTrain.Controllers
{
    /// <summary>
    /// ModalController that selects the rectangular region
    /// and do something with it.
    /// </summary>
    public abstract class RectSelectorController : IModalController, ILocationDisambiguator
    {
        /// <summary>Constant</summary>
        private static readonly Location unplaced = World.Location.Unplaced;

        /// <summary>
        /// 
        /// </summary>
        protected static Location Unplaced
        {
            get { return RectSelectorController.unplaced; }
        } 

        /// <summary>
        /// 
        /// </summary>
        private Location anchor = unplaced;

        /// <summary>
        /// 
        /// </summary>
        protected Location Anchor
        {
            get { return anchor; }
            set { anchor = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private Location currentLoc = unplaced;

        /// <summary>
        /// 
        /// </summary>
        protected Location CurrentLocation
        {
            get { return currentLoc; }
            set { currentLoc = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly IControllerSite site;

        /// <summary>
        /// 
        /// </summary>
        protected IControllerSite Site
        {
            get { return site; }
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        public RectSelectorController(IControllerSite site)
        {
            this.site = site;
        }

        //
        // methods that can/should be overrided by derived classes
        //

        /// <summary>
        /// Called when the selection is completed.
        /// </summary>
        protected abstract void OnRectSelected(Location loc1, Location loc2);

        /// <summary>
        /// Called when the selection is changed.
        /// </summary>
        protected virtual void OnRectUpdated(Location loc1, Location loc2) { }

        /// <summary>
        /// Called when the user wants to cancel the modal controller.
        /// </summary>
        protected virtual void OnCanceled()
        {
            site.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get { return site.Name; } }

        // can be overrided by a derived class to return another object.
        /// <summary>
        /// 
        /// </summary>
        public virtual IMapOverlay Overlay
        {
            get
            {
                // return this object if it implements MapOverlay by itself.
                return this as IMapOverlay;
            }
        }

        /// <summary>
        /// North-west corner of the selected region.
        /// </summary>
        protected Location LocationNW
        {
            get
            {
                Debug.Assert(currentLoc != unplaced);
                return new Location(
                    Math.Min(currentLoc.x, anchor.x),
                    Math.Min(currentLoc.y, anchor.y),
                    anchor.z);
            }
        }

        /// <summary>
        /// South-east corner of the selected region.
        /// </summary>
        protected Location LocationSE
        {
            get
            {
                Debug.Assert(currentLoc != unplaced);
                return new Location(
                    Math.Max(currentLoc.x, anchor.x),
                    Math.Max(currentLoc.y, anchor.y),
                    anchor.z);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILocationDisambiguator Disambiguator { get { return this; } }

        /// <summary> 
        /// LocationDisambiguator implementation 
        /// </summary>
        public bool IsSelectable(Location loc)
        {
            if (anchor != unplaced)
                return loc.z == anchor.z;
            else
                // lands can be placed only on the ground
                return GroundDisambiguator.theInstance.IsSelectable(loc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void OnClick(MapViewWindow view, Location loc, Point ab)
        {
            if (anchor == unplaced)
            {
                anchor = loc;
            }
            else
            {
                OnRectSelected(this.LocationNW, this.LocationSE);
                anchor = unplaced;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void OnRightClick(MapViewWindow source, Location loc, Point ab)
        {
            if (anchor == unplaced)
                OnCanceled();
            else
            {
                // cancel the anchor
                WorldDefinition.World.OnAllVoxelUpdated();
                anchor = unplaced;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void OnMouseMove(MapViewWindow view, Location loc, Point ab)
        {
            WorldDefinition w = WorldDefinition.World;

            if (anchor != unplaced && currentLoc != loc)
            {
                // the current location is moved.
                // update the screen
                currentLoc = loc;
                OnRectUpdated(this.LocationNW, this.LocationSE);
                w.OnAllVoxelUpdated();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnAttached() { }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnDetached()
        {
            // redraw the entire surface to erase any left-over from this controller
            WorldDefinition.World.OnAllVoxelUpdated();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Close()
        {
            OnCanceled();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdatePreview()
        { }
    }
}
