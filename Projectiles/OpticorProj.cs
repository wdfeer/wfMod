using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wfMod.Projectiles
{
    internal class OpticorProj : ModProjectile
    {
        public override string Texture => "wfMod/EmptyTexture";
        wfGlobalProj globalProj;
        public Player owner;
        public Func<Vector2> getPositionNearThePlayer;
        public Func<Vector2> getBaseVelocity;
        public override void SetDefaults()
        {
            globalProj = projectile.GetGlobalProjectile<wfGlobalProj>();
            projectile.width = 32;
            projectile.height = 32;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.extraUpdates = 0;
            projectile.penetrate = -1;
            projectile.timeLeft = 200;
            projectile.hide = true;
            projectile.tileCollide = false;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 6;
        }
        bool playedSound = false;
        public override void AI()
        {
            if (projectile.timeLeft >= 95)
            {
                if (projectile.velocity != Vector2.Zero) projectile.velocity = Vector2.Zero;
                projectile.position = owner.position + getPositionNearThePlayer();
                var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 187, getBaseVelocity().X + owner.velocity.X, getBaseVelocity().Y + owner.velocity.Y)];
                dust.noGravity = true;

                if (projectile.timeLeft == 146 && !playedSound)
                {
                    playedSound = true;
                }
                if (owner.dead) projectile.Kill();
            }
            else
            {
                if (projectile.velocity == Vector2.Zero) projectile.velocity = getBaseVelocity();
                if (!projectile.tileCollide)
                    projectile.tileCollide = true;
                projectile.extraUpdates = 100;
                for (int num = 0; num < 8; num++)
                {
                    Vector2 position2 = projectile.position;
                    position2 -= projectile.velocity * (num * 0.25f);
                    int num353 = Dust.NewDust(position2, 1, 1, 180);
                    Dust dust = Main.dust[num353];
                    dust.position = position2;
                    dust.position.X += projectile.width / 2;
                    dust.position.Y += projectile.height / 2;
                    dust.scale = Main.rand.Next(70, 110) * 0.013f;
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            globalProj.Explode(144);
            for (int i = 0; i < projectile.width / 4; i++)
            {
                int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, 0f, 80, default(Color), 1.2f);
                var dust = Main.dust[dustIndex];
                dust.noGravity = true;
                dust.velocity *= 1.5f;
            }
            return false;
        }
        public override bool CanDamage()
        {
            if (projectile.extraUpdates == 0) return false;
            else return true;
        }
    }
}