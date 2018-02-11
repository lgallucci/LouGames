using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CardGame;

namespace Hearts
{
    internal class AI
    {
        /* Properties */
        private Hand _AIHand;
        internal Hand AIHand
        {
            get { return _AIHand; }
            set { _AIHand = value; }
        }

        private string name;
        internal string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<int> score;
        internal ReadOnlyCollection<int> Score
        {
            get { return score.AsReadOnly(); }
        }

        private int roundScore;
        internal int RoundScore
        {
            get { return roundScore; }
            set { roundScore = value; }
        }

        private bool preferredSpades;
        internal bool PreferredSpades
        {
            get { return preferredSpades; }
            set { preferredSpades = value; }
        }

        private bool preferredHearts;
        internal bool PreferredHearts
        {
            get { return preferredHearts; }
            set { preferredHearts = value; }
        }

        private bool preferredClubs;
        internal bool PreferredClubs
        {
            get { return preferredClubs; }
            set { preferredClubs = value; }
        }

        private bool preferredDiamonds;
        internal bool PreferredDiamonds
        {
            get { return preferredDiamonds; }
            set { preferredDiamonds = value; }
        }

        /* Constructor */
        internal AI()
        {
            AIHand = new Hand();
            score = new List<int>();
        }

        /* AI Card Action Methods */
        private Card ChooseLead(PlayState playState)
        {
            Card ret;
            if (AIHand.Contains(Suit.Clubs, FaceValue.Two))
            {
                return new Card(Suit.Clubs, FaceValue.Two, true);
            }
            else
            {
                if (!playState.QueenHasBeenPlayed && !AIHand.Contains(Suit.Spades, FaceValue.Queen))
                    ret = GetHighestSpadeBelowQueen(playState);
                else if (playState.IsBroken)
                    ret = GetLowestCard();
                else
                    ret = GetLowestNonPointCard();

                if (ret == null)
                    ret = GetLowestCard();

                return ret;
            }
        }

        private Card ChooseFollow(PlayState playState)
        {
            Card ret = null;
            if (!HasSuit(playState.Lead.CardSuit))
            {
                if (playState.Lead.CardSuit == Suit.Clubs && playState.Lead.CardValue == FaceValue.Two)
                    ret = GetHighestNonPointCard();
                else if (AIHand.Contains(Suit.Spades, FaceValue.Queen))
                    ret = new Card(Suit.Spades, FaceValue.Queen, true);
                else
                    ret = GetHighestHeartOrOtherCard();
            }
            else
            {
                if (playState.Lead.CardSuit == Suit.Clubs && playState.Lead.CardValue == FaceValue.Two)
                    ret = GetHighestCardOfSuit(playState, Suit.Clubs);
                else if (playState.Trick.Count < 3 || PointInTrick(playState))
                    ret = TryToNotWinTrick(playState, GetHighestTrickCard(playState), playState.Trick.Count == 3 ? true : false);
                else
                    ret = GetHighestCardOfSuit(playState, playState.Lead.CardSuit);

                if (ret.CardSuit != playState.Lead.CardSuit)
                    throw new InvalidOperationException("AI - NOT FOLLOWING SUIT");
            }

            if (playState.Lead.CardSuit == Suit.Clubs && playState.Lead.CardValue == FaceValue.Two)
                if (ret.CardSuit == Suit.Spades && ret.CardValue == FaceValue.Queen || ret.CardSuit == Suit.Hearts)
                    throw new InvalidOperationException("AI - PLAYED POINT CARD FIRST TRICK!");

            return ret;
        }

        internal Card Play(PlayState playState)
        {
            Card ret = null;
            if (playState.Trick.Count == 0)
            {
                if ((ret = ChooseLead(playState)) == null)
                    throw new InvalidOperationException("AI - CHOOSE LEAD FAILED!");
            }
            else
                if ((ret = ChooseFollow(playState)) == null)
                    throw new InvalidOperationException("AI - CHOOSE FOLLOW FAILED!");

            if (!AIHand.Contains(ret.CardSuit, ret.CardValue))
                throw new InvalidOperationException("AI - PLAYED CARD NOT IN HAND !");

            AIHand.Remove(ret);
            AIHand.Sort();
            return ret;
        }

