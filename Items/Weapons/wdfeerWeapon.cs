using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace wdfeerMod.Items.Weapons
{
    public abstract class wdfeerWeapon : ModItem
    {
        //<summary>
        //Spawns and returns a projectile with extra parameters like spread multiplier and the horizontal offset of projectile's position 
        //</summary>
        public Projectile ShootWith(Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, float spreadMult = 0, float offset = 0, Terraria.Audio.LegacySoundStyle sound = null, int bursts = -1, int burstInterval = -1)
        {
            if (offset != 0)
            {
                Vector2 spawnOffset = new Vector2(speedX, speedY);
                spawnOffset.Normalize();
                spawnOffset *= item.width;
                position += spawnOffset;
            }

            if (bursts > 1 && burstInterval > 0)
            {
                var modPlayer = Main.LocalPlayer.GetModPlayer<wdfeerPlayer>();
                if (modPlayer.burstInterval == -1)
                {
                    modPlayer.offsetP = position - Main.LocalPlayer.position;
                    modPlayer.burstItem = this;
                    modPlayer.burstInterval = burstInterval;
                    modPlayer.burstsMax = bursts;
                    modPlayer.burstCount = 1;
                    modPlayer.speedXP = speedX;
                    modPlayer.speedYP = speedY;
                    modPlayer.typeP = type;
                    modPlayer.damageP = damage;
                    modPlayer.knockbackP = knockBack;
                }
            }

            if (sound != null) Main.PlaySound(sound, position);

            Vector2 spread = new Vector2(speedY, -speedX);
            var proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY) + spread * Main.rand.NextFloat(spreadMult, -spreadMult), type, damage, knockBack, Main.LocalPlayer.cHead);
            return Main.projectile[proj];
        }
    }
}