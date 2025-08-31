//Quinn Keenan, 301504914, 19/08/2025

namespace BlackjackLib
{
    public class BlackjackEntitiesController
    {
        private PlayerHand activePlayerHand;
        public PlayerHand ActivePlayerHand
        {
            get
            {
                return activePlayerHand;
            }
            internal set
            {
                activePlayerHand = value;
            }
        }

        internal readonly Dealer Dealer;
        internal readonly Player Player;

        internal BlackjackEntitiesController()
        {
            Dealer = new Dealer();
            Player = new Player();
            ActivePlayerHand = Player.MainHand as PlayerHand; 
        }

        internal void AlterHandStatusesAfterDealing()
        {
            if (Dealer.MainHand.IsBlackjack && !ActivePlayerHand.IsBlackjack)
            {
                ActivePlayerHand.Status = HandStatus.Standing; 
            }
            else if (!Dealer.MainHand.IsBlackjack && ActivePlayerHand.IsBlackjack)
            {
                Dealer.MainHand.Status = HandStatus.Standing;
            }
        }
        public void AlterActivePlayerHandAfterDrawingOnASplitHand(PlayerHand playerMainHand)
        {
            playerMainHand.Status = HandStatus.Drawing;
            ActivePlayerHand = playerMainHand;
        }

        internal string Deal()
        {
            try
            {
                Dealer.Deal(ActivePlayerHand);
                return StringInputOutputManager.DetermineDealBriefString(Dealer.MainHand as DealerHand, ActivePlayerHand); 
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

        internal string ExecuteDealerAction()
        {
            try
            {
                Dealer.Hit(Dealer.MainHand);

                if (Dealer.MainHand.IsBusted)
                {
                    return $"{StringInputOutputManager.DetermineEntityActionString(Dealer.MainHand as DealerHand)} {StringInputOutputManager.DetermineEntityBustsString(Dealer.MainHand)}";
                }
                else if (Dealer.MainHand.Status is HandStatus.Standing)
                {
                    return $"{StringInputOutputManager.DetermineEntityActionString(Dealer.MainHand as DealerHand)} {StringInputOutputManager.DetermineEntityStandsString(Dealer.MainHand)}";
                }

                return $"{StringInputOutputManager.DetermineEntityActionString(Dealer.MainHand as DealerHand)}";
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

        public string ExecutePlayerAction(string inputStr)
        {
            char inputChar; 
            
            try
            {
                switch (inputStr)
                {
                    case StringInputOutputManager.SPLIT_ABBREVIATION: 
                        Player.SplitHand = Player.Split(Dealer);
                        ActivePlayerHand = Player.SplitHand;
                        return StringInputOutputManager.DetermineSplitString(Player.MainHand as PlayerHand, Player.SplitHand);
                    case StringInputOutputManager.STAND_ABBREVIATION:
                        ActivePlayerHand.Stand();
                        return StringInputOutputManager.DetermineEntityStandsString(ActivePlayerHand);
                    case StringInputOutputManager.SURRENDER_ABBREVIATION:
                        ActivePlayerHand.Surrender();
                        return StringInputOutputManager.DetermineSurrenderString(ActivePlayerHand); 
                }

                inputChar = Convert.ToChar(inputStr);

                switch (inputChar)
                {
                    case StringInputOutputManager.DOUBLE_DOWN_ABBREVIATION:
                        Player.DoubleDownOnBet(Dealer, ActivePlayerHand);
                        return StringInputOutputManager.DetermineDoubleDownString(ActivePlayerHand); 
                    case StringInputOutputManager.HIT_ABBREVIATION:
                        Dealer.Hit(ActivePlayerHand);
                        return StringInputOutputManager.DetermineEntityActionString(ActivePlayerHand);
                    default:
                        throw new InvalidStringInputException(inputStr);
                }
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message;
            }
            catch (InvalidStringInputException iiException)
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

        public string PlaceInsuranceBet()
        {
            try
            {
                if (!InsuranceBetManager.InsuranceBetPossible(Dealer, Player))
                {
                    throw new InvalidOperationException("Placing an insurance bet is not possible at this time.");
                }

                InsuranceBetManager.PlaceInsuranceBet(Dealer, Player);
                return $"You have placed an insurance bet of {ActivePlayerHand.InsuranceBet.ChipAmount:C}";
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message;
            }
            catch (InsufficientChipsException icException)
            {
                return icException.Message;
            }
            catch (InvalidStringInputException iiException)
            {
                return iiException.Message;
            }
        }

        public string PlaceMainBet(decimal chipAmount)
        {
            PlayerHand mainHand = Player.MainHand as PlayerHand;
            mainHand.PlaceMainBet(chipAmount, Dealer);
            return $"You have placed a bet of {chipAmount:C} on your {ActivePlayerHand.HandTypeString}.";
        }
    }
}