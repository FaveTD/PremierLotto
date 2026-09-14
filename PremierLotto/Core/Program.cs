using PremierLotto.Core;
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

            while (true)
            {
                GameLevel? selectedLevel = MenuManager.ShowMainMenuAndSelect();

                if (selectedLevel.HasValue)
                {
                    GameSettings settings = new GameSettings(selectedLevel.Value);
                    ProfileDataManager dataManager = new ProfileDataManager();
                    Validation validator = new Validation();
                    FinanceManager financeSystem = new FinanceManager();
                    InputHandler input = new InputHandler();
                    LeaderboardManager leaderboard = new LeaderboardManager();

                    PoolManager poolManager = new PoolManager(financeSystem, settings.Level.ToString());
                    JackpotRolloverManager rolloverManager = new JackpotRolloverManager();
                    List<Player> playersList = new List<Player>();

                    GameStart.RegisterAgents(validator, playersList, dataManager);

                    TournamentManager.RunTournament(
                        playersList,
                        settings,
                        poolManager,
                        financeSystem,
                        input,
                        validator,
                        leaderboard,
                        rolloverManager
                    );

                    dataManager.UpdateAndSave(playersList);

                    if (!MenuManager.PromptForNextAction()) break;
                }
                else
                {
                    break;
                }
            }
        }
    }
}