using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using nft.framework;
using nft.framework.plugin;
using nft.core.game;
using nft.ui.command;
using nft.impl.game;
using nft.ui.mainframe;

namespace nft.debug
{
	/// <summary>
	/// TestGame の概要の説明です。
	/// </summary>
	public class TestGame : ICommandEntity
	{
		static TestGame theInstance;

		static public TestGame GetCommandEntity(string id)
		{
			return theInstance;
		}
		
		static protected IGame theGame
		{
			get{ return GameManager.theInstance.CurrentGame; }
			set{
				if(GameManager.theInstance.CurrentGame!=value)
					GameManager.theInstance.SetGame(value,false); 
			}

		}		

		static TestGame()
		{
			theInstance = new TestGame();
			GameManager.initManager((MainFrame)Main.mainFrame);
			theGame = new GameImpl();
		}
		#region ICommandEntity メンバ

		public void CommandExecuted(CommandUI cmdUI, object sender)
		{
			Debug.WriteLine(cmdUI.ID);
			if(cmdUI.ID.EndsWith("C_TestGameStart"))
				theGame.Start();
			else if(cmdUI.ID.EndsWith("C_TestGameSave"))
			{
				Stream stream = null;
				FileInfo fi = new FileInfo(FileName);
				try
				{
					stream = fi.OpenWrite();
					BinaryFormatter f = new BinaryFormatter();
					f.Serialize(stream,theGame);
				}
				finally
				{
					if(stream!=null)
						stream.Close();
				}
				
			}
			else if(cmdUI.ID.EndsWith("C_TestGameLoad"))
			{
				Stream stream = null;
				FileInfo fi = new FileInfo(FileName);
				try
				{
					stream = fi.OpenRead();
					BinaryFormatter f = new BinaryFormatter();
					f.Binder = PluginSerializationBinder.theInstance;
					theGame = (IGame)f.Deserialize(stream);
				}
				finally
				{
					if(stream!=null)
						stream.Close();
				}
			}
		}

		#endregion
		static private string FileName { get { return Directories.WorkDirPrimary+"test.sav"; } }

	}
}
