using System;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using freetrain.util;
using freetrain.controllers;
using freetrain.contributions.common;
using freetrain.framework.graphics;
using freetrain.framework.plugin;
using freetrain.world;
using freetrain.world.structs;
using org.kohsuke.directdraw;

namespace freetrain.contributions.structs
{
	/// <summary>
	/// Building of a variable height.
	/// </summary>
	[Serializable]
	public class VarHeightBuildingContribution : StructureContribution
	{
		public VarHeightBuildingContribution( XmlElement e ) : base(e) {
			price = int.Parse( XmlUtil.selectSingleNode(e,"price").InnerText );
			
			size = XmlUtil.parseSize( XmlUtil.selectSingleNode(e,"size").InnerText );

			minHeight = int.Parse( XmlUtil.selectSingleNode(e,"minHeight").InnerText );
			maxHeight = int.Parse( XmlUtil.selectSingleNode(e,"maxHeight").InnerText );

			XmlElement pics = (XmlElement)XmlUtil.selectSingleNode(e,"pictures");

			tops    = loadSpriteSets( pics.SelectNodes("top"   ) );
			bottoms = loadSpriteSets( pics.SelectNodes("bottom") );

			XmlElement m = (XmlElement)XmlUtil.selectSingleNode(pics,"middle");
			middle = PluginUtil.getSpriteLoader(m).load2D( m, size );
		}

		protected override StructureGroup getGroup( string name ) {
			return PluginManager.theInstance.varHeightBuildingsGroup[name];
		}

		private Sprite[][,] loadSpriteSets( XmlNodeList list ) {
			Sprite[][,] sprites = new Sprite[list.Count][,];

			int idx=0;
			foreach( XmlElement e in list )
				sprites[idx++] = PluginUtil.getSpriteLoader(e).load2D( e, size );
			
			return sprites;
		}

		/// <summary>Price of this structure per height.</summary>
		public readonly int price;

		/// <summary>Sprite sets.</summary>
		private readonly Sprite[][,] tops,bottoms;
		private readonly Sprite[,]   middle;

		/// <summary> Sprite to draw the structure </summary>
		public Sprite getSprite( int x, int y, int z, int height ) {
			if( z>=height-tops.Length )
				return tops[height-z-1][x,y];
			if( z<bottoms.Length )
				return bottoms[z][x,y];

			return middle[x,y];
		}

		/// <summary> Size of the basement of this structure in voxel by voxel. </summary>
		public readonly SIZE size;

		/// <summary> Range of the possible height of the structure in voxel unit. </summary>
		public readonly int minHeight,maxHeight;



		/// <summary>
		/// Creates a new instance of this structure type to the specified location.
		/// </summary>
		public Structure create( Location baseLoc, int height, bool initiallyOwned ) {
			Debug.Assert( canBeBuilt(baseLoc,height) );

			return new VarHeightBuilding(this,baseLoc,height,initiallyOwned);
		}

		/// <summary>
		/// Returns true iff this structure can be built at the specified location.
		/// </summary>
		public bool canBeBuilt( Location baseLoc, int height ) {
			for( int z=0; z<height; z++ )
				for( int y=0; y<size.y; y++ )
					for( int x=0; x<size.x; x++ )
						if( World.world[ baseLoc.x+x, baseLoc.y+y, baseLoc.z+z ]!=null )
							return false;

			return true;
		}

		public override PreviewDrawer createPreview( Size pixelSize ) {
			PreviewDrawer drawer = new PreviewDrawer( pixelSize, size, tops.Length+bottoms.Length+1/*middle*/ );

			int z=0;
			for( int i=bottoms.Length-1; i>=0; i-- )
				drawer.draw( bottoms[i], 0, 0, z++ );
			drawer.draw( middle, 0,0, z++ );
			for( int i=tops.Length-1; i>=0; i-- )
				drawer.draw( tops[i], 0, 0, z++ );

			return drawer;
		}

		public override ModalController createBuilder( IControllerSite site ) {
			// TODO
			throw new NotImplementedException();
		}
		public override ModalController createRemover( IControllerSite site ) {
			// TODO
			throw new NotImplementedException();
		}

	}
}
