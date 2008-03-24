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
using System.Runtime.Serialization;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.Framework.Graphics
{
    /// <summary>
    /// Sprite that draws nothing.
    /// </summary>
    [Serializable]
    public class NullSprite : Sprite, IObjectReference
    {
        private NullSprite() { }

        /// <summary>
        /// 
        /// </summary>
        public static readonly Sprite theInstance = new NullSprite();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        public void draw(Surface surface, Point pt) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        /// <param name="color"></param>
        public void drawShape(Surface surface, Point pt, Color color) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        public void drawAlpha(Surface surface, Point pt) { }

        /// <summary>
        /// 
        /// </summary>
        public Size size { get { return new Size(0, 0); } }
        /// <summary>
        /// 
        /// </summary>
        public Point offset { get { return new Point(0, 0); } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool HitTest(int x, int y) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctxt"></param>
        /// <returns></returns>
        public object GetRealObject(StreamingContext ctxt)
        {
            return theInstance;
        }
    }
}
