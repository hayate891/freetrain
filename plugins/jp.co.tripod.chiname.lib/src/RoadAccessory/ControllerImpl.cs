using System;
using System.Drawing;
using System.Windows.Forms;
using freetrain.framework;
using freetrain.views;
using freetrain.views.map;
using freetrain.controllers;

namespace freetrain.world.road.accessory
{
	/// <summary>
	/// ModalController implementation for road accessory contribution
	/// </summary>
    [CLSCompliant(false)]
    public class ControllerImpl : PointSelectorController
	{
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected readonly RoadAccessoryContribution contribution;
        /// <summary>
        /// 
        /// 
        /// </summary>
		protected readonly bool remove;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_contrib"></param>
        /// <param name="_site"></param>
        /// <param name="_remover"></param>
        [CLSCompliant(false)]
        public ControllerImpl(RoadAccessoryContribution _contrib, IControllerSite _site, bool _remover)
            : base(_site)
        {
			this.contribution = _contrib;
			this.remove = _remover;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        [CLSCompliant(false)]
        protected override void onLocationSelected(Location loc)
        {
			if( remove )
			{
				if( contribution.canBeBuilt(loc))
				{
					if( TrafficVoxel.get(loc).accessory != null )
						TrafficVoxel.get(loc).accessory = null;
				}
				else
					MainWindow.showError("Can not remove");
					//! MainWindow.showError("撤去できません");
			}
			else
			{
				if( contribution.canBeBuilt(loc) )
					contribution.create(loc);
				else
					MainWindow.showError("Can not place");
					
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        /// <param name="loc"></param>
        /// <param name="pt"></param>
        [CLSCompliant(false)]
        public override void drawVoxel(QuarterViewDrawer view, DrawContextEx canvas, Location loc, Point pt)
        {
			if( base.currentPos!=loc )		return;
			if( !contribution.canBeBuilt(loc) )	return;
			
			int x;
			RoadPattern rp = TrafficVoxel.get(loc).road.pattern;
			if( rp.hasRoad(Direction.NORTH) )	x=1;
			else								x=0;

			contribution.sprites[x,0].drawAlpha( canvas.surface, pt );
			contribution.sprites[x,1].drawAlpha( canvas.surface, pt );
		}

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public override LocationDisambiguator disambiguator
        {
			get {
				return RoadDisambiguator.theInstance;
			}
		}
	}

    /// <summary>
    /// 
    /// </summary>
	public class RoadDisambiguator : LocationDisambiguator
	{
		// the singleton instance
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public static LocationDisambiguator theInstance = new RoadDisambiguator();
		private RoadDisambiguator() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public bool isSelectable(Location loc) 
		{
			// if there's any rail roads, fine
			if( Road.get(loc)!=null )	return true;

			// or if we hit the ground
			if( World.world.getGroundLevel(loc)>=loc.z )	return true;

			return false;
		}
	}
}
