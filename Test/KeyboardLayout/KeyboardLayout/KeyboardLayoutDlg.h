
// KeyboardLayoutDlg.h : header file
//

#pragma once
#include "afxcmn.h"
#include "KLL.h"
#include "afxwin.h"
#include "KeyboardScanCode.h"


// CKeyboardLayoutDlg dialog
class CKeyboardLayoutDlg : public CDialogEx
{
// Construction
public:
	CKeyboardLayoutDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_KEYBOARDLAYOUT_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	//Sets welcome info and listview columns
	void SetListView();

	// List for showing chars and VK related
	CListCtrl m_list;

	//The Keyboard Layout Loader (KLL) manage 32/64-bit loading between
	CKLL m_kll;

	//Scan code presentation to comparison of the layout in "real life"
	CKeyboardScanCode *pScanCodeDlg;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedCancel();
	afx_msg void OnBnClickedLoadkbl();
	CStatic m_system;
	CStatic m_dll;
	afx_msg void OnBnClickedShowscancode();
	CButton m_scancodes;
};
