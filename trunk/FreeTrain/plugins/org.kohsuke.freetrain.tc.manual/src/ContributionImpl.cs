using System;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using freetrain.contributions.rail;
using freetrain.framework.plugin;

namespace freetrain.world.rail.manualtc
{
	/// <summary>
	/// Contribution implementation
	/// </summary>
	[Serializable]
	public class TrainConrollerContributionImpl : TrainControllerContribution
	{
		public TrainConrollerContributionImpl( XmlElement e ) : base(e) {}


		public override string name { get { return "�蓮�^�]�_�C���O����"; } }

		public override string description {
			get {
				return "�L�[�{�[�h�����Ԃ��蓮�ŉ^�]�ł���悤�ɂ��܂�";
			}
		}

		public override TrainController newController( string name ) {
			return new ManualTrainController(name,this);
		}
	}
}
