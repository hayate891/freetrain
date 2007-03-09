using System;
using System.Xml;
using freetrain.contributions.common;
using freetrain.framework.plugin;

namespace freetrain.contributions.rail
{
	/// <summary>
	/// Contribution that adds <c>TrafficVoxel.Accessory</c>
	/// </summary>
	[Serializable]
	public abstract class RailAccessoryContribution : EntityBuilderContribution
	{
		private readonly string _name;

		public RailAccessoryContribution( XmlElement e ) : base(e) {
			_name = XmlUtil.selectSingleNode(e,"name").InnerText;
		}

		public override string name { get { return _name; } }

		// TODO: do we need a method like
		// void create( Location loc ) ?
	}
}
