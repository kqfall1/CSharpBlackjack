//Quinn Keenan, 301504914, 19/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal class Game
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

        internal const byte BUST_SCORE_LIMIT = 21;
        internal readonly Dealer Dealer;

        private bool dealerHasPlayed;
        internal bool DealerHasPlayed
        {
            get
            {
                return dealerHasPlayed;
            }
            private set
            {
                dealerHasPlayed = value;
            }
        }

        internal const int INITIAL_CHIP_AMOUNT = 5000;
        internal const byte MAXIMUM_SPLIT_LEVEL = 1; 

        internal bool InsuranceBetPossible
        {
            get
            {
                DealerHand dealerHand = Dealer.MainHand as DealerHand;

                return Player.MainHand.UpCards.Count == 2 && 
                       dealerHand.UpCards[0].Rank is Rank.Ace && 
                       MainPot.ChipAmount / 2 <= Player.ChipAmount &&
                       MainPot.ChipAmount / 2 <= Dealer.ChipAmount &&
                       Player.InsuranceBet is null && InsurancePot.ChipAmount == 0; 
            }
        }

        internal bool InsuranceBetWon
        {
            get
            {
                return Dealer.MainHand.IsBlackjack && Player.InsuranceBet is not null; 
            }
        }

        private Pot insurancePot;
        internal Pot InsurancePot
        {
            get
            {
                return insurancePot;
            }
        }

        internal bool IsActive
        {
            get
            {
                return Player.ChipAmount > 0 && Dealer.ChipAmount > 0;
            }
        }

        internal readonly Player Player;

        private Pot mainPot;
        internal Pot MainPot
        {
            get
            {
                return mainPot;
            }
        }

        internal bool PlayerIsPlaying
        {
            get
            {
                return (ActivePlayerHand.Status is HandStatus.Drawing || ActivePlayerHand.Status is HandStatus.WaitingOnSplitHand) &&
                       PlayerController.GetDealerMainHand().Status is HandStatus.WaitingToDraw; 
            }
        }

        internal bool ShouldDealerPlay
        {
            get
            {
                return !PlayerIsPlaying &&
                       !ActivePlayerHand.IsBlackjack &&
                       !ActivePlayerHand.IsBusted &&
                       ActivePlayerHand.Status is not HandStatus.Surrendered &&
                       Dealer.MainHand.Status == HandStatus.WaitingToDraw;
            }
        }

        private Pot splitPot;
        internal Pot SplitPot
        {
            get
            {
                return splitPot;
            }
        }

        internal Game()
        {
            Dealer = new Dealer(new Deck());
            Dealer.MainHand = new DealerHand(Dealer);
            insurancePot = new Pot(); 
            Player = new Player();
            Player.MainHand = new PlayerHand(null, HandType.Dealt, Player);
            Player.SplitHand = new PlayerHand(null, HandType.SplitOnce, Player);
            mainPot = new Pot();
            splitPot = new Pot();
        }

        internal void AlterActivePlayerHand()
        {
            if (ActivePlayerHand.HandType is HandType.SplitOnce)
            {
                Player.MainHand.Status = HandStatus.Drawing;
                ActivePlayerHand = Player.MainHand as PlayerHand;
            }
        }
        internal string AlterConditionsAfterInsuranceBet()
        {
            decimal dealerWinnings;
            decimal insuranceBetChipAmount = Player.InsuranceBet.ChipAmount;
            decimal insuranceBetWinnings;

            if (Dealer.MainHand.Status is HandStatus.Blackjack)
            {
                insuranceBetWinnings = Player.InsuranceBet.Payout(Dealer, PayoutRatio.INSURANCE_BET);
                Player.AddChips(insuranceBetWinnings);
                return $"You win {insuranceBetWinnings:C} on your insurance bet of {insuranceBetChipAmount:C}";
            }

            dealerWinnings = InsurancePot.Scoop();
            Dealer.AddChips(dealerWinnings);
            Player.InsuranceBet = null;
            return $"The dealer does not have a blackjack! You forfeit your bet of {insuranceBetChipAmount:C}. Play on!";
        }
        internal void AlterHandStatusesAfterDealing()
        {
            if (Player.MainHand.IsBlackjack)
            {
                Player.MainHand.Status = HandStatus.Blackjack;
                Dealer.MainHand.Status = HandStatus.Standing; 
            }
            else
            {
                Player.MainHand.Status = HandStatus.Drawing;
            }

            if (Dealer.MainHand.IsBlackjack)
            {
                Dealer.MainHand.Status = HandStatus.Blackjack; 

                if (Player.MainHand.Status is HandStatus.Drawing)
                {
                    Player.MainHand.Status = HandStatus.Standing; 
                }
            }
        }

        internal static string BlackjackEntityToClassName(BlackjackEntity blackjackEntity)
        {
            return blackjackEntity.GetType().Name;
        }

        internal string Deal()
        {
            DealerHand dealerHand = Dealer.MainHand as DealerHand; 
            Card dealerUpCard; 
            string dealStr; 

            Hit(Player.MainHand);
            Hit(dealerHand);
            Hit(Player.MainHand);
            dealerHand.DownCard = Dealer.DealCard();

            ActivePlayerHand = Player.MainHand as PlayerHand;
            Dealer.MainHand = dealerHand;
            dealerUpCard = dealerHand.UpCards[0];
            dealStr = $"Your hand:\n\n\t{ActivePlayerHand.ToString()}.\n\n\tDealer's up card: {dealerUpCard.ToString()}.";

            if (InsuranceBetPossible)
            {
                return dealStr;
            }

            return $"{dealStr}\n\nGood luck!"; 
        }

        internal string DealerPlay()
        {
            string dealerPlayStr = $"\n\t{Dealer.MainHand.ToString()}\n\n";
            DealerHasPlayed = true;

            while (Dealer.MustHit)
            {
                dealerPlayStr = $"{dealerPlayStr}The dealer draws the {Dealer.MainHand.Hit(Dealer.DealCard())}. ";

                if (Dealer.MainHand.IsBusted)
                {
                    return dealerPlayStr;
                }
            }

            return $"{dealerPlayStr}{Stand(Dealer.MainHand)}";
        }

        static string DetermineShowdownStringPreamble(PlayerHand playerHand)
        { 
            switch (playerHand.HandType)
            {
                case HandType.Dealt:
                    return "Main hand:"; 
                case HandType.SplitOnce:
                    return "Split hand:"; 
                default:
                    throw new InvalidOperationException($"Unexpected hand type in Game.Showdown(): {playerHand.HandType}.");
            }
        }

        internal string Hit(Hand hand)
        {
            string hitStr = ""; 

            if (hand is DealerHand dealerMainHand)
            {
                hitStr = $"{BlackjackEntityToClassName(dealerMainHand.Dealer)} draws the {dealerMainHand.Hit(Dealer.DealCard())}.";
            }
            else if (hand is PlayerHand playerMainHand) 
            {
                hitStr = $"{BlackjackEntityToClassName(playerMainHand.Player)} draws the {playerMainHand.Hit(Dealer.DealCard())}. Score: {playerMainHand.Score}.";
            }
            else
            {
                throw new InvalidOperationException($"Unexpected hand type in Game.Hand(): {hand}.");
            }
             
            return hitStr; 
        }

        internal string PlaceDoubleDownBet()
        {
            string placeDoubleDownBetStr;  

            try
            {
                placeDoubleDownBetStr = Player.PlaceDoubleDownBet(ActivePlayerHand);
                return $"{placeDoubleDownBetStr}\n{Hit(ActivePlayerHand)}";
            }
            catch (InsufficientChipsException)
            {
                throw; 
            }
        }
        internal string PlaceInsuranceBet()
        {
            decimal insuranceBetChipAmount = MainPot.ChipAmount / 2; 
            string insuranceBetStr = $"You have placed an insurance bet of {insuranceBetChipAmount:C}";
            
            InsurancePot.ChipAmount = insuranceBetChipAmount;
            Player.InsuranceBet = Player.PlaceBet(insuranceBetChipAmount, insurancePot);
            return insuranceBetStr; 
        }
        internal void PlaceMainBet(decimal amount)
        {
            PlayerHand playerMainHand = Player.MainHand as PlayerHand;
            Dealer.MainHand.Status = HandStatus.WaitingToDraw;
            playerMainHand.Bet = Player.PlaceBet(amount, MainPot);
            playerMainHand.Status = HandStatus.Drawing;
            Player.MainHand = playerMainHand;
        }

        internal void ResetConditions()
        {
            Dealer.MainHand = new DealerHand(Dealer);
            DealerHasPlayed = false;
            Player.MainHand = new PlayerHand(null, HandType.Dealt, Player);
            Player.SplitHand = new PlayerHand(null, HandType.SplitOnce, Player); 
            Player.InsuranceBet = null;
            Dealer.Shuffle(); 
        }

        decimal PayoutPlayerHandBet(PayoutRatio payoutRatio, PlayerHand playerHand)
        {
            decimal winnings = playerHand.Bet.Payout(Dealer, payoutRatio);
            Player.AddChips(winnings);
            return winnings; 
        }

        internal string ShowdownBlackjack(PlayerHand playerHand)
        {
            string showdownString = DetermineShowdownStringPreamble(playerHand);

            if (playerHand.Status is HandStatus.Blackjack && Dealer.MainHand.Status is HandStatus.Blackjack)
            {
                return $"{showdownString} both you and the dealer were dealt a blackjack. Your bet of {PayoutPlayerHandBet(PayoutRatio.PUSH, playerHand):C} is pushed.";
            }
            else if (playerHand.Status is HandStatus.Blackjack)
            {
                return $"{showdownString} you were dealt a blackjack. You are victorious and receive {PayoutPlayerHandBet(PayoutRatio.MAIN_BET_BLACKJACK, playerHand):C} from your bet of {playerHand.Bet.ChipAmount:C}."; 
            }
            else if (Dealer.MainHand.Status is HandStatus.Blackjack)
            {
                Dealer.AddChips(playerHand.Bet.Pot.Scoop());
                return $"{showdownString} the dealer was dealt a blackjack. You have been defeated and forfeit your bet of {playerHand.Bet.ChipAmount:C}."; 
            }
            else
            {
                throw new InvalidOperationException("Game.ShowdownBlackjack() cannot determine which blackjack entity has a blackjack.");
            }
        }
        internal string ShowdownBusted(PlayerHand playerHand)
        {
            string showdownString = DetermineShowdownStringPreamble(playerHand);

            if (playerHand.IsBusted)
            {
                Dealer.AddChips(playerHand.Bet.Pot.Scoop());
                return $"{showdownString} you bust with a score of {playerHand.Score}. You are defeated and forfeit your bet of {playerHand.Bet.ChipAmount:C}.";
            }
            else if (Dealer.MainHand.IsBusted)
            {
                return $"{showdownString} the dealer busts with a score of {Dealer.MainHand.Score}. You are victorious and receive {PayoutPlayerHandBet(PayoutRatio.MAIN_BET, playerHand):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
            }
            else
            {
                throw new InvalidOperationException("Game.ShowdownBusted cannot determine which blackjack entity has busted.");
            }
        }
        internal string ShowdownNormal(PlayerHand playerHand)
        {
            string showdownString = $"{DetermineShowdownStringPreamble(playerHand)} your score is {playerHand.Score} and the dealer's score is {Dealer.MainHand.Score}.";

            if (playerHand.Score > Dealer.MainHand.Score)
            {
                return $"{showdownString} You are victorious and receive {PayoutPlayerHandBet(PayoutRatio.MAIN_BET, playerHand):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
            }
            else if (playerHand.Score < Dealer.MainHand.Score)
            {
                Dealer.AddChips(playerHand.Bet.Pot.Scoop());
                return $"{showdownString} You have been defeated and forfeit your bet of {playerHand.Bet.ChipAmount:C}.";
            }
            else
            {
                return $"{showdownString} You have tied the dealer's score and have your bet of {PayoutPlayerHandBet(PayoutRatio.PUSH, playerHand):C} pushed.";
            }
        }
        internal string ShowdownSurrendered(PlayerHand playerHand)
        {
            return $"{DetermineShowdownStringPreamble(playerHand)} You receive {PayoutPlayerHandBet(PayoutRatio.SURRENDER, playerHand):C} from your bet of {playerHand.Bet.ChipAmount:C}.";
        }

        internal string Split()
        {
            PlayerHand playerMainHand = Player.MainHand as PlayerHand;
            decimal potChipAmount = playerMainHand.Bet.ChipAmount;  
            Card splitCard = playerMainHand.UpCards[1];
            string splitStr; 

            if (!playerMainHand.CanSplit)
            {
                throw new InvalidOperationException($"You cannot split hand:\n\n\t{playerMainHand.ToString()}.");
            }

            Player.MainHand = new PlayerHand(playerMainHand.Bet, HandType.Dealt, Player);
            Player.MainHand.UpCards.Add(playerMainHand.UpCards[0]);
            splitStr = $"First hand: {Hit(Player.MainHand)}\n";
            Player.MainHand.Status = HandStatus.WaitingOnSplitHand;

            //Call Player.placebet()
            Player.SplitHand = new PlayerHand(Player.PlaceBet(potChipAmount, SplitPot), HandType.SplitOnce, Player);
            Player.SplitHand.UpCards.Add(splitCard);
            splitStr = $"Second hand: {splitStr}{Hit(Player.SplitHand)}\n";
            Player.SplitHand.Status = HandStatus.Drawing;
            ActivePlayerHand = Player.SplitHand;

            return $"Your first hand (score: {Player.MainHand.Score}) is now:\n\n\t{Player.MainHand.ToString()}\n\nYour second hand (score: {Player.SplitHand.Score}) is now:\n\n\t{Player.SplitHand.ToString()}";
        }

        internal string Stand(Hand hand)
        {
            hand.Status = HandStatus.Standing;

            if (hand is DealerHand dealerHand)
            {
                return $"{BlackjackEntityToClassName(Dealer)} elects to stand on {hand.Score}.";
            }
            else if (hand is PlayerHand playerHand) 
            {
                return $"{BlackjackEntityToClassName(Player)} elects to stand on {hand.Score}.";
            }
            else
            {
                throw new InvalidOperationException($"Unexpected hand type in Game.Stand(): \"{hand.GetType()}\"."); 
            }
        }

        internal string Surrender(PlayerHand playerHand)
        {
            playerHand.Status = HandStatus.Surrendered;
            return $"You surrender on a score of {playerHand.Score}.";
        }

        public override string ToString()
        {
            return $"Dealer: {Dealer}\n\nPlayer: {Player}";
        }
    }
}