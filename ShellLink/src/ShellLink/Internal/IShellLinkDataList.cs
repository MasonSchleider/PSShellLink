using System;
using System.Runtime.InteropServices;

namespace Console.Interop.Internal {
    [ComImport()
    , InterfaceType(ComInterfaceType.InterfaceIsIUnknown)
    , Guid("45E2B4AE-B1C3-11D0-B92F-00A0C90312E1")]
    internal interface IShellLinkDataList {
        void AddDataBlock(IntPtr pDataBlock);
        [PreserveSig()]
        int CopyDataBlock(uint dwSig, out IntPtr ppDataBlock);
        void RemoveDataBlock(uint dwSig);
        void GetFlags(out uint dwFlags);
        void SetFlags(uint dwFlags);
    }
}