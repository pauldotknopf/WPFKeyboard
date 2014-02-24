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
}

CKLL::~CKLL(void)
{
	this->ClearVKChar();
	this->ClearVKModifiers();
	this->ClearVKScanCodes();
	this->ClearSCText();
	this->UnloadDLL();
}
//////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////
// Load DLL and UnloadDLL functions
// Manages 32 and 64-bit system, when running a 32-bit app
BOOL CKLL::LoadDLL(char* sKeyboardDll )
{
	std::cout << "64bit\n";

	//Unload if loaded...
	if(hHandle)
		this->UnloadDLL();

	//Load the dll as usual
	//TRACE(L"Loading Keyboard DLL %ws\n", sKeyboardDll);
	hHandle = LoadLibraryA(sKeyboardDll);
	if (!hHandle)
	{
		//AfxMessageBox(L"Failed to load dll " + sKeyboardDll);
		this->UnloadDLL();
		return FALSE;
	}

	//Get the Keyboard import function
	pfnKbdLayerDescriptor = (PFN_KBDLAYERDESCRIPTOR)GetProcAddress(hHandle, "KbdLayerDescriptor");

	//Return if error
	if(!pfnKbdLayerDescriptor)
	{
		//AfxMessageBox(L"Could not load kbdLayerDescriptor, is it a real keyboard layout file?");
		this->UnloadDLL();
		return FALSE;
	}

	//Get the keyboard descriptor export and set table
	if(!Is64BitWindows()) //32-bit
	{
		//Init the tables
		KbdTables = (PKBDTABLES)pfnKbdLayerDescriptor();

		//If not set, unload
		if(!KbdTables)
		{
			this->UnloadDLL();
			return FALSE;
		}

		this->ClearVKChar();
		this->ClearVKModifiers();
		this->ClearVKScanCodes();
		this->ClearSCText();
		this->Fill32();
	}
	else //64-bit
	{
		//Init the tables
		KbdTables64 = (PKBDTABLES64)pfnKbdLayerDescriptor();

		//If not set, unload
		if(!KbdTables64)
		{
			this->UnloadDLL();
			return FALSE;
		}

		this->ClearVKChar();
		this->ClearVKModifiers();
		this->ClearVKScanCodes();
		this->ClearSCText();
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

	//If KbdTables aren't set, just silent return
	if(!KbdTables)
		return;

	///*
	//* Modifier keys
	//*/
	//PMODIFIERS pCharModifiers = KbdTables->pCharModifiers;
	//PVK_TO_BIT pVkToBit = pCharModifiers->pVkToBit;
	//while (pVkToBit->Vk)
	//{
	//	std::cout << "VkToBit " << pVkToBit->Vk << " -> " << pVkToBit->ModBits << "\n";
	//	++pVkToBit;
	//}

	////Handle all the chars with modifieres
	//PVK_TO_WCHAR_TABLE pVkToWchTbl = KbdTables->pVkToWcharTable;
	//while (pVkToWchTbl->pVkToWchars)
	//{
	//	PVK_TO_WCHARS1 pVkToWch = pVkToWchTbl->pVkToWchars;
	//	while (pVkToWch->VirtualKey)
	//	{
	//		VK_STRUCT *pVK = new VK_STRUCT();
	//		pVK->nVK = pVkToWch->VirtualKey;
	//		pVK->attributes = pVkToWch->Attributes;

	//		for (int i = 0; i < pVkToWchTbl->nModifications; ++i)
	//		{
	//			pVK->aChar.insert(pVK->aChar.end(), pVkToWch->wch[i]);
	//		}
	//		m_vkarray.insert(m_vkarray.end(), pVK);

	//		pVkToWch = (PVK_TO_WCHARS1)(((PBYTE)pVkToWch) + pVkToWchTbl->cbSize);
	//	}
	//	++pVkToWchTbl;
	//}

	////If KbdTables aren't set, just silent return
	//if(!KbdTables)
	//	return;

	////Fill all the SC into VKs array
	//for (int i = 0; i < KbdTables->bMaxVSCtoVK; i++)
	//	this->AddVKSC(KbdTables->pusVSCtoVK[i], i);

	////Handle all the chars with modifieres
	//PVK_TO_WCHAR_TABLE pVkToWchTbl = KbdTables->pVkToWcharTable;
	//while (pVkToWchTbl->pVkToWchars)
	//{
	//	PVK_TO_WCHARS1 pVkToWch = pVkToWchTbl->pVkToWchars;
	//	while (pVkToWch->VirtualKey)
	//	{
	//		for (int i = 0; i < pVkToWchTbl->nModifications; ++i)
	//		{
	//			sChar.Format(L"%wc (%.4x)", pVkToWch->wch[i], pVkToWch->wch[i]);
	//			this->AddVKChar(pVkToWch->VirtualKey, sChar);
	//		}

	//		pVkToWch = (PVK_TO_WCHARS1)(((PBYTE)pVkToWch) + pVkToWchTbl->cbSize);
	//	}
	//	++pVkToWchTbl;
	//}
}

void CKLL::Fill64()
{
	// if KbdTables64 aren't set, just silent return
	if(!KbdTables64)
		return;

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

	// virtual keys to chars with modifieres
	PVK_TO_WCHAR_TABLE64 pVkToWchTbl = KbdTables64->pVkToWcharTable;
	while (pVkToWchTbl->pVkToWchars)
	{
		PVK_TO_WCHARS641 pVkToWch = pVkToWchTbl->pVkToWchars;
		while (pVkToWch->VirtualKey)
		{
			printf("Virtual Key: %d\n", pVkToWch->VirtualKey);
			VK_STRUCT *pVK = new VK_STRUCT();
			pVK->VirtualKey = (int)pVkToWch->VirtualKey;
			pVK->Attributes = pVkToWch->Attributes;

			for (int i = 0; i < pVkToWchTbl->nModifications; ++i)
			{
				//std::wcout << "Char: " << pVkToWch->wch[i] << "\n";
				printf("Char: %d\n", pVkToWch->wch[i]);
				pVK->Characters.insert(pVK->Characters.end(), pVkToWch->wch[i]);
			}
			m_vkarray.insert(m_vkarray.end(), pVK);

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

	///*PDEADKEY64 pDEADKEY64 = KbdTables64->pDEADKEY64;
	//int count = 0;
	//while(pDEADKEY64){
	//count++;
	//pDEADKEY64++;
	//}*/
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