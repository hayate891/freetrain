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
using System.Runtime.InteropServices;
using MsHtmlHost;

namespace freetrain.controls
{
    /// <summary>
    /// IDocHostUIHandler implementation.
    /// </summary>
    public class DocHostUIHandlerImpl : IDocHostUIHandler
    {
        private readonly object externalObject;

        /// <param name="_externalObject">
        /// This object will be accessible from within HTML as "window.external"
        /// </param>
        public DocHostUIHandlerImpl(object _externalObject)
        {
            this.externalObject = _externalObject;
        }

        void IDocHostUIHandler.EnableModeless(int fEnable)
        {
        }

        void IDocHostUIHandler.FilterDataObject(IDataObject pDO, out IDataObject ppDORet)
        {
            ppDORet = null;
        }

        void IDocHostUIHandler.GetDropTarget(IDropTarget pDropTarget, out IDropTarget ppDropTarget)
        {
            ppDropTarget = null;
        }

        void IDocHostUIHandler.GetExternal(out object ppDispatch)
        {
            ppDispatch = externalObject;
        }

        void IDocHostUIHandler.GetHostInfo(ref _DOCHOSTUIINFO pInfo)
        {
        }

        void IDocHostUIHandler.GetOptionKeyPath(out string pchKey, uint dw)
        {
            pchKey = null;
        }

        void IDocHostUIHandler.HideUI()
        {
        }

        void IDocHostUIHandler.OnDocWindowActivate(int fActivate)
        {
        }

        void IDocHostUIHandler.OnFrameWindowActivate(int fActivate)
        {
        }

        void IDocHostUIHandler.ResizeBorder(ref tagRECT prcBorder, IOleInPlaceUIWindow pUIWindow, int fRameWindow)
        {
        }

        void IDocHostUIHandler.ShowContextMenu(uint dwID, ref tagPOINT ppt, object pcmdtReserved, object pdispReserved)
        {
        }

        void IDocHostUIHandler.ShowUI(uint dwID, IOleInPlaceActiveObject pActiveObject, IOleCommandTarget pCommandTarget, IOleInPlaceFrame pFrame, IOleInPlaceUIWindow pDoc)
        {
        }

        void IDocHostUIHandler.TranslateAccelerator(ref tagMSG lpmsg, ref Guid pguidCmdGroup, uint nCmdID)
        {
            throw new COMException("", 1);	// return 1
        }

        void IDocHostUIHandler.TranslateUrl(uint dwTranslate, ref ushort pchURLIn, IntPtr ppchURLOut)
        {
        }

        void IDocHostUIHandler.UpdateUI()
        {
        }
    }
}