        internal ReadOnlyCollection<int> PassIndexes()
        {
            Collection<int> ret = new Collection<int>();
            int suit = CheckShortSuited();

            if (AIHand.Contains(Suit.Spades, FaceValue.Queen))
            {
                ret.Add(AIHand.IndexOf(Suit.Spades, FaceValue.Queen));
            }

            if (AIHand.Contains(Suit.Spades, FaceValue.King))
            {
                ret.Add(AIHand.IndexOf(Suit.Spades, FaceValue.King));
            }

            if (AIHand.Contains(Suit.Spades, FaceValue.Ace))
            {
                ret.Add(AIHand.IndexOf(Suit.Spades, FaceValue.Ace));
            }

            if (suit != -1 && ret.Count == 0)
            {
                for (int i = 0; i < AIHand.Cards.Count && ret.Count < 3; i++)
                {
                    if (AIHand.Cards[i].CardSuit == (Suit)suit && !ret.Contains(i))
                    {
                        ret.Add(i);
                    }
                }
            }

            //Return High Cards Starting with Hearts
            int removeValue = (int)CardRank.Ten;
            while (ret.Count < 3)
            {
                for (int i = AIHand.Cards.Count - 1; i >= 0 && ret.Count < 3; i--)
                {
                    if (AIHand.Cards[i].CardRank >= removeValue)
                    {
                        if (AIHand.Cards[i].CardSuit == Suit.Spades && AIHand.Cards[i].CardValue == FaceValue.Ace)
                        {
                            if (!ret.Contains(i))
                            {
                                ret.Add(i);
                            }
                        }
                        else if (AIHand.Cards[i].CardSuit == Suit.Spades && AIHand.Cards[i].CardValue == FaceValue.King)
                        {
                            if (!ret.Contains(i))
                            {
                                ret.Add(i);
                            }
                        }
                        else if (AIHand.Cards[i].CardSuit == Suit.Spades && AIHand.Cards[i].CardValue == FaceValue.Queen)
                        {
                            if (!ret.Contains(i))
                            {
                                ret.Add(i);
                            }
                        }
                        else if (!ret.Contains(i))
                        {
                            ret.Add(i);
                        }
                    }
                }
                removeValue--;
            }

            return new ReadOnlyCollection<int>(ret);
        }

        internal Collection<Card> Pass()
        {
            if (AIHand.Cards.Count != 13)
                throw new InvalidOperationException("AI - HAND IS NOT FULL ON PASS");

            Collection<Card> ret = new Collection<Card>();
            int suit = CheckShortSuited();

            if (AIHand.Contains(Suit.Spades, FaceValue.Queen))
            {
                ret.Add(new Card(Suit.Spades, FaceValue.Queen, true));
            }

            if (AIHand.Contains(Suit.Spades, FaceValue.King))
            {
                ret.Add(new Card(Suit.Spades, FaceValue.King, true));
            }

            if (AIHand.Contains(Suit.Spades, FaceValue.Ace))
            {
                ret.Add(new Card(Suit.Spades, FaceValue.Ace, true));
            }

            if (suit != -1 && ret.Count == 0)
            {
                for (int i = 0; i < AIHand.Cards.Count && ret.Count < 3; i++)
                {
                    if (AIHand.Cards[i].CardSuit == (Suit)suit && !ret.Contains(AIHand.Cards[i]))
                    {
                        ret.Add(AIHand.Cards[i]);
                    }
                }
            }

            //Return High Cards Starting with Hearts
            int removeValue = (int)CardRank.Ten;
            while (ret.Count < 3)
            {
                for (int i = AIHand.Cards.Count - 1; i >= 0 && ret.Count < 3; i--)
                {
                    if (AIHand.Cards[i].CardRank >= removeValue)
                    {
                        if (AIHand.Cards[i].CardSuit == Suit.Spades && AIHand.Cards[i].CardValue == FaceValue.Ace)
                        {
                            if (!ret.Contains(new Card(Suit.Spades, FaceValue.Ace, true)))
                            {
                                ret.Add(AIHand.Cards[i]);
                            }
                        }
                        else if (AIHand.Cards[i].CardSuit == Suit.Spades && AIHand.Cards[i].CardValue == FaceValue.King)
                        {
                            if (!ret.Contains(new Card(Suit.Spades, FaceValue.King, true)))
                            {
                                ret.Add(AIHand.Cards[i]);
                            }
                        }
                        else if (AIHand.Cards[i].CardSuit == Suit.Spades && AIHand.Cards[i].CardValue == FaceValue.Queen)
                        {
                            if (!ret.Contains(new Card(Suit.Spades, FaceValue.Queen, true)))
                            {
                                ret.Add(AIHand.Cards[i]);
                            }
                        }
                        else if (!ret.Contains(AIHand.Cards[i]))
                        {
                            ret.Add(AIHand.Cards[i]);
                        }
                    }
                }
                removeValue--;
            }

            Remove(ret);

            if (AIHand.Cards.Count != 10)
                throw new InvalidOperationException("AI - NOT REMOVING CORRECTLY");

            return ret;
        }

