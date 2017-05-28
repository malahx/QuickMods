/* 
QuickSAS
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

namespace QuickSAS {
	public class QKey : QuickSAS {
	
		internal static Key SetKey = Key.None;

		internal enum Key {
			None,
			Current,
			Prograde,
			Retrograde,
			Normal,
			AntiNormal,
			RadialIn,
			RadialOut,
			TargetPrograde,
			TargetRetrograde,
			Maneuver,
			WarpToNode,
		}					
	
		internal static KeyCode DefaultKey(Key key) {
			switch (key) {
				case Key.Current:
					return KeyCode.Keypad5;
				case Key.Prograde:
					return KeyCode.Keypad8;
				case Key.Retrograde:
					return KeyCode.Keypad2;
				case Key.Normal:
					return KeyCode.Keypad9;
				case Key.AntiNormal:
					return KeyCode.Keypad3;
				case Key.RadialIn:
					return KeyCode.Keypad6;
				case Key.RadialOut:
					return KeyCode.Keypad4;
				case Key.TargetPrograde:
					return KeyCode.Keypad7;
				case Key.TargetRetrograde:
					return KeyCode.Keypad1;
				case Key.Maneuver:
					return KeyCode.Keypad0;
				case Key.WarpToNode:
					return KeyCode.KeypadEnter;
			}
			return KeyCode.None;
		}

		internal static bool isKeyDown(Key key) {
			return Input.GetKeyDown (CurrentKey (key));
		}

		internal static string GetText(Key key) {
			switch (key) {
				case Key.Current:
					return Localizer.Format("quicksas_stability");
				case Key.Prograde:
					return Localizer.Format("quicksas_prograde");
				case Key.Retrograde:
					return Localizer.Format("quicksas_retrograde");
				case Key.Normal:
					return Localizer.Format("quicksas_normal");
				case Key.AntiNormal:
					return Localizer.Format("quicksas_antiNormal");
				case Key.RadialIn:
					return Localizer.Format("quicksas_radial");
				case Key.RadialOut:
					return Localizer.Format("quicksas_antiRadial");
				case Key.TargetPrograde:
					return Localizer.Format("quicksas_target");
				case Key.TargetRetrograde:
					return Localizer.Format("quicksas_antiTarget");
				case Key.Maneuver:
					return Localizer.Format("quicksas_maneuver");
				case Key.WarpToNode:
					return Localizer.Format("quicksas_warpToNode");
			}
			return string.Empty;
		}

		internal static VesselAutopilot.AutopilotMode GetAutoPilot(Key key) {
			switch (key) {
				case Key.Prograde:
					return VesselAutopilot.AutopilotMode.Prograde;
				case Key.Retrograde:
					return VesselAutopilot.AutopilotMode.Retrograde;
				case Key.Normal:
					return VesselAutopilot.AutopilotMode.Normal;
				case Key.AntiNormal:
					return VesselAutopilot.AutopilotMode.Antinormal;
				case Key.RadialIn:
					return VesselAutopilot.AutopilotMode.RadialIn;
				case Key.RadialOut:
					return VesselAutopilot.AutopilotMode.RadialOut;
				case Key.TargetPrograde:
					return VesselAutopilot.AutopilotMode.Target;
				case Key.TargetRetrograde:
					return VesselAutopilot.AutopilotMode.AntiTarget;
				case Key.Maneuver:
					return VesselAutopilot.AutopilotMode.Maneuver;
			}
			return VesselAutopilot.AutopilotMode.StabilityAssist;
		}

		internal static KeyCode CurrentKey(Key key) {
			switch (key) {
				case Key.Current:
					return QSettings.Instance.KeyCurrent;
				case Key.Prograde:
					return QSettings.Instance.KeyPrograde;
				case Key.Retrograde:
					return QSettings.Instance.KeyRetrograde;
				case Key.Normal:
					return QSettings.Instance.KeyNormal;
				case Key.AntiNormal:
					return QSettings.Instance.KeyAntiNormal;
				case Key.RadialIn:
					return QSettings.Instance.KeyRadialIn;
				case Key.RadialOut:
					return QSettings.Instance.KeyRadialOut;
				case Key.TargetPrograde:
					return QSettings.Instance.KeyTargetPrograde;
				case Key.TargetRetrograde:
					return QSettings.Instance.KeyTargetRetrograde;
				case Key.Maneuver:
					return QSettings.Instance.KeyManeuver;
				case Key.WarpToNode:
					return QSettings.Instance.KeyWarpToNode;
			}
			return KeyCode.None;
		}

		internal static void VerifyKey(Key key) {
			try {
				Input.GetKey(CurrentKey(key));
			} catch {
				Warning ("Wrong key: " + CurrentKey(key), "QKey");
				SetCurrentKey (key, DefaultKey(key));
			}
		}

		internal static void VerifyKey() {
			VerifyKey (Key.Current);
			VerifyKey (Key.Prograde);
			VerifyKey (Key.Retrograde);
			VerifyKey (Key.Normal);
			VerifyKey (Key.AntiNormal);
			VerifyKey (Key.RadialIn);
			VerifyKey (Key.RadialOut);
			VerifyKey (Key.TargetPrograde);
			VerifyKey (Key.TargetRetrograde);
			VerifyKey (Key.Maneuver);
			VerifyKey (Key.WarpToNode);
			Log ("VerifyKey", "QKey");
		}

		internal static void SetCurrentKey(Key key, KeyCode currentKey) {
			switch (key) {
				case Key.Current:
					QSettings.Instance.KeyCurrent = currentKey;
					break;
				case Key.Prograde:
					QSettings.Instance.KeyPrograde = currentKey;
					break;
				case Key.Retrograde:
					QSettings.Instance.KeyRetrograde = currentKey;
					break;
				case Key.Normal:
					QSettings.Instance.KeyNormal = currentKey;
					break;
				case Key.AntiNormal:
					QSettings.Instance.KeyAntiNormal = currentKey;
					break;
				case Key.RadialIn:
					QSettings.Instance.KeyRadialIn = currentKey;
					break;
				case Key.RadialOut:
					QSettings.Instance.KeyRadialOut = currentKey;
					break;
				case Key.TargetPrograde:
					QSettings.Instance.KeyTargetPrograde = currentKey;
					break;
				case Key.TargetRetrograde:
					QSettings.Instance.KeyTargetRetrograde = currentKey;
					break;
				case Key.Maneuver:
					QSettings.Instance.KeyManeuver = currentKey;
					break;
				case Key.WarpToNode:
					QSettings.Instance.KeyWarpToNode = currentKey;
					break;
			}
			Log (string.Format("SetCurrentKey({0}): {1}", GetText(key), currentKey), "QKey");
		}

		internal static void DrawSetKey(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label (Localizer.Format("quicksas_pressKey", GetText (SetKey)));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (Localizer.Format("quicksas_clearAssign"), GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, KeyCode.None);
				SetKey = Key.None;
			}
			if (GUILayout.Button (Localizer.Format("quicksas_defaultAssign"), GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, DefaultKey (SetKey));
				SetKey = Key.None;
			}
			if (GUILayout.Button (Localizer.Format("quicksas_cancelAssign"), GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetKey = Key.None;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			GUILayout.EndVertical ();
		}

		internal static void DrawConfigKey(Key key) {
			GUILayout.BeginHorizontal ();
			GUILayout.Label (string.Format ("{0}: <color=#FFFFFF><b>{1}</b></color>", GetText (key), CurrentKey (key)), GUILayout.Width (350));
			GUILayout.FlexibleSpace();
			if (GUILayout.Button (Localizer.Format("quicksas_set"), GUILayout.ExpandWidth (true), GUILayout.Height (20))) {
				SetKey = key;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
		}
	}
}