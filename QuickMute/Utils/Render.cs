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

using KSP.UI;

namespace QuickMute.QUtils {
    static class QRender {

        internal static void Lock(bool activate, ControlTypes Ctrl) {
            if (HighLogic.LoadedSceneIsEditor) {
                if (activate) {
                    if (InputLockManager.GetControlLock("EditorLock" + QVars.MOD) == ControlTypes.None) {
                        EditorLogic.fetch.Lock(true, true, true, "EditorLock" + QVars.MOD);
                    }
                } else {
                    if (InputLockManager.GetControlLock("EditorLock" + QVars.MOD) != ControlTypes.None) {
                        EditorLogic.fetch.Unlock("EditorLock" + QVars.MOD);
                    }
                }
            }
            if (activate) {
                if (InputLockManager.GetControlLock("Lock" + QVars.MOD) == ControlTypes.None) {
                    InputLockManager.SetControlLock(Ctrl, "Lock" + QVars.MOD);
                }
                return;
            }
            if (InputLockManager.GetControlLock("Lock" + QVars.MOD) != ControlTypes.None) {
                InputLockManager.RemoveControlLock("Lock" + QVars.MOD);
            }
        }

        internal static void Lock(bool activate) {
            Lock(activate, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
        }

        internal static bool isHide {
            get {
                return UIMasterController.Instance.uiCamera != null ? !UIMasterController.Instance.uiCamera.enabled : true &&
                    !UIMasterController.Instance.mainCanvas.enabled &&
                    !UIMasterController.Instance.appCanvas.enabled &&
                    !UIMasterController.Instance.actionCanvas.enabled &&
                    !UIMasterController.Instance.dialogCanvas.enabled &&
                    !UIMasterController.Instance.tooltipCanvas.enabled;
            }
        }
    }
}