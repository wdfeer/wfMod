using Terraria;
using Terraria.ModLoader;

namespace wfMod.Buffs
{
    public class ArgonScopeBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Argon Scope");
            Description.SetDefault("+15% Critical Chance");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeCrit += 15;
            player.magicCrit += 15;
            player.rangedCrit += 15;
        }
    }
}
