#pragma once
using namespace System;
namespace WPFKeyboardNative 
{
	public ref class VirtualKey
	{
	public:
		VirtualKey(int virtualKey);
		property int Key { int get (); }
	};
}
