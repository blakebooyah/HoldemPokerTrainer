# :slot_machine: HoldemPokerTrainer 
Train yourself for live Texas Holdem games by seeing the probability of winning change as more cards are dealt.

![what poker training mode looks like](screenshots/screenshot_poker%20training%20mode_3.JPG)

# How it works

:arrows_counterclockwise: This program simulates thousands of Texas Holdem Hands and saves them to a *sqlite* database.

Once the database of saved games is built up, the user can enter "Training Mode" where cards are dealt and the
probability of winning the hand is shown to them.

The probabilities are calculated by running **SQL queries** on the database of simulated games (i.e. monte carlo).

For example, if you have pocket jacks and the flop is 2, 4, queen unsuited, what are your chances
of winning a game with 4 other players? This program teaches you that.

♣️ ♦️ ♥️ ♠️
# Design rationale
This is a "rapid working prototype" and not a tutorial on optimal C# design. It is the first of of two small projects that I've done in that language. 
