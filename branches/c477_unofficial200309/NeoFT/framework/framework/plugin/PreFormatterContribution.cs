using System;
using System.Xml;
using nft.util;

namespace nft.framework.plugin
{
	/// <summary>
	/// CtbPreFormatter は他のコントリビューションがplugin.xmlを読む前に
	/// 修正を加えることができる。
	/// </summary>
	public class CtbPreFormatter : Contribution
	{
		public CtbPreFormatter(XmlElement contrib):base(contrib)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}
	}
}
