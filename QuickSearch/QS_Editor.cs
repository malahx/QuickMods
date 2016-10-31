/* 
QuickSearch
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

using KSP.UI;
using KSP.UI.Screens;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace QuickSearch {
	public partial class QEditor {

		public static QEditor Instance;

		bool Ready = false;

		public bool isReady {
			get {
				return Ready && searchFilterParts == EditorPartList.Instance.SearchFilterParts;
			}
		}

		public static EditorPartListFilter<AvailablePart> searchFilterParts;

		Image searchImage = null;

		protected override void Awake() {
			if (HighLogic.LoadedScene != GameScenes.EDITOR) {
				Warning ("The editor search function works only on the on the Editor. Destroy.", "QEditor");
				Destroy (this);
				return;
			}
			if (Instance != null) {
				Warning ("There's already an Instance of " + MOD + ". Destroy.", "QEditor");
				Destroy (this);
				return;
			}
			Instance = this;
			if (!QSettings.Instance.EditorSearch) {
				Warning ("The editor search function is disabled. Destroy.", "QEditor");
				Destroy (this);
				return;
			}
			base.Awake ();
			Log ("Awake", "QEditor");
		}

		protected override void Start() {
			base.Start ();
			Func<AvailablePart, bool> _criteria = (_aPart) => QSearch.FindPart(_aPart);
			searchFilterParts = new EditorPartListFilter<AvailablePart> (MOD, _criteria);
			PartCategorizer.Instance.searchField.onValueChanged.RemoveAllListeners ();
			PointerClickHandler _pointerClickSearch = null;
			PartCategorizer.Instance.searchField.GetComponentCached<PointerClickHandler> (ref _pointerClickSearch);
			if (_pointerClickSearch != null) {
				_pointerClickSearch.onPointerClick.RemoveAllListeners ();
				_pointerClickSearch.onPointerClick.AddListener (new UnityAction<PointerEventData> (SearchField_OnClick));
			}
			PartCategorizer.Instance.searchField.onEndEdit.AddListener (new UnityAction<string> (SearchField_OnEndEdit));
			PartCategorizer.Instance.searchField.onValueChanged.AddListener (new UnityAction<string> (SearchField_OnValueChange));
			PartCategorizer.Instance.searchField.GetComponentCached<Image> (ref searchImage);
			setSearchFilter ();
			Log ("Start", "QEditor");
		}

		void Update() {
			if (!isReady) {
				return;
			}
			if (Input.GetKeyDown (GameSettings.Editor_partSearch.primary) || Input.GetKeyDown (GameSettings.Editor_partSearch.secondary)) {
				InitSearch ();
			}
		}

		protected override void OnDestroy() {
			base.OnDestroy ();
			Log ("OnDestroy", "QEditor");
		}

		void SearchField_OnClick(PointerEventData eventData) {
			if (!Ready) {
				return;
			}
			InitSearch ();
			Log ("SearchField_OnClick", "QEditor");
		}

		void InitSearch() {
			PartCategorizer.Instance.FocusSearchField ();
			if (searchImage != null) {
				searchImage.color = Color.cyan;
			}
			setSearchFilter();
			EditorPartList.Instance.Refresh (EditorPartList.State.PartSearch);
			InputLockManager.SetControlLock (ControlTypes.KEYBOARDINPUT, MOD + "-KeyBoard");
			Log ("InitSearch", "QEditor");
		}

		void SearchField_OnValueChange(string s) {
			if (!isReady) {
				return;
			}
			QSearch.Text = s;
			EditorPartList.Instance.Refresh ();
			Log ("SearchField_OnValueChange: " + s, "QEditor");
		}

		void SearchField_OnEndEdit(string s) {
			InputLockManager.RemoveControlLock (MOD + "-KeyBoard");
			Log ("SearchField_OnEndEdit", "QEditor");
		}

		public void Refresh() {
			QSearch.Text = PartCategorizer.Instance.searchField.text;
			setSearchFilter ();
			EditorPartList.Instance.Refresh ();
		}

		void setSearchFilter() {
			EditorPartList.Instance.SearchFilterParts = searchFilterParts;
			Ready = true;
			Log ("setSearchFilter", "QEditor");
		}

		void resetSearchFilter() {
			EditorPartList.Instance.SearchFilterParts = null;
			Log ("resetSearchFilter", "QEditor");
		}
	}
}