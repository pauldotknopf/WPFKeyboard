#include "stdafx.h"
#include "KeyboardLayout.h"


WPFKeyboardNative::KeyboardLayout::KeyboardLayout(String^ keyboardLayoutDllPath)
{
	_virtualkeys = gcnew List<WPFKeyboardNative::VirtualKey^>();
}

List<WPFKeyboardNative::VirtualKey^>^ WPFKeyboardNative::KeyboardLayout::VirtualKeys::get() 
{
	return nullptr;
}

void WPFKeyboardNative::KeyboardLayout::AddVirtualKey(int virtualKey)
{
	_virtualkeys->Add(gcnew VirtualKey(virtualKey));
}