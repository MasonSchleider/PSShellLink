using Console.Interop.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Console.Interop {
    public class ShellLink : IDisposable {
        private IPersistFile _handle;

        private const int MAX_PATH = 260;
        private const int SLGP_RAWPATH = 0x00000004;
        private const int STGM_DEFAULT = 0x00000000;

        private ShellLink() {
            _handle = new ShellLinkCoClass() as IPersistFile;
            ConsoleProperties = new ConsoleProperties();
        }

        public ShellLink(string path) : this(path, STGM_DEFAULT) { }

        public ShellLink(string path, int mode) : this() {
            Load(path, mode);
        }
        
        public void Dispose() {
            Marshal.ReleaseComObject(_handle);
            _handle = null;

            GC.SuppressFinalize(this);
        }

        ~ShellLink() {
            Dispose();
        }
        
        private void Load(string path, int mode) {
            _handle.Load(path, mode);

            if (HasConsoleProperties)
                ReadConsoleProperties();
        }

        public void Save() {
            _handle.Save(null, true);
        }

        public string Target {
            get {
                StringBuilder stringBuilder = new StringBuilder(MAX_PATH);
                WIN32_FIND_DATAW findData;
                (_handle as IShellLink).GetPath(stringBuilder, stringBuilder.Capacity, out findData, SLGP_RAWPATH);
                return stringBuilder.ToString();
            }
        }

        public bool HasDataBlock(uint signature) {
            IntPtr ppDataBlock;
            int hResult = (_handle as IShellLinkDataList).CopyDataBlock(signature, out ppDataBlock);
            if (hResult != 0) {
                return false;
            }

            Marshal.FreeHGlobal(ppDataBlock);
            return true;
        }

        public void RemoveDataBlock(uint signature) {
            switch (signature) {
                case ConsoleProperties.SIGNATURE:
                    (_handle as IShellLinkDataList).RemoveDataBlock(signature);
                    break;
                default:
                    throw new ArgumentException("Data block signature is invalid.");
            }
        }

        public ConsoleProperties ConsoleProperties { get; private set; }

        public bool HasConsoleProperties {
            get {
                return HasDataBlock(ConsoleProperties.SIGNATURE);
            }
        }

        public bool ReadConsoleProperties() {
            IntPtr ppConsoleProperties;
            int hResult = (_handle as IShellLinkDataList).CopyDataBlock(ConsoleProperties.SIGNATURE, out ppConsoleProperties);
            if (hResult != 0) {
                return false;
            }

            NT_CONSOLE_PROPS _ntConsoleProps = (NT_CONSOLE_PROPS)Marshal.PtrToStructure(ppConsoleProperties, typeof(NT_CONSOLE_PROPS));
            Marshal.FreeHGlobal(ppConsoleProperties);

            ConsoleProperties.ntConsoleProps = _ntConsoleProps;
            return true;
        }

        public void WriteConsoleProperties() {
            RemoveDataBlock(ConsoleProperties.SIGNATURE);
            IntPtr pDataBlock = Marshal.AllocCoTaskMem(unchecked((int)ConsoleProperties.SIZE));
            Marshal.StructureToPtr(ConsoleProperties.ntConsoleProps, pDataBlock, false);
            (_handle as IShellLinkDataList).AddDataBlock(pDataBlock);
            Marshal.FreeCoTaskMem(pDataBlock);
        }

        public void ResetConsoleProperties(bool usePerAppSettings = true) {
            Dictionary<string, ConsoleSettings> allConsoleSettings = ConsoleSettings.GetPerAppSettings();
            string appKeyName = Target.Replace("\\", "_");

            if (usePerAppSettings && allConsoleSettings.ContainsKey(appKeyName)) {
                ConsoleProperties = allConsoleSettings[appKeyName].ToConsoleProperties();
            } else {
                ConsoleProperties = allConsoleSettings["DEFAULT"].ToConsoleProperties();
            }
        }

        public void CopyConsolePropertiesFrom(string path) {
            ShellLink targetLink = new ShellLink(path);
            if (targetLink.HasConsoleProperties) {
                ConsoleProperties.ntConsoleProps = targetLink.ConsoleProperties.ntConsoleProps;
                WriteConsoleProperties();
            }
            targetLink.Dispose();
        }
    }
}