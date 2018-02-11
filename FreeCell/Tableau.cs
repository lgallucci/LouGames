using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using CardGame;

namespace FreeCell
{
    internal class Tableau
    {
        private Card[] cells;
        private Card[] foundation;
        private List<Card>[] piles;
        private int cardsMovable;
        private int emptyPiles;
        private int[] clicked;
        private int maxFoundationMoveRed;
        private int maxFoundationMoveBlack;
        private int[] topCard;

        private int fireworksLeft;
        private int[] fireworkPosX;
        private int[] fireworkPosY;
        private Color[] fireworkColorMin;
        private Color[] fireworkColorMax;

        /* Properties */
        internal Card[] Cells
        {
            get { return cells; }
        }

        internal Card[] Foundation
        {
            get { return foundation; }
        }

        internal List<Card>[] Piles
        {
            get { return piles; }
        }

        internal int CardsMovable
        {
            get { return cardsMovable; }
            set { cardsMovable = value; }
        }

        internal int EmptyPiles
        {
            get { return emptyPiles; }
            set { emptyPiles = value; }
        }

        internal int[] Clicked
        {
            get { return clicked; }
        }

        internal int[] TopCard
        {
            get { return topCard; }
        }

        internal int MaxFoundationMoveRed
        {
            get { return maxFoundationMoveRed; }
        }

        internal int MaxFoundationMoveBlack
        {
            get { return maxFoundationMoveBlack; }
        }

        internal int FireworksLeft
        {
            get { return fireworksLeft; }
            set { fireworksLeft = value; }
        }

        internal Color[] FireworkColorMin
        {
            get { return fireworkColorMin; }
        }

        internal Color[] FireworkColorMax
        {
            get { return fireworkColorMax; }
        }

        internal int[] FireworkPosX
        {
            get { return fireworkPosX; }
        }
        internal int[] FireworkPosY
        {
            get { return fireworkPosY; }
        }

        /* Constructor */
        internal Tableau()
        {
            clicked = new int[2];
            clicked[0] = -1; clicked[1] = -1;
            topCard = new int[2];
            topCard[0] = -1; topCard[1] = -1;
            foundation = new Card[4];
            cells = new Card[4];
            piles = new List<Card>[8];

            for (int i = 0; i < 8; i++)
            {
                piles[i] = new List<Card>();
            }

            cardsMovable = 5;

            maxFoundationMoveRed = (int)CardRank.Two;
            maxFoundationMoveBlack = (int)CardRank.Two;

            fireworksLeft = -1;
        }

        /* Card Table Action Methods */
        internal void SetFireworks(int num)
        {
            Random rand = new Random();

            fireworksLeft = num;
            fireworkPosX = new int[num];
            fireworkPosY = new int[num];
            fireworkColorMin = new Color[num];
            fireworkColorMax = new Color[num];

            for (int i = 0; i < num; i++)
            {
                fireworkPosX[i] = rand.Next(80, 600);
                fireworkPosY[i] = rand.Next(100, 400);
            }
            SetFireworkColors();
        }

        private void SetFireworkColors()
        {
            Random rand = new Random();
            for (int i = 0; i < fireworksLeft; i++)
            {
                switch (rand.Next(1, 6))
                {
                    case 1:
                        fireworkColorMin[i] = Color.FromArgb(Color.SkyBlue.R - 50, Color.SkyBlue.G, Color.SkyBlue.B - 50);
                        fireworkColorMax[i] = Color.SkyBlue;
                        break;
                    case 2:
                        fireworkColorMin[i] = Color.FromArgb(0, 0, 200);
                        fireworkColorMax[i] = Color.Blue;
                        break;
                    case 3:
                        fireworkColorMin[i] = Color.Yellow;
                        fireworkColorMax[i] = Color.FromArgb(255, 255, Color.Yellow.B + 50);
                        break;
                    case 4:
                        fireworkColorMin[i] = Color.FromArgb(Color.White.R - 50, Color.White.G - 50, Color.White.B - 50);
                        fireworkColorMax[i] = Color.White;
                        break;
                    case 5:
                        fireworkColorMin[i] = Color.FromArgb(200, Color.Orange.G - 50, 0);
                        fireworkColorMax[i] = Color.Orange;
                        break;
                    default:
                        fireworkColorMin[i] = Color.FromArgb(Color.White.R - 50, Color.White.G - 50, Color.White.B - 50);
                        fireworkColorMax[i] = Color.White;
                        break;
                }
            }
        }

