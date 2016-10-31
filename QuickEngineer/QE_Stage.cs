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
using System.Collections.Generic;
using UnityEngine;

namespace QuickEngineer {
	public class QStage : QuickEngineer {

		public QStage (CelestialBody body = null, bool atmosphere = false, double mach = 0d, float altitude = 0f, bool atmcorrected = false, Stage vacstage = null, Stage atmstage = null) {
			Body = (body != null ? body : QTools.Home);
			Atmosphere = atmosphere;
			Altitude = altitude;
			atmCorrected = atmcorrected;
			vacStage = vacstage;
			atmStage = atmstage;
		}

		public QStage (QStage qStage) {
			Body = (qStage.Body != null ? qStage.Body : QTools.Home);
			Atmosphere = qStage.Atmosphere;
			Altitude = qStage.Altitude;
			atmCorrected = qStage.atmCorrected;
			vacStage = qStage.vacStage;
			atmStage = qStage.atmStage;
		}

		internal static List<QStage> QStages = new List<QStage>();

		public static bool stageExists(int stageIndex) {
			return QStages[stageIndex] != null;
		}

		public static List<QStage> uniqueCalc {
			get {
				Populate ();
				int _qStageCount = stagesCount;
				List<QStage> _list = new List<QStage> ();
				for (int i = 0; i < _qStageCount; i++) {
					QStage _qStage = QStages [i];
					if (!_list.Exists (qs => isSame(qs , _qStage))) {
						_list.Add (_qStage);
						Log ("uniqueCalc added: " + (_qStage.Body == null ? QTools.Home.bodyName : _qStage.Body.bodyName), "QStage");
						Log ("\tAtmosphere: \t" + _qStage.Atmosphere, "QStage");
						Log ("\tMach: \t\t" + _qStage.Mach, "QStage");
						Log ("\tAltitude: \t\t" + _qStage.Altitude, "QStage");
					}
					if (_qStage.Atmosphere && _qStage.atmCorrected) {
						QStage _vacQStage = new QStage (_qStage);
						if (!_list.Exists (qs => isSame(qs , _vacQStage))) {
							_list.Add (_vacQStage);
							Log ("uniqueCalc added: " + (_vacQStage.Body == null ? QTools.Home.bodyName : _vacQStage.Body.bodyName), "QStage");
							Log ("\tAtmosphere: \t" + _vacQStage.Atmosphere, "QStage");
							Log ("\tMach: \t\t" + _vacQStage.Mach, "QStage");
							Log ("\tAltitude: \t\t" + _vacQStage.Altitude, "QStage");
						}
					}
				}
				return _list;
			}
		}
			
		private static bool isSame (QStage qs, QStage qStage, bool forceVAC = false) {
			if (qs.Atmosphere == qStage.Atmosphere && !qs.Atmosphere) {
				return qs.Body == qStage.Body;
			}
			return qs.Body == qStage.Body && qs.Atmosphere == qStage.Atmosphere && qs.Mach == qStage.Mach && qs.Altitude == qStage.Altitude;
		}

		public static Stage GetvacStage (int stageIndex) {
			QStage _qStage = QStages[stageIndex];
			if (_qStage == null) {
				return null;
			}
			return _qStage.vacStage;
		}
		public static Stage GetatmStage (int stageIndex) {
			QStage _qStage = QStages[stageIndex];
			if (_qStage == null) {
				return null;
			}
			return _qStage.atmStage;
		}
		public static CelestialBody GetBody (int stageIndex) {
			QStage _qStage = QStages[stageIndex];
			if (_qStage == null) {
				return null;
			}
			return _qStage.Body;
		}
		public static bool GetAtmosphere (int stageIndex) {
			QStage _qStage = QStages[stageIndex];
			if (_qStage == null) {
				return false;
			}
			return _qStage.Atmosphere;
		}
		public static double GetAltitude (int stageIndex) {
			QStage _qStage = QStages[stageIndex];
			if (_qStage == null) {
				return -1f;
			}
			return _qStage.Altitude;
		}
		public static double GetMach (int stageIndex) {
			QStage _qStage = QStages[stageIndex];
			if (_qStage == null) {
				return -1f;
			}
			return _qStage.Mach;
		}
		public static bool GetatmCorrected (int stageIndex) {
			QStage _qStage = QStages[stageIndex];
			if (_qStage == null) {
				return false;
			}
			return _qStage.atmCorrected;
		}

		public static int stagesCount {
			get {
				// Staging.StageCount can return 1 when there's no stages.
				return Staging.fetch.stages.Count;
			}
		}

		public static double TotalDeltaV {
			get {
				int _qStageCount = QStages.Count;
				double _totalDeltaV = 0;
				for (int i = 0; i < _qStageCount; i++) {
					QStage _qStage = QStages [i];
					_totalDeltaV += _qStage.deltaV;
				}
				return _totalDeltaV;
			}
		}

		public static QStage CreateQStage(CelestialBody body = null, bool atmosphere = false, double mach = 0d, float altitude = 0f, bool atmcorrected = false, Stage vacstage = null, Stage atmstage = null) {
			QStage _qStage;
			_qStage = new QStage (body, atmosphere, mach, altitude, atmcorrected, vacstage, atmstage);
			Log ("QStage created: " + (body == null ? QTools.Home.bodyName : body.bodyName), "QStage");
			Log ("\tAtmosphere: \t" + atmosphere, "QStage");
			Log ("\tMach: \t\t" + mach, "QStage");
			Log ("\tAltitude: \t\t" + altitude, "QStage");
			QStages.Add (_qStage);
			return _qStage;
		}