        /* AI State Change Methods */
        internal void AddToScore(int position, int newScore)
        {
            score[position] += newScore;
        }

        internal void CreateNewScore(int startScore)
        {
            score.Add(startScore);
        }

        internal void SetHand(Hand hand)
        {
            _AIHand.SetHand(hand.Cards);

            if (AIHand.Contains(Suit.Spades, FaceValue.Queen))
            {
                preferredSpades = false;
            }
        }

        internal void Add(Collection<Card> added)
        {
            foreach (Card card in added)
            {
                AIHand.Add(card);
            }
        }

        private void Remove(Collection<Card> removed)
        {
            foreach (Card c in removed)
            {
                AIHand.Remove(c);
            }
        }

        internal void Reset()
        {
            AIHand = new Hand();
            score.Clear();
            roundScore = 0;
            preferredClubs = false;
            preferredDiamonds = false;
            preferredHearts = false;
            preferredSpades = false;
        }

        internal void RemoveSuitPreference(Suit suit)
        {
            switch ((int)suit)
            {
                case 0:
                    preferredClubs = false;
                    break;
                case 1:
                    preferredDiamonds = false;
                    break;
                case 2:
                    preferredSpades = false;
                    break;
                case 3:
                    preferredHearts = false;
                    break;
            }
        }

        /* AI Specific Card Type Retrieve Methods */
        private static Card GetHighestTrickCard(PlayState playState)
        {
            Card ret = playState.Trick[0];
            foreach (Card c in playState.Trick)
            {
                if (c.CardRank > ret.CardRank)
                    ret = c;
            }
            return ret;
        }

