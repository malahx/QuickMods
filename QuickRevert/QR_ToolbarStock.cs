/* 
QuickRevert
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
namespace QuickRevert {
	public partial class QStockToolbar  {

        internal ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.SPACECENTER;
		static string TexturePath = relativePath + "/Textures/StockToolBar";

		void OnClick() { 
			QGUI.Instance.Settings ();
		}

        ToolbarControl toolbarControl = null;


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
			//DontDestroyOnLoad (Instance);

            Init();

			Log ("Awake", "QStockToolbar");
		}

		protected override void Start() {
			Log ("Start", "QStockToolbar");
		}
			
        void OnDestory()
        {
            toolbarControl.OnDestroy();
            Destroy(toolbarControl);
        }


        internal const string MODID = "QuickRevert_NS";
        internal const string MODNAME = "QuickRevert";
        internal void Init() {
            Debug.Log("QuickRevert.Init");

            if (toolbarControl == null)
            {
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(OnClick, OnClick,
                    AppScenes,
                    MODID,
                    "﻿quickRevertButton",
                    TexturePath,
                    QuickRevert.relativePath + "/Textures/BlizzyToolBar",
                    MODNAME
                );
                Debug.Log("QuickRevert.Init toolbar created");
            }
            Log ("Init", "QStockToolbar");
		}

		internal void Set(bool SetTrue, bool force = false) {

			if (toolbarControl != null) {
				if (SetTrue) {
					//if (appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.False) {
                        toolbarControl.SetTrue (force);
					//}
				} else {
					//if (appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True) {
                        toolbarControl.SetFalse (force);
					//}
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