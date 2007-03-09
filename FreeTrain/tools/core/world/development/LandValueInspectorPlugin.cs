using System;
using System.Windows.Forms;
using System.Xml;
using freetrain.contributions.others;
using freetrain.framework.plugin;

namespace freetrain.world.development
{
	public class LandValueInspectorPlugin : MenuContribution
	{
		public LandValueInspectorPlugin( XmlElement e ) : base(e) {}

		public override void mergeMenu( MainMenu containerMenu ) {
			MenuItem item = new MenuItem();
			item.Text = "�n���̌���";
			item.Click += new System.EventHandler(onClick);

			containerMenu.MenuItems[1].MenuItems.Add(item);
		}

		private void onClick( object sender, EventArgs args ) {
			new LandValueInspector();
		}
	}
}
