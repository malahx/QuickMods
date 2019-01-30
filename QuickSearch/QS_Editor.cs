/* 
QuickSearch
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

using KSP.UI;
using KSP.UI.Screens;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using QuickSearch.QUtils;

namespace QuickSearch
{
    public partial class QEditor
    {

        public static QEditor Instance;

        bool Ready = false;

        public bool isReady
        {
            get
            {
                return Ready && searchFilterParts == EditorPartList.Instance.SearchFilterParts;
            }
        }

        public static EditorPartListFilter<AvailablePart> searchFilterParts;

        Image searchImage = null;

        protected override void Awake()
        {
            if (HighLogic.LoadedScene != GameScenes.EDITOR)
            {
                QDebug.Warning("The editor search function works only on the on the Editor. Destroy.", "QEditor");
                Destroy(this);
                return;
            }
            if (Instance != null)
            {
                QDebug.Warning("There's already an Instance of " + MOD + ". Destroy.", "QEditor");
                Destroy(this);
                return;
            }
            Instance = this;
            if (!QSettings.Instance.EditorSearch)
            {
                QDebug.Warning("The editor search function is disabled. Destroy.", "QEditor");
                Destroy(this);
                return;
            }
            base.Awake();
            QDebug.Log("Awake", "QEditor");
        }

        protected override void Start()
        {
            base.Start();
            Func<AvailablePart, bool> _criteria = (_aPart) => QSearch.FindPart(_aPart);
            searchFilterParts = new EditorPartListFilter<AvailablePart>(MOD, _criteria);
            PartCategorizer.Instance.searchField.onValueChanged.RemoveAllListeners();
            PointerClickHandler _pointerClickSearch = null;
            PartCategorizer.Instance.searchField.GetComponentCached<PointerClickHandler>(ref _pointerClickSearch);
            if (_pointerClickSearch != null)
            {
                _pointerClickSearch.onPointerClick.RemoveAllListeners();
                _pointerClickSearch.onPointerClick.AddListener(new UnityAction<PointerEventData>(SearchField_OnClick));
            }
            PartCategorizer.Instance.searchField.onEndEdit.AddListener(new UnityAction<string>(SearchField_OnEndEdit));
            PartCategorizer.Instance.searchField.onValueChanged.AddListener(new UnityAction<string>(SearchField_OnValueChange));
            PartCategorizer.Instance.searchField.GetComponentCached<Image>(ref searchImage);
            setSearchFilter();
            QDebug.Log("Start", "QEditor");
        }

        void Update()
        {
            if (!isReady)
            {
                return;
            }

            if (GameSettings.Editor_partSearch.GetKeyDown())
            {
                InitSearch();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            QDebug.Log("OnDestroy", "QEditor");
        }

        void SearchField_OnClick(PointerEventData eventData)
        {
            if (!Ready)
            {
                return;
            }
            else
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    InitSearch();
                    QDebug.Log("SearchField_OnClick", "QEditor");
                }
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    string newString = "";
                    PartCategorizer.Instance.searchField.text = newString;
                    SearchField_OnValueChange(newString);
                    QDebug.Log("SearchField_OnClick", "Text Deleted");
                }
            }
        }

        void InitSearch()
        {
            PartCategorizer.Instance.FocusSearchField();
            if (searchImage != null)
            {
                searchImage.color = Color.cyan;
            }
            setSearchFilter();
            if (!QSettings.Instance.enableEnterToSearch)
            {
                EditorPartList.Instance.Refresh(EditorPartList.State.PartSearch);
            }
            ShowHistory();
            InputLockManager.SetControlLock(ControlTypes.KEYBOARDINPUT, MOD + "-KeyBoard");
            QDebug.Log("InitSearch", "QEditor");
        }

        void SearchField_OnValueChange(string s)
        {
            if (!isReady || QSettings.Instance.enableEnterToSearch)
            {
                return;
            }
            ShowHistory();
            QSearch.Text = s;
            QDebug.Log("SearchField_OnValueChange: " + s, "QEditor");
        }

        void SearchField_OnEndEdit(string s)
        {
            if (QSettings.Instance.enableEnterToSearch)
            {
                QSearch.Text = s;
            }
            HideHistory();
            InputLockManager.RemoveControlLock(MOD + "-KeyBoard");
            QDebug.Log("SearchField_OnEndEdit", "QEditor");
        }

        public void Refresh()
        {
            setSearchFilter();
            QSearch.Text = PartCategorizer.Instance.searchField.text;
        }

        void setSearchFilter()
        {
            EditorPartList.Instance.SearchFilterParts = searchFilterParts;
            Ready = true;
            QDebug.Log("setSearchFilter", "QEditor");
        }

        void resetSearchFilter()
        {
            EditorPartList.Instance.SearchFilterParts = null;
            QDebug.Log("resetSearchFilter", "QEditor");
        }
    }
}
