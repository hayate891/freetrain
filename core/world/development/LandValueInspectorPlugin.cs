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
using System.Windows.Forms;
using System.Xml;
using FreeTrain.Contributions.Others;
using FreeTrain.Framework.plugin;

namespace FreeTrain.world.development
{
    /// <summary>
    /// 
    /// </summary>
    public class LandValueInspectorPlugin : MenuContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public LandValueInspectorPlugin(XmlElement e) : base(e) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerMenu"></param>
        public override void mergeMenu(MainMenu containerMenu)
        {
            MenuItem item = new MenuItem();
            item.Text = "Inspect Land Value";
            //! item.Text = "地価の検査";
            item.Click += new System.EventHandler(onClick);

            containerMenu.MenuItems[1].MenuItems.Add(item);
        }

        private void onClick(object sender, EventArgs args)
        {
            new LandValueInspector();
        }
    }
}
