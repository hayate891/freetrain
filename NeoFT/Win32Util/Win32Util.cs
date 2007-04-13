using System;

namespace nft.win32util
{
	/// <summary>
	/// GenericUtil ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public class Win32Util
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")] 
		private static extern int MessageBeep(uint n); 

		public static void MessageBeep()
		{
			MessageBeep(0);
		}
	}
}
