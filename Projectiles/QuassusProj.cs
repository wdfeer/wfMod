using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace wfMod.Projectiles
{
    internal class QuassusProj : ModProjectile
    {
        public override string Texture => "wfMod/EmptyTexture";
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 8;
            Projectile.width = 8;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                var dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 53, Scale: 0.69f)];
                dust.noGravity = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.rand.Next(0,100) < Main.LocalPlayer.GetCritChance(DamageClass.Generic)) crit = true; else crit = false;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}