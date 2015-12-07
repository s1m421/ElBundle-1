﻿namespace ElUtilitySuite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using ItemData = LeagueSharp.Common.Data.ItemData;

    /// <summary>
    ///     Casts Cleanse on dangerous spells.
    /// </summary>
    public class SpellCleanser
    {
        #region Static Fields

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        private static Spell cleanseSpell;

        private static SpellDataInst slot1;

        private static SpellDataInst slot2;

        private static SpellSlot summonerCleanse;

        private static Items.Item qss;

        private static Items.Item mikaels = ItemData.Mikaels_Crucible.GetItem();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     The Cleanse item
        /// </summary>
        /// <summary>
        ///     Initializes the <see cref="Cleanse" /> class.
        /// </summary>
        static SpellCleanser()
        {
            #region Spells Init

            Spells = new List<CleanseSpell>()
                         {
                             new CleanseSpell
                                 {
                                     ChampionName = "ashe", SDataName = "enchantedcrystalarrow",
                                     MissileName = "enchantedcrystalarrow", Delay = 250, MissileSpeed = 1600,
                                     CastRange = 20000f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "malzahar", SDataName = "alzaharnethergrasp", MissileName = "",
                                     Delay = 250, MissileSpeed = int.MaxValue, CastRange = 700f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "zed", SDataName = "zedr", MissileName = "", Delay = 250,
                                     MissileSpeed = int.MaxValue, CastRange = 850f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "fiddlesticks", SDataName = "terrify", MissileName = "", Delay = 250,
                                     MissileSpeed = int.MaxValue, CastRange = 575f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "shaco", SDataName = "jackinthebox", MissileName = "", Delay = 250,
                                     MissileSpeed = 1450, CastRange = 0f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "shen", SDataName = "shenshadowdash", MissileName = "shenshadowdash",
                                     Delay = 250, MissileSpeed = 1250, CastRange = 600f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "twistedfate", SDataName = "goldcardpreattack", MissileName = "",
                                     Delay = 250, MissileSpeed = int.MaxValue, CastRange = 600f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "evelynn", SDataName = "evelynnr", MissileName = "evelynnr",
                                     Delay = 250, MissileSpeed = int.MaxValue, CastRange = 900f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "hecarim", SDataName = "hecarimult", MissileName = "", Delay = 50,
                                     MissileSpeed = 1200, CastRange = 1350f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "lissandra", SDataName = "lissandrar", MissileName = "", Delay = 250,
                                     MissileSpeed = int.MaxValue, CastRange = 550f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "lissandra", SDataName = "lissandraw", MissileName = "", Delay = 250,
                                     MissileSpeed = int.MaxValue, CastRange = 450f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "leona", SDataName = "leonasolarflare",
                                     MissileName = "leonasolarflare", Delay = 1200, MissileSpeed = int.MaxValue,
                                     CastRange = 1200f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "leona", SDataName = "leonashieldofdaybreak", MissileName = "",
                                     Delay = 250, MissileSpeed = int.MaxValue, CastRange = 215f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "elise", SDataName = "elisehumane", MissileName = "elisehumane",
                                     Delay = 250, MissileSpeed = 1600, CastRange = 1075f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "cassiopeia", SDataName = "cassiopeiapetrifyinggaze",
                                     MissileName = "cassiopeiapetrifyinggaze", Delay = 350, MissileSpeed = int.MaxValue,
                                     CastRange = 875f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "braum", SDataName = "braumqmissle", MissileName = "", Delay = 250,
                                     MissileSpeed = 1200, CastRange = 1100f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "braum", SDataName = "braumq", MissileName = "braumqmissile",
                                     Delay = 250, MissileSpeed = 1200, CastRange = 1100f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "syndra", SDataName = "syndrar", MissileName = "", Delay = 450,
                                     MissileSpeed = 1250, CastRange = 675f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "ahri", SDataName = "ahriseduce", MissileName = "ahriseducemissile",
                                     Delay = 250, MissileSpeed = 1550, CastRange = 975f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "amumu", SDataName = "curseofthesadmummy", MissileName = "",
                                     Delay = 250, MissileSpeed = int.MaxValue, CastRange = 560f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "amumu", SDataName = "bandagetoss",
                                     MissileName = "sadmummybandagetoss", Delay = 250, MissileSpeed = 2000,
                                     CastRange = 1100f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "skarner", SDataName = "skarnerimpale", MissileName = "", Delay = 350,
                                     MissileSpeed = int.MaxValue, CastRange = 350f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "sejuani", SDataName = "sejuaniglacialprisoncast",
                                     MissileName = "sejuaniglacialprison", Delay = 250, MissileSpeed = 1600,
                                     CastRange = 1200f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "orianna", SDataName = "orianadetonatecommand",
                                     MissileName = "orianadetonatecommand", Delay = 500, MissileSpeed = int.MaxValue,
                                     CastRange = 425f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "nocturne", SDataName = "nocturneunspeakablehorror", MissileName = "",
                                     Delay = 250, MissileSpeed = int.MaxValue, CastRange = 350f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "nami", SDataName = "namiq", MissileName = "namiqmissile", Delay = 250,
                                     MissileSpeed = 1750, CastRange = 875f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "nami", SDataName = "namir", MissileName = "namirmissile", Delay = 250,
                                     MissileSpeed = 1200, CastRange = 2550f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "morgana", SDataName = "darkbindingmissile",
                                     MissileName = "darkbindingmissile", Delay = 250, MissileSpeed = 1200,
                                     CastRange = 1175f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "lux", SDataName = "luxlightbinding",
                                     MissileName = "luxlightbindingmis", Delay = 250, MissileSpeed = 1200,
                                     CastRange = 1300f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "mordekaiser", SDataName = "mordekaiserchildrenofthegrave",
                                     MissileName = "", Delay = 250, MissileSpeed = int.MaxValue, CastRange = 850f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "vladimir", SDataName = "vladimirhemoplague", MissileName = "",
                                     Delay = 250, MissileSpeed = int.MaxValue, CastRange = 875f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "fiora", SDataName = "fiorar", MissileName = "", Delay = 150,
                                     MissileSpeed = int.MaxValue, CastRange = 500f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "fizz", SDataName = "fizzmarinerdoom",
                                     MissileName = "fizzmarinerdoommissile", Delay = 250, MissileSpeed = 1300,
                                     CastRange = 1275f
                                 },
                             new CleanseSpell
                                 {
                                     ChampionName = "rammus", SDataName = "puncturingtaunt", MissileName = "", Delay = 250,
                                     MissileSpeed = int.MaxValue, CastRange = 325f
                                 }
                         };

            #endregion
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the spells.
        /// </summary>
        /// <value>
        ///     The spells.
        /// </value>
        public static List<CleanseSpell> Spells { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the player.
        /// </summary>
        /// <value>
        ///     The player.
        /// </value>
        private static Obj_AI_Hero Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static void Init()
        {
            // 3140 = Quicksilver
            // 3137 = Dervish Blade
            // 3193 = Mercurial Scimitar
            // 3222 = Mikael's Crucible

            slot1 = Entry.Player.Spellbook.GetSpell(SpellSlot.Summoner1);
            slot2 = Entry.Player.Spellbook.GetSpell(SpellSlot.Summoner2);

            //Soon riot will introduce multiple cleanses, mark my words.
            var cleanseNames = new[] { "summonerboost" };

            if (cleanseNames.Contains(slot1.Name))
            {
                cleanseSpell = new Spell(SpellSlot.Summoner1, 550f);
                summonerCleanse = SpellSlot.Summoner1;
            }
            if (cleanseNames.Contains(slot2.Name))
            {
                cleanseSpell = new Spell(SpellSlot.Summoner2, 550f);
                summonerCleanse = SpellSlot.Summoner2;
            }

            GameObject.OnCreate += GameObjectOnCreate;
            Obj_AI_Base.OnProcessSpellCast += ObjAiBaseOnProcessSpellCast;
            Game.OnUpdate += OnUpdate;
        }

        #endregion

        #region Methods

        private static void AllyCleanse()
        {
            var delay = InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10;

            foreach (var unit in
                ObjectManager.Get<Obj_AI_Hero>()
                    .Where(
                        x =>
                        x.IsAlly && !x.IsMe && x.IsValidTarget(900, false)
                        && InitializeMenu.Menu.Item("Protect.Cleanse.Kappa" + x.CharData.BaseSkinName).GetValue<bool>()
                        && InitializeMenu.Menu.Item("Protect.Cleanse.Mikaels.Activated").GetValue<bool>())
                    .OrderByDescending(xe => xe.Health / xe.MaxHealth * 100))
            {
                foreach (var b in unit.Buffs)
                {
                    if (mikaels.IsReady())
                    {
                        var buffMenuItem = InitializeMenu.Menu.Item(string.Format("Protect.Cleanse.{0}.Ally", b.Type.ToString()));
                        if (buffMenuItem != null && 
                            buffMenuItem.GetValue<bool>())
                        {
                            Utility.DelayAction.Add(delay, () => mikaels.Cast(unit));
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Fired when a game object is created.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void GameObjectOnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.IsValid<MissileClient>() || sender.IsAlly)
            {
                return;
            }

            var missile = (MissileClient)sender;
            var sdata =
                Spells.FirstOrDefault(
                    x =>
                    missile.SData.Name.ToLower().Equals(x.MissileName)
                    || missile.SData.Name.ToLower().Equals(x.SDataName));

            // Not in database
            if (sdata == null)
            {
                return;
            }

            if (!InitializeMenu.Menu.Item(string.Format("Cleanse{0}", sdata.SDataName)).IsActive()
                || !InitializeMenu.Menu.Item("CleanseDangerous").IsActive())
            {
                return;
            }

            // Correct the end position
            var endPosition = missile.EndPosition;

            if (missile.StartPosition.Distance(endPosition) > sdata.CastRange)
            {
                endPosition = missile.StartPosition
                              + Vector3.Normalize(endPosition - missile.StartPosition) * sdata.CastRange;
            }

            if (summonerCleanse != SpellSlot.Unknown
                && Entry.Player.Spellbook.CanUseSpell(cleanseSpell.Slot) == SpellState.Ready)
            {
                Utility.DelayAction.Add(
                     400 + (int)sdata.Delay + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                    () => Entry.Player.Spellbook.CastSpell(cleanseSpell.Slot, Entry.Player));
                return;
            }

            if (missile.SData.LineWidth + Player.BoundingRadius
                > Player.ServerPosition.To2D()
                      .Distance(
                          Player.ServerPosition.To2D()
                      .ProjectOn(missile.StartPosition.To2D(), endPosition.To2D())
                      .SegmentPoint))
            {
                if (qss.IsReady())
                {
                    Utility.DelayAction.Add(
                        400 + (int)sdata.Delay + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                        () => qss.Cast());
                    return;
                }

                if (mikaels.IsReady())
                {
                    Utility.DelayAction.Add(
                        400 + (int)sdata.Delay + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                        () => mikaels.Cast());
                    return;
                }

                if (ItemData.Dervish_Blade.GetItem().IsReady())
                {
                    Utility.DelayAction.Add(
                        400 + (int)sdata.Delay + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                        () => ItemData.Dervish_Blade.GetItem().Cast());
                    return;
                }
            }
        }

        /// <summary>
        ///     Fired when the game processes a spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private static void ObjAiBaseOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsAlly)
            {
                return;
            }

            var spellData =
                Spells.FirstOrDefault(
                    x => x.SDataName == args.SData.Name.ToLower() || x.MissileName == args.SData.Name.ToLower());

            if (spellData == null)
            {
                return;
            }

            if (!InitializeMenu.Menu.Item(string.Format("Cleanse{0}", spellData.SDataName)).IsActive()
                || !InitializeMenu.Menu.Item("CleanseDangerous").IsActive())
            {
                return;
            }

            if (Player.Distance(args.Start) > spellData.CastRange)
            {
                return;
            }

            // Targetted spells
            if (args.SData.TargettingType == SpellDataTargetType.Unit && args.Target.IsMe
                || args.SData.TargettingType == SpellDataTargetType.SelfAndUnit && args.Target.IsMe
                || args.SData.TargettingType == SpellDataTargetType.Self
                || args.SData.TargettingType == SpellDataTargetType.SelfAoe
                && Player.Distance(sender) < spellData.CastRange)
            {
                Console.WriteLine("Should cleanse");

                if (summonerCleanse != SpellSlot.Unknown
                    && Entry.Player.Spellbook.CanUseSpell(cleanseSpell.Slot) == SpellState.Ready)
                {
                    Utility.DelayAction.Add(
                          400 + (int)spellData.Delay + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                        () => Entry.Player.Spellbook.CastSpell(cleanseSpell.Slot, Entry.Player));
                    return;
                }

                if (qss.IsReady())
                {
                    Utility.DelayAction.Add(
                        400 + (int)spellData.Delay
                        + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                        () => qss.Cast());
                    return;
                }

                if (mikaels.IsReady())
                {
                    Utility.DelayAction.Add(
                        400 + (int)spellData.Delay
                        + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                        () => mikaels.Cast(Player));
                    return;
                }

                if (Game.MapId == GameMapId.TwistedTreeline || Game.MapId == GameMapId.CrystalScar)
                {
                    if (ItemData.Dervish_Blade.GetItem().IsReady())
                    {
                        Utility.DelayAction.Add(
                            400 + (int)spellData.Delay
                            + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                            () => ItemData.Dervish_Blade.GetItem().Cast());
                        return;
                    }
                }
                return;
            }

            // Anything besides a skillshot return
            if (!args.SData.TargettingType.ToString().Contains("Location")
                && args.SData.TargettingType != SpellDataTargetType.Cone)
            {
                return;
            }

            // Correct the end position
            var endPosition = args.End;

            if (args.Start.Distance(endPosition) > spellData.CastRange)
            {
                endPosition = args.Start + Vector3.Normalize(endPosition - args.Start) * spellData.CastRange;
            }

            // credits to kurisu
            var isLinear = args.SData.TargettingType == SpellDataTargetType.Cone || args.SData.LineWidth > 0;
            var width = isLinear && args.SData.TargettingType != SpellDataTargetType.Cone
                            ? args.SData.LineWidth
                            : (args.SData.CastRadius < 1 ? args.SData.CastRadiusSecondary : args.SData.CastRadius);

            if ((isLinear
                 && width + Player.BoundingRadius
                 > Player.ServerPosition.To2D()
                       .Distance(
                           Player.ServerPosition.To2D().ProjectOn(args.Start.To2D(), endPosition.To2D()).SegmentPoint))
                || (!isLinear && Player.Distance(endPosition) <= width + Player.BoundingRadius))
            {
                if (summonerCleanse != SpellSlot.Unknown
                    && Entry.Player.Spellbook.CanUseSpell(cleanseSpell.Slot) == SpellState.Ready)
                {
                    Utility.DelayAction.Add(
                          400 + (int)spellData.Delay + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                        () => Entry.Player.Spellbook.CastSpell(cleanseSpell.Slot, Entry.Player));
                    return;
                }

                if (qss.IsReady())
                {
                    Utility.DelayAction.Add(
                        400 + (int)spellData.Delay
                        + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                        () => qss.Cast());
                    return;
                }

                if (mikaels.IsReady())
                {
                    Utility.DelayAction.Add(
                       400 + (int)spellData.Delay
                        + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                        () => mikaels.Cast());
                    return;
                }

                if (Game.MapId == GameMapId.TwistedTreeline || Game.MapId == GameMapId.CrystalScar)
                {
                    if (ItemData.Dervish_Blade.GetItem().IsReady())
                    {
                        Utility.DelayAction.Add(
                           400 + (int)spellData.Delay
                            + InitializeMenu.Menu.Item("New.Cleanse.Delay").GetValue<Slider>().Value * 10,
                            () => ItemData.Dervish_Blade.GetItem().Cast());
                        return;
                    }
                }
            }
        }

        private static void OnUpdate(EventArgs args)
        {
            try
            {
                qss = ObjectManager.Player.InventoryItems.Any(item => item.Id == ItemId.Mercurial_Scimitar) ? ItemData.Mercurial_Scimitar.GetItem() : ItemData.Quicksilver_Sash.GetItem();

                if (Entry.Player.IsDead)
                {
                    return;
                }

                if (InitializeMenu.Menu.Item("Protect.Cleanse.Mikaels.Activated").GetValue<bool>())
                {
                    AllyCleanse();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }

        #endregion

        public class CleanseSpell
        {
            #region Public Properties

            /// <summary>
            ///     Gets or sets the cast range.
            /// </summary>
            /// <value>
            ///     The cast range.
            /// </value>
            public float CastRange { get; set; }

            /// <summary>
            ///     Gets or sets the name of the champion.
            /// </summary>
            /// <value>
            ///     The name of the champion.
            /// </value>
            public string ChampionName { get; set; }

            /// <summary>
            ///     Gets or sets the delay.
            /// </summary>
            /// <value>
            ///     The delay.
            /// </value>
            public float Delay { get; set; }

            /// <summary>
            ///     Gets or sets the name of the missile.
            /// </summary>
            /// <value>
            ///     The name of the missile.
            /// </value>
            public string MissileName { get; set; }

            /// <summary>
            ///     Gets or sets the missile speed.
            /// </summary>
            /// <value>
            ///     The missile speed.
            /// </value>
            public int MissileSpeed { get; set; }

            /// <summary>
            ///     Gets or sets the name of the s data.
            /// </summary>
            /// <value>
            ///     The name of the s data.
            /// </value>
            public string SDataName { get; set; }

            #endregion
        }
    }
}