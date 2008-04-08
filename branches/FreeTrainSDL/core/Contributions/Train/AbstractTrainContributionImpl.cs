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
                    minutesPerVoxel = 4; break;
                case "medium":
                    minutesPerVoxel = 3; break;
                case "fast":
                    minutesPerVoxel = 2; break;
                case "superb":
                    minutesPerVoxel = 1; break;
                default:
                    try
                    {
                        minutesPerVoxel = int.Parse(speedStr);
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
        public override string Name { get { return _typeName + " " + _nickName; } }

        private readonly string _nickName;
        /// <summary>
        /// 
        /// </summary>
        public override string NickName { get { return _nickName; } }

        private readonly string _typeName;
        /// <summary>
        /// 
        /// </summary>
        public override string TypeName { get { return _typeName; } }

        private readonly string _author;
        /// <summary>
        /// 
        /// </summary>
        public override string Author { get { return _author; } }

        private readonly string _companyName;
        /// <summary>
        /// 
        /// </summary>
        public override string CompanyName { get { return _companyName; } }

        private readonly string _description;
        /// <summary>
        /// 
        /// </summary>
        public override string Description { get { return _description; } }

        private readonly int _price;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public override int Price(int length) { return _price * length; }

        private readonly int minutesPerVoxel;
        /// <summary>
        ///
        /// </summary>
        public override int MinutesPerVoxel { get { return minutesPerVoxel; } }

        readonly int fare = 600;	// TODO
        /// <summary>
        /// 
        /// </summary>
        public override int Fare { get { return fare; } }

        readonly int maxlength = 3;	// TODO
        /// <summary>
        /// 
        /// </summary>
        public override int MaxLength { get { return maxlength; } }
    }
}
