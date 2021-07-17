﻿/* 
QuickSearch
Copyright 2017 Malah

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
using KSP.UI.Screens;
using System.Collections.Generic;
using UnityEngine;
using QuickSearch.QUtils;
//using QuickSearch.Toolbar;

namespace QuickSearch {
	public partial class QRnD {

		public static QRnD Instance;

		GUIStyle ButtonStyle;

		public string Text = string.Empty;
		public bool Ready = false;

		public Rect RectRDSearch {
			get {
				return new Rect (Screen.width - 250, Screen.height - 50, 200, 40);
			}
		}

		protected override void Awake() {
			if (HighLogic.LoadedScene != GameScenes.SPACECENTER) {
				QDebug.Warning ("The RnD search function works only on the SpaceCenter. Destroy.", "QRnD");
				Destroy (this);
				return;
			}
			QStock.ResetScenes ();
			if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER && HighLogic.CurrentGame.Mode != Game.Modes.SCIENCE_SANDBOX) {
				QDebug.Warning ("The RnD search function works only on a Career or on a Science gamemode. Destroy.", "QRnD");
				Destroy (this);
				return;
			}
			if (Instance != null) {
				QDebug.Warning ("There's already an Instance of " + RegisterToolbar.MOD +". Destroy.", "QRnD");
				Destroy (this);
				return;
			}
			Instance = this;
			if (!QSettings.Instance.RnDSearch) {
				QDebug.Warning ("The RnD search function is disabled. Destroy.", "QRnD");
				Destroy (this);
				return;
			}
			base.Awake ();
			QDebug.Log ("Awake", "QRnD");
		}

		protected override void Start() {
			ButtonStyle = new GUIStyle(HighLogic.Skin.button);
			ButtonStyle.alignment = TextAnchor.MiddleCenter;
			ButtonStyle.padding = new RectOffset (0, 0, 0, 0);
			ButtonStyle.imagePosition = ImagePosition.ImageOnly;
			GameEvents.onGUIRnDComplexSpawn.Add (RnDComplexSpawn);
			GameEvents.onGUIRnDComplexDespawn.Add (RnDComplexDespawn);
			base.Start ();
			QDebug.Log ("Start", "QRnD");
		}

		void RnDComplexSpawn() {
			Ready = true;
			QSearch.Text = string.Empty;
			QDebug.Log ("RnDComplexSpawn", "QRnD");
		}

		void RnDComplexDespawn() {
			Ready = false;
			QSearch.Text = string.Empty;			
			QDebug.Log ("RnDComplexDespawn", "QRnD");
		}

		protected override void OnDestroy() {
			GameEvents.onGUIRnDComplexSpawn.Remove (RnDComplexSpawn);
			GameEvents.onGUIRnDComplexDespawn.Remove (RnDComplexDespawn);
			base.OnDestroy ();
			QDebug.Log ("OnDestroy", "QRnD");
		}

		protected override void OnGUI() {
			base.OnGUI ();
			if (!Ready || !QSettings.Instance.RnDSearch) {
				return;
			}
			GUI.skin = HighLogic.Skin;
			GUILayout.BeginArea (RectRDSearch);
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUI.SetNextControlName ("searchField");
			Text = GUILayout.TextField (Text, TextField,GUILayout.Height(30));
			if (GUILayout.Button (QUtils.Texture.Search, ButtonStyle, GUILayout.Width (30), GUILayout.Height (30))) {
				GUIUtility.keyboardControl = 0;
			}
			if (GUILayout.Button (new GUIContent (QUtils.Texture.Delete, "Clear the search bar"), ButtonStyle,GUILayout.Height(30),GUILayout.Width(30))) {
				Text = string.Empty;
			}
			if (Text != QSearch.Text && !QSettings.Instance.enableEnterToSearch) {
				QSearch.Text = Text;
			}
			if (GUI.GetNameOfFocusedControl () == "searchField") {
				ShowHistory ();
			}
			else {
				if (QSettings.Instance.enableEnterToSearch) {
					QSearch.Text = Text;
				}
				HideHistory ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
			GUILayout.EndArea ();
		}

		internal static void Find(bool clean = false) {
			List<RDNode> _nodes = RDController.Instance.nodes;
			for (int _i = _nodes.Count - 1; _i >= 0; --_i) {
				RDNode _node = _nodes[_i];
                RDTech _rdTech = _node.tech;
                if (_node.graphics != null) {
                    UIStateButton _button = _node.graphics.button;
                    if (!clean && _rdTech.partsAssigned.Find(aPart => QSearch.FindPart(aPart)) != null) {
                        _button.Image.color = new Color(1f, 0f, 0f);
                        continue;
                    }
                    _button.Image.color = new Color(1f, 1f, 1f);   
                }  
			}
			//QDebug.Log ("Find: " + QSearch.Text, "QRnD");
		}
	}
}