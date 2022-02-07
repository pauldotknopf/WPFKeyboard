#include "StdAfx.h"
#include "KLL.h"
#include <iostream>

//////////////////////////////////////////////////////////////////////////
// Create / destruct of CKLL class
CKLL::CKLL(void)
{
	hHandle = NULL;
	KbdTables = NULL;
	KbdTables64 = NULL;
	std::cout << "Is64BitWindows=" << Is64BitWindows() << "\n";
}

CKLL::~CKLL(void)
{
	UnloadData();
	this->UnloadDLL();
}
//////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////
// Load DLL and UnloadDLL functions
// Manages 32 and 64-bit system, when running a 32-bit app
BOOL CKLL::LoadDLL(char* sKeyboardDll )
{
	std::cout << "LoadDLL...  " << sKeyboardDll << "\n";

	//Unload if loaded...
	if(hHandle)
		this->UnloadDLL();

	//Load the dll as usual
	//TRACE(L"Loading Keyboard DLL %ws\n", sKeyboardDll);
	hHandle = LoadLibraryA(sKeyboardDll);

	if (!hHandle)
	{
		std::cout << "Failed to load DLL...\n";
		this->UnloadDLL();
		return FALSE;
	}

	//Get the Keyboard import function
	pfnKbdLayerDescriptor = (PFN_KBDLAYERDESCRIPTOR)GetProcAddress(hHandle, "KbdLayerDescriptor");

	//Return if error
	if(!pfnKbdLayerDescriptor)
	{
		std::cout << "Could not load kbdLayerDescriptor, is it a real keyboard layout file?";
		this->UnloadDLL();
		return FALSE;
	}

	//Get the keyboard descriptor export and set table
	if(!Is64BitWindows()) //32-bit
	{
		std::cout << "Detected 32bit...\n";

		//Init the tables
		KbdTables = (PKBDTABLES)pfnKbdLayerDescriptor();

		//If not set, unload
		if(!KbdTables)
		{
			this->UnloadDLL();
			return FALSE;
		}

		this->UnloadData();
		this->Fill32();
	}
	else //64-bit
	{
		std::cout << "Detected 64bit...\n";

		//Init the tables
		KbdTables64 = (PKBDTABLES64)pfnKbdLayerDescriptor();

		//If not set, unload
		if(!KbdTables64)
		{
			this->UnloadDLL();
			return FALSE;
		}

		this->UnloadData();
		this->Fill64();
	}

	return TRUE;
}

void CKLL::UnloadDLL()
{
	//Free DLL, if loaded
	if(hHandle != NULL)
	{
		FreeLibrary(hHandle);
		hHandle = NULL;
		KbdTables = NULL;
		KbdTables64 = NULL;
	}
}

