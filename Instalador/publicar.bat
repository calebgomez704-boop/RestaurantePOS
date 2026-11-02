@echo off
setlocal EnableExtensions EnableDelayedExpansion

rem ===========================================================================
rem  Sabores del Valle POS - Publicacion con doble clic
rem  1) Restaura NuGet  2) Compila en Release  3) Copia a Instalador\Publicar
rem  4) Genera Instalador\Salida\Setup_SaboresDelValle.exe (si esta Inno Setup)
rem ===========================================================================

set "INST_DIR=%~dp0"
if "%INST_DIR:~-1%"=="\" set "INST_DIR=%INST_DIR:~0,-1%"
for %%I in ("%INST_DIR%\..") do set "ROOT=%%~fI"
set "CSPROJ=%ROOT%\RestaurantePOS.csproj"
set "OUT_BIN=%ROOT%\bin\Release"
set "PUBLICAR=%INST_DIR%\Publicar"
set "ISS=%INST_DIR%\RestaurantePOS.iss"
set "ASSEMBLY_INFO=%ROOT%\Properties\AssemblyInfo.cs"
set "LOG=%INST_DIR%\log_publicar.txt"
set "PF86=%ProgramFiles(x86)%"
set "PF=%ProgramFiles%"
del /q "%LOG%" >nul 2>nul

call :log ==============================================
call :log   Sabores del Valle POS - Publicacion
call :log ==============================================
call :log

if not exist "%CSPROJ%" (
  call :log [ERROR] No se encontro el proyecto: %CSPROJ%
  goto :fallo
)
if not exist "%ASSEMBLY_INFO%" (
  call :log [ERROR] No se encontro AssemblyInfo.cs: %ASSEMBLY_INFO%
  goto :fallo
)
set "APP_VERSION="
for /f "tokens=2 delims=^(" %%I in ('findstr /c:"AssemblyFileVersion" "%ASSEMBLY_INFO%"') do set "APP_VERSION=%%~I"
for /f "tokens=1 delims=)]" %%I in ("!APP_VERSION!") do set "APP_VERSION=%%~I"
set "APP_VERSION=!APP_VERSION:"=!"
if not defined APP_VERSION (
  call :log [ERROR] No se pudo leer AssemblyFileVersion desde %ASSEMBLY_INFO%
  goto :fallo
)
call :log Version leida de AssemblyInfo.cs: !APP_VERSION!

