#pragma  once
#include "kbd.h"
#include "kbd64.h"
#include <vector>

//Loaddefs for a 32/64-bit handle of keyboard-loading
typedef PKBDTABLES(* KbdLayerDescriptor)(VOID);
typedef PKBDTABLES64(* KbdLayerDescriptor64)(VOID);
typedef PVOID(*PFN_KBDLAYERDESCRIPTOR)(VOID);



class __declspec( dllexport ) CKLL
{
public:

	struct VK_STRUCT
	{
		USHORT nVK;
		BYTE attributes;
		std::vector<wchar_t> characters;
	};
	struct VK_MODIFIER
	{
		int VirtualKey;
		int ModifierBits;
	};
	struct VK_SCANCODE
	{
		USHORT nVK;
		int scanCode;
	};

	CKLL(void);
	~CKLL(void);

	BOOL	LoadDLL(char* sKeyboardDll);
	
	BOOL	Is64BitWindows();

	USHORT	GetVKCount();
	CKLL::VK_STRUCT* CKLL::GetVKAtIndex(BYTE index);

	USHORT GetModifiersCount();
	CKLL::VK_MODIFIER* GetModifierAtIndex(BYTE index);

	USHORT GetScanCodesCount();
	CKLL::VK_SCANCODE* GetScanCodeAtIndex(BYTE index);

private:
	//Our loaded DLL
	HMODULE hHandle;
	void	UnloadDLL();

	//The actual DLL-function that init and fill the array
	PFN_KBDLAYERDESCRIPTOR pfnKbdLayerDescriptor;

	//32-bit variables
	PKBDTABLES KbdTables;
	void Fill32();

	//64-bit variables
	PKBDTABLES64 KbdTables64;
	void Fill64();

	std::vector<VK_STRUCT*> m_vkarray;
	void ClearVKChar();	
	std::vector<VK_MODIFIER*> m_vkModifiersArray;
	void ClearVKModifiers();	
	std::vector<VK_SCANCODE*> m_vkScanCodesArray;
	void ClearVKScanCodes();
};
