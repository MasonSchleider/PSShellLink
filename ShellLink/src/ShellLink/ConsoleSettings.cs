using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Console.Interop {
    public class ConsoleSettings {
        private const string DEFAULT_CONSOLE_KEY = "Console";
        private const int COLOR_TABLE_SIZE = 16;

        public ushort ScreenColors { get; private set; }
        public ushort PopupColors { get; private set; }
        public Coordinate ScreenBufferSize { get; private set; }
        public Coordinate WindowSize { get; private set; }
        public Coordinate WindowPosition { get; private set; }
        public Coordinate FontSize { get; private set; }
        public uint FontFamily { get; private set; }
        public uint FontWeight { get; private set; }
        public string FaceName { get; private set; }
        public uint CursorSize { get; private set; }
        public bool FullScreen { get; private set; }
        public bool QuickEdit { get; private set; }
        public bool InsertMode { get; private set; }
        public uint HistoryBufferSize { get; private set; }
        public uint NumberOfHistoryBuffers { get; private set; }
        public bool HistoryNoDup { get; private set; }
        public uint[] ColorTable { get; private set; }

        private ConsoleSettings() {
            ColorTable = new uint[COLOR_TABLE_SIZE];
        }

        public static ConsoleSettings GetDefaultSettings() {
            return GetSettings(DEFAULT_CONSOLE_KEY);
        }

        public static Dictionary<string, ConsoleSettings> GetPerAppSettings() {
            ConsoleSettings defaultSettings = GetDefaultSettings();
            Dictionary<string, ConsoleSettings> allConsoleSettings = new Dictionary<string, ConsoleSettings>(StringComparer.OrdinalIgnoreCase) {
                { "DEFAULT", defaultSettings }
            };
            RegistryKey consoleKey = Registry.CurrentUser.OpenSubKey(DEFAULT_CONSOLE_KEY);

            foreach (string subKeyName in consoleKey.GetSubKeyNames()) {
                ConsoleSettings appSettings = GetSettings(String.Format(@"{0}\{1}", DEFAULT_CONSOLE_KEY, subKeyName), defaultSettings);
                allConsoleSettings.Add(subKeyName, appSettings);
            }

            consoleKey.Close();
            return allConsoleSettings;
        }

        private static ConsoleSettings GetSettings(string keyPath, ConsoleSettings defaultSettings = null) {
            ConsoleSettings consoleSettings = new ConsoleSettings();
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(keyPath);

            foreach (PropertyInfo property in typeof(ConsoleSettings).GetProperties()) {
                var registryValue = registryKey.GetValue(property.Name);
                if (registryValue != null) {
                    if (property.PropertyType.Equals(typeof(Coordinate))) {
                        Coordinate coord = new Coordinate((short)((int)registryValue & 0x0000FFFF), (short)((int)registryValue >> 16));
                        property.SetValue(consoleSettings, coord);
                    } else {
                        property.SetValue(consoleSettings, Convert.ChangeType(registryValue, property.PropertyType));
                    }
                } else if (defaultSettings != null) {
                    property.SetValue(consoleSettings, property.GetValue(defaultSettings));
                }
            }

            for (int i = 0; i < COLOR_TABLE_SIZE; i++) {
                var registryValue = registryKey.GetValue(String.Format("ColorTable{0:D2}", i));
                if (registryValue != null) {
                    consoleSettings.ColorTable[i] = (uint)Convert.ChangeType(registryValue, typeof(uint));
                } else if (defaultSettings != null) {
                    consoleSettings.ColorTable[i] = defaultSettings.ColorTable[i];
                }
            }

            registryKey.Close();
            return consoleSettings;
        }

        public ConsoleProperties ToConsoleProperties() {
            ConsoleProperties consoleProperties = new ConsoleProperties();

            foreach (PropertyInfo consoleSettingsProperty in typeof(ConsoleSettings).GetProperties()) {
                PropertyInfo consolePropertiesProperty = typeof(ConsoleProperties).GetProperty(consoleSettingsProperty.Name);
                if (consolePropertiesProperty.CanWrite)
                    consolePropertiesProperty.SetValue(consoleProperties, consoleSettingsProperty.GetValue(this));
            }

            for (int i = 0; i < COLOR_TABLE_SIZE; i++) {
                consoleProperties.ColorTable[i] = this.ColorTable[i];
            }

            return consoleProperties;
        }
    }
}
