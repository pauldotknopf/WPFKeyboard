#pragma once
#include "CharModifier.h"
#include "VirtualKey.h"
#include "ScanCode.h"
#include "ScanCodeText.h"
using namespace System;
using namespace System::Collections::Generic;
namespace WPFKeyboardNative 
{
	public ref class KeyboardLayout
	{
	private:
		List<CharModifier^>^ _charModifiers;
		List<VirtualKey^>^ _virtualKeys;
		List<ScanCode^>^ _scanCodes;
		List<ScanCodeText^>^ _scanCodeText;
	public:
		KeyboardLayout(String^ keyboardLayoutDllPath);
		property List<CharModifier^>^ CharModifiers { List<CharModifier^>^ get (); };
		property List<VirtualKey^>^ VirtualKeys { List<VirtualKey^>^ get (); };
		property List<ScanCode^>^ ScanCodes { List<ScanCode^>^ get (); };
		property List<ScanCodeText^>^ CodeText { List<ScanCodeText^>^ get (); };
	};
}