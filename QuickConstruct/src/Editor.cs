using KSP.Localization;
using UnityEngine;

namespace QuickConstruct
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class Editor : MonoBehaviour
    {

        private double last;
        
        private void Start()
        {
            Debug.Log($"[QuickConstruct]({name}) Start");
        }

        private void Update()
        {
            if (ConstructScenario.Instance.EditorTimePassed - last < 3600 * 6 * 7) 
                return;
            
            last = ConstructScenario.Instance.EditorTimePassed;
            var printTime = KSPUtil.PrintTime(ConstructScenario.Instance.EditorTimePassed, 2, false);
            var message = Localizer.Format("quickconstruct_editor_construct_message", printTime);
            ScreenMessages.PostScreenMessage(message, 10, ScreenMessageStyle.UPPER_RIGHT);
        }

        private void OnDestroy()
        {
            Debug.Log($"[QuickConstruct]({name}) Destroy");
        }
    }
}