using System;

namespace RoadSimulator
{
	/// <summary>
	/// ���H�̉�
	/// </summary>
	internal class RoadBud
	{
		private int _level;
		private int _sleep;
		// �x�����̉�H
		public bool sleeping{ get { return (_sleep>0); }}
		public int level { get { return _level; }}

		internal int[] lastBranch;
		// �萁���ς݁H
		internal bool sprouted;

		public RoadBud(int level)
		{
			this._level = level;
			_sleep = 0;
			sprouted = false;
			//lastBranch = new int[Configure.RoadLevelMax];
		}
		public RoadBud(int level, int sleep)
		{
			this._level = level;
			this._sleep = sleep;
			sprouted = false;
			//lastBranch = new int[Configure.RoadLevelMax];
		}
		internal void stepSleep() 
		{
			_sleep--;
		}
	}
}
