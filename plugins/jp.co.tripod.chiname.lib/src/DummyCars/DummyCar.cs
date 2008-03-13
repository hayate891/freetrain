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
using freetrain.world;

namespace freetrain.world.road.dummycar
{
    /// <summary>
    /// Accessory implementation.
    /// </summary>
    [Serializable]
    public class DummyCar : TrafficVoxel.Accessory
    {
        private readonly byte index;
        private readonly DummyCarContribution contrib;
        private readonly int color;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="_contrib"></param>
        /// <param name="_color"></param>
        /// <param name="_index"></param>
        [CLSCompliant(false)]
        public DummyCar(TrafficVoxel target, DummyCarContribution _contrib, int _color, int _index)
        {
            this.index = (byte)_index;
            this.contrib = _contrib;
            this.color = _color;
            target.accessory = this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        [CLSCompliant(false)]
        public void drawBefore(DrawContext display, Point pt)
        {
            //contrib.sprites[index].draw( display.surface, pt );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        [CLSCompliant(false)]
        public void drawAfter(DrawContext display, Point pt)
        {
            contrib.sprites[color, index].draw(display.surface, pt);
        }
        /// <summary>
        /// 
        /// </summary>
        public void onRemoved() { }
    }
}
