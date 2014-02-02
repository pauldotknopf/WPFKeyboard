#include "StdAfx.h"
#include "KLL.h"


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
	this->UnloadDLL();
}
//////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////
// Load DLL and UnloadDLL functions
// Manages 32 and 64-bit system, when running a 32-bit app
BOOL CKLL::LoadDLL( CString sKeyboardDll )
{
	//Unload if loaded...
	if(hHandle)
		this->UnloadDLL();

	//Load the dll as usual
	TRACE(L"Loading Keyboard DLL %ws\n", sKeyboardDll);
	hHandle = LoadLibrary(sKeyboardDll);
	if (!hHandle)
	{
		AfxMessageBox(L"Failed to load dll " + sKeyboardDll);
		this->UnloadDLL();
		return FALSE;
	}

	//Get the Keyboard import function
	pfnKbdLayerDescriptor = (PFN_KBDLAYERDESCRIPTOR)GetProcAddress(hHandle, "KbdLayerDescriptor");

	//Return if error
	if(!pfnKbdLayerDescriptor)
	{
		AfxMessageBox(L"Could not load kbdLayerDescriptor, is it a real keyboard layout file?");
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

		//Clear, then fill the array
		this->ClearVKChar();
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

		//Clear, then fill the array
		this->ClearVKChar();
		this->Fill64();
	}

	return TRUE;
}

void CKLL::UnloadDLL()
{
	//Free DLL, if loaded
	if(hHandle != NULL)
	{
		TRACE(L"Free handle of keyboard dll...\n");
		FreeLibrary(hHandle);
		hHandle = NULL;
		KbdTables = NULL;
		KbdTables64 = NULL;
	}
}
//////////////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////////////
// Fill functions add all the chars based on the VK to an array
void CKLL::Fill32()
{
	//If KbdTables aren't set, just silent return
	if(!KbdTables)
		return;

	//Variable to handle char
	CString sChar;

	//Fill all the SC into VKs array
	for (int i = 0; i < KbdTables->bMaxVSCtoVK; i++)
		this->AddVKSC(KbdTables->pusVSCtoVK[i], i);

	//Handle all the chars with modifieres
	PVK_TO_WCHAR_TABLE pVkToWchTbl = KbdTables->pVkToWcharTable;
	while (pVkToWchTbl->pVkToWchars)
	{
		PVK_TO_WCHARS1 pVkToWch = pVkToWchTbl->pVkToWchars;
		while (pVkToWch->VirtualKey)
		{
			for (int i = 0; i < pVkToWchTbl->nModifications; ++i)
			{
				sChar.Format(L"%wc (%.4x)", pVkToWch->wch[i], pVkToWch->wch[i]);
				this->AddVKChar(pVkToWch->VirtualKey, sChar);
			}

			pVkToWch = (PVK_TO_WCHARS1)(((PBYTE)pVkToWch) + pVkToWchTbl->cbSize);
		}
		++pVkToWchTbl;
	}
}

void CKLL::Fill64()
{
	//If KbdTables64 aren't set, just silent return
	if(!KbdTables64)
		return;

	//Variable to handle char
	CString sChar;

	//Fill all the SC into VKs array
	for (int i = 0; i < KbdTables64->bMaxVSCtoVK; i++)
		this->AddVKSC(KbdTables64->pusVSCtoVK[i], i);

	//Handle all the chars with modifieres
	PVK_TO_WCHAR_TABLE64 pVkToWchTbl = KbdTables64->pVkToWcharTable;
	while (pVkToWchTbl->pVkToWchars)
	{
		PVK_TO_WCHARS641 pVkToWch = pVkToWchTbl->pVkToWchars;
		while (pVkToWch->VirtualKey)
		{
			for (int i = 0; i < pVkToWchTbl->nModifications; ++i)
			{
				sChar.Format(L"%wc (%.4x)", pVkToWch->wch[i], pVkToWch->wch[i]);
				this->AddVKChar(pVkToWch->VirtualKey, sChar);
			}

			pVkToWch = (PVK_TO_WCHARS641)(((PBYTE)pVkToWch) + pVkToWchTbl->cbSize);
		}
		++pVkToWchTbl;
	}
}
//////////////////////////////////////////////////////////////////////////
// Returns the size of our array (actually the count of VKs)
USHORT CKLL::GetVKCount()
{
	return m_vkarray.GetCount();
}
//////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////
// Get char returns the char(s) defined on that iVK (ignore keyboard state) 
CString CKLL::GetChar( USHORT iVK )
{
	//Return nothing, if we past the count
	if(iVK >= this->GetVKCount())
		return L"";

	//Return variable
	CString sText = L"";

	//Return our chars
	VK_STRUCT *pVK = (VK_STRUCT*)m_vkarray.GetAt(iVK);
	for(int i=0;i<pVK->aChar.GetCount();i++)
		sText = sText+pVK->aChar.GetAt(i)+L", ";

	//Remove the last ", "
	sText = sText.Left(sText.GetLength() - 2);
	return sText;
}

