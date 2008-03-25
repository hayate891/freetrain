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
using FreeTrain.Framework.Graphics;

namespace FreeTrain.Views
{
	/// <summary>
	/// Draws overlay images to a QuarterViewDrawer.
	/// </summary>
	public interface WeatherOverlay : IDisposable
	{
		/// <summary>
		/// Called when the size of the QuarterViewDrawer is changed.
		/// </summary>
		void setSize( Size sz );

		/// <summary>
		/// Draws the contents of the given drawer with the overlay
		/// to the target image.
		/// </summary>
		void draw( QuarterViewDrawer drawer, Surface target, Point pt );

		/// <summary>
		/// TBD: Periodical timer notification.
		/// </summary>
		/// <returns>
		/// true if the screen needs to be updated.
		/// </returns>
		bool onTimerFired();
	}
}