// Fill functions add all the chars based on the VK to an array
void CKLL::Fill32()
{
	std::cout << "32bit\n";

	// if KbdTables64 aren't set, just silent return
	if(!KbdTables)
		return;

	_localeFlags = KbdTables->fLocaleFlags;

	// modifier keys
	PMODIFIERS pCharModifiers = KbdTables->pCharModifiers;
	PVK_TO_BIT pVkToBit = pCharModifiers->pVkToBit;
	while (pVkToBit->Vk)
	{
		VK_MODIFIER *modifier = new VK_MODIFIER();
		modifier->VirtualKey = pVkToBit->Vk;
		modifier->ModifierBits = pVkToBit->ModBits;
		m_vkModifiersArray.insert(m_vkModifiersArray.end(), modifier);
		++pVkToBit;
	}
	
	// modifier bits/combinations
	for(int x = 0; x <= pCharModifiers->wMaxModBits; x++)
	{
		m_modBits.insert(m_modBits.end(), pCharModifiers->ModNumber[x]); 
	}

	// virtual keys to chars with modifieres
	PVK_TO_WCHAR_TABLE pVkToWchTbl = KbdTables->pVkToWcharTable;
	while (pVkToWchTbl->pVkToWchars)
	{
		PVK_TO_WCHARS1 pVkToWch = pVkToWchTbl->pVkToWchars;
		while (pVkToWch->VirtualKey)
		{
			VK_STRUCT *pVK = new VK_STRUCT();
			pVK->VirtualKey = (int)pVkToWch->VirtualKey;
			pVK->Attributes = pVkToWch->Attributes;

			for (int i = 0; i < pVkToWchTbl->nModifications; ++i)
			{
				if (pVkToWch->wch[i] == WCH_LGTR)
				{
					VK_STRUCT_KEY key;
					key.Character = 0;
					key.IsLig = true;

					PLIGATURE1 current = KbdTables->pLigature;
					while (current)
					{
						if (current->VirtualKey == pVK->VirtualKey)
						{
							key.Ligs.insert(key.Ligs.end(), current->wch[i]);
						}
						current = (PLIGATURE1)(((PBYTE)current) + KbdTables64->cbLgEntry);
					}

					pVK->Characters.insert(pVK->Characters.end(), key);
				}
				else
				{
					VK_STRUCT_KEY key;
					key.Character = pVkToWch->wch[i];
					key.IsLig = false;

					pVK->Characters.insert(pVK->Characters.end(), key);
				}
			}
			m_vkarray.insert(m_vkarray.end(), pVK);
		}
		++pVkToWchTbl;
	}

	// virtual key scan codes
	for(int i = 0; i < KbdTables->bMaxVSCtoVK; i++ ) 
	{
		VK_SCANCODE *scanCode = new VK_SCANCODE();
		scanCode->VirtualKey = KbdTables->pusVSCtoVK[i];
		scanCode->ScanCode = i;
		m_vkScanCodesArray.insert(m_vkScanCodesArray.end(), scanCode);
	}

	PVSC_VK E0ScanCodes = KbdTables->pVSCtoVK_E0;
	while(E0ScanCodes->Vsc > 0)
	{
		VK_SCANCODE *scanCode = new VK_SCANCODE();
		scanCode->VirtualKey = E0ScanCodes->Vk;
		scanCode->ScanCode = E0ScanCodes->Vsc;
		scanCode->E0Set = true;
		m_vkScanCodesArray.insert(m_vkScanCodesArray.end(), scanCode);
		E0ScanCodes++;
	}

	PVSC_VK E1ScanCodes = KbdTables->pVSCtoVK_E1;
	while(E1ScanCodes->Vsc > 0)
	{
		VK_SCANCODE *scanCode = new VK_SCANCODE();
		scanCode->VirtualKey = E1ScanCodes->Vk;
		scanCode->ScanCode = E1ScanCodes->Vsc;
		scanCode->E1Set = true;
		m_vkScanCodesArray.insert(m_vkScanCodesArray.end(), scanCode);
		E1ScanCodes++;
	}

	// virtual key text
	PVSC_LPWSTR keyNames = KbdTables->pKeyNames;
	while(keyNames->vsc)
	{
		SC_TEXT *scanCodeText = new SC_TEXT();
		scanCodeText->ScanCode = keyNames->vsc;
		scanCodeText->Text = keyNames->pwsz;
		m_scTextArray.insert(m_scTextArray.end(), scanCodeText);
		keyNames++;
	}
}

void CKLL::UnloadData()
{
	_localeFlags = 0;
	m_vkModifiersArray.clear();
	this->ClearVKChar();
	this->ClearVKModifiers();
	this->ClearVKScanCodes();
	this->ClearSCText();
}

