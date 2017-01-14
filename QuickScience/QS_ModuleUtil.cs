/* 
QuickScience
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

using System.Collections.Generic;

namespace QuickScience {

	public static class QModuleUtil {

		internal static string lastBiome = string.Empty;
		internal static ExperimentSituations lastSituation = ExperimentSituations.SrfLanded;

		public static string getBiome(this Vessel vessel) {
			return vessel.landedAt != string.Empty ?
							Vessel.GetLandedAtString (vessel.landedAt) :
							ScienceUtil.GetExperimentBiome (vessel.mainBody, vessel.latitude, vessel.longitude);
		}

		public static bool IsTestable(this ModuleScienceExperiment experiment) {
			
			ScienceExperiment _exp = experiment.experiment;

			Vessel _vessel = experiment.vessel;
			CelestialBody _body = _vessel.mainBody;
			string _biome = _exp.BiomeIsRelevantWhile (lastSituation) ? lastBiome : string.Empty;

			ScienceSubject _subject = ResearchAndDevelopment.GetExperimentSubject (_exp, lastSituation, _body, _biome);
			float _data = _exp.baseValue * _exp.dataScale;
			float _scienceValue = ResearchAndDevelopment.GetScienceValue (_data, _subject) * HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;

			bool newScience = QSettings.Instance.NewSciencePoint ? _scienceValue > 1f : _scienceValue / _subject.scienceCap > 0.1;

			return !experiment.Inoperable &&
						!experiment.Deployed &&
						experiment.GetScienceCount () == 0 &&
						_exp.IsAvailableWhile (lastSituation, _body) &&
						newScience;
		}

		public static bool hasEmptyTest(this List<ModuleScienceExperiment> experiments) {
			for (int _i = experiments.Count - 1; _i >= 0; --_i) {
				ModuleScienceExperiment _experiment = experiments[_i];
				if (_experiment.IsTestable ()) {
					return true;
				}
			}
			return false;
		}

	}
}