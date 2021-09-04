using System;
using KSP.Localization;
using KSP.UI.TooltipTypes;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace QuickIronMan
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class Editor : MonoBehaviour
    {
        private TooltipController_Text launchTooltipController;
        private string intialTooltip = "";
        private string simulateMessage = "";
        private string launchMessage = "";
        
        private Image launchImage;
        private Sprite simulateSprite;
        private Sprite launchSprite;
        private SpriteState launchSpriteState;
        private SpriteState simulateSpriteState;

        private void Start()
        {
            
            Debug.Log($"QuickIronMan[{SimConfig.INSTANCE.Version}] Start...");
            
            // Retrieve launch button components 
            launchTooltipController = EditorLogic.fetch.launchBtn.GetComponent<TooltipController_Text>();
            launchImage = EditorLogic.fetch.launchBtn.GetComponent<Image>();
            
            // Init icon
            var texture = Resources.FindObjectsOfTypeAll<Texture2D>().FirstOrDefault(t => t.name == SimConfig.SimulationTexturePath);
            simulateSprite = Sprite.Create(texture, new Rect(128, 128, 128, 128), Vector2.zero);
            simulateSpriteState = CreateSpriteState(texture);
            launchSprite = launchImage.sprite;
            launchSpriteState = EditorLogic.fetch.launchBtn.spriteState;
            
            // Init tooltip
            intialTooltip = launchTooltipController.textString;
            var launchInfo = Localizer.Format("quickironman_launch_info_tooltip", SimConfig.INSTANCE.Key);
            simulateMessage = $"{Localizer.Format("quickironman_simulate_tooltip")}\n{launchInfo}";
            launchMessage = $"{Localizer.Format(intialTooltip)}\n{Localizer.Format("quickironman_launch_info_tooltip", SimConfig.INSTANCE.Key)}";
            
            // Init Simulation
            SetSimulation(SimConfig.INSTANCE.DefaultIsSimulation);
            
            Debug.Log($"QuickIronMan[{SimConfig.INSTANCE.Version}] Started.");
        }

        private void SetSimulation(bool value)
        {
            SimConfig.INSTANCE.InSimulation = value;
            launchTooltipController.SetText(value ? simulateMessage : launchMessage);
            launchImage.sprite = value ? simulateSprite : launchSprite;
            EditorLogic.fetch.launchBtn.spriteState = value ? simulateSpriteState : launchSpriteState;
        }

        private void Update()
        {
            if (Input.GetKeyDown(SimConfig.INSTANCE.Key))
            {
                SetSimulation(!SimConfig.INSTANCE.InSimulation);
            }
        }

        private void OnDestroy()
        {
            Debug.Log($"QuickIronMan[{SimConfig.INSTANCE.Version}] Destroyed.");
        }

        private static SpriteState CreateSpriteState(Texture2D texture)
        {
            return new SpriteState
            {
                selectedSprite = Sprite.Create(texture, new Rect(0, 128, 128, 128), Vector2.zero),
                highlightedSprite = Sprite.Create(texture, new Rect(0, 128, 128, 128), Vector2.zero),
                pressedSprite = Sprite.Create(texture, new Rect(128, 0, 128, 128), Vector2.zero),
                disabledSprite = Sprite.Create(texture, new Rect(128, 0, 128, 128), Vector2.zero)
            };
        }
    }
}