using System;
using freetrain.contributions.land;

namespace freetrain.world.development
{
	/// <summary>
	/// Plan of land surfaces such as crop fields.
	/// </summary>
	class LandPlan : Plan
	{
		private readonly LandBuilderContribution contrib;
		private readonly Location loc;

		internal LandPlan( LandBuilderContribution _contrib, ULVFactory factory, Location _loc )
			: base(factory.create(new Cube(_loc,2,2,0))) {

			this.contrib = _contrib;
			this.loc = _loc;
		}

		public override int value { get { return contrib.price*4; } }

		public override Cube cube { get { return new Cube(loc,2,2,1); } }

		public override void build() {
			contrib.create(loc,loc+new Distance(1,1,0));	// inclusive
		}
	}

}
