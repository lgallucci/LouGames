using System;
using System.Collections.Generic;
using System.Text;

namespace Bejeweled
{
    internal class GemCount
    {
        private int[] gemCounts;
        private int totalCount;

        internal GemCount(int numOfGems)
        {
            gemCounts = new int[numOfGems];
        }

        internal int this[int position]
        {
            get { return gemCounts[position]; }
            set 
            {
                totalCount -= gemCounts[position];
                gemCounts[position] = value;
                totalCount += value;
            }
        }

        internal void Clear()
        {
            for (int i = 0; i < gemCounts.Length; i++)
            {
                gemCounts[i] = 0;
            }
        }
    }

    class Board
    {
        Random rand = new Random();
        internal Gem[,] data;
        internal int sel_x;
        internal int sel_y;
        internal int hint_x;
        internal int hint_y;
        internal bool clicked;
        internal int numOfGems;
        internal GemCount gemCount;

        private bool isEmpty;

        internal bool IsEmpty
        {
            get { return isEmpty; }
            set { isEmpty = value; }
        }

        /* Constructor */
        internal Board(int gems)
        {
            data = new Gem[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    data[i, j] = new Gem();

            isEmpty = true;
            numOfGems = gems+1;
            gemCount = new GemCount(numOfGems);
        }

        /* Board Creating / Emptying Functions */
        internal void StartSparkles(int x, int y)
        {
            gemCount[data[x, y].Type]--;
            data[x, y].Type = 8;
        }

        internal void EndSparkles(int x, int y)
        {
            data[x, y].Type = 9;
        }

        internal void EmptySquare(int x, int y)
        {
            data[x, y].Type = 0;
        }

        internal void CreateBoard()
        {
            sel_x = 0;
            sel_y = 0;
            hint_x = 0;
            hint_y = 0;
            gemCount.Clear();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    int r_num = rand.Next(1, numOfGems);

                    if (i > 1 && j > 1)
                        while ((r_num == data[i, j - 1].Type && r_num == data[i, j - 2].Type) ||
                            (r_num == data[i - 1, j].Type && r_num == data[i - 2, j].Type))
                            r_num = rand.Next(1, numOfGems);
                    else if (i > 1)
                        while ((r_num == data[i - 1, j].Type && r_num == data[i - 2, j].Type))
                            r_num = rand.Next(1, numOfGems);
                    else if (j > 1)
                        while ((r_num == data[i, j - 1].Type && r_num == data[i, j - 2].Type))
                            r_num = rand.Next(1, numOfGems);

                    data[i, j].Type = r_num;
                    data[i, j].Position.X = 235 + (i * 75);
                    data[i, j].Position.Y = j * 75 - 600;
                    gemCount[r_num]++;
                }

            isEmpty = false;
        }

        private int GetNextRandomGem(int numOfGems)
        {
            
            for (int i = 1; i < numOfGems; i++)
            {
                if (gemCount[i] <= 4)
                {
                    if (rand.Next(1, 100) < 25)
                        return i;
                }
                else if (gemCount[i] <= 6)
                {
                    if (rand.Next(1, 100) < 15)
                        return i;
                }
            }

            return rand.Next(1, numOfGems);
        }

        /* Gem Moves Functions */
        private bool GemsAreEqual(int x, int y, int type)
        {
            if (x < 0 || x >= 8 || y < 0 || y >= 8)
                return false;

            if (data[x, y].Type == type)
                return true;
            else
                return false;
        }

        internal bool CheckMoves(int x, int y)
        {
            if ((GemsAreEqual(x - 2, y, data[x, y].Type) &&
                 GemsAreEqual(x - 1, y, data[x, y].Type))
                ||
                (GemsAreEqual(x - 1, y, data[x, y].Type) &&
                 GemsAreEqual(x + 1, y, data[x, y].Type))
                ||
                (GemsAreEqual(x + 1, y, data[x, y].Type) &&
                 GemsAreEqual(x + 2, y, data[x, y].Type))
                ||
                (GemsAreEqual(x, y - 2, data[x, y].Type) &&
                 GemsAreEqual(x, y - 1, data[x, y].Type))
                ||
                (GemsAreEqual(x, y - 1, data[x, y].Type) &&
                 GemsAreEqual(x, y + 1, data[x, y].Type))
                ||
                (GemsAreEqual(x, y + 1, data[x, y].Type) &&
                 GemsAreEqual(x, y + 2, data[x, y].Type)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool FindMoves()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    if (y >= 1)
                    {
                        SwapPieces(x, y, x, y - 1);
                        if (CheckMoves(x, y))
                        {
                            SwapPieces(x, y, x, y - 1);
                            hint_x = x;
                            hint_y = y;
                            return true;
                        }
                        SwapPieces(x, y, x, y - 1);
                    }

                    if (y < 8 - 1)
                    {
                        SwapPieces(x, y, x, y + 1);
                        if (CheckMoves(x, y))
                        {
                            SwapPieces(x, y, x, y + 1);
                            hint_x = x;
                            hint_y = y;
                            return true;
                        }
                        SwapPieces(x, y, x, y + 1);
                    }

                    if (x >= 1)
                    {
                        SwapPieces(x, y, x - 1, y);
                        if (CheckMoves(x, y))
                        {
                            SwapPieces(x, y, x - 1, y);
                            hint_x = x;
                            hint_y = y;
                            return true;
                        }
                        SwapPieces(x, y, x - 1, y);
                    }

                    if (x < 8 - 1)
                    {
                        SwapPieces(x, y, x + 1, y);
                        if (CheckMoves(x, y))
                        {
                            SwapPieces(x, y, x + 1, y);
                            hint_x = x;
                            hint_y = y;
                            return true;
                        }
                        SwapPieces(x, y, x + 1, y);
                    }
                }
            return false;
        }

        /* Board Changing Functions */
        internal void SwapPieces(int x, int y, int x2, int y2)
        {
            int temp1 = data[x2, y2].Type;

            int[] temp2 = new int[2];
            temp2[0] = data[x2, y2].Position.X;
            temp2[1] = data[x2, y2].Position.Y;
            data[x2, y2].Type = data[x, y].Type;
            data[x2, y2].Position.X = data[x, y].Position.X;
            data[x2, y2].Position.Y = data[x, y].Position.Y;
            data[x, y].Position.X = temp2[0];
            data[x, y].Position.Y = temp2[1];
            data[x, y].Type = temp1;
        }

        internal void ApplyGravity()
        {
            for (int i = 0; i < 7; i++)
                for (int y = 0; y < 7; y++)
                    for (int x = 0; x < 8; x++)
                        if (data[x, y + 1].Type == 0)
                        {
                            if (data[x, y].Type == 0)
                            {
                                int rnum = GetNextRandomGem(numOfGems); 
                                data[x, y + 1].Type = rnum;
                                data[x, y].Type = 0;
                                data[x, y + 1].Position.Y -= 75;
                                gemCount[rnum]++;
                            }
                            else
                            {
                                data[x, y + 1].Type = data[x, y].Type;
                                data[x, y].Type = 0;
                                data[x, y + 1].Position.Y = data[x, y].Position.Y;
                            }
                        }



            for (int x = 0; x < 8; x++)
                if (data[x, 0].Type == 0)
                {
                    int rnum = GetNextRandomGem(numOfGems); 
                    data[x, 0].Type = rnum;
                    data[x, 0].Position.Y = data[x, 1].Position.Y - 75;
                    gemCount[rnum]++;
                }

        }
    }
}
