#include "stdafx.h"
#include "KeyboardLayoutHelper.h"
#include "KLL.h"

WPFKeyboardNative::KeyboardLayoutHelper::KeyboardLayoutHelper(void)
{
}

WPFKeyboardNative::KeyboardLayout^ WPFKeyboardNative::KeyboardLayoutHelper::GetLayout(System::String^ keyboardLayoutDll)
{
	CKLL *kll = nullptr;
	try{

		kll = new CKLL();

		System::IntPtr p = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(keyboardLayoutDll);
		char *charKeyboardLayoutDll = static_cast<char*>(p.ToPointer());
		bool result = kll->LoadDLL(charKeyboardLayoutDll);
		System::Runtime::InteropServices::Marshal::FreeHGlobal(p);

		if(!result)
			throw gcnew Exception(String::Format("Unabled to load keyboard layout dll {0}.", keyboardLayoutDll));

		KeyboardLayout^ layout = gcnew KeyboardLayout(keyboardLayoutDll);

		for(int i=0;i < kll->GetModifiersCount(); i++)
		{
			layout->CharModifiers->Add(gcnew CharModifier(kll->GetModifierAtIndex(i)->VirtualKey, kll->GetModifierAtIndex(i)->ModifierBits));
		}

		for(BYTE i=0;i < kll->GetVKCount(); i++)
		{
			CKLL::VK_STRUCT *vk = kll->GetVKAtIndex(i);

			array<String^>^ characters = gcnew array<String^>(vk->characters.size());

			for(int y = 0; y <vk->characters.size(); y++)
			{
				characters[y] = gcnew String(vk->characters[y], 1);
			}

			layout->VirtualKeys->Add(gcnew VirtualKey(vk->nVK, vk->attributes, characters));
		}

		for(BYTE i=0;i < kll->GetScanCodesCount(); i++)
		{
			layout->ScanCodes->Add(gcnew ScanCode(kll->GetScanCodeAtIndex(i)->nVK, kll->GetScanCodeAtIndex(i)->scanCode));
		}

		return layout;
	} finally {
		if(kll != nullptr)
		{
			delete kll;
			kll = nullptr;
		}
	}

}
