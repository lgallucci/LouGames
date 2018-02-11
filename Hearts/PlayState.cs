using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CardGame;

namespace Hearts
{
    internal class PlayState
    {
        /* Properties */
        private List<Card> trick;
        internal ReadOnlyCollection<Card> Trick
        {
            get { return trick.AsReadOnly(); }
        }

        private Card lead;
        internal Card Lead
        {
            get { return lead; }
            set { lead = value; }
        }

        private bool isBroken;
        internal bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        private bool queenHasBeenPlayed;
        internal bool QueenHasBeenPlayed
        {
            get { return queenHasBeenPlayed; }
            set { queenHasBeenPlayed = value; }
        }

        private int leader;
        internal int Leader
        {
            get { return leader; }
            set { leader = value; }
        }
	
        private int turn;
        internal int Turn
        {
            get { return turn; }
            set { turn = value; }
        }
	
        private int passingMode;
        internal int PassingMode
        {
            get { return passingMode; }
            set { passingMode = value; }
        }

        private int clicked;
        internal int Clicked
        {
            get { return clicked; }
            set { clicked = value; }
        }

        private string userName;
        internal string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private List<int>[] cardsPassing;

        /* Constructor */
        internal PlayState()
        {
            trick = new List<Card>();
            clicked = -1;

            cardsPassing = new List<int>[3];

            for (int i = 0; i < 3; i++)
                cardsPassing[i] = new List<int>();

        }

        /* PlayState Editting Methods */
        internal void Reset()
        {
            trick = new List<Card>();
            clicked = -1;

            cardsPassing = new List<int>[3];

            for (int i = 0; i < 3; i++)
                cardsPassing[i] = new List<int>();

            passingMode = 0;
            turn = 0;
            isBroken = false;
            queenHasBeenPlayed = false;
            leader = 0;
        }
        
        internal void SetPassing(int index, ReadOnlyCollection<int> passing)
        {
                cardsPassing[index] = new List<int>(passing);
        }

        internal void AddTrick(Card card)
        {
            trick.Add(card);
        }

        internal void ClearTrick()
        {
            trick.Clear();
        }

        internal bool CardsPassingContains(int index, int number)
        {
            foreach (int c in cardsPassing[index])
            {
                if (c == number)
                    return true;
            }
            return false;
        }

    }
}
