//Quinn Keenan, 301504914, 19/08/2025

namespace BlackjackLib
{
    public static class PlayerController
    {
        static Game game;

        public static void AlterActivePlayerHand()
        {
            game.AlterActivePlayerHand();
        }
        public static string AlterGameConditionsAfterInsuranceBet()
        {
            return game.AlterConditionsAfterInsuranceBet();
        }

        public static string CreateGame()
        {
            try
            {
                if (game is not null)
                {
                    throw new InvalidOperationException("Cannot create a new game when an instance of a game already exists.");
                }
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message;
            }

            game = new Game();
            return "Shuffle up and deal!";
        }

        public static string Deal()
        {
            string dealStr; 
            
            try
            {
                EnsureGameInstanceExists();
                dealStr = game.Deal();
                game.AlterHandStatusesAfterDealing(); 
                return dealStr;
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message;
            }
            catch (EmptyDeckException edException)
            {
                return edException.Message;
            }
        }

        public static string DealerPlay()
        {
            return game.DealerPlay(); 
        }

        private static void EnsureGameInstanceExists()
        {
            if (game is null)
            {
                throw new InvalidOperationException("Cannot deal cards when no instance of a game exists.");
            }
        }

        public static string ExecutePlayerAction(string inputStr)
        {
            string doubleDownStr; 
            
            try
            {
                EnsureGameInstanceExists();

                if (inputStr != "D" && inputStr != "H" && inputStr != "SP" && inputStr != "ST")
                {
                    throw new InvalidInputException(inputStr);
                }

                switch (inputStr)
                {
                    case "D":
                        doubleDownStr = PlaceDoubleDownBet();
                        return doubleDownStr; 
                    case "H":
                        return Hit(GetActivePlayerHand());
                    case "SP":
                        return Split(); 
                    case "ST":
                        game.Stand(GetActivePlayerHand()); 
                        return $"You elect to stand on {GetActivePlayerHand().Score}.";
                    default:
                        throw new InvalidOperationException($"Unexpected input string in \"PlayerInterface.PlayerAction\": \"{inputStr}.\"");
                }
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message;
            }
            catch (InvalidInputException iiException)
            {
                return iiException.Message;
            }
            catch (EmptyDeckException edException)
            {
                return edException.Message;
            }
            catch (InsufficientChipsException icException)
            {
                return icException.Message;
            }
        }
        
        public static bool GetEntityHasBlackJack()
        { 
            return game.EntityHasBlackJack; 
        }
        public static bool GetEntityIsBusted()
        {
            return game.EntityIsBusted;
        }
        public static PlayerHand GetActivePlayerHand()
        {
            return game.ActivePlayerHand;
        }
        public static HandType GetActivePlayerHandType()
        {
            return GetActivePlayerHand().HandType; 
        }
        public static Dealer GetDealer()
        {
            return game.Dealer;
        }
        public static bool GetDealerHasPlayed()
        {
            return game.DealerHasPlayed; 
        }
        public static DealerHand GetDealerMainHand()
        {
            return game.Dealer.MainHand as DealerHand;
        }
        public static bool GetDealerMainHandIsBlackjack()
        {
            return GetDealerMainHand().IsBlackjack; 
        }
        public static bool GetGameIsActive()
        {
            return game.IsActive;
        }
        public static bool GetInsuranceBetPossible()
        {
            return game.InsuranceBetPossible;
        }
        public static Player GetPlayer()
        {
            return game.Player;
        }
        public static List<PlayerHand> GetPlayerHands()
        {
            List<PlayerHand> playerHands = game.Player.Hands; 

            if (!playerHands[1].IsValid)
            {
                playerHands.RemoveAt(1);
            }

            return playerHands;
        }
        public static Bet GetPlayerInsuranceBet()
        {
            return GetPlayer().InsuranceBet; 
        }
        public static bool GetPlayerInsuranceBetWon()
        {
            return GetPlayer().InsuranceBetWon; 
        }
        public static bool GetPlayerIsPlaying()
        {
            return game.PlayerIsPlaying; 
        }
        public static PlayerHand GetPlayerMainHand()
        {
            return GetPlayer().MainHand as PlayerHand; 
        }
        public static Bet GetPlayerMainHandBet()
        {
            return GetPlayerMainHand().Bet;
        }
        public static bool GetShouldDealerPlay()
        {
            return game.ShouldDealerPlay; 
        }

