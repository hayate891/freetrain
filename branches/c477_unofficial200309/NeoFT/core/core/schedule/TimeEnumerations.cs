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
	{�@Daybreak,Morning,Afternoon,Evening,EarlyNight,Midnight }

	// Weather types
	// Cloudy is an intermediate state, so assign low level of fine as Cloudy.
	public enum Weather : byte
	{ Fine, Rain, Thunder, Snow, Tempest, Sandstorm, Foggy }

	
	public enum MajorBiome : byte
	{ Tropical, Monsoon, Warm, Dessert, Savanna, Prairie, Mediterranean, Taiga, Tundra, Alpine }
/*
	�P Antarctica ��Ɉ� 
	�Q Main Taiga ��v�^�C�K 
	�R Cool Conifer ��ѐj�t���� 
	�S Cool Mixed ��э����� 
	�T Warm deciduous ���ї��t���� 
	�U Warm mixed ���э����� 
	�V Warm conifer ���ѐj�t���� 
	�W Tropical Montane �M�юR�n�� 
	�X Tropical Seasonal �M�ыG�ߗ� 
	�P�O Equatorial Evergreen �ԓ���Ύ��� 
	�P�P Cool Crops ��э��q�n�� 
	�P�Q Warm Crops ���э��q�n�� 
	�P�R Tropical Dry Forest �M�ъ����� 
	�P�S Paddylands ���n 
	�P�T Warm Irrigated ���ъ���n 
	�P�U Cool Irrigated ��ъ���n 
	�P�V Cold Irrigated ���ъ���n 
	�P�W Cool Grass/Shrub ��ё��n�^��ؒn 
	�P�X Warm Grass/Shrub ���ё��n�^��ؒn 
	�Q�O Highland Shrub ���n��ؒn 
	�Q�P Med. Grazing �H���q�n 
	�Q�Q Semiarid Woods �������ђn 
	�Q�R Siberian Parks �V�x���A�����n�H 
	�Q�S Heaths, Moors �q�[�X�A�r�� 
	�Q�T Succulent Thorns ���t���΂�� 
	�Q�U Northern Taiga �k�^�C�K 
	�Q�V Tropical Savanna �M�уT�o���i 
	�Q�W Cool Field/Woods ��ѕ����^�ђn 
	�Q�X Warm Field/Woods ���ѕ����^�ђn 
	�R�O Warm Forest/Field ���ѐX�с^�ђn 
	�R�P Cool Forest/Field ��ѐX�с^�ђn 
	�R�Q Southern Taiga ��^�C�K 
	�R�R Eastern�H Southern Taiga ����^�C�K�H 
	�R�S Tropical Montane �M�юR�n�сH 
	�R�T Marsh, Swamp ���n�A���n 
	�R�U Mangroves �}���O���[�u�� 
	�R�V Low Scrub ��G�� 
	�R�W Bogs, Bog Woods ���A���n�� 
	�R�X Hot Desert �������� 
	�S�O Cool Desert ��э��� 
	�S�P Wooded Tundra �ђn�c���h�� 
	�S�Q Tundra �c���h�� 
	�S�R Sand Desert ���� 
	�S�S Polar Desert �Ɉ捻�� 
*/
}
