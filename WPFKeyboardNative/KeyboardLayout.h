#pragma once
#include "CharModifier.h"
#include "VirtualKey.h"
#include "ScanCode.h"
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
	public:
		KeyboardLayout(String^ keyboardLayoutDllPath);
		property List<CharModifier^>^ CharModifiers { List<CharModifier^>^ get (); };
		property List<VirtualKey^>^ VirtualKeys { List<VirtualKey^>^ get (); };
		property List<ScanCode^>^ ScanCodes { List<ScanCode^>^ get (); };
	};
}