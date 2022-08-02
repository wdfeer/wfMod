using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wfMod.Items.Accessories
{
    
    public class ArcaneEnergize : ExclusiveAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("On mana star pickup: 60% Chance to grant an extra 100 mana\nMay drop from any boss in Expert Mode");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = -12;
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(gold: 4);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Here we add an additional effect
            player.GetModPlayer<wfPlayer>().arcaneEnergize = true;
        }
    }
}