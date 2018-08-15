using System;
using System.Runtime.InteropServices;

namespace Console.Interop.Internal {
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct NT_CONSOLE_PROPS {
        public DATABLOCK_HEADER dbh;
        public ushort wFillAttribute;
        public ushort wPopupFillAttribute;
        public COORD dwScreenBufferSize;
        public COORD dwWindowSize;
        public COORD dwWindowOrigin;
        public uint nFont;
        public uint nInputBufferSize;
        public COORD dwFontSize;
        public uint uFontFamily;
        public uint uFontWeight;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FaceName;
        public uint uCursorSize;
        public bool bFullScreen;
        public bool bQuickEdit;
        public bool bInsertMode;
        public bool bAutoPosition;
        public uint uHistoryBufferSize;
        public uint uNumberOfHistoryBuffers;
        public bool bHistoryNoDup;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public uint[] ColorTable;
    }
}