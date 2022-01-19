﻿namespace QModManager.Patching
{
    using System.Reflection;
    using System.Text.RegularExpressions;
    using QModManager.API;

    internal class QModPlaceholder : QMod, IQMod
    {
        /// <summary>
        /// The dummy <see cref="QMod"/> which is used to represent QModManager
        /// </summary>
        internal static QModPlaceholder QModManager { get; } = new QModPlaceholder
        {
            Id = "QModManager",
            DisplayName = "QModManager",
            Author = "QModManager",
            LoadedAssembly = Assembly.GetExecutingAssembly(),
            SupportedGame = QModGame.GraveyardKeeper,
        };

        private QModPlaceholder()
        {
            // Hide empty constructor. Only to be used for QModManager singlton.
            this.Status = ModStatus.Success;
        }

        internal QModPlaceholder(string name)
            : this(name, ModStatus.UnidentifiedMod)
        {
        }

        internal QModPlaceholder(string name, ModStatus status)
        {
            Regex regex = new Regex(Patcher.IDRegex);
            string cleanName = regex.Replace(name, "");

            this.Id = cleanName;
            this.DisplayName = name;
            this.Author = "Unknown";
            this.SupportedGame = QModGame.None;
            this.Enable = false;
            this.Status = status;
        }
    }
}
