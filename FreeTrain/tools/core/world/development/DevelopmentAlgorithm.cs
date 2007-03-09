using System;
using System.Collections;
using System.Diagnostics;
using freetrain.contributions.common;
using freetrain.contributions.land;
using freetrain.contributions.structs;
using freetrain.framework;
using freetrain.framework.plugin;
using freetrain.util;
using freetrain.world.rail;
using freetrain.world.land;
using freetrain.world.structs;

namespace freetrain.world.development
{
	/// <summary>
	/// Receives clock event and build a new structure if appropriate
	/// </summary>
	public class DevelopmentAlgorithm : ULVFactory
	{
		private static readonly Random rnd = new Random();

		/// <summary>
		/// Invoked by the timer.
		/// Run the development algorithm.
		/// </summary>
		public static void handleClock() {
			DateTime start = DateTime.Now;
			new DevelopmentAlgorithm().doClock();
			double d = (DateTime.Now - start).TotalMilliseconds;
			Debug.WriteLine( "development: "+d+"ms" );
		}


		private DevelopmentAlgorithm() {}	// shouldn't be instanciated from outside

		/// <summary>
		/// Dictionary from Cube to its ULV.
		/// </summary>
		private readonly IDictionary ULVs = new Hashtable();


		/// <summary>
		/// Do the actual development algorithm.
		/// </summary>
		private void doClock() {
			Location loc;
			World w = World.world;


			switch( rnd.Next(2) ) {
			case 0:
				// completely randomly select a voxel from a map
				Distance sz = w.size;
				loc = new Location( rnd.Next(sz.x), rnd.Next(sz.y), 0 );
				break;
			case 1:
				// randomly select a station
				if( w.stations.Count==0 )
					return;
				Station st = w.stations.get( rnd.Next(w.stations.Count) );
				loc = st.baseLocation;
				// then randomly pick nearby voxel
				loc.x += rnd.Next(41)-20;
				loc.y += rnd.Next(41)-20;
				break;
			default:
				return;
			}

			loc.z = w.getGroundLevel(loc);
			

			Voxel v = w[loc];

			// quick rejection for increased performance
			if( v!=null && v.entity.isOwned )	return;	// cannot replace this voxel.
			if( loc.z < w.waterLevel )			return;	// below water



			Hashtable structs = new Hashtable();	// map from Builder->probability
			double denominator = 0;
			foreach( Contribution contrib in Core.plugins.contributions ) {
				Plan plan=null;
				if( contrib is EntityBuilderContribution ) {
					if(((EntityBuilderContribution)contrib).computerCannotBuild)
						continue;
				}

				if( contrib is LandBuilderContribution ) {
					plan = new LandPlan(
						(LandBuilderContribution)contrib,
						this, loc );
				} else
				if( contrib is CommercialStructureContribution ) {
					CommercialStructureContribution csc = (CommercialStructureContribution)contrib;
					if( csc.canBeBuilt(loc) )
						plan = new CommercialStructurePlan( csc, this, loc );
				} else
				if( contrib is VarHeightBuildingContribution ) {
					// TODO: how to determine the height?
					VarHeightBuildingContribution vhbc = (VarHeightBuildingContribution)contrib;
					if( vhbc.canBeBuilt(loc,3) )
						plan = new VarHeightBuildingPlan( vhbc, this, loc, 3 );
				} else
					continue;

				if( plan==null || plan.ulv==null )	continue;	// not a valid plan

				double prob = f1(plan.ulv.landValue - plan.value);
				prob *= plan.bias;

				structs.Add( plan, prob*(1-f2(plan.ulv.landValue-plan.ulv.entityValue)) );
				denominator += prob;
			}


			///////////////////////////////////////////////////////////


			// roll a dice and determine what to build
			double d = rnd.NextDouble()*denominator;

			foreach( Plan plan in structs.Keys ) {
				d -= (double)structs[plan];
				if( d<0 ) {
					// remove obstacles
					bool OK = true;
					Entity[] es = plan.cube.getEntities();
					foreach( Entity e in es ) {
						if( e.isOwned ) {
							OK = false;
							break;
						}
					}

					if( !OK )	continue;	// there's no room for this structure

					foreach( Entity e in es )
						e.remove();

					// realize this plan.
					plan.build();
					return;
				}
			}
		}

		private static double f1( double diff ) {
			return Math.Pow( Math.E,  -diff*diff );
		}
		private static double f2( double diff ) {
			return Math.Pow( Math.E,  -diff*diff/100.0 );
		}



		
		/// <summary>
		/// Computes the "unused land value."
		/// If any structure cannot be built in this cube. returns null.
		/// </summary>
		public ULV create( Cube cube ) {
			ULV ulv = (ULV)ULVs[cube];
			if( ulv==null )
				ULVs[cube] = ulv = ULV.create(cube);

			return ulv;
		}
	}
}
