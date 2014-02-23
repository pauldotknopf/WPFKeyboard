#pragma once
namespace WPFKeyboardNative 
{
	public ref class ScanCode
	{
	private:
		int _virtualKey;
		int _scanCode;
		bool _e0Set;
		bool _e1Set;
	public:
		ScanCode(int virtualKey, int scanCode, bool e0Set, bool e1Set);
		property int VirtualKey { int get (); };
		property int Code { int get (); };
		property bool E0Set { bool get (); };
		property bool E1Set { bool get (); };
	};
}