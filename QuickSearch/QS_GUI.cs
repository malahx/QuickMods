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
GNU General Public Licence for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using System.Collections;
using System.Text.RegularExpressions;
using KSP.Localization;
using KSP.UI;
using KSP.UI.Screens;
using QuickSearch.QUtils;
//using QuickSearch.Toolbar;
using UnityEngine;

using ClickThroughFix;


namespace QuickSearch
{

    public partial class QuickSearch
    {

        protected GUIStyle TextField;
        protected bool WindowHistory = false;
        bool WindowSettings = false;
        Coroutine tryHideHistory;

        Rect rectSettings = new Rect(0, 0, 0, 0);
        Rect RectSettings
        {
            get
            {
                Rect _rect = rectSettings;
#if false
                if (!QSettings.Instance.StockToolBar) {
					_rect.x = (Screen.width - _rect.width) / 2;
					_rect.y = (Screen.height - _rect.height) / 2;
				} else 
#endif
                {
                    _rect.x = Screen.width - _rect.width - 75;
                    _rect.y = Screen.height - _rect.height - 40;
                }
                return _rect;
            }
            set
            {
                rectSettings = value;
            }
        }

        Rect rectHistory;
        Rect RectHistory
        {
            get
            {
                if (rectHistory == new Rect())
                {
                    if (HighLogic.LoadedSceneIsEditor)
                    {
                        rectHistory = new Rect(50 + PartCategorizer.Instance.searchField.textViewport.rect.width, 20 + PartCategorizer.Instance.searchField.textViewport.rect.height, 250, 0);
                    }
                    else
                    {
                        rectHistory = new Rect(QRnD.Instance.RectRDSearch.x, 0, QRnD.Instance.RectRDSearch.width, 0);
                    }
                }
                if (!HighLogic.LoadedSceneIsEditor)
                {
                    rectHistory.y = QRnD.Instance.RectRDSearch.y - rectHistory.height - 5;
                }
                return rectHistory;
            }
            set
            {
                rectHistory = value;
            }
        }

        bool IsMouseOver()
        {
            return (WindowSettings && RectSettings.Contains(Mouse.screenPos)) || (WindowHistory && RectHistory.Contains(Mouse.screenPos));
        }

        internal static void instancedSettings()
        {
            if (QEditor.Instance != null)
            {
                QEditor.Instance.Settings();
                return;
            }
            if (QRnD.Instance != null)
            {
                QRnD.Instance.Settings();
                return;
            }
            QDebug.Log("No instance");
        }

        void Settings()
        {
            WindowSettings = !WindowSettings;
            QStock.Instance.Set(WindowSettings);
            if (!WindowSettings)
            {
                Save();
            }
            QDebug.Log("Settings", "QGUI");
        }

        void HideSettings()
        {
            WindowSettings = false;
            QStock.Instance.Set(WindowSettings);
            Save();
            QDebug.Log("HideSettings", "QGUI");
        }

        void ShowSettings()
        {
            WindowSettings = true;
            QStock.Instance.Set(WindowSettings);
            QDebug.Log("ShowSettings", "QGUI");
        }

        protected void HideHistory()
        {
            if (!WindowHistory || tryHideHistory != null)
            {
                return;
            }
            tryHideHistory = StartCoroutine(hideHistory());
        }

        IEnumerator hideHistory()
        {
            yield return new WaitForEndOfFrame();
            while (Mouse.Left.GetButton())
            {
                yield return 0;
            }
            yield return new WaitForEndOfFrame();
            QHistory.Instance.Add(QSearch.Text);
            WindowHistory = false;
            tryHideHistory = null;
            QDebug.Log("HideHistory", "QGUI");
        }

        protected void ShowHistory()
        {
            if (WindowHistory)
            {
                return;
            }
            if (!QSettings.Instance.enableHistory)
                return;

            WindowHistory = true;
            QDebug.Log("ShowHistory", "QGUI");
        }

