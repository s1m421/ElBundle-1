﻿namespace ElUtilitySuite.Items.OffensiveItems
{
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    internal class Hydra : Item
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Item" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        public Hydra(Menu menu)
            : base(menu)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public override ItemId Id
        {
            get
            {
                return ItemId.Ravenous_Hydra_Melee_Only;
            }
        }

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        /// <value>
        ///     The name of the item.
        /// </value>
        public override string Name
        {
            get
            {
                return "Hydra";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Shoulds the use item.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldUseItem()
        {
            return this.Menu.Item("Hydracombo").IsActive() && this.ComboModeActive
                   && HeroManager.Enemies.Any(x => x.Distance(this.Player) < 400);
        }

        #endregion
    }
}