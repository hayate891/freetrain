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

namespace FreeTrain.World
{
    /// <summary>
    /// Empty, in the sense that nothing will be drawn, but
    /// occupied, in the sense that the space is already in use.
    /// </summary>
    [Serializable]
    public class EmptyVoxel : AbstractVoxelImpl
    {
        private readonly Entity _entity;
        /// <summary>
        /// 
        /// </summary>
        public override bool transparent { get { return true; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public EmptyVoxel(Entity e, int x, int y, int z) : this(e, new Location(x, y, z)) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="loc"></param>
        public EmptyVoxel(Entity e, Location loc)
            : base(loc)
        {
            this._entity = e;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        /// <param name="heightCutDiff"></param>
        public override void draw(DrawContext surface, Point pt, int heightCutDiff)
        {
            // draw nothing
        }
        /// <summary>
        /// 
        /// </summary>
        public override Entity entity { get { return _entity; } }
    }
}
