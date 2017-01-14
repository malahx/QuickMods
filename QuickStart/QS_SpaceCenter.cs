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
using Contracts;
using Strategies;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace QuickStart {
	public partial class QSpaceCenter {

		public bool Ready = false;

		public static QSpaceCenter Instance {
			get;
			private set;
		}

		void Awake() {
			if (QLoading.Ended) {
				QuickStart.Warning ("Reload? Destroy.", "QSpaceCenter");
				Destroy (this);
				return;
			}
			if (Instance != null) {
				QuickStart.Warning ("There's already an Instance", "QSpaceCenter");
				Destroy (this);
				return;
			}
			Instance = this;
			QuickStart.Log ("Awake", "QSpaceCenter");
		}

		void Start() {
			InputLockManager.RemoveControlLock("applicationFocus");
			if (!QSettings.Instance.Enabled || QSettings.Instance.gameScene == (int)GameScenes.SPACECENTER) {
				QuickStart.Log ("Not need to keep it loaded.", "QSpaceCenter");
				QLoading.Ended = true;
				Destroy (this);
				return;
			}
			StartCoroutine (QStart ());
			QuickStart.Log ("Start", "QSpaceCenter");
		}

		bool contractsAreInited {
			get {
				return ContractSystem.Instance != null && 
					                 ContractSystem.MandatoryTypes != null &&
					                 ContractSystem.ContractTypes != null &&
					                 ContractSystem.PredicateTypes != null &&
				        			 ContractSystem.ParameterTypes != null;
			}
		}

		bool strategyAreInited {
			get {
				return StrategySystem.Instance != null && 
					                 StrategySystem.StrategyEffectTypes != null && 
					                 StrategySystem.StrategyTypes != null;
			}
		}

		IEnumerator QStart() {
			while (!Ready || !QuickStart_Persistent.Ready) {
				yield return 0;
			}
			if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER) {
				var date = DateTime.Now;
				while ((!contractsAreInited || !strategyAreInited) && DateTime.Now.Second - date.Second < 5) {
					yield return 0;
				}
			}
			yield return new WaitForSecondsRealtime (QSettings.Instance.WaitLoading);
			yield return new WaitForEndOfFrame ();
			QuickStart.Log ("SpaceCenter Loaded", "QSpaceCenter");
			if (QSettings.Instance.gameScene == (int)GameScenes.FLIGHT) {
				string _saveGame = GamePersistence.SaveGame (HighLogic.CurrentGame, QSaveGame.File, HighLogic.SaveFolder, SaveMode.OVERWRITE);
				if (!string.IsNullOrEmpty (QuickStart_Persistent.vesselID)) {
					int _idx = HighLogic.CurrentGame.flightState.protoVessels.FindLastIndex (pv => pv.vesselID == QuickStart_Persistent.VesselID);
					if (_idx != -1) {
						QuickStart.Log (string.Format("StartAndFocusVessel: {0}({1})[{2}] idx: {3}", QSaveGame.vesselName, QSaveGame.vesselType, QuickStart_Persistent.vesselID, _idx), "QSpaceCenter");
						FlightDriver.StartAndFocusVessel (_saveGame, _idx);
					} else {
						QuickStart.Warning ("QStart: invalid idx", "QSpaceCenter");
						DestroyThis ();
					}
				} else {
					QuickStart.Warning ("QStart: No vessel found", "QSpaceCenter");
					DestroyThis ();
				}
			}
			if (QSettings.Instance.gameScene == (int)GameScenes.TRACKSTATION) {
				HighLogic.LoadScene	(GameScenes.LOADINGBUFFER);
				HighLogic.LoadScene (GameScenes.TRACKSTATION);
				InputLockManager.ClearControlLocks ();
				QuickStart.Log ("Goto Tracking Station", "QSpaceCenter");
				DestroyThis ();
			}
			if (QSettings.Instance.gameScene == (int)GameScenes.EDITOR) {
				if (QSettings.Instance.enableEditorLoadAutoSave && File.Exists (QuickStart_Persistent.shipPath)) {
					EditorDriver.StartAndLoadVessel(QuickStart_Persistent.shipPath, (EditorFacility)QSettings.Instance.editorFacility);
					QuickStart.Log ("StartAndLoadVessel: " + QuickStart_Persistent.shipPath, "QSpaceCenter");
				} else {
					EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.START_CLEAN;
					EditorDriver.StartEditor((EditorFacility)QSettings.Instance.editorFacility);
					QuickStart.Log ("StartEditor", "QSpaceCenter");
				}
				InputLockManager.ClearControlLocks ();
				QuickStart.Log ("Goto " + (QSettings.Instance.editorFacility == (int)EditorFacility.VAB ? "Vehicle Assembly Building" : "Space Plane Hangar"), "QSpaceCenter");
				DestroyThis ();
			}
			Destroy (this);
			yield break;
		}

		void LateUpdate() {
			if (!Ready) {
				QuickStart.Log ("Ready", "QSpaceCenter");
				Ready = true;
			}
		}

		void OnDestroy() {
			QuickStart.Log ("OnDestroy", "QSpaceCenter");
		}

		void DestroyThis() {
			QuickStart.Log ("DestroyThis", "QSpaceCenter");
			QLoading.Ended = true;
			Destroy (this);
		}

		void OnGUI() {
			if (HighLogic.LoadedScene != GameScenes.SPACECENTER || QLoading.Ended) {
				return;
			}
			GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height), QLoading.labelStyle);
			GUILayout.Label (QuickStart.MOD + "...", QLoading.labelStyle);
			GUILayout.EndArea ();
		}
	}
}