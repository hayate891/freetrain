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
using FreeTrain.Controllers;
using FreeTrain.Framework;
using FreeTrain.Views.Map;
using FreeTrain.World.Rail;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.World.Development
{
	/// <summary>
	/// Controller that checks the land value.
	/// </summary>
	internal class LandValueInspector : AbstractControllerImpl/*, MapOverlay*/
	{
		public LandValueInspector() {
			InitializeComponent();

			Show();
			Activate();
		}

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}


		public override void OnMouseMove( MapViewWindow view, Location loc, Point ab ) {
			//MainWindow.mainWindow.statusText = "Land value:" + World.world.landValue[loc];
			//! MainWindow.mainWindow.statusText = "地価：" + World.world.landValue[loc];
		}

		//
		// Disambiguator implementation
		//
		public override LocationDisambiguator Disambiguator { get { return GroundDisambiguator.theInstance; } }



//		//
//		// MapOverlay implementation
//		//
//		public void drawVoxel( MapViewWindow view, DrawContext context, Location loc, Point pt ) {
//		}
//
//		public void drawBefore( MapViewWindow view, DrawContext context ) {}
//		public void drawAfter( MapViewWindow view, DrawContext context ) {}

		#region Designer generated code
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;

		/// <summary>
		/// Designer サポートに必要なメソッドです。コード エディタで
		/// このメソッドのコンテンツを変更しないでください。
		/// </summary>
		private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 69);
            this.label1.TabIndex = 0;
            this.label1.Text = "Move the cursor to display land value";
            // 
            // LandValueInspector
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(120, 75);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LandValueInspector";
            this.Text = "Land Value";
            this.ResumeLayout(false);

		}
		#endregion
	
	}
}
