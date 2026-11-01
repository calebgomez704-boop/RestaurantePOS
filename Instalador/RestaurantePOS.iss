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
#define MyDataFolder   "SaboresDelValle"

[Setup]
AppId={{B7E2C4A1-5D3F-4E8A-9C61-2F7A0D5E8B13}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=Salida
OutputBaseFilename=Setup_SaboresDelValle
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
; cada instalacion crea la suya en %LocalAppData%\SaboresDelValle al primer arranque.
Source: "{#SourceDir}\*"; DestDir: "{app}"; Excludes: "*.pdb,*.db,*.db-wal,*.db-shm,*.vshost.*"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExe}"
Name: "{group}\Desinstalar {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExe}"; Tasks: desktopicon

[Run]
; runasoriginaluser: la aplicacion se abre con el usuario normal (no como administrador)
; para que su base de datos quede en el %LocalAppData% de ese usuario.
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
end;

// Al desinstalar: preguntar si se conservan los datos (por defecto SI se conservan).
procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  Carpeta: String;
begin
  if CurUninstallStep = usPostUninstall then
  begin
    Carpeta := ExpandConstant('{localappdata}\{#MyDataFolder}');
    if DirExists(Carpeta) and (not UninstallSilent) then
    begin
      if MsgBox('Desea CONSERVAR sus datos (usuarios, pedidos, ventas, reportes)?' + #13#10#13#10 +
                'Si elige "Si", se conservan en:' + #13#10 + Carpeta + #13#10 +
                'y estaran disponibles si vuelve a instalar el programa.' + #13#10#13#10 +
                'Si elige "No", se ELIMINAN de forma permanente.',
                mbConfirmation, MB_YESNO or MB_DEFBUTTON1) = IDNO then
      begin
        DelTree(Carpeta, True, True, True);
      end;
    end;
  end;
end;
