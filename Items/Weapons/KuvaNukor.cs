using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace wdfeerMod.Items.Weapons
{
    public class KuvaNukor : wdfeerWeapon
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Will confuse enemies\nCan hit up to 3 enemies\n+120% Critical Damage\nLimited range");
        }
        public override void SetDefaults()
        {
            item.damage = 17;
            item.crit = 3;
            item.magic = true;
            item.width = 32;
            item.height = 24;
            item.useTime = 6;
            item.useAnimation = 6;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 20000;
            item.rare = 4;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Projectiles.NukorProj>();
            item.shootSpeed = 16f;
            item.mana = 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("Nukor"));
            recipe.AddIngredient(mod.ItemType("Kuva"), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        int lastShotTime = 0;
        int timeSinceLastShot = 60;
        SoundEffectInstance sound;
        int shots = 0;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            var proj = ShootWith(position, speedX, speedY, type, damage, knockBack, offset: item.width + 1);
            var a = 0;
            var b = false;
            proj.modProjectile.ModifyHitPvp(Main.LocalPlayer, ref a, ref b);
            var gProj = proj.GetGlobalProjectile<Projectiles.wdfeerGlobalProj>();
            gProj.critMult = 2.5f;
            gProj.onHit = (NPC target) =>
            {
                gProj.hitNPCs[gProj.hits] = target;
                gProj.hits++;
            };

            timeSinceLastShot = player.GetModPlayer<wdfeerPlayer>().longTimer - lastShotTime;
            lastShotTime = player.GetModPlayer<wdfeerPlayer>().longTimer;

            if (timeSinceLastShot > 12)
            {
                sound = mod.GetSound("Sounds/KuvaNukorStartSound").CreateInstance();
                sound.Volume = 0.4f;
                sound.Play();
            }
            else if (shots % 2 == 0)
            {
                int rand = Main.rand.Next(3);

                switch (rand)
                {
                    case 0:
                        sound = mod.GetSound("Sounds/KuvaNukorLoop1").CreateInstance();
                        break;
                    case 1:
                        sound = mod.GetSound("Sounds/KuvaNukorLoop2").CreateInstance();
                        break;
                    default:
                        sound = mod.GetSound("Sounds/KuvaNukorLoop3").CreateInstance();
                        break;
                }

                sound.Volume = 0.3f;
                sound.Play();
            }
            shots++;

            return false;
        }
    }
}