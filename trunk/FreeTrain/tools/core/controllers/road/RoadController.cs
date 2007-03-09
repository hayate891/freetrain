using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.contributions.road;
using freetrain.framework;
using freetrain.framework.plugin;
using freetrain.views.map;
using freetrain.world;
using freetrain.world.road;

namespace freetrain.controllers.road
{
	/// <summary>
	/// Controller to place/remove roads
	/// </summary>
	public class RoadController : AbstractLineController
	{
		public RoadController( RoadContribution type ) : base(type) {}
		
		protected override void draw( Direction d, DrawContextEx canvas, Point pt ) {
			ResourceUtil.emptyChip.drawShape( canvas.surface, pt, Color.Blue );
//			RoadPattern.getStraight(d).drawAlpha( canvas.surface, pt );
		}
	}
}
