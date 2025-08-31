//Quinn Keenan, 301504914, 29/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal static class ShowdownManager
    {
        //REPLACE ALL THESE STRING LITERALS WITH CALLS TO MEMBERS OF STRINGINPUTOUTPUTMANAGER
        internal static string Blackjack(Dealer dealer, PlayerHand playerHand)
        {
            string showdownString = $"Your {playerHand.HandTypeString}:";

            if (dealer.MainHand.IsBlackjack && playerHand.IsBlackjack)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.PUSH)); 
                return $"{showdownString} both you and the dealer were dealt a blackjack. Your bet of {playerHand.Bet.ChipAmount:C} is pushed.";
            }
            else if (dealer.MainHand.IsBlackjack)
            {
                dealer.AddChips(playerHand.Bet.Pot.Scoop());
                return $"{showdownString} the dealer was dealt a blackjack. You have been defeated and forfeit your bet of {playerHand.Bet.ChipAmount:C}.";
            }
            else if (playerHand.IsBlackjack)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.BLACKJACK)); 
                return $"{showdownString} you were dealt a blackjack. You are victorious and receive {playerHand.Bet.PayoutAmount(PayoutRatio.BLACKJACK):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
            }
            else
            {
                throw new InvalidOperationException("Game.ShowdownBlackjack() cannot determine which blackjack entity has a blackjack.");
            }
        }
        internal static string Busted(Dealer dealer, PlayerHand playerHand)
        {
            string showdownString = $"Your {playerHand.HandTypeString}:";

            if (playerHand.IsBusted)
            {
                dealer.AddChips(playerHand.Bet.Pot.Scoop());
                return $"{showdownString} you bust with a score of {playerHand.Score}. You are defeated and forfeit your bet of {playerHand.Bet.ChipAmount:C}.";
            }
            else if (dealer.MainHand.IsBusted)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.MAIN_BET));
                return $"{showdownString} the dealer busts with a score of {dealer.MainHand.Score}. You are victorious and receive {playerHand.Bet.PayoutAmount(PayoutRatio.MAIN_BET):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
            }
            else
            {
                throw new InvalidOperationException("Game.ShowdownBusted cannot determine which blackjack entity has busted.");
            }
        }
        internal static string Normal(Dealer dealer, PlayerHand playerHand)
        {
            string showdownString = $"Your {playerHand.HandTypeString}: your score is {playerHand.Score} and the dealer's score is {dealer.MainHand.Score}.";

            if (playerHand.Score > dealer.MainHand.Score)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.MAIN_BET));
                return $"{showdownString} You are victorious and receive {playerHand.Bet.PayoutAmount(PayoutRatio.MAIN_BET):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
            }
            else if (playerHand.Score < dealer.MainHand.Score)
            {
                dealer.AddChips(playerHand.Bet.Pot.Scoop());
                return $"{showdownString} You have been defeated and forfeit your bet of {playerHand.Bet.ChipAmount:C}.";
            }
            else
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.PUSH));
                return $"{showdownString} You have tied the dealer's score and have your bet of {playerHand.Bet.PayoutAmount(PayoutRatio.PUSH):C} pushed.";
            }
        }
        internal static string Surrendered(Dealer dealer, PlayerHand playerHand)
        {
            playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.SURRENDER)); 
            return $"Your {playerHand.HandTypeString}: you receive {playerHand.Bet.PayoutAmount(PayoutRatio.SURRENDER):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
        }
    }
}