#pragma once
namespace WPFKeyboardNative 
{
	public ref class CharModifier
	{
	private:
		int _virtualKey;
		int _modifierBits;
	public:
		CharModifier(int virtualKey, int modifierBits);
		property int VirtualKey { int get (); };
		property int ModifierBits { int get (); };
	};
}