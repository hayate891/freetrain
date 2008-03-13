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

namespace freetrain.world
{
    /// <summary>
    /// Better "Size" class.
    /// </summary>
    [Serializable]
    public struct SIZE
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sz"></param>
        public SIZE(Size sz)
            : this(sz.Width, sz.Height)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public SIZE(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }

        /// <summary>
        /// 
        /// </summary>
        public int x;
        /// <summary>
        /// 
        /// </summary>
        public int y;

        /// <summary>
        /// Area of this size.
        /// </summary>
        public int area
        {
            get
            {
                return x * y;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool is1 { get { return x == 1 && y == 1; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static implicit operator Size(SIZE sz)
        {
            return new Size(sz.x, sz.y);
        }
    }
}
