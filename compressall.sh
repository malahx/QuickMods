#!/bin/bash
FILE=${1}-$(date +%F).zip
if [ -e ../00RELEASE/$FILE ]; then
	echo "This version seems to have been compressed: $FILE"
	exit 0
fi
zip -rv $FILE GameData/ README.md COPYING
mv -v $FILE ../00RELEASE/
