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
using KSP.Localization;

using ClickThroughFix;

namespace QuickStart {

    public partial class QLoading {

		[KSPField (isPersistant = true)] public static bool Ended = false;

		internal bool WindowSettings = false;
		private string StopWatchText = "";

		Rect rectSettings = new Rect ();
		Rect RectSettings {
			get {
                if (rectSettings.position == Vector2.zero && rectSettings.size != Vector2.zero) {
                    rectSettings.x = Screen.width - rectSettings.width;
                    rectSettings.y = Screen.height - rectSettings.height - 100;
                }
				return rectSettings;
			}
			set {
				rectSettings = value;
			}
		}

		Rect RectGUI {
			get {
				return new Rect (0, Screen.height - 50, Screen.width, 100);
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

            InvokeRepeating("UpdateStopWatch", 0, 1.0f);
        }

		void OnDestroy() {
			QSettings.Instance.Save ();
			QDebug.Log ("OnDestroy", "QLoading");
		}

        void UpdateStopWatch() {
            StopWatchText = QuickStart_Persistent.StopWatchText;
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

        static class LabelWidth {
            public static GUILayoutOption Enabled { get; } = GUILayout.Width(200);
            public static GUILayoutOption KSC { get; } = GUILayout.Width(100);
            public static GUILayoutOption VAB { get; } = GUILayout.Width(100);
            public static GUILayoutOption SPH { get; } = GUILayout.Width(100);
            public static GUILayoutOption TrackingStation { get; } = GUILayout.Width(200);
            public static GUILayoutOption Vessel { get; } = GUILayout.Width(250);
        };

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
				RectSettings = ClickThruBlocker.GUILayoutWindow (1545177, RectSettings, DrawSettings, QuickStart.MOD + " " + QuickStart.VERSION);
			}

            GUILayout.BeginArea (RectGUI);
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
            GUILayout.Label(StopWatchText);

            if (QSettings.Instance.Enabled) {
                if (GUILayout.Button("◄", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    QSaveGame.Prev();
                }
                GUILayout.Label(!string.IsNullOrEmpty(QSaveGame.LastUsed) ?
                                                     Localizer.Format("quickstart_lastGame", QSaveGame.LastUsed) :
                                                     Localizer.Format("quickstart_noLastGame"));
				if (GUILayout.Button ("►", QStyle.Button, GUILayout.Width (20), GUILayout.Height (20))) {
					QSaveGame.Next ();
				}
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();

			if (!string.IsNullOrEmpty (QSaveGame.LastUsed)) {
				GUILayout.BeginHorizontal ();
				QSettings.Instance.Enabled = GUILayout.Toggle (QSettings.Instance.Enabled, Localizer.Format("quickstart_enable", QuickStart.MOD), LabelWidth.Enabled);
				if (QSettings.Instance.Enabled) {
                    if (QSettings.Instance.evenlySpaceToggles)
					    GUILayout.FlexibleSpace ();
					if (GUILayout.Toggle (QSettings.Instance.gameScene == (int)GameScenes.SPACECENTER, Localizer.Format(
                         QSettings.Instance.abbreviations ? "quickstart_sc_abbr" : "quickstart_sc"
                        ), LabelWidth.KSC)) {
						if (QSettings.Instance.gameScene != (int)GameScenes.SPACECENTER) {
							QSettings.Instance.gameScene = (int)GameScenes.SPACECENTER;
						}
					}
                    if (QSettings.Instance.evenlySpaceToggles)
                        GUILayout.FlexibleSpace();
                    if (GUILayout.Toggle (QSettings.Instance.editorFacility == (int)EditorFacility.VAB && QSettings.Instance.gameScene == (int)GameScenes.EDITOR, Localizer.Format(
                        QSettings.Instance.abbreviations?"quickstart_vab_abbr": "quickstart_vab"
                        ), LabelWidth.VAB)) {
						if (QSettings.Instance.gameScene != (int)GameScenes.EDITOR || QSettings.Instance.editorFacility != (int)EditorFacility.VAB) {
							QSettings.Instance.gameScene = (int)GameScenes.EDITOR;
							QSettings.Instance.editorFacility = (int)EditorFacility.VAB;
						}
					}
                    if (QSettings.Instance.evenlySpaceToggles)
                        GUILayout.FlexibleSpace();
                    if (GUILayout.Toggle (QSettings.Instance.editorFacility == (int)EditorFacility.SPH && QSettings.Instance.gameScene == (int)GameScenes.EDITOR, Localizer.Format(
                           QSettings.Instance.abbreviations ? "quickstart_sph_abbr" : "quickstart_sph"
                        ), LabelWidth.SPH)) {
						if (QSettings.Instance.gameScene != (int)GameScenes.EDITOR || QSettings.Instance.editorFacility != (int)EditorFacility.SPH) {
							QSettings.Instance.gameScene = (int)GameScenes.EDITOR;
							QSettings.Instance.editorFacility = (int)EditorFacility.SPH;
						}
					}
                    if (QSettings.Instance.evenlySpaceToggles)
                        GUILayout.FlexibleSpace();
                    if (GUILayout.Toggle (QSettings.Instance.gameScene == (int)GameScenes.TRACKSTATION, Localizer.Format("quickstart_ts"), LabelWidth.TrackingStation)) {
						if (QSettings.Instance.gameScene != (int)GameScenes.TRACKSTATION) {
							QSettings.Instance.gameScene = (int)GameScenes.TRACKSTATION;
						}
					}
                    if (QSettings.Instance.evenlySpaceToggles)
                        GUILayout.FlexibleSpace();
                    GUI.enabled = !string.IsNullOrEmpty (QuickStart_Persistent.vesselID);
					if (GUILayout.Toggle (QSettings.Instance.gameScene == (int)GameScenes.FLIGHT, (!string.IsNullOrEmpty (QSaveGame.vesselName) ? Localizer.Format("quickstart_lastVessel", QSaveGame.vesselName, QSaveGame.vesselType) : Localizer.Format("quickstart_noVessel")), LabelWidth.Vessel)) {
						if (QSettings.Instance.gameScene != (int)GameScenes.FLIGHT) {
							QSettings.Instance.gameScene = (int)GameScenes.FLIGHT;
						}
					}
                    GUI.enabled = true;
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(Localizer.Format("quickstart_settings"), QStyle.Button, GUILayout.Height(20)))
                    {
                        Settings();
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
			GUILayout.Box (Localizer.Format("quickstart_options"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();

            GUILayout.BeginHorizontal();
            QSettings.Instance.enableStopWatch = GUILayout.Toggle(QSettings.Instance.enableStopWatch, Localizer.Format("quickstart_enableStopWatch"), GUILayout.Width(400));
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal ();
			GUILayout.Label (Localizer.Format("quickstart_waitLoading"), GUILayout.Width (300));
			int _int;
			if (int.TryParse (GUILayout.TextField (QSettings.Instance.WaitLoading.ToString (), GUILayout.Width (100)), out _int)) {
				QSettings.Instance.WaitLoading = _int;
			}
			GUILayout.EndHorizontal ();

            GUILayout.BeginHorizontal();
            bool b = GUILayout.Toggle(QSettings.Instance.enableBlackScreen, Localizer.Format("quickstart_blackScreen"), GUILayout.Width(400));
            if (b != QSettings.Instance.enableBlackScreen) {
                QSettings.Instance.enableBlackScreen = b;
                QStyle.Label = null;
            }
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal ();
			QSettings.Instance.enableEditorAutoSaveShip = GUILayout.Toggle (QSettings.Instance.enableEditorAutoSaveShip, Localizer.Format("quickstart_autoSaveShip"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();

			if (QSettings.Instance.enableEditorAutoSaveShip) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label (Localizer.Format("quickstart_saveEach"), GUILayout.Width (300));
				if (int.TryParse (GUILayout.TextField (QSettings.Instance.editorTimeToSave.ToString (), GUILayout.Width (100)), out _int)) {
					QSettings.Instance.editorTimeToSave = _int;
				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.BeginHorizontal ();
			QSettings.Instance.enableEditorLoadAutoSave = GUILayout.Toggle (QSettings.Instance.enableEditorLoadAutoSave, Localizer.Format("quickstart_loadLastShip"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			QSettings.Instance.enablePauseOnFlight = GUILayout.Toggle (QSettings.Instance.enablePauseOnFlight, Localizer.Format("quickstart_pauseLoad"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();

            GUILayout.BeginHorizontal();
            QSettings.Instance.evenlySpaceToggles = GUILayout.Toggle(QSettings.Instance.evenlySpaceToggles, Localizer.Format("quickstart_evenlySpaceToggles"), GUILayout.Width(400));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            QSettings.Instance.abbreviations = GUILayout.Toggle(QSettings.Instance.abbreviations, Localizer.Format("quickstart_abbreviations"), GUILayout.Width(400));
            GUILayout.EndHorizontal();

            

            QKey.DrawConfigKey();

            GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button (Localizer.Format("quickstart_close"), GUILayout.Height (30))) {
				Settings ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();

            GUI.DragWindow();
		}
	}
}