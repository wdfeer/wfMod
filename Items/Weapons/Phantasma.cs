using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace wdfeerMod.Items.Weapons
{
    public class Phantasma : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right Click to cast a slow, exploding projectile\n-25% Critical Damage");
        }
        public override void SetDefaults()
        {
            item.damage = 32;
            item.crit = 0;
            item.magic = true;
            item.width = 40;
            item.height = 23;
            item.useTime = 5;
            item.useAnimation = 5;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = Item.buyPrice(gold: 4);
            item.rare = 8;
            item.UseSound = SoundID.Item91.WithPitchVariance(-0.5f).WithVolume(0.6f);
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Projectiles.PhantasmaProj>();
            item.shootSpeed = 16f;
            item.mana = 6;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = 30;
                item.useAnimation = 30;
            }
            else
            {
                item.useTime = 5;
                item.useAnimation = 5;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                damage *= 4;
                speedX *= 0.9f;
                speedY *= 0.9f;
                type = mod.ProjectileType("PhantasmaProj2");
            } 

            Vector2 spawnOffset = new Vector2(speedX, speedY);
            spawnOffset.Normalize();
            spawnOffset *= 50;
            position += spawnOffset;

            int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, Main.LocalPlayer.cHead);
            var globalProj = Main.projectile[proj].GetGlobalProjectile<Projectiles.wdfeerGlobalProj>();
            globalProj.critMult = 0.75f;

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpectreBar, 9);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}