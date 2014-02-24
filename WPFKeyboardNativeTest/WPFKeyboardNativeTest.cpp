// WPFKeyboardNativeTest.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "KLL.h"

int _tmain(int argc, _TCHAR* argv[])
{
	CKLL *kll = new CKLL();
	// KBDGR - german
	// KBDUSA - USA
	kll->LoadDLL("C:\\Windows\\SysWOW64\\KBDGR.DLL");
	Sleep(1000000);
	return 0;
}

