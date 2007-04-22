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
			new OpponentTeam("JefYouKnighthead Chiha",0),
			new OpponentTeam("Sunflash Hirojima",10),
			new OpponentTeam("Yokobama-F Marinades",20),
			new OpponentTeam("Nakoya Grandpas-eighty",30),
			new OpponentTeam("Kajima Untollders",40),
			new OpponentTeam("Oita Trineat",50),
			new OpponentTeam("Shovel Iwada",60),
			new OpponentTeam("Shimitsu Essplus",70),
			new OpponentTeam("Nanba Ozaka",80),
			new OpponentTeam("Kawazaki Fractale",90),
			new OpponentTeam("Urawa Mets",100)
			//! new OpponentTeam("ジェフユーナイトヘッド千葉",0),
			//! new OpponentTeam("サンフラッシュ広島",10),
			//! new OpponentTeam("横浜Ｆマリネーズ",20),
			//! new OpponentTeam("名古屋グランパズエイティ",30),
			//! new OpponentTeam("鹿島アントールダーズ",40),
			//! new OpponentTeam("大分トリニート",50),
			//! new OpponentTeam("ショベル磐田",60),
			//! new OpponentTeam("清水エスプラス",70),
			//! new OpponentTeam("ナンバ大阪",80),
			//! new OpponentTeam("川崎フラクターレ",90),
			//! new OpponentTeam("浦和メッツ",100)
		};


		/// <summary>
		/// Select one team randomly.
		/// </summary>
		public static OpponentTeam drawRandom() {
			return OPPONENTS[Const.rnd.Next(OPPONENTS.Length)];
		}
	}
}
