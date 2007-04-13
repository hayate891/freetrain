using System;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using nft.framework;
using nft.framework.drawing;
using nft.win32util;

namespace nft.util
{
	/// <summary>
	/// ConfigureService ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class ConfigureService
	{
		private ConfigureService()
		{
		}

		static public string GetSystemInfo()
		{
			if( sysInfoText==null)
				BuildSysInfoText();
			return sysInfoText;
		}

		static private ArrayList list = new ArrayList();
		static public void RegisterAssembly( Assembly asm )
		{
			list.Add(asm);
			BuildSysInfoText();
		}

		#region build system information text
		static private string sysInfoText;
		static private void BuildSysInfoText()
		{
			append("Assembly",Assembly.GetExecutingAssembly().FullName);
			foreach( Assembly asm in list )
				append("Assembly",asm.FullName);
			append("CLR Version",RuntimeEnvironment.GetSystemVersion());
			Assembly mscore = Assembly.GetAssembly(sysInfoText.GetType());			
			appendInline(mscore.GetName().Name+" win32 version",Win32Version.GetAssemblyFileVersion(mscore));
//			using( Form f = new Form())
//			{
//				Assembly forms = Assembly.GetAssembly(f.GetType());
//				append(forms.GetName().Name,Win32Version.GetAssemblyFileVersion(forms));
//			}
			append("Operating System",Environment.OSVersion.ToString());
			GlobalMemoryInfo info = new GlobalMemoryInfo();
			string memtext = string.Format("Physical {0:#,##0,,}MB/{1:#,##0,,}MB,  Virtual {2:#,##0,,}MB/{3:#,##0,,}MB",
					info.PhysicalMemAvailable,info.PhysicalMemTotal,
					info.VirtualMemAvailable,info.VirtualMemTotal);
			append("System Memory",memtext);
			string dxver = RegistryHelper.ReadKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX","Version");
			if(dxver == null)
				dxver = "Unknown";
			append("DirectX Version",dxver);
			try
			{
				IGraphicManager gm = GlobalModules.GraphicManager;
				if(gm!=null){
					append("Video Memory",string.Format("{0:#,##0,,}MB/{1:#,##0,,}MB",
						gm.AvailableVideoMemory,gm.TotalVideoMemory));
					Size scsize = Screen.PrimaryScreen.Bounds.Size;
					string modetext = string.Format("color {0} {1}x{2}",gm.CurrentColorMode,scsize.Width,scsize.Height);
					append("Display Mode",modetext);
				}
			}
			catch(Exception e)
			{
				Debug.WriteLine(e.Message);
				Debug.WriteLine(e.StackTrace);
			}
		}
		static private void append(string name,string val)
		{
			if(sysInfoText!=null)
				sysInfoText += Environment.NewLine;
			sysInfoText += string.Format("{0} : {1}",name,val);
		}
		static private void appendInline(string name,string val)
		{
			sysInfoText += string.Format(", {0} : {1}",name,val);
		}
		#endregion
	}
}
