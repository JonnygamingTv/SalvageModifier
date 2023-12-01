using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Linq;

namespace SalvageModifier
{
    public class SalvageModifierPlugin : RocketPlugin<SalvageModifierConfig>
    {
        public static SalvageModifierPlugin Instance { get; private set; }

        protected override void Load()
        {
            Instance = this;

            U.Events.OnPlayerConnected += SetPlayerSalvageTime;

            foreach (SteamPlayer steamPlayer in Provider.clients.Where(x => x != null))
            {
                UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(steamPlayer);

                SetPlayerSalvageTime(player);
            }
            Rocket.Core.Logging.Logger.Log("Plugin has been loaded.");
            Rocket.Core.Logging.Logger.Log("This version is edited by JonHosting (2022) with micro-optimizations and a few changes, always put the salvage modification time on the top of permission list per group for the smoothest performance. Check config to see or change the permission for dynamic modification.\nThis version of the plugin compromised a bit of the user-friendly permission system for better microperformance, if you need help feel free to contact the JonHosting.com support team.");
            Rocket.Core.Logging.Logger.Log("Original: https://github.com/IAmSilK/SalvageModifier");
        }
        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= SetPlayerSalvageTime;

            Instance = null;
        }

        public static float GetSalvageTime(UnturnedPlayer player)
        {
            bool CarePerm = true;
            float salvageTime = player.IsAdmin ? 1f : 8f;

            if (Instance.Configuration.Instance.DefaultSalvageTime < salvageTime)
            {
                salvageTime = Instance.Configuration.Instance.DefaultSalvageTime;
            }
            if (Instance.Configuration.Instance.Items.Count != 0 && player.Player.equipment.itemID != 0) {
                Item gg = Instance.Configuration.Instance.Items.Find(i => i.ItemID == player.Player.equipment.itemID);
                if(gg != null)
                {
                    salvageTime = gg.SalvageTime;
                    CarePerm = gg.PermissionsOverride;
                }
            }

            // foreach (var permission in player.GetPermissions())
            if (CarePerm)
            {
                System.Collections.Generic.List<Rocket.API.Serialisation.Permission> Perms = player.GetPermissions();
                for (int i = 0; i < Perms.Count; i++)
                {
                    // string perm = permission.Name;
                    if (Perms[i].Name.Length > Instance.Configuration.Instance.PermissionPrefix.Length && Perms[i].Name.Substring(0, Instance.Configuration.Instance.PermissionPrefix.Length) == Instance.Configuration.Instance.PermissionPrefix)
                    {

                        if (!float.TryParse(Perms[i].Name.Substring(Instance.Configuration.Instance.PermissionPrefix.Length), out float time))
                        {
                            Rocket.Core.Logging.Logger.Log("Invalid salvage time permission: " + Perms[i].Name);
                            continue;
                        }

                        if (time < salvageTime)
                        {
                            salvageTime = time;
                            break;
                        }
                    }
                }
            }

            return salvageTime;
        }

        public static void SetPlayerSalvageTime(UnturnedPlayer player)
        {
            float salvageTime = GetSalvageTime(player);

            player.Player.interact.sendSalvageTimeOverride(salvageTime);
        }
    }
}
