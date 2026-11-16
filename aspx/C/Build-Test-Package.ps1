$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot
$msbuild = Get-Command msbuild.exe -ErrorAction SilentlyContinue
if (-not $msbuild) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (Test-Path $vswhere) {
        $buildPath = & $vswhere -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
    }
    if (-not $buildPath) { throw 'Open a Visual Studio Developer PowerShell with ASP.NET Framework build tools installed.' }
} else { $buildPath = $msbuild.Source }
& $buildPath '.\PartC.sln' /p:Configuration=Release /t:Rebuild /nologo
if ($LASTEXITCODE -ne 0) { throw 'Web application build failed.' }
$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
if (-not (Test-Path $compiler)) { throw '.NET Framework C# compiler not found.' }
New-Item -ItemType Directory -Force -Path '.\dist' | Out-Null
& $compiler /nologo /target:exe /out:dist\PriorityRulesTests.exe /r:System.Web.Services.dll '.\TaskNotes.PartC\PriorityService.asmx.cs' '.\Tests\PriorityRulesTests.cs'
if ($LASTEXITCODE -ne 0) { throw 'Test compilation failed.' }
& '.\dist\PriorityRulesTests.exe'
if ($LASTEXITCODE -ne 0) { throw 'Priority tests failed.' }
$staging = Join-Path $PSScriptRoot 'dist\Page0'
if (Test-Path $staging) { Remove-Item -Recurse -Force $staging }
New-Item -ItemType Directory -Force -Path "$staging\bin", "$staging\App_Data" | Out-Null
foreach ($file in @('Default.aspx','PriorityService.asmx','Global.asax','Web.config','Styles.css')) {
    Copy-Item (Join-Path '.\TaskNotes.PartC' $file) $staging
}
Copy-Item '.\TaskNotes.PartC\bin\TaskNotes.PartC.dll' "$staging\bin"
# Start deployment with an empty log; never publish personal error records.
Set-Content -Encoding UTF8 -Path "$staging\App_Data\ErrorLog.xml" -Value '<?xml version="1.0" encoding="utf-8"?><errors />'
Add-Type -AssemblyName System.IO.Compression.FileSystem
$zip = Join-Path $PSScriptRoot 'dist\FilesInPage0.zip'
if (Test-Path $zip) { Remove-Item $zip }
[System.IO.Compression.ZipFile]::CreateFromDirectory($staging, $zip)
Write-Host 'Build and rule tests passed. Created dist\FilesInPage0.zip. Still run the browser checklist before deployment/submission.'
