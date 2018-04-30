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

using ToolbarControl_NS;

namespace QuickMute.Toolbar
{

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class QStock : MonoBehaviour
    {

        internal static QStock Instance
        {
            get;
            private set;
        }

        internal static bool IsHovering()
        {
            return Instance != null /* && Instance.isActive */ && Instance.isHovering;
        }

        ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.SPACECENTER |
            ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.TRACKSTATION | ApplicationLauncher.AppScenes.VAB;

        ToolbarControl toolbarControl;

        bool _isTrue = false;
        internal bool isTrue
        {
            get
            {
                return _isTrue;
            }
        }

        internal bool isFalse
        {
            get
            {
                return !_isTrue;
            }
        }

        internal bool isHovering
        {
            get
            {
                return _isHovering;
            }
        }

        internal Rect Position
        {
            get
            {

                var r = toolbarControl.Position;
                if (r == null)
                {
                    return new Rect();
                }
                return (Rect)r;
            }
        }

        void OnClick()
        {
            QuickMute.Instance.Mute();
        }
        bool _isHovering = false;
        void OnHover()
        {
            _isHovering = true;
        }
        void OnHoverOut()
        {
            _isHovering = false;
        }

        void OnHide()
        {
            QuickMute.Instance.gui.level.Hide();
        }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(Instance);
            Init();
            QDebug.Log("QStock.Awake", "QStockToolbar");
        }


        internal const string MODID = "QuickMute_NS";
        internal const string MODNAME = "QuickMute";

        void Init()
        {
            Debug.Log("QuickMute.Init");

            if (toolbarControl == null)
            {
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(OnClick, OnClick,
                    OnHover, OnHoverOut, null, OnHide,
                    AppScenes,
                     MODID,
                    "quickMuteButton",
                    QTexture.StockTexture,
                    QTexture.BlizzyTexturePath,
                    MODNAME
                );

            }
            QDebug.Log("QStock.Init", "QStockToolbar");
        }

        internal void Set(bool SetTrue, bool force = false)
        {

            if (toolbarControl != null)
            {
                if (SetTrue)
                {
                    toolbarControl.SetTrue(force);
                }
                else
                {
                    toolbarControl.SetFalse(force);
                }
            }
            QDebug.Log("QStock.Set", "QStockToolbar");
        }

        internal void Reset()
        {
            if (toolbarControl != null)
            {
                Set(false);
            }
            else
                Init();

            QDebug.Log("QStock.Reset", "QStockToolbar");
        }

        internal void Refresh()
        {
            if (toolbarControl != null)
            {
                if (QSettings.Instance.Muted && isFalse)
                {
                    toolbarControl.SetTrue(false);
                    _isTrue = true;
                }
                if (!QSettings.Instance.Muted && isTrue)
                {
                    toolbarControl.SetFalse(false);
                    _isTrue = false;
                }
                toolbarControl.SetTexture(QTexture.StockTexture, QTexture.BlizzyTexturePath);
            }
            QDebug.Log("QStock.Refresh", "QStockToolbar");
        }
    }
}