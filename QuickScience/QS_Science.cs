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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickScience {

	public partial class QScience {

		public static QScience Instance {
			get;
			private set;
		}

		List<ModuleScienceExperiment> experiments;
		public List<ModuleScienceExperiment> Experiments {
			get {
				if (experiments == null) {
					experiments = FlightGlobals.ActiveVessel.FindPartModulesImplementing<ModuleScienceExperiment> ();
				}
				return experiments;
			}
		}

		List<ModuleScienceContainer> containers;
		public List<ModuleScienceContainer> Containers {
			get {
				if (containers == null) {
					containers = FlightGlobals.ActiveVessel.FindPartModulesImplementing<ModuleScienceContainer> ();
				}
				return containers;
			}
		}

		protected override void Awake() {
			if (!HighLogic.LoadedSceneIsGame || Instance != null) {
				Destroy (this);
			}
			Instance = this;
			GameEvents.onVesselChange.Add (OnVesselChange);
			Log ("Awake", "QScience");
		}

		protected override void Start() {
			StartCoroutine (checkVesselDatas ());
			Log ("Start", "QScience");
		}

		protected override void OnDestroy() {
			GameEvents.onVesselChange.Remove (OnVesselChange);
			Log ("OnDestroy", "QScience");
		}

		void OnVesselChange(Vessel vessel) {
			if (!vessel.isActiveVessel) {
				return;
			}
			experiments = null;
			containers = null;
			Refresh ();
			Log ("OnVesselChange", "QScience");
		}

		IEnumerator checkVesselDatas() {
			while (HighLogic.LoadedSceneIsFlight) {
				yield return new WaitForFixedUpdate();
				Vessel _vessel = FlightGlobals.ActiveVessel;
				string _biome = _vessel.getBiome ();
				ExperimentSituations _situation = ScienceUtil.GetExperimentSituation (_vessel);
				if (QModuleUtil.lastBiome != _biome || QModuleUtil.lastSituation != _situation) {
					Refresh ();
					if (QSettings.Instance.StopTimeWarp && TimeWarp.CurrentRate > 1) {
						TimeWarp.fetch.CancelAutoWarp ();
						TimeWarp.SetRate (0, false);
					}
					QModuleUtil.lastBiome = _biome;
					QModuleUtil.lastSituation = _situation;
				}
				Log ("_biome " + _biome, "QScience");
				Log ("_situation " + _situation, "QScience");
			}
			Log ("checkVesselDatas", "QScience");
		}

		void Update() {
			if (!HighLogic.LoadedSceneIsFlight) {
				return;
			}
			if (QKey.isKeyDown (QKey.Key.TestAll)) {
				TestAll ();
			}
			if (QKey.isKeyDown (QKey.Key.CollectAll)) {
				CollectAll ();
			}
		}

		public void TestAll() {
			for (int _i = Experiments.Count - 1; _i >= 0; --_i) {
				ModuleScienceExperiment _experiment = Experiments[_i];
				if (!_experiment.IsTestable ()) {
					continue;
				}
				_experiment.DeployExperiment ();
			}
			Refresh ();
			Log ("TestAll", "QScience");
		}

		public void CollectAll() {
			for (int _i = Experiments.Count - 1; _i >= 0; --_i) {
				ModuleScienceExperiment _experiment = Experiments[_i];
				if (_experiment.IsTestable ()) {
					continue;
				}
				ScienceData[] _datas = _experiment.GetData ();
				for (int _j = _datas.Length - 1; _j >= 0; --_j) {
					ScienceData _data = _datas[_j];
					for (int _k = Containers.Count - 1; _k >= 0; --_k) {
						ModuleScienceContainer _container = Containers[_k];
						if (!_container.HasData (_data)) {
							_experiment.onCollectData (_container);
							break;
						}
					}
				}
			}
			Refresh ();
			Log ("CollectAll", "QScience");
		}

		void Refresh() {
			QStockToolbar.Instance.Refresh ();
			Log ("Refresh", "QScience");
		}
	}
}