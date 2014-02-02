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

void WPFKeyboardNative::KeyboardLayout::AddVirtualKey(int virtualKey, array<unsigned int>^ scanCodes, array<String^>^ characters)
{
	_virtualkeys->Add(gcnew VirtualKey(virtualKey, scanCodes, characters));
}