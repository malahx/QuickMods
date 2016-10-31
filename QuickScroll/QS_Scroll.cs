/* 
QuickScroll
Copyright 2015 Malah

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

using KSP.UI;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace QuickScroll {

	public class QScroll {
		private static bool isSimple {
			get {
				return EditorLogic.Mode == EditorLogic.EditorModes.SIMPLE;
			}
		}

		public static Vector3 GetPosition(Transform trans) {
			Camera _uiCam = UIMainCamera.Camera;
			if (_uiCam != null) {
				Vector3 _screenPos = _uiCam.WorldToScreenPoint (trans.position);
				_screenPos.y = Screen.height - _screenPos.y;
				return _screenPos;
			}
			return Vector3d.zero;
		}

		public static Vector3 GetlocalPosition(Transform trans) {
			Camera _uiCam = UIMainCamera.Camera;
			if (_uiCam != null) {
				Vector3 _screenPos = _uiCam.WorldToScreenPoint (trans.localPosition);
				_screenPos.y = Screen.height - _screenPos.y;
				return _screenPos;
			}
			return Vector3d.zero;
		}

		public static Vector3 GetVPosition(Vector3 pos) {
			Camera _uiCam = UIMainCamera.Camera;
			if (_uiCam != null) {
				Vector3 _screenPos = _uiCam.WorldToScreenPoint (pos);
				_screenPos.y = Screen.height - _screenPos.y;
				return _screenPos;
			}
			return Vector3d.zero;
		}
			
		public static Rect ListScreenPos(UIList list) {
			Vector3 _position = GetPosition (list.ListAnchor);
			Vector3 _localPosition = GetlocalPosition (list.ListAnchor);
			Rect _rect = new Rect (0, _position.y, _position.x + 10, _localPosition.x);
			return _rect;
		}

		public static Rect partListScreenPos {
			get {
				Vector3 _position = QScroll.GetPosition (EditorPartList.Instance.partListScrollRect.transform);
				float _height = QScroll.GetVPosition (EditorPartList.Instance.partListScrollRect.content.anchoredPosition3D).x - _position.y;
				float _width = EditorPartList.Instance.partListScrollRect.content.rect.width;
				return new Rect (_position.x, _position.y, _width, _height);
			}
		}

		private static bool isOverArrow {
			get {
				Rect _rect = new Rect (0, 0, 60, 24);
				return _rect.Contains(Mouse.screenPos);
			}
		}

		private static bool isOverParts {
			get {
				return partListScreenPos.Contains(Mouse.screenPos);
			}
		}

		private static bool isOverCategories {
			get {
				Rect _cat = QScroll.ListScreenPos (PartCategorizer.Instance.scrollListSub);
				if (!isSimple) {
					Rect _filter = QScroll.ListScreenPos (PartCategorizer.Instance.scrollListMain);
					_cat.x = _filter.width;
				}
				return _cat.Contains (Mouse.screenPos);
			}
		}

		private static bool isOverFilters {
			get {
				if (isSimple) {
					return false;
				}
				return QScroll.ListScreenPos(PartCategorizer.Instance.scrollListMain).Contains(Mouse.screenPos);;
			}
		}

		//Too many private in EditorPartList ...
		//private List<AvailablePart> ;
		//private EditorPartList.State ;
		public static int indexParts {
			get {
				List<AvailablePart> _aParts = new List<AvailablePart> ();
				if (!string.IsNullOrEmpty(PartCategorizer.Instance.searchField.text) || PartCategorizer.Instance.searchField.isFocused) {
					_aParts = EditorPartList.Instance.ExcludeFilters.GetFilteredList (PartLoader.LoadedPartsList);
					_aParts = _aParts.Where (EditorPartList.Instance.AmountAvailableFilter.FilterCriteria).ToList<AvailablePart> ();
					_aParts = _aParts.Where (EditorPartList.Instance.SearchFilterParts.FilterCriteria).ToList<AvailablePart> ();
				} else {
					switch (QCategory.CurrentCategory.displayType) {
					case EditorPartList.State.PartsList:
						_aParts = EditorPartList.Instance.ExcludeFilters.GetFilteredList (PartLoader.LoadedPartsList);
						_aParts = _aParts.Where (EditorPartList.Instance.AmountAvailableFilter.FilterCriteria).ToList<AvailablePart> ();
						_aParts = EditorPartList.Instance.CategorizerFilters.GetFilteredList (_aParts);
						break;
					case EditorPartList.State.CustomPartList:
						_aParts = EditorPartList.Instance.ExcludeFilters.GetFilteredList (PartLoader.LoadedPartsList);
						_aParts = _aParts.Where (EditorPartList.Instance.AmountAvailableFilter.FilterCriteria).ToList<AvailablePart> ();
						_aParts = EditorPartList.Instance.CategorizerFilters.GetFilteredList (_aParts);
						break;
					case EditorPartList.State.SubassemblyList:
						break;
					case EditorPartList.State.Nothing:
						break;
					case EditorPartList.State.PartSearch:
						_aParts = EditorPartList.Instance.ExcludeFilters.GetFilteredList (PartLoader.LoadedPartsList);
						_aParts = _aParts.Where (EditorPartList.Instance.AmountAvailableFilter.FilterCriteria).ToList<AvailablePart> ();
						_aParts = _aParts.Where (EditorPartList.Instance.SearchFilterParts.FilterCriteria).ToList<AvailablePart> ();
						break;
					}
				}
				return _aParts.Count;
			}
		}

		public static int partsPages {
			get {
				int _iconHeight = 71;
				float _partsOnOnePage = 3 * QScroll.partListScreenPos.height / _iconHeight;
				int _pages = (int)Math.Ceiling((QScroll.indexParts / _partsOnOnePage));
				return _pages;
			}
		}

		internal static void Update() {
			if (EditorLogic.fetch.editorScreen != EditorScreen.Parts || !QSettings.Instance.EnableWheelScroll || !EditorPanels.Instance.IsMouseOver ()) {
				return;
			}
			if (EditorPartList.Instance != null) {
				if (EditorPartList.Instance.partListScrollRect.vertical && EditorPartList.Instance.partListScrollRect.verticalScrollbar.IsInteractable()) {
					if (string.IsNullOrEmpty (PartCategorizer.Instance.searchField.text) && !PartCategorizer.Instance.searchField.isFocused) {
						if (indexParts > 0) {
							int _partsPages = partsPages;
							if (EditorPartList.Instance.partListScrollRect.verticalScrollbar.numberOfSteps != _partsPages) {
								EditorPartList.Instance.partListScrollRect.verticalScrollbar.numberOfSteps = _partsPages;
								EditorPartList.Instance.partListScrollRect.scrollSensitivity = _partsPages * 150f;
								QuickScroll.Log ("Set pages: " + _partsPages, "QScroll");
								QuickScroll.Log ("\tscrollSensitivity " + _partsPages * 150f, "QScroll");
							}
						}
					}
					if (PartCategorizer.Instance.searchField.isFocused) {
						if (EditorPartList.Instance.partListScrollRect.scrollSensitivity != 27f || EditorPartList.Instance.partListScrollRect.verticalScrollbar.numberOfSteps != 0) {
							EditorPartList.Instance.partListScrollRect.verticalScrollbar.numberOfSteps = 0;
							EditorPartList.Instance.partListScrollRect.scrollSensitivity = 27f;
							QuickScroll.Log ("Reset scrollSensitivity and numberOfSteps", "QScroll");
						}
					}
				}
			}
			float _scroll = Input.GetAxis ("Mouse ScrollWheel");
			if (_scroll == 0) {
				return;
			}
			if (isOverArrow) {
				if (isSimple) {
					PartCategorizer.Instance.SetAdvancedMode ();
				} else {
					PartCategorizer.Instance.SetSimpleMode ();
				}
				return;
			}
			bool _ModKeyFilterWheel = false;
			bool _ModKeyCategoryWheel = false;
			if (QSettings.Instance.EnableWheelShortCut) {
				_ModKeyFilterWheel = Input.GetKey (QSettings.Instance.ModKeyFilterWheel);
				_ModKeyCategoryWheel = Input.GetKey (QSettings.Instance.ModKeyCategoryWheel);
			}
			bool _ModKeyWheel = _ModKeyFilterWheel || _ModKeyCategoryWheel;
			if (isOverFilters || (_ModKeyWheel && isOverCategories) || (_ModKeyFilterWheel && isOverParts)) {
				if (isSimple) {
					PartCategorizer.Instance.SetAdvancedMode ();
				}
				QCategory.SelectPartFilter (_scroll > 0);
			} else if (isOverCategories || (_ModKeyCategoryWheel && isOverParts)) {
				QCategory.SelectPartCategory (_scroll > 0);
			} 
			/*else if (isOverParts && indexParts > 0) {
				QCategory.PartListTooltipsTWEAK (false);
			}*/
		}
	}
}