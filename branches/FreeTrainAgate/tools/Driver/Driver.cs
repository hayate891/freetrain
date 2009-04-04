using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using freetrain.framework;
using AgateLib.DisplayLib;

namespace Driver
{
	public class Driver
	{		
        /// <summary>
        /// Main Entry point
        /// </summary>
        /// <param name="args"></param>
		[STAThread]
		static void Main( string[] inArguments ) 
		{
            // Create the agatesetup object passing in args to allow the user to select a driver (for debug purposes only)
            using( AgateLib.AgateSetup theAgateSetupObject = new AgateLib.AgateSetup( "FreeTrain", inArguments ) )
            {
                // Scans the directory where the .exe is and loads Display, Audio, Input, and Desktop drivers.
                // Display driver: AdgateDrawning.
                // TODO: AdgateDrawing is for debug builds only. DO NOT DISTRIBUTE WITH RELEASE BUILDS. 
                // Input and Audio driver: AdgateSDL
                // Desktop: Adgate.WinForms <-- Not sure if we want to use this yet...
                theAgateSetupObject.InitializeAll();

                // If the user cancels the driver select dialog
                if( theAgateSetupObject.WasCanceled )
                {
                    // TODO: Generally bad coding practice to have returns/breaks in these situations
                    return;
                }

                // record the installation directory
                Core.installationDirectory = Directory.GetParent( Application.ExecutablePath ).FullName;

                // When exceptions are thrown let the IDE take us to it
                // TODO: Should be removed when all is said and done
                if( Debugger.IsAttached )
                {
                    run( inArguments );
                }
                else
                {
                    try
                    {
                        run( inArguments );
                    }
                    catch( Exception inException )
                    {
                        ErrorMessageBox.show( null, "An error has occurred", inException );
                        //! ErrorMessageBox.show(null,"エラーが発生しました",e);
                    }
                }
            }
		}

        /// <summary>
        /// Responsible for running the application.
        /// </summary>
        /// <param name="args"></param>
		private static void run( string[] inArguments ) 
        {
            #warning STUB
			// start the game
			Application.Run( new MainWindow(inArguments, false) );
		}
	}
}
