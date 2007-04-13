using System;
using System.Diagnostics;
using nft.core.game;
using nft.core.schedule;

namespace nft.impl.game
{
	/// <summary>
	/// GameImpl �̊T�v�̐����ł��B
	/// </summary>
	[Serializable]
	public class GameImpl : IGame
	{
		protected Clock clock;

		protected string name;

		public GameImpl()
		{
			// TODO : this is test code
			clock = new Clock(new Time(0));
		}

		#region IGame �����o

		public void Start()
		{
			// TODO : this is test code
			for(int i = 0; i<30000; i++ )
			{
				//System.Threading.Thread.Sleep(100);
				clock.Tick(Time.MINUTE);
			}
		}

		public void Close()
		{
			// TODO:  GameImpl.Close ������ǉ����܂��B
		}

		public bool Modified
		{
			get
			{
				// TODO:  GameImpl.Modified getter ������ǉ����܂��B
				return false;
			}
		}

		public nft.core.schedule.Clock Clock
		{
			get
			{
				return clock;
			}
		}

		public IClimateController ClimateController
		{
			get
			{
				// TODO:  GameImpl.OverrideTable getter ������ǉ����܂��B
				return null;
			}
		}

		public string Name
		{
			get{ return name; }
			set{ name = value; }
		}

		#endregion
	}
}
