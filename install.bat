
@echo off
set GAMEDIR=QuickMods
set GAMEDATA=GameData

set MODS="%1"


copy /Y "%1%2" "%GAMEDATA%\%GAMEDIR%\%3\Plugins"


IF EXIST "%1..\Lang\" xcopy /y /s /I "%1..\Lang" "%GAMEDATA%\%GAMEDIR%\%3\Lang"

copy /y  README.md %GAMEDATA%\%GAMEDIR%\%3
copy /y  COPYING %GAMEDATA%\%GAMEDIR%\%3
copy /y  %1..\%3.version  %GAMEDATA%\%GAMEDIR%\%3

