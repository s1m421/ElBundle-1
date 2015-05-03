﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using LeagueSharp.Common.Data;
using Collision = LeagueSharp.Common.Collision;


namespace ElKalista
{
    internal enum Spells
    {
        Q,
        W,
        E,
        R
    }

    internal class Kalista
    {
        public static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        public static Orbwalking.Orbwalker Orbwalker;
        private static Obj_AI_Hero ConnectedAlly;
        private static Dictionary<float, float> _incomingDamage = new Dictionary<float, float>();
        private static Dictionary<float, float> _instantDamage = new Dictionary<float, float>();
        public static float IncomingDamage
        {
            get { return _incomingDamage.Sum(e => e.Value) + _instantDamage.Sum(e => e.Value); }
        }



        public static Dictionary<Spells, Spell> spells = new Dictionary<Spells, Spell>()
        {
            { Spells.Q, new Spell(SpellSlot.Q, 1180) },
            { Spells.W, new Spell(SpellSlot.W, 5200) },
            { Spells.E, new Spell(SpellSlot.E, 1000) },
            { Spells.R, new Spell(SpellSlot.R, 1400) }
        };

        #region hitchance

        private static HitChance CustomHitChance
        {
            get { return GetHitchance(); }
        }

        private static HitChance GetHitchance()
        {
            switch (ElKalistaMenu._menu.Item("ElKalista.hitChance").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                case 3:
                    return HitChance.VeryHigh;
                default:
                    return HitChance.Medium;
            }
        }

        #endregion

        #region Gameloaded 

        public static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.BaseSkinName != "Kalista") return;
           
            Console.WriteLine("Injected");

            Notifications.AddNotification("ElKalista by jQuery v1.0.1.8", 5000);

            spells[Spells.Q].SetSkillshot(0.25f, 30f, 1700f, true, SkillshotType.SkillshotLine);

            ElKalistaMenu.Initialize();
            Game.OnUpdate += OnGameUpdate;
            Drawing.OnDraw += Drawings.Drawing_OnDraw;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        #endregion

        #region OnGameUpdate

