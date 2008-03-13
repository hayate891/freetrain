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
using System.Windows.Forms;
using freetrain.framework;
using freetrain.contributions.others;
using freetrain.util.command;

namespace freetrain.framework.plugin.generic
{
    /// <summary>
    /// Adds "signal" menu to the main window
    /// </summary>
    [CLSCompliant(false)]
    public class MenuContributionImpl : MenuContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public MenuContributionImpl(XmlElement e) : base(e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerMenu"></param>
        public override void mergeMenu(MainMenu containerMenu)
        {
            //! MenuItem item = new MenuItem("(&S)...");
            MenuItem item = new MenuItem("&Structure Type Tree...");
            item.Click += new System.EventHandler(onClick);

            containerMenu.MenuItems[4].MenuItems.Add(0, item);
            //MainWindow.mainWindow.SetToolBarButtonHandler("toolBar1",10,new CommandHandlerNoArg(this.ShowControllerForm));
        }

        private void onClick(object sender, EventArgs args)
        {
            ShowControllerForm();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowControllerForm()
        {
            MultiSelectorController.create();
        }

    }
}
