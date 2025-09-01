//Quinn Keenan, 301504914, 01/09/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal static class PromptManager
    {
        internal const string PLACE_BET_PROMPT = "Place a bet";
        internal const string PLAYER_ACTION_PROMPT_CAN_DOUBLE_DOWN_AND_SPLIT_AND_SURRENDER = "Would you like to (d)ouble down, (h)it, (sp)lit, (st)and, or (su)rrender? ";
        internal const string PLAYER_ACTION_PROMPT_CAN_DOUBLE_DOWN_AND_SPLIT_NO_SURRENDER = "Would you like to (d)ouble down, (h)it, (sp)lit, or (st)and? ";
        internal const string PLAYER_ACTION_PROMPT_CAN_DOUBLE_DOWN_NO_SPLIT_CAN_SURRENDER = "Would you like to (d)ouble down, (h)it, (st)and, or (su)rrender? ";
        internal const string PLAYER_ACTION_PROMPT_CAN_DOUBLE_DOWN_NO_SPLIT_NOR_SURRENDER = "Would you like to (d)ouble down, (h)it, or (st)and? ";
        internal const string PLAYER_ACTION_PROMPT_CANT_DOUBLE_DOWN_CAN_SPLIT_AND_SURRENDER = "Would you like to (h)it, (sp)lit, (st)and, or (su)rrender? ";
        internal const string PLAYER_ACTION_PROMPT_CANT_DOUBLE_DOWN_CAN_SPLIT_NO_SURRENDER = "Would you like to (h)it, (sp)lit, or (st)and? ";
        internal const string PLAYER_ACTION_PROMPT_CANT_DOUBLE_DOWN_NOR_SPLIT_CAN_SURRENDER = "Would you like to (h)it, (st)and, or (su)rrender? ";
        internal const string PLAYER_ACTION_PROMPT_CANT_DOUBLE_DOWN_NOR_SPLIT_NOR_SURRENDER = "Would you like to (h)it, (sp)lit, or (st)and? ";

        internal static PlayerInputAbbreviation FormatUserInputPrompt(PlayerHand activePlayerHand)
        {
            string inputPromptStr = $"You are currently playing your {activePlayerHand.HandTypeString} (score: {activePlayerHand.Score}).";

            switch (activePlayerHand.CanDoubleDown)
            {
                case true:
                    if (activePlayerHand.CanSplit && activePlayerHand.Player.CanSurrender)
                    {
                        return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CAN_DOUBLE_DOWN_AND_SPLIT_AND_SURRENDER}");
                    }
                    else if (activePlayerHand.CanSplit && !activePlayerHand.Player.CanSurrender)
                    {
                        return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CAN_DOUBLE_DOWN_AND_SPLIT_NO_SURRENDER}");
                    }
                    else if (!activePlayerHand.CanSplit && activePlayerHand.Player.CanSurrender)
                    {
                        return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CAN_DOUBLE_DOWN_NO_SPLIT_CAN_SURRENDER}");
                    }

                    return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CAN_DOUBLE_DOWN_NO_SPLIT_NOR_SURRENDER}");
                default:
                    if (activePlayerHand.CanSplit && activePlayerHand.Player.CanSurrender)
                    {
                        return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CANT_DOUBLE_DOWN_CAN_SPLIT_AND_SURRENDER}");
                    }
                    else if (activePlayerHand.CanSplit && !activePlayerHand.Player.CanSurrender)
                    {
                        return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CANT_DOUBLE_DOWN_CAN_SPLIT_NO_SURRENDER}");
                    }
                    else if (!activePlayerHand.CanSplit && activePlayerHand.Player.CanSurrender)
                    {
                        return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CANT_DOUBLE_DOWN_NOR_SPLIT_CAN_SURRENDER}");
                    }

                    return PromptPlayerForActionInput($"{inputPromptStr} {PLAYER_ACTION_PROMPT_CANT_DOUBLE_DOWN_NOR_SPLIT_NOR_SURRENDER}");
            }
        }

        internal static PlayerInputAbbreviation PromptPlayerForActionInput(string promptString)
        {
            string inputString;
            string validatedInput;

            do
            {
                Console.Write(promptString);
                inputString = Console.ReadLine(); //This will change in the future as I switch to using a GUI application to use the library.
                validatedInput = ValidateInput(inputString);

                try
                {
                    if (validatedInput is null)
                    {
                        throw new InvalidStringInputException(validatedInput);
                    }

                    if (validatedInput != PlayerInputAbbreviation.D.ToString() &&
                       validatedInput != PlayerInputAbbreviation.H.ToString() &&
                       validatedInput != PlayerInputAbbreviation.SP.ToString() &&
                       validatedInput != PlayerInputAbbreviation.ST.ToString() &&
                       validatedInput != PlayerInputAbbreviation.SU.ToString())
                    {
                        throw new InvalidStringInputException(validatedInput);
                    }
                }
                catch (InvalidStringInputException iiException)
                {
                    Console.WriteLine(iiException.Message);
                }
            } while (validatedInput is null ||
                    (validatedInput != PlayerInputAbbreviation.D.ToString() &&
                    validatedInput != PlayerInputAbbreviation.H.ToString() &&
                    validatedInput != PlayerInputAbbreviation.SP.ToString() &&
                    validatedInput != PlayerInputAbbreviation.ST.ToString() &&
                    validatedInput != PlayerInputAbbreviation.SU.ToString()));

            switch (validatedInput)
            {
                case "D":
                    return PlayerInputAbbreviation.D;
                case "H":
                    return PlayerInputAbbreviation.H;
                case "SP":
                    return PlayerInputAbbreviation.SP;
                case "ST":
                    return PlayerInputAbbreviation.ST;
                default:
                    return PlayerInputAbbreviation.SU;
            }
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
        internal static PlayerInputAbbreviation PromptPlayerForYesOrNoInput(string promptString)
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

                    if (validatedInputChar != Convert.ToChar(PlayerInputAbbreviation.Y.ToString()) &&
                       validatedInputChar != Convert.ToChar(PlayerInputAbbreviation.N.ToString()))
                    {
                        throw new InvalidStringInputException(validatedInput);
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
            } while (validatedInput is null ||
                    (validatedInputChar != Convert.ToChar(PlayerInputAbbreviation.Y.ToString()) &&
                    validatedInputChar != Convert.ToChar(PlayerInputAbbreviation.N.ToString())));

            switch (validatedInputChar)
            {
                case 'Y':
                    return PlayerInputAbbreviation.Y;
                default:
                    return PlayerInputAbbreviation.N;
            }
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