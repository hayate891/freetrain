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
using System.Diagnostics;
using System.Windows.Forms;

namespace FreeTrain.Framework
{
    /// <summary>
    /// Enumeration of the menus.
    /// </summary>
    public sealed class MenuItemConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly string displayName;

        private MenuItemConstants(string _displayName)
        {
            this.displayName = _displayName;
        }

        /// <summary>
        /// Obtain a menu item constants from its display name.
        /// Throws an exception if it fails to parse.
        /// </summary>
        public static MenuItemConstants parse(string name)
        {
            foreach (MenuItemConstants c in
                new MenuItemConstants[] { FILE, VIEW, RAIL, ROAD, CONSTRUCTION, CONFIG, HELP })
            {

                if (c.displayName.Equals(name))
                    return c;
            }

            throw new Exception("undefined menu item:" + name);
        }

        /// <summary>
        /// Associated menu item.
        /// </summary>
        public MenuItem menuItem
        {
            get
            {
                MainWindow mw = MainWindow.mainWindow;
                // TODO: is there any better way to implement this?
                if (this == FILE) return mw.menuItem_file;
                if (this == VIEW) return mw.menuItem_view;
                if (this == RAIL) return mw.menuItem_rail;
                if (this == ROAD) return mw.menuItem_road;
                if (this == CONSTRUCTION) return mw.menuItem_construction;
                if (this == CONFIG) return mw.menuItem_config;
                if (this == HELP) return mw.menuItem_help;

                Debug.Fail("undefined menu item constant");
                return null;
            }
        }

        //
        // constants
        // 
        /// <summary>
        /// 
        /// </summary>
        public static readonly MenuItemConstants FILE = new MenuItemConstants("file");
        /// <summary>
        /// 
        /// </summary>
        public static readonly MenuItemConstants VIEW = new MenuItemConstants("view");
        /// <summary>
        /// 
        /// </summary>
        public static readonly MenuItemConstants RAIL = new MenuItemConstants("rail");
        /// <summary>
        /// 
        /// </summary>
        public static readonly MenuItemConstants ROAD = new MenuItemConstants("road");
        /// <summary>
        /// 
        /// 
        /// </summary>
        public static readonly MenuItemConstants CONSTRUCTION = new MenuItemConstants("construction");
        /// <summary>
        /// 
        /// </summary>
        public static readonly MenuItemConstants CONFIG = new MenuItemConstants("config");
        /// <summary>
        /// 
        /// </summary>
        public static readonly MenuItemConstants HELP = new MenuItemConstants("help");

        private static readonly MenuItemConstants[] all = {
			FILE, VIEW, RAIL, ROAD, CONSTRUCTION, CONFIG, HELP
		};
    }
}
