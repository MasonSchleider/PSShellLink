Push-Location $PSScriptRoot\..

$SourceFiles = Get-ChildItem .\src\ShellLink, .\src\ShellLink\Internal *.cs | Resolve-Path -Relative
$OutputPath = '.\lib\ShellLink.dll'

if (Test-Path $OutputPath) {
    $LibFile = Get-Item $OutputPath
    
    foreach ($SourcePath in $SourceFiles) {
        $SourceFile = Get-Item $SourcePath
        
        if ($SourceFile.LastWriteTime -gt $LibFile.LastWriteTime) {
            Add-Type -Path $SourceFiles -OutputAssembly $OutputPath -OutputType Library
            break
        }
    }
} else {
    Add-Type -Path $SourceFiles -OutputAssembly $OutputPath -OutputType Library
}

Pop-Location