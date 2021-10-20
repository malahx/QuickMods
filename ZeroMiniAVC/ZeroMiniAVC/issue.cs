// 
//     Copyright (C) 2014 CYBUTEK
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

#region Using Directives

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#endregion

namespace ZeroMiniAVC
{
    public class IssueGui : MonoBehaviour
    {
        #region Fields


        private GUIStyle boxStyle;
        private GUIStyle buttonStyle;
        private bool hasCentred;
        private GUIStyle labelStyle;
        private GUIStyle messageStyle;
        private GUIStyle nameLabelStyle;
        private GUIStyle nameTitleStyle;
        private Rect position = new Rect(Screen.width, Screen.height, Screen.width / 2, Screen.height / 2);
        private GUIStyle titleStyle;
        //private bool isInitialised = false;


        #endregion

        #region Methods: protected

        protected void Awake()
        {
            try
            {
                DontDestroyOnLoad(this);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            Debug.Log("IssueGui was created.");
        }

        protected void OnDestroy()
        {
            Debug.Log("IssueGui was destroyed.");
        }

        protected void OnGUI()
        {
            try
            {
                this.position = GUILayout.Window(this.GetInstanceID(), this.position, this.Window, "Zero Mini AVC - Duplicate DLL Monitor", HighLogic.Skin.window);
                this.CentreWindow();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        protected void Start()
        {
            try
            {
                this.InitialiseStyles();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        #endregion

        #region Methods: private

        private void CentreWindow()
        {
            if (this.hasCentred || !(this.position.width > 0) || !(this.position.height > 0))
            {
                return;
            }
            this.position.center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            this.hasCentred = true;
        }




        private void DrawUpdateHeadings()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" ", GUILayout.Width(90));
            GUILayout.Label("DLL", this.nameTitleStyle, GUILayout.Width(250.0f));
            GUILayout.Label("DLL Path", this.titleStyle, GUILayout.Width(100.0f));
            GUILayout.EndHorizontal();
        }

        string lastFileName = "";
        private void DrawUpdateInformation(string path)
        {
            if (Path.GetFileName(path) == "KatLib.dll")
            {
                if (path.Contains("KXAPI"))
                    GUILayout.Label("Keep: ", GUILayout.Width(90));
                else
                    GUILayout.Label(" ", GUILayout.Width(90));
            }
            else
            {
                if (lastFileName != Path.GetFileName(path))
                {
                    GUILayout.Label("Keep: ", GUILayout.Width(90));
                    lastFileName = Path.GetFileName(path);
                }
                else
                    GUILayout.Label(" ", GUILayout.Width(90));
            }
            GUILayout.Label(Path.GetFileName(path), this.nameLabelStyle, GUILayout.Width(250.0f));
            GUILayout.Label(path, this.labelStyle);
        }

        Vector2 scroll;
        private void DrawUpdateIssues()
        {
            lastFileName = "";
            GUILayout.BeginVertical();
            GUILayout.Label("This is a  list of duplicate DLLs in the game.  The game will not work properly until the duplicates have been removed.");
            GUILayout.Label("This mod cannot determine which ones should be removed!!!");
            GUILayout.Label("It is suggested that you keep the first of each and delete the rest");
            GUILayout.EndVertical();
            GUILayout.BeginVertical(this.boxStyle);
            this.DrawUpdateHeadings();
            scroll = GUILayout.BeginScrollView(scroll);
            foreach (var addon in ZeroMiniAVC.duplicateDlls)
            {
                GUILayout.BeginHorizontal();
                this.DrawUpdateInformation(addon);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void InitialiseStyles()
        {
            //if (Configuration.UseKspSkin)
            //{
            //    GUI.skin = HighLogic.Skin;
            //}
            this.boxStyle = new GUIStyle(HighLogic.Skin.box)
            {
                padding = new RectOffset(10, 10, 5, 5)
            };

            this.nameTitleStyle = new GUIStyle(HighLogic.Skin.label)
            {
                normal =
                {
                    textColor = Color.white
                },
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                stretchWidth = true
            };

            this.titleStyle = new GUIStyle(HighLogic.Skin.label)
            {
                normal =
                {
                    textColor = Color.white
                },
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };

            this.nameLabelStyle = new GUIStyle(HighLogic.Skin.label)
            {
                fixedHeight = 25.0f,
                alignment = TextAnchor.MiddleLeft,
                stretchWidth = true
            };

            this.labelStyle = new GUIStyle(HighLogic.Skin.label)
            {
                fixedHeight = 25.0f,
                alignment = TextAnchor.MiddleCenter,
            };

            this.messageStyle = new GUIStyle(HighLogic.Skin.label)
            {
                stretchWidth = true
            };

            this.buttonStyle = new GUIStyle(HighLogic.Skin.button)
            {
                normal =
                {
                    textColor = Color.white
                }
            };

            //isInitialised = true;
        }

        private void Window(int id)
        {
            try
            {
                if (ZeroMiniAVC.duplicateDlls.Count > 0)
                {
                    this.DrawUpdateIssues();
                }
                if (GUILayout.Button("CLOSE", this.buttonStyle))
                {
                    Destroy(this);
                }
                GUI.DragWindow();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        #endregion
    }
}