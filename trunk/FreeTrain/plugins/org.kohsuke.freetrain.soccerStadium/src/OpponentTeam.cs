using System;

namespace freetrain.world.soccerstadium
{
	/// <summary>
	/// Opponent team.
	/// </summary>
	[Serializable]
	public class OpponentTeam
	{
		/// <summary> Name of the team. </summary>
		public readonly string name;
		/// <summary> Strength of the team. </summary>
		public readonly int strength;

		private OpponentTeam( string _name, int _strength ) {
			this.name = _name;
			this.strength = _strength;
		}

		public static readonly OpponentTeam[] OPPONENTS = new OpponentTeam[]{
			new OpponentTeam("�T�K�����|",0),
			new OpponentTeam("�z�[���[�z�b�N�O��",10),
			new OpponentTeam("�Ök�x���}�[��",20),
			new OpponentTeam("�R���T�h�[����y",30),
			new OpponentTeam("���B�b�Z�����",40),
			new OpponentTeam("�W���s���֌�",50),
			new OpponentTeam("�����A���g���[�Y",60),
			new OpponentTeam("�����x���f�B",70),
			new OpponentTeam("�ؔ����C�\��",80),
			new OpponentTeam("�Y�a�u���[�X",90),
			new OpponentTeam("�����G�X�v���Y",100)
		};


		/// <summary>
		/// Select one team randomly.
		/// </summary>
		public static OpponentTeam drawRandom() {
			return OPPONENTS[Const.rnd.Next(OPPONENTS.Length)];
		}
	}
}
