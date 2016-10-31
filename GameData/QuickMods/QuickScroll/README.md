#### QuickScroll
#### A plugin for Kerbal Space Program 1.0.X
#### Copyright 2015 Malah

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

QuickScroll is a small plugin which adds the possibility to scroll the parts pages, the categories and the filters on the editor with the mouse wheel and with keyboard shortcuts.

Shortcut:
    - When the mouse is on the part list:
        * Left CTRL + mouse wheel = scroll categories,
        * Left Shift + mouse wheel = scroll filters.
    - When the mouse is on the categories:
        * Left Shift or Left CTRL + mouse wheel = scroll filters.
    - Switch the default categories with Enter + KeyPad 1, KeyPad 2, KeyPad 3 ...
	- Previous/next category Enter + up/down.
	- Previous/next filter Enter + page up/page down.

I suggest you to look at the awesome mod PartCatalog which also has this feature (with many others nice features).

#### How to install it?

Unzip all files. Put the QuickScroll folder in your KSP/GameData folder.

#### How to update it?

Unzip all files. Merge the new QuickScroll folder with the old folder which is in your KSP/GameData folder.

#### How to uninstall it?

Delete the QuickScroll folder in your KSP/GameData folder.

#### Changelog

v2.0.0-pre1 - 2016.04.01
* New: Added a page scrolling,
* New: Deleted support of the keyboard shortcut for parts page scrolling,
* New: Deleted all optional versions of QuickScroll,
* New: Deleted PartListTooltipsTWEAK (the auto hide of the tooltips),
* New: Deleted the autofocus on the good category with huge filters (it will use the stock scrolling),
* Compiled against KSP 1.1.0.1174

v1.32 - 2015.10.14
* New: Added an option to disable the hovering on the Stock Toolbar,
* Fix: Corrected the position of the scroll of the partlist,
* Fix: Locked the parts scrolling when the PartToolTips is pinned (we now can scroll the part info).

v1.31 - 2015.06.26
* Fix: Deleted the debug warning.

v1.30 - 2015.06.25
* New: Added an option to block the scrolling at the beginning and at the end of a category (enabled by default),
* New: Added scroll to enable simple/advanced mode,
* New: Added a button to return to the default key assignment,
* Fix: Corrected the key assignment to accept special's key,
* Fix: Corrected the stock scrolling when there are many categories/filters (thanks to Alewx to remind me of this old bug which I had forgot to correct),
* Fix: Corrected the part page scroll which always block at the beggining / end of the pages,
* Fix: Changed the default key for Modifier Keyboard to Keypad Enter,
* Fix: Enhanced the Stock Toolbar,
* Fix: Some other minor fixes,
* Optional: Added an optional version of QuickScroll with only the mouse scrolling,
* Optional: Added an optional version of QuickScroll with only the keyboard shortcuts,
* Updated to KSP 1.0.X

v1.22 - 2015.05.03
* Fix: Corrected the Stock Toolbar which can show two buttons,
* Updated ToolbarWrapper to 1.7.9
* Updated to KSP 1.0.2

v1.21 - 2015.04.28
* Fix: Converted textures to DDS,
* Fix: Corrected the Stock Toolbar icon not being created,
* Fix: Corrected the loading of the config file (thanks Tarheel1999),
* Fix: Some minor bug,
* Optional: Added an optional version of QuickScroll without GUI and Stock/Blizzy Toolbar support,
* Updated to KSP 1.0.0

v1.20 - 2015.03.06
* New: Added a GUI to config the shortcuts,
* New: Added the support of the Stocktoolbar and the Toolbar mod,
* Fix: Corrected the PartListTooltips tweak.

v1.10 - 2015.01.22
* New: Added enable/disable the mouse wheel scrolling,
* New: Added enable/disable the shortcuts mouse wheel scrolling,
* New: Added keyboard shortcuts,
* New: Added a tweak for the PartListTooltips, with it, the tooltips can't popup until you right click on a part (this feature is disabled by default),
* Fix: Move the config file to GameData/QuickScroll/Config.txt

v1.01 - 2015.01.02
* Fix: Hide the part tool tip on scroll.

v1.00 - 2014.12.25
* New: Added a scroll on the categories buttons,
* New: Added a scroll on the filters buttons,
* New: Added a config file for the Key modifier.

v0.10 - 2014.12.21
* Initial release

#### Troubleshooting?

If you use the search part function with an empty text, when you unfocus the search field, the scrollbar will be locked.

#### Thanks!

* to BlackNecro for his awesome PartCatalog mod,
* to blizzy for his Toolbar mod,
* to Matthieu James for the Faenza icon theme,
* to all mods developers which make this game really huge,
* to my friend Neimad who corrects my bad english ...
* to Squad for this awesome game.

#### Links

* http://forum.kerbalspaceprogram.com/threads/95168#QuickScroll
* https://kerbalstuff.com/mod/436/QuickScroll
* http://kerbal.curseforge.com/ksp-mods/226335-quickscroll
* https://github.com/malahx/QuickScroll
* PartCatalog: http://forum.kerbalspaceprogram.com/threads/35018
* Toolbar: http://forum.kerbalspaceprogram.com/threads/60863
* Faenza icon theme: http://gnome-look.org/content/show.php/Faenza?content=128143
