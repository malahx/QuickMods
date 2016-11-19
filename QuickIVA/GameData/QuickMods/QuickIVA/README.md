#### QuickIVA
#### A plugin for Kerbal Space Program
#### Copyright 2016 Malah

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

QuickIVA is a small plugin which adds a switch to the IVA at the loading or the launch of a vessel.

I suggest you to use the awesome mod RasterPropMonitor which makes the IVA usable.

#### How does it work ?

It will put you on IVA at launch or at load. It will select preferably the first Pilot on a pod or the first Kerbal on a pod.

It will not put you on IVA, if there are:
* no Kerbals alive on the vessel,
* no IVA model.

The HUD can be automaticaly hide and the others camera / MapView can be disabled.

Keyboard shortcuts:
* "end" to recovery,
* "home" to EVA.

#### How to install it?

Unzip all files. Put the QuickMods folder in your KSP/GameData folder.

#### How to update it?

Unzip all files. Merge the new QuickMods folder with the old folder which is in your KSP/GameData folder.

#### How to uninstall it?

Delete the QuickMods/QuickIVA folder in your KSP/GameData folder.

#### Changelog

v1.20 - 2016.11.19
* New: Added support of the LanguagePatches,
* I've translated it to french, if you want more, you can translate and PR ;)

v1.18 - 2016.11.03
* Fix: Corrected directories for windows (thanks RealKolago).

v1.17 - 2016.11.02
* Compiled against KSP 1.2.1.1604

v1.16 - 2016.10.31
* New: Changed the directory to GameData/QuickMods/QuickIVA
* Fix: Corrected the stocktoolbar button scenes,
* Fix: Corrected the toolbar button init/destroy,
* Fix: Updated ToolbarWrapper to Toolbar 1.7.13,
* The default QuickMods repository is now: https://github.com/malahx/QuickMods

v1.15 - 2016.10.15
* Compiled against KSP 1.2.0.1586

v1.14 - 2016.09.14
* Compiled against KSP 1.2.0.1473

v1.13 - 2016.07.18
* Fix: Rewrite of the key lock to not use inputLockMask (thanks LtMtz),
* Fix: Corrected the reset of the Blizzy toolbar button,
* Fix: Enhanced the settings functions,
* Fix: Deleted useless libraries,
* Fix: Deleted foreach functions,
* Fix: Deleted support of Probe Control Room (when it will be updated to KSP 1.1.X, I will readd/rewrite my warpper),
* Compiled against KSP 1.1.3.1289

v1.12 - 2016.04.30
* Fix: Corrected many functions for KSP 1.1,
* Compiled against KSP 1.1.2.1260

v1.11 - 2015.06.25
* Updated to KSP 1.0.X

v1.10 - 2015.05.08
* New: Added the support of the mod Probe Control Room,
* Fix: (Again) corrected the Stock Toolbar which can show two buttons,

v1.03 - 2015.05.03
* Fix: Corrected an error on the GoEVA,
* Fix: Corrected the Stock Toolbar which can show two buttons,
* Fix: Enabled the loading of the settings at the Space Center,
* Updated ToolbarWrapper to 1.7.9
* Updated to KSP 1.0.2

v1.02 - 2015.04.28
* Fix: Converted textures to DDS,
* Fix: Corrected the Stock Toolbar icon not being created,
* Fix: Deleted the default QuickIVA's config file to keep your config file after an update,
* Fix: Some minor fixes,
* Updated to KSP 1.0.0

v1.01 - 2015.02.13
* Fix: Change the default EVA shortcut to "Home", (backspace is the default shortcut for abort).

v1.00 - 2015.02.07
* Initial release

#### Thanks!

* to Alshain and monstah to have requested this mod,
* to Mihara and MOARdv for the awesome mod RasterPropMonitor,
* to Tabakhase for the mod Probe Control Room,
* to blizzy for his Toolbar mod,
* to all mods developers which make this game really huge,
* to my friend Neimad who corrects my bad english ...
* to Squad for this awesome game.

#### Links

* http://forum.kerbalspaceprogram.com/index.php?/topic/85834-1
* http://spacedock.info/mod/107/QuickIVA
* http://kerbal.curseforge.com/ksp-mods/227809-quickiva
* https://github.com/malahx/QuickMods
* Toolbar: http://forum.kerbalspaceprogram.com/index.php?/topic/55420-1
* RasterPropMonitor: http://forum.kerbalspaceprogram.com/threads/117471
* Probe Control Room: http://forum.kerbalspaceprogram.com/threads/67450
