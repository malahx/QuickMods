using QuickIronMan.construction;

namespace QuickIronMan.utils
{
    public static class MessageUtils
    {
        public static string PrepareMessage(ShipTemplate shipTemplate)
        {
            return ConstructionService.Instance.CanLaunch(shipTemplate)
                ? PrepareReadyMessage(shipTemplate)
                : ConstructionService.Instance.ConstructionStarted(shipTemplate)
                    ? PrepareStartedMessage(shipTemplate)
                    : PrepareNotBuildMessage(shipTemplate);
        }

        private static string PrepareStartedMessage(ShipTemplate shipTemplate)
        {
            var constructionFinishAt = ConstructionService.Instance.ConstructionFinishAt(shipTemplate);
            var date = KSPUtil.PrintDateDeltaCompact(constructionFinishAt, false, false);
            return
                $"<color=yellow>Ready in {date}</color>";
        }

        private static string PrepareReadyMessage(ShipTemplate shipTemplate)
        {
            var inConstruction = ConstructionService.Instance.InConstruction(shipTemplate);
            return inConstruction > 0
                ? "<color=green>Ready</color>, <color=yellow>build {inConstruction} vessels</color>"
                : "<color=green>Ready to launch</color>";
        }

        private static string PrepareNotBuildMessage(ShipTemplate shipTemplate)
        {
            var constructionFinishAt = ConstructionService.Instance.ConstructionFinishAt(shipTemplate);
            var date = KSPUtil.PrintDateDeltaCompact(constructionFinishAt, false, false);
            return
                $"<color=orange>Build in {date}</color>";
        }
    }
}