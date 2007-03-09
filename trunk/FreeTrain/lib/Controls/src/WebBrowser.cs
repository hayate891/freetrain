using System;
using MsHtmlHost;
using AxSHDocVw;

namespace freetrain.controls
{
	/// <summary>
	/// WebBrowser control.
	/// </summary>
	public class WebBrowser : AxWebBrowser
	{
		public WebBrowser() {
		}

		public void navigate( string url ) {
            object flags = 0;
            object targetFrame = String.Empty;
            object postData = String.Empty;
            object headers = String.Empty;
            base.Navigate(url, ref flags, ref targetFrame, ref postData, ref headers);
		}

		public IDocHostUIHandler docHostUIHandler {
			set {
				ICustomDoc cDoc = (ICustomDoc)base.Document;
				cDoc.SetUIHandler(value);
			}
		}
	}
}
