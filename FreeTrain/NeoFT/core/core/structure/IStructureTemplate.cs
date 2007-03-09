using System;
using nft.framework;
using nft.core.geometry;

namespace nft.core.structure
{
	/// <summary>
	/// IStructure1 �̊T�v�̐����ł��B
	/// </summary>
	public interface IStructureTemplate : IHasNameAndID
	{
		Size3D Size { get; }
		object Category { get; }
		int Level { get; }
		int BuildCost { get; }
	}
}
