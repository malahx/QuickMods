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

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QuickEngineer {

	public class QBody : QuickEngineer { 
		private static string FileDeltaV = KSPUtil.ApplicationRootPath + "GameData/" + MOD + "/BodiesConfig.cfg";

		// Delta V From Swash's Delta-V Map - KSP 1.0.5: http://forum.kerbalspaceprogram.com/index.php?/topic/87463-1 CC BY-NC-SA 4.0
		private static Dictionary<string, int> DefaultDeltaV = new Dictionary<string, int>() {
			{"Sun.261600000.600000", 	67000},
			{"Eve.700000.90000", 		8000},
			{"Kerbin.600000.70000", 	3400},
			{"Duna.320000.50000", 		1450},
			{"Jool.6000000.200000", 	14000},
			{"Laythe.500000.50000", 	2900}
		};

		private static ConfigNode Persistent = new ConfigNode();

		private static List<CelestialBody> bodies = new List<CelestialBody>();
		internal static List<CelestialBody> Bodies {
			get {
				List<CelestialBody> _bodies = FlightGlobals.Bodies;
				if (_bodies.Count > 0) {
					return _bodies;
				}
				if (bodies.Count == 0) {
					CelestialBody _Sun = Planetarium.fetch.Sun;
					addBodies (_Sun);
				}
				return bodies;
			}
		}

		public static double deltaVout(CelestialBody body) {
			if (!body.atmosphere) {
				return 0;
			}
			if (!hasAtmValue (body)) {
				return 0;
			}
			double _mEp = Planetarium.GetUniversalTime();
			// From HyperEdit: GPL v3
			while (_mEp < 0) {
				_mEp += Math.PI * 2;
			}
			while (_mEp > Math.PI * 2) {
				_mEp -= Math.PI * 2;
			}
			//
			Orbit _orbit = new Orbit (0, 0, body.Radius + body.atmosphereDepth + 10000, 0, 0, _mEp, 0, body);
			return atmDeltaV(body) - _orbit.getOrbitalSpeedAt(Planetarium.GetUniversalTime());
		}

		public static int atmDeltaV(CelestialBody body) {
			if (!hasAtmValue (body)) {
				return 0;
			}
			string _key = convertToKey(body);
			int _deltaV = 0;
			if (!int.TryParse(Persistent.GetNode (_key).GetValue("deltaV"), out _deltaV)) {
				Warning ("Can't parse deltaV for: " + body.name, "QBody");
			}
			return _deltaV;
		}

		public static bool hasAtmValue(CelestialBody body) {
			string _key = convertToKey(body);
			if (Persistent.HasNode(_key)) {
				ConfigNode _node = Persistent.GetNode (_key);
				if (_node.HasValue ("deltaV")) {
					return _node.GetValue ("deltaV") != "0";
				}
			}
			return false;
		}

		private static string convertToKey(CelestialBody body) {
			return body.bodyName + "." + body.Radius + "." + body.atmosphereDepth;
		}

		public static void Set(CelestialBody body, int deltaV) {
			if (deltaV == 0) {
				return;
			}
			string _key = convertToKey(body);
			if (!Persistent.HasNode (_key)) {
				Persistent.AddNode (_key);
			}
			Persistent.GetNode (_key).SetValue ("deltaV", deltaV.ToString(), true);
			Log (string.Format ("Set atmospheric deltaV: {0}({1})", body.bodyName, deltaV.ToString ()), "QBody");
		}

		private static void addBodies(CelestialBody Body) {
			if (!bodies.Contains (Body)) {
				bodies.Add (Body);
				QuickEngineer.Log ("AddBodies: " + Body.bodyName,"QBody");
				QuickEngineer.Log ("\torbitingBodies: " + Body.orbitingBodies.Count,"QBody");
			}
			List<CelestialBody> _bodies = Body.orbitingBodies;
			int _bodiesCount = _bodies.Count;
			for (int i = 0; i < _bodiesCount; i++) {
				CelestialBody _body = _bodies [i];
				addBodies (_body);
			}
		}

		public static void Save(bool init = false) {
			if (init) {
				List<string> _keys = new List<string>(DefaultDeltaV.Keys);
				foreach (string _key in _keys) {
					if (Persistent.HasNode (_key)) {
						if (Persistent.GetNode (_key).GetValue ("deltaV") != "0") {
							continue;
						}
					} else {
						Persistent.AddNode (_key);
					}
					int _deltaV = 0;
					if (DefaultDeltaV.TryGetValue (_key, out _deltaV)) {
						Persistent.GetNode (_key).SetValue ("deltaV", _deltaV.ToString(), true);
						Log (string.Format ("Initialise atmospheric Delta V: {0}({1}m/s)", _key, _deltaV), "QBody");
					}
				}
			}
			Persistent.Save (FileDeltaV);
			Log ("Atmospheric deltaV Saved", "QBody");
		}

		public static void Load() {
			if (File.Exists (FileDeltaV)) {
				try {
					Persistent = ConfigNode.Load (FileDeltaV);
					Log ("Atmospheric deltaV Loaded", "QBody");
				} catch {
					Save ();
					Load ();
				}
			} else {
				Save (true);
				Load ();
			}
		}
	}
}