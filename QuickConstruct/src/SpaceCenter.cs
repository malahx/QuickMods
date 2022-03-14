using System.Collections;
using System.Collections.Generic;
using KSP.UI;
using QuickConstruct.utils;
using Smooth.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace QuickConstruct
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class SpaceCenter : MonoBehaviour
    {
        private Button launchBtn;
        private Button constructBtn;
        private ShipTemplate selectedShip;
        private Dictionary<ShipTemplate,VesselListItem> vesselListItems;

        private void Start()
        {
            // No scenario no needs
            if (ConstructScenario.Instance == null)
                Destroy(this);
            
            // Retrieve the launch button
            GameEvents.onGUILaunchScreenSpawn.Add(OnGUILaunchScreenSpawn);
            GameEvents.onGUILaunchScreenVesselSelected.Add(OnGUILaunchScreenVesselSelected);
            
            Debug.Log($"[QuickConstruct]({name}): Start");
        }

        private void OnGUILaunchScreenVesselSelected(ShipTemplate data)
        {
            selectedShip = data;

            ButtonUtils.RefreshButton(data, launchBtn, constructBtn);
            
            ConstructScenario.Instance.spaceCenterSelectedShipName = VesselUtils.ShipName(data);
            
            Debug.Log($"[QuickConstruct]({name}): Vessel selected");
        }


        // Search of the launch button
        private void OnGUILaunchScreenSpawn(GameEvents.VesselSpawnInfo data)
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            Debug.Log($"[QuickConstruct]({name}): Initialize Launch Screen");
            
            // Search of the buttons to move / use them
            Button editBtn = null;
            Button deleteBtn = null;
            launchBtn = null;
            vesselListItems = null;

            while (editBtn == null || deleteBtn == null || launchBtn == null)
            {
                editBtn = editBtn ? editBtn : ButtonUtils.FindButtons("Button_edit");
                deleteBtn = deleteBtn ? deleteBtn : ButtonUtils.FindButtons("Button_delete");
                launchBtn = launchBtn ? launchBtn : ButtonUtils.FindButtons("Button_launch");
                yield return new WaitForFixedUpdate();
            }

            constructBtn = ButtonUtils.CreateConstructionButton(editBtn, OnClickOnConstruct);
            
            // Move other buttons
            deleteBtn.transform.Translate(-(editBtn.transform.position - deleteBtn.transform.position));
            editBtn.transform.Translate(-(launchBtn.transform.position - editBtn.transform.position));

            while (vesselListItems == null)
            {
                vesselListItems = VesselUtils.InitializeVessels();
                yield return new WaitForFixedUpdate();
            }

            Debug.Log($"[QuickConstruct]({name}): Launch Screen initialized");        
        }
        
        private void OnClickOnConstruct()
        {
            if (selectedShip != null)
            {
                ConstructScenario.Instance.AddToConstruction(selectedShip);
                var vesselListItem = vesselListItems.TryGet(selectedShip);
                if (vesselListItem.isSome)
                {
                    vesselListItem.value.vesselWarnings.text = MessageUtils.PrepareMessage(selectedShip);
                }
            }
                
            ButtonUtils.RefreshButton(selectedShip, launchBtn, constructBtn);

            Debug.Log($"[QuickConstruct]({name}): Construct");
        }

        private void OnDestroy()
        {
            GameEvents.onGUILaunchScreenSpawn.Remove(OnGUILaunchScreenSpawn);
            GameEvents.onGUILaunchScreenVesselSelected.Remove(OnGUILaunchScreenVesselSelected);
            Debug.Log($"[QuickConstruct]({name}): Destroy");
        }
    }
}