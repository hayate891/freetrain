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
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.World;

namespace FreeTrain.Contributions.Train
{
    /// <summary>
    /// Train car type.
    /// </summary>
    [Serializable]
    public abstract class TrainCarContribution : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public TrainCarContribution(XmlElement e)
            : base(e)
        {
            _capacity = int.Parse(XmlUtil.selectSingleNode(e, "capacity").InnerText);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cap"></param>
        protected TrainCarContribution(string id, int cap)
            : base("trainCar", id)
        {
            _capacity = cap;
        }

        /// <summary>
        /// Number of passengers this car can hold.
        /// </summary>
        public int capacity { get { return _capacity; } }
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected int _capacity;

        /// <summary>
        /// Draws a car to the specified position.
        /// </summary>
        /// <param name="angle">[0,16). Angle of the car. 2*direction.index</param>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        public abstract void draw(Surface display, Point pt, int angle);

        /// <summary>
        /// Dras a car on a slope.
        /// </summary>
        /// <param name="angle">one of 4 directions</param>
        /// <param name="isClimbing">true if the car is climbing</param>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        public abstract void drawSlope(Surface display, Point pt, Direction angle, bool isClimbing);

        // TODO: support cargos
    }
}
