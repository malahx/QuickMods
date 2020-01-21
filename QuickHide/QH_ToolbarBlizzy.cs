/* 
QuickHide
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

using System.IO;
using System.Reflection;
using KSP.Localization;
using UnityEngine;

namespace QuickHide
{
    public class QBlizzyToolbar
    {

        internal bool Enabled
        {
            get
            {
                return QSettings.Instance.BlizzyToolBar;
            }
        }
        public static string TexturePathShow { get { return RegisterToolbar.relativePath + "/Textures/BlizzyToolBar_Show"; } }
        public static string TexturePathHide { get { return RegisterToolbar.relativePath + "/Textures/BlizzyToolBar_Hide"; } }
        public static string TexturePathConf { get { return RegisterToolbar.relativePath + "/Textures/BlizzyToolBar"; } }

        public static string TexturePath { get { return (QSettings.Instance.isHidden ? TexturePathShow : TexturePathHide); } }

        void OnClick(ClickEvent e)
        {
            if (e.MouseButton == 1)
                QHide.Instance.Settings();
           else
                QHide.Instance.HideMods();
        }

        IButton Button;
        IButton ButtonConf;

        internal static bool isAvailable
        {
            get
            {
                return ToolbarManager.ToolbarAvailable && ToolbarManager.Instance != null;
            }
        }

        internal bool isActive
        {
            get
            {
                return Button != null && isAvailable;
            }
        }

        internal void Start()
        {
            if (!HighLogic.LoadedSceneIsGame || !isAvailable || !Enabled)
            {
                return;
            }
            //RegisterToolbar.relativePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../";

            if (Button == null)
            {
                Button = ToolbarManager.Instance.add(RegisterToolbar.MOD, RegisterToolbar.MOD);
                Button.TexturePath = TexturePath;
                Button.ToolTip = (QSettings.Instance.isHidden ? RegisterToolbar.MOD + ": " + Localizer.Format("quickhide_show") : RegisterToolbar.MOD + ": " + Localizer.Format("quickhide_hide"));
                Button.OnClick += OnClick;
            }
            if (ButtonConf == null)
            {
                ButtonConf = ToolbarManager.Instance.add(RegisterToolbar.MOD + "Conf", RegisterToolbar.MOD + "Conf");
                ButtonConf.TexturePath = TexturePathConf;
                ButtonConf.ToolTip = RegisterToolbar.MOD + ": " + Localizer.Format("quickhide_settings");
                ButtonConf.OnClick += (e) => QHide.Instance.Settings();
            }
        }


        internal void OnDestroy()
        {
            if (!isAvailable)
            {
                return;
            }
            if (Button != null)
            {
                Button.Destroy();
                Button = null;
            }
            if (ButtonConf != null)
            {
                ButtonConf.Destroy();
                ButtonConf = null;
            }
        }

        internal void Reset()
        {
            if (Enabled)
            {
                Start();
            }
            else
            {
                OnDestroy();
            }
        }

        internal void Refresh()
        {
            if (isActive)
            {
                Button.TexturePath = TexturePath;
                Button.ToolTip = (QSettings.Instance.isHidden ? RegisterToolbar.MOD + ": " + Localizer.Format("quickhide_show") : RegisterToolbar.MOD + ": " + Localizer.Format("quickhide_hide"));
            }
        }
    }
}