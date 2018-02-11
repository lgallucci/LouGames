using System;
using System.Collections.Generic;
using System.Text;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Windows;
using SdlDotNet.Particles;
using SdlDotNet.Particles.Emitters;
using SdlDotNet.Particles.Manipulators;
using System.Drawing;
using System.Windows.Forms;
using CardGame;

namespace FreeCell
{
    internal class GraphicsEngine : IDisposable
    {
        internal const int TopOfPiles = 150;
        internal const int LeftOfCells = 10;
        internal const int LeftOfFoundation = 375;
        internal const int TopOfCells = 10;
        internal const int CardWidth = 67;
        internal const int CardHeight = 105;
        internal const int PileSpacing = 15;
        internal const int CardSpacing = 20;
        internal const int LeftOfPiles = 15;

        private Surface surf;
        private Surface cellBackground;
        private Surface Font;
        private Surface FontShadow;
        private Surface KingLeft;
        private Surface KingRight;
        private Surface KingSmile;
        private CardDrawer drawer;
        private ParticleSystem particles;

        private int gameOver;
        internal int GameOver
        {
            get { return gameOver; }
            set { gameOver = value; }
        }

        private int kingState;
        internal int KingState
        {
            set { kingState = value; }
        }

        private SdlDotNet.Graphics.Font font;

        /* Constructor */
        internal GraphicsEngine(SurfaceControl surfaceControl)
        {
            surf = Video.SetVideoMode(surfaceControl.Width, surfaceControl.Height, 32, false, false, false, false, false);
            LoadImages();
            drawer = new CardDrawer();

            particles = new ParticleSystem();
            particles.Manipulators.Add(new ParticleBoundary(surf.Size));
        }

        private void LoadImages()
        {
            cellBackground = new Surface(Properties.Resources.CellBackground);
            KingLeft = new Surface(Properties.Resources.KingLeft);
            KingRight = new Surface(Properties.Resources.KingRight);
            KingSmile = new Surface(Properties.Resources.KingSmile);

            font = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\arial.ttf", 72);
        }

        /* Various Drawing Functions */
        internal void DrawScreen(SurfaceControl surfaceControl, Tableau tableau)
        {
            surf.Fill(Color.Green);

            DrawCards(tableau);

            if (GameOver == -1)
            {
                Font = font.Render("You Lose !", Color.Red);
                FontShadow = font.Render("You Lose !", Color.DarkRed);
                surf.Blit(FontShadow, new Point((surf.Width - Font.Width) / 2 + 2, (surf.Height - Font.Height) / 2 + 2));
                surf.Blit(Font, new Point((surf.Width - Font.Width) / 2, (surf.Height - Font.Height) / 2));
            }
            else if (GameOver == 1)
            {
                Font = font.Render("You Win !", Color.Red);
                FontShadow = font.Render("You Win !", Color.DarkRed);
                surf.Blit(FontShadow, new Point((surf.Width - Font.Width) / 2 + 2, (surf.Height - Font.Height) / 2 + 2));
                surf.Blit(Font, new Point((surf.Width - Font.Width) / 2, (surf.Height - Font.Height) / 2));  
            }
            if (tableau.FireworksLeft >= 0)
            {
                particles.Update();
                particles.Render(surf);
            }
            surfaceControl.Blit(surf);
        }

        internal void ExplodeFirework(Tableau tableau)
        {
            ParticleCircleEmitter explosion = new ParticleCircleEmitter(particles, tableau.FireworkColorMin[tableau.FireworksLeft], tableau.FireworkColorMax[tableau.FireworksLeft], 1, 3);
            explosion.X = tableau.FireworkPosX[tableau.FireworksLeft]; // location
            explosion.Y = tableau.FireworkPosY[tableau.FireworksLeft];
            explosion.Life = 20; // life of the explosion
            explosion.Frequency = 100000;
            explosion.LifeMin = 10;
            explosion.LifeMax = 60;
            explosion.LifeFullMin = 10;
            explosion.LifeFullMax = 60;
            explosion.SpeedMin = 2;
            explosion.SpeedMax = 10;
        }

