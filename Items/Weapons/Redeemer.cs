using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace wdfeerMod.Items.Weapons
{
    public class Redeemer : ModItem
    {
        Random rand = new Random();
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires 6 pellets without consuming ammo");
        }
        public override void SetDefaults()
        {
            item.damage = 9; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
            item.crit = 10;
            item.melee = true; // sets the damage type to ranged
            item.noMelee = false; //so the item's animation doesn't do damage
            item.width = 48; // hitbox width of the item
            item.height = 24; // hitbox height of the item
            item.scale = 1f;
            item.useTime = 60; // The item's use time in ticks (60 ticks == 1 second.)
            item.useAnimation = 60; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
            item.knockBack = 1; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
            item.value = 15000; // how much the item sells for (measured in copper)
            item.rare = ItemRarityID.Green; // the color that the item's name will be in-game
            item.UseSound = SoundID.Item36.WithVolume(1f); // The sound that this item plays when used.
            item.autoReuse = true; // if you can hold click to automatically use it again
            item.shoot = ProjectileID.Bullet; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 16f; // the speed of the projectile (measured in pixels per frame)
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("IronBar", 18);
            recipe.AddRecipeGroup("Wood",12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 spawnOffset = new Vector2(speedX, speedY);
            spawnOffset.Normalize();
            spawnOffset *= item.width;
            position += spawnOffset;
            Vector2 spread = new Vector2(speedY, -speedX);
            for (int i = 0; i < 6; i++)
            {
                int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY) + spread * Main.rand.NextFloat(-0.14f, 0.14f), type, damage, knockBack, Main.LocalPlayer.cHead);
                var projectile = Main.projectile[proj];
            }
            return false;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            damage *= 6;
            if (rand.Next(0,100) <= 22)
            {
                target.AddBuff(mod.BuffType("SlashProc"), 300);
                target.GetGlobalNPC<wdfeerGlobalNPC>().slashProcs += Convert.ToInt32(damage * 0.2f);
            }
            base.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
        }
    }
}