using System;
using nft.ui.command;

namespace nft.debug
{
	/// <summary>
	/// TestErrorMessage の概要の説明です。
	/// </summary>
	public class TestErrorMessage : ICommandEntity
	{
		public TestErrorMessage()
		{			
		}
		
		public void CommandExecuted( CommandUI cmdUI,object sender )
		{			
			throw new Exception("これはテストで発生させた例外です。");
		}		
	}
}
