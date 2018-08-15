using System;
using System.Runtime.InteropServices;

namespace Console.Interop.Internal {
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct COORD {
        public short X;
        public short Y;
    }
}