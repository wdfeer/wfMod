using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace wfMod.Items.Weapons
{
    internal class SecuraPenta : wfWeapon
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Launches grenades that stick to surfaces and have to be detonated manually\nRight Click to trigger the explosions\nDirect hits deal 40% of the damage\nOnly five grenades can be active at a time");
        }
        public override void SetDefaults()
        {
            Item.damage = 66;
            Item.crit = 22;
            Item.knockBack = 9.2f;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 46;
            Item.height = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.rare = 6;
            Item.value = Item.buyPrice(gold: 4);
            Item.shoot = Mod.Find<ModProjectile>("PentaProj").Type;
            Item.shootSpeed = 17f;
            Item.useAmmo = ItemID.Grenade;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2 && nextProjIndex != -1)
                return base.CanUseItem(player);
            else if (player.altFunctionUse == 2 && projs.Any<Projectile>(X => X.active && X.type == Item.shoot))
            {
                for (int i = 0; i < projs.Length; i++)
                {
                    if (!projs[i].active || projs[i].type != Item.shoot) continue;
                    projs[i].GetGlobalProjectile<Projectiles.wfGlobalProj>().Explode(150);
                    projs[i].localNPCHitCooldown = -1;
                }
                projs = new Projectile[5] { new Projectile(), new Projectile(), new Projectile(), new Projectile(), new Projectile() };

                return false;
            }
            else return false;
        }
        int nextProjIndex
        {
            get
            {
                int n = Array.FindIndex<Projectile>(projs, x => !x.active || x.type != Item.shoot);
                if (n >= 0 && n < 5)
                {
                    projs[n] = new Projectile();
                    return n;
                }
                else
                    return -1;
            }
        }
        Projectile[] projs = new Projectile[5] { new Projectile(), new Projectile(), new Projectile(), new Projectile(), new Projectile() };
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var proj = ShootWith(position, speedX, speedY, Mod.Find<ModProjectile>("PentaProj").Type, damage, knockBack, offset: 16);
            projs[nextProjIndex] = proj;
            var gProj = proj.GetGlobalProjectile<Projectiles.wfGlobalProj>();
            if (player.GetModPlayer<wfPlayer>().napalmGrenades)
                gProj.AddProcChance(new ProcChance(BuffID.OnFire, 30));

            if (player.GetModPlayer<wfPlayer>().napalmGrenades)
                sound = Mod.GetSound("Sounds/PentaNapalmSound").CreateInstance();
            else sound = Mod.GetSound("Sounds/PentaSound").CreateInstance();
            sound.Volume = 0.5f;
            sound.Pitch += Main.rand.NextFloat(0f, 0.1f);
            sound.Play();

            return false;
        }
    }
}