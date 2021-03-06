using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using wfMod.Items.Accessories;
using wfMod.Items.Weapons;
using System.IO;

namespace wfMod.Items
{
    public class wfGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public bool energized;
        public wfGlobalItem()
        {
            energized = false;
        }
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            wfGlobalItem globalItem = new wfGlobalItem();
            wfGlobalItem oldGlobalItem = item.GetGlobalItem<wfGlobalItem>();
            globalItem.energized = oldGlobalItem.energized;
            return globalItem;
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(energized);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            energized = reader.ReadBoolean();
        }
        public override void SetDefaults(Item item)
        {
            base.SetDefaults(item);
            if (item.type == ItemID.Grenade) item.ammo = item.type;
        }
        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 offset = new Vector2(speedX, -speedY);
            offset *= Main.rand.NextFloat(-player.GetModPlayer<wfPlayer>().spreadMult, player.GetModPlayer<wfPlayer>().spreadMult);
            speedX += offset.X;
            speedY += offset.Y;
            if (mod != null)
                if (player.HasBuff(mod.BuffType("EnergyConversionBuff")) && item.magic)
                {
                    player.DelBuff(player.FindBuffIndex(mod.BuffType("EnergyConversionBuff")));
                }

            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            if (player.GetModPlayer<wfPlayer>().hypeThrusters)
            {
                maxAscentMultiplier *= 1.25f;
                constantAscend *= 1.25f;
            }
        }
    }
}