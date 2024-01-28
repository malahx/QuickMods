/* 
QuickScience
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
using QuickScience.Utils;
using QuickScience.Toolbar;
using UnityEngine;

namespace QuickScience {

    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class QuickScience : MonoBehaviour {

        internal static QuickScience Instance;
        [KSPField(isPersistant = true)] internal static QBlizzy BlizzyToolbar;

        void Awake() {
            if (Instance != null) {
                QDebug.Log("Destroy, already exists!");
                Destroy(this);
                return;
            }
            Instance = this;
            if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzy();
            QDebug.Log("Awake");
        }

        void Start() {
            if (BlizzyToolbar != null) BlizzyToolbar.Init();
            QDebug.Log("Start");
        }

        void OnDestroy() {
            if (BlizzyToolbar != null) BlizzyToolbar.Destroy();
            QDebug.Log("OnDestroy");
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