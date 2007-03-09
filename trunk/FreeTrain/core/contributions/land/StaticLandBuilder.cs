using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml;
using org.kohsuke.directdraw;
using freetrain.controllers;
using freetrain.contributions.population;
using freetrain.framework.graphics;
using freetrain.framework.plugin;
using freetrain.views;
using freetrain.world;
using freetrain.world.land;

namespace freetrain.contributions.land
{
	/// <summary>
	/// Places static chip as the land.
	/// </summary>
	[Serializable]
	public class StaticLandBuilder : LandBuilderContribution
	{
		public StaticLandBuilder( XmlElement e ) : base(e) {
			// picture
			XmlElement spr = (XmlElement)XmlUtil.selectSingleNode(e,"sprite");
			sprite = PluginUtil.getSpriteLoader(spr).load2D( spr, 1,1 )[0,0];

			// population
			XmlElement pop = (XmlElement)e.SelectSingleNode("population");
			if(pop!=null)
				population = new PersistentPopulation(
					Population.load(pop),
					new PopulationReferenceImpl(this.id));			}


		/// <summary> Population of this structure, or null if this structure is not populated. </summary>
		public readonly Population population;

		/// <summary> Sprite of this land contribution. </summary>
		public readonly Sprite sprite;


		/// <summary>
		/// Gets the land that should be used to fill (x,y) within [x1,y1]-[x2,y2] (inclusive).
		/// </summary>
		public override void create( int x1, int y1, int x2, int y2, int z ) {
			for( int x=x1; x<=x2; x++ ) {
				for( int y=y1; y<=y2; y++ ) {
					Location loc = new Location(x,y,z);
					if( LandVoxel.canBeBuilt(loc) )
						new StaticLandVoxel( loc, this );
				}
			}
		}



		/// <summary>
		/// Creates the preview image of the land builder.
		/// </summary>
		public override PreviewDrawer createPreview( Size pixelSize ) {
			PreviewDrawer drawer = new PreviewDrawer( pixelSize, new Size(10,10), 0 );

			for( int y=0; y<10; y++ )
				for( int x=0; x<10; x++ )
					drawer.draw( sprite, x, y );

			return drawer;
		}

		public override ModalController createBuilder( IControllerSite site ) {
			return new DefaultControllerImpl(this,site,new DefaultControllerImpl.SpriteBuilder(getSprite));
		}

		private Sprite getSprite() {
			return sprite;
		}



	
		/// <summary>
		/// Used to resolve references to the population object.
		/// </summary>
		[Serializable]
		internal sealed class PopulationReferenceImpl : IObjectReference {
			internal PopulationReferenceImpl( string id ) { this.id=id; }
			private string id;
			public object GetRealObject(StreamingContext context) {
				return ((StaticLandBuilder)PluginManager.theInstance.getContribution(id)).population;
			}
		}
	}
}
