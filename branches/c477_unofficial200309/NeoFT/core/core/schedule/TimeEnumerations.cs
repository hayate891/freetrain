using System;

namespace nft.core.schedule
{
	// Yearly seasons
	public enum Season : byte 
	{ Spring, Summer, Autumn, Winter, Dry, Rainy }

	// Each 1/3 part of a month
	public enum TripartiteMonth : byte
	{ Early, Middle, Late }

	// Daily time zone.
	public enum DayNight : byte 
	{@Daybreak,Morning,Afternoon,Evening,EarlyNight,Midnight }

	// Weather types
	// Cloudy is an intermediate state, so assign low level of fine as Cloudy.
	public enum Weather : byte
	{ Fine, Rain, Thunder, Snow, Tempest, Sandstorm, Foggy }

	
	public enum MajorBiome : byte
	{ Tropical, Monsoon, Warm, Dessert, Savanna, Prairie, Mediterranean, Taiga, Tundra, Alpine }
/*
	‚P Antarctica “ì‹Éˆæ 
	‚Q Main Taiga å—vƒ^ƒCƒK 
	‚R Cool Conifer —â‘Ñj—t÷—Ñ 
	‚S Cool Mixed —â‘Ñ¬‡—Ñ 
	‚T Warm deciduous ‰·‘Ñ——t÷—Ñ 
	‚U Warm mixed ‰·‘Ñ¬‡—Ñ 
	‚V Warm conifer ‰·‘Ñj—t÷—Ñ 
	‚W Tropical Montane ”M‘ÑR’n—Ñ 
	‚X Tropical Seasonal ”M‘Ñ‹Gß—Ñ 
	‚P‚O Equatorial Evergreen Ô“¹í—Î÷—Ñ 
	‚P‚P Cool Crops —â‘Ñ’‘q’n‘Ñ 
	‚P‚Q Warm Crops ‰·‘Ñ’‘q’n‘Ñ 
	‚P‚R Tropical Dry Forest ”M‘ÑŠ£‘‡—Ñ 
	‚P‚S Paddylands ˆîì’n 
	‚P‚T Warm Irrigated ‰·‘ÑŠÁŸò’n 
	‚P‚U Cool Irrigated —â‘ÑŠÁŸò’n 
	‚P‚V Cold Irrigated Š¦‘ÑŠÁŸò’n 
	‚P‚W Cool Grass/Shrub —â‘Ñ‘’n^’á–Ø’n 
	‚P‚X Warm Grass/Shrub ‰·‘Ñ‘’n^’á–Ø’n 
	‚Q‚O Highland Shrub ‚’n’á–Ø’n 
	‚Q‚P Med. Grazing H•ú–q’n 
	‚Q‚Q Semiarid Woods ”¼Š£‘‡—Ñ’n 
	‚Q‚R Siberian Parks ƒVƒxƒŠƒAŒö‰€’nH 
	‚Q‚S Heaths, Moors ƒq[ƒXAr–ì 
	‚Q‚T Succulent Thorns ‘½‰t‚¢‚Î‚ç—Ñ 
	‚Q‚U Northern Taiga –kƒ^ƒCƒK 
	‚Q‚V Tropical Savanna ”M‘ÑƒTƒoƒ“ƒi 
	‚Q‚W Cool Field/Woods —â‘Ñ•½Œ´^—Ñ’n 
	‚Q‚X Warm Field/Woods ‰·‘Ñ•½Œ´^—Ñ’n 
	‚R‚O Warm Forest/Field ‰·‘ÑX—Ñ^—Ñ’n 
	‚R‚P Cool Forest/Field —â‘ÑX—Ñ^—Ñ’n 
	‚R‚Q Southern Taiga “ìƒ^ƒCƒK 
	‚R‚R EasternH Southern Taiga “Œ“ìƒ^ƒCƒKH 
	‚R‚S Tropical Montane ”M‘ÑR’n—ÑH 
	‚R‚T Marsh, Swamp ¼’nAÀ’n 
	‚R‚U Mangroves ƒ}ƒ“ƒOƒ[ƒu—Ñ 
	‚R‚V Low Scrub ’áG–Ø 
	‚R‚W Bogs, Bog Woods ÀAÀ’n—Ñ 
	‚R‚X Hot Desert ‚‰·»”™ 
	‚S‚O Cool Desert —â‘Ñ»”™ 
	‚S‚P Wooded Tundra —Ñ’nƒcƒ“ƒhƒ‰ 
	‚S‚Q Tundra ƒcƒ“ƒhƒ‰ 
	‚S‚R Sand Desert »”™ 
	‚S‚S Polar Desert ‹Éˆæ»”™ 
*/
}
