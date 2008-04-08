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
using System.Drawing;
using FreeTrain.Contributions.Common;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework.Graphics;
using FreeTrain.Controllers;
using FreeTrain.Contributions.Population;

namespace FreeTrain.Contributions.Rail
{
    /// <summary>
    /// Contribution that adds <c>TrafficVoxel.Accessory</c>
    /// </summary>
    [Serializable]
    public abstract class RailAccessoryContribution : Contribution, IEntityBuilder
    {
        private readonly string name;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public RailAccessoryContribution(XmlElement e)
            : base(e)
        {
            name = XmlUtil.SelectSingleNode(e, "name").InnerText;
        }


        // TODO: do we need a method like
        // void create( Location loc ) ?

        #region IEntityBuilder o
        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get { return name; } }
        /// <summary>
        /// 
        /// </summary>
        public virtual BasePopulation Population { get { return null; } }
        /// <summary>
        /// 
        /// </summary>
        public virtual int Price 
        { 
            get 
        { 
            return 0; 
        }
        set
        {
            int price = value;
        }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual double PricePerArea 
        {
            get 
        { 
            return 0; 
        }
        set
        {
            double price = value;
        }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ComputerCannotBuild { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public bool PlayerCannotBuild { get { return true; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        public abstract FreeTrain.Framework.Graphics.PreviewDrawer CreatePreview(System.Drawing.Size pixelSize);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public abstract FreeTrain.Controllers.IModalController CreateBuilder(FreeTrain.Controllers.IControllerSite site);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public abstract FreeTrain.Controllers.IModalController CreateRemover(FreeTrain.Controllers.IControllerSite site);

        #endregion
    }
}
