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

using UnityEngine;
using QuickStart.QUtils;

namespace QuickStart {

	public partial class QLoading {

		[KSPField (isPersistant = true)] public static bool Ended = false;

		internal bool WindowSettings = false;

		Rect rectSettings = new Rect ();
		Rect RectSettings {
			get {
                if (rectSettings.position == Vector2.zero && rectSettings.size != Vector2.zero) {
                    rectSettings.x = Screen.width - rectSettings.width;
                    rectSettings.y = Screen.width < 1440 ? 100 : Screen.height - rectSettings.height - 100;
                }
				return rectSettings;
			}
			set {
				rectSettings = value;
			}
		}

		Rect RectGUI {
			get {
				return new Rect (0, Screen.width < 1440 ? 0 : Screen.height - 50, Screen.width, 100);
			}
		}

		public static QLoading Instance {
			get;
			private set;
		}

		void Awake() {
			if (Ended) {
				QDebug.Warning ("Reload? Destroy.", "QLoading");
				Destroy (this);
				return;
			}
			if (HighLogic.LoadedScene != GameScenes.LOADING) {
				QDebug.Warning ("It's not a real Loading? Destroy.", "QLoading");
				Ended = true;
				Destroy (this);
				return;
			}
			if (Instance != null) {
				QDebug.Warning ("There's already an Instance", "QLoading");
				Destroy (this);
				return;
			}
			Instance = this;
			QDebug.Log ("Awake", "QLoading");
		}

		void Start() {
			if (string.IsNullOrEmpty (QSaveGame.LastUsed)) {
				ScreenMessages.PostScreenMessage ("[" + QuickStart.MOD + "]: No savegame found.", 10);
				QDebug.Log ("No savegame found, destroy...", "QLoading");
				Ended = true;
				Destroy (this);
				return;
			}
            QKey.VerifyKey();
			QDebug.Log ("Start", "QLoading");
		}

		void OnDestroy() {
			QSettings.Instance.Save ();
			QDebug.Log ("OnDestroy", "QLoading");
		}

        void Update() {
            if (QKey.SetKey()) {
                return;
            }
            if (QKey.isKeyDown(QKey.Key.Escape)) {
                QSettings.Instance.Enabled = false;
            }
        }

		void Settings() {
			WindowSettings = !WindowSettings;
			if (!WindowSettings) {
				QSettings.Instance.Save ();
			}
			QDebug.Log ("Settings", "QGUI");
		}

