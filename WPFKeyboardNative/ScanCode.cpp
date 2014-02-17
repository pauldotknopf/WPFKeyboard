#include "stdafx.h"
#include "ScanCode.h"


WPFKeyboardNative::ScanCode::ScanCode(int virtualKey, int scanCode)
{
	_virtualKey = virtualKey;
	_scanCode = scanCode;
}

int WPFKeyboardNative::ScanCode::VirtualKey::get() 
{
	return _virtualKey;
}

int WPFKeyboardNative::ScanCode::Code::get() 
{
	return _scanCode;
}