#include "stdafx.h"
#include "ScanCode.h"


WPFKeyboardNative::ScanCode::ScanCode(int virtualKey, int scanCode, bool e0Set, bool e1Set)
{
	_virtualKey = virtualKey;
	_scanCode = scanCode;
	_e0Set = e0Set;
	_e1Set = e1Set;
}

int WPFKeyboardNative::ScanCode::VirtualKey::get() 
{
	return _virtualKey;
}

int WPFKeyboardNative::ScanCode::Code::get() 
{
	return _scanCode;
}

bool WPFKeyboardNative::ScanCode::E0Set::get() 
{
	return _e0Set;
}

bool WPFKeyboardNative::ScanCode::E1Set::get() 
{
	return _e1Set;
}