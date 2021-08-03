using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerMod.Items.Accessories
{
    // Here we add our accessories, note that they inherit from ExclusiveAccessory, and not ModItem

    public class ArcanePulse : ExclusiveAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("On heart pickup: 60% Chance to grant an extra 40 health with a 15s cooldown\nMay drop from any boss in Expert Mode");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.rare = -12;
            item.width = 32;
            item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Here we add an additional effect
            player.GetModPlayer<wdfeerPlayer>().arcanePulse = true;
        }
    }
}