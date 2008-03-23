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
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.World;

namespace FreeTrain.Controllers
{
    /// <summary>
    /// Partial <c>ModalController</c> implementation that selects
    /// a particular location.
    /// </summary>
    public abstract class PointSelectorController : ModalController, MapOverlay
    {
        /// <summary>
        /// 
        /// </summary>
        protected Location currentPos = Location.UNPLACED;
        /// <summary>
        /// 
        /// </summary>
        protected readonly IControllerSite site;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_site"></param>
        public PointSelectorController(IControllerSite _site)
        {
            this.site = _site;
        }

        /// <summary>
        /// Called when a selected location is changed.
        /// Usually an application doesn't need to do anything.
        /// </summary>
        /// <param name="loc"></param>
        protected virtual void onSelectionChanged(Location loc)
        {
        }

        /// <summary>
        /// Called when the player selects a location.
        /// </summary>
        protected abstract void onLocationSelected(Location loc);




        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        public virtual void drawAfter(QuarterViewDrawer view, DrawContextEx canvas)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        public virtual void drawBefore(QuarterViewDrawer view, DrawContextEx canvas)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        /// <param name="loc"></param>
        /// <param name="pt"></param>
        public virtual void drawVoxel(QuarterViewDrawer view, DrawContextEx canvas, Location loc, Point pt)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void onAttached()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void onDetached()
        {
            // clear the remaining image
            if (currentPos != Location.UNPLACED)
                WorldDefinition.world.onVoxelUpdated(currentPos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void onMouseMove(MapViewWindow source, Location loc, Point ab)
        {
            if (currentPos != Location.UNPLACED)
                WorldDefinition.world.onVoxelUpdated(currentPos);
            currentPos = loc;
            WorldDefinition.world.onVoxelUpdated(currentPos);

            onSelectionChanged(currentPos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public void onRightClick(MapViewWindow source, Location loc, Point ab)
        {
            close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public void onClick(MapViewWindow source, Location loc, Point ab)
        {
            onLocationSelected(loc);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void close()
        {
            site.close();
        }
        /// <summary>
        /// 
        /// </summary>
        public abstract LocationDisambiguator Disambiguator { get; }
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

    }
}
