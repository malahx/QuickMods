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

using KerbalEngineer;
using KerbalEngineer.Editor;
using KerbalEngineer.VesselSimulator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickEngineer {
	public class QVessel : QuickEngineer {
		
		internal static Stage[] Stages;
		internal static Stage LastStage;
		private static bool simEnded = true;
		internal static bool calculationToRestart = false;

		internal static void Init(QStage qStage) {
			CelestialBodies.SetSelectedBody (qStage.Body.bodyName);
			SimManager.Gravity = QTools.Gravity (qStage.Body);
			BuildAdvanced.Altitude = qStage.Altitude;
			SimManager.Atmosphere = (qStage.Atmosphere ? CelestialBodies.SelectedBody.GetAtmospheres(BuildAdvanced.Altitude) : 0d);
			SimManager.Mach = qStage.Mach;
			SimManager.OnReady += GetSimManagerResults;
			Log ("Simulation inited: \t"+ qStage.Body.bodyName, "QVessel");
			Log ("\tGravity: \t\t"+ SimManager.Gravity, "QVessel");
			Log ("\tAtmosphere: \t"+ qStage.Atmosphere, "QVessel");
			Log ("\t\tCalculation: "+ SimManager.Atmosphere, "QVessel");
			Log ("\tMach: \t\t"+ SimManager.Mach, "QVessel");
			Log ("\tAltitude: \t\t"+ BuildAdvanced.Altitude, "QVessel");
		}
			
		internal static IEnumerator calcWithATM () {
			RESTART:
			List<QStage> _qStagesToCalculate = QStage.uniqueCalc;
			QStage.ClearAllStages ();
			int _qStageCount = _qStagesToCalculate.Count;
			Log (string.Format("Start {0} calculations ...", _qStagesToCalculate.Count), "QVessel");
			if (_qStageCount == 0) {
				QEditor.Instance.UpdateEngineer ("Vessel");
				Log ("No calcul needs", "QVessel");
				yield break;
			}
			QEditor.Instance.UpdateEngineer ("Vessel", true);
			for (int i = 0; i < _qStageCount; i++) {
				yield return new WaitForFixedUpdate ();
				QStage _qStage = _qStagesToCalculate [i];
				Init (_qStage);
				Stages = null;
				StartSim ();
				while (Stages == null && !simEnded) {
					yield return 0;
				}
				QStage.setAllStages (Stages, _qStage);
				yield return new WaitForSeconds(0.2f);
				if (calculationToRestart) {
					calculationToRestart = false;
					Log ("Calculation restarted", "QVessel");
					goto RESTART;
				}
			}
			QEditor.Instance.UpdateEngineer ("Vessel");
			QEditor.Instance.calculation = null;
			Log ("Calculation finished", "QVessel");
		}

		internal static void StartSim() {
			simEnded = false;
			SimManager.RequestSimulation();
			SimManager.TryStartSimulation();
			Log ("Simulation started", "QVessel");
		}

		internal static void GetSimManagerResults() {
			if (simEnded) {
				return;
			}
			Stages = SimManager.Stages;
			LastStage = SimManager.LastStage;
			if (QEditor.EditorIsActive && QSettings.Instance.EditorVesselEngineer_Simple) {
				QEditor.Instance.UpdateEngineer ("Vessel");
			}
			Log ("Simulation ended", "QVessel");
			simEnded = true;
		}
	}
}