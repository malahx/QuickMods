#!/bin/bash
MOD=$1
JSON=$(cat ${MOD}.version | jq '.VERSION')
VERSION=$(echo $JSON | jq '.MAJOR').$(echo $JSON | jq '.MINOR')$(echo $JSON | jq '.PATCH')
FILE=${MOD}-v${VERSION}.zip
if [ -e ../../00RELEASE/$FILE ]; then
	echo "This version seems to have been compressed: $FILE"
	exit 0
fi
zip -rv $FILE GameData/QuickMods/${MOD} README.md COPYING
mv -v $FILE ../../00RELEASE/
