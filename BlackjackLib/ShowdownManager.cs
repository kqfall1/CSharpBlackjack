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
        internal static string Blackjack(Dealer dealer, PlayerHand playerHand)
        {
            string showdownString = $"Your {playerHand.HandTypeString}:";

            if (dealer.MainHand.IsBlackjack && playerHand.IsBlackjack)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.PUSH));
                return $"{showdownString} {MessageManager.DetermineShowdownBlackjackPushString(playerHand.Bet.ChipAmount)}";
            }
            else if (dealer.MainHand.IsBlackjack)
            {
                dealer.AddChips(playerHand.Bet.Pot.Scoop());
                return $"{showdownString} {MessageManager.DetermineShowdownBlackjackDealerWinString(playerHand.Bet.ChipAmount)}";
            }
            else if (playerHand.IsBlackjack)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.BLACKJACK));
                return $"{showdownString} {MessageManager.DetermineShowdownBlackjackPlayerWinString(playerHand)}";
            }
            
            throw new InvalidOperationException(MessageManager.NO_BLACKJACK_FOUND_MESSAGE);
        }
        internal static string Busted(Dealer dealer, PlayerHand playerHand)
        {
            string showdownString = $"Your {playerHand.HandTypeString}:";

            if (playerHand.IsBusted)
            {
                dealer.AddChips(playerHand.Bet.Pot.Scoop());
                return $"{showdownString} {MessageManager.DetermineShowdownBustedDealerWinString(playerHand)}";
            }
            else if (dealer.MainHand.IsBusted)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.MAIN_BET));
                return $"{showdownString} {MessageManager.DetermineShowdownBustedPlayerWinString(dealer, playerHand)}";
            }

            throw new InvalidOperationException(MessageManager.NO_BUSTED_ENTITY_FOUND_MESSAGE);
        }
        internal static string Normal(Dealer dealer, PlayerHand playerHand)
        {
            string showdownString = MessageManager.DetermineShowdownNormalBriefString(dealer, playerHand);

            if (playerHand.Score > dealer.MainHand.Score)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.MAIN_BET));
                return $"{showdownString} {MessageManager.DetermineShowdownNormalPlayerWinString(playerHand)}";
            }
            else if (playerHand.Score < dealer.MainHand.Score)
            {
                dealer.AddChips(playerHand.Bet.Pot.Scoop());
                return $"{showdownString} {MessageManager.DetermineShowdownNormalDealerWinString(playerHand)}"; 
            }
            
            playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.PUSH));
            return $"{showdownString} {MessageManager.DetermineShowdownNormalPushString(playerHand)}";
        }
        internal static string Surrendered(Dealer dealer, PlayerHand playerHand)
        {
            playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.SURRENDER));
            return MessageManager.DetermineShowdownSurrenderString(playerHand);
        }
    }
}