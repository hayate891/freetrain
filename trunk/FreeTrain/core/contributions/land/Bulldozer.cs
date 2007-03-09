using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml;
using org.kohsuke.directdraw;
using freetrain.controllers;
using freetrain.framework.graphics;
using freetrain.framework.plugin;
using freetrain.framework;
using freetrain.views;
using freetrain.world;
using freetrain.world.terrain;
using freetrain.world.land;

namespace freetrain.contributions.land
{
	/// <summary>
	/// Removes any land voxel in the region.
	/// </summary>
	public class Bulldozer : LandBuilderContribution
	{
		public Bulldozer( XmlElement e ) : base(e) {
		}

		/// <summary>
		/// Gets the land that should be used to fill (x,y) within [x1,y1]-[x2,y2] (inclusive).
		/// </summary>
		public override void create( int x1, int y1, int x2, int y2, int z ) {
			bulldoze(new Location(x1,y1,z),new Location(x2,y2,z));
		}

		public static void bulldoze( Location loc1, Location loc2 ) {
			int z = loc1.z;
			for( int x=loc1.x; x<=loc2.x; x++ ) {
				for( int y=loc1.y; y<=loc2.y; y++ ) {
					if( World.world.isReusable(x,y,z) && World.world[x,y,z]!=null )
						World.world.remove(x,y,z);
					else if(World.world[x,y,z] is MountainVoxel)
					{
						MountainVoxel v = (MountainVoxel)World.world[x,y,z];
						v.removeTrees();
						World.world.onVoxelUpdated(new Location(x,y,z));
					}
				}
			}
		}



		/// <summary>
		/// Creates the preview image of the land builder.
		/// </summary>
		public override PreviewDrawer createPreview( Size pixelSize ) {
			return new PreviewDrawer( pixelSize, new Size(10,10), 0 );
		}

		public override ModalController createBuilder( IControllerSite site ) {
			return createRemover(site);
		}

	}
}
