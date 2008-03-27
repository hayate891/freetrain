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
    /// ModalController that selects a cube of the fixed size.
    /// </summary>
    public abstract class CubeSelectorController : ModalController
    {
        /// <summary>Constant</summary>
        protected static readonly Location UNPLACED = World.Location.UNPLACED;
        /// <summary>
        /// 
        /// </summary>
        protected Location location = UNPLACED;
        /// <summary>
        /// 
        /// </summary>
        protected readonly Distance size;
        /// <summary>
        /// 
        /// </summary>
        protected readonly IControllerSite site;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_size"></param>
        /// <param name="_site"></param>
        public CubeSelectorController(Distance _size, IControllerSite _site)
        {
            this.size = _size;
            this.site = _site;
        }

        //
        // methods that can/should be overrided by derived classes
        //

        /// <summary>
        /// Called when the selection is completed.
        /// </summary>
        protected abstract void onSelected(Cube cube);

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
        public virtual MapOverlay Overlay
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
        protected Cube currentCube
        {
            get
            {
                Debug.Assert(location != UNPLACED);
                return Cube.createExclusive(location, size);
            }
        }


        //
        // internal logic
        //

        /// <summary>
        /// 
        /// </summary>
        public virtual LocationDisambiguator Disambiguator { get { return GroundDisambiguator.theInstance; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void OnClick(MapViewWindow view, Location loc, Point ab)
        {
            onSelected(Cube.createExclusive(loc, size));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void OnRightClick(MapViewWindow source, Location loc, Point ab)
        {
            onCanceled();
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

            if (location != loc)
            {
                // the current location is moved.
                // update the screen
                w.onVoxelUpdated(currentCube);
                location = loc;
                w.onVoxelUpdated(currentCube);
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
            WorldDefinition.World.onAllVoxelUpdated();
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void close()
        {
            onCanceled();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdatePreview()
        { }
    }
}
