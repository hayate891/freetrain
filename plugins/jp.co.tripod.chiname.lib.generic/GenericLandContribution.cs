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
using System.Diagnostics;
using System.Xml;
using System.Collections;
using FreeTrain.Framework;
using FreeTrain.Framework.graphics;
using FreeTrain.Framework.plugin;
using FreeTrain.Controllers;
using FreeTrain.Controllers.Structs;
using FreeTrain.Contributions;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.population;
using FreeTrain.Contributions.Structs;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.world;
using FreeTrain.world.Structs;

namespace FreeTrain.Framework.plugin.Generic
{
    /// <summary>
    /// GenericLandContribution
    /// </summary>
    [CLSCompliant(false)]
    public class GenericLandContribution : GenericStructureContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public GenericLandContribution(XmlElement e)
            : base(e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void loadPrimitiveParams(XmlElement e)
        {
            XmlNode xn = e.SelectSingleNode("structure");
            if (xn != null)
                _categories = new StructCategories(xn, this.id);
            else
                _categories = new StructCategories();

            if (_categories.Count == 0)
            {
                StructCategory.Root.Entries.Add(this.id);
                _categories.Add(StructCategory.Root);
            }

            try
            {
                _design = e.SelectSingleNode("design").InnerText;
            }
            catch
            {
                //! _design = "標準";
                _design = "default";
            }

            try
            {
                _unitPrice = int.Parse(XmlUtil.selectSingleNode(e, "price").InnerText);
            }
            catch
            {
                _unitPrice = 0;
            }

            _size = new SIZE(1, 1);

            _minHeight = 2;
            _maxHeight = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="color"></param>
        /// <param name="contrib"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        protected override Contribution createPrimitiveContrib(XmlElement sprite, XmlNode color, XmlElement contrib)
        {
            sprite.AppendChild(color.Clone());
            PluginManager manager = PluginManager.theInstance;
            ContributionFactory factory = manager.getContributionFactory("land");
            XmlNode temp = contrib.Clone();
            foreach (XmlNode cn in temp.ChildNodes)
            {
                if (cn.Name.Equals("sptite") || cn.Name.Equals("picture"))
                    temp.RemoveChild(cn);
            }
            temp.AppendChild(sprite);
            contrib.AppendChild(temp);
            return factory.load(parent, (XmlElement)temp);
        }

    }
}
