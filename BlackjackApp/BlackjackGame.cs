//Quinn Keenan, 301504914, 23/08/2025

using BlackjackLib;
using System.Diagnostics;

namespace BlackjackApp
{
    static class BlackjackGame
    {
        const string INVALID_INPUT_NOTIFICATION = $"\nPlease enter a valid input.";
        const string GREETING_PROMPT = "Welcome to Blackjack!";
        const string PLACE_BET_PROMPT = "Place a bet";
        static string PLACE_INSURANCE_BET_PROMPT;
        const string PLAYER_ACTION_PROMPT_INITIAL = "Would you like to (d)ouble down, (h)it, (sp)lit, (st)and, or (su)rrender?";
        const string PLAYER_ACTION_PROMPT_WITHOUT_SURRENDERING = "Would you like to (d)ouble down, (h)it, (sp)lit, or (st)and?";

        static string BriefPlayerAboutChipAmounts()
        {
            return $"You have {PlayerController.GetPlayer().ChipAmount:C} in chips. The dealer has {PlayerController.GetDealer().ChipAmount:C} in chips.";
        }

        static void Main(string[] args)
        {
            Console.WriteLine(GREETING_PROMPT);
            Console.WriteLine(PlayerController.CreateGame());

            do 
            {
                PrepareGame();

                if (PlayerController.GetEntityHasBlackJack() || PlayerController.GetPlayerInsuranceBetWon())
                {
                    Showdown();
                    continue; 
                }

                PlayerInputLoop();
                Showdown(); 
            } while (PlayerController.GetGameIsActive());
        }

        static void PlayerInputLoop()
        {
            string inputStr;

            while (PlayerController.GetPlayerIsPlaying())
            {
                if (PlayerController.GetActivePlayerHandType() is HandType.Dealt)
                {
                    if (PlayerController.GetActivePlayerHand().CanSurrender)
                    {
                        inputStr = PromptPlayer($"You are currently playing your main hand. {PLAYER_ACTION_PROMPT_INITIAL}");
                    }
                    else
                    {
                        inputStr = PromptPlayer($"You are currently playing your main hand. {PLAYER_ACTION_PROMPT_WITHOUT_SURRENDERING}");
                    }
                }
                else
                {
                    if (PlayerController.GetActivePlayerHand().CanSurrender)
                    {
                        inputStr = PromptPlayer($"You are currently playing your split hand. {PLAYER_ACTION_PROMPT_INITIAL}");
                    }
                    else
                    {
                        inputStr = PromptPlayer($"You are currently playing your split hand. {PLAYER_ACTION_PROMPT_WITHOUT_SURRENDERING}");
                    }
                }

                Console.WriteLine(PlayerController.ExecutePlayerAction(inputStr));
                
                if (PlayerController.GetEntityHasBlackJack()) 
                {
                    if (PlayerController.GetActivePlayerHandType() is HandType.Dealt)
                    { //COMBINE THESE CONDITIONALS
                        break;
                    }

                    PlayerController.AlterActivePlayerHand();
                }
                
                if (PlayerController.GetEntityIsBusted())
                {
                    if (PlayerController.GetActivePlayerHandType() is HandType.Dealt)
                    {
                        break;
                    }

                    PlayerController.AlterActivePlayerHand();
                }

                if (PlayerController.GetActivePlayerHand().Status is HandStatus.Standing)
                {
                    if (PlayerController.GetActivePlayerHandType() is HandType.Dealt)
                    {
                        break;
                    }

                    PlayerController.AlterActivePlayerHand();
                }

                if (PlayerController.GetActivePlayerHand().Status is HandStatus.Surrendered)
                {
                    if (PlayerController.GetActivePlayerHandType() is HandType.Dealt)
                    {
                        break;
                    }

                    PlayerController.AlterActivePlayerHand();
                    continue; 
                }

                Console.WriteLine(); 
            }
        }

        static void PrepareGame()
        {
            string inputStr; 
            
            Console.WriteLine(BriefPlayerAboutChipAmounts());

            while (PlayerController.GetPlayerMainHandBet() is null)
            {
                inputStr = PromptPlayer(PLACE_BET_PROMPT);
                Console.WriteLine(PlayerController.PlaceMainBet(inputStr));
            }

            Console.WriteLine(PlayerController.Deal());

            if (PlayerController.GetInsuranceBetPossible())
            {
                PLACE_INSURANCE_BET_PROMPT = $"\nDo you wish to place an insurance bet of {PlayerController.GetPlayerMainHandBet().ChipAmount / 2:C} (Y/N)?";
                inputStr = PromptPlayerForYesOrNoAnswer(PLACE_INSURANCE_BET_PROMPT);

                switch (inputStr)
                {
                    case "Y":
                        Console.WriteLine(PlayerController.PlaceInsuranceBet());
                        Console.WriteLine(PlayerController.AlterGameConditionsAfterInsuranceBet());
                        break;
                    case "N":
                        Console.WriteLine("You have declined to place an insurance bet.");
                        break;
                }
            }
        }

        static string PromptPlayer(string promptStr)
        {
            string inputStr;
            string validatedInput; 

            do
            {
                Console.Write($"{promptStr}: ");
                inputStr = Console.ReadLine();
                validatedInput = ValidateInput(inputStr);

                if (validatedInput is null)
                {
                    Console.WriteLine(INVALID_INPUT_NOTIFICATION); 
                }
            } while (validatedInput is null);

            return validatedInput; 
        }
        static string PromptPlayerForYesOrNoAnswer(string promptStr)
        {
            string inputStr;
            string validatedInput;

            do
            {
                Console.Write($"{promptStr}: ");
                inputStr = Console.ReadLine();
                validatedInput = ValidateInput(inputStr);

                if (validatedInput is null || (validatedInput != "Y" && validatedInput != "N"))
                {
                    Console.WriteLine(INVALID_INPUT_NOTIFICATION);
                }
            } while (validatedInput is null || (validatedInput != "Y" && validatedInput != "N"));

            return validatedInput;
        }

        static void Showdown()
        {
            byte count; 
            PlayerHand[] playerHands = PlayerController.GetPlayerHands().ToArray();
            
            if (PlayerController.GetShouldDealerPlay())
            {
                Console.WriteLine(PlayerController.DealerPlay());
            }

            Array.Reverse(playerHands);

            for (count = 0; count < playerHands.Length; count++)
            {
                PlayerController.SetActivePlayerHand(playerHands[count]);

                if (PlayerController.GetEntityHasBlackJack())
                {
                    Console.WriteLine(PlayerController.ShowdownBlackjack(playerHands[count]));
                }
                else if (PlayerController.GetActivePlayerHand().Status is HandStatus.Surrendered)
                {
                    Console.WriteLine(PlayerController.ShowdownSurrendered(playerHands[count])); 
                }
                else if (PlayerController.GetEntityIsBusted())
                {
                    Console.WriteLine(PlayerController.ShowdownBusted(playerHands[count]));
                }
                else
                {
                    Console.WriteLine(PlayerController.ShowdownNormal(playerHands[count]));
                }
            }

            PlayerController.ResetConditions();
        }

        static string ValidateInput(string inputStr)
        {
            if (String.IsNullOrWhiteSpace(inputStr))
            {
                return null;
            }

            return inputStr.Trim().ToUpper();
        }
    }
}