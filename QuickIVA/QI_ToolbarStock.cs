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

using KSP.UI.Screens;
using UnityEngine;

using ToolbarControl_NS;

namespace QuickIVA {
	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class QStockToolbar : QuickIVA {


		ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.SPACECENTER;
        internal static string TexturePath;

		void OnClick() { 
			QGUI.Settings ();
		}

        ToolbarControl toolbarControl;

		internal static QStockToolbar Instance {
			get;
			private set;
		}

		protected override void Awake() {
			if (Instance != null) {
				Destroy (this);
				return;
			}
			Instance = this;
            //DontDestroyOnLoad (this);
            Init();
			Log ("Awake", "QStockToolbar");
		}

        new void OnDestroy()
        {
            toolbarControl.OnDestroy();
            Destroy(toolbarControl);
        }

        internal const string MODID = "QuickIVA_NS";
        internal const string MODNAME = "QuickIVA";

        void Init() {

            if (toolbarControl == null)
            {
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(OnClick, OnClick,
                    AppScenes,
                    MODID,
                    "quickIVAButton",
					"QuickMods/" + TexturePath,
					"QuickMods/" + RegisterToolbar.relativePath + "/Textures/BlizzyToolBar",
                    MODNAME
                );
                Log("toolbarControl Init", "QStockToolbar");
            }

            Log("Init", "QStockToolbar");
		}

		internal void Set(bool SetTrue, bool force = false) {
			if (toolbarControl != null) {
				if (SetTrue) {
                    toolbarControl.SetTrue (force);
				} else {
                    toolbarControl.SetFalse (force);
				}
			}
			Log ("Set " + SetTrue + " force: " + force, "QStockToolbar");
		}

		internal void Reset() {
			if (toolbarControl != null) 
				Set (false);
            else
				Init ();
			
			Log ("Reset", "QStockToolbar");
		}
	}
}