using System;
using System.Xml;

namespace nft.framework.plugin
{
	/// <summary>
	/// CtbContributionDefiner defines new Contribution class.
	/// (for the needs to distinguish other stereotyped contributions.)
	/// </summary>
	public class CtbContributionDefiner : Contribution
	{
		public CtbContributionDefiner(XmlElement e) : base(e)
		{
			//e.SelectSingleNode("implementesion");
		}
		
		public Type DefinedType
		{
			get{ 
				ContributionFactory factory = PluginManager.theInstance.GetContributionFactory(this.name);
				return factory.OutputType; 
			}
		}
	}
}
