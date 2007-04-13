using System;

namespace nft.framework.plugin
{
	public enum InstallationState{ Uninitialized, FatalError, PartialError, Ready };
	public delegate void AttachChangeEvent( IAddable sender );
	/// <summary>
	/// Addable interface (implemented by Plugins and Contributions)
	/// </summary>
	public interface IAddable
	{
		InstallationState State{ get; }
		// true if Detach operation available
		bool IsDetachable{ get; }
		// the chance for player to confirms before detach.
		bool QueryDetach();
		void Detach();
		void Attach();
		bool IsAttached { get; }
		// Plugin has several Contributions. This returns true if one or more Contribution detached
		bool IsPartiallyDetached { get; }
		AttachChangeEvent OnDetach { get; set; }
		AttachChangeEvent OnAttach { get; set; }
	}
}
