param([switch]$Database, [switch]$RemoveDatabase, [string]$MSBuildPath)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
if (-not $MSBuildPath) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    $MSBuildPath = & $vswhere -latest -prerelease -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
}
if (-not $MSBuildPath) { throw 'MSBuild was not found. Supply -MSBuildPath.' }
$outputDirectory = Join-Path $projectRoot 'bin\DraftVerification'
& $MSBuildPath (Join-Path $projectRoot 'InventorySystem.csproj') /t:Build /p:Configuration=Debug "/p:OutputPath=$outputDirectory\" /v:minimal /nologo
if ($LASTEXITCODE -ne 0) { throw 'Application build failed.' }
$compiler = Join-Path (Split-Path $MSBuildPath) 'Roslyn\csc.exe'
$verificationExe = Join-Path $outputDirectory 'SavedDraftsVerification.exe'
& $compiler /nologo /target:exe /platform:x64 "/out:$verificationExe" "/reference:$outputDirectory\InventorySystem.exe" /reference:System.dll /reference:System.Data.dll /reference:System.Windows.Forms.dll /reference:System.Drawing.dll (Join-Path $PSScriptRoot 'SavedDraftsVerification.cs')
if ($LASTEXITCODE -ne 0) { throw 'Verification build failed.' }
Copy-Item -LiteralPath (Join-Path $outputDirectory 'InventorySystem.exe.config') -Destination "$verificationExe.config"
$verificationArguments = @()
if ($Database) { $verificationArguments += '--database' }
if ($RemoveDatabase) { $verificationArguments += '--remove-database' }
& $verificationExe @verificationArguments
if ($LASTEXITCODE -ne 0) { throw 'Saved Drafts verification failed.' }
