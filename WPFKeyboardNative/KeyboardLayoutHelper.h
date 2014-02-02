#pragma once
#include "KeyboardLayout.h"
using namespace System;
namespace WPFKeyboardNative 
{
	public ref class KeyboardLayoutHelper
	{
	public:
		KeyboardLayoutHelper(void);
		static KeyboardLayout^ GetLayout(String^ keyboardLayoutDll);
	};
}