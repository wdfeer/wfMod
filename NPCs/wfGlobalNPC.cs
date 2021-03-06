using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using System;
using System.Collections.Generic;
using wfMod.Items.Weapons;
using wfMod.Items.Accessories;

namespace wfMod
{
    public class wfGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public List<StackableProc> procs = new List<StackableProc>();
        public static bool thermiteRounds;
        public void AddStackableProc(ProcType type, int duration, int damage)
        {
            StackableProc proc = new StackableProc(type, damage, null, duration);
            proc.OnEnd = () => procs.Remove(proc);
            procs.Add(proc);
        }
        public override void ResetEffects(NPC npc)
        {
            thermiteRounds = false;
        }
        const int stackableProcTickInterval = 30;
        int stackableProcTickTimer = 0;
        private int GetTickDamageOfStackableProcs(NPC npc)
        {
            int damage = 0;
            if (npc.HasBuff(BuffID.Electrified))
            {
                int totalDamage = 0;
                for (int i = 0; i < procs.Count; i++)
                    totalDamage += procs[i].type == ProcType.Electricity ? procs[i].dmg : 0;
                damage += totalDamage;
            }
            if (npc.HasBuff(mod.BuffType("SlashProc")))
            {
                int totalDamage = 0;
                for (int i = 0; i < procs.Count; i++)
                    totalDamage += procs[i].type == ProcType.Slash ? procs[i].dmg : 0;
                damage += totalDamage;
            }
            return damage * stackableProcTickInterval / 60;
        }
        public bool corrosion = false;
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            corrosion = (npc.HasBuff(BuffID.Electrified) && npc.HasBuff(BuffID.Poisoned)) || (npc.HasBuff(BuffID.Electrified) && npc.HasBuff(BuffID.Venom));
            if ((npc.HasBuff(BuffID.Electrified) || npc.HasBuff(mod.BuffType("SlashProc"))))
            {
                npc.lifeRegen = 0;
                stackableProcTickTimer++;
                if (stackableProcTickTimer >= stackableProcTickInterval)
                {
                    int tickDmg = GetTickDamageOfStackableProcs(npc);
                    {
                        int initialDef = npc.defense;
                        npc.defense = 0;
                        npc.StrikeNPC(tickDmg, 0, 0);
                        npc.defense = initialDef;
                    }
                    stackableProcTickTimer = 0;
                }
            }
            else
            {
                procs = new List<StackableProc>();
                stackableProcTickTimer = 0;
            }
            if (npc.HasBuff(BuffID.OnFire) && wfPlayer.thermiteRounds)
            {
                npc.lifeRegen -= 12;
            }
        }
        public override void AI(NPC npc)
        {
            if (npc.HasBuff(BuffID.Electrified))
            {
                for (int i = 0; i < (npc.width < 32 ? 1 : npc.width / 32); i++)
                {
                    int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, 0f, 67, default(Color), 0.5f);
                    var dust = Main.dust[dustIndex];
                    dust.velocity *= 0.6f;
                    dust.noGravity = true;
                }
            }
            if (npc.HasBuff(BuffID.Weak))
            {
                for (int i = 0; i < (npc.width < 48 ? 1 : npc.width / 48); i++)
                {
                    int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, 1, 0f, 0f, 67, default(Color), 0.35f);
                    var dust = Main.dust[dustIndex];
                    dust.velocity *= 0.2f;
                    dust.noGravity = true;
                }
            }
            if (npc.HasBuff(BuffID.Electrified) || npc.HasBuff(mod.BuffType("SlashProc")))
                for (int i = 0; i < procs.Count; i++)
                    procs[i].Update();

            bool bossAlive = wfMod.BossAlive();
            if (!bossAlive)
            {
                if (npc.HasBuff(BuffID.Frozen))
                {
                    npc.velocity *= 0f;

                    for (int i = 0; i < (npc.width < 48 ? 1 : npc.width / 48); i++)
                    {
                        int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, 67, 0f, 0f, 67, default(Color), 1f);
                        var dust = Main.dust[dustIndex];
                        dust.noGravity = true;
                    }
                }
                else if (npc.HasBuff(BuffID.Slow))
                {
                    npc.velocity *= 0.9f;

                    for (int i = 0; i < (npc.width < 48 ? 1 : npc.width / 48); i++)
                    {
                        int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, 68, 0f, 0f, 67, default(Color), 0.6f);
                        var dust = Main.dust[dustIndex];
                        dust.velocity *= 0.2f;
                        dust.noGravity = true;
                    }
                }
            }
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            if (npc.HasBuff(BuffID.Weak))
            {
                if (npc.boss)
                    damage = (int)(damage * 0.8f);
                else
                    damage /= 2;
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.HasBuff(BuffID.OnFire))
            {
                float defenseDmgReduction = wfMod.DefDamageReduction(npc.defense);
                float compensation = defenseDmgReduction * 0.1f;
                damage += (int)compensation;
            }
            if (corrosion)
            {
                damage += wfMod.DefDamageReduction(npc.defense) / 2;
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (npc.HasBuff(BuffID.OnFire))
            {
                float defenseDmgReduction = wfMod.DefDamageReduction(npc.defense);
                float compensation = defenseDmgReduction * 0.1f;
                damage += (int)compensation;
            }
            if (corrosion)
            {
                damage += wfMod.DefDamageReduction(npc.defense) / 2;
            }

            if (Main.player[projectile.owner].GetModPlayer<wfPlayer>().synthDeconstruct && projectile.minion && heartDropChance < SynthDeconstruct.heartDropChance)
                heartDropChance = SynthDeconstruct.heartDropChance;
        }
        public int[] martianTypes = { NPCID.MartianDrone, NPCID.MartianEngineer, NPCID.MartianSaucerCore, NPCID.MartianTurret, NPCID.MartianOfficer, NPCID.MartianWalker };
        public int[] goblins = { NPCID.GoblinArcher, NPCID.GoblinSorcerer, NPCID.GoblinWarrior, NPCID.GoblinThief, NPCID.GoblinSummoner };
        public int heartDropChance = 0; //Percent
        public override void NPCLoot(NPC npc)
        {
            if (npc.SpawnedFromStatue) return;

            switch (npc.type)
            {
                case NPCID.GoblinSorcerer when wfMod.Roll(2f):
                augurSecrets:
                    DropItem(npc, ModContent.ItemType<AugurSecrets>());
                    break;
                case NPCID.DD2DarkMageT1 when wfMod.Roll(20):
                    goto augurSecrets;
                case NPCID.GoblinArcher when wfMod.Roll(3f):
                    DropItem(npc, ModContent.ItemType<AugurMessage>());
                    break;
                case NPCID.GoblinWarrior when wfMod.Roll(3f):
                    DropItem(npc, ModContent.ItemType<AugurAccord>());
                    break;
                case 481 when wfMod.Roll(8): //Hoplite
                    DropItem(npc, ModContent.ItemType<ShieldCharger>());
                    break;
                case NPCID.GraniteFlyer when wfMod.Roll(8):
                    DropItem(npc, ModContent.ItemType<Guardian>());
                    break;
                case NPCID.DemonEye when wfMod.Roll(0.75f):
                    DropItem(npc, ModContent.ItemType<PiercingHit>());
                    break;
                case NPCID.BigMimicCorruption when wfMod.Roll(20):
                    DropItem(npc, ModContent.ItemType<ArgonScope>());
                    break;
                case NPCID.BigMimicCrimson when wfMod.Roll(20):
                    DropItem(npc, ModContent.ItemType<ArgonScope>());
                    break;
                case NPCID.FireImp when wfMod.Roll(6):
                    DropItem(npc, ModContent.ItemType<Blaze>());
                    break;
                case NPCID.Hellbat when wfMod.Roll(5):
                    DropItem(npc, ModContent.ItemType<ThermiteRounds>());
                    break;
                case NPCID.DarkCaster when wfMod.Roll(6):
                    DropItem(npc, ModContent.ItemType<Simulor>());
                    break;
                case NPCID.EyeofCthulhu when !Main.expertMode && wfMod.Roll(50):
                    Drop1ItemAtRandom(npc, new int[] { ModContent.ItemType<Sobek>(), ModContent.ItemType<Redirection>() });
                    break;
                case NPCID.BrainofCthulhu when !Main.expertMode && wfMod.Roll(33):
                    DropItem(npc, ModContent.ItemType<GorgonWraith>());
                    break;
                case NPCID.QueenBee when !Main.expertMode:
                    Drop1ItemAtRandom(npc, new int[] { ModContent.ItemType<Shred>(), ModContent.ItemType<Kohm>(), mod.ItemType("Vigor") });
                    break;
                case NPCID.SkeletronHead when !Main.expertMode && wfMod.Roll(40):
                    Drop1ItemAtRandom(npc, new int[] { ModContent.ItemType<InternalBleeding>(), ModContent.ItemType<Cestra>() });
                    break;
                case NPCID.WallofFlesh when !Main.expertMode && wfMod.Roll(33):
                    Drop1ItemAtRandom(npc, new int[] { ModContent.ItemType<QuickThinking>(), ModContent.ItemType<EnergyConversion>() });
                    break;
                case NPCID.TheDestroyer when !Main.expertMode && wfMod.Roll(25):
                    DropItem(npc, ModContent.ItemType<StasisField>());
                    break;
                case NPCID.Retinazer when !Main.expertMode && wfMod.Roll(12):
                Acceltra:
                    DropItem(npc, ModContent.ItemType<Acceltra>());
                    break;
                case NPCID.Spazmatism when !Main.expertMode && wfMod.Roll(12):
                    goto Acceltra;
                case NPCID.SkeletronPrime when !Main.expertMode && wfMod.Roll(25):
                    DropItem(npc, ModContent.ItemType<SecuraPenta>());
                    break;
                case NPCID.Plantera when !Main.expertMode && wfMod.Roll(25):
                    DropItem(npc, ModContent.ItemType<SynergizedProspectus>());
                    break;
                case NPCID.DukeFishron when !Main.expertMode && wfMod.Roll(33):
                    DropItem(npc, ModContent.ItemType<Items.Weapons.Summon.Oxylus>());
                    break;
                default:
                    if (martianTypes.Contains(npc.type) && wfMod.Roll(2))
                        DropItem(npc, ModContent.ItemType<Items.Fieldron>());
                    else if (goblins.Contains(npc.type) && wfMod.Roll(3))
                    {
                        Drop1ItemAtRandom(npc, new int[] { ModContent.ItemType<HunterMunitions>(), ModContent.ItemType<Penta>(), ModContent.ItemType<Tonkor>() });
                    }
                    break;
            }
            if (npc.rarity > 0 && !npc.friendly && wfMod.Roll(25))
            {
                int[] options = { ModContent.ItemType<VileAcceleration>(), ModContent.ItemType<CriticalDelay>(), ModContent.ItemType<HollowPoint>(), ModContent.ItemType<HeavyCaliber>() };
                Drop1ItemAtRandom(npc, options);
            }
            if (npc.boss && Main.expertMode && wfMod.Roll(15))
            {
                int[] options = { ModContent.ItemType<ArcaneAvenger>(), ModContent.ItemType<ArcaneGuardian>(), ModContent.ItemType<ArcaneAcceleration>(), ModContent.ItemType<ArcaneStrike>(), ModContent.ItemType<ArcaneEnergize>(), ModContent.ItemType<ArcanePulse>() };
                Drop1ItemAtRandom(npc, options);
            }

            if (wfMod.Roll(heartDropChance))
                DropItem(npc, ItemID.Heart);

            if (!npc.boss && !desecrated && !npc.friendly)
            {
                Player[] players = Main.player;
                for (int i = 0; i < players.Length && !desecrated; i++)
                {
                    var player = players[i];
                    desecrated = Desecrate.CanExtraLoot(player, npc);
                    if (desecrated)
                    {
                        npc.NPCLoot();
                    }
                }
            }
        }
        private bool desecrated = false;
        private void DropItem(NPC npc, int itemType)
        {
            Item.NewItem(npc.getRect(), itemType);
        }
        private void Drop1ItemAtRandom(NPC npc, IList<int> types)
        {
            int rand = Main.rand.Next(types.Count());
            DropItem(npc, types[rand]);
        }
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            int next = nextSlot;
            void SetShopItem(int itemType)
            {
                shop.item[next].SetDefaults(itemType);
                next++;
            }
            if (type == NPCID.TravellingMerchant)
            {
                if (NPC.downedQueenBee)
                    SetShopItem(ModContent.ItemType<PrismaTetra>());
                if (NPC.downedMechBossAny)
                    SetShopItem(ModContent.ItemType<PrismaGorgon>());
                if (NPC.downedPlantBoss)
                    SetShopItem(ModContent.ItemType<PrimedVigor>());
            }
            nextSlot = next;
        }
    }
}