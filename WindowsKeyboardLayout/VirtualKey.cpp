#include "stdafx.h"
#include "VirtualKey.h"

WPFKeyboardNative::VirtualKeyCharacter::VirtualKeyCharacter(bool isLig, bool isDeadKey, int character, array<int>^ ligs)
{
	_isLig = isLig;
	_isDeadKey = isDeadKey;
	_character = character;
	_ligs = ligs;
}

bool WPFKeyboardNative::VirtualKeyCharacter::IsLig::get()
{
	return _isLig;
}

bool WPFKeyboardNative::VirtualKeyCharacter::IsDeadKey::get()
{
	return _isDeadKey;
}

int WPFKeyboardNative::VirtualKeyCharacter::Character::get()
{
	return _character;
}

array<int>^ WPFKeyboardNative::VirtualKeyCharacter::Ligs::get()
{
	return _ligs;
}

WPFKeyboardNative::VirtualKey::VirtualKey(int virtualKey, int attributes, array<VirtualKeyCharacter^>^ characters)
{
	_attributes = attributes;
	_virtualkey = virtualKey;
	_characters = characters;
}

int WPFKeyboardNative::VirtualKey::Key::get()
{
	return _virtualkey;
}

array<WPFKeyboardNative::VirtualKeyCharacter^>^ WPFKeyboardNative::VirtualKey::Characters::get()
{
	return _characters;
}

int WPFKeyboardNative::VirtualKey::Attributes::get()
{
	return _attributes;
}
