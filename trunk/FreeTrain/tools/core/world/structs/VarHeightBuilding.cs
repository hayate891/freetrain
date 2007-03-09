using System;
using System.Drawing;
using freetrain.contributions.structs;
using freetrain.contributions.population;
using freetrain.framework;
using freetrain.framework.plugin;
using freetrain.world.subsidiaries;

namespace freetrain.world.structs
{
	/// <summary>
	/// Variable height building
	/// </summary>
	[Serializable]
	public class VarHeightBuilding : Structure, SubsidiaryEntity
	{
		public VarHeightBuilding( VarHeightBuildingContribution _type, Location loc,
			int _height, bool initiallyOwned ) {
			
			this.type = _type;
			this.height = _height;
			
			int Y = type.size.y;
			int X = type.size.x;
			int Z = height;

			voxels = new VoxelImpl[X,Y,Z];
			for( int z=0; z<Z; z++ )
				for( int y=0; y<Y; y++ )
					for( int x=0; x<X; x++ ) {
						Location l = loc;
						l.x+=x; l.y+=y; l.z+=z;
						voxels[x,y,z] = new VoxelImpl( this, (byte)x,(byte)y,(byte)z, l );
					}
			this.baseLocation = loc;

			this.subsidiary = new SubsidiaryCompany(this,initiallyOwned);
			
			if( type.population!=null )
				stationListener = new StationListenerImpl(
					new MultiplierPopulation( height, type.population ), loc );
		}

		/// <summary> Voxels that form this structure </summary>
		private readonly VoxelImpl[,,] voxels;


		private readonly VarHeightBuildingContribution type;
		
		private readonly Location baseLocation;

		private readonly SubsidiaryCompany subsidiary;

		private readonly int height;

		/// <summary>
		/// Used to draw the structure when the height-cut mode kicks in.
		/// </summary>
		private readonly Color heightCutColor = Color.Gray;


		// don't react to the mouse click
		public override bool onClick() { return false; }

		public override string name { get { return type.name; } }

		public long structurePrice {
			get {
				return type.price*height;
			}
		}

		public long totalLandPrice {
			get {
				return World.world.landValue[ baseLocation + new Distance(type.size,0)/2 ]*type.size.x*type.size.y;
			}
		}

		public Location locationClue {
			get {
				return baseLocation + new Distance(type.size,0)/2;
			}
		}


		#region Entity implementation
		public override bool isSilentlyReclaimable { get { return false; } }
		public override bool isOwned { get { return subsidiary.isOwned; } }
		public override int entityValue {
			get {
				return height * type.price;
			}
		}

		public override void remove() {
			if( stationListener!=null )
				stationListener.onRemoved();
			if( onEntityRemoved!=null )
				onEntityRemoved(this,null);

			World world = World.world;
			foreach( VoxelImpl v in voxels )
				world.remove(v);
		}

		public override event EventHandler onEntityRemoved;
		#endregion


		public override object queryInterface( Type aspect ) {
			if( aspect==typeof(rail.StationListener) )
				return stationListener;

			return base.queryInterface(aspect);
		}

		/// <summary>
		/// Station to which this structure sends population to.
		/// </summary>
		private readonly StationListenerImpl stationListener;



		/// <summary>
		/// StructureVoxel with default drawing mechanism.
		/// </summary>
		[Serializable]
		protected internal class VoxelImpl : StructureVoxel {
			protected internal VoxelImpl( VarHeightBuilding _owner, byte _x, byte _y, byte _z, Location _loc )
				: base(_owner,_loc) {

				this.x=_x;
				this.y=_y;
				this.z=_z;
			}

			protected new VarHeightBuilding owner { get { return (VarHeightBuilding)base.owner; } }
			
			/// <summary>The offset of the sprite.</summary>
			private readonly byte x,y,z;

			public override void draw( DrawContext display, Point pt, int heightCutDiff  ) {
				VarHeightBuilding o = owner;

				if( heightCutDiff<0 )
					o.type.getSprite(x,y,z,o.height).draw(display.surface,pt);
				else
				if( z==0 )
					ResourceUtil.emptyChip.drawShape(display.surface,pt, o.heightCutColor );
			}
		}


		/// <summary>
		/// Gets the station object if one is in the specified location.
		/// </summary>
		public static VarHeightBuilding get( Location loc ) {
			Voxel v = World.world[loc];
			if(!(v is VarHeightBuilding.VoxelImpl))	return null;

			return ((VarHeightBuilding.VoxelImpl)v).owner as VarHeightBuilding;
		}

		public static VarHeightBuilding get( int x, int y, int z ) { return get(new Location(x,y,z)); }
	}
}
