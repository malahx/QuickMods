# QuickMods
## A plugin collection for Kerbal Space Program
### Copyright 2024 Malah

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org/>

### What is it?

QuickMods is a tiny plugin that adds a few small features.

Features: 
* Brake: automatic brake vessel at load/control lost and toggle brake with key.
* Pause: pause the game when you escape it.
* PrecisionControl: force precision control at flight loading, it also adds notification when you toggle precision control mode.
* Revert: lost revert when you pass the atmosphere.
* Scroll: scroll the R&D view when click mouse.
* StopWarp: stop warp when vessel situation change.
* VesselNames: add a new vessel name when you create a new vessel in the VAB.

All the features are disabled by default.
To enable them, go to Settings -> Mods -> QuickMods -> Choose what you want.

#### VesselNames

All the vessel names are located in the folder <your installation path>\Kerbal Space Program 2\BepInEx\plugins\QuickMods\VesselNames

**ATTENTION: if you update QuickMods you will overwrite any changes made the txt files.**

By default, vessel names differ by vessel type. However, with the CustomVesselNames option, the vessel type is ignored.
To use this feature, you must enable it AND add a new file named CustomVesselNames.txt in the VesselNames folder. This file will not be overwritten when updating QuickMods.

### How to install/update it?

* Install BepInEx + SpaceWarp
* Extract the contents of the downloaded zip file into your KSP2 installation folder

### How to uninstall it?

* Delete the folder <your installation path>\Kerbal Space Program 2\BepInEx\plugins\QuickMods

### Thanks!

* to Linuxgurugamer for his incredible work,
* to all mod developers who made these games really huge,
* and to Squad & Intercept Games for creating these awesome games.
