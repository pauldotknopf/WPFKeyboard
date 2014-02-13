#pragma  once
#include "kbd.h"
#include "kbd64.h"
#include <vector>

//Loaddefs for a 32/64-bit handle of keyboard-loading
typedef PKBDTABLES(* KbdLayerDescriptor)(VOID);
typedef PKBDTABLES64(* KbdLayerDescriptor64)(VOID);
typedef PVOID(*PFN_KBDLAYERDESCRIPTOR)(VOID);

class CKLL
{
public:
	CKLL(void);
	~CKLL(void);

	//Public functions to return info regarding the keyboard-dll loaded
	BOOL	LoadDLL(char* sKeyboardDll);
	USHORT	GetVKCount();

	//Return TRUE if x64, false if not...
	BOOL	Is64BitWindows();

	//////////////////////////////////////////////////////////////////////////
	//Structs to handle all keys assigned to a VK
	struct VK_STRUCT
	{
		USHORT nVK;
		BYTE attributes;
		std::vector<int> aSC;
		std::vector<wchar_t> aChar;
	};

	CKLL::VK_STRUCT* CKLL::GetVKAtIndex(BYTE index);
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

	//Variable to keep track of VKs
	std::vector<VK_STRUCT*> m_vkarray;

	void ClearVKChar();	
};
