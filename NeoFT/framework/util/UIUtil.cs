using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using nft.framework;
using nft.win32util;

namespace nft.util
{
	public enum UIMessageType{ normal, info, warning, alert, question, hint }
	public enum UIQueryType{ ok_cancel, yes_no }
	public enum UIInformLevel{ minor, normal, severe }

	/// <summary>
	/// utilities about display message, can get simple answer.
	/// </summary>
	public class UIUtil
	{
		static private IMessageUIHandler handler = new SilentHandler();
		static private ImageList msgIcons = null;

		private UIUtil()
		{
		}

		static public ImageList MsgTypeIcons 
		{
			get
			{
				if(msgIcons==null)
				{
					msgIcons = new ImageList();
					msgIcons.ImageSize = new Size(16,16);
					msgIcons.Images.Add( Image.FromFile(Directories.SystemResourceDir+"null.bmp"));
					msgIcons.Images.Add( Image.FromFile(Directories.SystemResourceDir+"info.ico"));
					msgIcons.Images.Add( Image.FromFile(Directories.SystemResourceDir+"warning.ico"));
					msgIcons.Images.Add( Image.FromFile(Directories.SystemResourceDir+"error.ico"));
					msgIcons.Images.Add( Image.FromFile(Directories.SystemResourceDir+"question.ico"));
					msgIcons.Images.Add( Image.FromFile(Directories.SystemResourceDir+"hint.ico"));
				}
				return msgIcons;
			}
		}

		static public void OpenURLByBrowser(string url)
		{
			// Because shell execuse doesn't work
			// we have to specify executing module directory
			ProcessStartInfo info = new ProcessStartInfo();
			// get default browser (exe) path
			String val = RegistryHelper.ReadKey(@"HKEY_CLASSES_ROOT\http\shell\open\command","");
			if(val.StartsWith("\""))
			{
				int n = val.IndexOf("\"",1);
				info.FileName = val.Substring(1,n-1);
				info.Arguments = val.Substring(n+1);
			}
			else
			{
				string[] a = val.Split(new char[]{' '});
				info.FileName = a[0];
				info.Arguments = val.Substring(a[0].Length+1);
			}
			// Specifies working directory is necessary to run browser successfuly.
			info.WorkingDirectory = Path.GetDirectoryName(info.FileName);
			
			info.Arguments += url;
			Debug.WriteLine(info.FileName,"OpenBrowser");
			Debug.WriteLine(info.WorkingDirectory,"OpenBrowser");
			Debug.WriteLine(info.Arguments,"OpenBrowser");
			Process.Start(info);
		}

		static public void Message(string text,UIMessageType type,UIInformLevel level)
		{
			handler.ShowMessage(text,type,level);
		}
		static public bool YesNoMessage(string text)
		{
			return handler.ShowQueryMessage(text,UIQueryType.yes_no,UIMessageType.question,UIInformLevel.normal);			
		}
		static public bool ConfirmMessage(string text,UIMessageType type,UIInformLevel level)
		{
			return handler.ShowQueryMessage(text,UIQueryType.ok_cancel,UIMessageType.question,UIInformLevel.normal);			
		}
		static public void ShowException(string text,Exception caused,UIInformLevel level)
		{
			handler.ShowException(text,caused,level);
		}

		static public void SetHandler(IMessageUIHandler new_handler)
		{
			if(handler!=null)
				handler.Release();
			if(new_handler==null)
				handler = new SilentHandler();
			else
				handler = new_handler;
		}

		private class SilentHandler : IMessageUIHandler
		{
			public void ShowMessage(string text,UIMessageType type,UIInformLevel level)
			{
				Debug.WriteLine(text);
			}
			public void ShowException(string text,Exception caused,UIInformLevel level)
			{
				ShowErrorMessageBox(null,text,caused);
				Debug.WriteLine(text);
				Debug.WriteLine(caused.StackTrace);
			}
			public bool ShowQueryMessage(string text,UIQueryType query,UIMessageType type,UIInformLevel level)
			{
				return MessageBox.Show(text,"NeoFreeTrain",MessageBoxButtons.YesNo)==DialogResult.Yes;
			}
			public void Release()
			{
			}
		}

		public static void ShowErrorMessageBox(IWin32Window owner, string msg, Exception e )
		{
			Assembly asm = Assembly.LoadFrom(Directories.AppBaseDir+"NFT.UI.DLL");
			Type t = asm.GetType("nft.ui.system.ErrorMessageBox");
			t.InvokeMember("Show",BindingFlags.Public|BindingFlags.Static|BindingFlags.InvokeMethod,null,null,new object[]{owner,msg,e});
		}
	}

	public interface IMessageUIHandler
	{
		void ShowMessage(string text,UIMessageType type,UIInformLevel level);
		void ShowException(string text,Exception caused,UIInformLevel level);
		bool ShowQueryMessage(string text,UIQueryType query,UIMessageType type,UIInformLevel level);
		void Release();
	}
}
