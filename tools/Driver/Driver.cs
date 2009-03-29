using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using freetrain.framework;

namespace Driver
{
	public class Driver
	{
#if windows
		[DllImport("DirectDraw.AlphaBlend.dll",EntryPoint="DllRegisterServer")]
		private static extern void regsvr();
#else
#warning STUB
#endif
		
		[STAThread]
		static void Main( string[] args ) 
		{
#if windows
#else
			using(ERY.AgateLib.AgateSetup setup = new ERY.AgateLib.AgateSetup("FreeTrain", args))
			{
				// TODO: InitialzeVideo called by InitializeAll is blowing up currently because it can't
                // find a video assembly to load.  Need to fix this after updating to the next version of agate.
                //setup.InitializeAll();
                setup.InitializeAudio();
                setup.InitializeInput();
                
				if (setup.Cancel)
					return;
#warning Initialize window here already?
//				ERY.AgateLib.DisplayWindow window = new ERY.AgateLib.DisplayWindow("TreeTrain", 640, 480);
#endif
			// record the installation directory
			Core.installationDirectory =
				Directory.GetParent(Application.ExecutablePath).FullName;
			
			if( Debugger.IsAttached )
				run(args);
			else
				try {
					run(args);
				} catch( Exception e ) {
					ErrorMessageBox.show(null,"An error has occurred",e);
					//! ErrorMessageBox.show(null,"エラーが発生しました",e);
				}
#if windows
#else
			}
#endif
		}

		private static void run( string[] args ) {
#if windows
			// register alpha blending DLL
			regsvr();
#else
#warning STUB
#endif

			// start the game
			Application.Run(new MainWindow(args,false));
		}
	}
}
