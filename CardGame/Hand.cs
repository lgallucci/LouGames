using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace CardGame
{
    public class CardCollection : Collection<Card>
    {
        public CardCollection()
            : base(new List<Card>())
        {
        }

        protected internal void Sort(MyComparer match)
        {
            List<Card> items = (List<Card>)Items;

            items.Sort(match);
        }
    }

    public class MyComparer : IComparer<Card>
    {
        public int Compare(Card x, Card y)
        {
            if (x.CardSuit < y.CardSuit)
                return -1;
            else if (x.CardSuit == y.CardSuit)
                if (x.CardRank < y.CardRank)
                    return -1;
                else if (x.CardRank == y.CardRank)
                    return 0;
                else
                    return 1;
            else
                return 1;
        }
    }

    public class Hand
    {
        private List<Card> cards;

        public ReadOnlyCollection<Card> Cards
        {
            get { return cards.AsReadOnly(); }
        }

        /* Constructor */
        public Hand()
        {
            cards = new List<Card>();
        }

        /* List Manipulation Methods */
        public void Add(Card card)
        {
            cards.Add(card);
        }

        public void Clear()
        {
            cards.Clear();
        }

        public void SetHand(ReadOnlyCollection<Card> hand)
        {
            cards = new List<Card>(hand);
        }

        public void Sort()
        {
            cards.Sort(new MyComparer());
        }

        public void Remove(Card card)
        {
            foreach (Card c in cards)
            {
                if (card == c)
                {
                    cards.Remove(c);
                    return;
                }
            }
        }

        public int IndexOf(Suit suit, FaceValue faceValue)
        {
            int ret = -1;
            for(int i = 0; i < cards.Count; i++)
            {
                if (cards[i].CardSuit == suit && cards[i].CardValue == faceValue)
                    ret = i;
            }
            return ret;
        }

        public Card Play(int position)
        {
            Card ret = cards[position];
            cards.Remove(cards[position]);
            return ret;
        }

        public bool Contains(Suit suit, FaceValue faceValue)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].CardSuit == suit && cards[i].CardValue == faceValue)
                    return true;
            }
            return false;
        }
    }
}
