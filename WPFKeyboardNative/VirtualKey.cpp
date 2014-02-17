#include "stdafx.h"
#include "VirtualKey.h"

WPFKeyboardNative::VirtualKey::VirtualKey(int virtualKey, int attributes, array<String^>^ characters)
{
	_attributes = attributes;
	_virtualkey = virtualKey;
	_characters = characters;
}

int WPFKeyboardNative::VirtualKey::Key::get()
{
	return _virtualkey;
}

array<String^>^ WPFKeyboardNative::VirtualKey::Characters::get()
{
	return _characters;
}

int WPFKeyboardNative::VirtualKey::Attributes::get()
{
	return _attributes;
}
