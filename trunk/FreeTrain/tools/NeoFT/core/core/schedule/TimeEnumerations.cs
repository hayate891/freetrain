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
	P Antarctica μΙζ 
	Q Main Taiga εv^CK 
	R Cool Conifer βΡjtχΡ 
	S Cool Mixed βΡ¬Ρ 
	T Warm deciduous ·ΡtχΡ 
	U Warm mixed ·Ρ¬Ρ 
	V Warm conifer ·ΡjtχΡ 
	W Tropical Montane MΡRnΡ 
	X Tropical Seasonal MΡGίΡ 
	PO Equatorial Evergreen ΤΉνΞχΡ 
	PP Cool Crops βΡqnΡ 
	PQ Warm Crops ·ΡqnΡ 
	PR Tropical Dry Forest MΡ£Ρ 
	PS Paddylands ξμn 
	PT Warm Irrigated ·ΡΑςn 
	PU Cool Irrigated βΡΑςn 
	PV Cold Irrigated ¦ΡΑςn 
	PW Cool Grass/Shrub βΡn^αΨn 
	PX Warm Grass/Shrub ·Ρn^αΨn 
	QO Highland Shrub nαΨn 
	QP Med. Grazing Hϊqn 
	QQ Semiarid Woods Ό£Ρn 
	QR Siberian Parks VxAφnH 
	QS Heaths, Moors q[XArμ 
	QT Succulent Thorns ½t’ΞηΡ 
	QU Northern Taiga k^CK 
	QV Tropical Savanna MΡToi 
	QW Cool Field/Woods βΡ½΄^Ρn 
	QX Warm Field/Woods ·Ρ½΄^Ρn 
	RO Warm Forest/Field ·ΡXΡ^Ρn 
	RP Cool Forest/Field βΡXΡ^Ρn 
	RQ Southern Taiga μ^CK 
	RR EasternH Southern Taiga μ^CKH 
	RS Tropical Montane MΡRnΡH 
	RT Marsh, Swamp ΌnAΐn 
	RU Mangroves }O[uΡ 
	RV Low Scrub αGΨ 
	RW Bogs, Bog Woods ΐAΐnΡ 
	RX Hot Desert ·» 
	SO Cool Desert βΡ» 
	SP Wooded Tundra Ρnch 
	SQ Tundra ch 
	SR Sand Desert » 
	SS Polar Desert Ιζ» 
*/
}
