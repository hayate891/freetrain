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
using FreeTrain.Framework;
using FreeTrain.Framework.graphics;
using SDL.net;

namespace FreeTrain.Views
{
    /// <summary>
    /// WeatherOverlayImpl の概要の説明です。
    /// </summary>
    public class WeatherOverlayImpl : WeatherOverlay
    {
        private readonly WeatherOverlaySpriteSet spriteSet;
        private Size canvasSize;
        private Surface offscreenSurface;
        private int currentFrame = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_spriteSet"></param>
        public WeatherOverlayImpl(WeatherOverlaySpriteSet _spriteSet)
        {
            this.spriteSet = _spriteSet;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (offscreenSurface != null)
            {
                offscreenSurface.Dispose();
                offscreenSurface = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sz"></param>
        public void setSize(Size sz)
        {
            if (this.canvasSize == sz) return;
            this.canvasSize = sz;
            if (offscreenSurface != null)
                offscreenSurface.Dispose();
            //offscreenSurface = MainWindow.mainWindow.directDraw.createOffscreenSurface(sz);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawer"></param>
        /// <param name="target"></param>
        /// <param name="pt"></param>
        public void draw(QuarterViewDrawer drawer, Surface target, Point pt)
        {
            drawer.draw(offscreenSurface, new Point(0, 0));
            for (int x = 0; x < canvasSize.Width; x += spriteSet.imageSize.Width)
                for (int y = 0; y < canvasSize.Height; y += spriteSet.imageSize.Height)
                    spriteSet.overlayImages[currentFrame].draw(offscreenSurface, new Point(x, y));

            target.blt(pt, offscreenSurface);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool onTimerFired()
        {
            currentFrame = (currentFrame + 1) % spriteSet.overlayImages.Length;
            return true;
        }
    }
}
