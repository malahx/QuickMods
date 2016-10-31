/* 
QuickSAS
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

using System;
using System.Collections;
using UnityEngine;

namespace QuickSAS {

	public partial class QSAS {

		public static QSAS Instance {
			get;
			private set;
		}

		protected override void Awake() {
			if (!HighLogic.LoadedSceneIsGame || Instance != null) {
				Destroy (this);
			}
			Instance = this;
			Log ("Awake", "QSAS");
		}

		protected override void Start() {
			Log ("Start", "QSAS");
		}

		protected override void OnDestroy() {
			Log ("OnDestroy", "QSAS");
		}

		IEnumerator startSAS(VesselAutopilot.AutopilotMode autoPilot) {
			if (TimeWarp.CurrentRate > 1 && TimeWarp.WarpMode == TimeWarp.Modes.HIGH) {
				TimeWarp.fetch.CancelAutoWarp ();
				TimeWarp.SetRate (0, false);
			}
			while (TimeWarp.CurrentRate > 1 && TimeWarp.WarpMode == TimeWarp.Modes.HIGH) {
				yield return 0;
			}
			yield return new WaitForFixedUpdate ();
			Vessel _vessel = FlightGlobals.ActiveVessel;
			if (!_vessel.Autopilot.SAS.dampingMode) {
				_vessel.ActionGroups.SetGroup (KSPActionGroup.SAS, true);
			}
			yield return new WaitForFixedUpdate ();
			_vessel.Autopilot.SetMode (autoPilot);
		}

		void Update() {
			if (!HighLogic.LoadedSceneIsFlight) {
				return;
			}
			Vessel _vessel = FlightGlobals.ActiveVessel;
			string[] _keys = Enum.GetNames (typeof (QKey.Key));
			int _length = _keys.Length;
			for (int _key = 1; _key < _length; _key++) {
				QKey.Key _getKey = (QKey.Key)_key;
				if (_getKey == QKey.Key.WarpToNode) {
					continue;
				}
				if (QKey.isKeyDown (_getKey)) {
					StartCoroutine (startSAS (QKey.GetAutoPilot (_getKey)));
					ScreenMessages.PostScreenMessage (string.Format ("[{0}] {1}", MOD, QKey.GetText (_getKey)), 5, ScreenMessageStyle.UPPER_CENTER);
					Log (QKey.GetText (_getKey), "QSAS");
				}
			}
			if (QKey.isKeyDown (QKey.Key.WarpToNode)) {
				if (_vessel.patchedConicSolver.maneuverNodes.Count != 0) {
					double _UT;
					ManeuverNode _manNode = _vessel.patchedConicSolver.maneuverNodes[0];
					if (!QSettings.Instance.WarpToEnhanced) {
						_UT = _manNode.UT - 60;
					}
					else {
						double _estimatedBurnTime = _manNode.GetBurnVector (_vessel.orbit).magnitude / _vessel.specificAcceleration;
						_UT = _manNode.UT - (_estimatedBurnTime / 2) - 15;
					}
					if (Planetarium.GetUniversalTime () > _UT) {
						ScreenMessages.PostScreenMessage (string.Format ("[{0}] No need to time warp!", MOD), 5, ScreenMessageStyle.UPPER_CENTER);
						Log ("No need to time warp!", "QSAS");
						return;
					}
					TimeWarp.fetch.WarpTo (_UT);
					Log (QKey.GetText (QKey.Key.WarpToNode), "QSAS");
				}
				else {
					ScreenMessages.PostScreenMessage (string.Format ("[{0}] No maneuver node!", MOD), 5, ScreenMessageStyle.UPPER_CENTER);
					Log ("No maneuver node!", "QSAS");
				}
			}
		}
	}
}
