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
    internal static class StringInputOutputManager
    {
        //CONSIDER DIVIDING THIS CLASS INTO SUBCLASSES... IT IS BLOATING.
        internal const char DOUBLE_DOWN_ABBREVIATION = 'D'; //CONSIDER USING AN ENUMERATION FOR THESE!!!
        internal const char HIT_ABBREVIATION = 'H';
        internal const char NO_ABBREVIATION = 'N'; 
        internal const string SPLIT_ABBREVIATION = "SP";
        internal const string STAND_ABBREVIATION = "ST";
        internal const string SURRENDER_ABBREVIATION = "SU";
        internal const char YES_ABBREVIATION = 'Y'; 

        internal const string GREETING_PROMPT = "Welcome to Blackjack!";
        internal const string PLACE_BET_PROMPT = "Place a bet";
        internal const string PLAYER_ACTION_PROMPT_CAN_SPLIT_AND_SURRENDER = "Would you like to (d)ouble down, (h)it, (sp)lit, (st)and, or (su)rrender?";
        internal const string PLAYER_ACTION_PROMPT_CAN_SPLIT_NO_SURRENDER = "Would you like to (d)ouble down, (h)it, (sp)lit, or (st)and?";
        internal const string PLAYER_ACTION_PROMPT_CAN_SURRENDER_NO_SPLIT = "Would you like to (d)ouble down, (h)it, (st)and, or (su)rrender?";
        internal const string PLAYER_ACTION_PROMPT_CANT_SPLIT_NOR_SURRENDER = "Would you like to (d)ouble down, (h)it, (sp)lit, or (st)and?";

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
            return $"You elect to double down. {DetermineEntityActionString(playerHand)}";
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
                return $"The dealer doesn't have enough chips to bet {chipAmount:C}."; 
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
        internal static string DetermineInsuranceBetResolutionString(Dealer dealer, PlayerHand playerMainHand)
        {
            if (InsuranceBetManager.InsuranceBetWon(dealer, playerMainHand.InsuranceBet))
            {
                return $"You win {playerMainHand.InsuranceBet.PayoutAmount(PayoutRatio.INSURANCE_BET):C} on your insurance bet of {playerMainHand.InsuranceBet.ChipAmount:C}";
            }

            return $"The dealer does not have a blackjack. You forfeit your bet of {playerMainHand.InsuranceBet.ChipAmount:C}. Play on!";
        }
        internal static string DetermineSplitString(PlayerHand playerMainHand, PlayerHand playerSplitHand)
        {
            return $"Your first hand (score: {playerMainHand.Score}) is now:\n\n\t{playerMainHand.ToString()}\n\nYour second hand (score: {playerSplitHand.Score}) is now:\n\n\t{playerSplitHand.ToString()}";
        }
        internal static string DetermineSurrenderString(PlayerHand playerHand)
        {
            return $"You elect to surrender on a score of {playerHand.Score}.";
        }

        internal static string FormatUserInputPrompt(PlayerHand activePlayerHand) 
        {
            string inputPromptStr = $"You are currently playing your {activePlayerHand.HandTypeString}.";

            if (activePlayerHand.CanSplit && activePlayerHand.CanSurrender)
            {
                return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CAN_SPLIT_AND_SURRENDER}");
            }
            else if (activePlayerHand.CanSplit && !activePlayerHand.CanSurrender)
            {
                return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CAN_SPLIT_NO_SURRENDER}");
            }
            else if (!activePlayerHand.CanSplit && activePlayerHand.CanSurrender)
            {
                return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CAN_SURRENDER_NO_SPLIT}");
            }

            return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CANT_SPLIT_NOR_SURRENDER}");
        }

        internal static string PromptPlayerForActionInput(string promptString)
        {
            string inputString;
            string validatedInput;

            do
            {
                Console.Write($"{promptString}: ");
                inputString = Console.ReadLine(); //This will change in the future as I switch to using a GUI application to use the library.
                validatedInput = ValidateInput(inputString);

                try
                {
                    if (validatedInput is null)
                    {
                        throw new InvalidStringInputException(validatedInput);
                    }
 
                    if (validatedInput != SURRENDER_ABBREVIATION &&
                       validatedInput != SPLIT_ABBREVIATION && 
                       validatedInput != STAND_ABBREVIATION && 
                       validatedInput != DOUBLE_DOWN_ABBREVIATION.ToString() &&
                       validatedInput != HIT_ABBREVIATION.ToString())
                    {
                        throw new InvalidStringInputException(validatedInput);
                    }
                }
                catch (InvalidStringInputException iiException)
                {
                    Console.WriteLine(iiException.Message);
                }
            } while (validatedInput is null || 
                    (validatedInput != SURRENDER_ABBREVIATION &&
                    validatedInput != SPLIT_ABBREVIATION &&
                    validatedInput != STAND_ABBREVIATION &&
                    validatedInput != DOUBLE_DOWN_ABBREVIATION.ToString() &&
                    validatedInput != HIT_ABBREVIATION.ToString()));

            return validatedInput;
        }
        internal static decimal PromptPlayerForBetInput(Player player)
        {
            decimal chipAmount = 0; 
            string inputString; 
            string validatedInput; 

            do
            {
                Console.Write($"{PLACE_BET_PROMPT}: ");
                inputString = Console.ReadLine(); 
                validatedInput = ValidateInput(inputString);

                try
                {
                    if (validatedInput is null)
                    {
                        throw new InvalidStringInputException(validatedInput);
                    }

                    chipAmount = Convert.ToDecimal(validatedInput);

                    if (chipAmount <= 0)
                    {
                        throw new InvalidStringInputException(chipAmount.ToString("C"));
                    }
                    else if (chipAmount > player.ChipAmount)
                    {
                        throw new InsufficientChipsException(player, chipAmount);
                    }
                }
                catch (ArgumentNullException anException)
                {
                    Console.WriteLine(anException.Message); 
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (OverflowException overflowException)
                {
                    Console.WriteLine(overflowException.Message);
                }
                catch (InvalidStringInputException iiException)
                {
                    Console.WriteLine(iiException.Message);
                }
                catch (InsufficientChipsException icException)
                {
                    Console.WriteLine(icException.Message);
                }
            } while (validatedInput is null || chipAmount <= 0 || chipAmount > player.ChipAmount);

            return chipAmount; 
        }
        internal static char PromptPlayerForYesOrNoInput(string promptString)
        {
            string inputString;
            string validatedInput;
            char validatedInputChar = ' ';

            do
            {
                Console.Write($"{promptString}: ");
                inputString = Console.ReadLine();
                validatedInput = ValidateInput(inputString);

                try
                {
                    validatedInputChar = Convert.ToChar(validatedInput);

                    if (validatedInputChar != YES_ABBREVIATION && validatedInputChar != NO_ABBREVIATION)
                    {
                        throw new InvalidStringInputException(validatedInput); 
                    }
                }
                catch (InvalidStringInputException iiException)
                {
                    Console.WriteLine(iiException.Message);
                }
                catch (Exception e) //UPDATE THIS LATER
                {
                    Console.WriteLine(e.Message);
                }
            } while (validatedInput is null || (validatedInputChar != YES_ABBREVIATION && validatedInputChar != NO_ABBREVIATION));

            return validatedInputChar;
        }

        private static string ValidateInput(string inputStr)
        {
            if (System.String.IsNullOrWhiteSpace(inputStr))
            {
                return null;
            }

            return inputStr.Trim().ToUpper();
        }
    }
}