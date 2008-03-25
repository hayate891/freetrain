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
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FreeTrain.Framework;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.World;

namespace FreeTrain.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AbstractControllerImpl : AbstractControllerForm, ModalController
    {
        /// <summary>
        /// 
        /// </summary>
        public AbstractControllerImpl()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            // Attach the control when activated.
            try
            {
                MainWindow.mainWindow.attachController(this);
            }
            catch (NullReferenceException nre)
            {
                Debug.WriteLine(nre);
            }
        }

        /// <summary>
        /// Derived class still needs to extend this method and maintain
        /// the singleton.
        /// </summary>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Detach it when it is closed.
            if (MainWindow.mainWindow.currentController == this)
                MainWindow.mainWindow.detachController();
        }

        //
        // default implementation for ModalController
        //
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public void close()
        {
            base.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public string name { get { return Text; } }

        /// <summary>
        /// 
        /// </summary>
        public virtual LocationDisambiguator Disambiguator { get { return null; } }

        /// <summary>
        /// 
        /// </summary>
        public virtual MapOverlay Overlay { get { return this as MapOverlay; } }

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
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void OnClick(MapViewWindow source, Location loc, Point ab) { }
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void OnMouseMove(MapViewWindow view, Location loc, Point ab) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public virtual void OnRightClick(MapViewWindow source, Location loc, Point ab)
        {
            Close();	// cancel
        }
    }
}
