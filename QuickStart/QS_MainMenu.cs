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
using System;
using KSP.Localization;

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
				QDebug.Warning ("Reload? Destroy.", "QMainMenu");
				Destroy (this);
				return;
			}
			if (Instance != null) {
				QDebug.Warning ("There's already an Instance", "QMainMenu");
				Destroy (this);
				return;
			}
			Instance = this;
			QDebug.Log ("Awake", "QMainMenu");
		}

		void Start() {
			if (!QSettings.Instance.Enabled) {
				QDebug.Log ("No need to keep it loaded.", "QMainMenu");
				DestroyThis ();
				return;
			}
			start = StartCoroutine (QStart ());
			QDebug.Log ("Start", "QMainMenu");
		}

		IEnumerator QStart() {
			if (string.IsNullOrEmpty (QSaveGame.LastUsed)) {
				QDebug.Warning ("Last savegame not found!", "QMainMenu");
				DestroyThis ();
				yield break;
			}
			if (!QSettings.Instance.Enabled) {
				QDebug.Log ("QuickStart is disabled!", "QMainMenu");
				DestroyThis ();
				yield break;
			}
			while (!Ready) {
				yield return 0;
			}
			yield return new WaitForEndOfFrame ();
			yield return new WaitForSeconds (QSettings.Instance.WaitLoading);
			yield return new WaitForEndOfFrame ();
			QDebug.Log ("MainMenu Loaded", "QMainMenu");
			QDebug.Warning ("The last game found: " + QSaveGame.LastUsed, "QMainMenu");
			HighLogic.CurrentGame = GamePersistence.LoadGame (QSaveGame.FILE, QSaveGame.LastUsed, true, false);
			if (HighLogic.CurrentGame != null) {
				HighLogic.SaveFolder = QSaveGame.LastUsed;
				if (GamePersistence.UpdateScenarioModules (HighLogic.CurrentGame)) {
					GamePersistence.SaveGame (HighLogic.CurrentGame, QSaveGame.FILE, HighLogic.SaveFolder, SaveMode.OVERWRITE);
				}
				QDebug.Log ("Goto SpaceCenter", "QMainMenu");
				HighLogic.CurrentGame.startScene = GameScenes.SPACECENTER;
				HighLogic.CurrentGame.Start ();
				InputLockManager.ClearControlLocks ();
				Destroy (this);
				yield break;
			}
			QDebug.Warning ("Can't load the last save game", "QMainMenu");
			DestroyThis ();
			yield break;
		}

		void LateUpdate() {
			if (!Ready) {
				QDebug.Log ("Ready", "QMainMenu");
				Ready = true;
			}
		}

        void Update() {
            if (QKey.isKeyDown(QKey.Key.Escape)) {
                if (start != null) {
                    StopCoroutine(start);
                    QDebug.Log("Escape", "QSpaceCenter");
                    DestroyThis();
                }
            }
        }

		void DestroyThis() {
			QDebug.Log ("DestroyThis", "QMainMenu");
			QuickStart_Persistent.vesselID = string.Empty;
			QLoading.Ended = true;
			Destroy (this);
		}

		void OnDestroy() {
			QDebug.Log ("OnDestroy", "QMainMenu");
		}

		void OnGUI() {
			QuickStart_Persistent.SkippingScreen(GameScenes.MAINMENU, Localizer.Format("quickstart_mainMenu"));
		}
	}
}