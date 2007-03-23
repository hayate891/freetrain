using System;
using System.Xml;
using freetrain.framework;
using freetrain.framework.plugin;

namespace freetrain.contributions.train
{
	/// <summary>
	/// Parameterized train contribution implementation
	/// where an user can specify (a) head, (b) tail, and (c) other intermediate
	/// cars separately.
	/// </summary>
	[Serializable]
	public class ParamTrainImpl : AbstractTrainContributionImpl
	{
		/// <summary>
		/// Parses a train contribution from a DOM node.
		/// </summary>
		/// <exception cref="XmlException">If the parsing fails</exception>
		public ParamTrainImpl( XmlElement e ) : base(e) {
			composition = (XmlElement)XmlUtil.selectSingleNode(e,"composition");
		}

		/// <summary>
		/// &lt;composition> element in the plug-in xml file.
		/// </summary>
		private XmlElement composition;

		protected internal override void onInitComplete() {
			base.onInitComplete();

			carHeadType = getCarType(composition,"head");
			carBodyType = getCarType(composition,"body");
			carTailType = getCarType(composition,"tail");

			if(carBodyType==null)
				throw new FormatException("<body>Part was not specified");
				//! throw new FormatException("<body>要素が指定されませんでした");

			composition = null;
		}

		private TrainCarContribution getCarType( XmlElement comp, string name ) {
			XmlElement e = (XmlElement)comp.SelectSingleNode(name);
			if(e==null)		return null;

			string idref = e.Attributes["carRef"].Value;
			if(id==null)	throw new FormatException("carReflacks attribute");
			//! if(id==null)	throw new FormatException("carRef属性がありません");

			TrainCarContribution contrib = (TrainCarContribution)Core.plugins.getContribution(idref);
			if(contrib==null)	throw new FormatException(
				string.Format( "id='{0}' lacks TrainCar contribution", idref ));
				//! string.Format( "id='{0}'のTrainCarコントリビューションがありません", idref ));

			return contrib;
		}

		private TrainCarContribution carHeadType,carBodyType,carTailType;

		public override TrainCarContribution[] create( int length ) {
			TrainCarContribution[] r = new TrainCarContribution[length];
			for( int i=0; i<r.Length; i++ )
				r[i] = carBodyType;

			if( carHeadType!=null )
				r[0] = carHeadType;

			if( carTailType!=null )
				r[r.Length-1] = carTailType;

			return r;
		}

	}
}
