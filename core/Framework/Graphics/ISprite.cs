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
using FreeTrain.Framework.Graphics;

namespace FreeTrain.Framework.Graphics
{
    /// <summary>
    /// A sprite object that can draw itself to other surfaces.
    /// 
    /// Note that this structure doesn't handle resource management
    /// of the surface object it uses.
    /// 
    /// Sprites are serializable
    /// </summary>
    public interface ISprite
    {
        // draw a sprite to the given point.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        void Draw(Surface surface, Point pt);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        /// <param name="color"></param>
        void DrawShape(Surface surface, Point pt, Color color);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        void DrawAlpha(Surface surface, Point pt);
        /// <summary>
        /// 
        /// </summary>
        Size Size { get; }
        /// <summary>
        /// 
        /// 
        /// </summary>
        Point Offset { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool HitTest(int x, int y);
    }
}
