using System;
using System.Reflection;
using nft.framework;
using nft.util;

namespace nft.core
{
	/// <summary>
	/// Core ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public class Core
	{
		/// <summary> Main resources. </summary>
		public static readonly Properties resources = Properties.LoadFromFile(Directories.AppBaseDir+"nftcore.resource.xml",false);

		static Core()
		{
			ConfigureService.RegisterAssembly(Assembly.GetAssembly(Type.GetType("nft.core.Core")));
		}
	}
}