        void Save()
        {
            QStock.Instance.Reset();
            //BlizzyToolbar.Reset ();
            if (QEditor.Instance != null)
            {
                QEditor.Instance.Refresh();
            }
            QSettings.Instance.Save();
        }

        protected virtual void OnGUI()
        {
            GUI.skin = HighLogic.Skin;
            QGUI.Lock(IsMouseOver());
            if (WindowSettings)
            {
                RectSettings = ClickThruBlocker.GUILayoutWindow(1545146, RectSettings, DrawSettings, RegisterToolbar.MOD + " " + RegisterToolbar.VERSION);
            }
            if (WindowHistory)
            {
                RectHistory = ClickThruBlocker.GUILayoutWindow(1545147, RectHistory, QHistory.Instance.Draw, RegisterToolbar.MOD + ": " + Localizer.Format("quicksearch_history"));
            }
        }

        // Panneau de configuration
        void DrawSettings(int id)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Box(Localizer.Format("quicksearch_options"), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            QSettings.Instance.EditorSearch = GUILayout.Toggle(QSettings.Instance.EditorSearch, Localizer.Format("quicksearch_editorSearch"), GUILayout.Width(400));
            GUILayout.FlexibleSpace();
            QSettings.Instance.RnDSearch = GUILayout.Toggle(QSettings.Instance.RnDSearch, Localizer.Format("quicksearch_enableRnD"), GUILayout.Width(400));
            GUILayout.EndHorizontal();

            if (QSettings.Instance.EditorSearch || QSettings.Instance.RnDSearch)
            {

                GUILayout.BeginHorizontal();
                QSettings.Instance.enableSearchExtension = GUILayout.Toggle(QSettings.Instance.enableSearchExtension, Localizer.Format("quicksearch_enableExtended"), GUILayout.Width(400));
                GUILayout.FlexibleSpace();
                QSettings.Instance.enableHistory = GUILayout.Toggle(QSettings.Instance.enableHistory, Localizer.Format("quicksearch_enableHistory"), GUILayout.Width(400));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                QSettings.Instance.enableEnterToSearch = GUILayout.Toggle(QSettings.Instance.enableEnterToSearch, Localizer.Format("quicksearch_type"), GUILayout.Width(400));
                GUILayout.FlexibleSpace();
                GUILayout.Label("Time (in secs) to wait before starting search", GUILayout.Width(400));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Space(400);
                GUILayout.Label(QSettings.Instance.timeToWaitBeforeSearch.ToString("F1") + "(s)", GUILayout.Width(40));
                QSettings.Instance.timeToWaitBeforeSearch = GUILayout.HorizontalSlider(QSettings.Instance.timeToWaitBeforeSearch, 0f, 2f, GUILayout.Width(350));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                if (QSettings.Instance.enableHistory)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Box(Localizer.Format("quicksearch_history"), GUILayout.Height(30));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    bool b = QSettings.Instance.historySortby == (int)QHistory.SortBy.COUNT;
                    QSettings.Instance.historySortby = GUILayout.Toggle(b, Localizer.Format("quicksearch_sortCount"), GUILayout.Width(400)) ? (int)QHistory.SortBy.COUNT : (int)QHistory.SortBy.DATE;
                    GUILayout.FlexibleSpace();
                    QSettings.Instance.historySortby = GUILayout.Toggle(!b, Localizer.Format("quicksearch_sortDate"), GUILayout.Width(400)) ? (int)QHistory.SortBy.DATE : (int)QHistory.SortBy.COUNT;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(Localizer.Format("quicksearch_searchDisplay"), GUILayout.Width(300));
                    int i = 10;
                    if (int.TryParse(GUILayout.TextField(QSettings.Instance.historyIndex.ToString(), TextField, GUILayout.Width(75)), out i))
                    {
                        QSettings.Instance.historyIndex = i;
                    }
                    GUILayout.EndHorizontal();
                }

                if (QSettings.Instance.enableSearchExtension)
                {

                    GUILayout.BeginHorizontal();
                    GUILayout.Box(Localizer.Format("quicksearch_shortcut"), GUILayout.Height(30));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(Localizer.Format("quicksearch_not"), GUILayout.Width(100));
                    QSettings.Instance.searchNOT = cleanString(GUILayout.TextField(QSettings.Instance.searchNOT, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_and"), GUILayout.Width(100));
                    QSettings.Instance.searchAND = cleanString(GUILayout.TextField(QSettings.Instance.searchAND, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_or"), GUILayout.Width(100));
                    QSettings.Instance.searchOR = cleanString(GUILayout.TextField(QSettings.Instance.searchOR, TextField, GUILayout.Width(75)));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(Localizer.Format("quicksearch_wordBegin"), GUILayout.Width(100));
                    QSettings.Instance.searchBegin = cleanString(GUILayout.TextField(QSettings.Instance.searchBegin, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_wordEnd"), GUILayout.Width(100));
                    QSettings.Instance.searchEnd = cleanString(GUILayout.TextField(QSettings.Instance.searchEnd, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_word"), GUILayout.Width(100));
                    QSettings.Instance.searchWord = cleanString(GUILayout.TextField(QSettings.Instance.searchWord, TextField, GUILayout.Width(75)));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(Localizer.Format("quicksearch_name"), GUILayout.Width(100));
                    QSettings.Instance.searchName = cleanString(GUILayout.TextField(QSettings.Instance.searchName, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_title"), GUILayout.Width(100));
                    QSettings.Instance.searchTitle = cleanString(GUILayout.TextField(QSettings.Instance.searchTitle, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_descr"), GUILayout.Width(100));
                    QSettings.Instance.searchDescription = cleanString(GUILayout.TextField(QSettings.Instance.searchDescription, TextField, GUILayout.Width(75)));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(Localizer.Format("quicksearch_author"), GUILayout.Width(100));
                    QSettings.Instance.searchAuthor = cleanString(GUILayout.TextField(QSettings.Instance.searchAuthor, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_manufacturer"), GUILayout.Width(100));
                    QSettings.Instance.searchManufacturer = cleanString(GUILayout.TextField(QSettings.Instance.searchManufacturer, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_tag"), GUILayout.Width(100));
                    QSettings.Instance.searchTag = cleanString(GUILayout.TextField(QSettings.Instance.searchTag, TextField, GUILayout.Width(75)));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(Localizer.Format("quicksearch_resources"), GUILayout.Width(100));
                    QSettings.Instance.searchResourceInfos = cleanString(GUILayout.TextField(QSettings.Instance.searchResourceInfos, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_tech"), GUILayout.Width(100));
                    QSettings.Instance.searchTechRequired = cleanString(GUILayout.TextField(QSettings.Instance.searchTechRequired, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_size"), GUILayout.Width(100));
                    QSettings.Instance.searchPartSize = cleanString(GUILayout.TextField(QSettings.Instance.searchPartSize, TextField, GUILayout.Width(75)));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(Localizer.Format("quicksearch_module"), GUILayout.Width(100));
                    QSettings.Instance.searchModule = cleanString(GUILayout.TextField(QSettings.Instance.searchModule, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Localizer.Format("quicksearch_regex"), GUILayout.Width(100));
                    QSettings.Instance.searchRegex = cleanString(GUILayout.TextField(QSettings.Instance.searchRegex, TextField, GUILayout.Width(75)));
                    GUILayout.FlexibleSpace();
                    GUILayout.Space(185);
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(Localizer.Format("quicksearch_close"), GUILayout.Width(100)))
            {
                HideSettings();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        string cleanString(string text)
        {
            string _str = clean(text);
            if (string.IsNullOrEmpty(_str))
            {
                return string.Empty;
            }
            return _str.Substring(_str.Length - 1, 1);
        }
        string clean(string text)
        {
            return Regex.Replace(text, @"[^%ù£¤'#~&`_²\{\}!\.@\-|&/\(\)\[\]\+?,;:/\*µ\^\$=\ ""]", string.Empty);
        }
    }
}