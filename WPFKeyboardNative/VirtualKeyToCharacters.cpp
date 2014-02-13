#include "stdafx.h"
#include "VirtualKeyToCharacters.h"


WPFKeyboardNative::VirtualKeyToCharacters::VirtualKeyToCharacters(int virtualKey, int attributes, array<String^>^ characters)
{
	_attributes = attributes;
	_virtualkey = virtualKey;
	_characters = characters;
}

int WPFKeyboardNative::VirtualKeyToCharacters::Key::get()
{
	return _virtualkey;
}

array<String^>^ WPFKeyboardNative::VirtualKeyToCharacters::Characters::get()
{
	return _characters;
}

int WPFKeyboardNative::VirtualKeyToCharacters::Attributes::get()
{
	return _attributes;
}
