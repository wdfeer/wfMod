using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wfMod.Items.Accessories
{

    public class ArcaneAvenger : ExclusiveAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("35% Chance to get +45% Critical Chance for 12s after getting hit for more than 4 damage\nMay drop from any boss in Expert Mode");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.rare = -12;
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(gold: 4);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Here we add an additional effect
            player.GetModPlayer<wfPlayer>().avenger = true;
        }
    }
}