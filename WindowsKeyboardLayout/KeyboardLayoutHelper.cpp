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

		KeyboardLayout^ layout = gcnew KeyboardLayout(keyboardLayoutDll, kll->GetLocaleFlags());

		for(int i=0;i < kll->GetModifiersCount(); i++)
		{
			layout->CharModifiers->Add(gcnew CharModifier(kll->GetModifierAtIndex(i)->VirtualKey, kll->GetModifierAtIndex(i)->ModifierBits));
		}

		for(int x = 0; x < kll->GetModBitsSize(); x++)
		{
			layout->ModifierBits->Add(kll->GetModBitAtIndex(x));
		}

		for(int i=0;i < kll->GetVKCount(); i++)
		{
			CKLL::VK_STRUCT *vk = kll->GetVKAtIndex(i);

			array<VirtualKeyCharacter^>^ characters = gcnew array<VirtualKeyCharacter^>(vk->Characters.size());

			for(int y = 0; y <vk->Characters.size(); y++)
			{
				array<int>^ ligs = gcnew array<int>(vk->Characters[y].Ligs.size());
				for (int z = 0; z < vk->Characters[y].Ligs.size(); z++)
				{
					ligs[z] = vk->Characters[y].Ligs.at(z);
				}
				characters[y] = gcnew VirtualKeyCharacter(vk->Characters[y].IsLig, vk->Characters[y].IsDeadKey, vk->Characters[y].Character, ligs);
			}

			layout->VirtualKeys->Add(gcnew VirtualKey(vk->VirtualKey, vk->Attributes, characters));
		}

		for(int i=0;i < kll->GetScanCodesCount(); i++)
		{
			layout->ScanCodes->Add(gcnew ScanCode(kll->GetScanCodeAtIndex(i)->VirtualKey, kll->GetScanCodeAtIndex(i)->ScanCode, kll->GetScanCodeAtIndex(i)->E0Set, kll->GetScanCodeAtIndex(i)->E1Set));
		}

		for(int i=0;i < kll->GetScanCodeTextCount(); i++)
		{
			layout->CodeText->Add(gcnew ScanCodeText(kll->GetScanCodeTextAtIndex(i)->ScanCode, gcnew String(kll->GetScanCodeTextAtIndex(i)->Text)));
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
