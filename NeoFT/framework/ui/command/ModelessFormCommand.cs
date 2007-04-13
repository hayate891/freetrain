using System;
using System.Windows.Forms;
using nft.framework;

namespace nft.ui.command
{
	/// <summary>
	/// 
	/// </summary>
	public class ModelessFormCommand : nft.ui.command.ICommandEntity
	{
		readonly Type formClass;

		public ModelessFormCommand(Type FormClass)
		{
			formClass = FormClass;
		}

		public void CommandExecuted( CommandUI cmdUI,object sender )
		{
			Form f = Activator.CreateInstance(formClass) as Form;
			f.Show();
		}
	}
}
