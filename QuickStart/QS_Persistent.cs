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
using System.Collections;
using UnityEngine;

namespace QuickStart {
	public partial class QuickStart_Persistent {

		public static bool Ready = false;

		public static QuickStart_Persistent Instance {
			get;
			private set;
		}

		[KSPField(isPersistant = true)]	public static string vesselID = string.Empty;

		public static Guid VesselID {
			get {
				return new Guid (vesselID);
			}
		}

		public static readonly string shipFilename = "Auto-Saved Ship";

		public static string shipPath {
			get {
				return string.Format ("{0}/Ships/{1}/{2}.craft", QSaveGame.saveDir, ShipConstruction.GetShipsSubfolderFor ((EditorFacility)QSettings.Instance.editorFacility), shipFilename);
			}
		}

		public override void OnAwake() {
			Instance = this;
			GameEvents.onFlightReady.Add (OnFlightReady);
			GameEvents.onVesselChange.Add (OnVesselChange);
			GameEvents.onGameSceneSwitchRequested.Add (OnSceneSwitch);
			if (QSpaceCenter.Instance == null) {
				QSettings.Instance.gameScene = (int)HighLogic.LoadedScene;
				if (HighLogic.LoadedSceneIsEditor) {
					QSettings.Instance.editorFacility = (int)EditorDriver.editorFacility;
				}
			}
			QSettings.Instance.Save ();
			QuickStart.Log ("OnAwake", "QPersistent");
		}

		void Start() {
			if (HighLogic.LoadedSceneIsEditor && QSettings.Instance.enableEditorAutoSaveShip) {
				StartCoroutine (autoSaveShip ());
			}
			Ready = true;
			QuickStart.Log ("Start", "QPersistent");
		}

		IEnumerator autoSaveShip() {
			QuickStart.Log ("autoSaveShip: start", "QPersistent");
			while (HighLogic.LoadedSceneIsEditor && QSettings.Instance.enableEditorAutoSaveShip) {
				yield return new WaitForSeconds (QSettings.Instance.editorTimeToSave);
				ShipConstruction.SaveShip(shipFilename);
				QuickStart.Log ("autoSaveShip: save", "QPersistent");
			}
			QuickStart.Log ("autoSaveShip: end", "QPersistent");
		}

		void OnDestroy() {
			GameEvents.onFlightReady.Remove (OnFlightReady);
			GameEvents.onVesselChange.Remove (OnVesselChange);
			GameEvents.onGameSceneSwitchRequested.Remove (OnSceneSwitch);
			QuickStart.Log ("OnDestroy", "QPersistent");
		}

		void OnSceneSwitch(GameEvents.FromToAction<GameScenes,GameScenes> gameScenes) {
			if (gameScenes.to == GameScenes.MAINMENU) {
				vesselID = string.Empty;
			}
		}

		public override void OnLoad(ConfigNode node) {
			try {
				if (node != null) {
					if (vesselID == string.Empty) {
						if (node.HasValue ("vesselID")) {
							vesselID = node.GetValue ("vesselID");
							QuickStart.Log ("OnLoad " + vesselID, "QPersistent");
						}
					}
				}
			} catch (Exception e) {
				QuickStart.Warning ("Can't load: {0} " + e, "QPersistent");
			}
		}

		public override void OnSave(ConfigNode node) {
			try {
				if (!string.IsNullOrEmpty (vesselID)) {
					node.AddValue ("vesselID", vesselID);
				}
				QuickStart.Log ("OnSave " + vesselID, "QPersistent");
			} catch (Exception e) {
				QuickStart.Warning ("Can't save: {0} " + e, "QPersistent");
			}
		}

		void OnFlightReady() {
			vesselID = FlightGlobals.ActiveVessel.id.ToString();
			QuickStart.Log ("OnFlightReady " + vesselID, "QPersistent");
		}

		void OnVesselChange(Vessel vessel) {
			vesselID = vessel.id.ToString();
			QuickStart.Log ("OnVesselChange " + vesselID, "QPersistent");
		}
	}
}

