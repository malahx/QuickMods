/* 
QuickManeuver
Copyright 2021 Malah

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

using KSP.Localization;
using UnityEngine;

namespace QuickVesselName
{

    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class QuickVesselName : MonoBehaviour
    {
        private VesselNameService vesselNameService;

        private void Awake()
        {
            vesselNameService = new VesselNameService(Config.INSTANCE);
            Debug.Log ("[QuickVesselName] Awake");
        }

        private void Start()
        {
            GameEvents.onEditorPodPicked.Add(OnEditorPodPicked);
            GameEvents.onEditorPodDeleted.Add(OnEditorPodDeleted);
            Debug.Log ("[QuickVesselName] Start");
        }

        private void OnEditorPodPicked(Part data)
        {
            var vesselName = EditorLogic.fetch.shipNameField.text;
            if (vesselName != "" && vesselName != Localizer.Format("#autoLOC_900530"))
            {
                return;
            }
            
            var vesselDefinition = new VesselDefinition
            {
                PartName = data.name,
                VesselType = data.vesselType,
                HasCrew = data.CrewCapacity > 0
            };
            
            EditorLogic.fetch.shipNameField.text = vesselNameService.RetrieveVesselName(vesselDefinition);
            Debug.Log ("[QuickVesselName] Vessel name changed");
        }

        private void OnEditorPodDeleted()
        {
            if (!vesselNameService.IsARetrievedVesselName(EditorLogic.fetch.shipNameField.text))
            {
                return;
            }
            EditorLogic.fetch.shipNameField.text = "";
            Debug.Log ("[QuickVesselName] Vessel name deleted");
        }

        private void OnDestroy()
        {
            GameEvents.onEditorPodPicked.Remove(OnEditorPodPicked);
            GameEvents.onEditorPodDeleted.Remove(OnEditorPodDeleted);
            Debug.Log ("[QuickVesselName] OnDestroy");
        }
    }
}