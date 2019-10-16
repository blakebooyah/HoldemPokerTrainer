﻿using System;
using System.Collections.Generic;

namespace PokerConsoleApp
{
        public enum HandType
        {
            // enume for the name of the hand
            NotAssignedYet = 9,
            StraightFlush = 8,
            FourOfAKind = 7,
            FullHouse = 6,
            Flush = 5,
            Straight = 4,
            ThreeOfAKind = 3,
            TwoPair = 2,
            OnePair = 1,
            HighCard = 0,
        };

        public enum Suit { Heart = 1, Diamond = 2, Spade = 3, Club = 4 };
        public enum Rank
        {
            TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8,
            NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13, ACE = 14
        };

        public class Hand
        {
            private readonly Card[] cards = new Card[5];
            private HandType hand_type = new HandType();
            private bool is_sorted = false;
            private int prime_rank = -1;

            public Hand()
            {
                this.cards.Capacity = 5;
                hand_type = HandType.NotAssignedYet;
                for (int i = 0; i < 4; i++)
                    suit_tally[i] = 0;
                for (int i = 0; i < 14; i++)
                    rank_tally[i] = 0;
            }

            public Card this[int index]
            {
                // The get accessor.
                get
                {
                    if (index >= 5 || index < 0)
                        throw new Exception("index of card indexer out of range");
                    return this.cards[index];
                }

                // The set accessor.
                set
                {
                    if (index >= 5 || index < 0)
                        throw new Exception("index of card indexer out of range");
                    this.cards[index] = value;
                }
            }

            public override string ToString()
            {
                string ret_string = "";
                for (int i = 0; i < 5; i++)
                {
                    ret_string += " ";
                    ret_string = ret_string + this.cards[i].ToString();
                    ret_string += " ";
                }
                return ret_string;
            }

        }

        public class Board
        {

            public Card[] flop = new Card[3];
            public Card turn = new Card();
            public Card river = new Card();
            public Player[] players;
            public Card[] deck;

            public Board(int player_count)
            {
                // Board constructor
                deck = new Card[52];
                players = new Player[player_count];

                for (int i = 0; i < player_count; i++)
                {
                    this.players[i] = new Player();
                }
            }
            public override string ToString()
            {
                string ret_string;
                var table = new ConsoleTable("flop", "turn", "river");
                table.AddRow($"{flop[0].ToString()} {flop[1].ToString()} {flop[2].ToString()}", turn.ToString(), river.ToString());

                ret_string = Utility_Methods.Trim_To_End(table.ToString(), "Count:");

                return ret_string;
            }

        }

        public class Player
        {
            public Card[] hole = new Card[2];

            public bool Won_The_Hand { get; set; }
            public Hand best_hand = new Hand();
            public Player()
            {
                this.Won_The_Hand = false;
                this.hole[0] = new Card();
                this.hole[1] = new Card();
            }
            public int GetWinflag()
            {
                if (this.Won_The_Hand == true)
                    return 1;
                else
                    return 0;
            }
            public override string ToString()
            {
                string ret_string;
                ret_string = "" + hole[0] + " " + hole[1];
                return ret_string;
            }
        }
    
}