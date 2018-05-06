
@echo off

rem H is the destination game folder
rem GAMEDIR is the name of the mod folder (usually the mod name)
rem GAMEDATA is the name of the local GameData
rem VERSIONFILE is the name of the version file, usually the same as GAMEDATA,
rem    but not always

set H=R:\KSP_1.4.1_dev
set GAMEDIR=QuickMods
set GAMEDATA="GameData"
set VERSIONFILE=%GAMEDIR%.version

copy /y  QuickMods.version  %GAMEDATA%\%GAMEDIR%
copy /y  README.md %GAMEDATA%\%GAMEDIR%\%3
copy /y  COPYING %GAMEDATA%\%GAMEDIR%\%3

xcopy /y /s /I %GAMEDATA%\%GAMEDIR% "%H%\GameData\%GAMEDIR%"
