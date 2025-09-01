//Quinn Keenan, 301504914, 29/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal static class InsuranceBetManager
    {
        internal static string AlterConditionsAfterInsuranceBet(Player player, Dealer dealer)
        {
            PlayerHand playerMainHand = player.MainHand as PlayerHand; 
            
            decimal dealerWinnings;
            decimal insuranceBetChipAmount = playerMainHand.InsuranceBet.ChipAmount;
            decimal insuranceBetWinnings;

            if (dealer.MainHand.IsBlackjack)
            {
                insuranceBetWinnings = playerMainHand.InsuranceBet.Payout(dealer, PayoutRatio.INSURANCE_BET);
                player.AddChips(insuranceBetWinnings);
                return MessageManager.DetermineInsuranceBetWinString(insuranceBetWinnings, insuranceBetChipAmount);
            }

            dealerWinnings = playerMainHand.InsuranceBet.Pot.Scoop();
            dealer.AddChips(dealerWinnings);
            playerMainHand.InsuranceBet = null;
            return MessageManager.DetermineInsuranceBetLossString(playerMainHand.InsuranceBet.ChipAmount); 
        }

        internal static bool InsuranceBetPossible(Dealer dealer, Player player)
        {
            DealerHand dealerHand = dealer.MainHand as DealerHand;
            PlayerHand playerMainHand = player.MainHand as PlayerHand;

            return playerMainHand.HandType is HandType.Main && 
                playerMainHand.UpCards.Count == 2 &&
                dealerHand.UpCards[0].Rank is Rank.Ace &&
                playerMainHand.Bet.InsuranceBetChipAmount <= player.ChipAmount &&
                playerMainHand.Bet.InsuranceBetChipAmount <= dealer.ChipAmount &&
                playerMainHand.InsuranceBet is null;
        }

        internal static bool InsuranceBetWon(Dealer dealer, Bet insuranceBet)
        {
            return dealer.MainHand.IsBlackjack && insuranceBet is not null;
        }

        internal static void PlaceInsuranceBet(Dealer dealer, Player player)
        {
            PlayerHand playerMainHand = player.MainHand as PlayerHand; 
            playerMainHand.InsuranceBet = player.CreateBet(dealer, playerMainHand.Bet.ChipAmount);
        }

        internal static void ResolveInsuranceBet(Dealer dealer, Player player)
        {
            PlayerHand playerMainHand = player.MainHand as PlayerHand; 

            if (dealer.MainHand.IsBlackjack)
            {
                player.AddChips(playerMainHand.InsuranceBet.Payout(dealer, PayoutRatio.INSURANCE_BET));
            }
            else
            {
                dealer.AddChips(playerMainHand.InsuranceBet.Pot.Scoop());
            }
        }
    }
}