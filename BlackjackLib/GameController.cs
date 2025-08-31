//Quinn Keenan, 301504914, 30/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public static class GameController
    {
        private static Game game;

        private static void BettingRoundLoop()
        {
            do
            {
                PrepareForBettingRound(); 

                if (!(game.EntitiesController.Dealer.MainHand.IsBlackjack || game.EntitiesController.ActivePlayerHand.IsBlackjack))
                {
                    PlayerMainActionStringInputLoop();
                }

                if (game.EntitiesController.Dealer.ShouldDealerPlay(game.EntitiesController.Player, game.EntitiesController.ActivePlayerHand))
                {
                    DealerPlay();
                }

                PerformShowdowns(); 
            }
            while (game.IsActive);
        }

        public static void CreateGameAndPlay()
        {
            if (game is null)
            {
                game = new Game();
            }

            BettingRoundLoop(); 
        }

        static void DealerPlay()
        {
            Console.WriteLine(StringInputOutputManager.DetermineDealerHandStringBrief(game.EntitiesController.Dealer.MainHand as DealerHand));
            
            while (game.EntitiesController.Dealer.MustHit)
            {
                Console.WriteLine(game.EntitiesController.ExecuteDealerAction()); 
            }
        }

        static void PerformShowdowns()
        {
            PlayerHand[] playerHands = game.EntitiesController.Player.HandsInShowdownOrder;

            foreach (PlayerHand playerHand in playerHands)
            {
                game.EntitiesController.ActivePlayerHand = playerHand;

                if (game.EntitiesController.Dealer.MainHand.IsBlackjack || game.EntitiesController.ActivePlayerHand.IsBlackjack)
                {
                    Console.WriteLine(ShowdownManager.Blackjack(game.EntitiesController.Dealer, game.EntitiesController.ActivePlayerHand));
                }
                else if (game.EntitiesController.Dealer.MainHand.IsBusted || game.EntitiesController.ActivePlayerHand.IsBusted)
                {
                    Console.WriteLine(ShowdownManager.Busted(game.EntitiesController.Dealer, game.EntitiesController.ActivePlayerHand));
                }
                else if (game.EntitiesController.ActivePlayerHand.Status is HandStatus.Surrendered)
                {
                    Console.WriteLine(ShowdownManager.Surrendered(game.EntitiesController.Dealer, game.EntitiesController.ActivePlayerHand));
                }
                else
                {
                    Console.WriteLine(ShowdownManager.Normal(game.EntitiesController.Dealer, game.EntitiesController.ActivePlayerHand));
                }
            }

            game.ResetAfterBettingRound();
        }

        static void PlayerMainActionStringInputLoop()
        {
            string inputStr;

            while (game.EntitiesController.Player.IsPlaying)
            {
                inputStr = StringInputOutputManager.FormatUserInputPrompt(game.EntitiesController.ActivePlayerHand);
                Console.WriteLine(game.EntitiesController.ExecutePlayerAction(inputStr));

                if (game.EntitiesController.ActivePlayerHand.HandType is HandType.Main &&
                   (game.EntitiesController.ActivePlayerHand.IsBlackjack ||
                   game.EntitiesController.ActivePlayerHand.IsBusted) ||
                   game.EntitiesController.ActivePlayerHand.Status is HandStatus.Standing ||
                   game.EntitiesController.ActivePlayerHand.Status is HandStatus.Surrendered)
                {
                    break;
                }

                game.EntitiesController.AlterActivePlayerHandAfterDrawingOnASplitHand(game.EntitiesController.Player.MainHand as PlayerHand); 
            }
        }

        static void PrepareForBettingRound()
        {
            decimal chipAmount; 
            PlayerHand playerMainHand = game.EntitiesController.ActivePlayerHand;
            char inputCharacter = ' '; 

            Console.WriteLine(StringInputOutputManager.DetermineChipAmountBriefString(game.EntitiesController.Dealer, game.EntitiesController.Player));

            while (playerMainHand.Bet is null)
            {
                chipAmount = StringInputOutputManager.PromptPlayerForBetInput(game.EntitiesController.Player);
                game.EntitiesController.ActivePlayerHand.PlaceMainBet(chipAmount, game.EntitiesController.Dealer);
            }

            game.EntitiesController.Dealer.Deal(game.EntitiesController.ActivePlayerHand);
            Console.WriteLine(StringInputOutputManager.DetermineDealBriefString(game.EntitiesController.Dealer.MainHand as DealerHand, game.EntitiesController.ActivePlayerHand)); 

            if (InsuranceBetManager.InsuranceBetPossible(game.EntitiesController.Dealer, game.EntitiesController.Player))
            {
                while (inputCharacter == ' ')
                {
                    inputCharacter = StringInputOutputManager.PromptPlayerForYesOrNoInput(StringInputOutputManager.DetermineInsuranceBetPromptString(game.EntitiesController.ActivePlayerHand.Bet.InsuranceBetChipAmount));

                    switch (inputCharacter)
                    {
                        case StringInputOutputManager.YES_ABBREVIATION:
                            InsuranceBetManager.PlaceInsuranceBet(game.EntitiesController.Dealer, game.EntitiesController.Player);
                            InsuranceBetManager.ResolveInsuranceBet(game.EntitiesController.Dealer, game.EntitiesController.Player);
                            Console.WriteLine(StringInputOutputManager.DetermineInsuranceBetResolutionString(game.EntitiesController.Dealer, game.EntitiesController.ActivePlayerHand)); 
                            break;
                    }
                }
            }
        }
    }
}