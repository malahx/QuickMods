using System.Collections;
using System.Collections.Generic;
using KSP.UI;
using QuickIronMan.simulation;
using QuickIronMan.utils;
using Smooth.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace QuickIronMan.construction
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class SpaceCenterConstruct : MonoBehaviour
    {
        private Button launchBtn;
        private Button constructBtn;
        private ShipTemplate selectedShip;
        private Dictionary<ShipTemplate, VesselListItem> vesselListItems;


        private void Awake() {
            Simulation.INSTANCE.LockSimulation(false);
            Debug.Log($"[QuickIronMan]({name}): Awake");
        }

        private void Start()
        {
            // Retrieve the launch button
            GameEvents.onGUILaunchScreenSpawn.Add(OnGUILaunchScreenSpawn);
            GameEvents.onGUILaunchScreenVesselSelected.Add(OnGUILaunchScreenVesselSelected);
            
            Debug.Log($"[QuickIronMan]({name}): Start");
        }

        private void OnGUILaunchScreenVesselSelected(ShipTemplate data)
        {
            selectedShip = data;

            ButtonUtils.RefreshButton(data, launchBtn, constructBtn);
            if (vesselListItems.ContainsKey(data))
                vesselListItems[data].vesselWarnings.text = MessageUtils.PrepareMessage(selectedShip);
            
            Debug.Log($"[QuickIronMan]({name}): Vessel selected");
        }


        // Search of the launch button
        private void OnGUILaunchScreenSpawn(GameEvents.VesselSpawnInfo data)
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            Debug.Log($"[QuickIronMan]({name}): Initialize Launch Screen");
            
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
            
            deleteBtn.onClick.AddListener(() =>
            {
                vesselListItems = VesselUtils.InitializeVessels();
                ConstructionService.Instance.CleanedConstruction(vesselListItems.Keys);
            });

            var launchOnClick = launchBtn.onClick;
            launchBtn.onClick = new Button.ButtonClickedEvent();
            launchBtn.onClick.AddListener(() =>
            {
                ConstructionService.Instance.LaunchShip(selectedShip);
                launchOnClick.Invoke();
            });

            while (vesselListItems == null)
            {
                vesselListItems = VesselUtils.InitializeVessels();
                yield return new WaitForFixedUpdate();
            }

            ConstructionService.Instance.CleanedConstruction(vesselListItems.Keys);

            Debug.Log($"[QuickIronMan]({name}): Launch Screen initialized");
        }

        private void OnClickOnConstruct()
        {
            if (selectedShip != null)
            {
                ConstructionService.Instance.AddToConstruction(selectedShip);
                var vesselListItem = vesselListItems.TryGet(selectedShip);
                if (vesselListItem.isSome)
                    vesselListItem.value.vesselWarnings.text = MessageUtils.PrepareMessage(selectedShip);
            }

            ButtonUtils.RefreshButton(selectedShip, launchBtn, constructBtn);

            Debug.Log($"[QuickIronMan]({name}): Construct");
        }

        private void OnDestroy()
        {
            GameEvents.onGUILaunchScreenSpawn.Remove(OnGUILaunchScreenSpawn);
            GameEvents.onGUILaunchScreenVesselSelected.Remove(OnGUILaunchScreenVesselSelected);
            Debug.Log($"[QuickIronMan]({name}): Destroy");
        }
    }
}