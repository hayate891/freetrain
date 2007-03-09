using System;

namespace freetrain.world.development
{
	/// <summary>
	/// Computes and maintains land value.
	/// 
	/// This algorithm is based on the heat conductivity model,
	/// where the source of value is considered as a heat source,
	/// and temprature is in turn considered as land value.
	/// </summary>
	[Serializable]
	public sealed class LandValue
	{
		/// <summary>
		/// Creates a new object and associates that with the world.
		/// </summary>
		/// <param name="w"></param>
		public LandValue( World w ) {
			w.otherObjects["{51CD7E24-4296-4043-B58D-A654AB71F121}"] = this;

			H = w.size.x;
			V = w.size.y;
			q = new float[H+2,V+2];
			back = new float[H+2,V+2];
			rho = new float[H+2,V+2];

			// fill the array by 1.
			for( int h=H; h>0; h-- )
				for( int v=V; v>0; v-- )
					rho[h,v] = 1.0f;

			// register the event notification so that we can update rho correctly
			w.onVoxelChanged += new VoxelChangeListener(updateRho);
			w.clock.registerRepeated( new ClockHandler(next), TimeLength.fromHours(UPDATE_FREQUENCY) );
		}
		
		/// <summary> "tempratures" for each (h,v) </summary>
		private float[,] q;
		
		/// <summary> back buffer </summary>
		private float[,] back;

		/// <summary> heat conductivity (0-1) </summary>
		private float[,] rho;

		// size of the world
		private readonly int H;
		private readonly int V;

		/// <summary>
		/// Returns the land value for the given voxel.
		/// </summary>
		public int this [ int h, int v ] {
			get {
				return (int)Math.Pow( q[h+1,v+1], 0.3 )*10;
			}
		}

		/// <summary>
		/// Returns the land value for the given voxel.
		/// </summary>
		public int this [ Location loc ] {
			get {
				int h,v;
				World.world.toHV( loc.x, loc.y, out h, out v );
				return this[h,v];
			}
		}

		/// <summary>
		/// Made public just because of a bug in .NET.
		/// Compute the next step.
		/// </summary>
		public void next() {
			{// flip the buffer
				float[,] t = q;
				q = back;
				back = t;
			}

			// compute next
			for( int h=H; h>0; h-- ) {
				for( int v=V; v>0; v-- ) {
					float t = back[h,v];
					float tr;
					if( (v%2)==0 ) {
						tr = (back[h  ,v-1]-t)*rho[h  ,v-1];
						tr+= (back[h  ,v+1]-t)*rho[h  ,v+1];
						tr+= (back[h+1,v-1]-t)*rho[h+1,v-1];
						tr+= (back[h+1,v+1]-t)*rho[h+1,v+1];
					} else {
						tr = (back[h  ,v-1]-t)*rho[h  ,v-1];
						tr+= (back[h  ,v+1]-t)*rho[h  ,v+1];
						tr+= (back[h-1,v-1]-t)*rho[h-1,v-1];
						tr+= (back[h-1,v+1]-t)*rho[h-1,v+1];
					}

					t = back[h,v]*DIFF + tr * ALPHA * rho[h,v];
					if(t<0)	t=0;	// try to save the algorithm just in case something goes terribly wrong
					q[h,v] = t;
				}
			}
		}
		
		/// <summary>
		/// Deposites "heat".
		/// </summary>
		public void addQ( Location loc, float deltaQ ) {
			int h,v;
			World.world.toHV( loc, out h, out v );
			q[h,v] += deltaQ * UPDATE_FREQUENCY * 10;
		}

		/// <summary>
		/// Public simply because of a bug in .NET
		/// Updates the heat conductivity according to the voxels we have.
		/// </summary>
		public void updateRho( Location loc ) {
			int h,v;
			World.world.toHV( loc.x, loc.y, out h, out v );
			
			bool hasRoad = false;
			bool hasMountain = false;

			// FIXME: this code shouldn't have the knowledge of any particular voxel type.
			for( int z=0; z<World.world.size.z; z++ ) {
				Voxel vxl = World.world[loc.x,loc.y,z];
				if( vxl is TrafficVoxel && ((TrafficVoxel)vxl).road!=null )
					hasRoad = true;
				if( vxl is terrain.MountainVoxel )
					hasMountain = true;
			}
			
			bool hasSea = World.world.getGroundLevelFromHV(h,v) < World.world.waterLevel;

			if( hasRoad ) {
				rho[h,v] = 1.4f;
			} else if( hasSea ) {
				rho[h,v] = 0.2f;
			} else if( hasMountain ) {
				rho[h,v] = 0.6f;
			} else {
				rho[h,v] = 1.0f;
			}
		}

		/// <summary>
		/// Heat conductivity factor.
		/// The larger the value, the faster heat spreads.
		/// No more than 0.25. Otherwise the model becomes chaotic.
		/// </summary>
		private const float ALPHA = 0.245f;
		
		/// <summary>
		/// Diffusion. 1-epsilon.
		/// The larger the epsilon, the more heat evaporates.
		/// </summary>
		private const float DIFF = 1f-0.001f;

		/// <summary>
		/// N where the land values are recomputed for every N hours.
		/// </summary>
		private const int UPDATE_FREQUENCY = 6;
	}
}
