/* 
QuickRevert
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

namespace QuickRevert {
	public partial class QFlight {

		[KSPField(isPersistant = true)]	internal readonly static QFlightData data = new QFlightData ();

		public static QFlight Instance;

		protected override void Awake() {
			if (!HighLogic.LoadedSceneIsFlight) {
				Warning ("QFlight needs to be load only on flight.", "QFlight");
				Destroy (this);
				return;
			}
			if (Instance != null) {
				Warning ("There's already an Instance", "QFlight");
				Destroy (this);
				return;
			}
			Instance = this;
			if (QSettings.Instance.EnableRevertKeep) {
				GameEvents.onFlightReady.Add (OnFlightReady);
			}
			if (QSettings.Instance.EnableRevertLoss) {
				if (Planetarium.fetch.Home.atmosphere) {
					GameEvents.VesselSituation.onReachSpace.Add (OnReachSpace);
				}
				else {
					GameEvents.VesselSituation.onEscape.Add (OnEscape);
				}
			}
			Log ("Awake", "QFlight");
		}

		protected override void Start() {
			Log ("Start", "QFlight");
		}

		protected override void OnDestroy() {
			GameEvents.onFlightReady.Remove (OnFlightReady);
			GameEvents.VesselSituation.onReachSpace.Remove (OnReachSpace);
			GameEvents.VesselSituation.onEscape.Remove (OnEscape);
			Log ("OnDestroy", "QFlight");
		}

		void OnFlightReady() {
			if (!data.isActiveVessel) {
				if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.PRELAUNCH) {
					if (data.Store ()) {
						ScreenMessages.PostScreenMessage (string.Format ("[{0}] " + QLang.translate ("Revert saved"), MOD), 10, ScreenMessageStyle.UPPER_CENTER);
					}
				}
			}
			else {
				if (data.Restore ()) {
					ScreenMessages.PostScreenMessage (string.Format ("[{0}] " + QLang.translate ("Revert restored"), MOD), 10, ScreenMessageStyle.UPPER_CENTER);
				}
			}
			Log ("OnFlightReady", "QFlight");
		}

		void OnReachSpace (Vessel vessel) {
			if (data.VesselGuid != vessel.id || !vessel.mainBody.isHomeWorld) {
				return;
			}
			data.Reset ();
			Log ("OnReachSpace: " + vessel.vesselName, "QFlight");
		}

		void OnEscape (Vessel vessel, CelestialBody body) {
			if (data.VesselGuid != vessel.id || !vessel.mainBody.isHomeWorld) {
				return;
			}
			data.Reset ();
			Log ("OnEscape: " + vessel.vesselName, "QFlight");
		}
	}
}