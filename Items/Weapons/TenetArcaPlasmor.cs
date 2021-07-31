using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace wdfeerMod.Items.Weapons
{
    public class TenetArcaPlasmor : wdfeerWeapon
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots a piercing projectile\nMight confuse enemies");
        }
        public override void SetDefaults()
        {
            item.damage = 797; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
            item.crit = 18;
            item.magic = true; // sets the damage type to ranged
            item.mana = 8;
            item.width = 48; // hitbox width of the item
            item.height = 16; // hitbox height of the item
            item.useTime = 60; // The item's use time in ticks (60 ticks == 1 second.)
            item.useAnimation = 60; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 7; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
            item.value = 150000; // how much the item sells for (measured in copper)
            item.rare = 10; // the color that the item's name will be in-game
            item.autoReuse = true; // if you can hold click to automatically use it again
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/ArcaPlasmorSound");
            item.shoot = ModContent.ProjectileType<Projectiles.ArcaPlasmorProj>();
            item.shootSpeed = 30f; // the speed of the projectile (measured in pixels per frame)
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            // ItemType<ExampleItem>() is how to get the ExampleItem item, 10 is the amount of that item you need to craft the recipe
            recipe.AddIngredient(mod.ItemType("ArcaPlasmor"), 1);
            recipe.AddIngredient(mod.ItemType("Fieldron"));
            recipe.AddIngredient(ItemID.FragmentNebula, 8);
            // You can use recipe.AddIngredient(ItemID.TheItemYouWantToUse, the amount of items needed); for a vanilla item.
            recipe.AddTile(412); // Set the crafting tile to ExampleWorkbench
            recipe.SetResult(this); // Set the result to this item (ExampleSword)
            recipe.AddRecipe(); // When your done, add the recipe
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            var projectile = ShootWith(position, speedX, speedY, type, damage, knockBack, offset: item.width);
            projectile.GetGlobalProjectile<Projectiles.wdfeerGlobalProj>().procChances.Add(new ProcChance(31, 34));
            projectile.timeLeft = 44;
            float rotation = Convert.ToSingle(-Math.Atan2(speedX, speedY));
            projectile.rotation = rotation;

            return false;
        }
    }
}