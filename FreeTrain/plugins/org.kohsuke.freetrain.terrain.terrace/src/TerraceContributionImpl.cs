using System;
using System.Xml;
using freetrain.contributions.structs;
using freetrain.framework.plugin;

namespace freetrain.world.terrain.terrace
{
	/// <summary>
	/// Contribution implementation
	/// </summary>
	[Serializable]
	public class TerraceContributionImpl : SpecialStructureContribution
	{
		public TerraceContributionImpl( XmlElement e ) : base(e) {
			theInstance = this;
		}

		internal static TerraceContributionImpl theInstance;

		public override string name { get { return "���d�Ɛ؂�ʂ�"; } }

		public override string oneLineDescription {
			get {
				return "�R���ɕ��n�𐷂�グ���������肵�ĕ��n��؂�J���܂�";
			}
		}

		public override void showDialog() {
			TerraceController.create();
		}
	}
}
