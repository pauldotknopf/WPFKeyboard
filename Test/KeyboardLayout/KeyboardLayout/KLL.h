#pragma  once
#include "kbd.h"
#include "kbd64.h"

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
	BOOL	LoadDLL(CString sKeyboardDll);
	USHORT	GetVKCount();
	CString	GetChar(USHORT iVK);
	CString GetSC(USHORT iVK);

	//Return TRUE if x64, false if not...
	BOOL	Is64BitWindows();

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

	//////////////////////////////////////////////////////////////////////////
	//Structs to handle all keys assigned to a VK
	struct VK_STRUCT
	{
		USHORT nVK;
		CUIntArray aSC;
		CStringArray aChar;
	};

	//Variable to keep track of VKs
	std::vector m_vkarray;

	//Function to add / clear new VK-char
	void AddVKChar(USHORT nVK, CString wChar);	//Returns the characters for the available VK
	void AddVKSC(USHORT nVK, USHORT nSC);		//Returns the scan code for the available VK
	int VKExist(USHORT nVK);					//Returns the number in the array for the VK
	void ClearVKChar();							//Clears the table
};
