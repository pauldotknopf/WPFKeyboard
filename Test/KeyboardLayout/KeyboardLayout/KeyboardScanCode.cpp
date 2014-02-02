// KeyboardScanCode.cpp : implementation file
//

#include "stdafx.h"
#include "KeyboardLayout.h"
#include "KeyboardScanCode.h"
#include "afxdialogex.h"
#include "KeyboardLayoutDlg.h"


// CKeyboardScanCode dialog

IMPLEMENT_DYNAMIC(CKeyboardScanCode, CDialogEx)

CKeyboardScanCode::CKeyboardScanCode(CWnd* pParent /*=NULL*/)
	: CDialogEx(CKeyboardScanCode::IDD, pParent)
{
}

CKeyboardScanCode::~CKeyboardScanCode()
{
}

void CKeyboardScanCode::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CKeyboardScanCode, CDialogEx)
	ON_BN_CLICKED(IDOK, &CKeyboardScanCode::OnBnClickedOk)
	ON_BN_CLICKED(IDCANCEL, &CKeyboardScanCode::OnBnClickedCancel)
END_MESSAGE_MAP()

// CKeyboardScanCode message handlers


void CKeyboardScanCode::OnBnClickedOk()
{
}

void CKeyboardScanCode::OnBnClickedCancel()
{
	CKeyboardLayoutDlg *pDlgParent = (CKeyboardLayoutDlg*)this->GetParent();
	pDlgParent->OnBnClickedShowscancode();
}
