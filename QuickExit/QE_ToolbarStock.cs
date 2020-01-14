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

using KSP.UI.Screens;
using UnityEngine;

using ToolbarControl_NS;

namespace QuickExit {
	public partial class QStockToolbar {


		ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.TRACKSTATION | ApplicationLauncher.AppScenes.VAB;
        internal static string TexturePath;

		void OnClick() { 
			QExit.Instance.Dialog ();
			Log ("OnClick", "QStockToolbar");
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
			DontDestroyOnLoad (this);
            Init();
			Log ("Awake", "QStockToolbar");
		}

		protected override void Start() {
			Log ("Start", "QStockToolbar");
		}
			

		void AppLauncherDestroyed() {
			Destroy ();
			Log ("AppLauncherDestroyed", "QStockToolbar");
		}

		protected override void OnDestroy() {

			Log ("OnDestroy", "QStockToolbar");
		}
        internal const string MODID = "QuickExit_NS";
        internal const string MODNAME = "QuickExit";
        void Init() {
            if (toolbarControl == null)
            {

                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(OnClick, null,
                    AppScenes,
                    MODID,
                    "quickExitButton",
					"QuickMods/" + TexturePath,
				   "QuickMods/" + QuickExit.relativePath + "/Textures/BlizzyToolBar",
                    MODNAME
                );

            }
            Log ("Init", "QStockToolbar");
		}

		void Destroy() {

            if (toolbarControl != null)
            {
                toolbarControl.OnDestroy();
                Destroy(toolbarControl);
                toolbarControl = null;
            }
            Log ("Destroy", "QStockToolbar");
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