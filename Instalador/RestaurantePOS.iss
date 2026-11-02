; ===========================================================================
;  Sabores del Valle POS - Script de Inno Setup 6
;  Se compila con publicar.bat (o abriendo este archivo en Inno Setup).
;
;  Parametros opcionales de linea de comandos (ISCC):
;    /DMyAppVersion=1.0.0         version mostrada en el instalador
;    /DSourceDir=<carpeta>        carpeta con la salida compilada (Release)
; ===========================================================================

#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif
#ifndef SourceDir
  #define SourceDir "Publicar"
#endif

#define MyAppName      "Sabores del Valle POS"
#define MyAppPublisher "Sabores del Valle"
#define MyAppExe       "RestaurantePOS.exe"
#define MyDataFolder   "RestaurantePOS"

[Setup]
AppId={{B7E2C4A1-5D3F-4E8A-9C61-2F7A0D5E8B13}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=Salida
OutputBaseFilename=RestaurantePOS_Setup
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64
MinVersion=6.1sp1
UninstallDisplayName={#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExe}

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
; Todo el contenido de la salida compilada. NUNCA se incluye una base de datos (.db):
; cada instalacion crea la suya en C:\ProgramData\RestaurantePOS al primer arranque.
Source: "{#SourceDir}\*"; DestDir: "{app}"; Excludes: "*.pdb,*.db,*.db-wal,*.db-shm,*.vshost.*"; Flags: ignoreversion recursesubdirs createallsubdirs

[Dirs]
Name: "{commonappdata}\RestaurantePOS"; Permissions: users-modify
Name: "{commonappdata}\RestaurantePOS\Datos"; Permissions: users-modify
Name: "{commonappdata}\RestaurantePOS\Respaldos"; Permissions: users-modify

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExe}"
Name: "{group}\Desinstalar {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExe}"; Tasks: desktopicon

[Run]
; runasoriginaluser: la aplicacion se abre con el usuario normal (no como administrador)
; para que la aplicacion se ejecute con el usuario normal.
Filename: "{app}\{#MyAppExe}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent runasoriginaluser

[Code]
const
  // Valor 'Release' del registro para .NET Framework 4.7.2 (461808 en Windows 10 abril 2018, 461814 en el resto)
  NetFx472Release = 461808;

function NetFramework472Instalado: Boolean;
var
  Release: Cardinal;
begin
  Result := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', Release)
            and (Release >= NetFx472Release);
end;

function VisualCRedistributableInstalado: Boolean;
begin
  Result := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\x64') or
            RegKeyExists(HKLM, 'SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\x86') or
            RegKeyExists(HKLM32, 'SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\x64') or
            RegKeyExists(HKLM32, 'SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\x86');
end;

function InitializeSetup: Boolean;
begin
  Result := True;
  if not NetFramework472Instalado then
  begin
    MsgBox('Sabores del Valle POS necesita Microsoft .NET Framework 4.7.2 o superior, ' +
           'y no se encontro en este equipo.' + #13#10#13#10 +
           'Instale .NET Framework 4.8 (gratuito) desde:' + #13#10 +
           'https://dotnet.microsoft.com/download/dotnet-framework/net48' + #13#10#13#10 +
           'Despues vuelva a ejecutar este instalador.',
           mbError, MB_OK);
    Result := False;
  end;
  if Result and (not VisualCRedistributableInstalado) then
    MsgBox('No se detectó Microsoft Visual C++ 2015-2022 Redistributable.' + #13#10#13#10 +
           'Descárguelo desde:' + #13#10 +
           'https://aka.ms/vs/17/release/vc_redist.x64.exe' + #13#10#13#10 +
           'La aplicación puede no iniciar hasta instalarlo.', mbInformation, MB_OK);
end;

// Al desinstalar: preguntar si se borran los datos (por defecto NO se borran).
procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  Carpeta: String;
begin
  if CurUninstallStep = usPostUninstall then
  begin
    Carpeta := ExpandConstant('{commonappdata}\{#MyDataFolder}');
    if DirExists(Carpeta) and (not UninstallSilent) then
    begin
      if MsgBox('¿Desea borrar todos los datos del restaurante?' + #13#10#13#10 +
                'Si elige "No", se conservarán en:' + #13#10 + Carpeta + #13#10 +
                'y estarán disponibles si vuelve a instalar el programa.' + #13#10#13#10 +
                'Esta acción no se puede deshacer.',
                mbConfirmation, MB_YESNO or MB_DEFBUTTON2) = IDYES then
      begin
        DelTree(Carpeta, True, True, True);
      end;
    end;
  end;
end;
