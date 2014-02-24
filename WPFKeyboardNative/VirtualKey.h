#pragma once
using namespace System;
namespace WPFKeyboardNative 
{
	public ref class VirtualKey
	{
	private:
		int _virtualkey;
		int _attributes;
		array<int>^ _characters;
	public:
		VirtualKey(int virtualKey, int attributes, array<int>^ characters);
		property int Key { int get (); }
		property array<int>^ Characters { array<int>^ get (); }
		property int Attributes { int get (); }
	};
}