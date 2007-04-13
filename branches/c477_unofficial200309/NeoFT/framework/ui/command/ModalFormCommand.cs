using System;
using System.Windows.Forms;
using nft.framework;

namespace nft.ui.command
{
	/// <summary>
	/// 
	/// </summary>
	public class ModalFormCommand : nft.ui.command.ICommandEntity
	{
		const string FormFullName = "System.Windows.Forms.Form";
		readonly Type formClass;

		public ModalFormCommand(Type FormClass)
		{
			formClass = FormClass;
		}

		public void CommandExecuted( CommandUI cmdUI,object sender )
		{
			Form f = Activator.CreateInstance(formClass) as Form;
			f.ShowDialog(Main.mainFrame as IWin32Window);
		}
	}
}