void CKLL::Fill64()
{
	// if KbdTables64 aren't set, just silent return
	if(!KbdTables64)
		return;

	_localeFlags = KbdTables64->fLocaleFlags;

	// modifier keys
	PMODIFIERS64 pCharModifiers = KbdTables64->pCharMODIFIERS64;
	PVK_TO_BIT64 pVkToBit = pCharModifiers->pVkToBit;
	while (pVkToBit->Vk)
	{
		VK_MODIFIER *modifier = new VK_MODIFIER();
		modifier->VirtualKey = pVkToBit->Vk;
		modifier->ModifierBits = pVkToBit->ModBits;
		m_vkModifiersArray.insert(m_vkModifiersArray.end(), modifier);
		++pVkToBit;
	}
	
	// modifier bits/combinations
	for(int x = 0; x <= pCharModifiers->wMaxModBits; x++)
	{
		m_modBits.insert(m_modBits.end(), pCharModifiers->ModNumber[x]); 
	}

	// virtual keys to chars with modifieres
	PVK_TO_WCHAR_TABLE64 pVkToWchTbl = KbdTables64->pVkToWcharTable;
	while (pVkToWchTbl->pVkToWchars)
	{
		PVK_TO_WCHARS641 pVkToWch = pVkToWchTbl->pVkToWchars;
		while (pVkToWch->VirtualKey)
		{
			if (pVkToWch->VirtualKey != 0xFF)
			{
				VK_STRUCT *pVK = new VK_STRUCT();
				pVK->VirtualKey = (int)pVkToWch->VirtualKey;
				pVK->Attributes = pVkToWch->Attributes;

				for (int i = 0; i < pVkToWchTbl->nModifications; ++i)
				{
					if (pVkToWch->wch[i] == WCH_LGTR)
					{
						VK_STRUCT_KEY key;
						key.Character = 0;
						key.IsLig = true;
						key.IsDeadKey = false;

						PLIGATURE641 current = KbdTables64->pLIGATURE64;
						while (current)
						{
							if (current->VirtualKey == pVK->VirtualKey && current->ModificationNumber == i)
							{
								key.Ligs.insert(key.Ligs.end(), current->wch[0]);
								key.Ligs.insert(key.Ligs.end(), current->wch[1]);
								break;
							}
							current = (PLIGATURE641)(((PBYTE)current) + KbdTables64->cbLgEntry);
						}

						/*key.Ligs.insert(key.Ligs.end(), KbdTables64->pLIGATURE64->wch[0]);
						key.Ligs.insert(key.Ligs.end(), KbdTables64->pLIGATURE64->wch[1]);*/

						pVK->Characters.insert(pVK->Characters.end(), key);
					}
					else if (pVkToWch->wch[i] == WCH_DEAD)
					{
						//Is DeadKey
						//Fetch Next key for character
						PVK_TO_WCHARS641 pVkToWchNext = (PVK_TO_WCHARS641)(((PBYTE)pVkToWch) + pVkToWchTbl->cbSize);
						VK_STRUCT_KEY key;
						key.Character = pVkToWchNext->wch[i];
						key.IsLig = false;
						key.IsDeadKey = true;

						pVK->Characters.insert(pVK->Characters.end(), key);
					}
					else
					{
						VK_STRUCT_KEY key;
						key.Character = pVkToWch->wch[i];
						key.IsLig = false;
						key.IsDeadKey = false;

						pVK->Characters.insert(pVK->Characters.end(), key);
					}
				}
				m_vkarray.insert(m_vkarray.end(), pVK);
			}
			pVkToWch = (PVK_TO_WCHARS641)(((PBYTE)pVkToWch) + pVkToWchTbl->cbSize);
		}
		++pVkToWchTbl;
	}

	// virtual key scan codes
	for(int i = 0; i < KbdTables64->bMaxVSCtoVK; i++ ) 
	{
		VK_SCANCODE *scanCode = new VK_SCANCODE();
		scanCode->VirtualKey = KbdTables64->pusVSCtoVK[i];
		scanCode->ScanCode = i;
		m_vkScanCodesArray.insert(m_vkScanCodesArray.end(), scanCode);
	}

	PVSC_VK64 E0ScanCodes = KbdTables64->pVSCtoVK_E0;
	while(E0ScanCodes->Vsc > 0)
	{
		VK_SCANCODE *scanCode = new VK_SCANCODE();
		scanCode->VirtualKey = E0ScanCodes->Vk;
		scanCode->ScanCode = E0ScanCodes->Vsc;
		scanCode->E0Set = true;
		m_vkScanCodesArray.insert(m_vkScanCodesArray.end(), scanCode);
		E0ScanCodes++;
	}

	PVSC_VK64 E1ScanCodes = KbdTables64->pVSCtoVK_E1;
	while(E1ScanCodes->Vsc > 0)
	{
		VK_SCANCODE *scanCode = new VK_SCANCODE();
		scanCode->VirtualKey = E1ScanCodes->Vk;
		scanCode->ScanCode = E1ScanCodes->Vsc;
		scanCode->E1Set = true;
		m_vkScanCodesArray.insert(m_vkScanCodesArray.end(), scanCode);
		E1ScanCodes++;
	}

	// virtual key text
	PVSC_LPWSTR64 keyNames = KbdTables64->pKeyNames;
	while(keyNames->vsc)
	{
		SC_TEXT *scanCodeText = new SC_TEXT();
		scanCodeText->ScanCode = keyNames->vsc;
		scanCodeText->Text = keyNames->pwsz;
		m_scTextArray.insert(m_scTextArray.end(), scanCodeText);
		keyNames++;
	}
}

