using System;
using System.Windows.Forms;
using nft.util;
using nft.framework;

namespace nft.ui
{
	/// <summary>
	/// InformationService ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class InformationService : IMessageUIHandler
	{
		public InformationService()
		{
			UIUtil.SetHandler(this);
		}

		protected virtual MessageBoxIcon ToIcon(UIMessageType type)
		{
			switch(type)
			{
				case UIMessageType.info:
					return MessageBoxIcon.Information;
				case UIMessageType.warning:
					return MessageBoxIcon.Warning;
				case UIMessageType.alert:
					return MessageBoxIcon.Error;
				case UIMessageType.question:
					return MessageBoxIcon.Question;
				default:
					return MessageBoxIcon.None;
			}
		}
		protected virtual MessageBoxButtons ToButtons(UIQueryType query)
		{
			switch(query)
			{
				case UIQueryType.yes_no:
					return MessageBoxButtons.YesNo;
				case UIQueryType.ok_cancel:
				default:
					return MessageBoxButtons.OKCancel;
			}
		}

		public void ShowException(string text,Exception e,UIInformLevel level)
		{
			UIUtil.ShowErrorMessageBox(null, text, e);
		}
		public void ShowMessage(string text,UIMessageType type,UIInformLevel level)
		{
			MessageBox.Show(text,"Message",MessageBoxButtons.OK,ToIcon(type));
		}
		public bool ShowQueryMessage(string text,UIQueryType query,UIMessageType type,UIInformLevel level)
		{
			DialogResult r = MessageBox.Show(text,"Message",ToButtons(query),ToIcon(type));
			return true;
		}
		public void Release()
		{
		}
	}
}
