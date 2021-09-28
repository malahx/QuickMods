using System.Collections;
using System.Linq;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace QuickConstruct
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class SpaceCenter : MonoBehaviour
    {
        private const string WarpTexturePath = "QuickMods/QuickConstruct/Textures/sim";
        
        private Button launchBtn;
        private Sprite launchSprite;
        private Sprite warpSprite;
        private SpriteState launchSpriteState;
        private SpriteState warpSpriteState;
        private Button.ButtonClickedEvent launchOnClick;
        private Button.ButtonClickedEvent warpOnClick;

        private void Start()
        {
            // No scenario no needs
            if (ConstructScenario.Instance == null)
                Destroy(this);
            
            // Prepare warp button clicks / sprites
            warpOnClick = new Button.ButtonClickedEvent();
            warpOnClick.AddListener(() => 
            {
                // Close the vessel spawn dialog to can cancel warp and to have access to UI
                if (VesselSpawnDialog.Instance != null)
                    VesselSpawnDialog.Instance.ButtonClose();
            
                // Warp
                TimeWarp.fetch.WarpTo(Planetarium.GetUniversalTime() + ConstructScenario.Instance.EditorTimePassed);
            
                Debug.Log($"[QuickConstruct]({name}): Warp");
            });
            
            var texture = Resources.FindObjectsOfTypeAll<Texture2D>().FirstOrDefault(t => t.name == WarpTexturePath);
            warpSprite = Sprite.Create(texture, new Rect(128, 128, 128, 128), Vector2.zero);
            warpSpriteState =  new SpriteState
            {
                selectedSprite = Sprite.Create(texture, new Rect(0, 128, 128, 128), Vector2.zero),
                highlightedSprite = Sprite.Create(texture, new Rect(0, 128, 128, 128), Vector2.zero),
                pressedSprite = Sprite.Create(texture, new Rect(128, 0, 128, 128), Vector2.zero),
                disabledSprite = Sprite.Create(texture, new Rect(128, 0, 128, 128), Vector2.zero)
            };
            
            // Add a listener for when the time passed is passed
            ConstructScenario.OnEndsConstruction.Add(ResetLaunchButton);
            
            // Retrieve the launch button
            GameEvents.onGUILaunchScreenSpawn.Add(OnGUILaunchScreenSpawn);
            Debug.Log($"[QuickConstruct]({name}): Start");
        }

        private void ResetLaunchButton()
        {
            if (launchBtn == null)
                return;

            launchBtn.onClick = launchOnClick;
            launchBtn.image.sprite = launchSprite;
            launchBtn.spriteState = launchSpriteState;
        }

        // Search of the launch button
        private void OnGUILaunchScreenSpawn(GameEvents.VesselSpawnInfo data)
        {
            StartCoroutine(nameof(RetrieveLaunchButton));
        }

        // Is it better in Update() ? I don't like expensive methods in Update() ...
        private IEnumerator RetrieveLaunchButton() 
        {
            Debug.Log($"[QuickConstruct]({name}): Search LaunchButton");
            
            // Search of the launch button
            while (launchBtn == null)
            {
                if (VesselSpawnDialog.Instance != null && VesselSpawnDialog.Instance.Visible)
                {
                    launchBtn = (Button) FindObjectsOfType(typeof(Button))
                        .FirstOrDefault(c => c.name == "Button_launch");
                }
                yield return new WaitForFixedUpdate();
            }

            // Save launch sprite & onClick for a latter reuse
            launchSprite = launchBtn.image.sprite;
            launchSpriteState = launchBtn.spriteState;
            launchOnClick = launchBtn.onClick;

            // Switch launch sprite & onClick if there is editor time passed
            if (ConstructScenario.Instance.HasTimeToPass)
            {
                launchBtn.onClick = warpOnClick;
                launchBtn.image.sprite = warpSprite;
                launchBtn.spriteState = warpSpriteState;
            }
            Debug.Log($"[QuickConstruct]({name}): LaunchButton found");
        }

        private void OnDestroy()
        {
            GameEvents.onGUILaunchScreenSpawn.Remove(OnGUILaunchScreenSpawn);
            ConstructScenario.OnEndsConstruction.Remove(ResetLaunchButton);
            Debug.Log($"[QuickConstruct]({name}): Destroy");
        }
    }
}