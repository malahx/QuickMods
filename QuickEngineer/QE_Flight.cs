/* 
QuickEngineer
Copyright 2016 Malah

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

using KerbalEngineer.VesselSimulator;
using System;
using System.Collections;
using UnityEngine;

namespace QuickEngineer {
	public partial class QFlight {

		public static QFlight Instance;

		public static bool appIsLive;

		private GUISkin skin;
		private GUIStyle scrollView;
		private GUIStyle stageTitle;
		private GUIStyle stageInfo;
		private GUIStyle windowTitle;
		private static Vector2 scrollPosition = new Vector2 ();

		private Vector2 flightEngineerDim = new Vector2 (170, 50);
		private Rect flightEngineerRect = new Rect();
		internal Rect FlightEngineerRect {
			get {
				if (QStockToolbar.Instance == null) {
					return new Rect ();
				}
				Rect _position = QStockToolbar.Instance.Position;
				if (_position.x + flightEngineerDim.x > Screen.width) {
					_position.x = Screen.width - flightEngineerDim.x;
				}
				flightEngineerRect.height = 75 + ((!QSettings.Instance.VesselEngineer_hidedeltaV ? 40 : 0) + (!QSettings.Instance.VesselEngineer_hideTWR ? 40 : 0));
				flightEngineerRect = new Rect (_position.x, _position.y + _position.height, flightEngineerDim.x, flightEngineerRect.height);
				return flightEngineerRect;
			}
			set {
				flightEngineerRect = value;
			}
		}

		internal void DisplayApp () {
			if (appIsLive) {
				Log ("App is already opened", "QFlight");
				return;
			}
			RenderingManager.AddToPostDrawQueue(3, new Callback(DrawEngineer));
			appIsLive = true;
			Log ("DisplayApp", "QFlight");
		}

		internal void HideApp () {
			if (!appIsLive) {
				Log ("App is already closed", "QFlight");
				return;
			}
			RenderingManager.RemoveFromPostDrawQueue(3, new Callback(DrawEngineer));
			appIsLive = false;
			Log ("HideApp", "QFlight");
		}

		public void OnAppDestroy ()	{
			Log ("OnDestroy", "QFlight");
		}

		internal IEnumerator OnAppStarted () {
			while (!QStockToolbar.isActive) {
				yield return 0;
			}

			Log ("OnAppStarted", "QFlight");
		}

		private void DrawEngineer() {
			GUI.skin = skin;
			FlightEngineerRect = GUILayout.Window(4512345, FlightEngineerRect, MainEngineer, string.Format ("{0} ({1})", MOD, VERSION), windowTitle);
		}

		private void MainEngineer(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			scrollPosition = GUILayout.BeginScrollView (scrollPosition, scrollView);
			if (QVessel.Stages != null && QVessel.Stages.Length > 0) {
				for (int i = 0; i < QVessel.Stages.Length; i++) {
					Stage _stage = QVessel.Stages [i];
					if (QSettings.Instance.VesselEngineer_hideEmptyStages && (_stage.deltaV == 0 || _stage.maxThrustToWeight == 0)) {
						continue;
					}
					GUILayout.BeginHorizontal();
					GUILayout.Label (textFlightStage (i), stageTitle);
					GUILayout.Label (textEngineer(i, _stage, false), stageInfo);
					GUILayout.EndHorizontal();
				}
			}
			GUILayout.BeginHorizontal();
			GUILayout.Label ("Total deltaV:", stageTitle);
			GUILayout.Label (textTotalDeltaV(QVessel.LastStage, false), stageInfo);
			GUILayout.EndHorizontal();
			GUILayout.EndScrollView ();
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}

		protected override void Awake() {
			if (Instance != null) {
				Warning ("There's already an Instance of " + MOD);
				Destroy (this);
				return;
			}
			Instance = this;
			Log ("Awake", "QFlight");
		}

		protected override void Start() {
			QSettings.Instance.Load ();
			if (!HighLogic.LoadedSceneIsFlight) {
				Warning ("It's not Flight scene? Destroy", "QFlight");
				Destroy (this);
				return;
			}
			if (QSettings.Instance.FlightVesselEngineer_Disable || QSettings.Instance.AllVesselEngineer_Disable) {
				Warning ("VesselEngineer is disabled. Destroy", "QFlight");
				Destroy (this);
				return;
			}
			skin = HighLogic.Skin;
			windowTitle = new GUIStyle (skin.window);
			windowTitle.alignment = TextAnchor.UpperLeft;
			windowTitle.fontSize = 14;
			windowTitle.normal.textColor = new Color (1, 1, 1);
			windowTitle.stretchHeight = true;
			scrollView = new GUIStyle (skin.scrollView);
			scrollView.stretchHeight = true;
			stageTitle = new GUIStyle(skin.label);
			stageTitle.alignment = TextAnchor.UpperLeft;
			stageTitle.fontSize = 12;
			stageTitle.normal.textColor = new Color (1, 1, 1);
			stageTitle.stretchWidth = true;
			stageTitle.margin = new RectOffset (1, 1, 1, 1);
			stageTitle.padding = new RectOffset (-5, 1, 1, 1);
			stageInfo = new GUIStyle(stageTitle);
			stageInfo.alignment = TextAnchor.UpperRight;
			stageInfo.normal.textColor = new Color (0.698f, 0.824f, 0.337f);
			Log ("Start", "QFlight");
		}

		private void Update() {
			if (!QStockToolbar.isActive) {
				return;
			}
			if (appIsLive) {
				Vessel _vessel = FlightGlobals.ActiveVessel;
				if (_vessel != null) {
					if (TimeWarp.WarpMode == TimeWarp.Modes.LOW || TimeWarp.CurrentRate <= TimeWarp.MaxPhysicsRate) {
						QStage _qStage = new QStage (_vessel.mainBody, _vessel.atmDensity != 0d, _vessel.mach, (float)_vessel.altitude);
						QVessel.Init (_qStage);
						QVessel.StartSim ();
					}
				}
			}
		}
	}
}