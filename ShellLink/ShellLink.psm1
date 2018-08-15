

function Get-ShellLink {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [string]$Path
    )
    
    if (!(Test-Path -LiteralPath $Path)) {
        Write-Error "Cannot find path '$Path' because it does not exist."
    }
    if (!($Path -like "*.lnk")) {
        Write-Error "File '$Path' is not a valid shortcut (.lnk) file."
    }
    
    $ShellLink = New-Object Console.Interop.ShellLink (Convert-Path $Path)
    Write-Output $ShellLink
}

function Copy-ConsoleProperties {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [string]$Path,
        
        [Parameter(Mandatory=$true)]
        [string]$Destination
    )
    
    $DestinationLink = Get-ShellLink $Destination
    $DestinationLink.CopyConsolePropertiesFrom((Convert-Path $Path))
    $DestinationLink.WriteConsoleProperties()
    $DestinationLink.Save()
    $DestinationLink.Dispose()
}

function Reset-ConsoleProperties {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [string]$Path
    )
    
    $ShellLink = Get-ShellLink $Path
    $ShellLink.ResetConsoleProperties()
    $ShellLink.WriteConsoleProperties()
    $ShellLink.Save()
    $ShellLink.Dispose()
}

function Export-ConsoleProperties {
    [CmdletBinding(DefaultParameterSetName="Path", PositionalBinding=$false)]
    Param (
        [Parameter(Mandatory=$true, Position=0, ParameterSetName="Path", ValueFromPipeline=$true)]
        [string]$Path,
        
        [Parameter(Mandatory=$true, Position=0, ParameterSetName="ConsoleProperties", ValueFromPipeline=$true)]
        [Console.Interop.ConsoleProperties]$ConsoleProperties,
        
        [Parameter(Mandatory=$true, Position=1)]
        [string]$Destination,
        
        [Switch]$PassThru
    )
    
    $ExcludeProperties = "AutoPosition", "FontIndex", "InputBufferSize"
    
    if ($Path) {
        $ShellLink = Get-ShellLink $Path
        $ConsoleProperties = $ShellLink.ConsoleProperties
    }
    [psobject]$ConsoleProperties = $ConsoleProperties | Select-Object * -ExcludeProperty $ExcludeProperties
    
    $FileContent = @"
Windows Registry Editor Version 5.00

[HKEY_CURRENT_USER\Console]
"@
    
    foreach ($Property in $ConsoleProperties.psobject.Properties) {
        switch ($Property.TypeNameOfValue) {
            "Console.Interop.Coordinate" {
                $RegValue = '"{0}"=dword:{1:x4}{2:x4}' -f $Property.Name, $Property.Value.Y, $Property.Value.X
                break
            }
            "System.String" {
                $RegValue = '"{0}"="{1}"' -f $Property.Name, $Property.Value
                break
            }
            "System.UInt32[]" {
                $RegValue = ''
                for ($i = 0; $i -lt $Property.Value.Length; $i++) {
                    if ($RegValue -ne '') { $RegValue += "`r`n" }
                    $RegValue += '"{0}{1:D2}"=dword:{2:x8}' -f $Property.Name, $i, $Property.Value[$i]
                }
                break
            }
            default {
                $RegValue = '"{0}"=dword:{1:x8}' -f $Property.Name, [int]$Property.Value
            }
        }
        $FileContent += "`r`n$RegValue"
    }
    
    if ($ShellLink) {
        $ShellLink.Dispose()
    }
    Set-Content $Destination $FileContent -Encoding Unicode -NoNewline
    
    if ($PassThru) {
        Write-Output $FileContent
    }
}

function Get-DefaultConsoleSettings {
    [CmdletBinding()]
    Param ()
    
    $ConsoleSettings = [Console.Interop.ConsoleSettings]::GetDefaultSettings()
    Write-Output $ConsoleSettings
}