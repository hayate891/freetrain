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
        /// <summary>
        /// Main Entry point
        /// </summary>
        /// <param name="args"></param>
		[STAThread]
		static void Main( string[] inArguments ) 
		{
            // Create the agatesetup object passing in args to allow the user to select a driver (for debug purposes only)
            using( ERY.AgateLib.AgateSetup theAgateSetupObject = new ERY.AgateLib.AgateSetup( "FreeTrain", inArguments ) )
            {
                // TODO: InitialzeVideo called by InitializeAll is blowing up currently because it can't
                // find a video assembly to load.  Need to fix this after updating to the next version of agate.
                //setup.InitializeAll();
                theAgateSetupObject.InitializeAudio();
                theAgateSetupObject.InitializeInput();

                // If the user cancels the driver select dialog
                if( theAgateSetupObject.Cancel )
                {
                    // TODO: Generally bad coding practice to have returns/breaks in these situations
                    return;
                }

                #warning Initialize window here already?

                // Create the window
                //ERY.AgateLib.DisplayWindow window = new ERY.AgateLib.DisplayWindow("TreeTrain", 640, 480);

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
