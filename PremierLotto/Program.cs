using PremierLotto.Data;
using PremierLotto.Game;
using PremierLotto.Models;
using PremierLotto.Utilities;
using PremierLotto.Finance;
using System;
using System.Collections.Generic;

namespace PremierLotto.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            "***************************************".WriteCentered(ConsoleColor.Yellow);
            "** WELCOME TO PREMIER Lotto 🤞       **".WriteCentered(ConsoleColor.Yellow);
            "***************************************".WriteCentered(ConsoleColor.Yellow);

            if (!GameStart.VerifyAgentAccess()) return;

            GameOption selectedOption = MenuManager.ShowMainMenuAndSelect();
            GameSettings settings = new GameSettings(selectedOption);

            ProfileDataManager dataManager = new ProfileDataManager();
            Validation validator = new Validation();
            List<Player> playersList = new List<Player>();

            FinanceManager financeSystem = new FinanceManager();
            PoolManager poolManager = new PoolManager(financeSystem); 

            GameStart.RegisterAgents(validator, playersList, dataManager);

            TournamentManager.RunTournament(playersList, settings, poolManager);

            dataManager.UpdateAndSave(playersList);

            LeaderboardManager boardManager = new LeaderboardManager();
        }
    }
}
