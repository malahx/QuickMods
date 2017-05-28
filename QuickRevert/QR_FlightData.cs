/* 
QuickRevert
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
using KSP.Localization;

namespace QuickRevert {
	public class QFlightData : QuickRevert {

		public bool hasLoaded = false;

		static string FileFlightState = PATH + "/PluginData/{0}-flightstate.txt";

		static string PathFlightState {
			get {
				if (!HighLogic.LoadedSceneIsGame) {
					return string.Empty;
				}
				return string.Format (FileFlightState, HighLogic.SaveFolder);
			}
		}

		static bool ConfigNodeHasPostInitState(ConfigNode nodes) {
			return nodes.HasNode ("PostInitState");
		}

		private static bool ConfigNodeHasPreLaunchState(ConfigNode nodes) {
			return nodes.HasNode ("PreLaunchState") &&
				nodes.HasValue ("newShipFlagURL") &&
				nodes.HasValue ("newShipToLoadPath") &&
				nodes.HasValue ("ShipType");
		}

		static bool isHardSaved {
			get {
				if (!HighLogic.LoadedSceneIsGame) {
					return false;
				}
				return File.Exists (PathFlightState);
			}
		}

		static bool CanStorePostInitState {
			get {
				if (!HighLogic.LoadedSceneIsFlight) {
					return false;
				}
				return FlightDriver.PostInitState != null;
			}
		}

		static bool CanStorePreLaunchState {
			get {
				if (!HighLogic.LoadedSceneIsFlight) {
					return false;
				}
				return FlightDriver.PreLaunchState != null &&
					FlightDriver.newShipFlagURL != string.Empty &&
					FlightDriver.newShipToLoadPath != string.Empty &&
					FlightDriver.newShipManifest != null &&
					ShipConstruction.ShipConfig != null &&
					ShipConstruction.ShipType != EditorFacility.None;
			}
		}

		public QFlightData() {
			Reset ();
		}
		public string newShipToLoadPath {
			get;
			private set;
		}
		public string newShipFlagURL {
			get;
			private set;
		}
		public VesselCrewManifest newShipManifest {
			get;
			private set;
		}
		public GameBackup PostInitState {
			get;
			private set;
		}
		public GameBackup PreLaunchState {
			get;
			private set;
		}
		public ConfigNode ShipConfig {
			get;
			private set;
		}
		public EditorFacility ShipType {
			get;
			private set;
		}

		public bool CanFindVessel {
			get {
				return HighLogic.LoadedSceneHasPlanetarium && FlightGlobals.Vessels.Count > 0;
			}
		}
		public Guid VesselGuid {
			get {
				if (PostInitState == null) {
					return Guid.Empty;
				}
				return PostInitState.ActiveVesselID;
			}
		}
		public int activeVesselIdx {
			get {
				if (PostInitState == null) {
					return -1;
				}
				return PostInitState.ActiveVessel;
			}
		}
		public int currentActiveVesselIdx {
			get {
				Guid _guid = VesselGuid;
				if (_guid == Guid.Empty) {
					return -1;
				}
				if (CanFindVessel) {
					return FlightGlobals.Vessels.FindLastIndex(v => v.id == _guid);
				}
				return HighLogic.CurrentGame.Updated ().flightState.protoVessels.FindLastIndex (pv => pv.vesselID == _guid);
			}
		}
		public bool VesselExists {
			get {
				Guid _guid = VesselGuid;
				if (_guid == Guid.Empty) {
					return false;
				}
				if (CanFindVessel) {
					return FlightGlobals.Vessels.Exists(v => v.id == _guid);
				}
				return HighLogic.CurrentGame.Updated().flightState.protoVessels.Exists(pv => pv.vesselID == _guid);
			}
		}
		public ProtoVessel pVessel {
			get {
				Guid _guid = VesselGuid;
				if (_guid == Guid.Empty) {
					return null;
				}
				if (CanFindVessel) {
					Vessel _vessel = FlightGlobals.Vessels.FindLast (v => v.id == _guid);
					if (_vessel != null) {
						return _vessel.protoVessel;
					}
				}
				return HighLogic.CurrentGame.Updated().flightState.protoVessels.FindLast (pv => pv.vesselID == _guid);
			}
		}
		public Vessel vessel {
			get {
				Guid _guid = VesselGuid;
				if (_guid == Guid.Empty || !CanFindVessel) {
					return null;
				}
				return FlightGlobals.Vessels.FindLast (v => v.id == _guid);
			}
		}
		public bool isActiveVessel {
			get {
				Guid _guid = VesselGuid;
				if (_guid == Guid.Empty || FlightGlobals.ActiveVessel == null) {
					return false;
				}
				return _guid == FlightGlobals.ActiveVessel.id;
			}
		}
		public bool PostInitStateIsSaved {
			get {
				return PostInitState != null;
			}
		}
		public bool PreLaunchStateIsSaved {
			get {
				return PreLaunchState != null &&
					newShipToLoadPath != string.Empty &&
					newShipFlagURL != string.Empty &&
					newShipManifest != null &&
					ShipType != EditorFacility.None;
			}
		}
		public bool Store() {
			if (CanStorePostInitState) {
				ConfigNode _flightstate = new ConfigNode ();
				PostInitState = FlightDriver.PostInitState;
				FlightDriver.CanRevertToPostInit = true;
				_flightstate.AddNode ("PostInitState").AddData (PostInitState.Config);
				Log ("PostInitState stored", "QFlightData");
				if (CanStorePreLaunchState) {
					PreLaunchState = FlightDriver.PreLaunchState;
					FlightDriver.CanRevertToPrelaunch = true;
					newShipToLoadPath = FlightDriver.newShipToLoadPath;
					newShipFlagURL = FlightDriver.newShipFlagURL;
					newShipManifest = FlightDriver.newShipManifest;
					ShipConfig = ShipConstruction.ShipConfig;
					ShipType = ShipConstruction.ShipType;
					_flightstate.AddNode ("PreLaunchState").AddData (PreLaunchState.Config);
					_flightstate.AddValue ("newShipFlagURL", newShipFlagURL);
					_flightstate.AddValue ("newShipToLoadPath", newShipToLoadPath);
					_flightstate.AddNode ("ShipConfig").AddData (ShipConfig);
					_flightstate.AddValue ("ShipType", (ShipType == EditorFacility.SPH ? "SPH" : "VAB"));
					Log ("PreLaunchState stored", "QFlightData");
				}
				try {
					_flightstate.Save (PathFlightState);
					Log ("Data saved to file", "QFlightData");
					return true;
				}
				catch (Exception e) {
					Warning (string.Format ("Can't save to file flight state: {0}", e), "QFlightData");
				}
			}
			Log ("No data to store", "QFlightData");
			return false;
		}
		public bool Restore() {
			if (PostInitStateIsSaved) {
				FlightDriver.PostInitState = PostInitState;
				FlightDriver.CanRevertToPostInit = true;
				Log ("PostInitState restored", "QFlightData");
				if (PreLaunchStateIsSaved) {
					FlightDriver.PreLaunchState = PreLaunchState;
					FlightDriver.CanRevertToPrelaunch = true;
					FlightDriver.newShipToLoadPath = newShipToLoadPath;
					FlightDriver.newShipFlagURL = newShipFlagURL;
					FlightDriver.newShipManifest = newShipManifest;
					ShipConstruction.ShipConfig = ShipConfig;
					ShipConstruction.ShipType = ShipType;
					ShipConstruction.ShipManifest = newShipManifest;
					Log ("PreLaunchState restored", "QFlightData");
				}
				return true;
			}
			Log ("No data to restore", "QFlightData");
			return false;
		}
		public bool Load() {
			hasLoaded = true;
			if (isHardSaved) {
				ConfigNode _flightstate = ConfigNode.Load (PathFlightState);
				if (ConfigNodeHasPostInitState (_flightstate)) {
					Game _gamePostInit = new Game (_flightstate.GetNode ("PostInitState"));
					if (!_gamePostInit.compatible) {
						Warning ("Post Init State is not compatible.", "QFlightData");
						Reset ();
						return false;
					}
					PostInitState = new GameBackup (_gamePostInit);
					Log ("PostInitState loaded", "QFlightData");
					if (ConfigNodeHasPreLaunchState (_flightstate)) {
						Game _gamePreLaunch = new Game (_flightstate.GetNode ("PreLaunchState"));
						if (!_gamePreLaunch.compatible) {
							Warning ("Pre Launch State is not compatible.", "QFlightData");
							Reset ();
							return false;
						}
						PreLaunchState = new GameBackup (_gamePreLaunch);
						newShipFlagURL = _flightstate.GetValue ("newShipFlagURL");
						newShipToLoadPath = _flightstate.GetValue ("newShipToLoadPath");
						ShipConfig = _flightstate.GetNode ("ShipConfig");
						ShipType = (_flightstate.GetValue ("ShipType") == "SPH" ? EditorFacility.SPH : EditorFacility.VAB);
						newShipManifest = _gamePreLaunch.CrewRoster.DefaultCrewForVessel (ShipConfig, null);
						Log ("PostInitState loaded", "QFlightData");
					}
					Log ("Data loaded from file", "QFlightData");
					return true;
				} else {
					Warning ("Flight state is not correctly saved.", "QFlightData");
					Reset ();
				}
			} else {
				Warning ("Nothing to load.", "QFlightData");
				Reset ();
			}
			return false;
		}
		public void Reset(bool complete = false) {
			if (HighLogic.LoadedSceneIsFlight) {
				FlightDriver.CanRevertToPostInit = false;
				FlightDriver.CanRevertToPrelaunch = false;
			}
			if (HighLogic.LoadedSceneIsGame && pVessel != null) {
				string _vesselName = pVessel.vesselName;
				Log ("You have lost the possibility to revert: " + _vesselName, "QFlightData");
				ScreenMessages.PostScreenMessage (Localizer.Format("quickrevert_revertLost", MOD, _vesselName), 10, ScreenMessageStyle.UPPER_CENTER);
			}
			PostInitState = null;
			PreLaunchState = null;
			newShipToLoadPath = string.Empty;
			newShipFlagURL = string.Empty;
			newShipManifest = null;
			ShipConfig = null;
			ShipType = EditorFacility.None;
			if (PathFlightState != string.Empty) {
				if (File.Exists (PathFlightState)) {
					File.Delete (PathFlightState);
				}
			}
			if (complete) {
				hasLoaded = false;
			}
			Log ("Data reset", "QFlightData");
		}
	}
}