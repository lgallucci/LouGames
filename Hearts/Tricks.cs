using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CardGame;

namespace Hearts
{
    internal class Tricks
    {
        private List<Card> cards;

        internal ReadOnlyCollection<Card> Cards
        {
            get { return cards.AsReadOnly(); }
        }
	
        /* Constructor */
        internal Tricks()
        {
            cards = new List<Card>();
        }

        /* List Edit Methods */
        internal void Add(Card trick)
        {
            cards.Add(trick);
        }

        internal void Clear()
        {
            cards.Clear();
        }
    }
}
