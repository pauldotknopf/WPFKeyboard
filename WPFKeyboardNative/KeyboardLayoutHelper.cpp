#include "stdafx.h"
#include "KeyboardLayoutHelper.h"
#include "KLL.h"

WPFKeyboardNative::KeyboardLayoutHelper::KeyboardLayoutHelper(void)
{
}

WPFKeyboardNative::KeyboardLayout^ WPFKeyboardNative::KeyboardLayoutHelper::GetLayout(System::String^ keyboardLayoutDll)
{
	CKLL *kll = new CKLL();

	System::IntPtr p = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(keyboardLayoutDll);
	char *charKeyboardLayoutDll = static_cast<char*>(p.ToPointer());
	bool result = kll->LoadDLL(charKeyboardLayoutDll);
	System::Runtime::InteropServices::Marshal::FreeHGlobal(p);

	int count = kll->GetVKCount();

	if(!result)
		throw gcnew Exception(String::Format("Unabled to load keyboard layout dll {0}.", keyboardLayoutDll));

	KeyboardLayout^ layout = gcnew KeyboardLayout(keyboardLayoutDll);

	//Loop through each VK and show the ScanCode (SC) and chars attached to that VK
	for(BYTE i=0;i < kll->GetVKCount(); i++)
	{
		layout->AddVirtualKey(i);
	}

	return layout;
}
