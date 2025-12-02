# Super Kiro World - Windows EXE Builder (PowerShell)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Super Kiro World - Windows EXE Builder" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# ビルド設定
$ProjectPath = "SuperKiroWorld\SuperKiroWorld.csproj"
$OutputDir = ".\build"

# 前回のビルドをクリーン
Write-Host "[1/4] Cleaning previous builds..." -ForegroundColor Yellow
if (Test-Path $OutputDir) {
    Remove-Item -Path $OutputDir -Recurse -Force
}
New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null

# 依存関係の復元
Write-Host "[2/4] Restoring dependencies..." -ForegroundColor Yellow
dotnet restore $ProjectPath
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to restore dependencies" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

# Windows x64ビルド
Write-Host "[3/4] Building Windows x64 executable..." -ForegroundColor Yellow
dotnet publish $ProjectPath `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -o "$OutputDir\win-x64"

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Build failed" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

# Windows x86ビルド
Write-Host "[4/4] Building Windows x86 executable..." -ForegroundColor Yellow
dotnet publish $ProjectPath `
    -c Release `
    -r win-x86 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -o "$OutputDir\win-x86"

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Build failed" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Output locations:" -ForegroundColor Cyan
Write-Host "  64-bit: $OutputDir\win-x64\SuperKiroWorld.exe" -ForegroundColor White
Write-Host "  32-bit: $OutputDir\win-x86\SuperKiroWorld.exe" -ForegroundColor White
Write-Host ""
Write-Host "You can now run the game by double-clicking the EXE file." -ForegroundColor Yellow
Write-Host ""
Read-Host "Press Enter to exit"