        private Card TryToNotWinTrick(PlayState playState, Card highCard, bool last)
        {
            if ((playState.Trick.Contains(new Card(Suit.Spades, FaceValue.Ace, true))
                || playState.Trick.Contains(new Card(Suit.Spades, FaceValue.King, true)))
                && AIHand.Contains(Suit.Spades, FaceValue.Queen) && playState.Lead.CardSuit == Suit.Spades)
            {
                return new Card(Suit.Spades, FaceValue.Queen, true);
            }
            Card ret = null;
            foreach (Card c in AIHand.Cards)
            {
                if (c.CardSuit == playState.Lead.CardSuit)
                {
                    if (c.CardRank <= highCard.CardRank)
                    {
                        if (!(c.CardSuit == Suit.Spades && c.CardValue == FaceValue.Queen))
                            ret = c;
                    }
                }
            }

            if (ret == null)
            {
                if (playState.Lead.CardSuit == Suit.Spades)
                {
                    if (AIHand.Contains(Suit.Spades, FaceValue.King) && AIHand.Contains(Suit.Spades, FaceValue.Queen))
                        ret = new Card(Suit.Spades, FaceValue.King, true);
                    else if (AIHand.Contains(Suit.Spades, FaceValue.Ace) && AIHand.Contains(Suit.Spades, FaceValue.Queen))
                        ret = new Card(Suit.Spades, FaceValue.Ace, true);
                    else if (last == true)
                        ret = GetHighestCardOfSuit(playState, playState.Lead.CardSuit);
                    else
                        ret = GetLowestCardOfSuit(playState.Lead.CardSuit);
                }
                else
                {
                    if (last == true)
                        ret = GetHighestCardOfSuit(playState, playState.Lead.CardSuit);
                    else
                        ret = GetLowestCardOfSuit(playState.Lead.CardSuit);
                }
            }

            if (ret.CardSuit != playState.Lead.CardSuit)
                throw new InvalidOperationException("AI - NOT FOLLOWING SUIT");
            return ret;
        }

        private Card GetLowestCardOfSuit(Suit suit)
        {
            Card ret = null;
            foreach (Card c in AIHand.Cards)
            {
                if (c.CardSuit == suit)
                    if (ret == null)
                        ret = c;
                    else if (ret.CardRank > c.CardRank)
                    {
                        if (!(c.CardValue == FaceValue.Queen && c.CardSuit == Suit.Spades))
                            ret = c;
                    }
            }
            return ret;
        }

        private Card GetHighestNonPointCard()
        {
            int removeValue = (int)CardRank.AceHigh;

            if (AIHand.Contains(Suit.Spades, FaceValue.Ace))
                return new Card(Suit.Spades, FaceValue.Ace, true);
            else if (AIHand.Contains(Suit.Spades, FaceValue.King))
                return new Card(Suit.Spades, FaceValue.King, true);

            while (removeValue >= 0)
            {
                for (int i = AIHand.Cards.Count - 1; i >= 0; i--)
                {
                    if (AIHand.Cards[i].CardSuit != Suit.Hearts)
                        if (!(AIHand.Cards[i].CardSuit == Suit.Spades && AIHand.Cards[i].CardValue == FaceValue.Queen))
                            if (AIHand.Cards[i].CardRank >= removeValue)
                                return AIHand.Cards[i];
                }
                removeValue--;
            }
            return null;
        }

        private Card GetLowestNonPointCard()
        {
            Card ret = null;
            int removeValue = (int)CardRank.AceHigh;
            while (removeValue >= 0)
            {
                for (int i = AIHand.Cards.Count - 1; i >= 0; i--)
                {
                    if (AIHand.Cards[i].CardSuit != Suit.Hearts)
                        if (!(AIHand.Cards[i].CardSuit == Suit.Spades && AIHand.Cards[i].CardValue == FaceValue.Queen))
                        {
                            if (AIHand.Cards[i].CardRank <= removeValue)
                                ret = AIHand.Cards[i];
                        }
                }
                removeValue--;
            }
            return ret;
        }

        private Card GetHighestHeartOrOtherCard()
        {
            int removeValue = (int)CardRank.AceHigh;
            while (removeValue >= 0)
            {
                for (int i = AIHand.Cards.Count - 1; i >= 0; i--)
                {
                    if (AIHand.Cards[i].CardSuit == Suit.Hearts)
                        return AIHand.Cards[i];

                    if (AIHand.Cards[i].CardRank >= removeValue)
                    {
                        return AIHand.Cards[i];
                    }
                }
                removeValue--;
            }
            removeValue = (int)CardRank.AceHigh;
            while (removeValue >= 0)
            {
                for (int i = AIHand.Cards.Count - 1; i >= 0; i--)
                {
                    if (AIHand.Cards[i].CardRank >= removeValue)
                    {
                        return AIHand.Cards[i];
                    }
                }
                removeValue--;
            }
            return null;
        }

