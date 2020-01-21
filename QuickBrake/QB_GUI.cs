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

using KSP.Localization;
using UnityEngine;

using ClickThroughFix;

namespace QuickBrake {
	public partial class QGUI {
		public static QGUI Instance {
			get;
			private set;
		}

		internal bool WindowSettings = false;

		Rect rectSettings = new Rect();
		Rect RectSettings {
			get {
				rectSettings.x = (Screen.width - rectSettings.width) / 2;
				rectSettings.y = (Screen.height - rectSettings.height) / 2;
				return rectSettings;
			}
			set {
				rectSettings = value;
			}
		}
		//internal QBlizzyToolbar BlizzyToolbar;
			
		protected override void Awake () {
			if (!HighLogic.LoadedSceneIsGame || QGUI.Instance != null) {
				Destroy (this);
			}
			Instance = this;

			Log ("Awake", "QGUI");
		}

		protected override void Start () {
			//BlizzyToolbar.Start ();
			Log ("Start", "QGUI");
		}

		protected override void OnDestroy () {
			//BlizzyToolbar.OnDestroy ();
			Log ("OnDestroy", "QGUI");
		}

		void Lock (bool activate, ControlTypes Ctrl = 0)	{
			if (activate) {
				InputLockManager.SetControlLock (Ctrl, "Lock" + RegisterToolbar.MOD);
				return;
			}
			InputLockManager.RemoveControlLock ("Lock" + RegisterToolbar.MOD);
			if (InputLockManager.GetControlLock ("Lock" + RegisterToolbar.MOD) != 0) {
				InputLockManager.RemoveControlLock ("Lock" + RegisterToolbar.MOD);
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

		void Save () {
			QStockToolbar.Instance.Reset ();
			//BlizzyToolbar.Reset ();
			QSettings.Instance.Save ();
			Log ("Save", "QGUI");
		}

		internal void OnGUI () {
			if (!WindowSettings) {
				return;
			}
			GUI.skin = HighLogic.Skin;
			RectSettings = ClickThruBlocker.GUILayoutWindow (1545165, RectSettings, DrawSettings, RegisterToolbar.MOD + " " + RegisterToolbar.VERSION, GUILayout.Width (RectSettings.width), GUILayout.ExpandHeight (true));
		}

		void DrawSettings (int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Box (Localizer.Format("quickbrake_toolbars"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Box (Localizer.Format("quickbrake_options"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			QSettings.Instance.AlwaysBrakeLandedVessel = GUILayout.Toggle (QSettings.Instance.AlwaysBrakeLandedVessel, Localizer.Format("quickbrake_landedVatStart"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
			if (!QSettings.Instance.AlwaysBrakeLandedVessel) {
				GUILayout.BeginHorizontal ();
				QSettings.Instance.AlwaysBrakeLandedPlane = GUILayout.Toggle (QSettings.Instance.AlwaysBrakeLandedPlane, Localizer.Format("quickbrake_landedPatStart"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				QSettings.Instance.AlwaysBrakeLandedRover = GUILayout.Toggle (QSettings.Instance.AlwaysBrakeLandedRover, Localizer.Format("quickbrake_landedRatStart"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				QSettings.Instance.AlwaysBrakeLandedBase = GUILayout.Toggle (QSettings.Instance.AlwaysBrakeLandedBase, Localizer.Format("quickbrake_landedBatStart"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				QSettings.Instance.AlwaysBrakeLandedLander = GUILayout.Toggle (QSettings.Instance.AlwaysBrakeLandedLander, Localizer.Format("quickbrake_landedLatStart"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				QSettings.Instance.EnableBrakeAtLaunchPad = GUILayout.Toggle (QSettings.Instance.EnableBrakeAtLaunchPad, Localizer.Format("quickbrake_launchPad"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				QSettings.Instance.EnableBrakeAtRunway = GUILayout.Toggle (QSettings.Instance.EnableBrakeAtRunway, Localizer.Format("quickbrake_runway"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
			}
			GUILayout.BeginHorizontal ();
			QSettings.Instance.EnableBrakeAtControlLost = GUILayout.Toggle (QSettings.Instance.EnableBrakeAtControlLost, Localizer.Format("quickbrake_controlLost"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			QSettings.Instance.EnableUnBrakeAtLaunch = GUILayout.Toggle (QSettings.Instance.EnableUnBrakeAtLaunch, Localizer.Format("quickbrake_unbrake"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button (Localizer.Format("quickbrake_close"), GUILayout.Height (30))) {
				HideSettings ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}
	}
}