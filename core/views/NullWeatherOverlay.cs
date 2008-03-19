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
using SDL.net;

namespace FreeTrain.Views
{
    /// <summary>
    /// NullWeatherOverlay の概要の説明です。
    /// </summary>
    public sealed class NullWeatherOverlay : WeatherOverlay
    {
        private NullWeatherOverlay() { }
        /// <summary>
        /// 
        /// </summary>
        public static readonly WeatherOverlay theInstance = new NullWeatherOverlay();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sz"></param>
        public void setSize(Size sz) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawer"></param>
        /// <param name="target"></param>
        /// <param name="pt"></param>
        public void draw(QuarterViewDrawer drawer, Surface target, Point pt)
        {
            drawer.draw(target, pt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool onTimerFired()
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose() { }
    }
}