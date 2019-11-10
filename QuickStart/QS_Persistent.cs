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
using KSP.Localization;
using QuickStart.QUtils;
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

		public static String StopWatchText {
			get {
				if (QSettings.Instance.enableStopWatch) {
					float RunningTime = Time.realtimeSinceStartup;
					int min = (int)(RunningTime / 60f);
					int sec = (int)(RunningTime) % 60;

					return Localizer.Format("quickstart_stopWatch", min.ToString("00"), sec.ToString("00")) + " ";
				}
				else {
					return "";
				}
			}
		}

		public static void SkippingScreen(GameScenes scene, string scene_name) {
			if (HighLogic.LoadedScene != scene || QLoading.Ended) return;

			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height), QStyle.Label);
			GUILayout.Label(QuickStart.MOD + Environment.NewLine
				+ StopWatchText + Localizer.Format("quickstart_skipping", scene_name) + Environment.NewLine
				+ Localizer.Format("quickstart_abort", QSettings.Instance.KeyEscape), QStyle.Label);
			GUILayout.EndArea();
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
			QDebug.Log ("OnAwake", "QPersistent");
		}

		void Start() {
			if (HighLogic.LoadedSceneIsEditor && QSettings.Instance.enableEditorAutoSaveShip) {
				StartCoroutine (autoSaveShip ());
			}
			Ready = true;
			QDebug.Log ("Start", "QPersistent");
		}

		IEnumerator autoSaveShip() {
			QDebug.Log ("autoSaveShip: start", "QPersistent");
			while (HighLogic.LoadedSceneIsEditor && QSettings.Instance.enableEditorAutoSaveShip) {
				yield return new WaitForSeconds (QSettings.Instance.editorTimeToSave);
				ShipConstruction.SaveShip(shipFilename);
				QDebug.Log ("autoSaveShip: save", "QPersistent");
			}
			QDebug.Log ("autoSaveShip: end", "QPersistent");
		}

		void OnDestroy() {
			GameEvents.onFlightReady.Remove (OnFlightReady);
			GameEvents.onVesselChange.Remove (OnVesselChange);
			GameEvents.onGameSceneSwitchRequested.Remove (OnSceneSwitch);
			QDebug.Log ("OnDestroy", "QPersistent");
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
							QDebug.Log ("OnLoad " + vesselID, "QPersistent");
						}
					}
				}
			} catch (Exception e) {
				QDebug.Warning ("Can't load: {0} " + e, "QPersistent");
			}
		}

		public override void OnSave(ConfigNode node) {
			try {
				if (!string.IsNullOrEmpty (vesselID)) {
					node.AddValue ("vesselID", vesselID);
				}
				QDebug.Log ("OnSave " + vesselID, "QPersistent");
			} catch (Exception e) {
				QDebug.Warning ("Can't save: {0} " + e, "QPersistent");
			}
		}

		void OnFlightReady() {
			vesselID = FlightGlobals.ActiveVessel.id.ToString();
			QDebug.Log ("OnFlightReady " + vesselID, "QPersistent");
		}

		void OnVesselChange(Vessel vessel) {
			vesselID = vessel.id.ToString();
			QDebug.Log ("OnVesselChange " + vesselID, "QPersistent");
		}
	}
}

