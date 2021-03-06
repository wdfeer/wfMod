using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace wfMod.Items.Weapons
{
    public class TenoraPrime : wfWeapon
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots rapidly with +10% critical damage and near-perfect accuraccy after spooling up\nRight Click to shoot a single shot with greatly increased damage and +50% critical damage\n70% Chance not to consume ammo");
        }
        const int primaryDamage = 45;
        const int secondaryDamage = 310;
        const int baseFireRate = 18;
        const int spooledFireRate = 5;
        public override void SetDefaults()
        {
            item.damage = primaryDamage;
            item.crit = 26;
            item.ranged = true;
            item.width = 60;
            item.height = 16;
            item.useTime = baseFireRate;
            item.useAnimation = baseFireRate;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = Item.buyPrice(gold: 5);
            item.rare = 9;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0.75f, 0.3f);
        }
        public override bool ConsumeAmmo(Player player)
        {
            if (Main.rand.Next(0, 100) <= 70) return false;
            return base.ConsumeAmmo(player);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FragmentVortex, 8);
            recipe.AddIngredient(ItemID.HallowedBar, 15);
            recipe.AddTile(412);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        int lastShotTime = 0;
        int timeSinceLastShot => Main.player[item.owner].GetModPlayer<wfPlayer>().longTimer - lastShotTime;
        int spooledShots = 1;
        float spreadMult => 1f / (spooledShots <= 16 ? (float)Math.Sqrt(spooledShots) : spooledShots / 4);
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2)
            {
                item.autoReuse = true;
                item.crit = 26;
                if (lastShotTime == -1)
                {
                    item.useTime = baseFireRate;
                    item.useAnimation = baseFireRate;
                }

                if (item.useTime > spooledFireRate)
                {
                    item.useTime -= 3;
                    item.useAnimation -= 3;
                    if (item.useTime < spooledFireRate)
                    {
                        item.useTime = spooledFireRate;
                        item.useAnimation = spooledFireRate;
                    }
                }
                else if (timeSinceLastShot > 16)
                {
                    item.useTime += timeSinceLastShot / 3;
                    item.useAnimation += timeSinceLastShot / 3;
                    if (item.useTime > baseFireRate)
                    {
                        item.useTime = baseFireRate;
                        item.useAnimation = baseFireRate;
                    }
                }

                if (timeSinceLastShot < spooledFireRate + 3)
                    spooledShots++;
                else spooledShots = 1;
                lastShotTime = player.GetModPlayer<wfPlayer>().longTimer;
            }
            else
            {
                item.crit = 36;
                item.useTime = 40;
                item.useAnimation = 40;
                item.autoReuse = false;

                lastShotTime = -1;
            }

            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            bool secondary = player.altFunctionUse == 2;

            pathToSound = secondary ? "Sounds/TenoraPrimeSecondarySound" : "Sounds/TenoraPrimePrimarySound";
            PlaySound(Main.rand.NextFloat(0, 0.1f), 0.8f);

            var proj = ShootWith(position, speedX, speedY, type, damage, knockBack, (secondary ? 0 : 0.09f * spreadMult), 57);
            var gProj = proj.GetGlobalProjectile<Projectiles.wfGlobalProj>();
            if (secondary)
            {
                if (proj.penetrate != -1) proj.penetrate += 1;
                proj.damage += (int)((secondaryDamage - primaryDamage) * player.rangedDamageMult * player.allDamageMult);
                proj.knockBack *= 4;
                gProj.critMult = 1.5f;
            }
            else
            {
                gProj.critMult = 1.1f;
                gProj.AddProcChance(new ProcChance(mod.BuffType("SlashProc"), 7));
            }
            return false;
        }
    }
}