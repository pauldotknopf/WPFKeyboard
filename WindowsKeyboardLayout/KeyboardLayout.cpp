#include "stdafx.h"
#include "KeyboardLayout.h"

WPFKeyboardNative::KeyboardLayout::KeyboardLayout(String^ keyboardLayoutDllPath, int localeFlags)
{
	_charModifiers = gcnew List<WPFKeyboardNative::CharModifier^>();
	_modifierBits = gcnew List<int>();
	_virtualKeys = gcnew List<WPFKeyboardNative::VirtualKey^>();
	_scanCodes = gcnew List<WPFKeyboardNative::ScanCode^>();
	_scanCodeText = gcnew List<WPFKeyboardNative::ScanCodeText^>();
	_localeFlags = localeFlags;
}

List<WPFKeyboardNative::CharModifier^>^ WPFKeyboardNative::KeyboardLayout::CharModifiers::get()
{
	return _charModifiers;
}

List<int>^ WPFKeyboardNative::KeyboardLayout::ModifierBits::get()
{
	return _modifierBits;
}

List<WPFKeyboardNative::VirtualKey^>^ WPFKeyboardNative::KeyboardLayout::VirtualKeys::get()
{
	return _virtualKeys;
}

List<WPFKeyboardNative::ScanCode^>^ WPFKeyboardNative::KeyboardLayout::ScanCodes::get()
{
	return _scanCodes;
}

List<WPFKeyboardNative::ScanCodeText^>^ WPFKeyboardNative::KeyboardLayout::CodeText::get()
{
	return _scanCodeText;
}

int WPFKeyboardNative::KeyboardLayout::LocaleFlags::get()
{
	return _localeFlags;
}