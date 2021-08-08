namespace QuickModsInfo
{
    public class ModsInfoService
    {
        private readonly ModsInfoConfig cfg;

        public ModsInfoService(ModsInfoConfig cfg)
        {
            this.cfg = cfg;
        }

        public void UpdateDescription(AvailablePart part)
        {
            part.description += CalculateDescription(part);
        }

        private string CalculateDescription(AvailablePart part)
        {
            var mod = FindOrCreateModDefinition(part?.partUrl, 0);

            if (mod == null)
                return "";

            var modName = "";
            var message = cfg.ModMessage;

            if (mod.Stock)
            {
                modName = ModNameCalculation.CalculateModName(mod);
                message = cfg.StockMessage;
            }
            else if (mod.Expansion || mod.HasSubMods)
            {
                var subMod = FindOrCreateModDefinition(part?.partUrl, 1);
                modName = ModNameCalculation.CalculateModName(mod, subMod);
                message = mod.Expansion ? cfg.ExpansionMessage : cfg.SubModMessage;
            }
            else
            {
                modName = ModNameCalculation.CalculateModName(mod);
            }

            return !string.IsNullOrEmpty(modName) ? $" {message.Replace("[modName]", modName)}" : "";
        }

        private ModDefinition FindOrCreateModDefinition(string part, int subModIndex)
        {
            if (part == null)
                return new ModDefinition();

            var splitPart = part.Split('/');
            if (splitPart.Length <= subModIndex)
                return new ModDefinition();

            var modName = splitPart[subModIndex];
            var mod = cfg.Mods.Find(m => modName.Equals(m.Name));

            return mod ?? new ModDefinition
            {
                Name = modName
            };
        }
    }
}