// DirectDraw.AlphaBlend.cpp : DLL �G�N�X�|�[�g�̎����ł��B

#include "stdafx.h"
#include "resource.h"
#include "DirectDraw.AlphaBlend.h"

class CDirectDrawAlphaBlendModule : public CAtlDllModuleT< CDirectDrawAlphaBlendModule >
{
public :
	DECLARE_LIBID(LIBID_DirectDrawAlphaBlendLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_DIRECTDRAWALPHABLEND, "{A622BFB1-EAFB-4471-8523-313435ED01D6}")
};

CDirectDrawAlphaBlendModule _AtlModule;


// DLL �G���g�� �|�C���g
extern "C" BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
	hInstance;
    return _AtlModule.DllMain(dwReason, lpReserved); 
}


// DLL �� OLE �ɂ���ăA�����[�h�ł���悤�ɂ��邩�ǂ������w�肵�܂��B
STDAPI DllCanUnloadNow(void)
{
    return _AtlModule.DllCanUnloadNow();
}


// �v�����ꂽ�^�̃I�u�W�F�N�g���쐬����N���X �t�@�N�g����Ԃ��܂��B
STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
    return _AtlModule.DllGetClassObject(rclsid, riid, ppv);
}


// DllRegisterServer - �G���g�����V�X�e�� ���W�X�g���ɒǉ����܂��B
STDAPI DllRegisterServer(void)
{
    // �I�u�W�F�N�g�A�^�C�v ���C�u��������у^�C�v ���C�u�������̑S�ẴC���^�[�t�F�C�X��o�^���܂�
    HRESULT hr = _AtlModule.DllRegisterServer();
	return hr;
}


// DllUnregisterServer - �G���g�������W�X�g������폜���܂��B
//
STDAPI DllUnregisterServer(void)
{
	HRESULT hr = _AtlModule.DllUnregisterServer();
	return hr;
}