        private void DrawCards(Tableau tableau)
        {
            DrawBackground();

            DrawCells(tableau);

            DrawFoundation(tableau);

            DrawPiles(tableau);
        }

        private void DrawBackground()
        {
            for (int i = 0; i < 4; i++)
            {
                surf.Blit(cellBackground, new Point(LeftOfCells + (i * 71), TopOfCells));
                surf.Blit(cellBackground, new Point(LeftOfFoundation + (i * 71), TopOfCells));
            }
            int positionX = (LeftOfFoundation - cellBackground.Width - 4);
            int positionY = (cellBackground.Height - KingSmile.Height) / 2 + TopOfCells;
            switch (kingState)
            {
                case -1:
                    surf.Blit(KingSmile, new Point(positionX, positionY));
                    break;
                case 0:
                    surf.Blit(KingLeft, new Point(positionX, positionY));
                    break;
                case 1:
                    surf.Blit(KingRight, new Point(positionX, positionY));
                    break;
            }
        }

        private void DrawCells(Tableau tableau)
        {
            for (int i = 0; i < 4; i++)
            {
                if (tableau.Cells[i] != null)
                {
                    if (tableau.Clicked[0] == 8 && tableau.Clicked[1] == i)
                    {

                        surf.Blit(drawer.DrawCardInverted(tableau.Cells[i]), 
                            new Point(LeftOfCells + (i * (CardWidth + 4)) + 2, TopOfCells + 2));
                    }
                    else
                    {

                        surf.Blit(drawer.DrawCardFaceUp(tableau.Cells[i]), 
                            new Point(LeftOfCells + (i * (CardWidth + 4)) + 2, TopOfCells + 2));
                    }
                }
            }
        }

        private void DrawFoundation(Tableau tableau)
        {
            for (int i = 0; i < 4; i++)
            {
                if (tableau.Foundation[i] != null)
                {
                    surf.Blit(drawer.DrawCardFaceUp(tableau.Foundation[i]), 
                        new Point(LeftOfFoundation + (i * (CardWidth + 4)) + 2, TopOfCells + 2));
                }
            }
        }

        private void DrawPiles(Tableau tableau)
        {
            for (int i = 0; i < 8; i++)
            {
                int spacing = CardSpacing;
                if (tableau.Piles[i].Count > 10)
                {
                    spacing -= (int)((tableau.Piles[i].Count - 10) * 1.5);
                }
                for (int j = 0; j < tableau.Piles[i].Count; j++)
                {
                    if (tableau.Clicked[0] == i && tableau.Clicked[1] <= j)
                    {
                        surf.Blit(drawer.DrawCardInverted(tableau.Piles[i][j]), 
                            new Point(LeftOfPiles + (i * (CardWidth + PileSpacing)), TopOfPiles + (j * spacing)));
                    }
                    else
                    {
                        surf.Blit(drawer.DrawCardFaceUp(tableau.Piles[i][j]), 
                            new Point(LeftOfPiles + (i * (CardWidth + PileSpacing)), TopOfPiles + (j * spacing)));
                    }
                }

                if (tableau.TopCard[0] == i)
                {
                    surf.Blit(drawer.DrawCardFaceUp(tableau.Piles[tableau.TopCard[0]][tableau.TopCard[1]]),
                            new Point(LeftOfPiles + (tableau.TopCard[0] * (CardWidth + PileSpacing)), TopOfPiles + (tableau.TopCard[1] * spacing)));
                }
            }
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
                if (surf != null)
                {
                    surf.Dispose();
                    surf = null;
                }
                if (cellBackground != null)
                {
                    cellBackground.Dispose();
                    cellBackground = null;
                }
                if (font != null)
                {
                    font.Dispose();
                    font = null;
                }
                if (Font != null)
                {
                    Font.Dispose();
                    Font = null;
                }
                if (KingLeft != null)
                {
                    KingLeft.Dispose();
                    KingLeft = null;
                }
                if (KingRight != null)
                {
                    KingRight.Dispose();
                    KingRight = null;
                }
                if (KingSmile != null)
                {
                    KingSmile.Dispose();
                    KingSmile = null;
                }
                if (drawer != null)
                {
                    drawer.Dispose();
                    drawer = null;
                }
            }
        }

        #endregion
    }
}