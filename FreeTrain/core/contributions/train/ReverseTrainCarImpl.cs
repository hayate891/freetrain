using System;
using System.Drawing;
using System.Xml;
using org.kohsuke.directdraw;
using freetrain.world;
using freetrain.framework;
using freetrain.framework.plugin;

namespace freetrain.contributions.train
{
	/// <summary>
	/// TrainCarContribution that draws cars from another TrainCarContribution
	/// in an opposite direction.
	/// 
	/// Intended to be used to realize the last car.
	/// </summary>
	[Serializable]
	public class ReverseTrainCarImpl : TrainCarContribution
	{
		public ReverseTrainCarImpl( XmlElement e ) : base(e.Attributes["id"].Value,0) {

			XmlElement b= (XmlElement)XmlUtil.selectSingleNode(e,"base");

			baseId = b.Attributes["carRef"].Value;
		}

		private string baseId;

		protected internal override void onInitComplete() {
			core = Core.plugins.getContribution(baseId) as TrainCarContribution;
			if(core==null)
				throw new FormatException("'"+id+"'���Q�Ƃ���TrainCar�R���g���r���[�V����'"+baseId+"'��������܂���");
			this._capacity = core.capacity;
		}


		private TrainCarContribution core;


		public override void draw( Surface display, Point pt, int angle ) {
			angle ^= 8;
			core.draw(display,pt,angle);
		}

		public override void drawSlope( Surface display, Point pt, Direction angle, bool isClimbing ) {
			angle = angle.opposite;
			isClimbing = !isClimbing;

			core.drawSlope( display, pt, angle, isClimbing );
		}

	}
}
