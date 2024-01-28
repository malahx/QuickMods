using System.Linq;
using KSP.UI;
using KSP.UI.TooltipTypes;
using QuickIronMan.construction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace QuickIronMan.utils
{
    public static class ButtonUtils
    {
        private const string TexturePath = "QuickMods/QuickIronMan/Textures/construct";

        private static Texture2D _texture;
        
        public static Button CreateConstructionButton(Button toClone, UnityAction onClick)
        {
            var obj = Object.Instantiate(toClone.gameObject, toClone.transform.parent, true);
            Object.DestroyImmediate(obj.GetComponent<Button>());
            obj.GetComponent<TooltipController_Text>().textString = ConstructionService.Instance.CanConstruct() ? "Start a construction" : "To many constructions.<br/>The storage is full.";
            var constructBtn = obj.AddOrGetComponent<Button>();
            constructBtn.image = obj.GetComponent<Image>();
            constructBtn.transition = Selectable.Transition.SpriteSwap;
            var texture = _texture ? _texture : _texture = Resources.FindObjectsOfTypeAll<Texture2D>().FirstOrDefault(t => t.name == TexturePath);
            constructBtn.image.sprite = Sprite.Create(texture, new Rect(0, 41, 41, 41), Vector2.zero);
            constructBtn.spriteState =  new SpriteState
            {
                disabledSprite = Sprite.Create(texture, new Rect(40, 0, 41, 41), Vector2.zero),
                selectedSprite = Sprite.Create(texture, new Rect(0, 41, 41, 41), Vector2.zero),
                highlightedSprite = Sprite.Create(texture, new Rect(0, 0, 41, 41), Vector2.zero),
                pressedSprite = Sprite.Create(texture, new Rect(40, 41, 41, 41), Vector2.zero)
            };
            
            constructBtn.onClick.AddListener(onClick);
            constructBtn.gameObject.SetActive(true);
            constructBtn.transform.SetPositionAndRotation(toClone.transform.position, new Quaternion());
            return constructBtn;
        }

        
        public static Button FindButtons(string name)
        {
            return (Button) Object.FindObjectsOfType(typeof(Button))
                .FirstOrDefault(c => c.name == name);
        }

        public static void RefreshButton(ShipTemplate shipTemplate, Selectable launchBtn, Selectable constructBtn)
        {
            if (ConstructionService.Instance.CanLaunch(shipTemplate))
            {
                launchBtn.Unlock();
            }
            else
            {
                launchBtn.Lock();
            }

            if (ConstructionService.Instance.CanConstruct())
            {
                constructBtn.Unlock();
            }
            else
            {
                constructBtn.Lock();
            }
        }
    }
}