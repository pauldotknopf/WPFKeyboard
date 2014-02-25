#include "stdafx.h"
#include "ScanCodeText.h"


WPFKeyboardNative::ScanCodeText::ScanCodeText(int scanCode, String^ text)
{
	_scanCode = scanCode;
	_text = text;
}

int WPFKeyboardNative::ScanCodeText::ScanCode::get() 
{
	return _scanCode;
}

String^ WPFKeyboardNative::ScanCodeText::Text::get() 
{
	return _text;
}