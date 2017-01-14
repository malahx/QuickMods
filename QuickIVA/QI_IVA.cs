/* 
QuickIVA
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickIVA {
	public partial class QIVA {

		public static QIVA Instance {
			get;
			private set;
		}

		bool isGoneIVA = false;
		bool NoMoreGoIVA = false;
		Kerbal EVAKerbal = null;
		ScreenMessage ScreenMsg = null;

		bool BlockedGoIVA {
			get {
				return isGoneIVA || NoMoreGoIVA;
			}
			set {
				isGoneIVA = value;
				NoMoreGoIVA = value;
			}
		}

		bool IVAIsInstantiate {
			get {
				if (CameraManager.Instance == null) {
					return false;
				}
				return CameraManager.Instance.enabled;
			}
		}

		bool isIVA() {
			if (!IVAIsInstantiate) {
				return false;
			}
			return isIVA(CameraManager.Instance.currentCameraMode);
		}

		bool isIVA(CameraManager.CameraMode cameraMode) {
			return cameraMode == CameraManager.CameraMode.IVA;
		}

		bool isMAP {
			get {
				if (!IVAIsInstantiate) {
					return false;
				}
				return CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Map;
			}
		}

		Kerbal CheckIVAKerbal(Vessel vessel) {
			List<InternalSeat> _seats = VesselSeats (vessel);
			for (int _i = _seats.Count - 1; _i >= 0; --_i) {
				Kerbal _kerbal = _seats[_i].kerbalRef;
				if (_kerbal.eyeTransform == InternalCamera.Instance.transform.parent) {
					return _kerbal;
				}
			}
			return null;
		}

		bool CheckEVAUnlocked (Vessel vessel) {
			return GameVariables.Instance.EVAIsPossible(GameVariables.Instance.UnlockedEVA (ScenarioUpgradeableFacilities.GetFacilityLevel (SpaceCenterFacility.AstronautComplex)), vessel);
		}

		bool CheckVessel(Vessel vessel) {
			return !vessel.isEVA && !vessel.packed && (vessel.situation != Vessel.Situations.PRELAUNCH || !QSettings.Instance.IVAatLaunch);
		}

		bool KerbalIsOnVessel (Vessel vessel, Kerbal kerbal) {
			if (kerbal == null) {
				return false;
			}
			return vessel.GetVesselCrew ().Contains (kerbal.protoCrewMember);
		}

		List<InternalSeat> VesselSeats(Vessel vessel) {
			bool _hasOnlyPlaceholder;
			return VesselSeats (vessel, true, out _hasOnlyPlaceholder);
		}

		List<InternalSeat> VesselSeats(Vessel vessel, bool withPlaceholder, out bool hasOnlyPlaceholder) {
			int _index = 0;
			hasOnlyPlaceholder = true;
			List<Part> _parts = vessel.parts;
			List<InternalSeat> _result = new List<InternalSeat> ();
			for (int _i = _parts.Count - 1; _i >= 0; --_i) {
				Part _part = _parts[_i];
				if (_part.internalModel != null) {
					if (_part.internalModel.internalName != "Placeholder" || withPlaceholder) {
						hasOnlyPlaceholder = false;
						List<InternalSeat> _seats = _part.internalModel.seats;
							for (int _j = _seats.Count - 1; _j >= 0; --_j) {
								InternalSeat _seat = _seats[_j];
								if (_seat.taken && _seat.kerbalRef != null) {
									Kerbal _kerbal = _seat.kerbalRef;
									if (_kerbal.state == Kerbal.States.ALIVE || _kerbal.state == Kerbal.States.NO_SIGNAL) {
									if (_part.partInfo.category == PartCategories.Pods) {
										_result.Insert (_index, _seat);
										_index++;
									}
									else {
										_result.Add (_seat);
									}
								}
							}
						}
					}
				}
			}
			return _result;
		}
			
		bool VesselHasSeatTaken(Vessel vessel) {
			bool _hasOnlyPlaceholder;
			Kerbal IVAKerbal;
			return VesselHasSeatTaken(vessel, out IVAKerbal, out _hasOnlyPlaceholder);
		}

		bool VesselHasSeatTaken(Vessel vessel, out Kerbal IVAKerbal, out bool hasOnlyPlaceholder) {
			hasOnlyPlaceholder = true;
			bool _result = false;
			Kerbal _first = null;
			Kerbal _firstPilot = null;
			List<InternalSeat> _seats = VesselSeats (vessel);
			for (int _i = _seats.Count - 1; _i >= 0; --_i) {
				Kerbal _kerbal = _seats[_i].kerbalRef;
				if (_first == null) {
					_first = _kerbal;
				}
				if (_firstPilot == null && _kerbal.protoCrewMember.experienceTrait.Title == "Pilot") {
					_firstPilot = _kerbal;
				}
				_result = true;
			}
			IVAKerbal = (_firstPilot != null ? _firstPilot : _first);
			return _result;
		}

		bool VesselHasCrewAlive (Vessel vessel) {
			Kerbal _first;
			return  VesselHasCrewAlive (vessel, out _first);
		}

		bool VesselHasCrewAlive (Vessel vessel, out Kerbal first) {
			bool _crewAreLoaded;
			return  VesselHasCrewAlive (vessel, out first, out _crewAreLoaded);
		}

		bool VesselHasCrewAlive (Vessel vessel, out Kerbal first, out bool crewAreLoaded) {
			crewAreLoaded = true;
			first = null;
			List<ProtoCrewMember> _crews = vessel.GetVesselCrew ();
			for (int _i = _crews.Count - 1; _i >= 0; --_i) {
				Kerbal _kerbal = _crews[_i].KerbalRef;
				if (_kerbal.state == Kerbal.States.ALIVE || _kerbal.state == Kerbal.States.NO_SIGNAL) {
					first = _kerbal;
					crewAreLoaded = true;
					return true;
				}
				if (_kerbal.state == Kerbal.States.NO_SIGNAL) {
					crewAreLoaded = false;
				}
			}
			return false;
		}

		bool VesselIsAlone (Vessel vessel) {
			List<Vessel> _vessels = FlightGlobals.Vessels;
			for (int _i = _vessels.Count - 1; _i >= 0; --_i) {
				Vessel _vessel = _vessels[_i];
				if (_vessel != vessel && _vessel.loaded) {
					return false;
				}
			}
			return true;
		}

		IEnumerator WaitToHideUI() {
			if (!QSettings.Instance.AutoHideUI) {
				Log ("Auto Hide UI is disabled", "QIVA");
				yield break;
			}
			yield return new WaitForEndOfFrame ();
			while (PauseMenu.isOpen || FlightDriver.Pause) {
				yield return 0;
			}
			GameEvents.onHideUI.Fire ();
			Log ("Auto Hide UI", "QIVA");
		}

		IEnumerator WaitToShowUI() {
			if (!QSettings.Instance.AutoHideUI) {
				Log ("Auto Hide UI is disabled", "QIVA");
				yield break;
			}
			yield return new WaitForEndOfFrame ();
			GameEvents.onShowUI.Fire ();
			Log ("Auto Show UI", "QIVA");
		}

		void DisableToggleUI(bool enable = true) {
			if (enable && QSettings.Instance.DisableShowUIonIVA) {
				GameSettings.TOGGLE_UI.switchState = InputBindingModes.None;
				GameSettings.TOGGLE_UI.switchStateSecondary = InputBindingModes.None;
				StartCoroutine(WaitToHideUI ());
				Log ("Disable Toggle UI shortcut", "QIVA");
			} else {
				GameSettings.TOGGLE_UI.switchState = InputBindingModes.Any;
				GameSettings.TOGGLE_UI.switchStateSecondary = InputBindingModes.Any;
				Log ("Enable Toggle UI shortcut", "QIVA");
			}
		}

		void DisableMapView(bool enable = true) {
			if (enable && QSettings.Instance.DisableMapView) {
				GameSettings.MAP_VIEW_TOGGLE.switchState = InputBindingModes.None;
				GameSettings.MAP_VIEW_TOGGLE.switchStateSecondary = InputBindingModes.None;
				Log ("Disable MapView shortcut", "QIVA");
			} else {
				GameSettings.MAP_VIEW_TOGGLE.switchState = InputBindingModes.Any;
				GameSettings.MAP_VIEW_TOGGLE.switchStateSecondary = InputBindingModes.Any;
				Log ("Enable MapView shortcut", "QIVA");
			}
		}
			
		void DisableThirdPersonVessel(bool enable = true) {
			if (enable && QSettings.Instance.DisableThirdPersonVessel) {
				GameSettings.CAMERA_MODE.switchState = InputBindingModes.None;
				GameSettings.CAMERA_MODE.switchStateSecondary = InputBindingModes.None;
				GameSettings.FOCUS_NEXT_VESSEL.switchState = InputBindingModes.None;
				GameSettings.FOCUS_NEXT_VESSEL.switchStateSecondary = InputBindingModes.None;
				GameSettings.FOCUS_PREV_VESSEL.switchState = InputBindingModes.None;
				GameSettings.FOCUS_PREV_VESSEL.switchStateSecondary = InputBindingModes.None;
				if (FlightGlobals.ActiveVessel.GetCrewCount () == 1) {
					GameSettings.CAMERA_NEXT.switchState = InputBindingModes.None;
					GameSettings.CAMERA_NEXT.switchStateSecondary = InputBindingModes.None;
					Log ("Disable Camera Next (only one seat taken)", "QIVA");
				}
				Log ("Disable Third Person Vessel View", "QIVA");
			} else {
				GameSettings.CAMERA_MODE.switchState = InputBindingModes.Any;
				GameSettings.CAMERA_MODE.switchStateSecondary = InputBindingModes.Any;
				GameSettings.FOCUS_NEXT_VESSEL.switchState = InputBindingModes.Any;
				GameSettings.FOCUS_NEXT_VESSEL.switchStateSecondary = InputBindingModes.Any;
				GameSettings.FOCUS_PREV_VESSEL.switchState = InputBindingModes.Any;
				GameSettings.FOCUS_PREV_VESSEL.switchStateSecondary = InputBindingModes.Any;
				GameSettings.CAMERA_NEXT.switchState = InputBindingModes.Any;
				GameSettings.CAMERA_NEXT.switchStateSecondary = InputBindingModes.Any;
				Log ("Enable Third Person Vessel View", "QIVA");
			}
		}

		void ScrMsg(bool simple, Kerbal kerbal) {
			if (ScreenMsg != null) {
				ScreenMsg.duration = 0;
			}
			if (simple) {
				ScreenMsg = ScreenMessages.PostScreenMessage (string.Format ("{0} ({1})", kerbal.crewMemberName, kerbal.protoCrewMember.experienceTrait.Title), 5, ScreenMessageStyle.LOWER_CENTER);
			} else {
				ScreenMsg = ScreenMessages.PostScreenMessage (string.Format ("{1} ({2}){0}({3}) {4}", Environment.NewLine, kerbal.crewMemberName, kerbal.protoCrewMember.experienceTrait.Title, kerbal.protoCrewMember.seat.part.partInfo.category, kerbal.protoCrewMember.seat.part.name), 5, ScreenMessageStyle.LOWER_CENTER);
			}
		}

		void GoRecovery(Vessel vessel) {
			if (vessel.IsRecoverable) {
				GameEvents.OnVesselRecoveryRequested.Fire (vessel);
			} else {
				ScreenMessages.PostScreenMessage ("[QuickIVA] This vessel is not recoverable.", 5, ScreenMessageStyle.UPPER_CENTER);
			}
		}

		void GoEVA(Vessel vessel) {
			if (CheckEVAUnlocked (vessel)) {
				if (!vessel.isEVA && !vessel.packed) {
					if (vessel.GetCrewCount () > 0) {
						Kerbal _first;
						if (VesselHasCrewAlive (vessel, out _first)) {
							Kerbal _kerbal = (isIVA() ? CheckIVAKerbal (vessel) : _first);
							FlightEVA.SpawnEVA (_kerbal);
							CameraManager.Instance.SetCameraFlight ();
							Log (string.Format ("GoEVA {0}({1}) experienceTrait: {2}", _kerbal.crewMemberName, _kerbal.protoCrewMember.seatIdx, _kerbal.protoCrewMember.experienceTrait.Title), "QIVA");
						} else {
							ScreenMessages.PostScreenMessage ("[QuickIVA] This vessel has no crew alive.", 5, ScreenMessageStyle.UPPER_CENTER);
						}
					} else {
						ScreenMessages.PostScreenMessage ("[QuickIVA] This vessel has no crew.", 5, ScreenMessageStyle.UPPER_CENTER);
					}
				} else {
					ScreenMessages.PostScreenMessage ("[QuickIVA] You can't EVA an EVA.", 5, ScreenMessageStyle.UPPER_CENTER);
				}
			} else {
				ScreenMessages.PostScreenMessage ("[QuickIVA] EVA is locked: " + GameVariables.Instance.GetEVALockedReason (vessel, vessel.GetVesselCrew()[0]), 5, ScreenMessageStyle.UPPER_CENTER);
			}
		}

		void GoIVA(Vessel vessel) {
			if (BlockedGoIVA || !HighLogic.CurrentGame.Parameters.Flight.CanIVA || !FlightGlobals.ready) {
				return;
			}
			if (isIVA ()) {
				Log ("We are in IVA!", "QIVA");
				return;
			}
			if (vessel.GetCrewCount () > 0) {
				if (VesselHasCrewAlive (vessel)) {
					bool _VesselhasOnlyPlaceholder;
					Kerbal _IVAKerbal;
					if (VesselHasSeatTaken (vessel, out _IVAKerbal, out _VesselhasOnlyPlaceholder)) {
						if (EVAKerbal != null) {
							if (vessel.GetVesselCrew ().Contains (EVAKerbal.protoCrewMember)) {
								_IVAKerbal = EVAKerbal;
							} else {
								EVAKerbal = null;
							}
						}
						if (_IVAKerbal != null) {
							if (KerbalIsOnVessel (vessel, _IVAKerbal)) {
								isGoneIVA = CameraManager.Instance.SetCameraIVA (_IVAKerbal, true);
								Log (string.Format ("Go IVA on {0}({1}) experienceTrait: {2}, partName: ({3}){4}", _IVAKerbal.crewMemberName, _IVAKerbal.protoCrewMember.seatIdx, _IVAKerbal.protoCrewMember.experienceTrait.Title, _IVAKerbal.protoCrewMember.seat.part.partInfo.category, _IVAKerbal.protoCrewMember.seat.part.name), "QIVA");
								ScrMsg (false, _IVAKerbal);
							} else {
								isGoneIVA = CameraManager.Instance.SetCameraIVA ();
								Log ("Go IVA (first Kerbal selected)", "QIVA");
							}
						} else {
							isGoneIVA = CameraManager.Instance.SetCameraIVA ();
						}
					} else if (_VesselhasOnlyPlaceholder) {
						NoMoreGoIVA = true;
						Warning ("Disable GoIVA, it seems that this vessel has no IVA!", "QIVA");
						return;
					}
				} else {
					Warning ("Can't GoIVA, this vessel has no crew alive!", "QIVA");
				}
			} else {
				NoMoreGoIVA = true;
				Warning ("Disable GoIVA, this vessel has no crew!", "QIVA");
				return;
			}
			if (isGoneIVA) {
				EVAKerbal = null;
				DisableThirdPersonVessel ();
				DisableMapView ();
				DisableToggleUI ();
			} else {
				DisableThirdPersonVessel (false);
				DisableMapView (false);
				DisableToggleUI (false);
				Warning ("Can't Go IVA now!", "QIVA");
			}
		}


		protected override void Awake() {
			if (!HighLogic.LoadedSceneIsFlight) {
				Warning ("It's not flight ? Destroy !", "QIVA");
				Destroy (this);
				return;
			}
			if (!QSettings.Instance.Enabled || Instance != null) {
				Warning ("Disabled or an instance already exists, destroy !", "QIVA");
				Destroy (this);
				return;
			}
			Instance = this;
			GameEvents.onLaunch.Add (OnLaunch);
			GameEvents.onCrewBoardVessel.Add (OnCrewBoardVessel);
			GameEvents.onCrewOnEva.Add (OnCrewOnEva);
			GameEvents.onFlightReady.Add (OnFlightReady);
			GameEvents.OnIVACameraKerbalChange.Add (OnIVACameraKerbalChange);
			GameEvents.onVesselSwitching.Add (OnVesselSwitching);
			GameEvents.onShowUI.Add (OnShowUI);
			GameEvents.OnCameraChange.Add (OnCameraChange);
			GameEvents.OnMapEntered.Add (OnMapEntered);
			GameEvents.OnMapExited.Add (OnMapExited);
			Log ("Awake", "QIVA");
		}

		protected override void Start() {
			Log ("Start", "QIVA");
		}

		protected override void OnDestroy() {
			DisableThirdPersonVessel (false);
			DisableMapView (false);
			DisableToggleUI (false);
			GameEvents.onLaunch.Remove (OnLaunch);
			GameEvents.onCrewBoardVessel.Remove (OnCrewBoardVessel);
			GameEvents.onCrewOnEva.Remove (OnCrewOnEva);
			GameEvents.onFlightReady.Remove (OnFlightReady);
			GameEvents.OnIVACameraKerbalChange.Remove (OnIVACameraKerbalChange);
			GameEvents.onVesselSwitching.Remove (OnVesselSwitching);
			GameEvents.onShowUI.Remove (OnShowUI);
			GameEvents.OnCameraChange.Remove (OnCameraChange);
			GameEvents.OnMapEntered.Remove (OnMapEntered);
			GameEvents.OnMapExited.Remove (OnMapExited);
			Log ("OnDestroy", "QIVA");
		}

		// Disable Third Person Vessel and MapView
		void OnFlightReady() {
			/*if (!QSettings.Instance.IVAatLaunch) {
				StartCoroutine (WaitCrew ());
			}*/
			if (isGoneIVA) {
				StartCoroutine (WaitToHideUI ());
			}
			Log ("OnFlightReady", "QIVA");
		}

		// Wait to Enter to IVA
		IEnumerator WaitCrew() {
			if (BlockedGoIVA) {
				Log ("Are you in IVA?", "QIVA");
				yield break;
			}
			while (!FlightGlobals.ready) {
				yield return 0;
			}
			while (FlightGlobals.ActiveVessel == null) {
				yield return 0;
			}
			Vessel _vessel = FlightGlobals.ActiveVessel;
			Kerbal _first;
			bool _crewAreLoaded = false;
			while (!VesselHasCrewAlive(_vessel, out _first, out _crewAreLoaded) || !_crewAreLoaded) {
				yield return new WaitForSeconds (0.5f);
				yield return 0;
			}
			GoIVA (FlightGlobals.ActiveVessel);
			Log ("WaitCrew", "QIVA");
		}

		// Auto Enter to the IVA
		void OnVesselSwitching(Vessel oldVessel, Vessel newVessel) {
			if (newVessel != null) {
				BlockedGoIVA = false;
				if (QSettings.Instance.IVAatLaunch && newVessel.situation == Vessel.Situations.PRELAUNCH) {
					return;
				}
				StartCoroutine (WaitCrew ());
			}
			Log ("OnVesselSwitching", "QIVA");
		}

		// Go IVA at Launch
		void OnLaunch(EventReport eventReport) {
			if (QSettings.Instance.IVAatLaunch) {
				GoIVA (FlightGlobals.ActiveVessel);
			}
			Log ("OnLaunch", "QIVA");
		}

		// Which kerbal goes to EVA
		void OnCrewOnEva(GameEvents.FromToAction<Part, Part> part) {
			ScrMsg (true, part.to.protoModuleCrew[0].KerbalRef);
			DisableToggleUI (false);
			DisableThirdPersonVessel (false);
			Log ("OnCrewOnEva", "QIVA");
		}

		// Select the good kerbal on IVA after an EVA
		void OnCrewBoardVessel(GameEvents.FromToAction<Part, Part> part) {
			ProtoCrewMember _pcrew = part.to.protoModuleCrew.Find (item => item.KerbalRef.crewMemberName == part.from.vessel.vesselName);
			if (_pcrew != null) {
				EVAKerbal = _pcrew.KerbalRef;
			}
			Log ("OnCrewBoardVessel", "QIVA");
		}

		// AutoHide UI and Kerbal Info
		void OnIVACameraKerbalChange(Kerbal kerbal) {
			if (kerbal != null) {
				Log (string.Format ("IVA switch to {0}({1}) experienceTrait: {2}, partName: ({3}){4}", kerbal.crewMemberName, kerbal.protoCrewMember.seatIdx, kerbal.protoCrewMember.experienceTrait.Title, kerbal.protoCrewMember.seat.part.partInfo.category, kerbal.protoCrewMember.seat.part.name));
				ScrMsg (false, kerbal);
				StartCoroutine(WaitToHideUI ());
			} else {
				Log ("Can't find the current Kerbal", "QIVA");
			}
		}

		void OnCameraChange(CameraManager.CameraMode cameraMode) {
			if (QSettings.Instance.AutoHideUI) {
				if (isIVA ()) {
					StartCoroutine(WaitToHideUI ());
				}
				if (isMAP) {
					StartCoroutine(WaitToShowUI ());
				}
			}
			Log ("OnCameraChange", "QIVA");
		}

		// Disable UI
		void OnShowUI() {
			if (QSettings.Instance.DisableShowUIonIVA) {
				if (isIVA ()) {
					StartCoroutine(WaitToHideUI ());
				}
			}
			Log ("OnShowUI", "QIVA");
		}

		//Unlock Map
		void OnMapEntered() {
			DisableThirdPersonVessel (false);
			DisableToggleUI (false);
		}

		void OnMapExited() {
			DisableThirdPersonVessel ();
			DisableToggleUI ();
		}

		// Keyboard shortcuts
		void FixedUpdate() {
			if (HighLogic.LoadedSceneIsFlight) {
				if (FlightGlobals.ready && !FlightDriver.Pause) {
					Vessel _vessel = FlightGlobals.ActiveVessel;
					if (_vessel != null) {
						if (GameSettings.FOCUS_NEXT_VESSEL.GetKeyDown (true) || GameSettings.FOCUS_PREV_VESSEL.GetKeyDown (true)) {
							if (!VesselIsAlone (_vessel)) {
								DisableThirdPersonVessel (false);
							}
						}
					}
				}
			}
		}

		void Update() {
			if (QSettings.Instance.KeyEnabled) {
				if (HighLogic.LoadedSceneIsFlight) {
					if (FlightGlobals.ready && !FlightDriver.Pause) {
						if (Input.GetKeyDown (QSettings.Instance.KeyRecovery)) {
							GoRecovery (FlightGlobals.ActiveVessel);
						}
						if (Input.GetKeyDown (QSettings.Instance.KeyEVA)) {
							GoEVA (FlightGlobals.ActiveVessel);
						}
					}
				}
			}
		}
	}
}