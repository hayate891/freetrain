using System;
using nft.ui.command;

namespace nft.debug
{
	/// <summary>
	/// TestErrorMessage �̊T�v�̐����ł��B
	/// </summary>
	public class TestErrorMessage : ICommandEntity
	{
		public TestErrorMessage()
		{			
		}
		
		public void CommandExecuted( CommandUI cmdUI,object sender )
		{			
			throw new Exception("����̓e�X�g�Ŕ�����������O�ł��B");
		}		
	}
}