//Get the scan code(s) for this virtual key
CString CKLL::GetSC( USHORT iVK )
{
	//Return nothing, if we past the count
	if(iVK >= this->GetVKCount())
		return L"";

	//Return variable
	CString sText = L"", sSC;

	//Return our chars
	VK_STRUCT *pVK = (VK_STRUCT*)m_vkarray.GetAt(iVK);
	for(int i=0;i<pVK->aSC.GetCount();i++)
	{
		sSC.Format(L"%x, ", pVK->aSC.GetAt(i));
		sText = sText+sSC;
	}

	//Remove the last ", "
	sText = sText.Left(sText.GetLength() - 2);
	return sText;
}

//////////////////////////////////////////////////////////////////////////
// Array functions
// Just a simple store array for VK+SC+CHARS and it doesn't care about modifiers
void CKLL::AddVKChar( USHORT nVK, CString wChar )
{
	//Check if the VK already exists, add char if so
	INT_PTR iVKArray = this->VKExist(nVK);
	if(iVKArray!=-1)
	{
		((VK_STRUCT *)m_vkarray.GetAt(iVKArray))->aChar.Add(wChar);
		return;
	}

	//Create a new item and store into the array
	VK_STRUCT *pVK = new VK_STRUCT();
	pVK->nVK = nVK;
	pVK->aChar.Add(wChar);
	m_vkarray.Add(pVK);
}


void CKLL::AddVKSC( USHORT nVK, USHORT nSC )
{
	//Check if the VK already exists, add SC if so
	INT_PTR iVKArray = this->VKExist(nVK);
	if(iVKArray!=-1)
	{
		((VK_STRUCT *)m_vkarray.GetAt(iVKArray))->aSC.Add(nSC);
		return;
	}

	//Create a new item and store into the array
	VK_STRUCT *pVK = new VK_STRUCT();
	pVK->nVK = nVK;
	pVK->aSC.Add(nSC);
	m_vkarray.Add(pVK);
}

//Returns the number of where the item exists
int CKLL::VKExist( USHORT nVK )
{
	//Check if the VK already exists, add char if so
	for(INT_PTR i = 0;i<m_vkarray.GetCount();i++)
	{
		VK_STRUCT *pVK = (VK_STRUCT*)m_vkarray.GetAt(i);
		if(pVK->nVK == nVK)
			return i;
	}

	//Not found
	return -1;
}

void CKLL::ClearVKChar()
{
	for(INT_PTR i = 0;i<m_vkarray.GetCount();i++)
	{
		VK_STRUCT *pVK = (VK_STRUCT*)m_vkarray.GetAt(i);
		delete pVK;
	}
	m_vkarray.RemoveAll();
}
//////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////
//Detection of x64 or not (from http://blogs.msdn.com/b/oldnewthing/archive/2005/02/01/364563.aspx )
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

	fnIsWow64Process = (LPFN_ISWOW64PROCESS) GetProcAddress(GetModuleHandle(_T("kernel32")),"IsWow64Process");
	if(NULL != fnIsWow64Process)
	{
		return fnIsWow64Process(GetCurrentProcess(),&f64) && f64;
	}
	return FALSE;
#else
	return FALSE; // Win64 does not support Win16
#endif
}

//////////////////////////////////////////////////////////////////////////