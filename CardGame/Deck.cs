using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public class Deck
    {
        private List<Card> cards = new List<Card>();
        public Card this[int position] { get { return cards[position]; } }

        /* Constructor */
        public Deck(bool aceHigh)
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                foreach (FaceValue faceValue in Enum.GetValues(typeof(FaceValue)))
                    if (faceValue != FaceValue.None)
                        cards.Add(new Card(suit, faceValue, aceHigh));
        }

        /* Deck Shuffle Methods */
        public void Shuffle()
        {
            Random random = new Random();
            for (int i = 0; i < cards.Count; i++)
            {
                int index1 = i;
                int index2 = random.Next(cards.Count);
                SwapCard(index1, index2);
            }
        }

        private void SwapCard(int index1, int index2)
        {
            Card card = cards[index1];
            cards[index1] = cards[index2];
            cards[index2] = card;
        }
    }
}
