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

using System.IO;
using System.Reflection;
using QuickSearch.QUtils;
using QuickSearch.Toolbar;
using UnityEngine;

namespace QuickSearch
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public partial class QRnD : QuickSearch { }

    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public partial class QEditor : QuickSearch { }

    public partial class QuickSearch : MonoBehaviour
    {

        public static string VERSION;
        public static string MOD = "";
        public static string relativePath;
        public static string PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../";        
        internal static string FileConfig = QuickSearch.PATH + "/Config.txt";


        protected virtual void Awake()
        {
            TextField = new GUIStyle(HighLogic.Skin.textField);
            TextField.stretchWidth = true;
            TextField.stretchHeight = true;
            TextField.alignment = TextAnchor.MiddleCenter;

            VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
            MOD = Assembly.GetExecutingAssembly().GetName().Name;
            relativePath =  MOD;
            PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../" ;
            FileConfig = PATH + "/Config.txt";
            Debug.Log("QS.Awake, PATH: " + PATH);
            QDebug.Log("Awake");
        }

        protected virtual void Start()
        {
            //if (BlizzyToolbar != null) BlizzyToolbar.Init ();
            QDebug.Log("Start");
        }

        protected virtual void OnDestroy()
        {
            //if (BlizzyToolbar != null) BlizzyToolbar.Destroy ();
            QDebug.Log("OnDestroy");
        }

        protected virtual void FixedUpdate()
        {
            if (!WindowHistory)
            {
                return;
            }
            QHistory.Instance.Keys();
        }
    }
}