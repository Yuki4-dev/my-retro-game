@echo off
echo ========================================
echo Super Kiro World - Windows EXE Builder
echo ========================================
echo.

REM ビルド設定
set PROJECT_PATH=SuperKiroWorld\SuperKiroWorld.csproj
set OUTPUT_DIR=.\build

echo [1/4] Cleaning previous builds...
if exist "%OUTPUT_DIR%" rmdir /s /q "%OUTPUT_DIR%"
mkdir "%OUTPUT_DIR%"

echo [2/4] Restoring dependencies...
dotnet restore "%PROJECT_PATH%"
if errorlevel 1 (
    echo ERROR: Failed to restore dependencies
    pause
    exit /b 1
)

echo [3/5] Building Windows x64 executable...
dotnet publish "%PROJECT_PATH%" ^
    -c Release ^
    -r win-x64 ^
    --self-contained true ^
    -p:PublishSingleFile=false ^
    -o "%OUTPUT_DIR%\win-x64"

if errorlevel 1 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo [3.5/5] Copying Content files for x64...
xcopy /E /I /Y "SuperKiroWorld\Content\bin\DesktopGL\Content" "%OUTPUT_DIR%\win-x64\Content"

echo [4/5] Building Windows x86 executable...
dotnet publish "%PROJECT_PATH%" ^
    -c Release ^
    -r win-x86 ^
    --self-contained true ^
    -p:PublishSingleFile=false ^
    -o "%OUTPUT_DIR%\win-x86"

if errorlevel 1 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo [4.5/5] Copying Content files for x86...
xcopy /E /I /Y "SuperKiroWorld\Content\bin\DesktopGL\Content" "%OUTPUT_DIR%\win-x86\Content"

echo.
echo ========================================
echo Build completed successfully!
echo ========================================
echo.
echo Output locations:
echo   64-bit: %OUTPUT_DIR%\win-x64\SuperKiroWorld.exe
echo   32-bit: %OUTPUT_DIR%\win-x86\SuperKiroWorld.exe
echo.
echo You can now run the game by double-clicking the EXE file.
echo.
pause
