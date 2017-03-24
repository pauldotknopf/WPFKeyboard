#pragma once
using namespace System;
namespace WPFKeyboardNative 
{
	public ref class VirtualKeyCharacter
	{
	private:
		bool _isLig;
		int _character;
		array<int>^ _ligs;
	public:
		VirtualKeyCharacter(bool isLog, int character, array<int>^ ligs);
		property bool IsLig { bool get(); }
		property int Character { int get(); }
		property array<int>^ Ligs { array<int>^ get(); }
	};

	public ref class VirtualKey
	{
	private:
		int _virtualkey;
		int _attributes;
		array<VirtualKeyCharacter^>^ _characters;
	public:
		VirtualKey(int virtualKey, int attributes, array<VirtualKeyCharacter^>^ characters);
		property int Key { int get (); }
		property array<VirtualKeyCharacter^>^ Characters { array<VirtualKeyCharacter^>^ get (); }
		property int Attributes { int get (); }
	};
}