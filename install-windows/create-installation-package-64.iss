; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Infinite Diagram"
#define MyAppVersion "0.5.23"
#define MyAppPublisher "pekand@gmail.com"
#define MyAppURL "http://www.infinite-diagram.com"
#define MyAppExeName "Diagram.exe"


[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{E381FAEC-36E6-4BD0-A685-B130B8E56D7D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
LicenseFile=..\LICENSE.md
OutputBaseFilename=infinite-diagram-{#MyAppVersion}
SetupIconFile=files\Diagram.ico
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64
DisableDirPage=auto

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "files\Diagram.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\diagram.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\IronPython.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\IronPython.Modules.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\IronPython.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\IronPython.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\Microsoft.Dynamic.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\Microsoft.Scripting.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\Microsoft.Scripting.AspNet.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\Microsoft.Scripting.Metadata.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\NCalc.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "files\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Registry]

Root: HKCR; Subkey: ".diagram"; ValueType: string; ValueName: ""; ValueData: "diagramfile" 
Root: HKCR; Subkey: ".diagram"; ValueType: string; ValueName: "Content Type"; ValueData: "text/plain" 
Root: HKCR; Subkey: ".diagram\ShellNew"; ValueType: binary; ValueName: "Data"; ValueData: "3c 64 69 61 67 72 61 6d 3e 3c 2f 64 69 61 67 72 61 6d 3e" 
Root: HKCR; Subkey: "diagramfile"; ValueType: string; ValueName: ""; ValueData: "Diagram File" 
Root: HKCR; Subkey: "diagramfile\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\diagram.ico" 
Root: HKCR; Subkey: "diagramfile\shell\open\command"; ValueType: string; ValueName: ""; ValueData: "{app}\Diagram.exe ""%1""" 

[Code]
function IsDotNetDetected(version: string; service: cardinal): boolean;
// Indicates whether the specified version and service pack of the .NET Framework is installed.
//
// version -- Specify one of these strings for the required .NET Framework version:
//    'v1.1.4322'     .NET Framework 1.1
//    'v2.0.50727'    .NET Framework 2.0
//    'v3.0'          .NET Framework 3.0
//    'v3.5'          .NET Framework 3.5
//    'v4\Client'     .NET Framework 4.0 Client Profile
//    'v4\Full'       .NET Framework 4.0 Full Installation
//    'v4.5'          .NET Framework 4.5
//
// service -- Specify any non-negative integer for the required service pack level:
//    0               No service packs required
//    1, 2, etc.      Service pack 1, 2, etc. required
var
    key: string;
    install, release, serviceCount: cardinal;
    check45, success: boolean;
begin
    // .NET 4.5 installs as update to .NET 4.0 Full
    if version = 'v4.5' then begin
        version := 'v4\Full';
        check45 := true;
    end else
        check45 := false;

    // installation key group for all .NET versions
    key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\' + version;

    // .NET 3.0 uses value InstallSuccess in subkey Setup
    if Pos('v3.0', version) = 1 then begin
        success := RegQueryDWordValue(HKLM, key + '\Setup', 'InstallSuccess', install);
    end else begin
        success := RegQueryDWordValue(HKLM, key, 'Install', install);
    end;

    // .NET 4.0/4.5 uses value Servicing instead of SP
    if Pos('v4', version) = 1 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Servicing', serviceCount);
    end else begin
        success := success and RegQueryDWordValue(HKLM, key, 'SP', serviceCount);
    end;

    // .NET 4.5 uses additional value Release
    if check45 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Release', release);
        success := success and (release >= 378389);
    end;

    result := success and (install = 1) and (serviceCount >= service);
end;


function InitializeSetup(): Boolean;
begin
    if not IsDotNetDetected('v4\Client', 0) then begin
        MsgBox('MyApp requires Microsoft .NET Framework 4.0 Client Profile.'#13#13
            'Please use Windows Update to install this version,'#13
            'and then re-run the MyApp setup program.', mbInformation, MB_OK);
        result := false;
    end else
        result := true;
end;