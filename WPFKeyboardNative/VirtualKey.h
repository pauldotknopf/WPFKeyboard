#pragma once
using namespace System;
namespace WPFKeyboardNative 
{
	public ref class VirtualKey
	{
	private:
		int _virtualkey;
		array<unsigned int>^ _scanCodes; 
		array<String^>^ _characters;
	public:
		VirtualKey(int virtualKey, array<unsigned int>^ scanCodes, array<String^>^ characters);
		property int Key { int get (); }
		property array<unsigned int>^ ScanCodes { array<unsigned int>^ get (); }
		property array<String^>^ Characters { array<String^>^ get (); }
	};
}
