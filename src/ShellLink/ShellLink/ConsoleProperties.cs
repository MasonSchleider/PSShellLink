using Console.Interop.Internal;
using System;
using System.Runtime.InteropServices;

namespace Console.Interop {
    public class ConsoleProperties {
        public static readonly uint SIZE = unchecked((uint)Marshal.SizeOf(typeof(NT_CONSOLE_PROPS)));
        public const uint SIGNATURE = 0xA0000002;

        private const int MAX_FACE_NAME_LENGTH = 32;
        private const int COLOR_TABLE_SIZE = 16;

        internal NT_CONSOLE_PROPS ntConsoleProps;

        public ConsoleProperties() {
            ntConsoleProps = new NT_CONSOLE_PROPS();
            ntConsoleProps.dbh.cbSize = SIZE;
            ntConsoleProps.dbh.dwSignature = SIGNATURE;
            ntConsoleProps.ColorTable = new uint[COLOR_TABLE_SIZE];
        }

        #region Properties

        public ushort ScreenColors {
            get {
                return ntConsoleProps.wFillAttribute;
            }
            set {
                ntConsoleProps.wFillAttribute = value;
            }
        }

        public ushort PopupColors {
            get {
                return ntConsoleProps.wPopupFillAttribute;
            }
            set {
                ntConsoleProps.wPopupFillAttribute = value;
            }
        }

        public Coordinate ScreenBufferSize {
            get {
                return new Coordinate(ntConsoleProps.dwScreenBufferSize);
            }
            set {
                ntConsoleProps.dwScreenBufferSize = value.AsCOORD();
            }
        }

        public Coordinate WindowSize {
            get {
                return new Coordinate(ntConsoleProps.dwWindowSize);
            }
            set {
                ntConsoleProps.dwWindowSize = value.AsCOORD();
            }
        }

        public Coordinate WindowPosition {
            get {
                return new Coordinate(ntConsoleProps.dwWindowOrigin);
            }
            set {
                ntConsoleProps.dwWindowOrigin = value.AsCOORD();
            }
        }

        public uint FontIndex {
            get {
                return ntConsoleProps.nFont;
            }
            set {
                ntConsoleProps.nFont = value;
            }
        }

        public uint InputBufferSize {
            get {
                return ntConsoleProps.nInputBufferSize;
            }
            set {
                ntConsoleProps.nInputBufferSize = value;
            }
        }

        public Coordinate FontSize {
            get {
                return new Coordinate(ntConsoleProps.dwFontSize);
            }
            set {
                ntConsoleProps.dwFontSize = value.AsCOORD();
            }
        }

        public uint FontFamily {
            get {
                return ntConsoleProps.uFontFamily;
            }
            set {
                ntConsoleProps.uFontFamily = value;
            }
        }

        public uint FontWeight {
            get {
                return ntConsoleProps.uFontWeight;
            }
            set {
                ntConsoleProps.uFontWeight = value;
            }
        }

        public string FaceName {
            get {
                return ntConsoleProps.FaceName;
            }
            set {
                if (value.Length > MAX_FACE_NAME_LENGTH)
                    throw new ArgumentException("Value exceeds capacity of FaceName property.");

                ntConsoleProps.FaceName = value;
            }
        }

        public uint CursorSize {
            get {
                return ntConsoleProps.uCursorSize;
            }
            set {
                ntConsoleProps.uCursorSize = value;
            }
        }

        public bool FullScreen {
            get {
                return ntConsoleProps.bFullScreen;
            }
            set {
                ntConsoleProps.bFullScreen = value;
            }
        }

        public bool QuickEdit {
            get {
                return ntConsoleProps.bQuickEdit;
            }
            set {
                ntConsoleProps.bQuickEdit = value;
            }
        }

        public bool InsertMode {
            get {
                return ntConsoleProps.bInsertMode;
            }
            set {
                ntConsoleProps.bInsertMode = value;
            }
        }

        public bool AutoPosition {
            get {
                return ntConsoleProps.bAutoPosition;
            }
            set {
                ntConsoleProps.bAutoPosition = value;
            }
        }

        public uint HistoryBufferSize {
            get {
                return ntConsoleProps.uHistoryBufferSize;
            }
            set {
                ntConsoleProps.uHistoryBufferSize = value;
            }
        }

        public uint NumberOfHistoryBuffers {
            get {
                return ntConsoleProps.uNumberOfHistoryBuffers;
            }
            set {
                ntConsoleProps.uNumberOfHistoryBuffers = value;
            }
        }

        public bool HistoryNoDup {
            get {
                return ntConsoleProps.bHistoryNoDup;
            }
            set {
                ntConsoleProps.bHistoryNoDup = value;
            }
        }

        public uint[] ColorTable {
            get {
                return ntConsoleProps.ColorTable;
            }
        }

        #endregion
    }
}