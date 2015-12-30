﻿namespace ElRengarRevamped
{
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ItemData = LeagueSharp.Common.Data.ItemData;

    public class Standards
    {
        #region Static Fields

        private static readonly int[] BlueSmite = { 3706, 1400, 1401, 1402, 1403 };

        private static readonly int[] RedSmite = { 3715, 1415, 1414, 1413, 1412 };

        protected static readonly Dictionary<Spells, Spell> spells = new Dictionary<Spells, Spell>
                                                                         {
                                                                             {
                                                                                 Spells.Q,
                                                                                 new Spell(
                                                                                 SpellSlot.Q,
                                                                                 Orbwalking.GetRealAutoAttackRange(Player) + 100) // + 150
                                                                             },
                                                                             { Spells.W, new Spell(SpellSlot.W, 500) },
                                                                             { Spells.E, new Spell(SpellSlot.E, 1000) },
                                                                             { Spells.R, new Spell(SpellSlot.R, 2000) }
                                                                         };

        public static int LastSwitch;

        protected internal static Orbwalking.Orbwalker Orbwalker;

        protected static SpellSlot Ignite;

        protected static SpellSlot Smite;

        protected static Items.Item Youmuu;

        #endregion

        #region Public Properties

        public static string ScriptVersion
        {
            get
            {
                return typeof(Rengar).Assembly.GetName().Version.ToString();
            }
        }

        #endregion

        #region Properties

        protected static int Ferocity
        {
            get
            {
                return (int)ObjectManager.Player.Mana;
            }
        }

        protected static bool HasPassive
        {
            get
            {
                return Player.HasBuff("rengarpassivebuff");
            }
        }

        protected static Obj_AI_Hero Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        protected static bool RengarR
        {
            get
            {
                return Player.Buffs.Any(x => x.Name.Contains("RengarR"));
            }
        }

        #endregion

        #region Public Methods and Operators

        public static bool IsActive(string menuItem)
        {
            return MenuInit.Menu.Item(menuItem).GetValue<bool>();
        }

        #endregion

        #region Methods

        protected static float IgniteDamage(Obj_AI_Hero target)
        {
            if (Ignite == SpellSlot.Unknown || Player.Spellbook.CanUseSpell(Ignite) != SpellState.Ready)
            {
                return 0f;
            }
            return (float)Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);
        }

        protected static StringList IsListActive(string menuItem)
        {
            return MenuInit.Menu.Item(menuItem).GetValue<StringList>();
        }

        protected static void SmiteCombo()
        {
            if (BlueSmite.Any(id => Items.HasItem(id)))
            {
                Smite = Player.GetSpellSlot("s5_summonersmiteplayerganker");
                return;
            }

            if (RedSmite.Any(id => Items.HasItem(id)))
            {
                Smite = Player.GetSpellSlot("s5_summonersmiteduel");
                return;
            }

            Smite = Player.GetSpellSlot("summonersmite");
        }

        protected static void UseHydra()
        {
            if (Player.IsWindingUp)
            {
                return;
            }

            if (!ItemData.Tiamat_Melee_Only.GetItem().IsReady() && !ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady())
            {
                return;
            }

            ItemData.Tiamat_Melee_Only.GetItem().Cast();
            ItemData.Ravenous_Hydra_Melee_Only.GetItem().Cast();
        }

        #endregion
    }
}