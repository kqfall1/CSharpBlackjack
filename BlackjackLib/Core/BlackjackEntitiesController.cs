//Quinn Keenan, 301504914, 19/08/2025

namespace BlackjackLib
{
    internal class BlackjackEntitiesController
    {
        private PlayerHand activePlayerHand;
        internal PlayerHand ActivePlayerHand
        {
            get
            {
                return activePlayerHand;
            }
            set
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

        public void AlterActivePlayerHandAfterDrawing()
        {
            if (ActivePlayerHand == Player.SplitHand)
            {
                Player.MainHand.Status = HandStatus.Drawing;
                ActivePlayerHand = Player.MainHand as PlayerHand;
            }
        }

        internal string Deal()
        {
            try
            {
                Dealer.Deal(ActivePlayerHand);
            }
            catch (EmptyDeckException edException)
            {
                return edException.Message;
            }

            return MessageManager.DealBriefString(Dealer.MainHand as DealerHand, ActivePlayerHand);
        }

        internal string ExecuteDealerAction()
        {
            string entityActionString; 
            
            try
            {
                Dealer.Hit(Dealer.MainHand);
            }
            catch (EmptyDeckException edException)
            {
                return edException.Message;
            }

            entityActionString = MessageManager.EntityActionString(Dealer.MainHand as DealerHand);

            if (Dealer.MainHand.IsBusted)
            {
                return $"{entityActionString} {MessageManager.EntityBustsString(Dealer.MainHand)}";
            }
            else if (Dealer.MainHand.Status is HandStatus.Standing)
            {
                return $"{entityActionString} {MessageManager.EntityStandsString(Dealer.MainHand)}";
            }

            return entityActionString;
        }

        public string ExecutePlayerAction(PlayerInputAbbreviation playerInputAbbreviation)
        {
            try
            {
                switch (playerInputAbbreviation)
                {
                    case PlayerInputAbbreviation.D:
                        ActivePlayerHand.DoubleDownOnBet(Dealer);
                        ActivePlayerHand.Stand();
                        return MessageManager.DoubleDownString(ActivePlayerHand);
                    case PlayerInputAbbreviation.H:
                        Dealer.Hit(ActivePlayerHand);
                        return MessageManager.EntityActionString(ActivePlayerHand);
                    case PlayerInputAbbreviation.SP: 
                        Player.SplitHand = ActivePlayerHand.Split(Dealer);
                        ActivePlayerHand = Player.SplitHand;
                        return MessageManager.SplitString(Player.MainHand as PlayerHand, Player.SplitHand);
                    case PlayerInputAbbreviation.ST:
                        ActivePlayerHand.Stand();
                        return MessageManager.EntityStandsString(ActivePlayerHand);
                    default: 
                        ActivePlayerHand.Surrender();
                        return MessageManager.SurrenderString(ActivePlayerHand); 
                }
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message;
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
                if (!InsuranceManager.InsuranceBetPossible(Dealer, Player))
                {
                    throw new InvalidOperationException(MessageManager.INSURANCE_BET_NOT_POSSIBLE_MESSAGE);
                }

                InsuranceManager.PlaceInsuranceBet(Dealer, Player);
            }
            catch (InvalidOperationException ioException)
            {
                return ioException.Message;
            }
            catch (InsufficientChipsException icException)
            {
                return icException.Message;
            }

            return MessageManager.InsuranceBetBriefString(ActivePlayerHand.InsuranceBet.ChipAmount);
        }

        public string PlaceMainBet(decimal chipAmount)
        {
            try
            {
                PlayerHand mainHand = Player.MainHand as PlayerHand;
                mainHand.PlaceMainBet(chipAmount, Dealer);
            }
            catch (InsufficientChipsException icException)
            {
                return icException.Message;
            }

            return MessageManager.MainBetBriefString(ActivePlayerHand);
        }
    }
}