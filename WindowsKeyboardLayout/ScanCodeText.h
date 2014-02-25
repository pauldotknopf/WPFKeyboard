#pragma once
using namespace System;
namespace WPFKeyboardNative 
{
	public ref class ScanCodeText
	{
	private:
		int _scanCode;
		String^ _text;
	public:
		ScanCodeText(int scanCode, String^ text);
		property int ScanCode { int get (); };
		property String^ Text { String^ get (); };
	};
}