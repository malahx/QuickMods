#!/bin/bash
MODS=$1
cp -f bin/${MODS}.dll GameData/QuickMods/${MODS}/Plugins/
if [ -e Lang ]; then
        rm -rf Lang/*~
        cp -rf Lang/ GameData/QuickMods/${MODS}/
fi
cp -f README.md GameData/QuickMods/${MODS}/
cp -f COPYING GameData/QuickMods/${MODS}/
cp -f ${MODS}.version GameData/QuickMods/${MODS}/
rm -rf ../../00KSP-dev/GameData/QuickMods/${MODS}
cp -rf GameData/QuickMods ../../00KSP-dev/GameData/
cp -rf GameData/QuickMods ../GameData/
