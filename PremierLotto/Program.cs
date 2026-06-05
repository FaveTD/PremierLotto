using System;
using System.Collections.Generic;

namespace PremierLotto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            "***************************************".WriteCentered(ConsoleColor.Yellow);
            "** WELCOME TO PREMIER LOTTO 🤞       **".WriteCentered(ConsoleColor.Yellow);
            "***************************************".WriteCentered(ConsoleColor.Yellow);

            if (!GameStart.VerifyAgentAccess()) return;

            GameOption selectedOption = GameSettings.ShowMenuAndSelect();

            decimal userStake = GameStart.GetUserStake();

            bool allowDupes = GameSettings.GetDuplicatePreference();

            bool allowAlpha = GameSettings.GetAlphanumericPreference(selectedOption);

            GameSettings settings = new GameSettings(selectedOption, allowDupes, allowAlpha);

            Validation validator = new Validation();
            List<Player> playersList = new List<Player>();
            GameStart.RegisterAgents(validator, playersList);

            TournamentManager.RunTournament(playersList, settings, userStake);

            LeaderboardManager boardManager = new LeaderboardManager();
            boardManager.DisplayFinalResults(playersList, settings);
        }
    }
}
