using System;

namespace nft.win32util
{
	/// <summary>
	/// GenericUtil �̊T�v�̐����ł��B
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
