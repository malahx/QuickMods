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

namespace QuickStart {
	public partial class QFlight {

		public static QFlight Instance {
			get;
			private set;
		}

		void Awake() {
			if (QLoading.Ended) {
				QuickStart.Warning ("Reload? Destroy.", "QLoading");
				Destroy (this);
				return;
			}
			if (!QSettings.Instance.enablePauseOnFlight || !QSettings.Instance.Enabled || QSettings.Instance.gameScene != (int)GameScenes.FLIGHT) {
				QuickStart.Log ("Not need to keep it loaded.", "QFlight");
				QLoading.Ended = true;
				Destroy (this);
				return;
			}
			if (Instance != null) {
				QuickStart.Warning ("There's already an Instance", "QFlight");
				Destroy (this);
				return;
			}
			Instance = this;
			GameEvents.onFlightReady.Add (OnFlightReady);
			QuickStart.Log ("Awake", "QFlight");
		}

		void Start() {
			QuickStart.Log ("Start", "QFlight");
		}

		void OnDestroy() {
			GameEvents.onFlightReady.Remove (OnFlightReady);
			QuickStart.Log ("OnDestroy", "QFlight");
		}

		void OnFlightReady() {
			PauseMenu.Display ();
			QLoading.Ended = true;
			QuickStart.Log ("Not need to keep it loaded.", "QFlight");
			Destroy (this);
			return;
		}
	}
}