/* 
QuickEngineer
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

using KerbalEngineer.VesselSimulator;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuickEngineer {

	public partial class QEditor {

		public static QEditor Instance;

		internal Coroutine calculation = null;

		private bool settingsIsLive = false;
		private bool atmDeltaVIsLive = false;
		private GUISkin skin;
		private static Vector2 scrollPosition = new Vector2 ();
		private static Vector2 scrollAtmPosition = new Vector2 ();

		private Vector2 settingsDim = new Vector2 (900, 50);
		private Rect settingsRect = new Rect();
		internal Rect SettingsRect {
			get {
				Vector2 _screen = new Vector2(Screen.width, Screen.height);
				return new Rect((_screen.x - settingsRect.width) / 2, (_screen.y - settingsRect.height) / 2, settingsDim.x, settingsRect.height);
			}
			set {
				settingsRect = value;
			}
		}

		private Vector2 atmDeltaVDim = new Vector2 (200, 50);
		private Rect atmDeltaVRect = new Rect();
		internal Rect AtmDeltaVRect {
			get {
				Vector2 _screen = new Vector2(Screen.width, Screen.height);
				return new Rect((_screen.x - atmDeltaVRect.width) / 2, (_screen.y - atmDeltaVRect.height) / 2, atmDeltaVDim.x, atmDeltaVRect.height);
			}
			set {
				atmDeltaVRect = value;
			}
		}

		private GenericCascadingList gCascadingList;
		private GenericCascadingList GCascadingList {
			get {
				if (gCascadingList == null) {
					gCascadingList = EngineersReport.FindObjectOfType<GenericCascadingList> ();
				}
				return gCascadingList;
			}
		}

		private RUICascadingList.CascadingListItem VesselEngineer;

		private int defaultHeightApp = 0;
		private int DefaultHeightApp {
			get {
				if (defaultHeightApp == 0) {
					defaultHeightApp = GAppFrame.minHeight;
				}
				return defaultHeightApp;
			}
		}

		private int addHeightApp {
			get {
				int _i = 38;
				_i += (VesselEngineer.items.Count -1) * ((!QSettings.Instance.VesselEngineer_hidedeltaV || !QSettings.Instance.EditorVesselEngineer_Simple ? 14 : 0) + (!QSettings.Instance.VesselEngineer_hideTWR || !QSettings.Instance.EditorVesselEngineer_Simple ? 13 : 0));
				return _i;
			}
		}

		private GenericAppFrame gAppFrame;
		private GenericAppFrame GAppFrame {
			get {
				if (gAppFrame == null) {
					gAppFrame = EngineersReport.FindObjectOfType<GenericAppFrame> ();
				}
				return gAppFrame;
			}
		}

		private bool notPinnedShipModified = true;

		private bool EngineerIsAvailable {
			get {
				return EngineersReport.Ready && EngineersReport.Instance != null;
			}
		}

		private bool EngineerIsPinned {
			get {
				if (EditorIsActive) {
					if (EngineerIsAvailable) {
						if (EngineersReport.Instance.appLauncherButton != null) {
							if (EngineersReport.Instance.appLauncherButton.State == RUIToggleButton.ButtonState.TRUE) {
								return true;
							}
							if (GAppFrame != null) {
								if (GAppFrame.gameObject != null) {
									if (GAppFrame.gameObject.activeSelf) {
										return true;
									}
								}
							}
						}
					}
				}
				return false;
			}
		}

		internal static bool EditorIsActive {
			get {
				return HighLogic.LoadedSceneIsEditor && EditorLogic.RootPart != null && EditorLogic.SortedShipList.Count > 0;
			}
		}

		private List<UIListItemContainer> CreateVesselEngineerBody(bool calculation = false) {
			List<UIListItemContainer> _items = new List<UIListItemContainer> ();
			UIListItemContainer _item;
			if (calculation) {
				_item = GCascadingList.CreateBody (textColor("Calculation..."), string.Empty);
			} else {
				List<QStage> _qStages = null;
				Stage[] _stages = null;
				Stage _lastStage = null;
				int _length;
				bool _simple = QSettings.Instance.EditorVesselEngineer_Simple;
				if (_simple) {
					_stages = QVessel.Stages;
					_lastStage = QVessel.LastStage;
					_length = _stages.Length;
				} else {
					_qStages = QStage.QStages;
					_length = QStage.stagesCount;
				}
				if ((!_simple || _stages != null) && _length > 0) {
					for (int i = 0; i < _length; i++) {
						_item = null;
						if (_simple) {	
							Stage _stage = _stages [i];
							_item = CreateBodyStage (i, _stage);
						} else {
							QStage _qStage = _qStages [i];
							_item = CreateBodyStage (i, _qStage);
						}
						if (_item != null) {
							_items.Add (_item);
						}
					}
				}
				if (_items.Count > 0) {
					_item = GCascadingList.CreateBody ("Total deltaV:", (_simple ? textTotalDeltaV (_lastStage) : textTotalDeltaV (QStage.TotalDeltaV)));
				} else {
					_item = GCascadingList.CreateBody (textColor("Click here to config"), string.Empty);
				}
			}
			_item.AddInputDelegate (new EZInputDelegate (SetConfig));
			_items.Add (_item);
			Log ("VesselEngineer body created", "QEditor");
			return _items;
		}

		private UIListItemContainer CreateBodyStage(int stageIndex, QStage qStage) {
			if (QSettings.Instance.VesselEngineer_hideEmptyStages && (qStage.deltaV == 0 || qStage.maxThrustToWeight == 0)) {
				return null;
			}
			UIListItemContainer _item = GCascadingList.CreateBody (textEditorStage (stageIndex, qStage.Body, qStage.Atmosphere), textEngineer (stageIndex, qStage), true);
			_item.AddInputDelegate (new EZInputDelegate (SetConfig));
			Log ("Create body Stage: " + stageIndex, "QEditor");
			return _item;
		}

		private UIListItemContainer CreateBodyStage(int stageIndex, Stage stage, CelestialBody body = null, bool atmosphere = false) {
			if (QSettings.Instance.VesselEngineer_hideEmptyStages && (stage.deltaV == 0 || stage.maxThrustToWeight == 0)) {
				return null;
			}
			if (body == null) {
				body = QTools.Home;
			}
			UIListItemContainer _item = GCascadingList.CreateBody (textEditorStage (stageIndex, body, atmosphere), textEngineer (stageIndex, stage), true);
			_item.AddInputDelegate (new EZInputDelegate (SetConfig));
			Log ("Create body Stage: " + stageIndex, "QEditor");
			return _item;
		}

		private void OnEditorShipModified(ShipConstruct ship = null) {
			if (!EngineerIsPinned) {
				notPinnedShipModified = true;
				Log ("notPinned but ShipModified", "QEditor");
				return;
			}
			if (QSettings.Instance.AllVesselEngineer_Disable) {
				Log ("VesselEngineer is disabled", "QEditor");
				return;
			}
			if (EditorIsActive) {
				if (QStage.stagesCount <= 0) {
					Log ("No stage inited ... waiting.", "QEditor");
					return;
				}
				notPinnedShipModified = false;
				Log ("Start a simulation", "QEditor");
				if (QSettings.Instance.EditorVesselEngineer_Simple) {
					QVessel.Init (new QStage());
					QVessel.StartSim ();
				} else {
					if (calculation != null) {
						QVessel.calculationToRestart = true;
						Log ("Restart the calculations", "QEditor");
					} else {
						calculation = StartCoroutine (QVessel.calcWithATM ());
					}
				}
			} else {
				UpdateEngineer ("Vessel");
			}
			Log ("OnEditorShipModified", "QEditor");
		}

		private void EngineersReportReady() {
			UpdateEngineer ("Vessel");
		}

		internal void UpdateEngineer(string title, bool calculation = false) {
			BTButton button;
			UIListItemContainer header = GCascadingList.CreateHeader (textTitle(title), out button, true);
			UIListItemContainer footer = GCascadingList.CreateFooter ();
			List<UIListItemContainer> bodies = CreateVesselEngineerBody (calculation);
			if (VesselEngineer != null) {
				VesselEngineer = GCascadingList.ruiList.UpdateCascadingItem (VesselEngineer, header, footer, bodies, button);
			} else {
				VesselEngineer = GCascadingList.ruiList.AddCascadingItem (header, footer, bodies, button);
			}
			Log (title + "Engineer Updated", "QEditor");
		}

		private void SetConfig(ref POINTER_INFO ptr) {
			if (ptr.evt == POINTER_INFO.INPUT_EVENT.PRESS) {
				DisplayApp ();
			}
		}

		private void DisplayApp () {
			if (settingsIsLive) {
				Log ("Settings is already opened", "QEditor");
				return;
			}
			Lock (true);
			RenderingManager.AddToPostDrawQueue(3, new Callback(Draw));
			settingsIsLive = true;
			Log ("DisplayApp", "QEditor");
		}

		private void HideApp () {
			if (!settingsIsLive) {
				Log ("Settings is already closed", "QEditor");
				return;
			}
			Lock (false);
			RenderingManager.RemoveFromPostDrawQueue(3, new Callback(Draw));
			settingsIsLive = false;
			notPinnedShipModified = true;
			Log ("HideApp", "QEditor");
		}

		private void DisplayAtmDeltaV () {
			if (atmDeltaVIsLive) {
				Log ("AtmDeltaV is already opened", "QEditor");
				return;
			}
			atmDeltaVIsLive = true;
			Log ("DisplayAtmDeltaV", "QEditor");
		}

		private void HideAtmDeltaV () {
			if (!atmDeltaVIsLive) {
				Log ("AtmDeltaV is already closed", "QEditor");
				return;
			}
			atmDeltaVIsLive = false;
			Log ("HideAtmDeltaV", "QEditor");
		}


		private void Draw() {
			GUI.skin = skin;
			if (atmDeltaVIsLive) {
				AtmDeltaVRect = GUILayout.Window (4512346, AtmDeltaVRect, MainAtmDeltaV, "Atmospheric DeltaV");
				return;
			}
			if (settingsIsLive) {
				SettingsRect = GUILayout.Window (4512345, SettingsRect, MainSettings, string.Format ("{0} v{1} - VesselEngineer", MOD, VERSION));
			}
		}

		private void MainSettings(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Box ("Options", GUILayout.Height(25));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal();
			QSettings.Instance.AllVesselEngineer_Disable = GUILayout.Toggle (QSettings.Instance.AllVesselEngineer_Disable, "Always disable", GUILayout.Width (290));
			if (!QSettings.Instance.AllVesselEngineer_Disable) {
				GUILayout.FlexibleSpace ();
				QSettings.Instance.FlightVesselEngineer_Disable = GUILayout.Toggle (QSettings.Instance.FlightVesselEngineer_Disable, "Disable in Flight", GUILayout.Width (290));
				GUILayout.FlexibleSpace ();
				QSettings.Instance.EditorVesselEngineer_Simple = GUILayout.Toggle (QSettings.Instance.EditorVesselEngineer_Simple, "Editor calculation only on VAC", GUILayout.Width (290));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QSettings.Instance.VesselEngineer_hidedeltaV = GUILayout.Toggle (QSettings.Instance.VesselEngineer_hidedeltaV, "Hide deltaV", GUILayout.Width (290));
				GUILayout.FlexibleSpace ();
				QSettings.Instance.VesselEngineer_hideTWR = GUILayout.Toggle (QSettings.Instance.VesselEngineer_hideTWR, "Hide TWR", GUILayout.Width (290));
				GUILayout.FlexibleSpace ();
				QSettings.Instance.VesselEngineer_hideEmptyStages = GUILayout.Toggle (QSettings.Instance.VesselEngineer_hideEmptyStages, "Hide stage with no deltaV/TWR", GUILayout.Width (290));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QSettings.Instance.VesselEngineer_showEmptyTWR = GUILayout.Toggle (QSettings.Instance.VesselEngineer_showEmptyTWR, "Show max TWR", GUILayout.Width (290));
				GUILayout.FlexibleSpace ();
				QSettings.Instance.VesselEngineer_showStageTotaldV = GUILayout.Toggle (QSettings.Instance.VesselEngineer_showStageTotaldV, "Show Total deltaV on stage", GUILayout.Width (290));
				if (QSettings.Instance.VesselEngineer_showStageTotaldV && QSettings.Instance.VesselEngineer_showStageTotaldV == QSettings.Instance.VesselEngineer_showStageInverseTotaldV) {
					QSettings.Instance.VesselEngineer_showStageInverseTotaldV = false;
				}
				GUILayout.FlexibleSpace ();
				QSettings.Instance.VesselEngineer_showStageInverseTotaldV = GUILayout.Toggle (QSettings.Instance.VesselEngineer_showStageInverseTotaldV, "Show inverse Total deltaV on stage", GUILayout.Width (290));
				if (QSettings.Instance.VesselEngineer_showStageInverseTotaldV && QSettings.Instance.VesselEngineer_showStageTotaldV == QSettings.Instance.VesselEngineer_showStageInverseTotaldV) {
					QSettings.Instance.VesselEngineer_showStageTotaldV = false;
				}
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QSettings.Instance.Debug = GUILayout.Toggle (QSettings.Instance.Debug, "Show debug logs", GUILayout.Width (290));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				if (!QSettings.Instance.EditorVesselEngineer_Simple) {
					GUILayout.BeginHorizontal ();
					GUILayout.Box ("Stages parameters", GUILayout.Height (25));
					GUILayout.EndHorizontal ();
					GUILayout.BeginHorizontal ();
					GUILayout.Space (20);
					GUILayout.Label ("<b>Stages</b>", GUILayout.Width (50));
					GUILayout.Space (20);
					GUILayout.Label ("<b>CelestialBody</b>", GUILayout.Width (120));
					GUILayout.Space (40);
					GUILayout.Label ("<b>Atmospheric</b>", GUILayout.Width (100));
					GUILayout.Space (30);
					GUILayout.Label ("<b>Corrected dV</b>", GUILayout.Width (120));
					GUILayout.Space (30);
					GUILayout.Label ("<b>Altitude</b>", GUILayout.Width (150));
					GUILayout.Space (20);
					GUILayout.Label ("<b>Mach</b>", GUILayout.Width (150));
					GUILayout.EndHorizontal ();
					GUILayout.BeginHorizontal ();
					scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.Height (200));
					List<QStage> _qStages = QStage.QStages;
					int _stageCount = Math.Min (QStage.stagesCount, _qStages.Count);
					for (int i = 0; i < _stageCount; i++) {
						QStage _qStage = _qStages [i];
						if ((_qStage.Atmosphere && _qStage.atmStage == null) && (!_qStage.Atmosphere && _qStage.vacStage == null)) {
							continue;
						}
						if (QSettings.Instance.VesselEngineer_hideEmptyStages && (_qStage.deltaV == 0 || _qStage.maxThrustToWeight == 0)) {
							continue;
						}
						GUILayout.BeginHorizontal ();
						GUILayout.Label (string.Format ("<b>Stage {0}:</b>", i), GUILayout.Width (50));
						GUILayout.Space (20);
						if (GUILayout.Button ("◄", GUILayout.Width (20))) {
							int _index = QBody.Bodies.FindIndex (b => b == _qStage.Body) - 1;
							if (_index < 0) {
								_index = QBody.Bodies.Count - 1;
							}
							_qStage.Body = QBody.Bodies [_index];
						}
						GUILayout.Label (_qStage.Body.bodyName, GUILayout.Width (90), GUILayout.Height (20));
						if (GUILayout.Button ("►", GUILayout.Width (20))) {
							int _index = QBody.Bodies.FindIndex (b => b == _qStage.Body) + 1;
							if (_index >= QBody.Bodies.Count) {
								_index = 0;
							}
							_qStage.Body = QBody.Bodies [_index];
						}
						GUILayout.Space (20);
						GUI.enabled = _qStage.Body.atmosphere;
						if (!_qStage.Body.atmosphere) {
							_qStage.Atmosphere = false;
							_qStage.atmCorrected = false;
							_qStage.Altitude = 0f;
							_qStage.Mach = 0d;
						}
						_qStage.Atmosphere = GUILayout.Toggle (_qStage.Atmosphere, "Atmospheric", GUILayout.Width (100));
						GUILayout.Space (40);
						bool _canCorrecteddV = _qStage.Body.atmosphere && QBody.hasAtmValue (_qStage.Body);
						GUI.enabled = _canCorrecteddV;
						if (!_canCorrecteddV) {
							_qStage.atmCorrected = false;
						}
						_qStage.atmCorrected = GUILayout.Toggle (_qStage.atmCorrected, "Corrected dV", GUILayout.Width (120));
						GUILayout.Space (40);
						GUI.enabled = _qStage.Body.atmosphere;
						_qStage.Altitude = GUILayout.HorizontalSlider (_qStage.Altitude, 0f, (float)_qStage.Body.atmosphereDepth, GUILayout.Width (100), GUILayout.Height (30));
						GUILayout.Label (string.Format ("{0:0.0}km", (_qStage.Altitude / 1000)), GUILayout.Width (50));
						GUILayout.Space (20);
						float _maxMach = _qStage.maxMach;
						_qStage.Mach = (double)GUILayout.HorizontalSlider (Mathf.Clamp ((float)_qStage.Mach, 0f, _maxMach), 0f, _maxMach, GUILayout.Width (100), GUILayout.Height (30));
						GUILayout.Label (string.Format ("{0:0.00}", _qStage.Mach), GUILayout.Width (40));
						GUI.enabled = true;
						GUILayout.EndHorizontal ();
					}
					GUILayout.EndScrollView ();
					GUILayout.EndHorizontal ();
				}
			}
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add/Set Atmospheric value for celestial bodies")) {
				DisplayAtmDeltaV ();
			}
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Close")) {
				HideApp ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}

		private void MainAtmDeltaV(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Space (10);
			GUILayout.Label ("<b>Bodies</b>", GUILayout.Width (45));
			GUILayout.Space (20);
			GUILayout.Label ("<b>ATM deltaV</b>", GUILayout.Width (90));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal();
			scrollAtmPosition = GUILayout.BeginScrollView (scrollAtmPosition, GUILayout.Height (200));
			int _bodiesCount = QBody.Bodies.Count;
			for (int i = 0; i < _bodiesCount; i++) {
				CelestialBody _body = QBody.Bodies [i];
				if (_body.atmosphere) {
					GUILayout.BeginHorizontal ();
					GUILayout.Label (_body.bodyName + ":", GUILayout.Width (50));
					int _deltaV = 0;
					if (int.TryParse (GUILayout.TextField (QBody.atmDeltaV (_body).ToString (), GUILayout.Width (50)), out _deltaV)) {
						if (_deltaV != QBody.atmDeltaV (_body)) {
							QBody.Set (_body, _deltaV);
						}
					}
					GUILayout.Label ("m/s");
					GUILayout.EndHorizontal ();
				}
			}
			GUILayout.EndScrollView ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Cancel")) {
				HideAtmDeltaV ();
				QBody.Load ();
			}
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Save")) {
				HideAtmDeltaV ();
				QBody.Save ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}


		protected override void Awake() {
			if (Instance != null) {
				Warning ("There's already an Instance of " + MOD, "QEditor");
				Destroy (this);
				return;
			}
			Instance = this;
			Log ("Awake", "QEditor");
		}

		protected override void Start() {
			QSettings.Instance.Load ();
			QBody.Load ();
			if (!HighLogic.LoadedSceneIsEditor) {
				Warning ("It's not Editor scene?", "QEditor");
				Destroy (this);
				return;
			}
			skin = HighLogic.Skin;
			skin.label.alignment = TextAnchor.MiddleCenter;
			GameEvents.onEditorShipModified.Add (OnEditorShipModified);
			GameEvents.onGUIEngineersReportReady.Add (EngineersReportReady);
			Log ("Start", "QEditor");
		}

		private void Update() {
			if (HighLogic.LoadedSceneIsEditor && QStockToolbar.isAvailable) {
				if (!QSettings.Instance.AllVesselEngineer_Disable) {
					if (EngineerIsPinned && notPinnedShipModified) {
						Log ("Update() -> OnEditorShipModified", "QEditor");
						OnEditorShipModified ();
					}
				}
				if (EngineerIsAvailable) {
					if (GAppFrame != null) {
						if (GAppFrame.minHeight != DefaultHeightApp + addHeightApp) {
							GAppFrame.minHeight = DefaultHeightApp + addHeightApp;
							GAppFrame.gfxBg.height = GAppFrame.minHeight;
							GAppFrame.UpdateDraggingBounds (GAppFrame.minHeight, -GAppFrame.minHeight);
							GAppFrame.Reposition ();
							Log ("GAppFrame Updated", "QEditor");
						}
					}
				}
			}
		}
			
		protected override void OnDestroy() {
			GameEvents.onEditorShipModified.Remove (OnEditorShipModified);
			GameEvents.onGUIEngineersReportReady.Remove (EngineersReportReady);
			Log ("OnDestroy", "QEditor");
		}
	}
}