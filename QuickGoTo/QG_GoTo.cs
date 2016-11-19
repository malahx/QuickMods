/* 
QuickGoTo
Copyright 2015 Malah

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

using KSP.UI.Screens;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QuickGoTo {
	public partial class QGoTo {

		public enum GoTo {
			None,
			TrackingStation,
			SpaceCenter,
			MissionControl,
			Administration,
			RnD,
			AstronautComplex,
			VAB,
			SPH,
			LastVessel,
			Recover,
			Revert,
			RevertToEditor,
			RevertToSpaceCenter,
			MainMenu,
			Settings,
			Configurations
		}	

		public string GetText(GoTo goTo, bool force = false) {
			switch (goTo) {
			case GoTo.TrackingStation:
				return QLang.translate ("Go to the Tracking Station");
			case GoTo.SpaceCenter:
				return QLang.translate ("Go to the Space Center");
			case GoTo.MissionControl:
				return QLang.translate ("Go to the Mission Control");
			case GoTo.Administration:
				return QLang.translate ("Go to the Administration");
			case GoTo.RnD:
				return QLang.translate ("Go to the Research and Dev.");
			case GoTo.AstronautComplex:
				return QLang.translate ("Go to the Astronaut Complex");
			case GoTo.VAB:
				return QLang.translate ("Go to the Vehicle Assembly");
			case GoTo.SPH:
				return QLang.translate ("Go to the Space Plane Hangar");
			case GoTo.LastVessel:
				QData _lastVessel = LastVesselLastIndex ();
				return string.Format ("{0} {1}", QLang.translate ("Go to the"), (_lastVessel != null && !force ? QLang.translate ("vessel") + ": " + _lastVessel.protoVessel.vesselName : QLang.translate ("last Vessel")));
			case GoTo.Recover:
				return QLang.translate ("Recover");
			case GoTo.Revert:
				return QLang.translate ("Revert to Launch");
			case GoTo.RevertToEditor:
				return QLang.translate ("Revert to Editor");
			case GoTo.RevertToSpaceCenter:
				return QLang.translate ("Revert to SpaceCenter");
			case GoTo.MainMenu:
				return QLang.translate ("Go to The Main Menu");
			case GoTo.Settings:
				return QLang.translate ("Go to the Settings");
			case GoTo.Configurations:
				return string.Format("{0}: {1}", MOD, QLang.translate ("Settings"));
			}
			return string.Empty;
		}

		[KSPField(isPersistant = true)] internal static List<QData> LastVessels = new List<QData>();
		[KSPField(isPersistant = true)] static GoTo SavedGoTo = GoTo.None;

		string SaveGame = "persistent";

		public bool CanMainMenu {
			get {
				if (!HighLogic.LoadedSceneIsFlight) {
					return true;
				}
				if (FlightGlobals.ready && FlightGlobals.ActiveVessel != null && HighLogic.CurrentGame.Parameters.Flight.CanLeaveToMainMenu) {
					return FlightGlobals.ClearToSave() == ClearToSaveStatus.CLEAR;
				}
				return false;
			}
		}

		public bool CanTrackingStation {
			get {
				if (HighLogic.LoadedSceneIsGame) {
					if (HighLogic.LoadedScene != GameScenes.TRACKSTATION) {
						if (!HighLogic.LoadedSceneIsFlight) {
							return true;
						}
						if (FlightGlobals.ready && FlightGlobals.ActiveVessel != null && HighLogic.CurrentGame.Parameters.Flight.CanLeaveToTrackingStation) {
							return FlightGlobals.ActiveVessel.IsClearToSave() == ClearToSaveStatus.CLEAR;
						}
					}
				}
				return false;
			}
		}

		public bool CanSpaceCenter {
			get {
				if (HighLogic.LoadedSceneIsGame) {
					if (HighLogic.LoadedScene != GameScenes.SPACECENTER) {
						if (!HighLogic.LoadedSceneIsFlight) {
							return true;
						}
						if (FlightGlobals.ready && FlightGlobals.ActiveVessel != null && HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter) {
							return FlightGlobals.ActiveVessel.IsClearToSave() == ClearToSaveStatus.CLEAR;
						}
					}
				}
				return false;
			}
		}

		public bool CanRecover {
			get {
				if (HighLogic.LoadedSceneIsFlight) {
					if (FlightGlobals.ready && FlightGlobals.ActiveVessel != null) {
						if (FlightGlobals.ActiveVessel.IsClearToSave() == ClearToSaveStatus.CLEAR && FlightGlobals.ActiveVessel.IsRecoverable) {
							return true;
						}
					}
				}
				return false;
			}
		}

		public bool CanRevert {
			get {
				if (HighLogic.LoadedSceneIsFlight) {
					if (FlightGlobals.ready && HighLogic.CurrentGame.Parameters.Flight.CanRestart && FlightDriver.CanRevertToPostInit && FlightDriver.PostInitState != null) {
						return true;
					}
				}
				return false;
			}
		}

		public bool CanRevertToEditor {
			get {
				if (HighLogic.LoadedSceneIsFlight) {
					if (FlightGlobals.ready && HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor && FlightDriver.CanRevertToPrelaunch && FlightDriver.PreLaunchState != null && ShipConstruction.ShipType != EditorFacility.None) {
						return true;
					}
				}
				return false;
			}
		}

		public bool CanRevertToSpaceCenter {
			get {
				if (HighLogic.LoadedSceneIsFlight) {
					if (FlightGlobals.ready && HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter && FlightDriver.CanRevertToPrelaunch && FlightDriver.PreLaunchState != null && ShipConstruction.ShipType != EditorFacility.None) {
						return true;
					}
				}
				return false;
			}
		}

		public bool CanEditor(EditorFacility editorFacility) {
			if (HighLogic.LoadedSceneIsGame) {
				if (HighLogic.LoadedSceneIsEditor) {
					if (EditorDriver.fetch != null && EditorLogic.fetch != null) {
						if (EditorDriver.editorFacility != editorFacility) {
							return true;
						}
					}
					return false;
				}
				if (!HighLogic.LoadedSceneIsFlight) {
					return true;
				} 
				if (FlightGlobals.ready && FlightGlobals.ActiveVessel != null && HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor) {
					return FlightGlobals.ActiveVessel.IsClearToSave() == ClearToSaveStatus.CLEAR;
				}
			}
			return false;
		}

		public bool CanLastVessel {
			get {
				if (HighLogic.LoadedSceneIsGame) {
					QData _lastVessel = LastVesselLastIndex ();
					if (_lastVessel != null) {
						ProtoVessel _pVessel = _lastVessel.protoVessel;
						if (_pVessel != null) {
							if (pVesselExists (_pVessel)) {
								if (!HighLogic.LoadedSceneIsFlight) {
									return true;
								} else {
									if (FlightGlobals.ready && FlightGlobals.ActiveVessel != null) {
										Guid _guid = _pVessel.vesselID;
										if (_guid != Guid.Empty) {
											Vessel _vessel = FlightGlobals.Vessels.FindLast (v => v.id == _guid);
											if (_vessel != null) {
												if (!_vessel.isActiveVessel && (!_vessel.loaded && HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsFar) || (_vessel.loaded && HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsNear)) {
													return FlightGlobals.ActiveVessel.IsClearToSave () == ClearToSaveStatus.CLEAR;
												}
											}
										}
									}
								}
							}
						}
					}
				}
				return false;
			}
		}

		public bool CanFundBuilding {
			get {
				return HighLogic.CurrentGame.Mode == Game.Modes.CAREER;
			}
		}

		public bool CanScienceBuilding {
			get {
				return HighLogic.CurrentGame.Mode == Game.Modes.CAREER || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX;
			}
		}

		public bool isMissionControl {
			get {
				return MissionControl.Instance != null;
			}
		}

		public bool isAdministration {
			get {
				return Administration.Instance != null;
			}
		}

		public bool isLaunchScreen {
			get {
				return VesselSpawnDialog.Instance != null;
			}
		}

		public bool isAstronautComplex {
			get;
			internal set;
		}

		public bool isRnD {
			get;
			internal set;
		}

		public bool isBat {
			get {
				return isMissionControl || isAdministration || isAstronautComplex || isRnD;
			}
		}

		public bool pVesselExists(ProtoVessel pvessel) {
			FlightState _flightState = HighLogic.CurrentGame.flightState;
			if (_flightState != null) {
				return _flightState.protoVessels.Exists (pv => pv.vesselID == pvessel.vesselID);
			}
			return false;
		}

		QData LastVesselLastIndex() {
			int _index;
			return LastVesselLastIndex(out _index);
		}

		QData LastVesselLastIndex(out int index) {
			index = -1;
			QData _lastVessel = null;
			while (LastVessels.Count > 0) {
				index = LastVessels.Count - 1;
				_lastVessel = LastVessels [index];
				if (_lastVessel != null) {
					ProtoVessel _pVessel = _lastVessel.protoVessel;
					if (_pVessel != null) {
						if (pVesselExists (_pVessel)) {
							if (_lastVessel.idx != -1) {
								break;
							}
						}					
					}
				}
				LastVessels.RemoveAt (index);
				Warning ("Remove a vessel from the last Vessels", "QGoTo");
			}
			return _lastVessel;
		}

		IEnumerator loadScene(GameScenes scenes, EditorFacility facility = EditorFacility.None) {
			yield return new WaitForEndOfFrame ();
			if (scenes != GameScenes.EDITOR) {
				HighLogic.LoadScene (scenes);
			}
			else if (facility != EditorFacility.None) {
				EditorFacility editorFacility = EditorFacility.None;
				if (ShipConstruction.ShipConfig != null) {
					editorFacility = ShipConstruction.ShipType;
				}
				EditorDriver.StartupBehaviour = (editorFacility == facility ? EditorDriver.StartupBehaviours.LOAD_FROM_CACHE : EditorDriver.StartupBehaviours.START_CLEAN);
				EditorDriver.StartEditor (facility);
			}
			InputLockManager.ClearControlLocks ();
		}

		IEnumerator PostInit() {
			while (ApplicationLauncher.Instance == null) {
				yield return 0;
			}
			if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER) { 
				while (Funding.Instance == null) {
					yield return 0;
				}
				while (Reputation.Instance == null) {
					yield return 0;
				}
				while (Contracts.ContractSystem.Instance == null) {
					yield return 0;
				}
				while (Contracts.Agents.AgentList.Instance == null) {
					yield return 0;
				}
				while (FinePrint.ContractDefs.Instance == null) {
					yield return 0;
				}
				while (Strategies.StrategySystem.Instance == null) {
					yield return 0;
				}
				while (ScenarioUpgradeableFacilities.Instance == null) {
					yield return 0;
				}
				while (ContractsApp.Instance == null) {
					yield return 0;
				}
				while (CurrencyWidget.FindObjectOfType (typeof(CurrencyWidget)) == null) {
					yield return 0;
				}
			}
			if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX) {
				while (ResearchAndDevelopment.Instance == null) {
					yield return 0;
				}
			}
			PostInitSC ();
		}

		void PostInitSC() {
			if (LastVessels.Count < 1) {
				FlightState _flightState = HighLogic.CurrentGame.flightState;
				if (_flightState != null) {
					List<ProtoVessel> _pVessels = _flightState.protoVessels;
					for (int _i = _pVessels.Count - 1; _i >= 0; --_i) {
						ProtoVessel _pVessel = _pVessels[_i];
						if (_pVessel != null) {
							AddLastVessel (_pVessel);
						}
					}
				}
			} else {
				List<QData> _lastVessels = LastVessels;
				LastVessels = new List<QData> ();
				for (int _i = _lastVessels.Count - 1; _i >= 0; --_i) {
					QData _lastVessel = _lastVessels[_i];
					if (pVesselExists (_lastVessel.protoVessel)) {
						LastVessels.Add (_lastVessel);
					} else {
						Warning ("Remove from the last Vessels: " + _lastVessel.protoVessel.vesselName, "QGoTo");
					}
				}
			}
			if (SavedGoTo != GoTo.None) {
				switch (SavedGoTo) {
				case GoTo.Administration:
					administration ();
					break;
				case GoTo.AstronautComplex:
					astronautComplex ();
					break;
				case GoTo.RnD:
					RnD ();
					break;
				case GoTo.MissionControl:
					missionControl ();
					break;
				}
				SavedGoTo = GoTo.None;
			}
		}

		internal void AddLastVessel(ProtoVessel pVessel) {
			QData _lastVessel = LastVesselLastIndex ();
			if (_lastVessel!= null) {
				if (_lastVessel.protoVessel.vesselID == pVessel.vesselID) {
					Log ("Kept the last Vessel: " + pVessel.vesselName, "QGoTo");
					return;
				}
			}
			if (pVessel.vesselType == VesselType.Unknown || pVessel.vesselType == VesselType.SpaceObject || pVessel.vesselType == VesselType.Debris) {
				Warning (string.Format ("Can't save the last Vessel: ({0}) {1}", pVessel.vesselType.ToString (), pVessel.vesselName), "QGoTo");
				return;
			}
			LastVessels.Add (new QData (pVessel));
			Log (string.Format("Saved the last Vessel: ({0}){1}", LastVessels.Count, pVessel.vesselName), "QGoTo");
			if (LastVessels.Count > 10) {
				Log ("Delete the first vessel from last Vessel: " + LastVessels[0].protoVessel.vesselName, "QGoTo");
				LastVessels.RemoveAt (0);
			}
			Log ("Last Vessel has keep " + LastVessels.Count + " vessels", "QGoTo");
		}

		void screenMSG(GoTo scene) {
			Warning ("You can't " + GetText (scene), "QGoTo");
			ScreenMessages.PostScreenMessage (QLang.translate ("You can't") + " " + GetText (scene), 10, ScreenMessageStyle.UPPER_RIGHT);			
		}

		public void mainMenu() {
			SavedGoTo = GoTo.None;
			if (CanMainMenu) {
				ClearSpaceCenter ();
				GamePersistence.SaveGame (SaveGame, HighLogic.SaveFolder, SaveMode.OVERWRITE);
				Log (GetText (GoTo.MainMenu));
				StartCoroutine (loadScene (GameScenes.MAINMENU));
				return;
			}
			screenMSG (GoTo.MainMenu);
		}

		public void settings() {
			SavedGoTo = GoTo.None;
			if (CanMainMenu) {
				ClearSpaceCenter ();
				GamePersistence.SaveGame (SaveGame, HighLogic.SaveFolder, SaveMode.OVERWRITE);
				Log (GetText (GoTo.Settings));
				StartCoroutine (loadScene (GameScenes.SETTINGS));
				return;
			}
			screenMSG (GoTo.Settings);
		}

		public void trackingStation() {
			SavedGoTo = GoTo.None;
			if (CanTrackingStation) {
				ClearSpaceCenter ();
				GamePersistence.SaveGame (SaveGame, HighLogic.SaveFolder, SaveMode.OVERWRITE);
				//HighLogic.LoadScene	(GameScenes.LOADINGBUFFER);
				InputLockManager.ClearControlLocks ();
				Log (GetText (GoTo.TrackingStation));
				StartCoroutine (loadScene (GameScenes.TRACKSTATION));
				return;
			}
			screenMSG (GoTo.TrackingStation);
		}

		void gotoSpaceCenter(GameBackup gameBackup = null) {
			if (gameBackup == null) {
				GamePersistence.SaveGame (SaveGame, HighLogic.SaveFolder, SaveMode.OVERWRITE);
			}
			else {
				GamePersistence.SaveGame (gameBackup, SaveGame, HighLogic.SaveFolder, SaveMode.OVERWRITE);
			}
			StartCoroutine (loadScene(GameScenes.SPACECENTER));
		}

		public void spaceCenter() {
			SavedGoTo = GoTo.None;
			if (CanSpaceCenter) {
				gotoSpaceCenter ();
				Log (GetText (GoTo.SpaceCenter));
				return;
			}
			screenMSG (GoTo.SpaceCenter);
		}

		void ClearSpaceCenter() {
			if (HighLogic.LoadedScene != GameScenes.SPACECENTER) {
				return;
			}
			if (isLaunchScreen) {
				GameEvents.onGUILaunchScreenDespawn.Fire ();
				Log ("Clear LaunchScreen", "QGoTo");
			}
			if (isMissionControl) {
				GameEvents.onGUIMissionControlDespawn.Fire ();
				Log ("Clear MissionControl", "QGoTo");
			}
			if (isAdministration) {
				GameEvents.onGUIAdministrationFacilityDespawn.Fire ();
				Log ("Clear Administration", "QGoTo");
			}
			if (isAstronautComplex) {
				GameEvents.onGUIAstronautComplexDespawn.Fire ();
				Log ("Clear AstronautComplex", "QGoTo");
			}
			if (isRnD) {
				GameEvents.onGUIRnDComplexDespawn.Fire ();
				Log ("Clear Research And Development", "QGoTo");
			}
			InputLockManager.ClearControlLocks ();
			Log ("Clear SpaceCenter", "QGoTo");
		}

		public void missionControl() {
			SavedGoTo = GoTo.None;
			if (HighLogic.LoadedSceneIsGame) {
				if (CanFundBuilding) {
					if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
						ClearSpaceCenter ();
						GameEvents.onGUIMissionControlSpawn.Fire ();
						InputLockManager.ClearControlLocks ();
						Log (GetText(GoTo.MissionControl));
						return;
					}
					if (CanSpaceCenter) {
						SavedGoTo = GoTo.MissionControl;
						gotoSpaceCenter ();
						return;
					}
				}
			}
			screenMSG (GoTo.MissionControl);
		}

		public void administration() {
			SavedGoTo = GoTo.None;
			if (HighLogic.LoadedSceneIsGame) {
				if (CanFundBuilding) {
					if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
						ClearSpaceCenter ();
						GameEvents.onGUIAdministrationFacilitySpawn.Fire ();
						InputLockManager.ClearControlLocks ();
						Log (GetText(GoTo.Administration));
						return;
					}
					if (CanSpaceCenter) {
						SavedGoTo = GoTo.Administration;
						gotoSpaceCenter ();
						return;
					}
				}
			}
			screenMSG (GoTo.Administration);
		}

		public void RnD() {
			SavedGoTo = GoTo.None;
			if (HighLogic.LoadedSceneIsGame) {
				if (CanScienceBuilding) {
					if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
						ClearSpaceCenter ();
						GameEvents.onGUIRnDComplexSpawn.Fire ();
						InputLockManager.ClearControlLocks ();
						Log (GetText(GoTo.RnD));
						return;
					}
					if (CanSpaceCenter) {
						SavedGoTo = GoTo.RnD;
						gotoSpaceCenter ();
						return;
					}
				}
			}
			screenMSG (GoTo.RnD);
		}

		public void astronautComplex() {
			SavedGoTo = GoTo.None;
			if (HighLogic.LoadedSceneIsGame) {
				if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
					ClearSpaceCenter ();
					GameEvents.onGUIAstronautComplexSpawn.Fire ();
					InputLockManager.ClearControlLocks ();
					Log (GetText(GoTo.AstronautComplex));
					return;
				}
				if (CanSpaceCenter) {
					SavedGoTo = GoTo.AstronautComplex;
					gotoSpaceCenter ();
					return;
				}
			}
			screenMSG (GoTo.AstronautComplex);
		}

		void gotoVAB() {
			ClearSpaceCenter ();
			GamePersistence.SaveGame (SaveGame, HighLogic.SaveFolder, SaveMode.OVERWRITE);
			StartCoroutine (loadScene (GameScenes.EDITOR, EditorFacility.VAB));
		}

		public void VAB() {
			SavedGoTo = GoTo.None;
			if (CanEditor(EditorFacility.VAB)) {
				gotoVAB ();
				Log (GetText (GoTo.VAB));
				return;
			}
			screenMSG (GoTo.VAB);
		}

		void gotoSPH() {
			ClearSpaceCenter ();
			GamePersistence.SaveGame (SaveGame, HighLogic.SaveFolder, SaveMode.OVERWRITE);
			StartCoroutine (loadScene (GameScenes.EDITOR, EditorFacility.SPH));
		}

		public void SPH() {
			SavedGoTo = GoTo.None;
			if (CanEditor (EditorFacility.SPH)) {
				gotoSPH ();
				Log (GetText (GoTo.SPH));
				return;
			}
			screenMSG (GoTo.SPH);
		}

		public void Recover() {
			SavedGoTo = GoTo.None;
			if (CanRecover) {
				GameEvents.OnVesselRecoveryRequested.Fire (FlightGlobals.ActiveVessel);
				InputLockManager.ClearControlLocks ();
				Log (GetText (GoTo.Recover));
				return;
			}
			screenMSG (GoTo.Recover);
		}

		public void Revert() {
			SavedGoTo = GoTo.None;
			if (CanRevert) {
				FlightDriver.RevertToLaunch ();
				InputLockManager.ClearControlLocks ();
				Log (GetText (GoTo.Revert));
				return;
			}
			screenMSG (GoTo.Revert);
		}

		public void RevertToEditor() {
			SavedGoTo = GoTo.None;
			if (CanRevertToEditor) {
				FlightDriver.RevertToPrelaunch (ShipConstruction.ShipType);
				InputLockManager.ClearControlLocks ();
				Log (GetText (GoTo.RevertToEditor));
				return;
			}
			screenMSG (GoTo.RevertToEditor);
		}

		public void RevertToSpaceCenter() {
			SavedGoTo = GoTo.None;
			if (CanRevertToSpaceCenter) {
				gotoSpaceCenter (FlightDriver.PreLaunchState);
				Log (GetText (GoTo.RevertToSpaceCenter));
				return;
			}
			screenMSG (GoTo.RevertToSpaceCenter);
		}

		public void LastVessel() {
			SavedGoTo = GoTo.None;
			if (CanLastVessel) {
				int _index = -1;
				QData _lastVessel = LastVesselLastIndex (out _index);
				if (_lastVessel != null) {
					int _idx = _lastVessel.idx;
					if (_idx != -1) {
						ClearSpaceCenter ();
						string _saveGame = GamePersistence.SaveGame (SaveGame, HighLogic.SaveFolder, SaveMode.OVERWRITE);
						Log (GetText (GoTo.LastVessel));
						FlightDriver.StartAndFocusVessel (_saveGame, _idx);
						InputLockManager.ClearControlLocks ();
						LastVessels.RemoveAt (_index);
						Warning ("Remove from the last Vessels: " + _lastVessel.protoVessel.vesselName, "QGoTo");
						return;
					}
				}
			}
			screenMSG (GoTo.LastVessel);
		}
	}
}