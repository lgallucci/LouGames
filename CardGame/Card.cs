using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public enum CardColor
    {
        Red,
        Black
    }

    public enum Suit
    {
        Clubs,
        Diamonds,
        Spades,
        Hearts
    }

    public enum FaceValue
    {
        None = 0,
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    public enum CardRank
    {
        None = 0,
        AceLow = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        AceHigh
    }

    public class Card
    {
        /* Properties */
        private FaceValue cardValue;

        public FaceValue CardValue
        {
            get { return cardValue; }
        }

        private int cardRank;

        public int CardRank
        {
            get { return cardRank; }
        }

        private Suit cardSuit;

        public Suit CardSuit
        {
            get { return cardSuit; }
        }

        public CardColor CardColor
        {
            get
            {
                if (cardSuit == Suit.Clubs || cardSuit == Suit.Spades)
                    return CardColor.Black;
                else
                    return CardColor.Red;
            }
        }

        /* Constructors */
        public Card(Suit suit, FaceValue faceValue, bool aceHigh)
        {
            cardValue = faceValue;
            cardSuit = suit;
            if (aceHigh && faceValue == FaceValue.Ace)
            {
                cardRank = (int)CardGame.CardRank.AceHigh;
            }
            else
            {
                cardRank = (int)faceValue;
            }
        }

        /* Equality Operator Methods */
        public static bool operator ==(Card obj1, Card obj2)
        {
            bool isEqual;
            if (object.ReferenceEquals(obj1, null))
            {
                if (object.ReferenceEquals(obj2, null))
                {
                    isEqual = true;
                }
                else
                {
                    isEqual = false;
                }
            }
            else
            {
                isEqual = obj1.Equals(obj2);
            }
            return isEqual;
        }

        public static bool operator !=(Card obj1, Card obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object obj)
        {
            Card a = obj as Card;
            if ((object)a == null)
            {
                return false;
            }

            return cardSuit == a.CardSuit && a.CardValue == cardValue;
        }

        public override int GetHashCode()
        {
            return ((int)cardSuit * (int)cardValue) ^ 2;
        }
    }
}
