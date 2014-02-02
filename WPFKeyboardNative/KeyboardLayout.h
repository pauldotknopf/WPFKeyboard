#pragma once
#include "VirtualKey.h"
using namespace System;
using namespace System::Collections::Generic;
namespace WPFKeyboardNative 
{
	public ref class KeyboardLayout
	{
	private:
		List<VirtualKey^>^ _virtualkeys;
	public:
		KeyboardLayout(String^ keyboardLayoutDllPath);
		property List<VirtualKey^>^ VirtualKeys { List<VirtualKey^>^ get (); };
		void AddVirtualKey(int virtualKey, array<unsigned int>^ scanCodes, array<String^>^ characters);
	};
}