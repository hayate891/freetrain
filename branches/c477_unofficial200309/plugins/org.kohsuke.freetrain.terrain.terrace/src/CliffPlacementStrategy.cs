using System;
using System.Drawing;
using freetrain.framework;
using freetrain.controllers;
using freetrain.views;
using freetrain.views.map;

namespace freetrain.world.terrain.terrace
{
	/// <summary>
	/// Cliff indices mode
	/// </summary>
	internal class CliffPlacementStrategy : Strategy {
		public LocationDisambiguator disambiguator { get { return MountainDisambiguator.theInstance; } }

		public void onClick(MapViewWindow view, Location loc, Point ab ) {
			MountainVoxel mv = World.world[loc] as MountainVoxel;
			
			if(mv==null) {
				MainWindow.showError("�R�������H���ł��܂���");
				return;
			}

			int h = World.world.getGroundLevel(loc);

			for( int i=0; i<4; i++ ) {
				if( World.world.getGroundLevel(loc+Direction.get(i*2)) > h ) {
					MainWindow.showError("�ݒu�ʒu���s�K�؂ł�");
					return;
				}
			}
			
			World.world.remove(mv);
		}

		public void drawVoxel( QuarterViewDrawer view, DrawContextEx dc, Location loc, Point pt ) {
			if( World.world[loc] is MountainVoxel ) {
				ResourceUtil.emptyChip.drawShape( dc.surface, pt, Color.Red );
			}
		}
	}
}
