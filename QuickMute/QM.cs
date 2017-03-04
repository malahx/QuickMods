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

using System.Collections;
using QuickMute.QUtils;
using QuickMute.Toolbar;
using UnityEngine;

namespace QuickMute {
	
	[KSPAddon (KSPAddon.Startup.EveryScene, false)]
	public class QuickMute : MonoBehaviour {

        internal static QuickMute Instance;
        [KSPField(isPersistant = true)] internal static QBlizzy BlizzyToolbar;
        internal static QGUI gui;
        Coroutine wait;

        void Awake() {
            Instance = this;
            if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzy();
            GameEvents.onVesselGoOffRails.Add(OnVesselGoOffRails);
            gui = new QGUI();
            QDebug.Log("Awake");
        }

        IEnumerator Wait(int seconds) {
            yield return new WaitForSeconds(seconds);
            gui.draw = false;
            wait = null;
            QDebug.Log("Wait");
        }

        void Start() {
            if (BlizzyToolbar != null) BlizzyToolbar.Init();
            if (QSettings.Instance.Muted) {
                Mute(true);
            }
            QDebug.Log("Start");
        }

        void OnDestroy() {
            if (BlizzyToolbar != null) BlizzyToolbar.Destroy();
            GameEvents.onVesselGoOffRails.Remove(OnVesselGoOffRails);
            QDebug.Log("OnDestroy");
        }

        void OnVesselGoOffRails(Vessel vessel) {
            QMute.Verify();
            QDebug.Log("OnVesselGoOffRails");
        }

        void Update() {
            if (Input.GetKeyDown(QSettings.Instance.KeyMute)) {
                Mute();
            }
            if (QKey.SetKey == QKey.Key.None) {
                return;
            }
            if (Event.current.isKey) {
                KeyCode _key = Event.current.keyCode;
                if (_key != KeyCode.None) {
                    QKey.SetCurrentKey(QKey.SetKey, _key);
                    QKey.SetKey = QKey.Key.None;
                }
            }
        }

        void OnGUI() {
            gui.Render();
        }

        void OnApplicationQuit() {
            Mute(false);
            GameSettings.SaveSettings();
            QDebug.Log("OnApplicationQuit");
        }

        public void Mute() {
            Mute(!QSettings.Instance.Muted);
        }

        void Mute(bool mute) {
            QSettings.Instance.Muted = mute;
            if (BlizzyToolbar != null) {
                BlizzyToolbar.Refresh();
            }
            if (QStock.Instance != null) {
                QStock.Instance.Refresh();
            }
            QMute.refresh(mute);
            gui.draw = true;
            if (wait != null) {
                StopCoroutine(wait);
            }
            wait = StartCoroutine(Wait(5));
            QSettings.Instance.Save();
            QDebug.Log("Mute");
        }
	}
}