using System;
using System.Runtime.InteropServices;

namespace Console.Interop.Internal {
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DATABLOCK_HEADER {
        public uint cbSize;
        public uint dwSignature;
    }
}
