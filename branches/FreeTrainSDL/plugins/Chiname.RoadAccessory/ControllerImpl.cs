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
    public class ControllerImpl : PointSelectorController
	{
        /// <summary>
        /// 
        /// </summary>
        private readonly RoadAccessoryContribution contribution;

        /// <summary>
        /// 
        /// </summary>
        protected RoadAccessoryContribution Contribution
        {
            get { return contribution; }
        } 

        /// <summary>
        /// 
        /// 
        /// </summary>
        private readonly bool remove;

        /// <summary>
        /// 
        /// </summary>
        protected bool Remove
        {
            get { return remove; }
        } 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="contrib"></param>
        /// <param name="site"></param>
        /// <param name="remover"></param>
        public ControllerImpl(RoadAccessoryContribution contrib, IControllerSite site, bool remover)
            : base(site)
        {
			this.contribution = contrib;
			this.remove = remover;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        protected override void OnLocationSelected(Location loc)
        {
			if( remove )
			{
				if( contribution.CanBeBuilt(loc))
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
				if( contribution.CanBeBuilt(loc) )
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
        public override void DrawVoxel(QuarterViewDrawer view, DrawContext canvas, Location loc, Point pt)
        {
			if( base.currentPos!=loc )		return;
			if( !contribution.CanBeBuilt(loc) )	return;
			
			int x;
			RoadPattern rp = TrafficVoxel.get(loc).road.pattern;
			if( rp.hasRoad(Direction.NORTH) )	x=1;
			else								x=0;

			contribution.sprites[x,0].DrawAlpha( canvas.Surface, pt );
			contribution.sprites[x,1].DrawAlpha( canvas.Surface, pt );
		}

        /// <summary>
        /// 
        /// </summary>
        public override ILocationDisambiguator Disambiguator
        {
			get {
				return RoadDisambiguator.theInstance;
			}
		}
	}
}