        private Card GetLowestCard()
        {
            Card ret = null;
            Random rand = new Random();

            foreach (Card c in AIHand.Cards)
            {
                if (ret == null)
                    ret = c;

                if (c.CardValue == ret.CardValue)
                {
                    if (IsPreferrederedCard(c.CardSuit) || ret.CardRank > (int)CardRank.Eight)
                        if (rand.Next(0, 2) == 1)
                            ret = c;
                }
                else if (c.CardRank < ret.CardRank || (ret.CardSuit == Suit.Spades && ret.CardValue == FaceValue.Queen))
                    if (IsPreferrederedCard(c.CardSuit) || ret.CardRank > (int)CardRank.Eight)
                        ret = c;
            }
            return ret;
        }

        private Card GetHighestSpadeBelowQueen(PlayState playState)
        {
            Card ret = null;
            foreach (Card c in AIHand.Cards)
            {
                if (c.CardSuit == Suit.Spades)
                    if (c.CardRank < (int)CardRank.Queen)
                        ret = c;
            }
            if (ret == null)
            {
                if (playState.IsBroken)
                    ret = GetLowestCard();
                else
                    ret = GetLowestNonPointCard();
            }
            return ret;
        }

        private Card GetHighestCardOfSuit(PlayState playState, Suit suit)
        {
            Card ret = null;

            if ((playState.Trick.Contains(new Card(Suit.Spades, FaceValue.Ace, true))
                || playState.Trick.Contains(new Card(Suit.Spades, FaceValue.King, true)))
                && AIHand.Contains(Suit.Spades, FaceValue.Queen) && playState.Lead.CardSuit == Suit.Spades)
            {
                return new Card(Suit.Spades, FaceValue.Queen, true);
            }

            foreach (Card c in AIHand.Cards)
            {
                if (c.CardSuit == suit)
                    if (ret == null)
                        ret = c;
                    else if (ret.CardRank < c.CardRank)
                    {
                        if (!(c.CardValue == FaceValue.Queen && c.CardSuit == Suit.Spades))
                            ret = c;
                    }
            }
            return ret;
        }

        /* AI Hand Check Methods */
        private bool HasSuit(Suit suit)
        {
            foreach (Card c in AIHand.Cards)
            {
                if (c.CardSuit == suit)
                    return true;
            }
            return false;
        }

        internal bool HasTwoOfClubs()
        {
            if (AIHand.Contains(Suit.Clubs, FaceValue.Two))
                return true;
            return false;
        }

        private int CheckShortSuited()
        {
            int s = 0, c = 0, d = 0, h = 0;
            for (int i = 0; i < AIHand.Cards.Count; i++)
            {
                switch (AIHand.Cards[i].CardSuit)
                {
                    case Suit.Clubs:
                        c++;
                        break;
                    case Suit.Diamonds:
                        d++;
                        break;
                    case Suit.Hearts:
                        h++;
                        break;
                    case Suit.Spades:
                        s++;
                        break;
                }
            }
            if (s <= 3)
                return (int)Suit.Spades;
            if (c <= 3)
                return (int)Suit.Clubs;
            if (h <= 3)
                return (int)Suit.Hearts;
            if (d <= 3)
                return (int)Suit.Diamonds;

            return -1;
        }

        private static bool PointInTrick(PlayState playState)
        {
            foreach (Card c in playState.Trick)
            {
                if (c.CardSuit == Suit.Hearts)
                    return true;
                if (c.CardSuit == Suit.Spades && c.CardValue == FaceValue.Queen)
                    return true;
            }
            return false;
        }

        private bool IsPreferrederedCard(Suit suit)
        {
            if (suit == Suit.Spades && preferredSpades == true)
                return true;

            if (suit == Suit.Clubs && preferredClubs == true)
                return true;

            if (suit == Suit.Diamonds && preferredDiamonds == true)
                return true;

            if (suit == Suit.Hearts && preferredHearts == true)
                return true;

            return false;
        }
    }
}