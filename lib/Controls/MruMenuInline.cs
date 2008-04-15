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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
//using Microsoft.Win32;

namespace FreeTrain.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class MruMenuInline : MruMenu
    {
        /// <summary>
        /// 
        /// </summary>
        private MenuItem firstMenuItem;

        /// <summary>
        /// 
        /// </summary>
        protected MenuItem FirstMenuItem
        {
            get { return firstMenuItem; }
            set { firstMenuItem = value; }
        }

        #region Construction

        /// <summary>
        /// MruMenuInline shows the MRU list inline (without a popup)
        /// </summary>
        public MruMenuInline(MenuItem _recentFileMenuItem, ClickHandler _clickedHandler)
            : base(_recentFileMenuItem, _clickedHandler, null, false, 4)
        {
            MaxShortenPathLength = 128;
            firstMenuItem = _recentFileMenuItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_recentFileMenuItem"></param>
        /// <param name="_clickedHandler"></param>
        /// <param name="_maxEntries"></param>
        public MruMenuInline(MenuItem _recentFileMenuItem, ClickHandler _clickedHandler, int _maxEntries)
            : base(_recentFileMenuItem, _clickedHandler, null, false, _maxEntries)
        {
            MaxShortenPathLength = 128;
            firstMenuItem = _recentFileMenuItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_recentFileMenuItem"></param>
        /// <param name="_clickedHandler"></param>
        /// <param name="_registryKeyName"></param>
        public MruMenuInline(MenuItem _recentFileMenuItem, ClickHandler _clickedHandler, String _registryKeyName)
            : base(_recentFileMenuItem, _clickedHandler, _registryKeyName, true, 4)
        {
            MaxShortenPathLength = 128;
            firstMenuItem = _recentFileMenuItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_recentFileMenuItem"></param>
        /// <param name="_clickedHandler"></param>
        /// <param name="_registryKeyName"></param>
        /// <param name="_maxEntries"></param>
        public MruMenuInline(MenuItem _recentFileMenuItem, ClickHandler _clickedHandler, String _registryKeyName, int _maxEntries)
            : base(_recentFileMenuItem, _clickedHandler, _registryKeyName, true, _maxEntries)
        {
            MaxShortenPathLength = 128;
            firstMenuItem = _recentFileMenuItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_recentFileMenuItem"></param>
        /// <param name="_clickedHandler"></param>
        /// <param name="_registryKeyName"></param>
        /// <param name="loadFromRegistry"></param>
        public MruMenuInline(MenuItem _recentFileMenuItem, ClickHandler _clickedHandler, String _registryKeyName, bool loadFromRegistry)
            : base(_recentFileMenuItem, _clickedHandler, _registryKeyName, loadFromRegistry, 4)
        {
            MaxShortenPathLength = 128;
            firstMenuItem = _recentFileMenuItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recentFileMenuItem"></param>
        /// <param name="clickedHandler"></param>
        /// <param name="registryKeyName"></param>
        /// <param name="loadFromRegistry"></param>
        /// <param name="maxEntries"></param>
        public MruMenuInline(MenuItem recentFileMenuItem, ClickHandler clickedHandler, String registryKeyName, bool loadFromRegistry, int maxEntries)
            : base(recentFileMenuItem, clickedHandler, registryKeyName, loadFromRegistry, maxEntries)
        {
            MaxShortenPathLength = 128;
            this.firstMenuItem = recentFileMenuItem;
        }
        #endregion

        #region Overridden Properties

        /// <summary>
        /// 
        /// </summary>
        public override Menu.MenuItemCollection MenuItems
        {
            get
            {
                return firstMenuItem.Parent.MenuItems;
            }
        }
        /// <summary>
        /// /
        /// </summary>
        public override int StartIndex
        {
            get
            {
                return firstMenuItem.Index;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int EndIndex
        {
            get
            {
                return StartIndex + NumEntries;
            }
        }
        #endregion

        #region Overridden Methods

        /// <summary>
        /// 
        /// </summary>
        protected override void Enable()
        {
            MenuItems.Remove(RecentFileMenuItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuItem"></param>
        protected override void SetFirstFile(MenuItem menuItem)
        {
            firstMenuItem = menuItem;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Disable()
        {
            MenuItems.Add(firstMenuItem.Index, RecentFileMenuItem);
            MenuItems.Remove(firstMenuItem);
            firstMenuItem = RecentFileMenuItem;
        }
        #endregion
    }
}