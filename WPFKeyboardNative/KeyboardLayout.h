#pragma once
using namespace System;
using namespace System::Collections::Generic;
namespace WPFKeyboardNative 
{
	public ref class KeyboardLayout
	{
	private:
		//List<VirtualKeyToCharacters^>^ _virtualkeys;
	public:
		KeyboardLayout(String^ keyboardLayoutDllPath);
		//property List<VirtualKeyToCharacters^>^ VirtualKeysToCharacters { List<VirtualKeyToCharacters^>^ get (); };
	};
}