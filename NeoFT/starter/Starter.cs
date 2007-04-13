using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using nft.util;
using nft.ui.mainframe;

namespace Starter
{
	public class Starter
	{
		//[DllImport("DirectDraw.AlphaBlend.dll",EntryPoint="DllRegisterServer")]
		//private static extern void regsvr();

		[STAThread]
		static void Main( string[] args ) 
		{
			if( Debugger.IsAttached )
				run(args);
			else
				try {
					run(args);
				} catch( Exception e ) {
					UIUtil.ShowException("ÉGÉâÅ[Ç™î≠ê∂ÇµÇ‹ÇµÇΩ",e,UIInformLevel.normal);
				}
		}

		private static void run( string[] args ) {
			// register alpha blending DLL
			//regsvr();

			// start the game
			Application.Run(new MainFrame(args));
		}
	}
}
