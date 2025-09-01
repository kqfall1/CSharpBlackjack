//Quinn Keenan, 301504914, 30/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlackjackLib
{
    internal static class MessageManager
    {
        internal const string GREETING_PROMPT = "Welcome to blackjack!";
        internal const string NO_BLACKJACK_FOUND_MESSAGE = "Game.ShowdownBlackjack() cannot determine which blackjack entity has a blackjack.";
        internal const string NO_BUSTED_ENTITY_FOUND_MESSAGE = "Game.ShowdownBusted() cannot determine which blackjack entity has busted."; 
        
        internal static string DetermineChipAmountBriefString(Dealer dealer, Player player)
        {
            return $"You have {player.ChipAmount:C} in chips. The dealer has {dealer.ChipAmount:C} in chips.";
        }
        internal static string DetermineDealBriefString(DealerHand dealerHand, PlayerHand playerHand)
        {
            return $"Your hand:\n\n\t{playerHand.ToString()}\n\n\tDealer's up card: {dealerHand.UpCards[0]}.\n";
        }
        internal static string DetermineDealerHandStringBrief(DealerHand dealerHand)
        {
            return $"\nDealer's hand:\n\n\t{dealerHand.ToString()}\n";
        }
        internal static string DetermineDoubleDownString(PlayerHand playerHand)
        {
            return $"You elect to double down and your bet is now {playerHand.Bet.ChipAmount:C}. {DetermineEntityActionString(playerHand)}";
        }
        internal static string DetermineEntityActionString(Hand hand)
        {
            if (hand is DealerHand)
            {
                return $"Dealer draws the {hand.MostRecentlyDealtCard}.";
            }
            else
            {
                return $"You draw the {hand.MostRecentlyDealtCard}. Score: {hand.Score}.";
            }
        }
        internal static string DetermineEntityBustsString(Hand hand)
        {
            if (hand is DealerHand)
            {
                return $"The dealer busts on {hand.Score}.";
            }
            else
            {
                return $"You bust on {hand.Score}.";
            }
        }
        internal static string DetermineEntityStandsString(Hand hand)
        {
            if (hand is DealerHand)
            {
                return $"The dealer elects to stand on {hand.Score}.";
            }
            else
            {
                return $"You elect to stand on {hand.Score}.";
            }
        }
        internal static string DetermineInsufficientChipsExceptionMessage(BlackjackEntity blackjackEntity, decimal chipAmount)
        {
            if (blackjackEntity is Dealer)
            {
                return $"The dealer doesn't have enough chips to payout a bet of {chipAmount:C}."; 
            }
            else
            {
                return $"You don't have enough chips to bet {chipAmount:C}.";
            }
        }
        internal static string DetermineInsuranceBetPromptString(decimal chipAmount)
        {
            return $"Do you wish to place an insurance bet of {chipAmount:C} (Y/N)?";
        }
        internal static string DetermineInsuranceBetLossString(decimal chipAmount)
        {
            return $"The dealer does not have a blackjack! You forfeit your bet of {chipAmount:C}. Play on!";
        }
        internal static string DetermineInsuranceBetResolutionString(Dealer dealer, PlayerHand playerMainHand)
        {
            if (InsuranceBetManager.InsuranceBetWon(dealer, playerMainHand.InsuranceBet))
            {
                return $"You win {playerMainHand.InsuranceBet.PayoutAmount(PayoutRatio.INSURANCE_BET):C} on your insurance bet of {playerMainHand.InsuranceBet.ChipAmount:C}";
            }

            return $"The dealer does not have a blackjack. You forfeit your bet of {playerMainHand.InsuranceBet.ChipAmount:C}. Play on!";
        }
        internal static string DetermineInsuranceBetWinString(decimal insuranceBetWinnings, decimal insuranceBetChipAmount)
        {
            return $"You win {insuranceBetWinnings:C} on your insurance bet of {insuranceBetChipAmount:C}";
        }
        internal static string DetermineShowdownBlackjackDealerWinString(decimal chipAmount)
        {
            return $"the dealer was dealt a blackjack. You have been defeated and forfeit your bet of {chipAmount:C}.";
        }
        internal static string DetermineShowdownBlackjackPlayerWinString(PlayerHand playerHand)
        {
            return $"you were dealt a blackjack. You are victorious and receive {playerHand.Bet.PayoutAmount(PayoutRatio.BLACKJACK):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
        }
        internal static string DetermineShowdownBlackjackPushString(decimal chipAmount)
        {
            return $"both you and the dealer were dealt a blackjack. Your bet of {chipAmount:C} is pushed.";
        }
        internal static string DetermineShowdownBustedDealerWinString(PlayerHand playerHand)
        {
            return $"you bust with a score of {playerHand.Score}. You are defeated and forfeit your bet of {playerHand.Bet.ChipAmount:C}.";
        }
        internal static string DetermineShowdownBustedPlayerWinString(Dealer dealer, PlayerHand playerHand)
        {
            return $"the dealer busts with a score of {dealer.MainHand.Score}. You are victorious and receive {playerHand.Bet.PayoutAmount(PayoutRatio.MAIN_BET):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
        }
        internal static string DetermineShowdownNormalBriefString(Dealer dealer, PlayerHand playerHand)
        {
            return $"Your {playerHand.HandTypeString}: your score is {playerHand.Score} and the dealer's score is {dealer.MainHand.Score}.";
        }
        internal static string DetermineShowdownNormalDealerWinString(PlayerHand playerHand)
        {
            return $"You have been defeated and forfeit your bet of {playerHand.Bet.ChipAmount:C}.";
        }
        internal static string DetermineShowdownNormalPlayerWinString(PlayerHand playerHand)
        {
            return $"You are victorious and receive {playerHand.Bet.PayoutAmount(PayoutRatio.MAIN_BET):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
        }
        internal static string DetermineShowdownNormalPushString(PlayerHand playerHand)
        {
            return $"You have tied the dealer's score and have your bet of {playerHand.Bet.PayoutAmount(PayoutRatio.PUSH):C} pushed.";
        }
        internal static string DetermineShowdownSurrenderString(PlayerHand playerHand)
        {
            return $"Your {playerHand.HandTypeString}: you receive {playerHand.Bet.PayoutAmount(PayoutRatio.SURRENDER):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
        }
        internal static string DetermineSplitString(PlayerHand playerMainHand, PlayerHand playerSplitHand)
        {
            return $"Your first hand is now:\n\n\t{playerMainHand.ToString()}\n\nYour second hand is now:\n\n\t{playerSplitHand.ToString()}\n";
        }
        internal static string DetermineSurrenderString(PlayerHand playerHand)
        {
            return $"You elect to surrender on a score of {playerHand.Score}.";
        }
    }
}