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
using System.Runtime.Serialization;
using freetrain.framework;
using freetrain.framework.graphics;

namespace freetrain.world.road
{
    /// <summary>
    /// 
    /// </summary>
    public enum MajorRoadType : byte 
    { 
        /// <summary>
        /// 
        /// </summary>
        unknown,
        /// <summary>
        /// 
        /// 
        /// </summary>
        footpath, 
        /// <summary>
        /// 
        /// </summary>
        street, 
        /// <summary>
        /// 
        /// </summary>
        highway 
    }
    /// <summary>
    /// 
    /// </summary>
    public enum SidewalkType : byte 
    { 
        /// <summary>
        /// 
        /// </summary>
        none, 
        /// <summary>
        /// 
        /// </summary>
        shoulder, 
        /// <summary>
        /// 
        /// </summary>
        pavement 
    }

    /// <summary>
    /// Road style.
    /// </summary>
    [Serializable]
    public struct RoadStyle
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly MajorRoadType Type;
        /// <summary>
        /// 
        /// </summary>
        public readonly SidewalkType Sidewalk;
        private readonly byte _lanes;
        /// <summary>
        /// 
        /// </summary>
        public int CarLanes { get { return _lanes; } }
        private readonly byte _option;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mt"></param>
        /// <param name="st"></param>
        /// <param name="lanes"></param>
        public RoadStyle(MajorRoadType mt, SidewalkType st, int lanes)
        {
            Type = mt;
            Sidewalk = st;
            _lanes = (byte)(lanes & 0xff);
            _option = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mt"></param>
        /// <param name="st"></param>
        /// <param name="lanes"></param>
        /// <param name="option"></param>
        public RoadStyle(MajorRoadType mt, SidewalkType st, int lanes, int option)
        {
            Type = mt;
            Sidewalk = st;
            _lanes = (byte)(lanes & 0xff);
            _option = (byte)(option & 0xff);
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[type={0},sidewalk={1},lanes={2}]", Type, Sidewalk, CarLanes);
        }
        /// <summary>
        /// 
        /// </summary>
        static public readonly RoadStyle NullStyle = new RoadStyle(0, 0, 0, 0);
    }

}
