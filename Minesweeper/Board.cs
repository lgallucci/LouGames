using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper
{

    class Board
    {
        internal Square[,] data;
        internal int clickedX = -1, clickedY = -1;
        internal int squareSize;
        internal bool doubleClicked;

        private bool isEmpty;
        private int sizeX, sizeY;

        internal bool IsEmpty
        {
            get { return isEmpty; }
            set { isEmpty = value; }
        }
        internal int SizeX
        {
            get { return sizeX; }
        }
        internal int SizeY
        {
            get { return sizeY; }
        }

        /* Properties */
        private int[] errorFlag;

        internal int[] ErrorFlag
        {
            get { return errorFlag; }
        }

        private int numberOfMines;

        internal int NumberOfMines
        {
            get { return numberOfMines; }
        }

        /* Constructor */
        internal Board(int width, int height, int mines, int size)
        {
            sizeX = width;
            sizeY = height;
            numberOfMines = mines;
            squareSize = size;

            if (sizeX < 9)
            {
                sizeX = 9;
                sizeY = 9;
                numberOfMines = 10;
            }

            isEmpty = true;
            data = new Square[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    data[i, j] = new Square();

            errorFlag = new int[2];
            errorFlag[0] = -1; errorFlag[1] = -1;
        }

        /* Board Operation Methods */
        internal void CreateBoard(int width, int height)
        {
            isEmpty = false;

            //data = new Square[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    data[i, j].Value = 0;

            Random rand = new Random();

            int rx = 0, ry = 0;

            for (int i = 0; i < numberOfMines; i++)
            {
                rx = rand.Next(0, sizeX);
                ry = rand.Next(0, sizeY);

                while ((rx == width && ry == height) || data[rx, ry].Value == -1)
                {
                    rx = rand.Next(0, sizeX);
                    ry = rand.Next(0, sizeY);
                }
                data[rx, ry].Value = -1;
            }

            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                {
                    if (data[i, j].Value != -1)
                    {
                        data[i, j].Value = GetNumberOfAdjoiningBombs(i, j);
                    }
                }
        }

        private int GetNumberOfAdjoiningBombs(int width, int height)
        {
            int ret = 0;
            if (height > 0)
            {
                if (width > 0)
                    if (data[width - 1, height - 1].Value == -1)
                        ret++;
                if (data[width, height - 1].Value == -1)
                    ret++;
                if (width < sizeX - 1)
                    if (data[width + 1, height - 1].Value == -1)
                        ret++;
            }
            if (width > 0)
                if (data[width - 1, height].Value == -1)
                    ret++;
            if (width < sizeX - 1)
                if (data[width + 1, height].Value == -1)
                    ret++;

            if (height < sizeY - 1)
            {
                if (width > 0)
                    if (data[width - 1, height + 1].Value == -1)
                        ret++;
                if (data[width, height + 1].Value == -1)
                    ret++;
                if (width < sizeX - 1)
                    if (data[width + 1, height + 1].Value == -1)
                        ret++;
            }

            return ret;
        }

        internal int GetNumberOfAdjoiningFlags(int width, int height)
        {
            int ret = 0;
            if (height > 0)
            {
                if (width > 0)
                    if (data[width - 1, height - 1].displayInfo == DisplayInfo.BombFlag)
                        ret++;
                if (data[width, height - 1].displayInfo == DisplayInfo.BombFlag)
                    ret++;
                if (width < sizeX - 1)
                    if (data[width + 1, height - 1].displayInfo == DisplayInfo.BombFlag)
                        ret++;
            }
            if (width > 0)
                if (data[width - 1, height].displayInfo == DisplayInfo.BombFlag)
                    ret++;
            if (width < sizeX - 1)
                if (data[width + 1, height].displayInfo == DisplayInfo.BombFlag)
                    ret++;

            if (height < sizeY - 1)
            {
                if (width > 0)
                    if (data[width - 1, height + 1].displayInfo == DisplayInfo.BombFlag)
                        ret++;
                if (data[width, height + 1].displayInfo == DisplayInfo.BombFlag)
                    ret++;
                if (width < sizeX - 1)
                    if (data[width + 1, height + 1].displayInfo == DisplayInfo.BombFlag)
                        ret++;
            }

            return ret;
        }

    }
}
