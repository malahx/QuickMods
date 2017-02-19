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
using UnityEngine;
using QuickStart.QUtils;

namespace QuickStart {

	public partial class QMainMenu {

		public static QMainMenu Instance {
			get;
			private set;
		}

        public bool Ready = false;
        Coroutine start;

		void Awake() {
			if (QLoading.Ended) {
				QuickStart.Warning ("Reload? Destroy.", "QMainMenu");
				Destroy (this);
				return;
			}
			if (Instance != null) {
				QuickStart.Warning ("There's already an Instance", "QMainMenu");
				Destroy (this);
				return;
			}
			Instance = this;
			QuickStart.Log ("Awake", "QMainMenu");
		}

		void Start() {
			if (!QSettings.Instance.Enabled) {
				QuickStart.Log ("No need to keep it loaded.", "QMainMenu");
				DestroyThis ();
				return;
			}
			start = StartCoroutine (QStart ());
			QuickStart.Log ("Start", "QMainMenu");
		}

		IEnumerator QStart() {
			if (string.IsNullOrEmpty (QSaveGame.LastUsed)) {
				QuickStart.Warning ("Last savegame not found!", "QMainMenu");
				DestroyThis ();
				yield break;
			}
			if (!QSettings.Instance.Enabled) {
				QuickStart.Log ("QuickStart is disabled!", "QMainMenu");
				DestroyThis ();
				yield break;
			}
			while (!Ready) {
				yield return 0;
			}
			yield return new WaitForEndOfFrame ();
			yield return new WaitForSeconds (QSettings.Instance.WaitLoading);
			yield return new WaitForEndOfFrame ();
			QuickStart.Log ("MainMenu Loaded", "QMainMenu");
			QuickStart.Warning ("The last game found: " + QSaveGame.LastUsed, "QMainMenu");
			HighLogic.CurrentGame = GamePersistence.LoadGame (QSaveGame.File, QSaveGame.LastUsed, true, false);
			if (HighLogic.CurrentGame != null) {
				HighLogic.SaveFolder = QSaveGame.LastUsed;
				if (GamePersistence.UpdateScenarioModules (HighLogic.CurrentGame)) {
					GamePersistence.SaveGame (HighLogic.CurrentGame, QSaveGame.File, HighLogic.SaveFolder, SaveMode.OVERWRITE);
				}
				QuickStart.Log ("Goto SpaceCenter", "QMainMenu");
				HighLogic.CurrentGame.startScene = GameScenes.SPACECENTER;
				HighLogic.CurrentGame.Start ();
				InputLockManager.ClearControlLocks ();
				Destroy (this);
				yield break;
			}
			QuickStart.Warning ("Can't load the last save game", "QMainMenu");
			DestroyThis ();
			yield break;
		}

		void LateUpdate() {
			if (!Ready) {
				QuickStart.Log ("Ready", "QMainMenu");
				Ready = true;
			}
		}

        void Update() {
            if (QKey.isKeyDown(QKey.Key.Escape)) {
                if (start != null) {
                    StopCoroutine(start);
                    QuickStart.Log("Escape", "QSpaceCenter");
                    DestroyThis();
                }
            }
        }

		void DestroyThis() {
			QuickStart.Log ("DestroyThis", "QMainMenu");
			QuickStart_Persistent.vesselID = string.Empty;
			QLoading.Ended = true;
			Destroy (this);
		}

		void OnDestroy() {
			QuickStart.Log ("OnDestroy", "QMainMenu");
		}

		void OnGUI() {
			if (HighLogic.LoadedScene != GameScenes.MAINMENU) {
				return;
			}
			GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height), QStyle.Label);
			GUILayout.Label (QuickStart.MOD + "...", QStyle.Label);
			GUILayout.EndArea ();
		}
	}
}