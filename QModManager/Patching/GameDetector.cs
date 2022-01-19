namespace QModManager.Patching
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using QModManager.API;
    using QModManager.Utility;

    internal class GameDetector
    {
        internal readonly QModGame CurrentlyRunningGame;
        internal readonly int CurrentGameVersion = -1;

        /// <summary>
        /// Game -> game version.
        /// 0 = no minimum version required.
        /// </summary>
        private static readonly Dictionary<QModGame, int> SupportedGameVersions = new Dictionary<QModGame, int>
        {
            { QModGame.GraveyardKeeper, 0 },
        };

        internal bool IsValidGameRunning => SupportedGameVersions.ContainsKey(CurrentlyRunningGame);
        internal int MinimumBuildVersion => IsValidGameRunning ? SupportedGameVersions[CurrentlyRunningGame] : -1;
        internal bool IsValidGameVersion => IsValidGameRunning && MinimumBuildVersion == 0 || (CurrentGameVersion > -1 && CurrentGameVersion >= MinimumBuildVersion);

        internal GameDetector()
        {
            bool isGraveyardKeeper = Directory.GetFiles(Environment.CurrentDirectory, "GraveyardKeeper.exe", SearchOption.TopDirectoryOnly).Length > 0
                                     || Directory.GetDirectories(Environment.CurrentDirectory, "GraveyardKeeper.app", SearchOption.TopDirectoryOnly).Length > 0;

            if (isGraveyardKeeper)
            {
                Logger.Info("Detected game: GraveyardKeeper");
                CurrentlyRunningGame = QModGame.GraveyardKeeper;
            }
            else
            {
                Logger.Fatal("A fatal error has occurred. No game executable was found!");
                throw new FatalPatchingException("No game executable was found!");
            }

            Logger.Info($"Game Version: {SNUtils.GetPlasticChangeSetOfBuild()} Build Date: {SNUtils.GetDateTimeOfBuild():dd-MMMM-yyyy}");
            Logger.Info($"Loading QModManager v{Assembly.GetExecutingAssembly().GetName().Version.ToStringParsed()}{(IsValidGameRunning && MinimumBuildVersion != 0 ? $" built for {CurrentlyRunningGame} v{MinimumBuildVersion}" : string.Empty)}...");
            Logger.Info($"Today is {DateTime.Today:dd-MMMM-yyyy}");

            CurrentGameVersion = SNUtils.GetPlasticChangeSetOfBuild(-1);
            if (!IsValidGameVersion)
            {
                Logger.Fatal("A fatal error has occurred. An invalid game version was detected!");
                throw new FatalPatchingException("An invalid game version was detected!");
            }
        }
    }
}
