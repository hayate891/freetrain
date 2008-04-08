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
using FreeTrain.Framework.Plugin;

namespace FreeTrain.Contributions.Train
{
    /// <summary>
    /// Common base implementation of TrainContribution
    /// </summary>
    [Serializable]
    public abstract class AbstractTrainContributionImpl : TrainContribution
    {
        /// <summary>
        /// Parses a train contribution from a DOM node.
        /// </summary>
        /// <exception cref="XmlException">If the parsing fails</exception>
        protected AbstractTrainContributionImpl(XmlElement e)
            : base(e.Attributes["id"].Value)
        {
            _companyName = XmlUtil.SelectSingleNode(e, "company").InnerText;
            _typeName = XmlUtil.SelectSingleNode(e, "type").InnerText;
            _nickName = XmlUtil.SelectSingleNode(e, "name").InnerText;
            _description = XmlUtil.SelectSingleNode(e, "description").InnerText;
            _author = XmlUtil.SelectSingleNode(e, "author").InnerText;

            _price = int.Parse(XmlUtil.SelectSingleNode(e, "price").InnerText);
            // TODO: pictures

            string speedStr = XmlUtil.SelectSingleNode(e, "speed").InnerText.ToLower();
            switch (speedStr)
            {
                case "slow":
                    _minutesPerVoxel = 4; break;
                case "medium":
                    _minutesPerVoxel = 3; break;
                case "fast":
                    _minutesPerVoxel = 2; break;
                case "superb":
                    _minutesPerVoxel = 1; break;
                default:
                    try
                    {
                        _minutesPerVoxel = int.Parse(speedStr);
                    }
                    catch (Exception)
                    {
                        throw new XmlException("unknown speed:" + speedStr, null);
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string name { get { return _typeName + " " + _nickName; } }

        private readonly string _nickName;
        /// <summary>
        /// 
        /// </summary>
        public override string nickName { get { return _nickName; } }

        private readonly string _typeName;
        /// <summary>
        /// 
        /// </summary>
        public override string typeName { get { return _typeName; } }

        private readonly string _author;
        /// <summary>
        /// 
        /// </summary>
        public override string author { get { return _author; } }

        private readonly string _companyName;
        /// <summary>
        /// 
        /// </summary>
        public override string companyName { get { return _companyName; } }

        private readonly string _description;
        /// <summary>
        /// 
        /// </summary>
        public override string description { get { return _description; } }

        private readonly int _price;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public override int price(int length) { return _price * length; }

        private readonly int _minutesPerVoxel;
        /// <summary>
        ///
        /// </summary>
        public override int minutesPerVoxel { get { return _minutesPerVoxel; } }
        /// <summary>
        /// 
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public readonly int _fare = 600;	// TODO
        /// <summary>
        /// 
        /// </summary>
        public override int fare { get { return _fare; } }
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public readonly int _maxlength = 3;	// TODO
        /// <summary>
        /// 
        /// </summary>
        public override int maxLength { get { return _maxlength; } }
    }
}
