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

using System;
using System.Collections.Generic;
using System.IO;
using KSP.Localization;
using KSP.UI.Screens;
using QuickSearch.QUtils;
using UnityEngine;

namespace QuickSearch
{
    public class QHistory
    {

        public enum SortBy
        {
            NAME = 0,
            COUNT = 1,
            DATE = 2
        }

        [KSPField(isPersistant = true)]
        static QHistory instance;
        public static QHistory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QHistory();
                }
                return instance;
            }
        }

        readonly string CFGNODE = "SearchHistory";
        readonly string CONFIG_DIR = RegisterToolbar.PATH + "/PluginData";
        readonly string CONFIG_PATH = RegisterToolbar.PATH + "/PluginData" + "/History.cfg";

        readonly GUIStyle btnStyle;
        readonly List<Search> history;
        int index;
        string lastSearch = string.Empty;

        GUIStyle lblActive;
        GUIStyle LblActive
        {
            get
            {
                if (lblActive == null)
                {
                    lblActive = new GUIStyle(GUI.skin.label);
                    lblActive.normal.textColor = Color.red;
                }
                return lblActive;
            }
        }

        public QHistory()
        {
            history = new List<Search>();
            index = -1;
            btnStyle = new GUIStyle(HighLogic.Skin.button);
            btnStyle.border = new RectOffset();
            btnStyle.padding = new RectOffset();
            Load();
        }

        public void Add(string t)
        {
            if (lastSearch == t || t == string.Empty)
            {
                return;
            }
            Search s = history.Get(t);
            if (s != null)
            {
                s.count += 1;
                s.date = DateTime.Now;
            }
            else
            {
                history.Add(new Search(t, 1, DateTime.Now));
            }
            history.SortBy(QSettings.Instance.historySortby);
            index = -1;
            lastSearch = t;
            Save();
        }

        void Save()
        {
            ConfigNode node = new ConfigNode();
            for (int i = history.Count - 1; i >= 0; i--)
            {
                Search s = history[i];
                ConfigNode n = node.AddNode(CFGNODE);
                n.AddValue("text", s.text);
                n.AddValue("count", s.count);
                n.AddValue("date", s.date.Ticks);
            }
            if (!Directory.Exists(CONFIG_DIR))
                Directory.CreateDirectory(CONFIG_DIR);
            node.Save(CONFIG_PATH);
        }

        void Load()
        {
            if (!File.Exists(CONFIG_PATH))
            {
                return;
            }
            ConfigNode nodeLoaded = ConfigNode.Load(CONFIG_PATH);
            ConfigNode[] nodes = nodeLoaded.GetNodes(CFGNODE);
            for (int i = nodes.Length - 1; i >= 0; --i)
            {
                ConfigNode node = nodes[i];
                string text = "";
                int count = 0;
                long date = 0;
                if (node.TryGetValue("text", ref text) && node.TryGetValue("count", ref count) && node.TryGetValue("date", ref date))
                {
                    history.Add(new Search(text, count, new DateTime(date)));
                }
            }
            history.SortBy(QSettings.Instance.historySortby);
        }

        public void Keys()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PreviousIndex();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NextIndex();
            }
            if (Input.GetKeyDown(KeyCode.Return) && index > -1)
            {
                if (HighLogic.LoadedSceneIsEditor)
                {
                    PartCategorizer.Instance.searchField.text = history[index].text;
                }
                else
                {
                    QSearch.Text = history[index].text;
                }
            }
        }

        public void Draw(int id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Localizer.Format("quicksearch_lastSearch"));
            GUILayout.FlexibleSpace();
            GUILayout.Label(QSettings.Instance.historySortby == (int)SortBy.COUNT ? Localizer.Format("quicksearch_count") : Localizer.Format("quicksearch_date"));
            GUILayout.EndHorizontal();
            for (int i = 0, count = history.Count; i < count; i++)
            {
                if (i >= QSettings.Instance.historyIndex)
                {
                    break;
                }
                GUILayout.BeginHorizontal();
                Search s = history[i];
                if (GUILayout.Button(QUtils.Texture.Search, btnStyle, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    if (HighLogic.LoadedSceneIsEditor)
                    {
                        PartCategorizer.Instance.searchField.text = s.text;
                        if (QSettings.Instance.enableEnterToSearch)
                        {
                            QSearch.Text = s.text; // force search if enter to search is set.
                        }
                    }
                    else
                    {
                        QRnD.Instance.Text = s.text;
                        GUIUtility.keyboardControl = 0;
                    }
                }
                GUIStyle st = index == i ? LblActive : GUI.skin.label;
                GUILayout.Label(s.text, st);
                GUILayout.FlexibleSpace();
                GUILayout.Label(QSettings.Instance.historySortby == (int)SortBy.COUNT ? s.count.ToString() : s.getDate(), st);
                GUILayout.EndHorizontal();
            }
        }

        public void NextIndex()
        {
            if (index >= history.Count - 1 || index >= QSettings.Instance.historyIndex - 1)
            {
                index = -1;
            }
            index++;
        }

        public void PreviousIndex()
        {
            if (index <= 0)
            {
                index = history.Count > QSettings.Instance.historyIndex ? QSettings.Instance.historyIndex : history.Count;
            }
            index--;
        }

        public class Search
        {
            public string text;
            public int count;
            public DateTime date;

            public Search(string text, int count, DateTime date)
            {
                this.text = text;
                this.count = count;
                this.date = date;
            }

            public string getDate()
            {
                string s = string.Empty;
                DateTime today = DateTime.Today;
                if (date.ToLongDateString() == today.ToLongDateString())
                {
                    s = date.ToLongTimeString();
                }
                else if (date.ToLongDateString() == today.AddDays(-1).ToLongDateString())
                {
                    s = "Yesterday";
                }
                else
                {
                    s = date.ToShortDateString();
                }
                return s;
            }
        }
    }
}