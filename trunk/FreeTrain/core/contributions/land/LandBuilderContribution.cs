using System;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using org.kohsuke.directdraw;
using freetrain.world;
using freetrain.world.terrain;
using freetrain.contributions.common;
using freetrain.controllers;
using freetrain.framework;
using freetrain.framework.graphics;
using freetrain.framework.plugin;

namespace freetrain.contributions.land
{
	/// <summary>
	/// Plug-in that places land voxels.
	/// 
	/// This contribution allows the tiling algorithm to be customized.
	/// </summary>
	[Serializable]
	public abstract class LandBuilderContribution : EntityBuilderContribution
	{
		protected LandBuilderContribution( XmlElement e ) : base(e) {
			XmlNode nameNode = e.SelectSingleNode("name");
			XmlNode groupNode = e.SelectSingleNode("group");

			_name = (nameNode!=null)? nameNode.InnerText : (groupNode!=null ? groupNode.InnerText : null );
			if(_name==null)
				throw new FormatException("<name> and <group> are both missing");

			string groupName = (groupNode!=null)? groupNode.InnerText : _name;
			PluginManager.theInstance.landBuilderGroup[groupName].add(this);

			price = int.Parse( XmlUtil.selectSingleNode( e, "price" ).InnerText );
		}

		private readonly string _name;

		public override string name { get { return _name; } }

		/// <summary> Price of the land per voxel. </summary>
		public readonly int price;

		public abstract void create( int x1, int y1, int x2, int y2, int z );

		/// <summary>
		/// Fills the specified region with lands.
		/// </summary>
		public void create( Location loc1, Location loc2 ) {
			Debug.Assert( loc1.z==loc2.z );
			int z = loc1.z;

			int minx = Math.Min( loc1.x, loc2.x );
			int maxx = Math.Max( loc1.x, loc2.x );
			
			int miny = Math.Min( loc1.y, loc2.y );
			int maxy = Math.Max( loc1.y, loc2.y );

			create( minx, miny, maxx, maxy, z );
		}

		/// <summary> Creates a single patch. </summary>
		public void create( Location loc ) {
			create( loc, loc );
		}

		public override ModalController createRemover( IControllerSite site ) {
			return new DefaultControllerImpl(this,site,
				new DefaultControllerImpl.SpriteBuilder(getSprite));
		}

		private static Sprite getSprite() {
			return ResourceUtil.emptyChip;
		}
	}
}
