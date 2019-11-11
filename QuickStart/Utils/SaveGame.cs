/* 
QuickStart
Copyright 2017 Malah

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
*/

using System;
using System.IO;

namespace QuickStart.QUtils {

    public static class QSaveGame {

        static ConfigNode saveGame;

        public static string vesselName;
        public static string vesselType;

        internal static readonly string FOLDER = KSPUtil.ApplicationRootPath + "saves";
        internal static readonly string FILE = "persistent";
        internal static readonly string EXT = ".sfs";

        public static string saveFile {
            get {
                return string.Format("{0}/{1}{2}", saveDir, FILE, EXT);
            }
        }

        public static string saveDir {
            get {
                return string.Format("{0}/{1}", FOLDER, LastUsed);
            }
        }

        public static string[] gameBlackList = {
            "scenarios",
            "training",
            "missions"
        };

        static bool isBlackList(string game) {
            for (int _i = gameBlackList.Length - 1; _i >= 0; --_i) {
                if (gameBlackList[_i] == game) {
                    return true;
                }
            }
            return false;
        }

        static int indexSave = -1;

        [KSPField(isPersistant = true)]
        static string lastUsed;
        public static string LastUsed {
            get {
                if (lastUsed == null) {
                    DateTime _lastWriteTime = DateTime.MinValue;
                    string _lastDirectoryUsed = string.Empty;
                    DirectoryInfo[] _directories = new DirectoryInfo(FOLDER).GetDirectories();
                    for (int _i = _directories.Length - 1; _i >= 0; --_i) {
                        DirectoryInfo _directory = _directories[_i];
                        FileInfo[] _files = _directory.GetFiles();

                        FileInfo _file = Array.Find(_files, f => f.Name == FILE + EXT);
                        if (_file != null) {
                            if (_file.LastWriteTime > _lastWriteTime) {
                                _lastWriteTime = _file.LastWriteTime;
                                _lastDirectoryUsed = _directory.Name;
                                indexSave = _i;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(_lastDirectoryUsed) || isBlackList(_lastDirectoryUsed)) {
                        _lastDirectoryUsed = string.Empty;
                    }
                    lastUsed = _lastDirectoryUsed;
                    UpdateSave(true);
                    QDebug.Log("Last savegame used found: " + lastUsed, "QSaveGame");
                }
                return lastUsed;
            }
        }

        public static void Prev()
        {
            DirectoryInfo[] _directories = new DirectoryInfo(FOLDER).GetDirectories();
            for (int _i = _directories.Length - 1; _i >= 0; --_i)
            {
                indexSave--;
                if (indexSave < 0)
                    indexSave = _directories.Length - 1;
                if (indexSave >= _directories.Length)
                {
                    indexSave = 0;
                }
                if (!isBlackList(_directories[indexSave].Name))
                {
                    break;
                }
            }
            lastUsed = _directories[indexSave].Name;
            UpdateSave(true);
            QDebug.Log("Prev savegame found: " + lastUsed, "QSaveGame");
        }

        public static void Next() {
            DirectoryInfo[] _directories = new DirectoryInfo(FOLDER).GetDirectories();
            for (int _i = _directories.Length - 1; _i >= 0; --_i) {
                indexSave++;
                if (indexSave >= _directories.Length) {
                    indexSave = 0;
                }
                if (!isBlackList(_directories[indexSave].Name)) {
                    break;
                }
            }
            lastUsed = _directories[indexSave].Name;
            UpdateSave(true);
            QDebug.Log("Next savegame found: " + lastUsed, "QSaveGame");
        }

        static bool vesselIsOK(ConfigNode node) {
            if (node == null || !node.HasValue("type")) {
                return false;
            }
            return node.GetValue("type") != VesselType.Debris.ToString() && node.GetValue("type") != VesselType.Flag.ToString() && node.GetValue("type") != VesselType.SpaceObject.ToString() && node.GetValue("type") != VesselType.Unknown.ToString();
        }

        static bool vesselIsOK(ProtoVessel pv) {
            return pv.vesselType != VesselType.Debris && pv.vesselType != VesselType.Flag && pv.vesselType != VesselType.SpaceObject && pv.vesselType != VesselType.Unknown;
        }

        static void UpdateSave(bool force = false) {
            if (saveGame == null || force) {
                saveGame = ConfigNode.Load(saveFile);
            }
            if (saveGame != null) {
                if (saveGame.HasNode("GAME")) {
                    if (hasVesselNode) {
                        ConfigNode _flightState = saveGame.GetNode("GAME").GetNode("FLIGHTSTATE");
                        ConfigNode[] _vessels = _flightState.GetNodes("VESSEL");
                        ConfigNode _lastVessel = null;
                        if (_flightState.HasValue("activeVessel")) {
                            string _lastVesselIdx = _flightState.GetValue("activeVessel");
                            _lastVessel = _flightState.GetNode("VESSEL", int.Parse(_lastVesselIdx));
                            if (!vesselIsOK(_lastVessel)) {
                                _lastVessel = null;
                                QDebug.Log("No activeVessel found", "QSaveGame");
                            } else {
                                QDebug.Log("activeVessel found idx: " + _lastVesselIdx, "QSaveGame");
                            }
                        }
                        if (_lastVessel == null) {
                            double _lastLCT = 0;
                            for (int _i = _vessels.Length - 1; _i >= 0; --_i) {
                                ConfigNode _vessel = _vessels[_i];
                                if (_vessel.HasValue("lct") && vesselIsOK(_vessel)) {
                                    double _lct = double.Parse(_vessel.GetValue("lct"));
                                    if (_lct > _lastLCT) {
                                        _lastVessel = _vessel;
                                        _lastLCT = _lct;
                                    }
                                }
                            }
                            if (_lastVessel == null) {
                                QDebug.Log("No last launched vessel found", "QSaveGame");
                            } else {
                                QDebug.Log("Last launched vessel found", "QSaveGame");
                            }
                        }
                        if (_lastVessel != null) {
                            QuickStart_Persistent.vesselID = _lastVessel.GetValue("pid");
                            vesselName = _lastVessel.GetValue("name");
                            vesselType = _lastVessel.GetValue("type");
                            if (!vesselIsOK(_lastVessel)) {
                                _lastVessel = null;
                                QDebug.Log("No lastVessel found (activeVessel or last launched vessel)", "QSaveGame");
                            } else {
                                QDebug.Log(string.Format("lastVessel: {0}({1})[{2}]", vesselName, vesselType, QuickStart_Persistent.vesselID), "QSaveGame");
                            }
                        } else {
                            QDebug.Log("No lastVessel found (activeVessel or last launched vessel)", "QSaveGame");
                        }
                    }
                    ConfigNode[] _nodes = saveGame.GetNode("GAME").GetNodes("SCENARIO");
                    ConfigNode _node = Array.Find(_nodes, n => n.GetValue("name") == "QuickStart_Persistent");
                    if (_node != null) {
                        if (hasVesselNode) {
                            if (_node.HasValue("vesselID")) {
                                string _vesselName;
                                string _vesselType;
                                if (Exists(_node.GetValue("vesselID"), out _vesselName, out _vesselType)) {
                                    QuickStart_Persistent.vesselID = _node.GetValue("vesselID");
                                    vesselName = _vesselName;
                                    vesselType = _vesselType;
                                    QDebug.Log(string.Format("currentVessel: {0}({1})[{2}]", vesselName, vesselType, QuickStart_Persistent.vesselID), "QSaveGame");
                                } else {
                                    QDebug.Log("currentVessel not exist", "QSaveGame");
                                }
                            } else {
                                QDebug.Log("No currentVessel found", "QSaveGame");
                            }
                        } else {
                            QDebug.Log("There's no vessel on this savegame", "QSaveGame");
                        }
                    } else {
                        QDebug.Log("No Scenario found", "QSaveGame");
                    }
                }
            }
            if (QuickStart_Persistent.vesselID == string.Empty) {
                if (QSettings.Instance.gameScene == (int)GameScenes.FLIGHT) {
                    QSettings.Instance.gameScene = (int)GameScenes.SPACECENTER;
                }
            }
            QDebug.Log("Savegame loaded: " + LastUsed, "QSaveGame");
        }

        static bool Exists(string vesselID, out string vName, out string vType) {
            vName = string.Empty;
            vType = string.Empty;
            if (!string.IsNullOrEmpty(vesselID)) {
                if (hasVesselNode) {
                    ConfigNode[] _nodes = saveGame.GetNode("GAME").GetNode("FLIGHTSTATE").GetNodes("VESSEL");
                    ConfigNode _node = Array.Find(_nodes, n => new Guid(n.GetValue("pid")) == new Guid(vesselID));
                    if (_node != null) {
                        if (!_node.HasValue("name") || _node.HasValue("type")) {
                            return false;
                        }
                        vName = _node.GetValue("name");
                        vType = _node.GetValue("type");
                        return true;
                    }
                }
            }
            return false;
        }

        static bool hasVesselNode {
            get {
                if (saveGame != null && saveGame.HasNode("GAME") && saveGame.GetNode("GAME").HasNode("FLIGHTSTATE") && saveGame.GetNode("GAME").GetNode("FLIGHTSTATE").HasNode("VESSEL")) {
                    return true;
                }
                return false;
            }
        }
    }
}