rem ---------- 1) Localizar MSBuild con vswhere ----------
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
set "MSBUILD="
if exist "!VSWHERE!" for /f "usebackq delims=" %%I in (`"!VSWHERE!" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do if not defined MSBUILD set "MSBUILD=%%I"
if not defined MSBUILD if exist "!PF!\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=!PF!\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD if exist "!PF!\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=!PF!\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD if exist "!PF!\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=!PF!\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD if exist "!PF!\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=!PF!\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD if exist "!PF!\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=!PF!\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD if exist "!PF!\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=!PF!\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD (
  call :log [ERROR] No se encontro MSBuild.
  call :log Instale Visual Studio 2019/2022 o Build Tools con MSBuild y el targeting pack .NET Framework 4.7.2.
  goto :fallo
)
call :log MSBuild: !MSBUILD!

rem ---------- 2) Restaurar NuGet y compilar en Release ----------
call :log
call :log [1/4] Restaurando paquetes NuGet y compilando en Release...
set "TEMP_LOG=%TEMP%\RestaurantePOS_msbuild.log"
cmd /c ""!MSBUILD!" "!CSPROJ!" /restore /t:Rebuild /p:Configuration=Release /p:Platform=AnyCPU /verbosity:minimal /nologo > "!TEMP_LOG!" 2>&1"
type "!TEMP_LOG!"
type "!TEMP_LOG!" >> "!LOG!"
if errorlevel 1 (
  call :log [ERROR] La compilacion fallo. Revise %TEMP_LOG% y los mensajes anteriores.
  goto :fallo
)

rem ---------- 3) Copiar salida a una carpeta limpia ----------
call :log
call :log [2/4] Preparando carpeta limpia: %PUBLICAR%
if exist "%PUBLICAR%" rmdir /s /q "%PUBLICAR%"
mkdir "%PUBLICAR%"
if errorlevel 1 (
  call :log [ERROR] No se pudo crear la carpeta %PUBLICAR%
  goto :fallo
)

copy /y "%OUT_BIN%\RestaurantePOS.exe"        "%PUBLICAR%\" >nul
copy /y "%OUT_BIN%\RestaurantePOS.exe.config" "%PUBLICAR%\" >nul
copy /y "%OUT_BIN%\*.dll"                     "%PUBLICAR%\" >nul
if exist "%OUT_BIN%\x86\" xcopy "%OUT_BIN%\x86" "%PUBLICAR%\x86\" /e /i /y /q >nul
if exist "%OUT_BIN%\x64\" xcopy "%OUT_BIN%\x64" "%PUBLICAR%\x64\" /e /i /y /q >nul
if exist "%OUT_BIN%\Assets\" xcopy "%OUT_BIN%\Assets" "%PUBLICAR%\Assets\" /e /i /y /q >nul
if not exist "%PUBLICAR%\Assets\" if exist "%ROOT%\Assets\" (
  mkdir "%PUBLICAR%\Assets"
  copy /y "%ROOT%\Assets\*.png" "%PUBLICAR%\Assets\" >nul 2>nul
)

rem ---------- Verificar archivos indispensables ----------
call :log
call :log [3/4] Verificando archivos indispensables...
set "FALTA="
for %%F in ("RestaurantePOS.exe" "RestaurantePOS.exe.config" "System.Data.SQLite.dll" "BCrypt.Net-Next.dll" "PdfSharp.dll" "x64\SQLite.Interop.dll" "x86\SQLite.Interop.dll") do (
  if not exist "%PUBLICAR%\%%~F" (
    call :log   [FALTA] %%~F
    set "FALTA=1"
  ) else (
    call :log   [ok]    %%~F
  )
)
if defined FALTA (
  call :log
  call :log [ERROR] Faltan archivos en la salida. No se generara el instalador.
  call :log Compruebe que el restore de NuGet termino bien.
  goto :fallo
)
if exist "%PUBLICAR%\*.db" (
  call :log [ERROR] Hay un archivo .db en la salida. El instalador no debe incluir bases de datos.
  goto :fallo
)

rem ---------- 4) Generar el instalador con Inno Setup ----------
call :log
call :log [4/4] Buscando Inno Setup ISCC.exe...
set "ISCC="
if exist "!PF86!\Inno Setup 6\ISCC.exe" set "ISCC=!PF86!\Inno Setup 6\ISCC.exe"
if not defined ISCC if exist "!PF!\Inno Setup 6\ISCC.exe" set "ISCC=!PF!\Inno Setup 6\ISCC.exe"
if not defined ISCC if exist "%LocalAppData%\Programs\Inno Setup 6\ISCC.exe" set "ISCC=%LocalAppData%\Programs\Inno Setup 6\ISCC.exe"
if not defined ISCC (
  call :log [ERROR] Inno Setup 6 no esta instalado. No se genero el instalador.
  call :log Instale Inno Setup 6 desde https://jrsoftware.org/isdl.php y vuelva a ejecutar este archivo.
  goto :fallo
)
call :log ISCC: !ISCC!
set "TEMP_LOG=%TEMP%\RestaurantePOS_iscc.log"
cmd /c ""!ISCC!" /Qp "/DMyAppVersion=!APP_VERSION!" "/DSourceDir=!PUBLICAR!" "!ISS!" > "!TEMP_LOG!" 2>&1"
type "!TEMP_LOG!"
type "!TEMP_LOG!" >> "!LOG!"
if errorlevel 1 (
  call :log [ERROR] Inno Setup no pudo generar el instalador.
  goto :fallo
)

call :log
call :log ==============================================
call :log   LISTO. Instalador generado en:
call :log   %INST_DIR%\Salida\RestaurantePOS_Setup.exe
call :log ==============================================
goto :fin

:fallo
  call :log
call :log *** La publicacion NO se completo. ***
call :log Consulte el archivo %LOG%.
pause
exit /b 1

:fin
call :log
pause
exit /b 0

:log
echo(%*
>>"%LOG%" echo(%*
exit /b 0
