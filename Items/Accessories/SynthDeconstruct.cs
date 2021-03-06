using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wfMod.Items.Accessories
{
    
    public class SynthDeconstruct : ExclusiveAccessory
    {
        public const int heartDropChance = 25;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault($"Enemies damaged by minions have a {heartDropChance}% chance to drop a life heart on death");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.rare = 2;
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(silver: 50);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LifeCrystal);
            recipe.AddIngredient(ItemID.GoldBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Here we add an additional effect
            player.GetModPlayer<wfPlayer>().synthDeconstruct = true;
        }
    }
}