/* 
QuickScroll
Copyright 2015 Malah

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
using KSP.UI.Screens.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace QuickScroll {
	public class QCategory {
		// Lister tous les filtres (ainsi que les subassemblies)
		public static List<PartCategorizer.Category> Filters {
			get {
				List<PartCategorizer.Category> _filters = new List<PartCategorizer.Category>();
				_filters.AddRange(PartCategorizer.Instance.filters);
				_filters.AddRange(PartCategorizer.Instance.categories);
				return _filters;
			}
		}

		// Indiquer quel filtre est sélectionné
		public static PartCategorizer.Category CurrentFilter {
			get {
				return Filters.Find(f => f.button.activeButton.CurrentState == UIRadioButton.State.True);
			}
		}

		// Indiquer l'index de la catégorie sélectionnée
		public static int IndexCurrentFilter {
			get {
				return Filters.FindIndex(f => f.button.activeButton.CurrentState == UIRadioButton.State.True);
			}
		}

		// Lister toutes les catégories d'un filtre
		public static List<PartCategorizer.Category> Categories {
			get {
				return CurrentFilter.subcategories;
			}
		}

		// Indiquer quelle catégorie est sélectionnée
		public static PartCategorizer.Category CurrentCategory {
			get {
				return Categories.Find(f => f.button.activeButton.CurrentState == UIRadioButton.State.True);
			}
		}

		// Indiquer l'index de la catégorie sélectionnée
		public static int IndexCurrentCategory {
			get {
				return Categories.FindIndex(f => f.button.activeButton.CurrentState == UIRadioButton.State.True);
			}
		}

		// Sélectionner le filtre ou la catégorie suivant
		public static PartCategorizer.Category NextCategory(List<PartCategorizer.Category> categories, int index) {
			if (index >= categories.Count -1) {
				index = -1;
			}
			index++;
			return categories[index];
		}
		/*public static KSP.UI.UIListItem NextCategory(UIList list, int index) {
			if (index >= list.Count -1) {
				index = -1;
			}
			index++;
			return list.GetUilistItemAt(index);
		}*/

		// Sélectionner le filtre ou la catégorie précédent
		public static PartCategorizer.Category PrevCategory(List<PartCategorizer.Category> categories, int index) {
			if (index <= 0) {
				index = categories.Count;
			}
			index--;
			return categories[index];
		}
		/*public static KSP.UI.UIListItem PrevCategory(UIList list, int index) {
			if (index <= 0) {
				index = list.Count;
			}
			index--;
			return list.GetUilistItemAt(index);
		}*/

		// Changer de catégorie
		internal static void SelectPartCategory(bool dirScrolling) {
			SelectPartCategory (dirScrolling, Categories, IndexCurrentCategory, PartCategorizer.Instance.scrollListSub);
		}

		// Changer de filtre
		internal static void SelectPartFilter(bool dirScrolling) {
			SelectPartCategory (dirScrolling, Filters, IndexCurrentFilter, PartCategorizer.Instance.scrollListMain);
		}

		// Changer de filtre/catégorie
		internal static void SelectPartCategory(bool dirScrolling, List<PartCategorizer.Category> categories, int index, UIList list) {
			if (QSettings.Instance.EnableWheelBlockTopEnd) {
				if (dirScrolling && index == 0) {
					return;
				}
				if (!dirScrolling && index == categories.Count - 1) {
					return;
				}
			}
			PartCategorizer.Category _category = (dirScrolling ? PrevCategory (categories, index) : NextCategory (categories, index));
			UIRadioButton _btn = _category.button.activeButton;
			_btn.SetState (UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null, true);
			//PartListTooltipsTWEAK (false);
			QuickScroll.Log ("SelectPartCategory " + (dirScrolling ? "Prev" : "Next"), "QCategory");
		}

		// Petit tweak
		/*internal static void PartListTooltipsTWEAK(bool enable) {
			if (PartListTooltipMasterController.Instance == null) {
				return;
			}
			if (QSettings.Instance.EnableTWEAKPartListTooltips) {
				PartListTooltipMasterController.Instance.enabled = enable;
			} else {
				PartListTooltipMasterController.Instance.enabled = true;
				return;
			}
			if (!enable) {
				if (PartListTooltipMasterController.Instance.currentTooltip != null) {
					GameEvents.onTooltipDestroyRequested.Fire ();
					PartListTooltipMasterController.Instance.HideTooltip ();
				}
			} else {
				if (PartListTooltipMasterController.Instance.PartIcon != null) {
					if (PartListTooltipMasterController.Instance.PartIcon.MouseOver && PartListTooltipMasterController.Instance.currentTooltip == null) {
						PartListTooltipMasterController.Instance.enabled = true;
					}
				}
			}
			QuickScroll.Warning ("PartListTooltipsTWEAK " + enable, true);
		}
		internal static void PartListTooltipsTWEAK() {
			if (HighLogic.LoadedSceneIsEditor) {
				if (EditorLogic.fetch.editorScreen == EditorScreen.Parts) {
					// TWEAKPartListTooltips
					if (QSettings.Instance.EnableTWEAKPartListTooltips) {
						if (Input.GetKeyDown (QSettings.Instance.KeyPartListTooltipsActivate)) {
							PartListTooltipsTWEAK (true);
						}
						if (Input.GetKeyUp (QSettings.Instance.KeyPartListTooltipsDisactivate)) {
							PartListTooltipsTWEAK (false);
						}
					}
				}
			}
		}*/
	}
}
