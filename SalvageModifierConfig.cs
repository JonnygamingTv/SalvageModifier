using Rocket.API;

namespace SalvageModifier
{
    public class SalvageModifierConfig : IRocketPluginConfiguration
    {
        public float DefaultSalvageTime;
        public string PermissionPrefix;
        public System.Collections.Generic.List<Item> Items { get; set; }
        public void LoadDefaults()
        {
            DefaultSalvageTime = 8f;
            PermissionPrefix = "salvagetime.";
            Items = new System.Collections.Generic.List<Item>() { new Item(1764, 1, false) };
    }
    }
}
