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
using System.Xml;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Population;
using FreeTrain.Framework;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework.Plugin.Graphics;
using FreeTrain.Framework.Graphics;
using FreeTrain.World;
using FreeTrain.World.Structs;
using FreeTrain.Controllers;
using FreeTrain.Contributions.Structs;

namespace FreeTrain.World.Structs.HalfVoxelStructure
{
    #region SpriteSet
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SpriteSet
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public SpriteSet(int size)
        {
            sprites = new ISprite[size];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org"></param>
        /// <param name="variation"></param>
        public SpriteSet(SpriteSet org, Color variation)
        {
        }

        static internal int GetIndexOf(Direction d, PlaceSide s)
        {
            return d.index / 2 + (int)s * 4;
        }

        internal ISprite this[int idx]
        {
            get { return sprites[idx]; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public ISprite this[Direction d, PlaceSide s]
        {
            get
            {
                return sprites[GetIndexOf(d, s)];
            }
            set
            {
                sprites[GetIndexOf(d, s)] = value;
            }
        }

        private ISprite[] sprites;
    }
    #endregion
}
