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
using UnityEngine;

namespace QuickHide
{

    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public partial class QStockToolbar : QuickHide { }

    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
    public partial class QHide : QuickHide { }

    public class QuickHide : MonoBehaviour
    {
        internal static string FileConfig;

        internal static void Log(string String, string Title = null, bool force = false)
        {
            if (!force)
            {
                if (!QSettings.Instance.Debug)
                {
                    return;
                }
            }
            if (Title == null)
            {
                Title = RegisterToolbar.MOD;
            }
            else
            {
                Title = string.Format("{0}({1})", RegisterToolbar.MOD, Title);
            }
            Debug.Log(string.Format("{0}[{1}]: {2}", Title, RegisterToolbar.VERSION, String));
        }
        internal static void Warning(string String, string Title = null)
        {
            if (Title == null)
            {
                Title = RegisterToolbar.MOD;
            }
            else
            {
                Title = string.Format("{0}({1})", RegisterToolbar.MOD, Title);
            }
            Debug.LogWarning(string.Format("{0}[{1}]: {2}", Title, RegisterToolbar.VERSION, String));
        }

        protected virtual void Awake()
        {
            Log("Awake");
            FileConfig = RegisterToolbar.PATH + "/Config.txt";
            Debug.Log("QH.Awake, PATH: " + RegisterToolbar.PATH);
        }

        protected virtual void Start()
        {
            Log("Start");
        }

        protected virtual void OnDestroy()
        {
            Log("OnDestroy");
        }
    }
}