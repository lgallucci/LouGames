using System;
using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Windows;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace Minesweeper
{
    class GraphicsEngine : IDisposable
    {
        private Surface squareUnclicked;
        private Surface squareBackground;
        private Surface squareBombFlag;
        private Surface squareQuestionFlag;
        private Surface squareClicked;
        private Surface squareBomb;
        private Surface squareBombExplode;
        private Surface squareNotBomb;
        private Surface squareError;
        private Surface surf;
        private SdlDotNet.Graphics.Font numberFont;
        private Surface numbersFont;

        /* Constructor */
        internal GraphicsEngine(SurfaceControl surfaceControl)
        {
            surf = Video.SetVideoMode(surfaceControl.Width, surfaceControl.Height, 32, false, false, false, false, false);
            LoadImages();
        }

        /* Image Loading Method */
        private void LoadImages()
        {
            squareUnclicked = new Surface(Properties.Resources.SquareUnclicked).Convert(surf, false, true);
            squareBackground = new Surface(Properties.Resources.SquareBackground).Convert(surf, false, true);

            squareBombFlag = new Surface(Properties.Resources.SquareBombFlag).Convert(surf, false, true);
            squareBombFlag.Transparent = true;
            squareBombFlag.TransparentColor = Color.FromArgb(255, 000, 255);

            squareBomb = new Surface(Properties.Resources.SquareBomb).Convert(surf, false, true);
            squareBomb.Transparent = true;
            squareBomb.TransparentColor = Color.FromArgb(255, 000, 255);

            squareBombExplode = new Surface(Properties.Resources.SquareBombExplode).Convert(surf, false, true);
            squareBombExplode.Transparent = true;
            squareBombExplode.TransparentColor = Color.FromArgb(255, 000, 255);

            squareQuestionFlag = new Surface(Properties.Resources.SquareQuestionFlag).Convert(surf, false, true);
            squareQuestionFlag.Transparent = true;
            squareQuestionFlag.TransparentColor = Color.FromArgb(255, 000, 255);

            squareNotBomb = new Surface(Properties.Resources.SquareNotBomb).Convert(surf, false, true);
            squareNotBomb.Transparent = true;
            squareNotBomb.TransparentColor = Color.FromArgb(255, 000, 255);

            squareError = new Surface(Properties.Resources.SquareError).Convert(surf, false, true);
            squareError.Transparent = true;
            squareError.TransparentColor = Color.FromArgb(255, 000, 255);

            squareClicked = new Surface(Properties.Resources.SquareClicked).Convert(surf, false, true);

            numberFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\arial.ttf", 20);
        }

        /* Resizing Drawing Surface Method */
        internal void ResizeSurface(Control surfControl)
        {
            surf = Video.CreateRgbSurface(surfControl.Width, surfControl.Height, 32, 0, 0, 0, 0, true);
        }

        /* Drawing Methods */
        internal void DrawScreen(Board board, SurfaceControl surfaceControl)
        {
            surf.Fill(Color.Black);

            DrawGrid(board);

            surfaceControl.Blit(surf);
        }

        private void DrawGrid(Board board)
        {
            for (int x = 0; x < board.SizeX; x++)
                for (int y = 0; y < board.SizeY; y++)
                {
                    if (board.data[x, y].displayInfo == DisplayInfo.SquareUnclicked)
                    {
                        if ((board.clickedX == x && board.clickedY == y) ||
                            (Math.Abs(board.clickedX - x) <= 1 && Math.Abs(board.clickedY - y) <= 1 
                            && board.clickedX != -1 && board.clickedY != -1 && board.doubleClicked
                            && board.data[board.clickedX, board.clickedY].displayInfo == DisplayInfo.SquareClicked))
                        {
                            surf.Blit(squareClicked, new Point(x * 24, y * 24));
                        }
                        else
                        {
                            surf.Blit(squareUnclicked, new Point(x * 24, y * 24));
                        }
                    }
                    else if (board.data[x, y].displayInfo == DisplayInfo.BombFlag)
                    {
                        surf.Blit(squareUnclicked, new Point(x * 24, y * 24));
                        surf.Blit(squareBombFlag, new Point(x * 24, y * 24));
                    }
                    else if (board.data[x, y].displayInfo == DisplayInfo.QuestionFlag)
                    {
                        surf.Blit(squareUnclicked.Convert(surf, false, true), new Point(x * 24, y * 24));
                        surf.Blit(squareQuestionFlag, new Point(x * 24, y * 24));
                    }
                    else if (board.data[x, y].displayInfo == DisplayInfo.BombExplode)
                    {
                        surf.Blit(squareBackground, new Point(x * 24, y * 24));
                        surf.Blit(squareBombExplode, new Point(x * 24, y * 24));
                    }
                    else if (board.data[x, y].displayInfo == DisplayInfo.SquareClicked)
                    {
                        surf.Blit(squareBackground, new Point(x * 24, y * 24));

                        if (board.data[x, y].Value > 0)
                        {
                            numbersFont = numberFont.Render(board.data[x, y].Value.ToString(CultureInfo.CurrentCulture),
                                GetColorFromNumber(board.data[x, y].Value));
                            surf.Blit(numbersFont, new Point(x * 24 + 6, y * 24 + 1));
                        }
                        if (board.data[x, y].Value == -1)
                            surf.Blit(squareBomb, new Point(x * 24, y * 24));
                    }
                    else if (board.data[x, y].displayInfo == DisplayInfo.IncorrectBombFlag)
                    {
                        surf.Blit(squareBackground, new Point(x * 24, y * 24));
                        surf.Blit(squareNotBomb, new Point(x * 24, y * 24));
                    }
                }
            if (board.ErrorFlag[0] != -1 && board.ErrorFlag[1] != -1)
            {
                surf.Blit(squareError, new Point(board.ErrorFlag[0] * 24, board.ErrorFlag[1] * 24));
            }

        }

        private static Color GetColorFromNumber(int num)
        {
            switch (num)
            {
                case 1:
                    return Color.Blue;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Red;
                case 4:
                    return Color.DarkBlue;
                case 5:
                    return Color.DarkOliveGreen;
                case 6:
                    return Color.DarkRed;
                case 7:
                    return Color.Purple;
                case 8:
                    return Color.DarkOrange;
            }
            return Color.Pink;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (numberFont != null)
                {
                    numberFont.Dispose();
                    numberFont = null;
                }
                if (surf != null)
                {
                    surf.Dispose();
                    surf = null;
                }
            }
        }

        #endregion
    }
}
