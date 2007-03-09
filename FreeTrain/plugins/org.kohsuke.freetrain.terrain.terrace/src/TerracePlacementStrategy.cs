using System;
using System.Drawing;
using freetrain.controllers;
using freetrain.framework;
using freetrain.views;
using freetrain.views.map;

namespace freetrain.world.terrain.terrace
{
	/// <summary>
	/// Terrace placement mode
	/// </summary>
	internal class TerracePlacementStrategy : Strategy {
		public LocationDisambiguator disambiguator { get { return MountainDisambiguator.theInstance; } }

		public void onClick(MapViewWindow view, Location loc, Point ab ) {
			MountainVoxel mv = World.world[loc] as MountainVoxel;
			
			if(mv==null) {
				MainWindow.showError("éRîßÇ…ÇµÇ©ê›íuÇ≈Ç´Ç‹ÇπÇÒ");
				return;
			}

			TerraceVoxel.create(mv);
		}

		public void drawVoxel( QuarterViewDrawer view, DrawContextEx dc, Location loc, Point pt ) {
			if( World.world[loc] is MountainVoxel ) {
				ResourceUtil.emptyChip.drawAlpha( dc.surface, new Point(pt.X,pt.Y-16) );
				TerraceVoxel.image.drawAlpha( dc.surface, pt );
			}
		}
	}
}
