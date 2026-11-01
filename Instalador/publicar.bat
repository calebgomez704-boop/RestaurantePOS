@echo off
setlocal EnableExtensions EnableDelayedExpansion

rem ===========================================================================
rem  Sabores del Valle POS - Publicacion con doble clic
rem  1) Restaura NuGet  2) Compila en Release  3) Copia a Instalador\Publicar
rem  4) Genera Instalador\Salida\Setup_SaboresDelValle.exe (si esta Inno Setup)
rem ===========================================================================

rem Cambie aqui la version que mostrara el instalador
set "APP_VERSION=1.0.0"

set "INST_DIR=%~dp0"
if "%INST_DIR:~-1%"=="\" set "INST_DIR=%INST_DIR:~0,-1%"
for %%I in ("%INST_DIR%\..") do set "ROOT=%%~fI"
set "CSPROJ=%ROOT%\RestaurantePOS.csproj"
set "OUT_BIN=%ROOT%\bin\Release"
set "PUBLICAR=%INST_DIR%\Publicar"
set "ISS=%INST_DIR%\RestaurantePOS.iss"

echo.
echo ==============================================
echo   Sabores del Valle POS - Publicacion
echo ==============================================
echo.

if not exist "%CSPROJ%" (
  echo [ERROR] No se encontro el proyecto: %CSPROJ%
  goto :fallo
)

rem ---------- 1) Localizar MSBuild con vswhere ----------
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
set "MSBUILD="
if exist "%VSWHERE%" (
  for /f "usebackq delims=" %%I in (`"%VSWHERE%" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do (
    if not defined MSBUILD set "MSBUILD=%%I"
  )
)
if not defined MSBUILD (
  echo [ERROR] No se encontro MSBuild.
  echo.
  echo Instale UNO de estos y vuelva a ejecutar este archivo:
  echo   - Visual Studio Build Tools ^(gratuito^): https://visualstudio.microsoft.com/downloads/
  echo     Cargas de trabajo: "Herramientas de compilacion de .NET" y, en componentes
  echo     individuales, "Paquete de destino de .NET Framework 4.7.2".
  echo   - o Visual Studio con la carga "Desarrollo de escritorio con .NET".
  goto :fallo
)
echo MSBuild: %MSBUILD%

rem ---------- 2) Restaurar NuGet y compilar en Release ----------
echo.
echo [1/4] Restaurando paquetes NuGet y compilando en Release...
"%MSBUILD%" "%CSPROJ%" /restore /t:Rebuild /p:Configuration=Release /p:Platform=AnyCPU /verbosity:minimal /nologo
if errorlevel 1 (
  echo.
  echo [ERROR] La compilacion fallo. Revise los mensajes de arriba.
  echo Si dice que falta el destino .NETFramework v4.7.2, instale el
  echo "Paquete de destino de .NET Framework 4.7.2" desde Visual Studio Installer.
  echo Si no puede descargar paquetes, verifique su conexion a Internet ^(NuGet^).
  goto :fallo
)

rem ---------- 3) Copiar salida a una carpeta limpia ----------
echo.
echo [2/4] Preparando carpeta limpia: %PUBLICAR%
if exist "%PUBLICAR%" rmdir /s /q "%PUBLICAR%"
mkdir "%PUBLICAR%"
if errorlevel 1 (
  echo [ERROR] No se pudo crear la carpeta %PUBLICAR%
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
echo.
echo [3/4] Verificando archivos indispensables...
set "FALTA="
for %%F in ("RestaurantePOS.exe" "RestaurantePOS.exe.config" "System.Data.SQLite.dll" "BCrypt.Net-Next.dll" "PdfSharp.dll" "x64\SQLite.Interop.dll" "x86\SQLite.Interop.dll") do (
  if not exist "%PUBLICAR%\%%~F" (
    echo   [FALTA] %%~F
    set "FALTA=1"
  ) else (
    echo   [ok]    %%~F
  )
)
if defined FALTA (
  echo.
  echo [ERROR] Faltan archivos en la salida. No se generara el instalador.
  echo Compruebe que el restore de NuGet termino bien ^(paquete System.Data.SQLite.Core^).
  goto :fallo
)
if exist "%PUBLICAR%\*.db" (
  echo [ERROR] Hay un archivo .db en la salida. El instalador no debe incluir bases de datos.
  goto :fallo
)

rem ---------- 4) Generar el instalador con Inno Setup ----------
echo.
echo [4/4] Buscando Inno Setup ^(ISCC.exe^)...
set "ISCC="
for %%P in ("%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe" "%ProgramFiles%\Inno Setup 6\ISCC.exe" "%LocalAppData%\Programs\Inno Setup 6\ISCC.exe") do (
  if not defined ISCC if exist "%%~P" set "ISCC=%%~P"
)
if not defined ISCC (
  echo.
  echo [AVISO] Inno Setup 6 no esta instalado, asi que NO se genero el instalador.
  echo La aplicacion compilada esta lista en:
  echo   %PUBLICAR%
  echo Para crear Setup_SaboresDelValle.exe instale Inno Setup 6 ^(gratuito^):
  echo   https://jrsoftware.org/isdl.php
  echo y vuelva a ejecutar este archivo.
  goto :fin
)
echo ISCC: %ISCC%
"%ISCC%" /Qp "/DMyAppVersion=%APP_VERSION%" "/DSourceDir=%PUBLICAR%" "%ISS%"
if errorlevel 1 (
  echo.
  echo [ERROR] Inno Setup no pudo generar el instalador.
  goto :fallo
)

echo.
echo ==============================================
echo   LISTO. Instalador generado en:
echo   %INST_DIR%\Salida\Setup_SaboresDelValle.exe
echo ==============================================
goto :fin

:fallo
echo.
echo *** La publicacion NO se completo. ***
pause
exit /b 1

:fin
echo.
pause
exit /b 0
