using System;
using System.Xml;
using nft.util;

namespace nft.framework.plugin
{
	/// <summary>
	/// PreFormatterContribution は他のコントリビューションがplugin.xmlを読む前に
	/// 修正を加えることができる。
	/// </summary>
	public class PreFormatterContribution : Contribution
	{
		public PreFormatterContribution(XmlElement contrib):base(contrib)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}
	}
}
