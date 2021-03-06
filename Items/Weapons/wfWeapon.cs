using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;
namespace wfMod.Items.Weapons
{
    public abstract class wfWeapon : ModItem
    {
        protected string pathToSound;
        protected SoundEffect soundEffect = null;
        protected SoundEffectInstance sound = null;
        public void PlaySound(float pitchMod = 0f, float volume = 1f)
        {
            if (soundEffect == null)
            {
                soundEffect = mod.GetSound(pathToSound);
            }
            if (sound != null)
            {
                if (sound.State == SoundState.Playing)
                    volume /= 2;
                sound.Dispose();
            }
            sound = soundEffect.CreateInstance();
            sound.Pitch += pitchMod;
            sound.Volume = volume;
            Main.PlaySoundInstance(sound);
        }
        public static Vector2 findOffset(float speedX, float speedY, float offset)
        {
            Vector2 spawnOffset = new Vector2(speedX, speedY);
            spawnOffset.Normalize();
            spawnOffset *= offset;
            return spawnOffset;
        }
        //<summary>
        //Spawns and returns a projectile with extra parameters like spread multiplier and the horizontal offset of projectile's position 
        //</summary>
        public Projectile ShootWith(Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, float spreadMult = 0, float offset = 0, Terraria.Audio.LegacySoundStyle sound = null, int bursts = -1, int burstInterval = -1)
        {
            if (offset != 0)
            {
                Vector2 muzzleOffset = findOffset(speedX, speedY, offset);
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                    position += muzzleOffset;
            }

            if (bursts > 1 && burstInterval > 0)
            {
                var modPlayer = Main.player[item.owner].GetModPlayer<wfPlayer>();
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
            int projNum = Projectile.NewProjectile(position, new Vector2(speedX, speedY) + spread * Main.rand.NextFloat(spreadMult, -spreadMult), type, damage, knockBack, item.owner);
            var projectile = Main.projectile[projNum];
            return projectile;
        }
    }
}