		void OnGUI() {
			if (HighLogic.LoadedScene != GameScenes.LOADING) {
				return;
			}
			if (string.IsNullOrEmpty (QSaveGame.LastUsed)) {
				return;
			}
			GUI.skin = HighLogic.Skin;

            QKey.DrawSetKey();

			if (WindowSettings) {
				RectSettings = GUILayout.Window (1545177, RectSettings, DrawSettings, QuickStart.MOD + " " + QuickStart.VERSION);
			}

			GUILayout.BeginArea (RectGUI);
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			if (QSettings.Instance.Enabled) {
				string _text;
				if (!string.IsNullOrEmpty (QSaveGame.LastUsed)) {
					_text = string.Format ("{0}: <color=white><b>{1}</b></color>", QLang.translate ("Last game found"), QSaveGame.LastUsed);
				}
				else {
					_text = string.Format("<b><color=#000000>{0}</color></b>", QLang.translate ("No last game found"));
				}
				GUILayout.Label (string.Format ("[{0}] {1}", QuickStart.MOD, _text));
				if (GUILayout.Button ("►", QStyle.Button, GUILayout.Width (20), GUILayout.Height (20))) {
					QSaveGame.Next ();
				}
			}
			GUILayout.FlexibleSpace ();
            if (GUILayout.Button (QLang.translate ("Settings"), QStyle.Button, GUILayout.Height (20))) {
				Settings ();
			}
			GUILayout.EndHorizontal ();
			if (!string.IsNullOrEmpty (QSaveGame.LastUsed)) {
				GUILayout.BeginHorizontal ();
				QSettings.Instance.Enabled = GUILayout.Toggle (QSettings.Instance.Enabled, QLang.translate("Enable") + " " + QuickStart.MOD, GUILayout.Width (250));
				if (QSettings.Instance.Enabled) {
					GUILayout.FlexibleSpace ();
					if (GUILayout.Toggle (QSettings.Instance.gameScene == (int)GameScenes.SPACECENTER, QLang.translate("Space Center"), GUILayout.Width (250))) {
						if (QSettings.Instance.gameScene != (int)GameScenes.SPACECENTER) {
							QSettings.Instance.gameScene = (int)GameScenes.SPACECENTER;
						}
					}
					GUILayout.FlexibleSpace ();
					if (GUILayout.Toggle (QSettings.Instance.editorFacility == (int)EditorFacility.VAB && QSettings.Instance.gameScene == (int)GameScenes.EDITOR, QLang.translate ("Vehicle Assembly Building"), GUILayout.Width (250))) {
						if (QSettings.Instance.gameScene != (int)GameScenes.EDITOR || QSettings.Instance.editorFacility != (int)EditorFacility.VAB) {
							QSettings.Instance.gameScene = (int)GameScenes.EDITOR;
							QSettings.Instance.editorFacility = (int)EditorFacility.VAB;
						}
					}
					if (Screen.width < 1440) {
						GUILayout.EndHorizontal ();
						GUILayout.BeginHorizontal ();
					}
					else {
						GUILayout.FlexibleSpace ();
					}
					if (GUILayout.Toggle (QSettings.Instance.editorFacility == (int)EditorFacility.SPH && QSettings.Instance.gameScene == (int)GameScenes.EDITOR, QLang.translate ("Space Plane Hangar"), GUILayout.Width (250))) {
						if (QSettings.Instance.gameScene != (int)GameScenes.EDITOR || QSettings.Instance.editorFacility != (int)EditorFacility.SPH) {
							QSettings.Instance.gameScene = (int)GameScenes.EDITOR;
							QSettings.Instance.editorFacility = (int)EditorFacility.SPH;
						}
					}
					GUILayout.FlexibleSpace ();
					if (GUILayout.Toggle (QSettings.Instance.gameScene == (int)GameScenes.TRACKSTATION, QLang.translate ("Tracking Station"), GUILayout.Width (250))) {
						if (QSettings.Instance.gameScene != (int)GameScenes.TRACKSTATION) {
							QSettings.Instance.gameScene = (int)GameScenes.TRACKSTATION;
						}
					}
					GUILayout.FlexibleSpace ();
					GUI.enabled = !string.IsNullOrEmpty (QuickStart_Persistent.vesselID);
					if (GUILayout.Toggle (QSettings.Instance.gameScene == (int)GameScenes.FLIGHT, (!string.IsNullOrEmpty (QSaveGame.vesselName) ? string.Format("{0}: {1}({2})", QLang.translate("Last Vessel"), QSaveGame.vesselName, QSaveGame.vesselType) : QLang.translate ("No vessel found")), GUILayout.Width (250))) {
						if (QSettings.Instance.gameScene != (int)GameScenes.FLIGHT) {
							QSettings.Instance.gameScene = (int)GameScenes.FLIGHT;
						}
					}
				}
				GUILayout.EndHorizontal ();
			}
			GUILayout.EndVertical ();
			GUILayout.EndArea ();
		}

		void DrawSettings(int id) {
			GUILayout.BeginVertical ();

			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate ("Options"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label (QLang.translate ("Wait after Loading in seconds:"), GUILayout.Width (300));
			int _int;
			if (int.TryParse (GUILayout.TextField (QSettings.Instance.WaitLoading.ToString (), GUILayout.Width (100)), out _int)) {
				QSettings.Instance.WaitLoading = _int;
			}
			GUILayout.EndHorizontal ();

            GUILayout.BeginHorizontal();
            bool b = GUILayout.Toggle(QSettings.Instance.enableBlackScreen, QLang.translate("Black screen when it waits"), GUILayout.Width(400));
            if (b != QSettings.Instance.enableBlackScreen) {
                QSettings.Instance.enableBlackScreen = b;
                QStyle.Label = null;
            }
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal ();
			QSettings.Instance.enableEditorAutoSaveShip = GUILayout.Toggle (QSettings.Instance.enableEditorAutoSaveShip, QLang.translate ("Auto Save Ship on Editor"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();

			if (QSettings.Instance.enableEditorAutoSaveShip) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label (QLang.translate ("Save each seconds:"), GUILayout.Width (300));
				if (int.TryParse (GUILayout.TextField (QSettings.Instance.editorTimeToSave.ToString (), GUILayout.Width (100)), out _int)) {
					QSettings.Instance.editorTimeToSave = _int;
				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.BeginHorizontal ();
			QSettings.Instance.enableEditorLoadAutoSave = GUILayout.Toggle (QSettings.Instance.enableEditorLoadAutoSave, QLang.translate ("Auto Load the Last Saved Ship"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			QSettings.Instance.enablePauseOnFlight = GUILayout.Toggle (QSettings.Instance.enablePauseOnFlight, QLang.translate ("Pause the Flight at Load"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
            QKey.DrawConfigKey();
			QLang.DrawLang ();
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button (QLang.translate ("Close"), GUILayout.Height (30))) {
				Settings ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}
	}
}