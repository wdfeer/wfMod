using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wfMod.Items.Accessories
{
    
    public class CriticalDelay : ExclusiveAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+12% (+20% in Hardmode) Critical Chance, but -10% Fire Rate");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.rare = 4;
            item.value = Item.buyPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            int critBoost = Main.hardMode ? 20 : 12;
            player.magicCrit += critBoost;
            player.meleeCrit += critBoost;
            player.rangedCrit += critBoost;
            player.GetModPlayer<wfPlayer>().fireRateMult -= 0.1f;
        }
    }
}