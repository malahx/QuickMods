/* 
QuickBrake
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

namespace QuickBrake {
	public partial class QGUI {
		public static QGUI Instance {
			get;
			private set;
		}

		internal bool WindowSettings = false;

		private Rect rectSettings = new Rect();
		private Rect RectSettings {
			get {
				rectSettings.x = (Screen.width - rectSettings.width) / 2;
				rectSettings.y = (Screen.height - rectSettings.height) / 2;
				return rectSettings;
			}
			set {
				rectSettings = value;
			}
		}
		internal QBlizzyToolbar BlizzyToolbar;
			
		protected override void Awake () {
			if (!HighLogic.LoadedSceneIsGame || QGUI.Instance != null) {
				Destroy (this);
			}
			Instance = this;
			if (BlizzyToolbar == null) {
				BlizzyToolbar = new QBlizzyToolbar ();
			}
			Log ("Awake", "QGUI");
		}

		protected override void Start () {
			BlizzyToolbar.Start ();
			Log ("Start", "QGUI");
		}

		protected override void OnDestroy () {
			BlizzyToolbar.OnDestroy ();
			Log ("OnDestroy", "QGUI");
		}

		private void Lock (bool activate, ControlTypes Ctrl = 0)	{
			if (activate) {
				InputLockManager.SetControlLock (Ctrl, "Lock" + MOD);
				return;
			}
			InputLockManager.RemoveControlLock ("Lock" + MOD);
			if (InputLockManager.GetControlLock ("Lock" + MOD) != 0) {
				InputLockManager.RemoveControlLock ("Lock" + MOD);
			}
			Log ("Lock: " + activate, "QGUI");
		}

		public void Settings () {
			if (WindowSettings) {
				HideSettings ();
			}
			else {
				ShowSettings ();
			}
			Log ("Settings", "QGUI");
		}

		internal void ShowSettings () {
			WindowSettings = true;
			Switch (true);
			Log ("ShowSettings", "QGUI");
		}
			
		internal void HideSettings () {
			WindowSettings = false;
			Switch (false);
			Save ();
			Log ("HideSettings", "QGUI");
		}

		internal void Switch (bool set)	{
			QStockToolbar.Instance.Set (WindowSettings, false);
			QGUI.Instance.Lock (WindowSettings, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
			Log ("Switch: " + set, "QGUI");
		}

		private void Save () {
			QStockToolbar.Instance.Reset ();
			BlizzyToolbar.Reset ();
			QSettings.Instance.Save ();
			Log ("Save", "QGUI");
		}

		internal void OnGUI () {
			if (!WindowSettings) {
				return;
			}
			GUI.skin = HighLogic.Skin;
			RectSettings = GUILayout.Window (1545165, RectSettings, DrawSettings, QuickBrake.MOD + " " + QuickBrake.VERSION, GUILayout.Width (RectSettings.width), GUILayout.ExpandHeight (true));
		}

		private void DrawSettings (int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate ("Toolbars"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, QLang.translate ("Use the Stock Toolbar"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
			if (QBlizzyToolbar.isAvailable) {
				GUILayout.BeginHorizontal ();
				QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, QLang.translate ("Use the Blizzy Toolbar"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
			}
			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate ("Options"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			QSettings.Instance.AlwaysBrakeLandedVessel = GUILayout.Toggle (QSettings.Instance.AlwaysBrakeLandedVessel, QLang.translate ("Enable Brake Landed Vessel at Start"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
			if (!QSettings.Instance.AlwaysBrakeLandedVessel) {
				GUILayout.BeginHorizontal ();
				QSettings.Instance.AlwaysBrakeLandedPlane = GUILayout.Toggle (QSettings.Instance.AlwaysBrakeLandedPlane, QLang.translate ("Enable Brake Landed Plane at Start"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				QSettings.Instance.AlwaysBrakeLandedRover = GUILayout.Toggle (QSettings.Instance.AlwaysBrakeLandedRover, QLang.translate ("Enable Brake Landed Rover at Start"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				QSettings.Instance.AlwaysBrakeLandedBase = GUILayout.Toggle (QSettings.Instance.AlwaysBrakeLandedBase, QLang.translate ("Enable Brake Landed Base at Start"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				QSettings.Instance.AlwaysBrakeLandedLander = GUILayout.Toggle (QSettings.Instance.AlwaysBrakeLandedLander, QLang.translate ("Enable Brake Landed Lander at Start"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				QSettings.Instance.EnableBrakeAtLaunchPad = GUILayout.Toggle (QSettings.Instance.EnableBrakeAtLaunchPad, QLang.translate ("Enable Brake at LaunchPad"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				QSettings.Instance.EnableBrakeAtRunway = GUILayout.Toggle (QSettings.Instance.EnableBrakeAtRunway, QLang.translate ("Enable Brake at Runway"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
			}
			GUILayout.BeginHorizontal ();
			QSettings.Instance.EnableBrakeAtControlLost = GUILayout.Toggle (QSettings.Instance.EnableBrakeAtControlLost, QLang.translate ("Enable Brake at Control Lost"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			QSettings.Instance.EnableUnBrakeAtLaunch = GUILayout.Toggle (QSettings.Instance.EnableUnBrakeAtLaunch, QLang.translate ("Enable UnBrake at Launch"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
			QLang.DrawLang ();
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button (QLang.translate ("Close"), GUILayout.Height (30))) {
				HideSettings ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}
	}
}