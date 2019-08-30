﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
namespace PokerConsoleApp
{
    class Simulation
    {
        // variables that need to be accessible by producer and consumer methods
        private static BlockingCollection<GameRecord> collection;
        private static SQLiteConnection conn;
        private static SQLiteCommand command;
        private static SQLiteTransaction transaction;
        private static int gamesTotal;
        private static int gamesWritten;
        public static int Simulate_Games(int games_to_simulate)
        {
            gamesTotal = games_to_simulate;
            gamesWritten = 0;
            // Database writing setup code
            conn = SQLite_Methods.CreateConnection(Program.NUMBER_OF_PLAYERS);
            SQLite_Methods.CreateTableIfNotExists(conn);
            SQLite_Methods.DropIndexIfExists(conn);
            command = conn.CreateCommand();
            transaction = conn.BeginTransaction();
            int gamesPerTransaction = 500;
        
            do
            {

                // Execute Insert command on database, one row per player
                GameRecord record;
                while (true)
                {
                    if (collection.TryTake(out record, 1))
                        break;
                }
                SQLite_Methods.InsertResultItem(record, command);

                gamesWritten++;
                // end of deck loop
                if (gamesWritten % gamesPerTransaction == 0)
                {
                    transaction.Commit();
                    transaction = conn.BeginTransaction();
                }
            } while (gamesWritten < games_to_simulate); // end of main loop

            // inevitably we broke out of loop with a partial transaction. flush it to disk.
            transaction.Commit();
            // clean up
            command.Dispose();
            transaction.Dispose();
            conn.Dispose();
            return 0;
        }



        // Record producer simulates a game and adds the result to a BlockingCollection
        public static void RecordProducer()
        {
            do
            {
                Board b = new Board(Program.NUMBER_OF_PLAYERS);
                for (int deal_count = 0; deal_count <= 2; deal_count++) // two deals per deck
                {
                    b.Deal_Cards(Program.NUMBER_OF_PLAYERS);
                    if ((deal_count + 1) % 2 == 0)
                        b.Get_New_Deck();
                    List<Hand> lst_best_hands = new List<Hand> { };
                    for (int player_index = 0; player_index < Program.NUMBER_OF_PLAYERS; player_index++)
                    {
                        Card hole1 = b.players[player_index].hole[0];
                        Card hole2 = b.players[player_index].hole[1];
                        Card flop1 = b.flop_cards[0];
                        Card flop2 = b.flop_cards[1];
                        Card flop3 = b.flop_cards[2];
                        Card turn = b.turn_card;
                        Card river = b.river_card;
                        // Find individual players' best hand out of all possible
                        // combos of hole, flop, turn, and river cards
                        List<Hand> lst_hand = Hand.Build_List_21_Hands(hole1, hole2, flop1, flop2, flop3, turn, river);
                        List<int> winning_hand_indices = Hand.FindBestHand(lst_hand);
                        lst_best_hands.Add(lst_hand[winning_hand_indices[0]]);
                    }
                    List<int> winning_player_indices = Hand.FindBestHand(lst_best_hands);
                    // Set WON_THE_HAND boolean inside player class
                    foreach (var wi in winning_player_indices)
                        b.players[wi].Won_The_Hand = true;

                    /**************************************************************
                    * GAME HAS BEEN SIMULATED, NOW WRITE IT TO DATABASE
                    ***************************************************************/
                    for (int player_index = 0; player_index < Program.NUMBER_OF_PLAYERS; player_index++)
                    {
                        // Sort hole and flop cards uniquely 
                        List<Card> lst_hole_cards = new List<Card> { };
                        List<Card> lst_flop_cards = new List<Card> { };
                        for (int i = 0; i < 2; i++)
                            lst_hole_cards.Add(b.players[player_index].hole[i]);
                        for (int i = 0; i < 3; i++)
                            lst_flop_cards.Add(b.flop_cards[i]);
                        Card.Reorder_Cards_Uniquely(ref lst_hole_cards);
                        Card.Reorder_Cards_Uniquely(ref lst_flop_cards);
                        var record = new GameRecord(lst_hole_cards[0], lst_hole_cards[1], lst_flop_cards[0], lst_flop_cards[1], lst_flop_cards[2], b.players[player_index].GetWinflag());
                        while (true)
                        {
                            if (collection.TryAdd(record, 1) == true)
                                break;
                        }
                    }
                }
            } while (gamesWritten < gamesTotal);
        }

        // Record consumer writes records to the sqlite database
        public static void RecordConsumer()
        {

        }


        public struct GameRecord
        {
            public Card hole1, hole2, flop1, flop2, flop3;
            public int winFlag;
            public GameRecord(Card hole1, Card hole2, Card flop1, Card flop2, Card flop3, int winFlag)
            {
                this.hole1 = hole1;
                this.hole2 = hole2;
                this.flop1 = flop1;
                this.flop2 = flop2;
                this.flop3 = flop3;
                this.winFlag = winFlag;
            }
        }
    }
}
