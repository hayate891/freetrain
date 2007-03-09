using System;
using System.Xml;
using freetrain.framework.plugin;
using freetrain.contributions.structs;

namespace freetrain.world.soccerstadium
{
	[Serializable]
	public class ContributionImpl : SpecialStructureContribution
	{
		public ContributionImpl( XmlElement e ) : base(e) {
		}

		public override string name { get { return "�T�b�J�[�X�^�W�A��"; } }

		public override string oneLineDescription {
			get {
				return "�T�b�J�[�X�^�W�A�������݂��ă`�[�����o�c���܂�";
			}
		}

		public override void showDialog() {
			PlacementController.create();
		}



	}
}
