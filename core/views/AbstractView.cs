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
using freetrain.world;
using freetrain.framework;

namespace freetrain.views
{
    /// <summary>
    /// Viewからコンテナに対するインターフェース
    /// </summary>
    public interface IView
    {
        /// <summary>Starts displaying view window(s).</summary>
        void show(Form parent);

        /// <summary>Closes and disposes view window(s)</summary>
        void close();
    }

    /// <summary>
    /// Partial default implementation of IView for
    /// those views that only have one Form as its window.
    /// </summary>
    public abstract class AbstractView : IView
    {
        /// <summary>
        /// Reference to the view window.
        /// </summary>
        protected readonly Form form;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        public AbstractView(Form form)
        {
            this.form = form;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public void show(Form parent)
        {
            form.MdiParent = parent;
            form.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        public void close()
        {
            form.Close();
            form.Dispose();
            //MainWindow.mainWindow.removeView(this);
        }
    }
}
