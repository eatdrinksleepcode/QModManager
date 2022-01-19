namespace QModManager.API
{
    using System;

    /// <summary>
    /// Identifies the Subnautica games.
    /// </summary>
    [Flags]
    public enum QModGame
    {
        /// <summary>
        /// No game.
        /// </summary>
        None = 0b00,

        /// <summary>
        /// Subnautica.
        /// </summary>
        GraveyardKeeper = 0b01,
    }
}
