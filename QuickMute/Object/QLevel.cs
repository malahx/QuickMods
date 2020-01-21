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

using System;
using KSP.UI.Screens;
//using QuickMute.Toolbar;
using UnityEngine;

using ClickThroughFix;

namespace QuickMute.Object
{
    public class QLevel
    {

        bool keep = false;
        DateTime keepDate = DateTime.Now;
        readonly QVolume volume;
        readonly GUIStyle styleWindow;
        readonly GUIStyle styleSlider;
        readonly GUIStyle styleThumb;

        bool _window = false;
        internal bool Window
        {
            get
            {
                return _window || keep;
            }
            set
            {
                if (!value)
                {
                    //if (QStock.Instance != null && QStock.Instance.isActive && window)
                    if (QStock.Instance != null && _window)
                    {
                        keep = true;
                        keepDate = DateTime.Now;
                    }
                    dim.position = Vector2.zero;
                }
                _window = value;
            }
        }

        Rect dim = new Rect();
        internal Rect Dim
        {
            get
            {
                if (dim.IsEmpty())
                {
                    dim.x = (Screen.width - dim.width) / 2;
                    dim.y = (Screen.height - dim.height) / 2;
                    if (QStock.Instance != null/*  && QStock.Instance.isActive */)
                    {
                        Rect activeButtonPos = QStock.Instance.Position;
                        if (ApplicationLauncher.Instance.IsPositionedAtTop)
                        {
                            dim.x = activeButtonPos.x - dim.width;
                            dim.y = activeButtonPos.y - activeButtonPos.width / 2;
                        }
                        else
                        {
                            dim.x = activeButtonPos.x + activeButtonPos.width / 2 - dim.width / 2;
                            dim.y = activeButtonPos.y - dim.height;
                        }
                    }

                    QDebug.Log("Dim init", "QLevel");
                }
                return dim;
            }
            set
            {
                dim = value;
            }
        }

        internal bool mouseIsHover
        {
            get
            {
                return isHovering || QStock.IsHovering();
            }
        }

        internal bool isHovering
        {
            get
            {
                return Window && !Dim.IsEmpty() && Dim.Contains(Mouse.screenPos);
            }
        }

        public QLevel(QVolume volume)
        {
            styleWindow = new GUIStyle(HighLogic.Skin.window);
            styleWindow.padding = new RectOffset();
            styleSlider = new GUIStyle(HighLogic.Skin.verticalSlider);
            styleSlider.padding = new RectOffset();
            styleThumb = new GUIStyle(HighLogic.Skin.verticalSliderThumb);
            this.volume = volume;
            QDebug.Log("Init", "QLevel");
        }

        internal void OnHover()
        {
            if (!QSettings.Instance.Level)
            {
                return;
            }
            Show();
            QDebug.Log("OnHover", "QLevel");
        }

        internal void Hide()
        {
            Hide(false);
        }

        internal void Hide(bool force)
        {
            Window = false;
            if (force)
            {
                keep = false;
            }
            QSettings.Instance.Save();
            QuickMute.Instance.Refresh();
            QDebug.Log("Hide force: " + force, "QLevel");
        }

        internal void Show()
        {
            if (_window)
            {
                return;
            }
            Window = true;
            QDebug.Log("Show", "QLevel");
        }

        public void Render()
        {
            if (!Window)
            {
                return;
            }
            Dim = ClickThruBlocker.GUILayoutWindow(1584665, Dim, Draw, "Level", styleWindow);

            if (keep)
            {
                if ((DateTime.Now - keepDate).TotalSeconds > 1)
                {
                    keep = false;
                }
                if (mouseIsHover)
                {
                    keepDate = DateTime.Now;
                }
            }
            else if (!mouseIsHover)
            {
                Hide();
            }
        }

        void Draw(int id)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);
            volume.Master = GUILayout.VerticalSlider(GameSettings.MASTER_VOLUME, 1, 0, styleSlider, styleThumb);
            GUILayout.Space(12);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}