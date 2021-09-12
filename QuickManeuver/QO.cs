/* 
QuickManeuver
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

using System;
using UnityEngine;

namespace QuickManeuver {

	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class QuickManeuver : MonoBehaviour {
		private void Awake() {
			GameEvents.OnFlightGlobalsReady.Add(OnFlightGlobalsReady);
			Debug.Log ("[QuickManeuver] Awake");
		}

		private void OnFlightGlobalsReady(bool ready) {
			if (!ready || FlightGlobals.ActiveVessel == null) {
				return;
			}
			FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_INFO);
			FlightUIModeController.Instance.maneuverButton.ButtonCtrl.onClick.Invoke();
			Debug.Log("[QuickManeuver] Activate maneuver info");
			Destroy(this);
		}

		private void OnDestroy() {
			GameEvents.OnFlightGlobalsReady.Remove(OnFlightGlobalsReady);
			Debug.Log("[QuickManeuver] OnDestroy");
		}
	}
}
