#include "stdafx.h"
#include "VirtualKey.h"


WPFKeyboardNative::VirtualKey::VirtualKey(int virtualKey, array<unsigned int>^ scanCodes, array<String^>^ characters)
{
	_virtualkey = virtualKey;
	_scanCodes = scanCodes;
	_characters = characters;
}

int WPFKeyboardNative::VirtualKey::Key::get()
{
	return _virtualkey;
}

array<unsigned int>^ WPFKeyboardNative::VirtualKey::ScanCodes::get()
{
	return _scanCodes;
}

array<String^>^ WPFKeyboardNative::VirtualKey::Characters::get()
{
	return _characters;
}
