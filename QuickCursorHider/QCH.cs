/* 
QuickCursorHider
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

using KSP.UI;
using UnityEngine;

namespace QuickCursorHider
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class QuickCursorHider : MonoBehaviour {

		bool isHide {
			get {
				return UIMasterController.Instance.uiCamera != null ? !UIMasterController.Instance.uiCamera.enabled : true && 
					!UIMasterController.Instance.mainCanvas.enabled &&
					!UIMasterController.Instance.appCanvas.enabled &&
					!UIMasterController.Instance.actionCanvas.enabled &&
					!UIMasterController.Instance.dialogCanvas.enabled &&
					!UIMasterController.Instance.tooltipCanvas.enabled;
			}
		}
	
		void LateUpdate() {
			if (GameSettings.TOGGLE_UI.GetKeyDown ()) {
				if (isHide) {
					Cursor.visible = false;						
					Debug.Log ("QuickCursorHider: Hide Cursor");
				} else {
					Cursor.visible = true;
					Debug.Log ("QuickCursorHider: Show Cursor");
				}
			}
		}
	}
}