        private static void OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead)
                return;

            var target = TargetSelector.GetTarget(spells[Spells.Q].Range, TargetSelector.DamageType.Physical);

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo(target);
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();
                    JungleClear();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass(target);
                    break;
            }

            //KsMode();
            //JungleStealMode();
            //AutoHarassMode(target);

            SaveMode();
            SemiUltMode();
            AutoCastEMode();
        }

        #endregion

        public static float GetComboDamage(Obj_AI_Base enemy)
        {
            float damage = 0;

            if (spells[Spells.E].IsReady())
            {
                damage += spells[Spells.E].GetDamage(enemy);
            }

            return damage;
        }

        #region SuperSeCrEtSeTtInGs

       /* private static void AutoHarassMode(Obj_AI_Base target)
        {
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (ElKalistaMenu._menu.Item("ElKalista.AutoHarass").GetValue<KeyBind>().Active)
            {
                if (Player.ManaPercent < ElKalistaMenu._menu.Item("ElKalista.harass.mana").GetValue<Slider>().Value)
                {
                    return;
                }

                var q = ElKalistaMenu._menu.Item("ElKalista.UseQAutoHarass").GetValue<bool>();

                if (q && spells[Spells.Q].IsReady() && Player.Distance(target) <= 1000 &&
                    !Player.IsDashing() && !Player.IsWindingUp)
                {
                    var prediction = spells[Spells.Q].GetPrediction(target).Hitchance;
                    if (prediction >= CustomHitChance)
                        spells[Spells.Q].Cast(target);
                }
            }
        }*/

        private static void AutoCastEMode()
        {
            var target = TargetSelector.GetTarget(spells[Spells.E].Range, TargetSelector.DamageType.Physical);

            if (target == null || !target.IsValidTarget())
                return;

            var getEstacks =
                target.Buffs.Find(b => b.Caster.IsMe && b.IsValidBuff() && b.DisplayName == "KalistaExpungeMarker");

            if (getEstacks == null)
            {
                return;
            }

            var useE = ElKalistaMenu._menu.Item("ElKalista.E.Auto").GetValue<bool>();
            var useEStacks = ElKalistaMenu._menu.Item("ElKalista.E.Stacks").GetValue<Slider>().Value;

            if (spells[Spells.E].IsReady() && useE && getEstacks.Count >= useEStacks)
            {
                if (spells[Spells.E].GetDamage(target) > target.Health)
                {
                    spells[Spells.E].Cast(true);
                }
            }
            else if (target.ServerPosition.Distance(Player.ServerPosition, true) >
              Math.Pow(spells[Spells.E].Range * 0.8, 2) || getEstacks.EndTime - Game.Time < 0.3)
            {
                spells[Spells.E].Cast(true);
            }
        }

        private static void SemiUltMode()
        {
            var manualUlt = ElKalistaMenu._menu.Item("ElKalista.SemiR").GetValue<KeyBind>().Active;

            if (spells[Spells.R].IsReady() && manualUlt)
            {
                spells[Spells.R].Cast();
            }
        }

        private static void SaveMode()
        {
            if (Player.IsRecalling() || Player.InFountain())
                return;

            var save = ElKalistaMenu._menu.Item("ElKalista.misc.save").GetValue<bool>();
            var allyHp = ElKalistaMenu._menu.Item("ElKalista.misc.allyhp").GetValue<Slider>().Value;

            if (save)
            {
                if (ConnectedAlly == null)
                {
                    foreach (var cAlly in from ally in ObjectManager.Get<Obj_AI_Hero>().Where(b => b.IsAlly && !b.IsDead && !b.IsMe) where Player.Distance(ally) < spells[Spells.R].Range from buff in ally.Buffs where ally.HasBuff("kalistacoopstrikeally") select ally)
                    {
                        ConnectedAlly = cAlly;
                        break;
                    }
                }
                else
                {
                    // || IncomingDamage > ConnectedAlly.Health
                    if (ConnectedAlly.HealthPercent < allyHp && ConnectedAlly.CountEnemiesInRange(500) > 0)
                    {
                        spells[Spells.R].Cast();
                    }
                }
            }
        }

        //credits to hellsing
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy)
            {
                if (ConnectedAlly != null && spells[Spells.R].IsReady())
                {
                    if ((!(sender is Obj_AI_Hero) || args.SData.IsAutoAttack()) && args.Target != null && args.Target.NetworkId == ConnectedAlly.NetworkId)
                    {
                        _incomingDamage.Add(ConnectedAlly.ServerPosition.Distance(sender.ServerPosition) / args.SData.MissileSpeed + Game.Time, (float)sender.GetAutoAttackDamage(ConnectedAlly));
                    }
                    else if (sender is Obj_AI_Hero)
                    {
                        var attacker = (Obj_AI_Hero)sender;
                        var slot = attacker.GetSpellSlot(args.SData.Name);

                        if (slot != SpellSlot.Unknown)
                        {
                            if (slot == attacker.GetSpellSlot("SummonerDot") && args.Target != null && args.Target.NetworkId == ConnectedAlly.NetworkId)
                            {
                                _instantDamage.Add(Game.Time + 2, (float)attacker.GetSummonerSpellDamage(ConnectedAlly, Damage.SummonerSpell.Ignite));
                            }
                            else if (slot.HasFlag(SpellSlot.Q | SpellSlot.W | SpellSlot.E | SpellSlot.R) &&
                                ((args.Target != null && args.Target.NetworkId == ConnectedAlly.NetworkId) ||
                                args.End.Distance(ConnectedAlly.ServerPosition) < Math.Pow(args.SData.LineWidth, 2)))
                            {
                                _instantDamage.Add(Game.Time + 2, (float)attacker.GetSpellDamage(ConnectedAlly, slot));
                            }
                        }
                    }
                }
            }
        }

        /*private static void KsMode()
        {
            var useKs = ElKalistaMenu._menu.Item("ElKalista.misc.ks").GetValue<bool>();
            if (!useKs)
            {
                return;
            }

            var target =
                HeroManager.Enemies.FirstOrDefault(
                    x =>
                        !x.HasBuffOfType(BuffType.Invulnerability) && !x.HasBuffOfType(BuffType.SpellShield) &&
                        spells[Spells.E].CanCast(x) && (x.Health + (x.HPRegenRate / 2)) <= spells[Spells.E].GetDamage(x));

            if (spells[Spells.E].IsReady() && spells[Spells.E].CanCast(target))
            {
                spells[Spells.E].Cast();
            }

            if (spells[Spells.Q].IsReady() && spells[Spells.Q].CanCast(target))
            {
                spells[Spells.Q].Cast();
            }
        }*/

       /* private static void JungleStealMode()
        {
            var useJsm = ElKalistaMenu._menu.Item("ElKalista.misc.junglesteal").GetValue<bool>();

            if (!useJsm)
            {
                return;
            }

            var jMob =
                MinionManager.GetMinions(
                    Player.ServerPosition, spells[Spells.E].Range, MinionTypes.All, MinionTeam.All,
                    MinionOrderTypes.MaxHealth).FirstOrDefault(x => spells[Spells.E].GetDamage(x) > x.Health);

            if (spells[Spells.E].CanCast(jMob))
            {
                spells[Spells.E].Cast(jMob);
            }
        }*/

        #endregion

        #region itemusage

        private static void Items(Obj_AI_Base target)
        {
            var botrk = ItemData.Blade_of_the_Ruined_King.GetItem();
            var ghost = ItemData.Youmuus_Ghostblade.GetItem();
            var cutlass = ItemData.Bilgewater_Cutlass.GetItem();

            var useYoumuu = ElKalistaMenu._menu.Item("ElKalista.Items.Youmuu").GetValue<bool>();
            var useCutlass = ElKalistaMenu._menu.Item("ElKalista.Items.Cutlass").GetValue<bool>();
            var useBlade = ElKalistaMenu._menu.Item("ElKalista.Items.Blade").GetValue<bool>();

            var useBladeEhp = ElKalistaMenu._menu.Item("ElKalista.Items.Blade.EnemyEHP").GetValue<Slider>().Value;
            var useBladeMhp = ElKalistaMenu._menu.Item("ElKalista.Items.Blade.EnemyMHP").GetValue<Slider>().Value;

            if (botrk.IsReady() && botrk.IsOwned(Player) && botrk.IsInRange(target) &&
                target.HealthPercent <= useBladeEhp && useBlade)
            {
                botrk.Cast(target);
            }

            if (botrk.IsReady() && botrk.IsOwned(Player) && botrk.IsInRange(target) &&
                Player.HealthPercent <= useBladeMhp && useBlade)
            {
                botrk.Cast(target);
            }

            if (cutlass.IsReady() && cutlass.IsOwned(Player) && cutlass.IsInRange(target) &&
                target.HealthPercent <= useBladeEhp && useCutlass)
            {
                cutlass.Cast(target);
            }

            if (ghost.IsReady() && ghost.IsOwned(Player) && target.IsValidTarget(spells[Spells.Q].Range) && useYoumuu)
            {
                ghost.Cast();
            }
        }

        #endregion

        #region Harass

        private static void Harass(Obj_AI_Base target)
        {
            if (target == null || !target.IsValidTarget())
                return;

            if (Player.ManaPercent < ElKalistaMenu._menu.Item("ElKalista.minmanaharass").GetValue<Slider>().Value)
                return;

            var harassQ = ElKalistaMenu._menu.Item("ElKalista.Harass.Q").GetValue<bool>();

            if (harassQ && spells[Spells.Q].IsReady())
            {
                if (spells[Spells.Q].GetPrediction(target).Hitchance >= CustomHitChance)
                {
                    spells[Spells.Q].Cast(target);
                }
            }
        }

        #endregion

        #region Combo

        private static void Combo(Obj_AI_Base target)
        {
            if (target == null || !target.IsValidTarget())
                return;

            Items(target);

            var comboQ = ElKalistaMenu._menu.Item("ElKalista.Combo.Q").GetValue<bool>();
            var comboE = ElKalistaMenu._menu.Item("ElKalista.Combo.E").GetValue<bool>();

            if (comboQ && spells[Spells.Q].IsReady())
            {
                var qtarget = TargetSelector.GetTarget(spells[Spells.Q].Range, TargetSelector.DamageType.Physical);

                if (spells[Spells.Q].CanCast(qtarget) && spells[Spells.Q].GetPrediction(qtarget).Hitchance >= CustomHitChance && !Player.IsWindingUp && !Player.IsDashing())
                    spells[Spells.Q].Cast(qtarget);
            }

            var getEstacks =
                target.Buffs.Find(b => b.Caster.IsMe && b.IsValidBuff() && b.DisplayName == "KalistaExpungeMarker");

            if (getEstacks == null)
            {
                return;
            }

            var useE = ElKalistaMenu._menu.Item("ElKalista.ComboE.Auto").GetValue<bool>();
            var useEStacks = ElKalistaMenu._menu.Item("ElKalista.E.Stacks").GetValue<Slider>().Value;

            if (useE && comboE && spells[Spells.E].IsReady())
            {
                if (spells[Spells.E].IsInRange(target) && (target.IsRendKillable() || getEstacks.Count >= useEStacks))
                {
                    if (target.IsRendKillable())
                    {
                        spells[Spells.E].Cast(true);
                    }
                }
                else
                {
                    if (getEstacks.EndTime - Game.Time < 0.3)
                    {
                        spells[Spells.E].Cast(true);
                    }
                }
            }
        }

        #endregion

        #region Laneclear

        private static void JungleClear()
        {
            if (Player.ManaPercent < ElKalistaMenu._menu.Item("minmanaclear").GetValue<Slider>().Value)
                return;

            var useQ = ElKalistaMenu._menu.Item("useQFarmJungle").GetValue<bool>();
            var useE = ElKalistaMenu._menu.Item("useEFarmJungle").GetValue<bool>();

            var minions = MinionManager.GetMinions(
                Player.ServerPosition, spells[Spells.Q].Range, MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (minions.Count < 0)
                return;

            foreach (var minion in minions)
            {
                if (spells[Spells.Q].IsReady() && useQ)
                {
                    spells[Spells.Q].Cast(minion.ServerPosition);
                }

                if (spells[Spells.E].IsReady() && useE &&
                    minions[0].Health + minions[0].HPRegenRate / 2 < spells[Spells.E].GetDamage(minion))
                {
                    spells[Spells.E].Cast();
                }
            }
        }

        private static List<Obj_AI_Base> QGetCollisionMinions(Obj_AI_Hero source, Vector3 targetposition)
        {
            var input = new PredictionInput
            {
                Unit = source,
                Radius = spells[Spells.Q].Width,
                Delay = spells[Spells.Q].Delay,
                Speed = spells[Spells.Q].Speed,
            };

            input.CollisionObjects[0] = CollisionableObjects.Minions;

            return
                Collision.GetCollision(new List<Vector3> { targetposition }, input)
                    .OrderBy(obj => obj.Distance(source, false))
                    .ToList();
        }

        private static void LaneClear()
        {
            var useQ = ElKalistaMenu._menu.Item("useQFarm").GetValue<bool>();
            var useE = ElKalistaMenu._menu.Item("useQFarm").GetValue<bool>();
            var countMinions = ElKalistaMenu._menu.Item("ElKalista.Count.Minions").GetValue<Slider>().Value;
            var countMinionsE = ElKalistaMenu._menu.Item("ElKalista.Count.Minions.E").GetValue<Slider>().Value;

            if (Player.ManaPercent < ElKalistaMenu._menu.Item("minmanaclear").GetValue<Slider>().Value)
                return;

            var minions = MinionManager.GetMinions(Player.ServerPosition, spells[Spells.E].Range);

            if (minions.Count <= 0)
            {
                return;
            }

            if (spells[Spells.Q].IsReady() && useQ)
            {
                foreach (var minion in minions.Where(x => x.Health <= spells[Spells.Q].GetDamage(x)))
                {
                    var killcount = 0;

                    foreach (
                        var colminion in
                            QGetCollisionMinions(
                                Player, Player.ServerPosition.Extend(minion.ServerPosition, spells[Spells.Q].Range)))
                    {
                        if (colminion.Health <= spells[Spells.Q].GetDamage(colminion))
                        {
                            killcount++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (killcount >= countMinions && (!Player.IsWindingUp && !Player.IsDashing()))
                    {
                        spells[Spells.Q].Cast(minion.ServerPosition);
                        break;
                    }
                }
            }

            if (!useE || !spells[Spells.E].IsReady())
            {
                return;
            }

            var minionkillcount =
                minions.Count(x => spells[Spells.E].CanCast(x) && x.Health <= spells[Spells.E].GetDamage(x));

            if (minionkillcount >= countMinionsE)
            {
                spells[Spells.E].Cast();
            }
        }

        #endregion
    }
}
