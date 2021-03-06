using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria.ID;
using IL.Terraria.DataStructures;
using wfMod.Items.Accessories;

namespace wfMod
{
    public enum wfMessageType
    {
        DesecrateDamage,
        SyncPlayer,
        NapalmGrenadesChanged

    }
    public class wfMod : Mod
    {
        public static wfMod mod;
        public static bool EximusesEnabled => ModContent.GetInstance<wfServerConfig>().eximusSpawn;
        public override void PreUpdateEntities()
        {
            wfPlayer.thermiteRounds = false;
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            wfMessageType messageType = (wfMessageType)reader.ReadByte();
            switch (messageType)
            {
                case wfMessageType.DesecrateDamage:
                    whoAmI = reader.ReadByte();
                    var player = Main.player[whoAmI];
                    Desecrate.HurtByDesecration(player);
                    break;
                case wfMessageType.SyncPlayer:
                    whoAmI = reader.ReadByte();
                    player = Main.player[whoAmI];
                    player.GetModPlayer<wfPlayer>().napalmGrenades = reader.ReadBoolean();
                    break;
                case wfMessageType.NapalmGrenadesChanged:
                    whoAmI = reader.ReadByte();
                    player = Main.player[whoAmI];
                    player.GetModPlayer<wfPlayer>().napalmGrenades = reader.ReadBoolean();
                    break;
                default:
                    break;
            }
        }
        public override void Load()
        {
            mod = this;
        }
        public override void Unload()
        {
            mod = null;
        }
        public static int DefDamageReduction(int defense)
        {
            return (int)(defense * (Main.expertMode ? 0.75f : 0.5f));
        }
        public static bool Roll(float chance)
        {
            return Main.rand.NextFloat(100) < chance;
        }
        public static bool BossAlive()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].boss && Main.npc[i].active) return true;
            }
            return false;
        }
        public static void NewDustsCircleEdge(int count, Vector2 center, float radius, int type, Action<Dust> edit = null)
        {
            NewDustsCustom(count,
             () => Dust.NewDustPerfect(center + Main.rand.NextVector2CircularEdge(radius, radius), type),
             edit);
        }
        /// <summary>
        /// Spawns dusts in a circle and applies a velocity from the center of the circle to them
        /// </summary>
        public static void NewDustsCircleFromCenter(int count, Vector2 center, float radius, int type, float velocityMult, Action<Dust> edit = null)
        {
            NewDustsCircle(count, center, radius, type, (dust) =>
            {
                dust.velocity += Vector2.Normalize(dust.position - center) * velocityMult;
                if (edit != null)
                    edit(dust);
            });
        }
        public static void NewDustsCircle(int count, Vector2 center, float radius, int type, Action<Dust> edit = null)
        {
            NewDustsCustom(count,
             () => Dust.NewDustPerfect(center + Main.rand.NextVector2Circular(radius, radius), type),
             edit);
        }
        public static void NewDustsPerfect(int count, Vector2 position, int type, Action<Dust> edit = null)
        {
            NewDustsCustom(count,
             () => Dust.NewDustPerfect(position, type),
             edit);
        }
        /// <summary>
        /// Spawns dusts with the create function, calls the edit on each of them (if edit is specified)
        /// </summary>
        /// <param name="count">Number of dusts to spawn</param>
        /// <param name="create">Function that returns a dust</param>
        public static void NewDustsCustom(int count, Func<Dust> create, Action<Dust> edit = null)
        {
            for (int i = 0; i < count; i++)
            {
                Dust dust = create();
                if (edit != null)
                    edit(dust);
            }
        }
    }
}