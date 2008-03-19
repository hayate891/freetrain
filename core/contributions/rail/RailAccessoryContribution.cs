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
using FreeTrain.Framework.plugin;
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
        private readonly string _name;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public RailAccessoryContribution(XmlElement e)
            : base(e)
        {
            _name = XmlUtil.selectSingleNode(e, "name").InnerText;
        }


        // TODO: do we need a method like
        // void create( Location loc ) ?

        #region IEntityBuilder o
        /// <summary>
        /// 
        /// </summary>
        public virtual string name { get { return _name; } }
        /// <summary>
        /// 
        /// </summary>
        public virtual BasePopulation population { get { return null; } }
        /// <summary>
        /// 
        /// </summary>
        public virtual int price { get { return 0; } }
        /// <summary>
        /// 
        /// </summary>
        public virtual double pricePerArea { get { return 0; } }
        /// <summary>
        /// 
        /// </summary>
        public bool computerCannotBuild { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public bool playerCannotBuild { get { return true; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        public abstract FreeTrain.Framework.Graphics.PreviewDrawer createPreview(System.Drawing.Size pixelSize);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public abstract FreeTrain.Controllers.ModalController createBuilder(FreeTrain.Controllers.IControllerSite site);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public abstract FreeTrain.Controllers.ModalController createRemover(FreeTrain.Controllers.IControllerSite site);

        #endregion
    }
}
