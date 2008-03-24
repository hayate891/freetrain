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
using System.Diagnostics;
using System.Drawing;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.World.Land
{
    /// <summary>
    /// Player-owned land property.
    /// </summary>
    [Serializable]
    public class LandPropertyVoxel : AbstractVoxelImpl, Entity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        public LandPropertyVoxel(Location loc)
            : base(loc)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public override Entity entity { get { return this; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        /// <param name="heightCutDiff"></param>
        public override void draw(DrawContext surface, Point pt, int heightCutDiff)
        {
            sprite.draw(surface.surface, pt);
        }

        #region Entity implementation
        /// <summary>
        /// 
        /// </summary>
        public bool isSilentlyReclaimable { get { return true; } }
        /// <summary>
        /// 
        /// </summary>
        public bool isOwned { get { return true; } }
        /// <summary>
        /// 
        /// </summary>
        public int entityValue { get { return 0; } }
        /// <summary>
        /// 
        /// </summary>
        public void remove()
        {
            WorldDefinition.world.remove(this);
            if (onEntityRemoved != null) onEntityRemoved(this, null);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler onEntityRemoved;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public static readonly Sprite sprite;

        static LandPropertyVoxel()
        {
            Picture pic = PictureManager.get("{0E7A9F09-4482-4b78-8A8D-F59F02574B1B}");
            sprite = new SimpleSprite(pic, new Point(0, 0), new Point(0, 0), new Size(32, 16));
        }
    }
}
