#pragma once
using namespace System;
namespace WPFKeyboardNative 
{
	public ref class VirtualKey
	{
	private:
		int _virtualkey;
		int _attributes;
		array<String^>^ _characters;
	public:
		VirtualKey(int virtualKey, int attributes, array<String^>^ characters);
		property int Key { int get (); }
		property array<String^>^ Characters { array<String^>^ get (); }
		property int Attributes { int get (); }
	};
}