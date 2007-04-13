using System;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;
using freetrain.contributions.others;
using freetrain.framework;
using freetrain.framework.plugin;
using freetrain.world;

namespace freetrain.tools.terrainloader
{
	public class PluginImpl : NewGameContribution
	{
		public PluginImpl( XmlElement e ) : base(e) {}

		public override string author { get { return "-"; } }
		public override string name { get { return "�n�`���[�_�["; } }
		public override string description { get { return "�摜����n�`�f�[�^�݂̂����[�h���܂�"; } }

		public override World createNewGame() {
			using(LoadDialog dialog = new LoadDialog()) {
				if(dialog.ShowDialog(MainWindow.mainWindow)==DialogResult.OK)
					return dialog.createWorld();
				else
					return null;
			}
		}
	}
}
