#pragma once


// CKeyboardScanCode dialog

class CKeyboardScanCode : public CDialogEx
{
	DECLARE_DYNAMIC(CKeyboardScanCode)

public:
	CKeyboardScanCode(CWnd* pParent = NULL);   // standard constructor
	virtual ~CKeyboardScanCode();

// Dialog Data
	enum { IDD = IDD_KEYBOARDLAYOUT };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedCancel();
};
