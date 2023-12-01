using System.Xml.Serialization;

namespace SalvageModifier
{
    public class Item
    {
        [XmlAttribute] public float ItemID;
        [XmlAttribute] public float SalvageTime;
        [XmlAttribute] public bool PermissionsOverride;

        public Item() { }
        public Item(float i = 1764, float s = 1f, bool p = false) { ItemID = i; SalvageTime = s; PermissionsOverride = p; }
    }
}