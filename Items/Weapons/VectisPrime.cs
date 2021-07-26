using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace wdfeerMod.Items.Weapons
{
    public class VectisPrime : ModItem
    {
        Random rand = new Random();
        public override void SetDefaults()
        {
            item.damage = 210; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
            item.crit = 26;
            item.ranged = true; // sets the damage type to ranged
            item.width = 63; // hitbox width of the item
            item.height = 19; // hitbox height of the item
            item.useTime = 40; // The item's use time in ticks (60 ticks == 1 second.)
            item.useAnimation = 40; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
            item.value = Item.buyPrice(gold: 12); // how much the item sells for (measured in copper)
            item.rare = 8; // the color that the item's name will be in-game
            item.UseSound = SoundID.Item40; // The sound that this item plays when used.
            item.autoReuse = false; // if you can hold click to automatically use it again
            item.shootSpeed = 48f; // the speed of the projectile (measured in pixels per frame)
            item.shoot = ProjectileID.SniperBullet;
            item.useAmmo = AmmoID.Bullet; // The "ammo Id" of the ammo item that this weapon uses. Note that this is not an item Id, but just a magic value.
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0,4);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("Vectis"), 1);
            recipe.AddIngredient(ItemID.SniperRifle, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 spawnOffset = new Vector2(speedX, speedY);
            spawnOffset.Normalize();
            spawnOffset *= item.width;

            position += spawnOffset;
            var proj = Main.projectile[Projectile.NewProjectile(position, new Vector2(speedX, speedY), ProjectileID.BulletHighVelocity, damage, knockBack, Main.LocalPlayer.cHead)];
            proj.GetGlobalProjectile<Projectiles.wdfeerGlobalProj>().slashChance = 8;
            proj.usesLocalNPCImmunity = true;
            proj.localNPCHitCooldown = -1;
            return false;
        }
    }
}