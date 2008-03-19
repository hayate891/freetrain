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
using System.Xml;
using SDL.net;
using FreeTrain.World;
using FreeTrain.Framework;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.Contributions.Train
{
    /// <summary>
    /// TrainCarContribution that draws cars from another TrainCarContribution
    /// in an opposite direction.
    /// 
    /// Intended to be used to realize the last car.
    /// </summary>
    [Serializable]
    public class ReverseTrainCarImpl : TrainCarContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public ReverseTrainCarImpl(XmlElement e)
            : base(e.Attributes["id"].Value, 0)
        {

            XmlElement b = (XmlElement)XmlUtil.selectSingleNode(e, "base");

            baseId = b.Attributes["carRef"].Value;
        }

        private string baseId;
        /// <summary>
        /// 
        /// </summary>
        protected internal override void onInitComplete()
        {
            core = Core.plugins.getContribution(baseId) as TrainCarContribution;
            if (core == null)
                throw new FormatException("'" + id + "' refers to TrainCar contribution '" + baseId + "' that could not be found");
            //! throw new FormatException("'"+id+"'が参照するTrainCarコントリビューション'"+baseId+"'が見つかりません");
            this._capacity = core.capacity;
        }


        private TrainCarContribution core;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="angle"></param>
        public override void draw(Surface display, Point pt, int angle)
        {
            angle ^= 8;
            core.draw(display, pt, angle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="angle"></param>
        /// <param name="isClimbing"></param>
        public override void drawSlope(Surface display, Point pt, Direction angle, bool isClimbing)
        {
            angle = angle.opposite;
            isClimbing = !isClimbing;

            core.drawSlope(display, pt, angle, isClimbing);
        }

    }
}
