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
using System.Xml;
using FreeTrain.Framework;
using FreeTrain.Framework.plugin;

namespace FreeTrain.Contributions.train
{
    /// <summary>
    /// Parameterized train contribution implementation
    /// where an user can specify (a) head, (b) tail, and (c) other intermediate
    /// cars separately.
    /// </summary>
    [Serializable]
    public class ParamTrainImpl : AbstractTrainContributionImpl
    {
        /// <summary>
        /// Parses a train contribution from a DOM node.
        /// </summary>
        /// <exception cref="XmlException">If the parsing fails</exception>
        public ParamTrainImpl(XmlElement e)
            : base(e)
        {
            composition = (XmlElement)XmlUtil.selectSingleNode(e, "composition");
        }

        /// <summary>
        /// &lt;composition> element in the plug-in xml file.
        /// </summary>
        private XmlElement composition;
        /// <summary>
        /// 
        /// </summary>
        public override int maxLength
        {
            get
            {
                return 16;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override int minLength
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal override void onInitComplete()
        {
            base.onInitComplete();

            carHeadType = getCarType(composition, "head");
            carBodyType = getCarType(composition, "body");
            carTailType = getCarType(composition, "tail");

            if (carBodyType == null)
                throw new FormatException("<body> part was not specified");
            //! throw new FormatException("<body>要素が指定されませんでした");

            composition = null;
        }

        private TrainCarContribution getCarType(XmlElement comp, string name)
        {
            XmlElement e = (XmlElement)comp.SelectSingleNode(name);
            if (e == null) return null;

            string idref = e.Attributes["carRef"].Value;
            if (id == null) throw new FormatException("carRef܂");

            TrainCarContribution contrib = (TrainCarContribution)Core.plugins.getContribution(idref);
            if (contrib == null) throw new FormatException(
                 string.Format("id='{0}'TrainCarRgr[V܂", idref));

            return contrib;
        }

        private TrainCarContribution carHeadType, carBodyType, carTailType;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public override TrainCarContribution[] create(int length)
        {
            TrainCarContribution[] r = new TrainCarContribution[length];
            for (int i = 0; i < r.Length; i++)
                r[i] = carBodyType;

            if (carHeadType != null)
                r[0] = carHeadType;

            if (carTailType != null)
                r[r.Length - 1] = carTailType;

            return r;
        }

    }
}
