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

namespace FreeTrain.Util.controls
{
	/// <summary>
	/// Implements a context menu for toolbar and allows the player
	/// to move toolbars around.
	/// </summary>
	public class ToolBarCustomizerUI
	{
		/// <summary>
		/// Attachs to the given toolbar.
		/// </summary>
		public static void attach( ToolBar bar ) {
			new ToolBarCustomizerUI(bar);
		}


		private readonly ToolBar toolBar;
		private readonly ContextMenu menu;
		private readonly MenuItem[] menuItems = new MenuItem[4];

		private ToolBarCustomizerUI( ToolBar tb ) {
			this.toolBar = tb;

			toolBar.MouseUp += new MouseEventHandler(onMouseUp);

			menu = createMenu();
		}
		
		private ContextMenu createMenu() {
			menuItems[0] = new MenuItem( "Dock left", new EventHandler(dockLeft) );
			menuItems[1] = new MenuItem( "Dock right", new EventHandler(dockRight) );
			menuItems[2] = new MenuItem( "Dock up", new EventHandler(dockTop) );
			menuItems[3] = new MenuItem( "Dock down", new EventHandler(dockBottom) );
			//! menuItems[0] = new MenuItem( "左にドッキング", new EventHandler(dockLeft) );
			//! menuItems[1] = new MenuItem( "右にドッキング", new EventHandler(dockRight) );
			//! menuItems[2] = new MenuItem( "上にドッキング", new EventHandler(dockTop) );
			//! menuItems[3] = new MenuItem( "下にドッキング", new EventHandler(dockBottom) );
			
			foreach( MenuItem mi in menuItems )
				mi.RadioCheck = true;

			return new ContextMenu(menuItems);
		}

		private void onMouseUp( object sender, MouseEventArgs a ) {
			if( a.Button != MouseButtons.Right )
				return;

			// update the context menu
			menuItems[0].Checked = (toolBar.Dock==DockStyle.Left);
			menuItems[1].Checked = (toolBar.Dock==DockStyle.Right);
			menuItems[2].Checked = (toolBar.Dock==DockStyle.Top);
			menuItems[3].Checked = (toolBar.Dock==DockStyle.Bottom);

			// show context menu
			menu.Show(toolBar, new Point(a.X,a.Y) );
		}

		private void dockLeft  ( object sender, EventArgs a ) { dock(DockStyle.Left); }
		private void dockRight ( object sender, EventArgs a ) { dock(DockStyle.Right); }
		private void dockTop   ( object sender, EventArgs a ) { dock(DockStyle.Top); }
		private void dockBottom( object sender, EventArgs a ) { dock(DockStyle.Bottom); }

		private void dock( DockStyle pos) {
			toolBar.Dock = pos;
		}

	}
}
