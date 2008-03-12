#region LICENSE
/*
 * Copyright (C) 2004 - 2007 David Hudson (jendave@yahoo.com)
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
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using freetrain.contributions.others;
using freetrain.framework;
using freetrain.framework.plugin;

namespace freetrain.world.rail.cttrain
{
    /// <summary>
    /// Menu item contribution that allows an user to
    /// open a color config dialog.
    /// </summary>
    [CLSCompliant(false)]
    public class ColorConfigCommand : MenuContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public ColorConfigCommand(XmlElement e) : base(e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerMenu"></param>
        public override void mergeMenu(MainMenu containerMenu)
        {
            MenuItem item = new MenuItem();
            item.Text = "Color Test Train Settings";
            item.Click += new System.EventHandler(onClick);

            containerMenu.MenuItems[1].MenuItems.Add(item);
        }

        private void onClick(object sender, EventArgs args)
        {
            Form form = new ColorConfigDialog(ColorTestTrainCar.theInstance);
            MainWindow.mainWindow.AddOwnedForm(form);
            form.Show();
        }
    }
}
