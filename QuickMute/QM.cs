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
using QuickMute.Object;
using QuickMute.Toolbar;
using UnityEngine;

namespace QuickMute {
	
	[KSPAddon (KSPAddon.Startup.EveryScene, true)]
	public class QuickMute : MonoBehaviour {

        internal static QuickMute Instance;
        //[KSPField(isPersistant = true)] internal static QBlizzy BlizzyToolbar;
        internal QGui gui;
        internal QVolume volume;
        QKey qKey;
        QLevel level;

        internal bool mouseIsHover {
            get {
                return gui.isHovering || level.mouseIsHover;
            }
        }

        void Awake() {
            if (Instance != null) {
                QDebug.Log("Destroy, already exists!");
                Destroy(this);
                return;
            }
            Instance = this;

            GameEvents.onVesselGoOffRails.Add(OnVesselGoOffRails);
            if (System.Math.Abs(GameSettings.MASTER_VOLUME) < float.Epsilon) {
                GameSettings.MASTER_VOLUME = QSettings.Instance.Master;
                GameSettings.SaveSettings();
            }
            volume = new QVolume(GameSettings.MASTER_VOLUME, QSettings.Instance.Muted);
            level = new QLevel(volume);
            qKey = new QKey();
            gui = new QGui(qKey, level);
            DontDestroyOnLoad(this);
            QDebug.Log("Awake");
        }

        IEnumerator Wait(int seconds) {
            if (gui.draw) {
                yield break;
            }
            gui.draw = true;
            yield return new WaitForSecondsRealtime(seconds);
            gui.draw = false;
            QDebug.Log("Wait");
        }

        void Start() {
            QDebug.Log("Start");
        }

        void OnVesselGoOffRails(Vessel vessel) {
            QMute.Verify();
            QDebug.Log("OnVesselGoOffRails");
        }

        void Update() {
            qKey.Update();
            if (mouseIsHover) {
                if (QSettings.Instance.ScrollLevel && System.Math.Abs(Input.GetAxis("Mouse ScrollWheel")) > float.Epsilon) {
                    float scroll = Input.GetAxis("Mouse ScrollWheel");
                    volume.Master += scroll;
                }
                if (!QRender.isLock) {
                    QRender.Lock(true);
                }
            } else if (QRender.isLock) {
                QRender.Lock(false);
            }
        }

        void OnGUI() {
            gui.Render();
        }

        void OnApplicationQuit() {
            volume.Restore();
            QDebug.Log("OnApplicationQuit");
        }

        public void Mute() {
            Mute(!QSettings.Instance.Muted);
        }

        void Mute(bool mute) {
            volume.isMute = mute;
            Refresh();
            if (QSettings.Instance.MuteIcon) {
                StartCoroutine(Wait(5));
            }
            QDebug.Log("Mute");
        }

        public void Refresh() {
            //if (BlizzyToolbar != null) {
            //    BlizzyToolbar.Refresh();
           // }
            if (QStock.Instance != null) {
                QStock.Instance.Refresh();
            }
            QDebug.Log("Refresh");
        }
	}
}