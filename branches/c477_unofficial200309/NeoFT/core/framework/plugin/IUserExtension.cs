using System;

namespace nft.framework.plugin
{
	public enum ModuleState{ Uninitialized, Ready, PartialError, FatalError };
	/// <summary>
	/// IUserExtension ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public interface IUserExtension
	{
		ModuleState state{ get; }
		bool UserAvailable{ get; set; }
		bool ComAvailable{ get; set; }
	}
}
