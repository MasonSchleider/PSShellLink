using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Console.Interop.Internal {
    [ComImport()
    , InterfaceType(ComInterfaceType.InterfaceIsIUnknown)
    , Guid("000214F9-0000-0000-C000-000000000046")]
    internal interface IShellLink {
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out WIN32_FIND_DATAW pfd, int fFlags);
    }
}