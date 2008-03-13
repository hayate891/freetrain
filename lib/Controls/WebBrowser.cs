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
using MsHtmlHost;
using AxSHDocVw;

namespace freetrain.controls
{
    /// <summary>
    /// WebBrowser control.
    /// </summary>
    [CLSCompliant(false)]
    public class WebBrowser : AxWebBrowser
    {
        /// <summary>
        /// 
        /// </summary>
        public WebBrowser()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void navigate(string url)
        {
            object flags = 0;
            object targetFrame = String.Empty;
            object postData = String.Empty;
            object headers = String.Empty;
            base.Navigate(url, ref flags, ref targetFrame, ref postData, ref headers);
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public IDocHostUIHandler docHostUIHandler
        {
            set
            {
                ICustomDoc cDoc = (ICustomDoc)base.Document;
                cDoc.SetUIHandler(value);
            }
        }
    }
}
