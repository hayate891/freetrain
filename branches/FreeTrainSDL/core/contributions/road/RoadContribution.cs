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
using System.Xml;
using FreeTrain.Contributions.Common;
using FreeTrain.Framework.graphics;
using FreeTrain.world;
using FreeTrain.world.road;

namespace FreeTrain.Contributions.Road
{
    /// <summary>
    /// Road for cars/buses
    /// </summary>
    [Serializable]
    public abstract class RoadContribution : LineContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected RoadContribution(XmlElement e)
            : base("road", e.Attributes["id"].Value)
        {
            XmlNode nd = e.SelectSingleNode("style");
            if (nd == null)
                style = RoadStyle.NullStyle;
            else
            {
                MajorRoadType mt = MajorRoadType.unknown;
                XmlAttribute major = nd.Attributes["name"];
                if (major != null)
                    mt = (MajorRoadType)Enum.Parse(mt.GetType(), major.Value, true);
                SidewalkType st = SidewalkType.none;
                XmlAttribute sidewalk = nd.Attributes["sidewalk"];
                if (sidewalk != null)
                    st = (SidewalkType)Enum.Parse(st.GetType(), sidewalk.Value, true);
                int l = 0;
                XmlAttribute lanes = nd.Attributes["lanes"];
                if (lanes != null)
                    l = int.Parse(lanes.Value);
                style = new RoadStyle(mt, st, l);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public readonly RoadStyle style;

        private int previewPatternIdx = 0;
        /// <summary>
        /// 
        /// </summary>
        public int PreviewPatternIdx
        {
            get { return previewPatternIdx; }
            set { previewPatternIdx = value % 3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[, ,] previewPattern = 
		{
			{
				{00,00,00,10,00,00,00,00,00,00},
				{00,00,00,10,00,00,00,00,00,00},
				{00,00,00,10,00,08,00,00,00,00},
				{00,00,00,06,05,11,00,00,00,00},
				{00,00,00,00,00,10,00,00,12,03},
				{00,00,00,00,00,10,00,12,03,00},
				{05,05,05,05,05,15,05,03,00,00},
				{00,00,00,00,00,02,00,00,00,00},
				{00,00,00,00,00,00,00,00,00,00},
				{00,00,00,00,00,00,00,00,00,00}
			},
			{
				{00,00,00,00,00,00,00,00,00,00},
				{00,00,00,00,00,00,00,00,00,00},
				{00,05,05,05,05,13,05,05,05,05},
				{00,00,00,00,00,10,00,00,00,00},
				{00,00,00,00,12,15,13,01,00,00},
				{00,00,00,00,14,15,11,00,00,00},
				{00,00,00,00,06,07,03,00,00,00},
				{00,00,00,00,00,00,00,00,00,00},
				{00,00,00,00,00,00,00,00,00,00},
				{00,00,00,00,00,00,00,00,00,00}
			},
			{
				{00,00,00,00,00,00,00,00,00,00},
				{00,00,00,00,00,00,00,00,00,00},
				{00,00,00,00,00,00,00,00,00,00},
				{00,00,00,00,00,00,08,00,00,00},
				{00,00,00,00,00,00,10,00,00,00},
				{00,00,00,04,05,01,10,00,00,00},
				{00,00,00,00,00,00,10,00,00,00},
				{00,00,00,00,00,00,02,00,00,00},
				{00,00,00,00,00,00,00,00,00,00},
				{00,00,00,00,00,00,00,00,00,00}
			}
		};		// roads are always 4-way.
        /// <summary>
        /// 
        /// </summary>
        public override sealed DirectionModes DirectionMode
        {
            get
            {
                return DirectionModes.FourWay;
            }
        }
    }
}
