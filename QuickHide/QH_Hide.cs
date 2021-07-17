/* 
QuickHide
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

using UnityEngine.UI;
using KSP.UI.Screens;
using KSPAssets.KSPedia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using KSP.Localization;

namespace QuickHide {
	public partial class QHide {

		[KSPField (isPersistant = true)] internal static List<QMods> ModsToolbar = new List<QMods> ();
		DateTime dateAppLauncher = DateTime.Now;
		DateTime dateStage =  DateTime.Now;
		bool First = false;

		// For FAR compatibility
		bool EditorhasRootPart {
			get {
				if (!HighLogic.LoadedSceneIsEditor) {
					return true;
				}
				return EditorLogic.RootPart != null;
			}
		}

		Rect StockToolBar_Position {
			get {
				float _scale = 40 * GameSettings.UI_SCALE_APPS * GameSettings.UI_SCALE;
					if (ApplicationLauncher.Instance.IsPositionedAtTop) {
						return new Rect (Screen.width - _scale, 0, _scale, Screen.height);
					}
					else {
						return new Rect (Screen.width / 2, Screen.height - _scale, Screen.width / 2, _scale);
					}
			}
		}

		Rect Stage_Position {
			get {
				float _width = 75 * GameSettings.UI_SCALE_STAGINGSTACK * GameSettings.UI_SCALE;
				return new Rect ((HighLogic.LoadedSceneIsEditor ? Screen.width - _width : 0), 50, _width, Screen.height - 50);
			}
		}

		bool isPinned {
			get {
				if (WindowSettings) {
					return true;
				} 
				KSPediaApp _kSPediaApp = (KSPediaApp)KSPediaApp.FindObjectOfType (typeof (KSPediaApp));
				if (_kSPediaApp != null) {
					if (_kSPediaApp.appLauncherButton != null) {
						if (_kSPediaApp.appLauncherButton.IsHovering || _kSPediaApp.appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True) {
							return true;
						}
					}
				}
				if (MessageSystem.Instance) {
					if (MessageSystem.Instance.appLauncherButton != null) {
						if (MessageSystem.Instance.appLauncherButton.IsHovering || MessageSystem.Instance.appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True) {
							return true;
						}
					}
				}
				for (int _i = ModsToolbar.Count - 1; _i >= 0; --_i) {
					QMods _qMods = ModsToolbar[_i];
					if (_qMods == null) {
						continue;
					}
					if (!_qMods.CanBePin || !_qMods.isActive) {
						continue;
					}
					if (_qMods.isTrue) {
						return true;
					}
				}
				if (HighLogic.LoadedSceneIsFlight) {
					if (ResourceDisplay.Instance != null) {
						if (ResourceDisplay.Instance.appLauncherButton != null) {
							if (ResourceDisplay.Instance.appLauncherButton.IsHovering || ResourceDisplay.Instance.appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True) {
								return true;
							}
						}
					}
					if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX) {
						CurrencyWidgetsApp _currencyWidgetsApp = (CurrencyWidgetsApp)CurrencyWidgetsApp.FindObjectOfType (typeof (CurrencyWidgetsApp));
						if (_currencyWidgetsApp != null) {
							if (_currencyWidgetsApp.appLauncherButton != null) {
								if (_currencyWidgetsApp.appLauncherButton.IsHovering || _currencyWidgetsApp.appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True) {
									return true;
								}
							}
						}
					}
				}
				if (HighLogic.LoadedSceneIsEditor) {
					if (EngineersReport.Ready && EngineersReport.Instance != null) {
						if (EngineersReport.Instance.appLauncherButton != null) {
							if (EngineersReport.Instance.appLauncherButton.IsHovering || EngineersReport.Instance.appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True) {
								return true;
							}
						}
					}
				}
				if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER) {
					if (ContractsApp.Instance != null) {
						if (ContractsApp.Instance.appLauncherButton != null) {
							if (ContractsApp.Instance.appLauncherButton.IsHovering || ContractsApp.Instance.appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True) {
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		IEnumerator AfterAllAppAdded() {
			while (ApplicationLauncher.Instance == null) {
				yield return 0;
			}
			while (!ApplicationLauncher.Ready) {
				yield return 0;
			}
			yield return new WaitForSecondsRealtime (0.5f);
			if (QStockToolbar.Instance != null) {
				QStockToolbar.Instance.RefreshPos ();
			}
			PopulateAppLauncherButtons (true);
			yield return new WaitForSecondsRealtime (0.5f);
			First = true;
			Log ("AfterAllAppAdded", "QHide");
		}

		internal IEnumerator UpdateMods() {
			yield return new WaitForSecondsRealtime (1);
			Coroutine _coroutine = coroutineMods;
			Log ("Start UpdateMods " + _coroutine.GetHashCode (), "QHide");
			while (_coroutine == coroutineMods) {
				while (!QStockToolbar.isAvailable || !First || (!QSettings.Instance.isHidden && !WindowSettings)) {
					yield return 0;
				}
				PopulateAppLauncherButtons ();
				yield return new WaitForSecondsRealtime (10);
			}
			Log ("End UpdateMods " + _coroutine.GetHashCode (), "QHide");
		}

		void OnShowUI() {
			dateAppLauncher = DateTime.Now;
			dateStage = DateTime.Now;
			First = true;
			Log ("OnShowUI", "QHide");
		}

		void OnHideUI() {
			First = false;
			Log ("OnHideUI", "QHide");
		}

		void OnGameSceneLoadRequested(GameScenes gameScene) {
			First = false;
			Log ("OnGameSceneLoadRequested", "QHide");
		}

		void PopulateAppLauncherButtons(bool force = false) {
			if (!QStockToolbar.isAvailable || (!First && !force)) {
				return;
			}

			Log ("Begin PopulateAppLauncherButtons", "QHide");
			bool _cansave = false;
			ApplicationLauncherButton[] _appLauncherButtons = (ApplicationLauncherButton[])Resources.FindObjectsOfTypeAll (typeof (ApplicationLauncherButton));
			for (int _i = _appLauncherButtons.Length - 1; _i >= 0; --_i) {
				ApplicationLauncherButton _appLauncherButton = _appLauncherButtons[_i];
				if (!QStockToolbar.isModApp (_appLauncherButton) || QStockToolbar.Instance.isThisApp (_appLauncherButton)) {
					continue;
				}
                string toolTip;
				Log ("Mods: " + QMods.GetModName (_appLauncherButton, out toolTip) + " " + _appLauncherButton.GetInstanceID (), "QHide");
				QMods _QData = ModsToolbar.Find (q => q.AppRef == QMods.GetAppRef (_appLauncherButton));
				if (_QData != null) {
					if (_QData.ModName != "None" && _QData.isActive) {
						if (!_QData.isThisApp (_appLauncherButton)) {
							_QData.Refresh (_appLauncherButton);
						}
						if (_QData.isHidden != QSettings.Instance.isHidden) {
							_QData.isHidden = QSettings.Instance.isHidden;
						}
						continue;
					}
					ModsToolbar.Remove (_QData);
					//Log ("Deleted an lost AppLauncherButton", "QHide");
				}
				_QData = new QMods (_appLauncherButton);
				ModsToolbar.Add (_QData);
				_cansave = true;
				Log ("Added the AppLauncherButton of: " + _QData.ToolTip, "QHide");
			}
			if (_cansave) {
				QSettings.Instance.Save ();
			}
			Log ("End PopulateAppLauncherButtons", "QHide");
			Log ("AppLauncherButtons count: " + _appLauncherButtons.Length, "QHide");
			Log ("ModsToolbar count: " + ModsToolbar.Count, "QHide");
			Log ("ModHasFirstConfig count: " + QSettings.Instance.ModHasFirstConfig.Count, "QHide");
		}

		internal void DrawAppLauncherButtons() {
			List<string> _modsName = new List<string> ();
			for (int _i = ModsToolbar.Count - 1; _i >= 0; --_i) {
				QMods _qMods = ModsToolbar[_i];
				if (_qMods == null) {
					continue;
				}
				if (_qMods.ModName == "None" || _modsName.Contains (_qMods.ModName)) {
					continue;
				}

                _modsName.Add (_qMods.ModName);
				GUILayout.BeginHorizontal ();
				GUILayout.Label (string.Format ("<b>{0}</b>", _qMods.ToolTip), GUILayout.Width (200));
				bool _CanBePin = _qMods.CanBePin;
				GUILayout.FlexibleSpace ();
				_CanBePin = GUILayout.Toggle (_CanBePin, Localizer.Format("quickhide_pin"), GUILayout.Width (300));
				if (_CanBePin != _qMods.CanBePin) {
					_qMods.CanBePin = _CanBePin;
				}
				bool _CanBeHide = _qMods.CanBeHide;
				GUILayout.FlexibleSpace ();
				_CanBeHide = GUILayout.Toggle (_CanBeHide, Localizer.Format("quickhide_canBeHidden"), GUILayout.Width (200));
				if (_CanBeHide != _qMods.CanBeHide) {
					_qMods.CanBeHide = _CanBeHide;
				}
				if (_CanBeHide) {
					bool _CanSetFalse = _qMods.CanSetFalse;
					GUILayout.FlexibleSpace ();
					_CanSetFalse = GUILayout.Toggle (_CanSetFalse, Localizer.Format("quickhide_setFalse"), GUILayout.Width (400));
					if (_CanSetFalse != _qMods.CanSetFalse) {
						_qMods.CanSetFalse = _CanSetFalse;
					}
				}
				GUILayout.EndHorizontal ();
#if DEBUG
				GUILayout.BeginHorizontal();
				GUILayout.Space(210);
				GUILayout.Label("VisibleInScenes: " + _qMods.VisibleInScenes);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Space(210);
				GUILayout.Label("AppScenesSaved: " + _qMods.AppScenesSaved);
				GUILayout.EndHorizontal();
#endif
			}
		}

		public void HideMods() {
			HideMods (!QSettings.Instance.isHidden);
		}

		public void HideMods(bool value) {
			if (!QSettings.Instance.isHidden && value) {
				PopulateAppLauncherButtons ();
			}
			for (int _i = ModsToolbar.Count - 1; _i >= 0; --_i) {
				QMods _qMods = ModsToolbar[_i];
				if (!_qMods.CanBeHide) {
					continue;
				}
				if (value) {
					if (_qMods.CanSetFalse) {
						_qMods.SetFalse ();
					}
				}
				_qMods.isHidden = value;
			}
			if (QSettings.Instance.isHidden != value) {
				QSettings.Instance.isHidden = value;
				QSettings.Instance.Save ();
				QStockToolbar.Instance.Refresh ();
				BlizzyToolbar.Refresh ();
			}
			Log ((QSettings.Instance.isHidden ? "Hide mods buttons" : "Show mods buttons"));

		}

		void HideAppLauncher(bool hide) {
			if (ApplicationLauncher.Instance == null) {
				return;
			}
			if (hide) {
				ApplicationLauncher.Instance.Hide ();
			}
			else {
				ApplicationLauncher.Instance.Show ();
			}
			Log ("HideAppLauncher: " + hide);
		}

		void HideStage(bool hide) {
			if (StageManager.Instance.Visible == !hide) {
				return;
			}
			typeof (StageManager).GetProperty ("Visible").SetValue (StageManager.Instance, !hide, null);
			FieldInfo[] _properties = typeof (StageManager).GetFields (BindingFlags.NonPublic | BindingFlags.Instance);
			for (int _i = _properties.Length - 1; _i >= 0; --_i) {
				FieldInfo _property = _properties[_i];
				if (_property.FieldType == typeof (VerticalLayoutGroup)) {
					VerticalLayoutGroup _vLayoutGroup = (VerticalLayoutGroup)_property.GetValue (StageManager.Instance);
					_vLayoutGroup.gameObject.SetActive (!hide);
					break;
				}
			}
			Log ("HideStageManager: " + hide);
		}

		void LateUpdate() {
			if (!First) {
				return;
			}
			if (QSettings.Instance.HideAppLauncher && ApplicationLauncher.Instance != null) {
				AutoHideAppLauncher ();
			}
			if (QSettings.Instance.HideStage && (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor)) {
				AutoHideStage ();
			}
		}

		void AutoHideAppLauncher() {
			if (!ApplicationLauncher.Ready) {
				if (StockToolBar_Position.Contains (Mouse.screenPos) || !EditorhasRootPart) {
					dateAppLauncher = DateTime.Now;
					HideAppLauncher (false);
				}
			}
			else {
				if (!StockToolBar_Position.Contains (Mouse.screenPos) && EditorhasRootPart) {
					if ((DateTime.Now - dateAppLauncher).TotalSeconds > QSettings.Instance.TimeToKeep) {
						if (!isPinned) {
							HideAppLauncher (true);
						}
					}
					else {
						return;
					}
				}
				dateAppLauncher = DateTime.Now;
			}
		}

		void AutoHideStage() {
			if (!StageManager.Instance.Visible) {
				if (Stage_Position.Contains (Mouse.screenPos) || !EditorhasRootPart) {
					dateStage = DateTime.Now;
					HideStage (false);
				}
			}
			else {
				if (!Stage_Position.Contains (Mouse.screenPos) && EditorhasRootPart) {
					if ((DateTime.Now - dateStage).TotalSeconds > QSettings.Instance.TimeToKeep) {
						HideStage (true);
					}
					else {
						return;
					}
				}
				dateStage = DateTime.Now;
			}
		}
	}
}