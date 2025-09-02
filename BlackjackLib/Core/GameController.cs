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
            Console.WriteLine(MessageManager.GREETING_PROMPT);

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

        private static void DealerPlay()
        {
            Console.WriteLine(MessageManager.DealerHandStringBrief(game.EntitiesController.Dealer.MainHand as DealerHand));
            
            while (game.EntitiesController.Dealer.MustHit)
            {
                Console.WriteLine(game.EntitiesController.ExecuteDealerAction()); 
            }
        }

        private static void PerformShowdowns()
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

        private static void PlayerMainActionStringInputLoop()
        {
            PlayerInputAbbreviation playerInputAbbreviation; 

            while (game.EntitiesController.Player.IsPlaying)
            {
                playerInputAbbreviation = PromptManager.FormatUserInputPrompt(game.EntitiesController.ActivePlayerHand);
                Console.WriteLine(game.EntitiesController.ExecutePlayerAction(playerInputAbbreviation));

                if (game.EntitiesController.ActivePlayerHand.HandType is HandType.Split &&
                    (game.EntitiesController.ActivePlayerHand.IsBlackjack ||
                    game.EntitiesController.ActivePlayerHand.IsBusted ||
                    game.EntitiesController.ActivePlayerHand.Status is HandStatus.Standing ||
                    game.EntitiesController.ActivePlayerHand.Status is HandStatus.Surrendered))
                {
                    game.EntitiesController.AlterActivePlayerHandAfterDrawing();
                }
            }
        }

        private static void PrepareForBettingRound()
        {
            decimal chipAmount; 
            PlayerHand playerMainHand = game.EntitiesController.ActivePlayerHand;
            PlayerInputAbbreviation playerInputAbbreviation; 

            Console.WriteLine(MessageManager.ChipAmountBriefString(game.EntitiesController.Dealer, game.EntitiesController.Player));

            while (playerMainHand.Bet is null)
            {
                chipAmount = PromptManager.PromptPlayerForBetInput(game.EntitiesController.Player);
                game.EntitiesController.PlaceMainBet(chipAmount);
            }

            game.EntitiesController.Deal();
            Console.WriteLine(MessageManager.DealBriefString(game.EntitiesController.Dealer.MainHand as DealerHand, game.EntitiesController.ActivePlayerHand)); 

            if (InsuranceManager.InsuranceBetPossible(game.EntitiesController.Dealer, game.EntitiesController.Player))
            {
                playerInputAbbreviation = PromptManager.PromptPlayerForYesOrNoInput(MessageManager.InsuranceBetPromptString(game.EntitiesController.ActivePlayerHand.Bet.InsuranceBetChipAmount));

                    switch (playerInputAbbreviation)
                    {
                        case PlayerInputAbbreviation.Y:
                            game.EntitiesController.PlaceInsuranceBet();
                            InsuranceManager.ResolveInsuranceBet(game.EntitiesController.Dealer, game.EntitiesController.Player);
                            Console.WriteLine(MessageManager.InsuranceBetResolutionString(game.EntitiesController.Dealer, game.EntitiesController.ActivePlayerHand)); 
                            break;
                    }
            }
        }
    }
}