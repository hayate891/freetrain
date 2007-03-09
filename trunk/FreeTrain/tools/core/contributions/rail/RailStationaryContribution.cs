using System;
using System.Xml;
using freetrain.contributions.common;
using freetrain.framework.plugin;
using freetrain.world;
using freetrain.world.rail;
using freetrain.world.structs;

namespace freetrain.contributions.rail
{
	/// <summary>
	/// Stationary objects related to rail road.
	/// </summary>
	[Serializable]
	public class RailStationaryContribution : FixedSizeStructureContribution
	{
		/// <summary>
		/// Parses a commercial structure contribution from a DOM node.
		/// </summary>
		/// <exception cref="XmlException">If the parsing fails</exception>
		public RailStationaryContribution( XmlElement e ) : base(e) {}

		protected override StructureGroup getGroup( string name ) {
			return PluginManager.theInstance.railStationaryGroup[name];
		}

		public override Structure create( Location baseLoc, bool initiallyOwned ) {
			return new RailStationaryStructure( this, baseLoc );
		}

		public override bool canBeBuilt( Location baseLoc ) {
			return RailStationaryStructure.canBeBuilt( baseLoc, size );
		}

	}
}
