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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace freetrain.views.map
{
    /// <summary>
    /// Window that controls the height cut mode of the given quarter view drawer.
    /// </summary>
    public class HeightCutWindow : System.Windows.Forms.Form
    {
        private readonly MapViewWindow mapView;
        private readonly QuarterViewDrawer drawer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapView"></param>
        /// <param name="drawer"></param>
        public HeightCutWindow(MapViewWindow mapView, QuarterViewDrawer drawer)
        {
            this.mapView = mapView;
            this.drawer = drawer;
            InitializeComponent();

            trackBar.Minimum = 0;
            trackBar.Maximum = world.World.world.size.z - 1;
            trackBar.Value = drawer.heightCutHeight;

            drawer.OnHeightCutChanged += new EventHandler(onHeightCutChange);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            drawer.heightCutHeight = trackBar.Value;
        }
        #region Windows Form Designer generated code
        private freetrain.controls.TrackBarEx trackBar;
        private System.ComponentModel.Container components = null;

        private void InitializeComponent()
        {
            this.trackBar = new freetrain.controls.TrackBarEx();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar
            // 
            this.trackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar.Name = "trackBar";
            this.trackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar.Size = new System.Drawing.Size(42, 96);
            this.trackBar.TabIndex = 0;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_Scroll);
            // 
            // HeightCutWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(42, 96);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.trackBar});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "HeightCutWindow";
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            mapView.heightCutWindow = null;
            drawer.OnHeightCutChanged -= new EventHandler(onHeightCutChange);
        }

        private void onHeightCutChange(object sender, EventArgs e)
        {
            trackBar.Value = drawer.heightCutHeight;
        }

    }
}
