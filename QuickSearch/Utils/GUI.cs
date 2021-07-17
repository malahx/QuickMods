namespace QuickSearch {
    static class QGUI {

        internal static void Lock(bool activate, ControlTypes Ctrl) {
            if (HighLogic.LoadedSceneIsEditor) {
                if (activate) {
                    if (InputLockManager.GetControlLock("EditorLock" + RegisterToolbar.MOD) == ControlTypes.None) {
                        EditorLogic.fetch.Lock(true, true, true, "EditorLock" + RegisterToolbar.MOD);
                    }
                } else {
                    if (InputLockManager.GetControlLock("EditorLock" + RegisterToolbar.MOD) != ControlTypes.None) {
                        EditorLogic.fetch.Unlock("EditorLock" + RegisterToolbar.MOD);
                    }
                }
            }
            if (activate) {
                if (InputLockManager.GetControlLock("Lock" + RegisterToolbar.MOD) == ControlTypes.None) {
                    InputLockManager.SetControlLock(Ctrl, "Lock" + RegisterToolbar.MOD);
                }
                return;
            }
            if (InputLockManager.GetControlLock("Lock" + RegisterToolbar.MOD) != ControlTypes.None) {
                InputLockManager.RemoveControlLock("Lock" + RegisterToolbar.MOD);
            }
        }

        internal static void Lock(bool activate) {
            Lock(activate, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
        }
    }
}