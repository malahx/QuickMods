# QuickIronMan
#### A plugin for Kerbal Space Program
#### Copyright 2021 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 


#### What is it?

QuickIronMan is a plugin which turn each savegame to an IronMan mod (like in Europa Universalis).
(It's an easier revive of my old mod SRL - Simulate, Revert and Launch)

#### How does it work?

Each default launch are a simulation which can't be saved. To launch a "hard launch", you need to press left shift when click on the launch button. You can't launch simulation from the Space Center.

If you want simulation mod with more feature, I can suggest you to use one of these mods:
* KRASH: https://forum.kerbalspaceprogram.com/index.php?/topic/133082-1 
* KVASS: https://forum.kerbalspaceprogram.com/index.php?/topic/183393-1

The background, your savegame will be edited, with in default:

    HighLogic.CurrentGame.Parameters.Flight.CanRestart = false;
    HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor = false;

    HighLogic.CurrentGame.Parameters.Flight.CanQuickLoad = true;
    HighLogic.CurrentGame.Parameters.Flight.CanQuickSave = true;
    HighLogic.CurrentGame.Parameters.Flight.CanLeaveToTrackingStation = true;
    HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsNear = true;
    HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsFar = true;
    HighLogic.CurrentGame.Parameters.Flight.CanEVA = true;
    HighLogic.CurrentGame.Parameters.Flight.CanBoard = true;
    HighLogic.CurrentGame.Parameters.Flight.CanAutoSave = true;
    HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter = true;

These variable will be toggle each time the simulation is toggle.  

#### How to install it?

Unzip all files. Put the QuickMods folder in your KSP/GameData folder.

#### How to update it?

Unzip all files. Merge the new QuickMods folder with the old folder which is in your KSP/GameData folder.

#### How to uninstall it?

In game, toggle simulation off, start a new vessel, and save.

Delete the QuickMods/QuickIronMan folder in your KSP/GameData folder.

#### Changelog
1.1.0 - 2021.09.04
* Enhanced stock launch/simulate button,
* Switched toggle but by default to space,
* Disabled recover / go to space center in simulation from the altimeter,
* Added configuration file.

1.0.0 - 2021.08.08
* First release.

#### Thanks!

* to all mod developers which make this game really huge,
* and to Squad for this awesome game.

#### Links

* http://forum.kerbalspaceprogram.com/index.php?/topic/85834-1
* https://github.com/malahx/QuickMods