        internal void AddToPile(int pileNumber, IList<Card> cards)
        {
            foreach (Card c in cards)
            {
                piles[pileNumber].Add(c);
            }
        }

        internal int CanMoveCard(int pileNumber, int cardIndex, int newPile)
        {
            if (pileNumber == 8 || piles[pileNumber].Count == 0)
                return 0;
            int empty = emptyPiles;
            if (piles[newPile].Count == 0)
                empty--;
            if ((piles[pileNumber].Count - cardIndex) > cardsMovable + empty)
                return cardsMovable + emptyPiles;
            else
            {
                for (int i = 0; i < piles[pileNumber].Count; i++)
                {
                    if (i > cardIndex && (piles[pileNumber][i].CardColor == piles[pileNumber][i - 1].CardColor
                        || (piles[pileNumber][i].CardRank != piles[pileNumber][i - 1].CardRank - 1)))
                        return -1;
                }
            }

            return 0;
        }

        internal IList<Card> RemoveFromPile(int pileNumber, int index)
        {

            List<Card> ret = new List<Card>();
            for (int i = 0; i < piles[pileNumber].Count; i++)
            {
                if (i >= index)
                {
                    ret.Add(piles[pileNumber][i]);
                }
            }
            foreach (Card c in ret)
            {
                piles[pileNumber].Remove(c);
                if (piles[pileNumber].Count == 0)
                    emptyPiles++;
            }
            return ret;
        }

        internal bool AddToFoundation(Card card, int cell)
        {
            if (foundation[cell] == null)
            {
                if (card.CardValue == FaceValue.Ace)
                {
                    foundation[cell] = card;
                    return true;
                }
            }
            else
            {
                if (foundation[cell].CardSuit == card.CardSuit)
                {
                    if ((foundation[cell].CardValue == FaceValue.Ace && card.CardValue == FaceValue.Two)
                        || (foundation[cell].CardRank == card.CardRank - 1))
                    {
                        foundation[cell] = card;
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool TryToAddToFoundation(Card card)
        {
            if (card == null)
                return false;

            for (int i = 0; i < 4; i++)
            {
                if (foundation[i] == null)
                {
                    if (card.CardValue == FaceValue.Ace)
                    {
                        foundation[i] = card;
                        SetMaxFoundationMove();
                        return true;
                    }
                }
                else
                {
                    if (foundation[i].CardSuit == card.CardSuit)
                    {
                        if ((foundation[i].CardValue == FaceValue.Ace && card.CardValue == FaceValue.Two)
                            || (foundation[i].CardRank == card.CardRank - 1))
                        {
                            foundation[i] = card;
                            SetMaxFoundationMove();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal bool AddToCell(Card card, int cell)
        {
            if (cells[cell] == null)
            {
                cells[cell] = card;
                CardsMovable--;
                return true;
            }
            return false;
        }

        internal Card RemoveFromCell(int cell)
        {
            if (cells[cell] != null)
            {
                Card ret = cells[cell];
                cells[cell] = null;
                CardsMovable++;
                return ret;
            }
            return null;
        }

        internal bool TryToMoveToList(Card card)
        {
            if (card == null)
                return false;

            for (int i = 0; i < 8; i++)
            {
                if (piles[i].Count == 0)
                    continue;

                if (piles[i][piles[i].Count - 1].CardColor != card.CardColor)
                {
                    if ((piles[i][piles[i].Count - 1].CardValue == FaceValue.Two && card.CardValue == FaceValue.Ace)
                        || (piles[i][piles[i].Count - 1].CardRank == card.CardRank + 1))
                    {
                        piles[i].Add(card);
                        return true;
                    }
                }
            }
            return false;
        }

        private void SetMaxFoundationMove()
        {
            int lowValueRed = (int)CardRank.King;
            int lowValueBlack = (int)CardRank.King;
            foreach (Card c in Foundation)
            {
                if (c == null)
                {
                    maxFoundationMoveRed = (int)CardRank.Two;
                    maxFoundationMoveBlack = (int)CardRank.Two;
                    return;
                }
                else if (c.CardRank < lowValueRed && c.CardColor == CardColor.Red)
                {
                    lowValueRed = c.CardRank;
                }
                else if (c.CardRank < lowValueBlack && c.CardColor == CardColor.Black)
                {
                    lowValueBlack = c.CardRank;
                }
            }
            maxFoundationMoveBlack = lowValueRed + 1;
            maxFoundationMoveRed = lowValueBlack + 1;
        }
    }
}
