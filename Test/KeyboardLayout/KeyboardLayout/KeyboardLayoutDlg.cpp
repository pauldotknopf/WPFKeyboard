
// KeyboardLayoutDlg.cpp : implementation file
//

#include "stdafx.h"
#include "KeyboardLayout.h"
#include "KeyboardLayoutDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CKeyboardLayoutDlg dialog

CKeyboardLayoutDlg::CKeyboardLayoutDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CKeyboardLayoutDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CKeyboardLayoutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST, m_list);
	DDX_Control(pDX, IDC_X64, m_system);
	DDX_Control(pDX, IDC_DLL, m_dll);
	DDX_Control(pDX, IDC_SHOWSCANCODE, m_scancodes);
}

BEGIN_MESSAGE_MAP(CKeyboardLayoutDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDOK, &CKeyboardLayoutDlg::OnBnClickedOk)
	ON_BN_CLICKED(IDCANCEL, &CKeyboardLayoutDlg::OnBnClickedCancel)
	ON_BN_CLICKED(IDC_LOADKBL, &CKeyboardLayoutDlg::OnBnClickedLoadkbl)
	ON_BN_CLICKED(IDC_SHOWSCANCODE, &CKeyboardLayoutDlg::OnBnClickedShowscancode)
END_MESSAGE_MAP()


// CKeyboardLayoutDlg message handlers

BOOL CKeyboardLayoutDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	//Init the window
	this->SetListView();

	//Create the scan code dialog for comparison
	pScanCodeDlg = new CKeyboardScanCode(this);
	pScanCodeDlg->Create(IDD_KEYBOARDLAYOUT, this);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CKeyboardLayoutDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CKeyboardLayoutDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CKeyboardLayoutDlg::SetListView()
{
	//Insert header and resize colums
	m_list.InsertColumn(0, L"Virtual Key");
	m_list.InsertColumn(1, L"Scan code");
	m_list.InsertColumn(2, L"Char");
	CRect m_rect;
	m_list.GetClientRect(m_rect);
	m_list.SetColumnWidth(0, m_rect.Width()/5);
	m_list.SetColumnWidth(1, m_rect.Width()/5);
	m_list.SetColumnWidth(2, (m_rect.Width()/5)*3);
	m_list.SetExtendedStyle(LVS_EX_FULLROWSELECT);

	//Welcome info
	m_list.InsertItem(0, L"How to start");
	m_list.SetItemText(0, 2, L"Press the \"Load Keyboard\" and select a KBD*.dll file");

	//Set text based on 64-bit or 32-bit
	CString sText;
	int nSystem = 32;
	if(m_kll.Is64BitWindows())
		nSystem = 64;
	sText.Format(L"32-bit application running on a %i-bit system", nSystem);
	m_system.SetWindowText(sText);
}

void CKeyboardLayoutDlg::OnBnClickedOk()
{
}


void CKeyboardLayoutDlg::OnBnClickedCancel()
{
	//Delete the scancode window
	pScanCodeDlg->DestroyWindow();
	delete pScanCodeDlg;
	pScanCodeDlg = NULL;
	CDialogEx::OnCancel();
}


void CKeyboardLayoutDlg::OnBnClickedLoadkbl()
{
	//Get the folder where kbd*.dll files are located
	TCHAR sFolder[MAX_PATH];
	UINT nSize = GetSystemWow64Directory(sFolder, MAX_PATH);
	
	//Get the 32-bit system drive, since nSize = 0 equal no 64-bit system path
	if(nSize == 0)
		if (!SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, sFolder)))
			return;

	//Now prompt for a keyboard-dll-file
	OPENFILENAME ofn;
	WCHAR fileName[MAX_PATH] = L"";
	ZeroMemory(&ofn, sizeof(ofn));
	ofn.lStructSize = sizeof(OPENFILENAME);
	ofn.lpstrInitialDir = sFolder;
	ofn.hwndOwner = this->GetSafeHwnd();
	ofn.lpstrFilter = L"Keyboard DLLs\0kbd*.dll\0\0";
	ofn.lpstrFile = fileName;
	ofn.nMaxFile = MAX_PATH;
	ofn.Flags = OFN_EXPLORER | OFN_FILEMUSTEXIST | OFN_HIDEREADONLY | OFN_NOCHANGEDIR;
	ofn.lpstrDefExt = L"";
		
	//Only load DLL if the filename was set
	if ( GetOpenFileName(&ofn) )
	{
		//Load the dll 
		if(m_kll.LoadDLL(fileName))
		{
			CString sDllLoad(fileName);
			sDllLoad.Format(L"Loaded %s", sDllLoad.Right(sDllLoad.GetLength()-sDllLoad.ReverseFind('\\')-1));
			m_dll.SetWindowText(sDllLoad);

			//Clear old list
			m_list.DeleteAllItems();

			//Loop through each VK and show the ScanCode (SC) and chars attached to that VK
			for(BYTE i=0;i < m_kll.GetVKCount(); i++)
			{
				sDllLoad.Format(L"%i", i);
				m_list.InsertItem(i, sDllLoad);
				m_list.SetItemText(i, 1,  m_kll.GetSC(i));
				m_list.SetItemText(i, 2,  m_kll.GetChar(i));
			}
		}
	}
}

void CKeyboardLayoutDlg::OnBnClickedShowscancode()
{
	// TODO: Add your control notification handler code here
	CString sText = L" Scan Codes";

	if(pScanCodeDlg->IsWindowVisible())
		sText = L"Show" + sText;
	else
		sText = L"Hide" + sText;
	
	//Set text to button and show/hide window
	pScanCodeDlg->ShowWindow(!pScanCodeDlg->IsWindowVisible());	
	m_scancodes.SetWindowText(sText);
}
