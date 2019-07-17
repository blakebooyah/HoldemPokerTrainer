﻿using System;
/*
 * Purpose: this program uses monte carlo simulation to calculate
 *          a) the odds of winning a game texas holdem for a given set of hole cards.
 *                   
 * Input:                number of players
 * Output:               odds of winning and tieing for each set of hole cards
 */
namespace PokerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var d = new Deck();
            var h = new Hand();
            d.buildDeck();
            d.shuffleDeck();
            d.printDeck();
        }
    }
}
