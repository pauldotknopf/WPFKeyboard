#include "stdafx.h"
#include "KeyboardLayout.h"

WPFKeyboardNative::KeyboardLayout::KeyboardLayout(String^ keyboardLayoutDllPath)
{
	_charModifiers = gcnew List<WPFKeyboardNative::CharModifier^>();
	_virtualKeys = gcnew List<WPFKeyboardNative::VirtualKey^>();
	_scanCodes = gcnew List<WPFKeyboardNative::ScanCode^>();
}

List<WPFKeyboardNative::CharModifier^>^ WPFKeyboardNative::KeyboardLayout::CharModifiers::get()
{
	return _charModifiers;
}

List<WPFKeyboardNative::VirtualKey^>^ WPFKeyboardNative::KeyboardLayout::VirtualKeys::get()
{
	return _virtualKeys;
}

List<WPFKeyboardNative::ScanCode^>^ WPFKeyboardNative::KeyboardLayout::ScanCodes::get()
{
	return _scanCodes;
}