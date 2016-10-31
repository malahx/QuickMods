/* 
QuickHide
Copyright 2016 Malah

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

using KSP.UI.Screens;

namespace QuickHide {
	public class QMods {
		public QMods (ApplicationLauncherButton button) {
			appLauncherButton = button;
			AppRef = GetAppRef (appLauncherButton);
			ModName = GetModName (appLauncherButton);
			SaveCurrentAppScenes ();
			if (!QSettings.Instance.ModHasFirstConfig.Contains (ModName)) {
				CanBePin = true;
				CanBeHide = true;
				CanSetFalse = true;
				QSettings.Instance.ModHasFirstConfig.Add (ModName);
				QuickHide.Log ("Config set to default for the mod: " + AppRef, "QMods");
			}
			if (!isHidden && CanBeHide) {
				isHidden = QSettings.Instance.isHidden;
			}
		}
		ApplicationLauncherButton appLauncherButton;
		internal ApplicationLauncher.AppScenes AppScenesSaved = ApplicationLauncher.AppScenes.NEVER;
		internal string ModName {
			get;
			private set;
		}
		internal string AppRef {
			get;
			private set;
		}
		internal bool isActive {
			get {
				return appLauncherButton != null && QStockToolbar.isAvailable;
			}
		}
		internal bool isTrue {
			get {
				if (!isActive) {
					return false;
				}
				return appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True;
			}
		}
		internal bool isFalse {
			get {
				if (!isActive) {
					return false;
				}
				return appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.False;
			}
		}
		internal bool isEnabled {
			get {
				if (!isActive) {
					return false;
				}
				return appLauncherButton.IsEnabled;
			}
		}
		internal bool CanBePin {
			get {
				return QSettings.Instance.CanPin.Contains (ModName);
			}
			set {
				if (value) {
					if (!CanBePin) {
						QSettings.Instance.CanPin.Add (ModName);
					}
				} else {
					QSettings.Instance.CanPin.Remove (ModName);
				}
			}
		}
		internal bool CanBeHide {
			get {
				return QSettings.Instance.CanHide.Contains (ModName);
			}
			set {
				if (value) {
					if (!CanBeHide) {
						QSettings.Instance.CanHide.Add (ModName);
						if (QSettings.Instance.isHidden) {
							StoreAppScenes ();
						}
					}
				} else {
					QSettings.Instance.CanHide.Remove (ModName);
					if (QSettings.Instance.isHidden) {
						RestoreAppScenes ();
					}
				}
			}
		}
		internal bool CanSetFalse {
			get {
				return QSettings.Instance.CanSetFalse.Contains (ModName);
			}
			set {
				if (value) {
					if (!CanSetFalse) {
						QSettings.Instance.CanSetFalse.Add (ModName);
						if (QSettings.Instance.isHidden) {
							SetFalse ();
						}
					}
				} else {
					QSettings.Instance.CanSetFalse.Remove (ModName);
				}
			}
		}
		internal bool isHidden {
			get {
				if (!isActive) {
					return false;
				}
				return appLauncherButton.VisibleInScenes == ApplicationLauncher.AppScenes.NEVER;
			}
			set {
				if (CanBeHide) {
					if (value) {
						StoreAppScenes ();
					} else {
						RestoreAppScenes ();
					}
				}
			}
		}
		internal bool isSaved {
			get {
				return AppScenesSaved != ApplicationLauncher.AppScenes.NEVER;
			}
		}
		internal bool isStored {
			get {
				return AppScenesSaved == VisibleInScenes && isSaved;
			}
		}
		internal ApplicationLauncher.AppScenes VisibleInScenes {
			get {
				if (!isActive) {
					return ApplicationLauncher.AppScenes.NEVER;
				}
				return appLauncherButton.VisibleInScenes;
			}
		}
		internal bool isThisApp(ApplicationLauncherButton button) {
			if (!isActive) {
				return false;
			}
			return appLauncherButton == button;
		}
		internal static string GetModName(ApplicationLauncherButton button) {
			if (button == null) {
				return "None";
			}
			return button.onTrue.Method.Module.Assembly.GetName ().Name;
		}
		internal static string GetAppRef(ApplicationLauncherButton button) {
			if (button == null) {
				return "None";
			}
			return string.Format("{0} ({1}.{2})", GetModName(button), button.onTrue.Method.DeclaringType.FullName, button.onTrue.Method.Name);
		}
		internal void SaveCurrentAppScenes() {
			if (!isActive || isHidden) {
				return;
			}
			AppScenesSaved = appLauncherButton.VisibleInScenes;
		}
		internal void Toggle() {
			if (!isActive || !isEnabled) {
				return;
			}
			if (isTrue) {
				appLauncherButton.SetFalse (true);
			} else {
				appLauncherButton.SetTrue (true);
			}
			QuickHide.Log ("Toggle the AppLauncher: " + AppRef, "QMods");
		}
		internal void SetTrue(bool force = false) {
			if (!isActive || isTrue) {
				return;
			}
			appLauncherButton.SetTrue (force);
			QuickHide.Log ("SetFalse the AppLauncher: " + AppRef, "QMods");
		}
		internal void SetFalse(bool force = false) {
			if (!isActive || isFalse) {
				return;
			}
			appLauncherButton.SetFalse (force);
			QuickHide.Log ("SetFalse the AppLauncher: " + AppRef, "QMods");
		}
		private void StoreAppScenes() {
			if (!isActive || !CanBeHide || isHidden) {
				return;
			}
			SaveCurrentAppScenes ();
			appLauncherButton.VisibleInScenes = ApplicationLauncher.AppScenes.NEVER;
			QuickHide.Log ("Store the AppLauncher: " + AppRef, "QMods");
		}
		void RestoreAppScenes() {
			if (!isActive || isStored) {
				return;
			}
			appLauncherButton.VisibleInScenes = AppScenesSaved;
			QuickHide.Log ("Restore the AppLauncher: " + AppRef, "QMods");
		}
		internal void Refresh(ApplicationLauncherButton button) {
			if (button == null) {
				return;
			}
			appLauncherButton = button;
			QuickHide.Log ("Refresh the AppLauncher: " + AppRef, "QMods");
		}
	}
}