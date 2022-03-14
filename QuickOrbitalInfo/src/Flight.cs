using UnityEngine;

namespace QuickOrbitalInfo {

	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class Flight : MonoBehaviour {
		private void Awake() {
			GameEvents.onFlightReady.Add(OnFlightReady);
			Debug.Log ($"[QuickOrbitalInfo]({name}) Awake");
		}

		private void OnFlightReady()
		{
			if (FlightGlobals.ActiveVessel == null) {
				return;
			}
			FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_INFO);
			FlightUIModeController.Instance.maneuverButton.SetState(true);
			FlightUIModeController.Instance.dockingButton.SetState(false);
			FlightUIModeController.Instance.stagingButton.SetState(false);
			FlightUIModeController.Instance.mapModeButton.SetState(false);
			Debug.Log($"[QuickOrbitalInfo]({name}) Activate maneuver info");
			Destroy(this);
		}

		private void OnDestroy() {
			GameEvents.onFlightReady.Remove(OnFlightReady);
			Debug.Log($"[QuickOrbitalInfo]({name}) OnDestroy");
		}
	}
}
