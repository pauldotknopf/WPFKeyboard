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
		CKLL::VK_STRUCT *vk = kll->GetVKAtIndex(i);

		array<unsigned int>^ scanCodes = gcnew array<unsigned int>(vk->aSC.size());

		int size = vk->aSC.size();

		for(int y = 0; y <vk->aSC.size(); y++)
		{
			scanCodes[y] = (vk->aSC[y]);
		}

		array<String^>^ characters = gcnew array<String^>(vk->aChar.size());

		for(int y = 0; y <vk->aChar.size(); y++)
		{
			characters[y] = gcnew String(vk->aChar[y], 1);
		}

		layout->VirtualKeys->Add(gcnew VirtualKey(vk->nVK, vk->attributes, characters));
	}

	return layout;
}
