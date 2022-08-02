using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wfMod.Items.Accessories
{
    
    public class Berserker : ExclusiveAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+7% Melee Speed for 6s after a Melee Critical Hit\nStacks 3x");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = 4;
            Item.value = Item.buyPrice(gold: 4);
        }

        public override void AddRecipes()
        {
            // because we don't call base.AddRecipes(), we erase the previously defined recipe and can now make a different one
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PowerGlove, 1);
            recipe.AddIngredient(ItemID.WarriorEmblem, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Here we add an additional effect
            player.GetModPlayer<wfPlayer>().berserker = true;
        }      
    }
}