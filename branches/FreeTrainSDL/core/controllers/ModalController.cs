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
    /// User Interface by using MapViewWindow.
    /// 
    /// When active, a ModalController can receive mouse events
    /// on map windows, can modify the image of the map view,
    /// and can affect how mouse clicks are interpreted.
    /// </summary>
    public interface IModalController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">sender of the event.</param>
        /// <param name="loc">(X,Y,Z) location that was clicked</param>
        /// <param name="ab">(A,B) location that was clicked.</param>
        void OnClick(MapViewWindow source, Location loc, Point ab);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        void OnRightClick(MapViewWindow source, Location loc, Point ab);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        void OnMouseMove(MapViewWindow source, Location loc, Point ab);

        /// <summary>
        /// Called when the controller gets activated.
        /// </summary>
        void OnAttached();

        /// <summary>
        /// Called when the controller gets deactivated.
        /// </summary>
        void OnDetached();

        //		/// <summary>
        //		/// Closes the controller. A host uses this method to close
        //		/// a controller.
        //		/// </summary>
        //		void close();

        /// <summary>
        /// Gets the display name of this controller.
        /// </summary>
        string name { get; }

        /// <summary>
        /// Gets the disambiguator associated with this controller, if any.
        /// </summary>
        LocationDisambiguator Disambiguator { get; }

        /// <summary>
        /// If this controller needs to modify the map view, return non-null value.
        /// </summary>
        IMapOverlay Overlay { get; }

        /// <summary>
        /// 
        /// </summary>
        void UpdatePreview();
    }
}
