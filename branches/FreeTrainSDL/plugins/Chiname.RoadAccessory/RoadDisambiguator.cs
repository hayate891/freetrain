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
    /// 
    /// </summary>
	public class RoadDisambiguator : ILocationDisambiguator
	{
		// the singleton instance
        /// <summary>
        /// 
        /// </summary>
        public static ILocationDisambiguator theInstance = new RoadDisambiguator();
		private RoadDisambiguator() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
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
