@echo off
setlocal

set FONT_NAME=segmdl2.ttf
set FONT_PATH=C:\Users\user\source\repos\good-grades\Data\Assets\Fonts\%FONT_NAME%

echo Copying font...
copy "%FONT_PATH%" "%WINDIR%\Fonts"

echo Updating registry...
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "%FONT_NAME% (TrueType)" /t REG_SZ /d %FONT_NAME% /f

echo Font installed.
endlocal