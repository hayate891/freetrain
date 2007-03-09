using System;
using System.Xml;
using freetrain.contributions.common;
using freetrain.framework.plugin;
using freetrain.world;
using freetrain.world.structs;

namespace freetrain.contributions.structs
{
	/// <summary>
	/// commercial structure.
	/// 
	/// Including everything from convenience stores (like Seven-eleven)
	/// to shopping malls like Walmart.
	/// </summary>
	[Serializable]
	public class CommercialStructureContribution : FixedSizeStructureContribution
	{
		/// <summary>
		/// Parses a commercial structure contribution from a DOM node.
		/// </summary>
		/// <exception cref="XmlException">If the parsing fails</exception>
		public CommercialStructureContribution( XmlElement e ) : base(e) {}

		protected override StructureGroup getGroup( string name ) {
			return PluginManager.theInstance.commercialStructureGroup[name];
		}

		public override Structure create( Location baseLoc, bool initiallyOwned ) {
			return new Commercial( this, baseLoc, initiallyOwned );
		}

		public override bool canBeBuilt( Location baseLoc ) {
			return Commercial.canBeBuilt( baseLoc, size );
		}

		// TODO: additional parameters, like population and attractiveness.
	}
}
