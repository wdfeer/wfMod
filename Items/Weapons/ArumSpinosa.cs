using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace wdfeerMod.Items.Weapons
{
    public class ArumSpinosa : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Throws 6 projectiles that inflict Slash and Venom");
        }
        public override void SetDefaults()
        {
            item.damage = 28; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
            item.crit = 5;
            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.width = 48; // hitbox width of the item
            item.height = 43; // hitbox height of the item
            item.useTime = 48; // The item's use time in ticks (60 ticks == 1 second.)
            item.useAnimation = 48; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            item.useStyle = ItemUseStyleID.SwingThrow; // how you use the item (swinging, holding out, etc)
            item.knockBack = 4; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
            item.value = Item.buyPrice(gold: 10); // how much the item sells for (measured in copper)
            item.rare = 5; // the color that the item's name will be in-game
            item.shoot = ModContent.ProjectileType<Projectiles.ArumSpinosaProj>();
            item.UseSound = SoundID.Item39;
            item.autoReuse = true;
            item.shootSpeed = 16f; // the speed of the projectile (measured in pixels per frame)
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
            recipe.AddIngredient(ItemID.SpiderFang, 8);
            
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        int proj = 0;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 spread = new Vector2(speedY, -speedX);
            for (int i = 0; i < 6; i++)
            {
                proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY) + spread * Main.rand.NextFloat(-0.15f, 0.15f), type, damage, knockBack, Main.LocalPlayer.cHead);
                Main.projectile[proj].GetGlobalProjectile<Projectiles.wdfeerGlobalProj>().AddProcChance(new ProcChance(mod.BuffType("SlashProc"),50));
            }
            Main.PlaySound(SoundID.Item1, position);

            return false;
        }
    }
}