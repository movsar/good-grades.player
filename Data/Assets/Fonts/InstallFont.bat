@echo off
setlocal

for /F "tokens=1 delims= " %%A in ('tasklist ^| findstr /I "GGManager.exe GGPlayer.exe"') do (
    set ACTIVE_APP=%%A
)

if not defined ACTIVE_APP (
    echo No active application found.
    goto :eof
)

call set ACTIVE_APP=%%ACTIVE_APP:.exe=%%
set FONT_NAME=segmdl2.ttf
call set FONT_PATH=%%appdata%%\..\Local\%ACTIVE_APP%\current\Assets\Fonts\%FONT_NAME%%
echo %ACTIVE_APP%

echo Copying font...
echo %FONT_PATH%
copy "%FONT_PATH%" "%WINDIR%\Fonts"
echo Updating registry...
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "%FONT_NAME% (TrueType)" /t REG_SZ /d %FONT_NAME% /f

echo Font installed.
endlocal