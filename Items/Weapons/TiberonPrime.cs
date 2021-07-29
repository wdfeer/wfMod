using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace wdfeerMod.Items.Weapons
{
    public class TiberonPrime : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Always shoots golden bullets\nRight Click to switch between Auto, Burst and Semi-auto fire modes\n+40%, 50% or 70% Critical damage in Auto, Burst or Semi");
        }
        int mode = 1;
        public int Mode // 0 is Auto, 1 is Burst, 2 is Semi
        {
            get => mode;
            set
            {
                if (value > 2) value = 0;
                mode = value;
                SetDefaults();
            }
        }
        public override void SetDefaults()
        {
            switch (Mode)
            {
                case 0:
                    item.crit = 12;
                    item.useTime = 7;
                    item.useAnimation = 7;
                    item.autoReuse = true;
                    break;
                case 1:
                    item.crit = 24;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.autoReuse = false;
                    break;
                default:
                    item.crit = 26;
                    item.useTime = 10;
                    item.useAnimation = 10;
                    item.autoReuse = false;
                    break;
            }
            item.damage = 25;
            item.ranged = true;
            item.noMelee = true;
            item.width = 48;
            item.height = 24;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 3;
            item.value = Item.buyPrice(gold: 8);
            item.rare = 5;
            item.shoot = ProjectileID.GoldenBullet;
            item.shootSpeed = 17f;
            item.ammo = AmmoID.Bullet;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AvengerEmblem, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        Int64 lastModeChange;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.GetModPlayer<wdfeerPlayer>().longTimer - 30 > lastModeChange)
                {
                    Mode++;
                    lastModeChange = player.GetModPlayer<wdfeerPlayer>().longTimer;
                    Main.PlaySound(SoundID.Unlock);
                }
                return false;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Main.PlaySound(SoundID.Item11, position);
            if (Mode == 1)
            {
                var modPlayer = Main.LocalPlayer.GetModPlayer<wdfeerPlayer>();
                if (modPlayer.burstInterval == -1)
                {
                    modPlayer.offsetP = position - player.position;
                    modPlayer.burstItem = item.modItem;
                    modPlayer.burstInterval = 4;
                    modPlayer.burstsMax = 3;
                    modPlayer.burstCount = 1;
                    modPlayer.speedXP = speedX;
                    modPlayer.speedYP = speedY;
                    modPlayer.typeP = type;
                    modPlayer.damageP = damage;
                    modPlayer.knockbackP = knockBack;
                }
            }

            Vector2 spawnOffset = new Vector2(speedX, speedY);
            spawnOffset.Normalize();
            spawnOffset *= item.width;
            position += spawnOffset;
            Vector2 spread = new Vector2(speedY, -speedX);
            int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY) + spread * Main.rand.NextFloat(0.002f, -0.002f), type, damage, knockBack, Main.LocalPlayer.cHead);
            var projectile = Main.projectile[proj];
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 2;
            var gProj = projectile.GetGlobalProjectile<Projectiles.wdfeerGlobalProj>();
            gProj.slashChance = Mode == 0 ? 9 : 0;
            gProj.critMult = Mode == 0 ? 1.4f : (Mode == 1 ? 1.5f : 1.7f);

            return false;
        }
    }
}