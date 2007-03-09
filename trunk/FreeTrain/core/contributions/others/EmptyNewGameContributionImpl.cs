using System;
using System.Xml;
using System.Windows.Forms;
using freetrain.framework;
using freetrain.world;

namespace freetrain.contributions.others
{
	/// <summary>
	/// Creates a new empty game by allowing the user to specify the size of the world.
	/// </summary>
	public class EmptyNewGameContributionImpl : NewGameContribution
	{
		public EmptyNewGameContributionImpl( XmlElement e ) : base(e) {}

		public override string author { get { return "-"; } }
		public override string name { get { return "��}�b�v"; } }
		public override string description { get { return "�����Ȃ���̃}�b�v���쐬���܂�"; } }
		
		public override World createNewGame() {
			using( NewWorldDialog dialog = new NewWorldDialog() ) {
				if(dialog.ShowDialog(MainWindow.mainWindow)==DialogResult.OK)
					return dialog.createWorld();
				else
					return null;
			}
		}
	}
}
