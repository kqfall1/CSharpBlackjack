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

        static string FormatUserInputPrompt()
        {
            string handTypeStr = "";
            string inputPromptStr; 

            switch (PlayerController.GetActivePlayerHand().HandType)
            {
                case HandType.Dealt:
                    handTypeStr = "main";
                    break;
                case HandType.SplitOnce:
                    handTypeStr = "split";
                    break;
            }

            inputPromptStr = $"You are currently playing your {handTypeStr} hand.";

            if (PlayerController.GetActivePlayerHand().CanSurrender)
            {
                return PromptPlayer($"{inputPromptStr} {PLAYER_ACTION_PROMPT_INITIAL}");
            }

            return PromptPlayer($"{inputPromptStr} {PLAYER_ACTION_PROMPT_WITHOUT_SURRENDERING}");
        }

        static void Main(string[] args)
        {
            Console.WriteLine(GREETING_PROMPT);
            Console.WriteLine(PlayerController.CreateGame());

            do 
            {
                PrepareGame();

                if (PlayerController.GetPlayerMainHand().IsBlackjack || PlayerController.GetDealerMainHand().IsBlackjack)
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
                inputStr = FormatUserInputPrompt(); 

                Console.WriteLine(PlayerController.ExecutePlayerAction(inputStr));
                
                if (PlayerController.GetActivePlayerHand().HandType is HandType.Dealt && 
                   (PlayerController.GetActivePlayerHand().IsBlackjack ||
                   PlayerController.GetActivePlayerHand().IsBusted) ||
                   PlayerController.GetActivePlayerHand().Status is HandStatus.Standing ||
                   PlayerController.GetActivePlayerHand().Status is HandStatus.Surrendered)
                {
                    break; 
                }

                PlayerController.AlterActivePlayerHand();
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

                if (PlayerController.GetActivePlayerHand().IsBlackjack || PlayerController.GetDealerMainHand().IsBlackjack)
                {
                    Console.WriteLine(PlayerController.ShowdownBlackjack(playerHands[count]));
                }
                else if (PlayerController.GetActivePlayerHand().IsBusted || PlayerController.GetDealerMainHand().IsBusted)
                {
                    Console.WriteLine(PlayerController.ShowdownBusted(playerHands[count]));
                }
                else if (PlayerController.GetActivePlayerHand().Status is HandStatus.Surrendered)
                {
                    Console.WriteLine(PlayerController.ShowdownSurrendered(playerHands[count])); 
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