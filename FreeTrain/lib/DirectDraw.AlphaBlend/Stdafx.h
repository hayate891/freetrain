// stdafx.h : �W���̃V�X�e�� �C���N���[�h �t�@�C���̃C���N���[�h �t�@�C���A�܂���
// �Q�Ɖ񐔂������A�����܂�ύX����Ȃ��A�v���W�F�N�g��p�̃C���N���[�h �t�@�C��
// ���L�q���܂��B

#pragma once

#ifndef STRICT
#define STRICT
#endif

//// ���Ŏw�肳�ꂽ��`�̑O�ɑΏۃv���b�g�t�H�[�����w�肵�Ȃ���΂Ȃ�Ȃ��ꍇ�A�ȉ��̒�`��ύX���Ă��������B
//// �قȂ�v���b�g�t�H�[���ɑΉ�����l�Ɋւ���ŐV���ɂ��ẮAMSDN ���Q�Ƃ��Ă��������B
//#ifndef WINVER				// Windows 95 ����� Windows NT 4 �ȍ~�̃o�[�W�����ɌŗL�̋@�\�̎g�p�������܂��B
//#define WINVER 0x0400		// ����� Windows 98 ����� Windows 2000 �܂��͂���ȍ~�̃o�[�W���������ɓK�؂Ȓl�ɕύX���Ă��������B
//#endif
//
//#ifndef _WIN32_WINNT		// Windows NT 4 �ȍ~�̃o�[�W�����ɌŗL�̋@�\�̎g�p�������܂��B
//#define _WIN32_WINNT 0x0400	// ����� Windows 2000 �܂��͂���ȍ~�̃o�[�W���������ɓK�؂Ȓl�ɕύX���Ă��������B
//#endif						
//
//#ifndef _WIN32_WINDOWS		// Windows 98 �ȍ~�̃o�[�W�����ɌŗL�̋@�\�̎g�p�������܂��B
//#define _WIN32_WINDOWS 0x0410 // ����� Windows Me �܂��͂���ȍ~�̃o�[�W���������ɓK�؂Ȓl�ɕύX���Ă��������B
//#endif
//
//#ifndef _WIN32_IE			// IE 4.0 �ȍ~�̃o�[�W�����ɌŗL�̋@�\�̎g�p�������܂��B
//#define _WIN32_IE 0x0400	// ����� IE 5.0  �܂��͂���ȍ~�̃o�[�W���������ɓK�؂Ȓl�ɕύX���Ă��������B
//#endif

#define _ATL_APARTMENT_THREADED
#define _ATL_NO_AUTOMATIC_NAMESPACE

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// �ꕔ�� CString �R���X�g���N�^�͖����I�ł��B

// ��ʓI�Ŗ������Ă����S�� MFC �̌x�����b�Z�[�W�̈ꕔ�̔�\�����������܂��B
#define _ATL_ALL_WARNINGS


#include "resource.h"
#include <atlbase.h>
#include <atlcom.h>
#include <utility>

using namespace ATL;
using namespace std;


#import <dx7vb.dll> \
	rename("GetClassName","_GetClassName"), \
	rename("CreateEvent","_CreateEvent"), \
	rename("DrawText","_DrawText"), \
	rename("SetPort","_SetPort"), \
	rename("max","_max"), \
	rename("min","_min"), \
	rename("E_PENDING","_E_PENDING"), \
	rename("PC_EXPLICIT","_PC_EXPLICIT"), \
	rename("PC_RESERVED","_PC_RESERVED"), \
	rename("PC_NOCOLLAPSE","_PC_NOCOLLAPSE"), \
	rename("WAVE_FORMAT_1M08","_WAVE_FORMAT_1M08"), \
	rename("WAVE_FORMAT_1M16","_WAVE_FORMAT_1M16"), \
	rename("WAVE_FORMAT_1S08","_WAVE_FORMAT_1S08"), \
	rename("WAVE_FORMAT_1S16","_WAVE_FORMAT_1S16"), \
	rename("WAVE_FORMAT_2M08","_WAVE_FORMAT_2M08"), \
	rename("WAVE_FORMAT_2M16","_WAVE_FORMAT_2M16"), \
	rename("WAVE_FORMAT_2S08","_WAVE_FORMAT_2S08"), \
	rename("WAVE_FORMAT_2S16","_WAVE_FORMAT_2S16"), \
	rename("WAVE_FORMAT_4M08","_WAVE_FORMAT_4M08"), \
	rename("WAVE_FORMAT_4M16","_WAVE_FORMAT_4M16"), \
	rename("WAVE_FORMAT_4S08","_WAVE_FORMAT_4S08"), \
	rename("WAVE_FORMAT_4S16","_WAVE_FORMAT_4S16"), \
	rename("WAVE_FORMAT_PCM" ,"_WAVE_FORMAT_PCM")

using namespace DxVBLib;