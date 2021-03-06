using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wfMod.Items.Accessories
{
    
    public class ConditionOverload : ExclusiveAccessory
    {
        public const float damagePerStatus = 0.12f;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault($"+{(int)(damagePerStatus*100)}% Damage for each Unique Status Effect on the Target");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.rare = 7;
            item.value = Item.buyPrice(gold: 15);
        }

        public override void AddRecipes()
        {
            // because we don't call base.AddRecipes(), we erase the previously defined recipe and can now make a different one
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AnkhCharm, 1);
            recipe.AddIngredient(ItemID.AvengerEmblem, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Here we add an additional effect
            player.GetModPlayer<wfPlayer>().condOv = true;
        }
    }
}