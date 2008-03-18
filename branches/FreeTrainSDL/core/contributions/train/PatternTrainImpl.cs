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
using System.Collections;
using System.Xml;
using FreeTrain.Framework;
using FreeTrain.Framework.plugin;

namespace FreeTrain.Contributions.Train
{
    /// <summary>
    /// Parameterized train contribution implementation
    /// where an user can specify (a) head, (b) tail, and (c) other intermediate
    /// cars separately.
    /// </summary>
    [Serializable]
    public class PatternTrainImpl : AbstractTrainContributionImpl
    {
        /// <summary>
        /// Parses a train contribution from a DOM node.
        /// </summary>
        /// <exception cref="XmlException">If the parsing fails</exception>
        public PatternTrainImpl(XmlElement e)
            : base(e)
        {
            config = (XmlElement)XmlUtil.selectSingleNode(e, "config");
        }

        /// <summary>
        /// &lt;config> element in the plug-in xml file.
        /// </summary>
        private XmlElement config;

        /// <summary>
        /// Map from a char 'x' to TrainCarContribution
        /// </summary>
        private readonly IDictionary cars = new Hashtable();
        /// <summary>
        /// 
        /// </summary>
        public override int maxLength
        {
            get
            {
                int curMax = 0;
                for (int i = 16; i > 0 && curMax == 0; i--)
                    if (compositions[i] != null) curMax = i;
                return curMax;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override int minLength
        {
            get
            {
                int curMin = 0;
                for (int i = 1; i > 0 && curMin == 0; i++)
                    if (compositions[i] != null) curMin = i;
                return curMin;
            }
        }


        /// <summary>
        /// Map from length to its composition.
        /// </summary>
        private readonly IDictionary compositions = new Hashtable();
        /// <summary>
        /// 
        /// </summary>
        protected internal override void onInitComplete()
        {
            base.onInitComplete();

            XmlNodeList lst = config.SelectNodes("car");
            foreach (XmlElement e in lst)
                cars.Add(e.Attributes["char"].Value[0], getCarType(e));

            lst = config.SelectNodes("composition");
            foreach (XmlElement e in lst)
                loadComposition(e);

            config = null;
        }

        private TrainCarContribution getCarType(XmlElement e)
        {
            string idref = e.Attributes["ref"].Value;
            if (idref == null) throw new FormatException("ref attribute is missing");
            //! if(idref==null)	throw new FormatException("ref属性がありません");

            TrainCarContribution contrib = (TrainCarContribution)Core.plugins.getContribution(idref);
            if (contrib == null) throw new FormatException(
                 string.Format("id='{0}' is missing TrainCar contribution", idref));
            //! string.Format( "id='{0}'のTrainCarコントリビューションがありません", idref ));

            return contrib;
        }

        private void loadComposition(XmlElement e)
        {
            string comp = e.InnerText;
            ArrayList a = new ArrayList();

            while (comp.Length != 0)
            {
                char head = comp[0];
                comp = comp.Substring(1);

                if (Char.IsWhiteSpace(head))
                    continue;	// ignore whitespace

                // otherwise look up a table
                TrainCarContribution car = (TrainCarContribution)cars[head];
                if (car == null)
                    throw new FormatException("The following characters are undefined: " + head);
                //! throw new FormatException("次の文字は定義されていません:"+head);
                a.Add(car);
            }

            compositions.Add(a.Count, a.ToArray(typeof(TrainCarContribution)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public override TrainCarContribution[] create(int length)
        {
            TrainCarContribution[] r = (TrainCarContribution[])compositions[length];
            if (r == null) return null;
            else return (TrainCarContribution[])r.Clone();
        }

    }
}
