using System;

namespace RoadSimulator
{
	/// <summary>
	/// ���ݒ�
	/// </summary>
	public class Configure
	{
		//���H�̃��x���B�������A0����ԑ������H�B
		static public readonly int RoadLevelMax = 4;
		//�w�O���H�̉w����̕��ϋ����B
		static public readonly int MeanDistanceFromStation = 4;
		//�A�����s�̃E�F�C�g�B
		static public readonly int TimerInterval = 4;
		//�}������ő哹�H���x��(����ȍ~�̃��x���̓��H�͎}��������Ȃ�)
		static public readonly int noTrunkLevel = 4;

		//�w�̃��x�����w�̐��ʂɂł��铹�H�̃��x���B
		static private int stationLv = 1;
		static public int stationLevel 
		{
			get{ return stationLv; }
			set{ stationLv= Math.Max(0,Math.Min(RoadLevelMax,value)); }
		}

		private Configure()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
