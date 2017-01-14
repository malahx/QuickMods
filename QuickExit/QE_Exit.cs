/* 
QuickExit
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

using System.Collections;
using System.IO;
using UnityEngine;

namespace QuickExit {
	public partial class QExit {

		public static QExit Instance;

		[KSPField(isPersistant = true)] internal static QBlizzyToolbar BlizzyToolbar;

		public static readonly string shipFilename = "Auto-Saved Ship";

		int count = 5;

		Coroutine coroutineTryExit;
		bool IsTryExit {
			get {
				return coroutineTryExit != null;
			}
			set {
				if (coroutineTryExit != null) {
					StopCoroutine (coroutineTryExit);					
					coroutineTryExit = null;
				}
				saveDone = false;
				if (value) {
					if (HighLogic.LoadedSceneIsFlight) {
						if (PauseMenu.isOpen) {
							PauseMenu.Close ();
						}
						if (!FlightDriver.Pause) {
							FlightDriver.SetPause (true);
						}
					}
					count = (QSettings.Instance.CountDown ? 5 : 0);
					coroutineTryExit = StartCoroutine (tryExit ());
				}
				else {
					if (HighLogic.LoadedSceneIsFlight) {
						if (FlightDriver.Pause) {
							FlightDriver.SetPause (false);
						}
					}
				}
			}
		}

		bool needToSavegame {
			get {
				return QSettings.Instance.AutomaticSave && HighLogic.LoadedSceneIsGame;
			}
		}

		bool saveDone = false;
		bool CanSavegame {
			get {
				if (!HighLogic.LoadedSceneIsGame) {
					return false;
				}
				string _savegame = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs";
				if (File.Exists (_savegame)) {
					FileInfo _info = new FileInfo (_savegame);
					if (_info.IsReadOnly) {
						Warning (_savegame + " is read only.", "QExit");
						return false;
					}
				}
				if (HighLogic.LoadedSceneIsFlight) {
					if (FlightGlobals.ready) {
						if (FlightGlobals.ActiveVessel.state != Vessel.State.DEAD) {
							if (FlightGlobals.ActiveVessel.IsClearToSave () != ClearToSaveStatus.CLEAR) {
								return false;
							}
							ClearToSaveStatus clearToSaveStatus = FlightGlobals.ClearToSave ();
							if (clearToSaveStatus != ClearToSaveStatus.CLEAR) {
								if (clearToSaveStatus != ClearToSaveStatus.NOT_WHILE_ON_A_LADDER) {
									if (clearToSaveStatus != ClearToSaveStatus.NOT_WHILE_MOVING_OVER_SURFACE) {
										return true;
									}
								}
								return false;
							}
						}
					}
				}
				return true;
			}
		}

		protected override void Awake() {
			if (Instance != null) {
				Warning ("There's already an Instance of " + MOD + ". Destroy.", "QGUI");
				Destroy (this);
				return;
			}
			Instance = this;
			if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzyToolbar ();
			Log ("Awake", "QExit");
		}

		protected override void Start() {
			if (BlizzyToolbar != null) BlizzyToolbar.Init ();
			labelStyle = new GUIStyle ();
			labelStyle.stretchWidth = true;
			labelStyle.stretchHeight = true;
			labelStyle.alignment = TextAnchor.MiddleCenter;
			labelStyle.fontSize = (Screen.height/20);
			labelStyle.fontStyle = FontStyle.Bold;
			labelStyle.normal.textColor = Color.red;
			Log ("Start", "QExit");
		}

		void Update() {
			if (Input.GetKeyDown (QSettings.Instance.Key)) {
				if (IsTryExit) {
					TryExit ();
				} else {
					if (GameSettings.MODIFIER_KEY.GetKey ()) {
						TryExit ();
					} else {
						Dialog ();
					}
				}
			}
		}
			
		protected override void OnDestroy() {
			if (BlizzyToolbar != null) BlizzyToolbar.Destroy ();
			Log ("OnDestroy", "QExit");
		}

		IEnumerator tryExit() {
			if (needToSavegame) {
				if (CanSavegame) {
					if (GamePersistence.SaveGame ("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE) != string.Empty) {
						saveDone = true;
						ScreenMessages.PostScreenMessage (string.Format ("[{0}] {1}.", MOD, QLang.translate ("Game saved")), 5);
						Log ("Game saved.", "QExit");
					} else {
						count = 10;
						Log ("Can't save game.", "QExit");
						ScreenMessages.PostScreenMessage (string.Format ("[{0}] {1}.", MOD, QLang.translate ("Can't save game")), 10);
					}
					if (HighLogic.LoadedSceneIsEditor) {
						ShipConstruction.SaveShip (shipFilename);
						Log ("Ship saved.", "QExit");
						ScreenMessages.PostScreenMessage (string.Format ("[{0}] {1}.", MOD, QLang.translate ("Ship saved")), 5);
					}
				} else {
					count = 10;
					ClearToSaveStatus clearToSaveStatus = FlightGlobals.ClearToSave ();
					string _status = FlightGlobals.GetNotClearToSaveStatusReason (clearToSaveStatus, string.Empty);
					Log ("Can't game saved: " + _status, "QExit");
					ScreenMessages.PostScreenMessage (string.Format ("[{0}] {1}: {2}", MOD, QLang.translate ("Can't save game"), _status.ToString ()), 10);
				}
			}
			while (count >= 0) {
				yield return new WaitForSecondsRealtime (1f);
				Log ("Exit in " + count, "QExit");
				count--;
			}
			if (!IsTryExit) {
				Log ("tryExit stopped", "QExit");
				yield break;
			}
			Application.Quit ();
			Log ("tryExit ended", "QExit");
		}

		public void TryExit(bool force = false) {
			if (!IsTryExit || force) {
				IsTryExit = true;
			} else {
				IsTryExit = false;
			}
			Log ("TryExit: " + IsTryExit + " force: " + force, "QExit");
		}
	}
}