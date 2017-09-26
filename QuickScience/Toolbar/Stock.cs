/* 
QuickMute
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
using QuickMute.Object;
using UnityEngine;

namespace QuickMute.Toolbar {

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class QStock : MonoBehaviour {

        static bool CanUseIt {
            get {
                return HighLogic.LoadedSceneIsGame;
            }
        }

        internal static bool isAvailable {
            get {
                return ApplicationLauncher.Ready && ApplicationLauncher.Instance != null;
            }
        }

        internal static QStock Instance {
            get;
            private set;
        }

        internal static bool IsHovering() {
            return Instance != null && Instance.isActive && Instance.isHovering;
        }
        
        ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.TRACKSTATION | ApplicationLauncher.AppScenes.VAB;
        ApplicationLauncherButton appLauncherButton;

        internal bool isActive {
            get {
                return appLauncherButton != null && isAvailable;
            }
        }

        internal bool isTrue {
            get {
                return isActive && appLauncherButton.toggleButton.CurrentState == UIRadioButton.State.True;
            }
        }

        internal bool isFalse {
            get {
                return isActive && appLauncherButton.toggleButton.CurrentState == UIRadioButton.State.False;
            }
        }

        internal bool isHovering {
            get {
                return appLauncherButton != null && appLauncherButton.IsHovering;
            }
        }

        internal Rect Position {
            get {
                if (appLauncherButton == null || !isAvailable) {
                    return new Rect();
                }
                Camera _camera = UIMainCamera.Camera;
                Vector3 _pos = _camera.WorldToScreenPoint(appLauncherButton.GetAnchorUL());
                return new Rect(_pos.x, Screen.height - _pos.y, 41, 41);
            }
        }

        void OnClick() {
            QuickMute.Instance.Mute();
        }

        void OnHover() {
            QuickMute.Instance.gui.level.OnHover();
        }

        void OnHide() {
            QuickMute.Instance.gui.level.Hide();
        }

        void Awake() {
            if (Instance != null) {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(Instance);
            GameEvents.onGUIApplicationLauncherReady.Add(AppLauncherReady);
            GameEvents.onGUIApplicationLauncherDestroyed.Add(AppLauncherDestroyed);
            GameEvents.onLevelWasLoadedGUIReady.Add(AppLauncherDestroyed);
            QDebug.Log("Awake", "QStockToolbar");
        }

        void AppLauncherReady() {
            if (!QSettings.Instance.StockToolBar) {
                return;
            }
            Init();
        }

        void AppLauncherDestroyed(GameScenes gameScene) {
            if (CanUseIt) {
                return;
            }
            Destroy();
        }

        void AppLauncherDestroyed() {
            Destroy();
        }

        void OnDestroy() {
            GameEvents.onGUIApplicationLauncherReady.Remove(AppLauncherReady);
            GameEvents.onGUIApplicationLauncherDestroyed.Remove(AppLauncherDestroyed);
            GameEvents.onLevelWasLoadedGUIReady.Remove(AppLauncherDestroyed);
            QDebug.Log("OnDestroy", "QStockToolbar");
        }

        void Init() {
            if (!isAvailable || !CanUseIt) {
                return;
            }
            if (appLauncherButton == null) {
                appLauncherButton = ApplicationLauncher.Instance.AddModApplication(OnClick, OnClick, OnHover, null, null, OnHide, AppScenes, QTexture.StockTexture);
                appLauncherButton.onRightClick = delegate { QuickMute.Instance.gui.Settings(); };
            }
            QDebug.Log("Init", "QStockToolbar");
        }

        void Destroy() {
            if (appLauncherButton != null) {
                ApplicationLauncher.Instance.RemoveModApplication(appLauncherButton);
                appLauncherButton = null;
            }
            QDebug.Log("Destroy", "QStockToolbar");
        }

        internal void Set(bool SetTrue, bool force = false) {
            if (!isAvailable) {
                return;
            }
            if (appLauncherButton != null) {
                if (SetTrue) {
                    if (appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.False) {
                        appLauncherButton.SetTrue(force);
                    }
                } else {
                    if (appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True) {
                        appLauncherButton.SetFalse(force);
                    }
                }
            }
            QDebug.Log("Set", "QStockToolbar");
        }

        internal void Reset() {
            if (appLauncherButton != null) {
                Set(false);
                if (!QSettings.Instance.StockToolBar) {
                    Destroy();
                }
            }
            if (QSettings.Instance.StockToolBar) {
                Init();
            }
            QDebug.Log("Reset", "QStockToolbar");
        }

        internal void Refresh() {
            if (appLauncherButton != null) {
                if (QSettings.Instance.Muted && isFalse) {
                    appLauncherButton.SetTrue(false);
                }
                if (!QSettings.Instance.Muted && isTrue) {
                    appLauncherButton.SetFalse(false);
                }
                appLauncherButton.SetTexture(QTexture.StockTexture);
            }
            QDebug.Log("Refresh", "QStockToolbar");
        }
    }
}