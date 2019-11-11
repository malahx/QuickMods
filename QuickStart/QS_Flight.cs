/* 
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

using QuickStart.QUtils;

using KSP.UI.Screens;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using KSP.Localization;


namespace QuickStart
{
    public partial class QFlight
    {

        public static QFlight Instance
        {
            get;
            private set;
        }

        void Awake()
        {
            if (QLoading.Ended)
            {
                QDebug.Warning("Reload? Destroy.", "QLoading");
                Destroy(this);
                return;
            }
            if (!QSettings.Instance.enablePauseOnFlight || !QSettings.Instance.Enabled || QSettings.Instance.gameScene != (int)GameScenes.FLIGHT)
            {
                QDebug.Log("No need to keep it loaded.", "QFlight");
                QLoading.Ended = true;
                Destroy(this);
                return;
            }
            if (Instance != null)
            {
                QDebug.Warning("There's already an Instance", "QFlight");
                Destroy(this);
                return;
            }
            Instance = this;

            GameEvents.onFlightReady.Add(OnFlightReady);

            QDebug.Log("Awake", "QFlight");
        }
        int initted = 0;
        bool flightReady = false;
        void FixedUpdate()
        {
            if (flightReady)
            {
                initted++;
                var p = Planetarium.GetUniversalTime();
            }
            // Game seems to requier a bit more than 2 seconds (50 tics per second) before the
            // scene is ready
            if (initted >= 110 && flightReady)
            {
                PauseMenu.Display();

                QDebug.Log("FixedUpdate", "QFlight");
                QLoading.Ended = true;
                Destroy(this);
            }
        }

        void Start()
        {
            QDebug.Log("Start", "QFlight");
        }

        void OnDestroy()
        {
            GameEvents.onFlightReady.Remove(OnFlightReady);

            QDebug.Log("OnDestroy", "QFlight");
            Destroy(this);
        }

        void OnFlightReady()
        {
            flightReady = true;
        }
    }
}