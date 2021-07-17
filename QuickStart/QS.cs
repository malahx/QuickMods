﻿/* 
QuickStart
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

namespace QuickStart
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public partial class QLoading : MonoBehaviour { }

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public partial class QMainMenu : MonoBehaviour { }

    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public partial class QSpaceCenter : MonoBehaviour { }

    [KSPAddon(KSPAddon.Startup.Flight, true)]
    public partial class QFlight : MonoBehaviour { }

    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.EDITOR, GameScenes.TRACKSTATION, GameScenes.SPACECENTER)]
    public partial class QuickStart_Persistent : ScenarioModule { }

    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class QuickStart:MonoBehaviour
    {
        internal static string FileConfig;

        void Awake()
        {
            FileConfig = RegisterToolbar.PATH + "/Config.txt";
            Debug.Log("QuickStart.Awake, PATH: " + RegisterToolbar.PATH);
        }
    }
}
