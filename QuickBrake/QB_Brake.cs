/* 
QuickBrake
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

using System.Collections;
using UnityEngine;

namespace QuickBrake {

	public partial class QBrake {

		public static QBrake Instance {
			get;
			private set;
		}

		Coroutine controlLost;

		public bool BrakeIsLocked {
			get {
				return InputLockManager.IsLocked (ControlTypes.GROUP_BRAKES);
			}
		}

		public bool BrakeLandedRover(Vessel vessel) {
			return QSettings.Instance.AlwaysBrakeLandedRover && 
				            (vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.PRELAUNCH) && 
				            vessel.vesselType == VesselType.Rover;
		}

		public bool BrakeLandedBase(Vessel vessel) {
			return QSettings.Instance.AlwaysBrakeLandedBase && 
				            (vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.PRELAUNCH) && 
				            vessel.vesselType == VesselType.Base;
		}

		public bool BrakeLandedLander(Vessel vessel) {
			return QSettings.Instance.AlwaysBrakeLandedLander && 
				            (vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.PRELAUNCH) && 
				            vessel.vesselType == VesselType.Lander;
		}

		public bool BrakeLandedPlane(Vessel vessel) {
			return QSettings.Instance.AlwaysBrakeLandedPlane && 
				            (vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.PRELAUNCH) && 
				            vessel.vesselType == VesselType.Plane;
		}

		public bool BrakeLandedVessel(Vessel vessel) {
			return QSettings.Instance.AlwaysBrakeLandedVessel && 
				            (vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.PRELAUNCH);
		}

		public bool BrakeAtLaunchPad(Vessel vessel) {
			return QSettings.Instance.EnableBrakeAtLaunchPad && 
				            vessel.situation == Vessel.Situations.PRELAUNCH && 
				            vessel.landedAt == "LaunchPad";
		}

		public bool BrakeAtRunway(Vessel vessel) {
			return QSettings.Instance.EnableBrakeAtRunway && 
				            vessel.situation == Vessel.Situations.PRELAUNCH && 
				            vessel.landedAt == "Runway";
		}

		protected override void Awake() {
			if (!HighLogic.LoadedSceneIsGame || Instance != null) {
				Destroy (this);
			}
			Instance = this;
			GameEvents.OnFlightGlobalsReady.Add (OnFlightGlobalsReady);
			GameEvents.onLaunch.Add (OnLaunch);
			Log ("Awake", "QBrake");
		}

		protected override void Start() {
			controlLost = StartCoroutine (BrakeAtControlLost ());
			Log ("Start", "QBrake");
		}

		IEnumerator BrakeAtControlLost() {
			bool hasBrake = false;
			while (HighLogic.LoadedSceneIsFlight) {
				yield return new WaitForSecondsRealtime (1);
				Vessel _vessel = FlightGlobals.ActiveVessel;
				if (_vessel.CurrentControlLevel != Vessel.ControlLevel.FULL) {
					if (!hasBrake) {
						_vessel.ActionGroups.SetGroup (KSPActionGroup.Brakes, true);
						hasBrake = true;
						Log ("Brake at Control Lost", "QBrake");
					}
				}
				else if (hasBrake) {
					hasBrake = false;
				}
			}
		}

		void OnFlightGlobalsReady(bool ready) {
			Vessel _vessel = FlightGlobals.ActiveVessel;
			if (!ready || _vessel == null) {
				return;
			}
			if (BrakeLandedVessel(_vessel) || 
			    BrakeLandedRover(_vessel) || 
			    BrakeLandedBase(_vessel) || 
			    BrakeLandedLander(_vessel) || 
			    BrakeAtLaunchPad(_vessel) || 
			    BrakeAtRunway(_vessel) || 
			    BrakeLandedPlane(_vessel)) {
				_vessel.ActionGroups.SetGroup (KSPActionGroup.Brakes, true);
				Log ("Brake", "QBrake");
			}
		}

		void OnLaunch(EventReport e) {
			if (QSettings.Instance.EnableUnBrakeAtLaunch) {
				FlightGlobals.ActiveVessel.ActionGroups.SetGroup (KSPActionGroup.Brakes, false);
				Log ("Unbrake at Launch", "QBrake");
			}
		}

		protected override void OnDestroy() {
			GameEvents.OnFlightGlobalsReady.Remove (OnFlightGlobalsReady);
			GameEvents.onLaunch.Remove (OnLaunch);
			StopCoroutine (controlLost);
			Log ("OnDestroy", "QBrake");
		}
	}
}