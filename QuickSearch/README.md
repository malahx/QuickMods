#### QuickSearch
#### A plugin for Kerbal Space Program
#### Copyright 2017 Malah

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

QuickSearch is a small plugin which adds an extension to the part search function on the editor, it also adds a search parts on the tech tree.

#### What is the search extension?

By default, the search extension is enabled, and with it you can:
* search with regex 				/ab/,
* make a AND 					a&b,
* make a OR 					a|b,
* make a NOT 					!a,
* search a word begin 				(ab or "ab,
* search a word end 				ab) or ab",
* search a word 				(ab) or "ab",
* search only the tag				%ab,
* search only the name 				;ab,
* search only the title 			:ab,
* search only the description			-ab,
* search only the author			,ab,
* search only the manufacturer			?ab,
* search only the part size			.ab,
* search only the resources			+ab,
* search only the tech required			@ab,
* search only the module			_ab,
a and b are the search example, all the shortcut can be edited on the config file.
At this time, you can only cumulate AND, OR, NOT, words begin/end/full, but not with others shortcut.

#### How to install it?

Unzip all files. Put the QuickMods folder in your KSP/GameData folder.

#### How to update it?

Unzip all files. Merge the new QuickMods folder with the old folder which is in your KSP/GameData folder.

#### How to uninstall it?

Delete the QuickMods/QuickSearch folder in your KSP/GameData folder.

#### Changelog

v3.12 - 2017.01.14
* Fix: Corrected the error with LanguageAPI and unloaded plugins.

v3.11 - 2016.12.09
* Compiled against KSP 1.2.2.1622

v3.10 - 2016.11.19
* New: Added support of the LanguagePatches,
* I've translated it to french, if you want more, you can translate and PR ;)

v3.08 - 2016.11.03
* Fix: Corrected directories for windows (thanks RealKolago).

v3.07 - 2016.11.02
* Compiled against KSP 1.2.1.1604

v3.06 - 2016.10.31
* New: Changed the directory to GameData/QuickMods/QuickSearch
* Fix: Updated ToolbarWrapper to Toolbar 1.7.13,
* Fix: Corrected the stocktoolbar button,
* Fix: Enhanced the settings functions,
* The default QuickMods repository is now: https://github.com/malahx/QuickMods

v3.05 - 2016.10.14
* Compiled against KSP 1.2.0.1586

v3.04 - 2016.09.14
* Compiled against KSP 1.2.0.1473

v3.03 - 2016.08.30
* Fix: Corrected the reset of the Blizzy toolbar button,
* Fix: Corrected a bug with mods like Fog of Tech (by linuxgurugamer),
* Fix: Deleted useless libraries,
* Fix: Deleted linq library,
* Fix: Deleted foreach functions,
* Fix: Minors tweaks.

v3.02 - 2016.06.23
* Fix: Compiled against KSP 1.1.3.1289

v3.01 - 2016.05.10
* Fix: Corrected the search key lock which block the camera,
* Fix: Added a Keyboard lock when the search bar is active.

v3.00 - 2016.05.09
* New: Added some new search parameter,
* New: Enhanced the editor search,
* New: Added a GUI to edit the config (with the support of Stock and Blizzy Toolbar),
* Fix: Compiled against KSP 1.1.2.1260

v2.01 - 2016.04.23
* Fix: Enhanced the dimension of the search bar,
* Fix: Compiled against KSP 1.1.0.1230

v2.00-pre1 - 2016.03.30
* New: Deleted parts search functions from the editor,
* New: Deleted category functions from the editor,
* New: Deleted the tiny version of QuickSearch,
* Fix: Compiled against KSP 1.1.0.1172

v1.21 - 2016.02.08
* Fix: Corrected GUIStyle which block popups.

v1.20 - 2016.02.08
* New: Added part search functions for the tech tree,
* Fix: Compiled against KSP 1.0.5

v1.14 - 2015.10.14
* Fix: Corrected the part's search function for mods compatibility (part.category == none),
* Fix: Corrected the invalid character strip to accept this character: ",
* Fix: Enhanced the position of the search bar to fit with others resolutions,
* Optional: Added an optional version of QuickSearch with only the search bar (no back/bookmark functions as QuickSearch v0.10).

v1.13 - 2015.06.25
* Updated to KSP 1.0.X

v1.12 - 2015.05.03
* Updated to KSP 1.0.2

v1.11 - 2015.04.28
* Fix: Converted textures to DDS,
* Fix: Some minor bug,
* Updated to KSP 1.0.0

v1.10 - 2015.03.06
* New: Added a subassembly search,
* Fix: Corrected the search to accept a space.

v1.00 - 2015.02.17
* New: Added logical parameters (and &, or |) example mk1&mk2|mk3 is (mk1 and mk2) or mk3, space will no more be a or
* New: Added a search with regex (/patern/) example /mk[1-3]/,
* New: Added a back button to return to the previous category,
* New: Added a bookmark button to save your search,
* New: Added an automatic clear of the text field if you change the filter/category,
* Fix: Corrected how QuickSeach adds the filters and subcategories to be better integrated, 
* Fix: Corrected an error which can block the text field with some mods (thanks SWAGATRON),
* Fix: Some other minor fixes.

v0.10 - 2015.01.02
* Initial release

#### Thanks!

* to Borisbee to have requested this mod,
* to inigma and Yemo to have think of the tech tree part search,
* to linuxgurugamer for his help,
* to Magico13 for his Tree Toppler,
* to BlackNecro for his awesome PartCatalog,
* to Crzyrndm for his Filter Extensions,
* to simon56modder and Thomas P. for the Language Patches Project,
* to blizzy for his Toolbar mod, 
* to Banbury for his Part Search,
* to MrHappyFace for his PartSearch,
* to Konraden for his Part Search Plugin,
* to Matthieu James for the Faenza icon theme,
* to all mods developers which make this game really huge,
* to my friend Neimad who corrects my bad english ...
* to Squad for this awesome game.

#### Links

* http://forum.kerbalspaceprogram.com/index.php?/topic/85834-1
* http://spacedock.info/mod/101/QuickSearch
* http://kerbal.curseforge.com/ksp-mods/226668-quicksearch
* https://github.com/malahx/QuickMods
* Language Patches Project: http://forum.kerbalspaceprogram.com/index.php?/topic/85611-l
* Tree Toppler: http://forum.kerbalspaceprogram.com/index.php?/topic/97033-
* PartCatalog: http://forum.kerbalspaceprogram.com/threads/35018
* Filter Extensions: http://forum.kerbalspaceprogram.com/threads/104231
* Toolbar: http://forum.kerbalspaceprogram.com/index.php?/topic/55420-1
* Part Search: http://forum.kerbalspaceprogram.com/threads/95352
* PartSearch: http://forum.kerbalspaceprogram.com/threads/102375
* Part Search Plugin: http://forum.kerbalspaceprogram.com/threads/32983
* Faenza icon theme: http://gnome-look.org/content/show.php/Faenza?content=128143
