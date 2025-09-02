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
                return $"{showdownString} {MessageManager.ShowdownBlackjackPushString(playerHand.Bet.ChipAmount)}";
            }
            else if (dealer.MainHand.IsBlackjack)
            {
                dealer.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.FORFEIT));
                return $"{showdownString} {MessageManager.ShowdownBlackjackDealerWinString(playerHand.Bet.ChipAmount)}";
            }
            else if (playerHand.IsBlackjack)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.BLACKJACK));
                return $"{showdownString} {MessageManager.ShowdownBlackjackPlayerWinString(playerHand)}";
            }
            
            throw new InvalidOperationException(MessageManager.NO_BLACKJACK_FOUND_MESSAGE);
        }
        internal static string Busted(Dealer dealer, PlayerHand playerHand)
        {
            string showdownString = $"Your {playerHand.HandTypeString}:";

            if (playerHand.IsBusted)
            {
                dealer.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.FORFEIT));
                return $"{showdownString} {MessageManager.ShowdownBustedDealerWinString(playerHand)}";
            }
            else if (dealer.MainHand.IsBusted)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.MAIN_BET));
                return $"{showdownString} {MessageManager.ShowdownBustedPlayerWinString(dealer, playerHand)}";
            }

            throw new InvalidOperationException(MessageManager.NO_BUSTED_ENTITY_FOUND_MESSAGE);
        }
        internal static string Normal(Dealer dealer, PlayerHand playerHand)
        {
            string showdownString = MessageManager.ShowdownNormalBriefString(dealer, playerHand);

            if (playerHand.Score > dealer.MainHand.Score)
            {
                playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.MAIN_BET));
                return $"{showdownString} {MessageManager.ShowdownNormalPlayerWinString(playerHand)}";
            }
            else if (playerHand.Score < dealer.MainHand.Score)
            {
                dealer.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.FORFEIT));
                return $"{showdownString} {MessageManager.ShowdownNormalDealerWinString(playerHand)}"; 
            }
            
            playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.PUSH));
            return $"{showdownString} {MessageManager.ShowdownNormalPushString(playerHand)}";
        }
        internal static string Surrendered(Dealer dealer, PlayerHand playerHand)
        {
            playerHand.Player.AddChips(playerHand.Bet.Payout(dealer, PayoutRatio.SURRENDER));
            return MessageManager.ShowdownSurrenderString(playerHand);
        }
    }
}