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
using QuickMute.Object;
using UnityEngine;

namespace QuickMute {
	
	[KSPAddon (KSPAddon.Startup.EveryScene, false)]
	public class QuickMute : MonoBehaviour {

        internal static QuickMute Instance;
        [KSPField(isPersistant = true)] internal static QBlizzy BlizzyToolbar;
        internal static Gui gui = new Gui();
        internal Volume volume;

        Coroutine wait;

        void Awake() {
            Instance = this;
            if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzy();
            GameEvents.onVesselGoOffRails.Add(OnVesselGoOffRails);
            volume = new Volume(GameSettings.MASTER_VOLUME, QSettings.Instance.Muted);
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
            QDebug.Log("Start");
        }

        void OnDestroy() {
            if (BlizzyToolbar != null) BlizzyToolbar.Destroy();
            GameEvents.onVesselGoOffRails.Remove(OnVesselGoOffRails);
            volume.Restore();
            gui.level.Hide(true);
            QDebug.Log("OnDestroy");
        }

        void OnVesselGoOffRails(Vessel vessel) {
            QMute.Verify();
            QDebug.Log("OnVesselGoOffRails");
        }

        void Update() {
            if (QKey.SetKey != QKey.Key.None) {
                if (Event.current.isKey) {
                    KeyCode _key = Event.current.keyCode;
                    if (_key != KeyCode.None) {
                        QKey.SetCurrentKey(QKey.SetKey, _key);
                        QKey.SetKey = QKey.Key.None;
                    }
                }
                return;
            }
            if (Input.GetKeyDown(QSettings.Instance.KeyMute)) {
                Mute();
            }
            if (gui.level.Dim.Contains(Mouse.screenPos)) {
                if (QSettings.Instance.ScrollLevel && System.Math.Abs(Input.GetAxis("Mouse ScrollWheel")) > float.Epsilon) {
                    float scroll = Input.GetAxis("Mouse ScrollWheel");
                    volume.Master = Mathf.Clamp(volume.Master + scroll, 0 , 1);
                }
            }
        }

        void OnGUI() {
            gui.Render();
        }

        void OnApplicationQuit() {
            volume.Restore();
            GameSettings.SaveSettings();
            QSettings.Instance.Save();
            QDebug.Log("OnApplicationQuit");
        }

        public void Mute() {
            Mute(!QSettings.Instance.Muted);
        }

        void Mute(bool mute) {
            volume.isMute = mute;
            Refresh();
            if (QSettings.Instance.MuteIcon) {
                gui.draw = true;
                if (wait != null) {
                    StopCoroutine(wait);
                }
                wait = StartCoroutine(Wait(5));
            }
            QDebug.Log("Mute");
        }

        public void Refresh() {
            if (BlizzyToolbar != null) {
                BlizzyToolbar.Refresh();
            }
            if (QStock.Instance != null) {
                QStock.Instance.Refresh();
            }
            QDebug.Log("Refresh");
        }
	}
}