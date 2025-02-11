﻿using System;
using System.IO;

namespace PokerConsoleApp
{
    public static class Program
    {
        public static int PlayerCount { get; private set; }

        static void Main()
        {
            Console.WriteLine($"Current Directory: \n\t\t{Directory.GetCurrentDirectory().ToString()}");
            Console.ReadLine();
            DisplayMenu();

        }


        static Program()
        {
            PlayerCount = 4;
        }

        public static void DisplayMenu()
        {
            bool _exitFlag = false;
            do
            {
                Console.Clear();
                PrintMenuText();
                string sInput = Console.ReadLine();

                if (Int32.TryParse(sInput, out int userChoice))
                {
                    switch (userChoice)
                    {
                        case 1:
                            int _numGames = UtilityMethods.GetIntegerFromUser(3000, 2000000000);
                            var watch = new System.Diagnostics.Stopwatch();
                            watch.Start();
                            Simulation.SimulateGames(PlayerCount, _numGames);
                            watch.Stop();
                            Console.WriteLine($"Total Execution Time: {(watch.ElapsedMilliseconds / 60000.0).ToString("0.##")} minutes");
                            UtilityMethods.GetKeyPress();
                            break;
                        case 2:
                            Game.PlayGame();
                            UtilityMethods.GetKeyPress();
                            break;
                        case 3:
                            Console.WriteLine("Enter number of players (2 to 8):");
                            // update value of Program.PlayerCount for use by other methods
                            PlayerCount = UtilityMethods.GetIntegerFromUser(2, 8);
                            UtilityMethods.GetKeyPress();
                            break;
                        case 4:
                            SqliteMethods.ShowDatabaseStatistics();
                            UtilityMethods.GetKeyPress();
                            break;
                        case 5:
                            _exitFlag = true;
                            break;
                        default:
                            break;
                    }
                }
            } while (_exitFlag == false);

        }

        private static void PrintMenuText()
        {
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("                              MAIN MENU                                  ");
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("1 - Simulate games to build up database");
            Console.WriteLine("2 - Enter poker training mode");
            Console.WriteLine($"3 - Change number of players (currently set to {PlayerCount})");
            Console.WriteLine("4 - View database statistics");
            Console.WriteLine("5 - Exit");
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("Please make a selection:");
        }

    }

}
