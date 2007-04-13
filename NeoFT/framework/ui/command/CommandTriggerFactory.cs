using System;
using System.Windows.Forms;
using UtilityLibrary.CommandBars;

namespace nft.ui.command
{
	/// <summary>
	/// CommandTriggerFactory ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class CommandTriggerFactory
	{
		public static ICommandTrigger Create( MenuItem item )
		{ return new MenuItemTrigger(item); }
		public static ICommandTrigger Create( Button item )
		{ return new ButtonTrigger(item); }
		public static ICommandTrigger Create( CheckBox item )
		{ return new CheckBoxTrigger(item); }
		public static ICommandTrigger Create( ToolBarItem item )
		{ return new ToolBarItemTrigger(item); }
		public static ICommandTrigger Create( ToolBarButton item )
		{ return new ToolBarButtonTrigger(item); }
		public static ICommandTrigger Create( Control item )
		{ return new ControlTrigger(item); }
	}
}
