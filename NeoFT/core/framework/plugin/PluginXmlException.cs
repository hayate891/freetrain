using System;
using System.Xml;

namespace nft.framework.plugin
{
	/// <summary>
	/// XmlFormatExceptionClass1 ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class PluginXmlException : Exception
	{
		protected XmlNode node;
		protected Plugin plugin = null;
		protected Contribution contrib = null;

		public PluginXmlException(XmlNode node, string msg)
			:base(msg)
		{
			this.node = node;
		}

		public PluginXmlException(Plugin p, XmlNode node, string msg)
			:this(node,msg)
		{
			this.plugin = p;
			ConfirmPlugin(node);
		}

		public PluginXmlException(Contribution c, XmlNode node, string msg)
			:this(node,msg)
		{
			this.contrib = c;
			this.plugin = c.parent;
			ConfirmPlugin(node);
		}

		private void ConfirmPlugin(XmlNode node)
		{
			if(plugin==null)
			{
				string pname = PluginUtil.GetPruginDirName(node);
				plugin = Core.plugins.getPlugin(pname);
			}
		}

		public string getDetailedMessage(string linecode)
		{
			string msg = base.Message;
			if(plugin==null)
			{
				msg+=string.Format("Plug-In:{0}","N/A");
				return msg;
			}
			msg+=string.Format("Plug-In:{0}{1}",plugin.name,linecode);
			msg+=string.Format("Title:{0}{1}",plugin.title,linecode);
			msg+=string.Format("Author:{0}",plugin.author);
			if(contrib!=null)
			{
				msg+=string.Format("{0}Contrib-ID:{1}",linecode,contrib.id);
				msg+=string.Format("{0}Name:{1}",linecode,contrib.name);
			}
			return msg;
		}

		public XmlNode SourceNode{ get{ return node; } }
	}
}
