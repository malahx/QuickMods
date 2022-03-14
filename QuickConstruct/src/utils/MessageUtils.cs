using KSP.UI.Screens.Settings;

namespace QuickConstruct.utils
{
    public class MessageUtils
    {
        public static string PrepareMessage(ShipTemplate shipTemplate)
        {
            return ConstructScenario.Instance.CanLaunch(shipTemplate)
                ? PrepareReadyMessage(shipTemplate)
                : ConstructScenario.Instance.ConstructionStarted(shipTemplate)
                    ? PrepareStartedMessage(shipTemplate)
                    : PrepareNotBuildMessage(shipTemplate);
        }

        private static string PrepareStartedMessage(ShipTemplate shipTemplate)
        {
            return $"<color=yellow>Ready in {KSPUtil.PrintDateDeltaCompact(ConstructScenario.Instance.ConstructionFinishAt(shipTemplate), true, false)}</color>";
        }

        private static string PrepareReadyMessage(ShipTemplate shipTemplate)
        {
            return "<color=green>Ready to launch</color>";
        }

        private static string PrepareNotBuildMessage(ShipTemplate shipTemplate)
        {
            return $"<color=orange>Build in {KSPUtil.PrintDateDeltaCompact(ConstructScenario.Instance.ConstructionFinishAt(shipTemplate), true, false)}</color>";
        }
    }
}