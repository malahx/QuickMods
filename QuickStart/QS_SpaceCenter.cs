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

using System.Collections;
using System.IO;
using UnityEngine;
using QuickStart.QUtils;

namespace QuickStart {
	public partial class QSpaceCenter {

		public static QSpaceCenter Instance {
			get;
			private set;
		}

        public bool Ready = false;
        Coroutine start;

		void Awake() {
			if (QLoading.Ended) {
				QDebug.Warning ("Reload? Destroy.", "QSpaceCenter");
				Destroy (this);
				return;
			}
			if (Instance != null) {
				QDebug.Warning ("There's already an Instance", "QSpaceCenter");
				Destroy (this);
				return;
			}
			Instance = this;
			QDebug.Log ("Awake", "QSpaceCenter");
		}

		void Start() {
			InputLockManager.RemoveControlLock("applicationFocus");
			if (!QSettings.Instance.Enabled || QSettings.Instance.gameScene == (int)GameScenes.SPACECENTER) {
				QDebug.Log ("Not need to keep it loaded.", "QSpaceCenter");
				QLoading.Ended = true;
				Destroy (this);
				return;
			}
			start = StartCoroutine (QStart ());
			QDebug.Log ("Start", "QSpaceCenter");
		}

		IEnumerator QStart() {
			while (!Ready || !QuickStart_Persistent.Ready) {
				yield return 0;
			}
			yield return new WaitForSecondsRealtime (QSettings.Instance.WaitLoading);
			yield return new WaitForEndOfFrame ();
			QDebug.Log ("SpaceCenter Loaded", "QSpaceCenter");
			if (QSettings.Instance.gameScene == (int)GameScenes.FLIGHT) {
				string _saveGame = GamePersistence.SaveGame (QSaveGame.FILE, HighLogic.SaveFolder, SaveMode.OVERWRITE);
				if (!string.IsNullOrEmpty (QuickStart_Persistent.vesselID)) {
					int _idx = HighLogic.CurrentGame.flightState.protoVessels.FindLastIndex (pv => pv.vesselID == QuickStart_Persistent.VesselID);
					if (_idx != -1) {
						QDebug.Log (string.Format("StartAndFocusVessel: {0}({1})[{2}] idx: {3}", QSaveGame.vesselName, QSaveGame.vesselType, QuickStart_Persistent.vesselID, _idx), "QSpaceCenter");
						FlightDriver.StartAndFocusVessel (_saveGame, _idx);
					} else {
						QDebug.Warning ("QStart: invalid idx", "QSpaceCenter");
						DestroyThis ();
					}
				} else {
					QDebug.Warning ("QStart: No vessel found", "QSpaceCenter");
					DestroyThis ();
				}
			}
			if (QSettings.Instance.gameScene == (int)GameScenes.TRACKSTATION) {
				HighLogic.LoadScene	(GameScenes.LOADINGBUFFER);
				HighLogic.LoadScene (GameScenes.TRACKSTATION);
				InputLockManager.ClearControlLocks ();
				QDebug.Log ("Goto Tracking Station", "QSpaceCenter");
				DestroyThis ();
			}
			if (QSettings.Instance.gameScene == (int)GameScenes.EDITOR) {
				if (QSettings.Instance.enableEditorLoadAutoSave && File.Exists (QuickStart_Persistent.shipPath)) {
					EditorDriver.StartAndLoadVessel(QuickStart_Persistent.shipPath, (EditorFacility)QSettings.Instance.editorFacility);
					QDebug.Log ("StartAndLoadVessel: " + QuickStart_Persistent.shipPath, "QSpaceCenter");
				} else {
					EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.START_CLEAN;
					EditorDriver.StartEditor((EditorFacility)QSettings.Instance.editorFacility);
					QDebug.Log ("StartEditor", "QSpaceCenter");
				}
				InputLockManager.ClearControlLocks ();
				QDebug.Log ("Goto " + (QSettings.Instance.editorFacility == (int)EditorFacility.VAB ? "Vehicle Assembly Building" : "Space Plane Hangar"), "QSpaceCenter");
				DestroyThis ();
			}
			Destroy (this);
			yield break;
		}

		void LateUpdate() {
			if (!Ready) {
				QDebug.Log ("Ready", "QSpaceCenter");
				Ready = true;
			}
		}

        void Update() {
            if (QKey.isKeyDown(QKey.Key.Escape)) {
                if (start != null) {
                    StopCoroutine(start);
                    QDebug.Log ("Escape", "QSpaceCenter");
                    DestroyThis();
                }
            }
        }

		void OnDestroy() {
			QDebug.Log ("OnDestroy", "QSpaceCenter");
		}

		void DestroyThis() {
			QDebug.Log ("DestroyThis", "QSpaceCenter");
			QLoading.Ended = true;
			Destroy (this);
		}

		void OnGUI() {
			if (HighLogic.LoadedScene != GameScenes.SPACECENTER || QLoading.Ended) {
				return;
			}
			GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height), QStyle.Label);
            GUILayout.Label (QuickStart.MOD + "...\n" + string.Format(QLang.translate("Push on {0} to abort the operation"), QSettings.Instance.KeyEscape), QStyle.Label);
			GUILayout.EndArea ();
		}
	}
}