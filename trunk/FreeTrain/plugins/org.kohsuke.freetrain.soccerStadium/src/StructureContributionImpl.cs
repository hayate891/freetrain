using System;
using System.Xml;
using freetrain.contributions.common;
using freetrain.contributions.structs;
using freetrain.world.structs;

namespace freetrain.world.soccerstadium
{
	/// <summary>
	/// structure contribution for soccer stadium.
	/// </summary>
	[Serializable]
	public class StructureContributionImpl : FixedSizeStructureContribution
	{
		// contains stadium contribution alone
		internal static readonly StructureGroupGroup groupGroup = new StructureGroupGroup();

		public StructureContributionImpl( XmlElement e ) : base(e) {}

		protected override StructureGroup getGroup( string name ) {
			return groupGroup[name];
		}

		public override Structure create( Location baseLoc, bool initiallyOwned ) {
			return new StadiumStructure(this,baseLoc);
		}

		public override bool canBeBuilt( Location baseLoc ) {
			return StadiumStructure.canBeBuilt( baseLoc, size );
		}
	}
}
