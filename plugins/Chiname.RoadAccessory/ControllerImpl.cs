using System;
using System.Drawing;
using System.Windows.Forms;
using FreeTrain.Framework;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.Controllers;

namespace FreeTrain.World.Road.Accessory
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
					MessageBox.Show("Cannot remove");
					//! MessageBox.Show("撤去できません");
			}
			else
			{
				if( contribution.canBeBuilt(loc) )
					contribution.create(loc);
				else
					MessageBox.Show("Cannot place");
					
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
        public override void DrawVoxel(QuarterViewDrawer view, DrawContext canvas, Location loc, Point pt)
        {
			if( base.currentPos!=loc )		return;
			if( !contribution.canBeBuilt(loc) )	return;
			
			int x;
			RoadPattern rp = TrafficVoxel.get(loc).road.pattern;
			if( rp.hasRoad(Direction.NORTH) )	x=1;
			else								x=0;

			contribution.sprites[x,0].drawAlpha( canvas.Surface, pt );
			contribution.sprites[x,1].drawAlpha( canvas.Surface, pt );
		}

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public override ILocationDisambiguator Disambiguator
        {
			get {
				return RoadDisambiguator.theInstance;
			}
		}
	}

    /// <summary>
    /// 
    /// </summary>
	public class RoadDisambiguator : ILocationDisambiguator
	{
		// the singleton instance
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public static ILocationDisambiguator theInstance = new RoadDisambiguator();
		private RoadDisambiguator() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public bool IsSelectable(Location loc) 
		{
			// if there's any rail roads, fine
			if( BaseRoad.get(loc)!=null )	return true;

			// or if we hit the ground
			if( WorldDefinition.World.GetGroundLevel(loc)>=loc.z )	return true;

			return false;
		}
	}
}
