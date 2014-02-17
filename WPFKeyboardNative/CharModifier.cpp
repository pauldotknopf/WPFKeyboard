#include "stdafx.h"
#include "CharModifier.h"


WPFKeyboardNative::CharModifier::CharModifier(int virtualKey, int modifierBits)
{
	_virtualKey = virtualKey;
	_modifierBits = modifierBits;
}

int WPFKeyboardNative::CharModifier::VirtualKey::get() 
{
	return _virtualKey;
}

int WPFKeyboardNative::CharModifier::ModifierBits::get() 
{
	return _modifierBits;
}