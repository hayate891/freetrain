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
using FreeTrain.Framework.plugin;
using FreeTrain.Contributions.train;

namespace FreeTrain.world.Rail.cttrain
{
    /// <summary>
    /// TrainCar contribution implementation for ColorTestTrain.
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public class ColorTestTrainCar : ColoredTrainCarImpl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public ColorTestTrainCar(XmlElement e)
            :
            base(
                ColoredTrainPictureContribution.list()[0],
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                e.Attributes["id"].Value, 0)
        {

            theInstance = this;
        }

        /// <summary>
        /// 
        /// </summary>
        public static ColorTestTrainCar theInstance;
    }
}
