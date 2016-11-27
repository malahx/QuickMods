#!/usr/bin/bash
# Copyright 2016 Malah
# This is free and unencumbered software released into the public domain.

#https://api.github.com/repos/CYBUTEK/KerbalEngineer/releases/tags/1.1.0.1
URL=https://api.github.com/repos/CYBUTEK/KerbalEngineer/releases/latest

if [ -d KerbalEngineer ]; then
	echo "Déplacement de l'ancinne version et backup"
	if [ -d KerbalEngineer-old ]; then
		mv KerbalEngineer-old KerbalEngineer-$(date +%Y%m%d-%Hh%M-%S)
	fi
	mv KerbalEngineer KerbalEngineer-old
fi
echo "Téléchargement de la dernière version"
wget $(curl -s $URL | jq -r ".zipball_url") -O latest.zip -q
echo "Décompression de la dernière version"
unzip -q latest.zip 
rm latest.zip
mv CYBUTEK-KerbalEngineer-* KerbalEngineer
AVCFILE=$(cat KerbalEngineer/Output/KerbalEngineer/KerbalEngineer.version)
VERSION=$(echo $AVCFILE | jq -r ".VERSION.MAJOR").$(echo $AVCFILE | jq -r ".VERSION.MINOR").$(echo $AVCFILE | jq -r ".VERSION.PATCH").$(echo $AVCFILE | jq -r ".VERSION.BUILD")
echo "Tentative de nettoyage de KER $VERSION"
cat KerbalEngineer/KerbalEngineer/Helpers/Units.cs | sed -r 's/TimeFormatter.ConvertToString\(value\)/null/' > KerbalEngineer/KerbalEngineer/Helpers/Units-clean.cs && echo "Units.cs néttoyage de TimeFormatter réussi"
BuildAdvanced=$(cat KerbalEngineer/KerbalEngineer/Editor/BuildAdvanced.cs)
BuildAdvanced=$(echo "$BuildAdvanced" | sed -r 's:using (Unity;|Extensions|Flight|Helpers|KeyBinding|Settings|UIControls|VesselSimulator)://using \1:g') && echo "BuildAdvanced.cs néttoyage des imports réussi"
BuildAdvanced=$(echo "$BuildAdvanced" | sed -r 's|(\[KSPAddon\(KSPAddon.Startup.EditorAny, false\)\])|//\1|') && echo "BuildAdvanced.cs néttoyage de KSPAddon réussi"
echo "${BuildAdvanced%%public static float Altitude*}" > KerbalEngineer/KerbalEngineer/Editor/BuildAdvanced-clean.cs && echo "BuildAdvanced.cs néttoyage des fonctions inutiles"
echo "		public static float Altitude;
	}
}" >> KerbalEngineer/KerbalEngineer/Editor/BuildAdvanced-clean.cs
echo $(cat Properties/AssemblyInfo.cs | sed -r "s/AssemblyVersion \(\".*?\"\)/AssemblyVersion (\"$VERSION\")/") > Properties/AssemblyInfo.cs && echo "Ajout du numéro de version"
echo "Fin du nettoyage - Ne pas oublier de vérifier le code ;)"