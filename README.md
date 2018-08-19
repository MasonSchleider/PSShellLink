# PowerShell.ShellLink

PSShellLink is a PowerShell module for viewing and modifying Windows shell link metadata.

At the moment, its primary function is to assist in the management of Windows console application settings.

## Background Information [[ref]](https://blogs.msdn.microsoft.com/commandline/2017/06/20/understanding-windows-console-host-settings/)

When a console application (e.g., cmd.exe, PowerShell) is launched, its settings (e.g., font, layout, colors) are retreived from the following locations and applied in order (with later settings overriding earlier ones):

1. Hardcoded settings in `conhostv2.dll`.
2. Default settings stored in the registry key `HKEY_CURRENT_USER\Console`.
3. Per-application settings stored in subkeys of the above registry key.
   The subkey is assigned one of the following two names:
   <ol type="a">
     <li>The path of the console application (substituting '\' for '_').</li>
     <li>The title of the console window.</li>
   </ol>
4. If the console application was launched from a shortcut, settings stored in the metadata of the shortcut file.

## Commands

### Get-ShellLink

Retrieves an instance of the ShellLink class referencing a specified shortcut file. The ShellLink class is the principal class utilized by the module's underlying C# library. It contains methods for managing the auxiliary data blocks stored within a shortcut file (primarily the data block containing console properties).

### Copy-ConsoleProperties

Copies console properties from one shortcut file to another.

### Reset-ConsoleProperties

Resets the console properties of a shortcut file to the default settings found in the Windows Registry.

### Export-ConsoleProperties

Exports a shortcut file's console properties to a Windows Registry file.

### Get-DefaultConsoleSettings

Retrieves a data structure containing the default console settings found in the Windows Registry.
