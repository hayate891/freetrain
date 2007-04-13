using System;
using nft.framework;

namespace nft.core.game
{
	public enum PlayerMode { FreeBuild, Government, LocalAuthority, Company, Other };
	public enum Ownership { None, Government, LocalAuthority, PlayerCompany, NonPlayerCompany, Private };
	/// <summary>
	/// IGameMode �̊T�v�̐����ł��B
	/// </summary>
	public interface IGameMode : IHasNameAndID
	{
		PlayerMode PlayerMode { get; }
		bool DynLandEdit { get; }

		bool HasFareIncome { get; }
		bool CanOwnSubsidaries{ get; }
		bool CityDevelopment { get; }

		bool CheckBuildPermission(Ownership owner);
		bool CheckInvitePermission(Ownership owner);
		bool CheckSellPermission(Ownership owner);
		bool CheckBuyPermission(Ownership owner);
		bool CheckRemovePermission(Ownership owner);
	}
}
