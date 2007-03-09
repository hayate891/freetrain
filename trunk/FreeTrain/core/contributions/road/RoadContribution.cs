using System;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using freetrain.contributions.common;
using freetrain.framework.graphics;
using freetrain.world;

namespace freetrain.contributions.road
{
	/// <summary>
	/// Road for cars/buses
	/// </summary>
	[Serializable]
	public abstract class RoadContribution : LineContribution
	{
		protected RoadContribution( XmlElement e ) : base("road",e.Attributes["id"].Value) {}


		// roads are always 4-way.
		public override sealed DirectionMode directionMode {
			get {
				return DirectionMode.FourWay;
			}
		}
	}
}
