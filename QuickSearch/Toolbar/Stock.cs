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

using KSP.UI.Screens;
using QuickSearch.QUtils;
using UnityEngine;

using ToolbarControl_NS;

namespace QuickSearch {
	
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	public class QStock : MonoBehaviour {

		static ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB;

		void OnClick() { 
			QuickSearch.instancedSettings();
		}
        
        ToolbarControl toolbarControl;

        public static QStock Instance {
			get;
			private set;
		}

		void Awake() {
			if (Instance != null) {
				Destroy (this);
				return;
			}
			Instance = this;
			DontDestroyOnLoad (Instance);

            Init();

            QDebug.Log ("Awake", "QStockToolbar");
		}

		void Start() { 
			QDebug.Log ("Start", "QStockToolbar");
		}


        internal const string MODID = "QuickSearch_NS";
        internal const string MODNAME = "QuickSearch";
        void Init() {

            if (toolbarControl == null)
            {
                if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode != Game.Modes.CAREER && HighLogic.CurrentGame.Mode != Game.Modes.SCIENCE_SANDBOX)
                {
                    AppScenes = ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB;
                    QDebug.Warning("Hide applauncher on the SpaceCenter", "QStockToolbar");
                }
                else
                {
                    AppScenes = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB;
                }
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(OnClick, OnClick,
                    AppScenes,
                    MODID,
                    "quickSearchButton",
					QUtils.Texture.STOCKTOOLBAR_PATH,
					QUtils.Texture.BLIZZY_PATH,
                    MODNAME
                );
            }
            QDebug.Log ("Init", "QStockToolbar");
		}

		internal void Set(bool SetTrue, bool force = false) {
			if (toolbarControl != null) {
				if (SetTrue) {
                        toolbarControl.SetTrue (force);
				} else {
                        toolbarControl.SetFalse (force);
				}
			}
			QDebug.Log ("Set: " + SetTrue + " force: " + force, "QStockToolbar");
		}

		internal void Reset() {
            if (toolbarControl != null)
            {
                Set(false);
            }
            else
				Init ();
			
			QDebug.Log ("Reset", "QStockToolbar");
		}

		internal static void ResetScenes() {
			if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER && HighLogic.CurrentGame.Mode != Game.Modes.SCIENCE_SANDBOX) {
				AppScenes = ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB;
				QDebug.Warning ("Hide applauncher on the SpaceCenter", "QStockToolbar");
			} else {
				AppScenes = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB;
			}
			if (QStock.Instance == null) {
				return;
			}
            if (QStock.Instance.toolbarControl != null)
            {
                QStock.Instance.toolbarControl.OnDestroy();
                Destroy(QStock.Instance.toolbarControl﻿﻿);
                QStock.Instance.toolbarControl﻿﻿ = null;
            }
            QStock.Instance.Init();

			QDebug.Log ("ResetScenes", "QStockToolbar");
		}
	}
}