#include "stdafx.h"
#include "KeyboardLayout.h"


WPFKeyboardNative::KeyboardLayout::KeyboardLayout(String^ keyboardLayoutDllPath)
{
	_virtualkeys = gcnew List<WPFKeyboardNative::VirtualKey^>();
}

List<WPFKeyboardNative::VirtualKey^>^ WPFKeyboardNative::KeyboardLayout::VirtualKeys::get() 
{
	return _virtualkeys;
}