		public static void setAllStages(Stage[] stages, QStage qStageCalc) {
			if (qStageCalc.Body == null) {
				qStageCalc.Body = QTools.Home;
			}
			int _qStageCount = QStages.Count;
			int _stageLength = stages.Length;
			if (_qStageCount < _stageLength) {
				Populate();
			}
			for (int i = 0; i < _stageLength; i++) {
				if (_qStageCount <= i) {
					Warning ("Wrong stage count: " + i, "QStage");
					Warning ("\t_qStageCount " + _qStageCount, "QStage");
					Warning ("\t_stageLength " + _stageLength, "QStage");
					Warning ("\tQStage.stagesCount " + stagesCount, "QStage");
					break;
				}
				QStage _qStage = QStages[i];
				Stage _stage = stages[i];
				if (qStageCalc.Body == _qStage.Body) {
					if (!qStageCalc.Atmosphere) {
						LogSet (i, _stage, qStageCalc);
						_qStage.vacStage = _stage;
					} else if (isSame (_qStage, qStageCalc)) {
						LogSet (i, _stage, qStageCalc);
						_qStage.atmStage = _stage;
					}
				}
			}
			Log ("setAllStages", "QStage");
		}

		private static void LogSet (int i, Stage stage, QStage qStage) {
			Log ("Set Stage: " + i, "QStage");
			Log ("\tBody : \t\t" + qStage.Body.bodyName, "QStage");
			Log ("\tAtmosphere: \t" + qStage.Atmosphere, "QStage");
			if (qStage.Atmosphere) {
				Log ("\t\tCalculation: " + CelestialBodies.SelectedBody.GetAtmospheres (qStage.Altitude), "QStage");
				Log ("\tMach: \t\t" + qStage.Mach, "QStage");
				Log ("\tAltitude: \t\t" + qStage.Altitude, "QStage");
			}
			Log ("\tdeltaV: \t\t" + stage.deltaV, "QStage");
			Log ("\tTWR: \t\t" + stage.thrustToWeight, "QStage");
			Log ("\tmaxTWR: \t\t" + stage.maxThrustToWeight, "QStage");
		}

		public static void ClearAllStages() {
			int _qStageCount = QStages.Count;
			for (int i = 0; i < _qStageCount; i++) {
				QStage _qStage = QStages [i];
				_qStage.ClearStages (i);
			}
			Log ("ClearAllStages", "QStage");
		}

		public static void Populate() {
			int _stageCount = stagesCount;
			int _qStageCount = QStages.Count;
			if (_stageCount == _qStageCount) {
				Log ("Stages OK", "QStage");
				return;
			}
			if (_qStageCount >_stageCount) {
				Log ("More stage in qStages does nothing!", "QStage");
				return;
			}
			if (_qStageCount < _stageCount) {
				for (int i = _qStageCount; i <= _stageCount; i++) {
					CreateQStage ();
				}
			}
			Log ("Populate stages", "QStage");
		}

		public void ClearStages(int index) {
			vacStage = null;
			atmStage = null;
			Log ("ClearStages: " + index, "QStage");
		}

		public CelestialBody Body;
		public bool Atmosphere;
		public float Altitude;
		public double Mach;
		public bool atmCorrected;
		public Stage vacStage;
		public Stage atmStage;

		public float maxMach {
			get {
				if (atmStage != null) {
					return atmStage.maxMach;
				}
				if (vacStage != null) {
					return vacStage.maxMach;
				}
				return 1f;
			}
		}

		public double deltaV {
			get {
				if (Atmosphere) {
					if (atmStage != null) {
						if (atmCorrected && vacStage != null) {
							double _deltaVout = QBody.deltaVout (Body);
							if (_deltaVout > 0) {
								// From http://wiki.kerbalspaceprogram.com/wiki/Cheat_sheet#Delta-v_.28.CE.94v.29
								return Math.Max(atmStage.deltaV, ((atmStage.deltaV - _deltaVout) / atmStage.deltaV) * vacStage.deltaV + _deltaVout);
							}
						}
						return atmStage.deltaV;
					}
				}
				if (vacStage == null) {
					return 0;
				}
				return vacStage.deltaV;
			}
		}

		public double inverseTotalDeltaV(int index) {
			int _qStageCount = QStages.Count;
			double _totalDeltaV = 0;
			for (int i = index; i < _qStageCount; i++) {
				QStage _qStage = QStages [i];
				_totalDeltaV += _qStage.deltaV;
			}
			return _totalDeltaV;
		}

		public double totalDeltaV(int index) {
			double _totalDeltaV = 0;
			for (int i = index; i > 0; i--) {
				QStage _qStage = QStages [i];
				_totalDeltaV += _qStage.deltaV;
			}
			return _totalDeltaV;
		}

		public double thrustToWeight {
			get {
				if (Atmosphere) {
					if (atmStage != null) {
						return atmStage.thrustToWeight;
					}
				}
				if (vacStage == null) {
					return 0;
				}
				return vacStage.thrustToWeight;
			}
		}

		public double maxThrustToWeight {
			get {
				if (Atmosphere) {
					if (atmStage != null) {
						return atmStage.maxThrustToWeight;
					}
				}
				if (vacStage == null) {
					return 0;
				}
				return vacStage.maxThrustToWeight;
			}
		}
	}
}