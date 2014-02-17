#pragma once
namespace WPFKeyboardNative 
{
	public ref class ScanCode
	{
	private:
		int _virtualKey;
		int _scanCode;
	public:
		ScanCode(int virtualKey, int scanCode);
		property int VirtualKey { int get (); };
		property int Code { int get (); };
	};
}