int CKLL::GetVKCount()
{
	return m_vkarray.size();
}

CKLL::VK_STRUCT* CKLL::GetVKAtIndex(int index)
{
	return m_vkarray[index];
}

void CKLL::ClearVKChar()
{
	for(int i = 0;i<m_vkarray.size();i++)
	{
		VK_STRUCT *pVK = m_vkarray[i];
		delete pVK;
	}
	m_vkarray.clear();
}

int CKLL::GetModifiersCount()
{
	return m_vkModifiersArray.size();
}

CKLL::VK_MODIFIER* CKLL::GetModifierAtIndex(int index)
{
	return m_vkModifiersArray[index];
}

void CKLL::ClearVKModifiers()
{
	for(int i = 0;i<m_vkModifiersArray.size();i++)
	{
		VK_MODIFIER *pVK = m_vkModifiersArray[i];
		delete pVK;
	}
	m_vkModifiersArray.clear();
}


int CKLL::GetScanCodesCount()
{
	int result = m_vkScanCodesArray.size();
	return result;
}

CKLL::VK_SCANCODE* CKLL::GetScanCodeAtIndex(int index)
{
	return m_vkScanCodesArray[index];
}

void CKLL::ClearVKScanCodes()
{
	for(int i = 0;i<m_vkScanCodesArray.size();i++)
	{
		VK_SCANCODE *pVK = m_vkScanCodesArray[i];
		delete pVK;
	}
	m_vkScanCodesArray.clear();
}

int CKLL::GetScanCodeTextCount()
{
	return m_scTextArray.size();
}

CKLL::SC_TEXT* CKLL::GetScanCodeTextAtIndex(int index)
{
	return m_scTextArray[index];
}

void CKLL::ClearSCText()
{
	for(int i = 0;i<m_scTextArray.size();i++)
	{
		SC_TEXT *pVK = m_scTextArray[i];
		delete pVK;
	}
	m_scTextArray.clear();
}

int CKLL::GetModBitsSize()
{
	return m_modBits.size();
}

int CKLL::GetModBitAtIndex(int index)
{
	return m_modBits[index];
}

int CKLL::GetLocaleFlags()
{
	return _localeFlags;
}

typedef BOOL (WINAPI *LPFN_ISWOW64PROCESS) (HANDLE, PBOOL);
BOOL CKLL::Is64BitWindows()
{
#if defined(_WIN64)
	return TRUE;  // 64-bit programs run only on Win64
#elif defined(_WIN32)
	// 32-bit programs run on both 32-bit and 64-bit Windows
	// so must sniff
	BOOL f64 = FALSE;
	LPFN_ISWOW64PROCESS fnIsWow64Process;

	fnIsWow64Process = (LPFN_ISWOW64PROCESS) GetProcAddress(GetModuleHandleA("kernel32"),"IsWow64Process");
	if(NULL != fnIsWow64Process)
	{
		return fnIsWow64Process(GetCurrentProcess(),&f64) && f64;
	}
	return FALSE;
#else
	return FALSE; // Win64 does not support Win16
#endif
}