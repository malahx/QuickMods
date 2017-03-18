namespace QuickSearch {
    static class QGUI {

        internal static void Lock(bool activate, ControlTypes Ctrl) {
            if (HighLogic.LoadedSceneIsEditor) {
                if (activate) {
                    if (InputLockManager.GetControlLock("EditorLock" + QuickSearch.MOD) == ControlTypes.None) {
                        EditorLogic.fetch.Lock(true, true, true, "EditorLock" + QuickSearch.MOD);
                    }
                } else {
                    if (InputLockManager.GetControlLock("EditorLock" + QuickSearch.MOD) != ControlTypes.None) {
                        EditorLogic.fetch.Unlock("EditorLock" + QuickSearch.MOD);
                    }
                }
            }
            if (activate) {
                if (InputLockManager.GetControlLock("Lock" + QuickSearch.MOD) == ControlTypes.None) {
                    InputLockManager.SetControlLock(Ctrl, "Lock" + QuickSearch.MOD);
                }
                return;
            }
            if (InputLockManager.GetControlLock("Lock" + QuickSearch.MOD) != ControlTypes.None) {
                InputLockManager.RemoveControlLock("Lock" + QuickSearch.MOD);
            }
        }

        internal static void Lock(bool activate) {
            Lock(activate, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
        }
    }
}