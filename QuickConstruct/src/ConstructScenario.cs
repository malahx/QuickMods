using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QuickConstruct
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION)]
    public class ConstructScenario : ScenarioModule
    {
        
        internal static ConstructScenario Instance;
        
        public static EventVoid OnEndsConstruction = new EventVoid(nameof (OnEndsConstruction));

        private double editorTimePassed;
        private int editorTimePassedFactor = 3600;

        private double lastDate;

        public bool HasTimeToPass => editorTimePassed > 0;
        public double EditorTimePassed => editorTimePassed;
        
        public override void OnAwake()
        {
            Instance = this;
        }

        private void Start()
        {
            // Prepare time calculation
            lastDate = HighLogic.LoadedSceneIsEditor ? new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() : Planetarium.GetUniversalTime();
        }

        // Add or not the editor time and call listeners if need
        private void Update()
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                // Add time passed on the editor 
                var currentDate = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds(); 
                editorTimePassed += (currentDate - lastDate) * editorTimePassedFactor / 1000;
                lastDate = currentDate;
            }
            else if (editorTimePassed > 0 && Planetarium.GetUniversalTime() - lastDate > 0)
            {
                // Subtract time passed on other scene
                var currentDate = Planetarium.GetUniversalTime();
                editorTimePassed -= currentDate - lastDate;   
                lastDate = currentDate;
            }
            else if (editorTimePassed < 0)
            {
                // Reset time passed
                editorTimePassed = 0;
                OnEndsConstruction.Fire();
            }
        }

        // Save the QuickConstruct data
        public override void OnSave(ConfigNode node)
        {
            node.AddValue("editorTimePassed", editorTimePassed);
            node.AddValue("editorTimePassedFactor", editorTimePassedFactor);
            base.OnSave(node);
            
            Debug.Log($"[QuickConstruct]({name}): Saved");
        }

        // Retrieve QuickConstruct data
        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            
            if (node.HasValue("editorTimePassed")) 
                double.TryParse(node.GetValue("editorTimePassed"), out editorTimePassed);
            
            if (node.HasValue("editorTimePassedFactor"))
                int.TryParse(node.GetValue("editorTimePassedFactor"), out editorTimePassedFactor);
            
            Debug.Log($"[QuickConstruct]({name}): Loaded");
        }

        private void OnDestroy()
        {
            Debug.Log($"[QuickConstruct]({name}) Destroy, construction needs: {KSPUtil.PrintTime(editorTimePassed, 3, true)}");
        }
    }
}