        public static string Hit(Hand hand)
        {
            try
            {
                EnsureGameInstanceExists();
                return game.Hit(hand);
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message;
            }
            catch (EmptyDeckException edException)
            {
                return edException.Message;
            }
        }

        public static string PlaceDoubleDownBet()
        {
            decimal chipAmount = GetActivePlayerHand().Bet.ChipAmount * 2; 
            string placeDoubleDownBetStr; 
            
            try
            {
                EnsureGameInstanceExists();

                if (GetDealer().ChipAmount < chipAmount)
                {
                    throw new InsufficientChipsException(chipAmount, Game.BlackjackEntityToClassName(GetDealer()));
                }
                
                placeDoubleDownBetStr = game.PlaceDoubleDownBet();

                if (!GetActivePlayerHand().IsBusted)
                { 
                    GetActivePlayerHand().Status = HandStatus.Standing;
                }

                return placeDoubleDownBetStr; 
            }
            catch (InvalidOperationException iiException)
            {
                return iiException.Message;
            }
            catch (EmptyDeckException edException)
            {
                return edException.Message;
            }
            catch (InsufficientChipsException icException)
            {
                return icException.Message;
            }
        }
        public static string PlaceInsuranceBet()
        {
            try
            {
                EnsureGameInstanceExists();

                if (!game.InsuranceBetPossible)
                {
                    throw new InvalidOperationException("Placing an insurance bet is not possible at this time.");
                }

                return game.PlaceInsuranceBet();
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message;
            }
            catch (InsufficientChipsException icException)
            {
                return icException.Message;
            }
            catch (InvalidInputException iiException)
            {
                return iiException.Message;
            }
        }
        public static string PlaceMainBet(string inputStr)
        {
            decimal chipAmount;

            try
            {
                EnsureGameInstanceExists();
                chipAmount = Convert.ToDecimal(inputStr);

                if (chipAmount <= 0)
                {
                    throw new InvalidInputException(chipAmount.ToString());
                }
                else if (GetDealer().ChipAmount < chipAmount)
                {
                    throw new InsufficientChipsException(chipAmount, Game.BlackjackEntityToClassName(GetDealer()));
                }

                game.PlaceMainBet(chipAmount);
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message;
            }
            catch (InsufficientChipsException icException)
            {
                return icException.Message;
            }
            catch (InvalidInputException iiException)
            {
                return iiException.Message;
            }
            catch (Exception exception)
            {
                //FIX THIS UP LATER WITH SPECIFIC EXCEPTIONS
                return exception.Message;
            }

            return $"You have placed a bet of {chipAmount:C}.";
        }

        public static void ResetConditions()
        {
            game.ResetConditions(); 
        }

        public static void SetActivePlayerHand(PlayerHand playerHand)
        {
            game.ActivePlayerHand = playerHand;
        }

        public static string ShowdownBlackjack(PlayerHand playerHand)
        {
            return game.ShowdownBlackjack(playerHand);
        }
        public static string ShowdownBusted(PlayerHand playerHand)
        {
            return game.ShowdownBusted(playerHand);
        }
        public static string Showdown(PlayerHand playerHand)
        {
            return game.ShowdownNormal(playerHand);
        }

        public static string Split()
        {
            try
            {
                return game.Split(); 
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message; 
            }
            catch (InsufficientChipsException icException)
            {
                return icException.Message;
            }
            catch (EmptyDeckException edException)
            {
                return edException.Message;
            }
        }
    }
}