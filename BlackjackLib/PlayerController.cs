//Quinn Keenan, 301504914, 19/08/2025

namespace BlackjackLib
{
    public static class PlayerController
    {
        static readonly char DOUBLE_DOWN_ABBREVIATION = 'D'; 
        static Game game;
        static readonly char HIT_ABBREVIATION = 'H'; 
        static readonly string SPLIT_ABBREVIATION = "SP";
        static readonly string STAND_ABBREVIATION = "ST";
        static readonly string SURRENDER_ABBREVIATION = "SU"; 

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
            char inputChar; 
            string doubleDownStr; 
            
            try
            {
                EnsureGameInstanceExists();

                if (inputStr == SPLIT_ABBREVIATION)
                {
                    return Split();
                }
                else if (inputStr == STAND_ABBREVIATION)
                {
                    return game.Stand(GetActivePlayerHand());
                }
                else if (inputStr == SURRENDER_ABBREVIATION && GetActivePlayerHand().CanSurrender)
                {
                    return game.Surrender(GetActivePlayerHand()); 
                }
                else if (inputStr.Length != 1)
                {
                    throw new InvalidInputException(inputStr);
                }

                inputChar = Convert.ToChar(inputStr); 

                if (inputChar == DOUBLE_DOWN_ABBREVIATION)
                {
                    doubleDownStr = PlaceDoubleDownBet();
                    return doubleDownStr;
                }
                else if (inputChar == HIT_ABBREVIATION)
                {
                    return Hit(GetActivePlayerHand());
                }
                else
                {
                    throw new InvalidInputException(inputStr);
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
            catch (Exception exception)
            {
                return exception.Message; //FIX THIS UP LATER. 
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
        public static bool GetInsuranceBetWon()
        {
            return game.InsuranceBetWon;
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

        private static string Hit(PlayerHand playerHand)
        {
            try
            {
                EnsureGameInstanceExists();
                return game.Hit(playerHand);
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

        private static string PlaceDoubleDownBet()
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
        public static string ShowdownNormal(PlayerHand playerHand)
        {
            return game.ShowdownNormal(playerHand);
        }
        public static string ShowdownSurrendered(PlayerHand playerHand)
        {
            return game.ShowdownSurrendered(playerHand);
        }

        private static string Split()
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