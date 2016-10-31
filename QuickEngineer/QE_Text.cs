/* 
QuickEngineer
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

using KerbalEngineer.VesselSimulator;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuickEngineer {

	public partial class QuickEngineer {
		private string colorTitle = "<#e6752a>";
		private string colorValue = "<#b2d256>";
		private string colorCom = "<#bebebe>";

		protected string textTitle(string title) {
			return string.Format ("{0}{1} - {2}", colorTitle, MOD, title);
		}
		protected string textDeltaV(double dV, double totaldV) {
			if (QSettings.Instance.VesselEngineer_hidedeltaV) {
				return string.Empty;
			}
			return string.Format ((QSettings.Instance.VesselEngineer_showStageTotaldV || QSettings.Instance.VesselEngineer_showStageInverseTotaldV ? "{0:0} / {1:0} m/s" : "{0:0} m/s"), dV, totaldV);
		}
		protected string textLine(bool force = false) {
			if (!force && (QSettings.Instance.VesselEngineer_hidedeltaV || QSettings.Instance.VesselEngineer_hideTWR)) { 
				return string.Empty;
			}
			return Environment.NewLine;
		}
		protected string textTWR(double TWR, double emptyTWR) {
			if (QSettings.Instance.VesselEngineer_hideTWR) {
				return string.Empty;
			}
			return "TWR: " + string.Format ((QSettings.Instance.VesselEngineer_showEmptyTWR ? "{0:0.00} ({1:0.00})" : "{0:0.00}"), TWR, emptyTWR);
		}
		protected string textEngineer(int stageIndex, Stage stage, bool color = true) {
			return (color ? colorValue : string.Empty) + textDeltaV (stage.deltaV, (QSettings.Instance.VesselEngineer_showStageInverseTotaldV ? stage.inverseTotalDeltaV : stage.totalDeltaV)) + textLine(false) + textTWR (stage.thrustToWeight, stage.maxThrustToWeight);
		}
		protected string textEngineer(int stageIndex, QStage qStage, bool color = true) {
			return (color ? colorValue : string.Empty) + textDeltaV (qStage.deltaV, (QSettings.Instance.VesselEngineer_showStageInverseTotaldV ? qStage.inverseTotalDeltaV(stageIndex) : qStage.totalDeltaV(stageIndex))) + textLine(false) + textTWR (qStage.thrustToWeight, qStage.maxThrustToWeight);
		}
		protected string textTotalDeltaV(Stage lastStage, bool color = true) {
			return (color ? colorValue : string.Empty) + string.Format ("{0:0} m/s", (lastStage != null ? lastStage.totalDeltaV : 0));
		}
		protected string textTotalDeltaV(double totalDeltaV, bool color = true) {
			return (color ? colorValue : string.Empty) + string.Format ("{0:0} m/s", totalDeltaV);
		}
		protected string textEditorStage(int i, CelestialBody body, bool atmosphere = false) {
			string _text = string.Format ("Stage {0}:", i);
			if (QSettings.Instance.EditorVesselEngineer_Simple) {
				return _text;
			}
			return _text + string.Format ("{0}{1}({2}){3}", textLine(true), colorCom, (!atmosphere ? "VAC" : "ATM"), body.bodyName);
		}
		protected string textFlightStage(int i) {
			return string.Format ("Stage {0}:", i);
		}
		protected string textColor(string text) {
			return string.Format ("{0}{1}", colorCom, text);
		}
	}
}

