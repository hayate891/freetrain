using System;

namespace nft.framework.plugin
{
	public enum ModuleState{ Uninitialized, Ready, PartialError, FatalError };
	/// <summary>
	/// IUserExtension の概要の説明です。
	/// </summary>
	public interface IUserExtension
	{
		ModuleState state{ get; }
		bool UserAvailable{ get; set; }
		bool ComAvailable{ get; set; }
	}
}
