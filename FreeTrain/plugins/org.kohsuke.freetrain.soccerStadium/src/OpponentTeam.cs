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
			new OpponentTeam("Sagan Tosaku",0),
			new OpponentTeam("Hollyhock Santo",10),
			new OpponentTeam("Shohoku Bellmare",20),
			new OpponentTeam("Consadole Iyahoro",30),
			new OpponentTeam("Vissel Sakagibe",40),
			new OpponentTeam("Jubilo Iwakuchi",50),
			new OpponentTeam("Katori Antlers",60),
			new OpponentTeam("Tokyo Verdi",70),
			new OpponentTeam("Kijira Reysol",80),
			new OpponentTeam("Urawa Blues",90),
			new OpponentTeam("Shimizu Espirits",100)
			//! new OpponentTeam("サガン鳥酢",0),
			//! new OpponentTeam("ホーリーホック三戸",10),
			//! new OpponentTeam("湘北ベルマーレ",20),
			//! new OpponentTeam("コンサドーレ礼幌",30),
			//! new OpponentTeam("ヴィッセル榊戸",40),
			//! new OpponentTeam("ジュピロ磐口",50),
			//! new OpponentTeam("鹿鳥アントラーズ",60),
			//! new OpponentTeam("束京ベルディ",70),
			//! new OpponentTeam("木白レイソル",80),
			//! new OpponentTeam("浦和ブルース",90),
			//! new OpponentTeam("清水エスプリズ",100)
		};


		/// <summary>
		/// Select one team randomly.
		/// </summary>
		public static OpponentTeam drawRandom() {
			return OPPONENTS[Const.rnd.Next(OPPONENTS.Length)];
		}
	}
}
