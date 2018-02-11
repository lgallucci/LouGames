using System;
using System.IO;
using System.Drawing;
using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Windows;
using System.ComponentModel;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using CardGame;

namespace Hearts
{
    internal class GraphicsEngine : IDisposable
    {
        private Surface surf;
        private Surface passButton;
        private Surface passButtonFont;
        private Surface passDot;
        private Surface name1, name2, name3, name4;
        private List<Rectangle> userHand;
        private CardDrawer drawer;
        private SdlDotNet.Graphics.Font buttonFont;
        int passCount;

        internal const int CardWidth = 67;
        internal const int CardHeight = 105;

        /* Constructor */
        internal GraphicsEngine(SurfaceControl surfaceControl)
        {
            surf = Video.CreateRgbSurface(surfaceControl.Width, surfaceControl.Height, 32, 0, 0, 0, 0, true); 
            userHand = new List<Rectangle>();

            LoadImages();

            drawer = new CardDrawer();
        }

        /* Load Images */
        private void LoadImages()
        {
            passDot = new Surface(Properties.Resources.passDot);
            passDot.Transparent = true;
            passDot.TransparentColor = Color.FromArgb(255, 000, 255);

            passButton = new Surface(Properties.Resources.passButton);

            buttonFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\arial.ttf", 14);
        }

        /* Graphics Interection Methods */
        internal int GetCardIntersects(int mouseX, int mouseY)
        {
            for (int i = (userHand.Count - 1); i >= 0; i--)
                if (userHand[i].Left < mouseX && userHand[i].Right > mouseX
                    && userHand[i].Top < mouseY && userHand[i].Bottom > mouseY)
                    return i;
            return -1;
        }

        internal bool GetPassButtonIntersects(int mouseX, int mouseY)
        {
            if (passCount == 3 && 400 < mouseX && 490 > mouseX
                    && 400 < mouseY && 430 > mouseY)
                return true;
            return false;
        }

        internal void ClearHand()
        {
            userHand.Clear();
        }

        internal void UpdateHand(int count, int[] clicked)
        {
            for (int i = 0; i < count; i++)
            {
                if (clicked[0] == i || clicked[1] == i || clicked[2] == i)
                    userHand.Add(new Rectangle(new Point(250 + (i * 20), 452), new Size(79, 123)));
                else
                    userHand.Add(new Rectangle(new Point(250 + (i * 20), 462), new Size(79, 123)));
            }
        }

        /* Various Drawing Functions */
        internal void DrawScreen(SurfaceControl surfaceControl, PlayState playState, Hand userHand, AI[] computerPlayers,
                               int[] clicked, bool passing, bool playing, bool showAll)
        {
            surf.Fill(Color.Green);

            name1 = buttonFont.Render(playState.UserName, Color.White);
            name2 = buttonFont.Render(computerPlayers[0].Name, Color.White);
            name3 = buttonFont.Render(computerPlayers[1].Name, Color.White);
            name4 = buttonFont.Render(computerPlayers[2].Name, Color.White);

            if (playing || passing || showAll)
            {
                surf.Blit(name1, new Point(247 - name1.Width, 582 - name1.Height));
                surf.Blit(name2, new Point(15, 114 - name2.Height));
                surf.Blit(name3, new Point(532, 18));
                surf.Blit(name4, new Point(782 - name4.Width, 476));
            }
            int temp = 0;
            for (int i = 0; i < 3; i++)
            {
                if (clicked[i] >= 0)
                    temp++;
            }
            passCount = temp;
            if (passCount == 3 && passing && !playing)
            {
                if (playState.PassingMode == 0)
                {
                    surf.Blit(passButton, new Point(400, 400));
                    passButtonFont = buttonFont.Render("Pass Left", Color.Black);
                    surf.Blit(passButtonFont, new Point(411, 408));
                }
                if (playState.PassingMode == 1)
                {
                    surf.Blit(passButton, new Point(400, 400));
                    passButtonFont = buttonFont.Render("Pass Across", Color.Black);
                    surf.Blit(passButtonFont, new Point(403, 408));
                }
                if (playState.PassingMode == 2)
                {
                    surf.Blit(passButton, new Point(400, 400));
                    passButtonFont = buttonFont.Render("Pass Right", Color.Black);
                    surf.Blit(passButtonFont, new Point(409, 408));
                }
            }
            else if (passing && !playing)
            {
                if (playState.PassingMode == 0)
                {
                    surf.Blit(passButton, new Point(400, 400));
                    passButtonFont = buttonFont.Render("Pass Left", Color.Gray);
                    surf.Blit(passButtonFont, new Point(411, 408));
                }
                if (playState.PassingMode == 1)
                {
                    surf.Blit(passButton, new Point(400, 400));
                    passButtonFont = buttonFont.Render("Pass Across", Color.Gray);
                    surf.Blit(passButtonFont, new Point(403, 408));
                }
                if (playState.PassingMode == 2)
                {
                    surf.Blit(passButton, new Point(400, 400));
                    passButtonFont = buttonFont.Render("Pass Right", Color.Gray);
                    surf.Blit(passButtonFont, new Point(409, 408));
                }
            }

            int x = 250, y = 462;

            for (int j = 0; j < userHand.Cards.Count; j++)
            {
                if ((clicked[0] == j || clicked[1] == j || clicked[2] == j) && passing)
                    DrawCardFaceUp(userHand.Cards[j], x, y, true);
                else
                {
                    if (playState.Clicked == j)
                        DrawInverted(userHand.Cards[j], x, y);
                    else
                        DrawCardFaceUp(userHand.Cards[j], x, y, false);
                }
                x += 20;
            }

            if (!showAll)
            {
                x = 15; y = 120;
                for (int j = computerPlayers[0].AIHand.Cards.Count - 1; j >= 0; j--)
                {
                    DrawCardFaceDown(x, y, 1, passing, playState.CardsPassingContains(0, j));
                    y += 20;
                }

                x = 450; y = 15;
                for (int j = computerPlayers[1].AIHand.Cards.Count - 1; j >= 0; j--)
                {
                    DrawCardFaceDown(x, y, 2, passing, playState.CardsPassingContains(1, j));
                    x -= 20;
                }
                x = 706; y = 350;
                for (int j = computerPlayers[2].AIHand.Cards.Count - 1; j >= 0; j--)
                {
                    DrawCardFaceDown(x, y, 3, passing, playState.CardsPassingContains(2, j));
                    y -= 20;
                }
            }
            else
            {
                x = 15; y = 120;
                for (int j = 0; j < computerPlayers[0].AIHand.Cards.Count; j++)
                {
                    DrawCardFaceUp(computerPlayers[0].AIHand.Cards[j], x, y, false);
                    y += 20;

                }
                x = 450; y = 15;
                for (int j = 0; j < computerPlayers[1].AIHand.Cards.Count; j++)
                {
                    DrawCardFaceUp(computerPlayers[1].AIHand.Cards[j], x, y, false);
                    x -= 20;
                }
                x = 706; y = 350;
                for (int j = 0; j < computerPlayers[2].AIHand.Cards.Count; j++)
                {
                    DrawCardFaceUp(computerPlayers[2].AIHand.Cards[j], x, y, false);
                    y -= 20;
                }
            }

            if (playState.Trick.Count != 0)
            {
                DrawTricks(playState);
            }

            surfaceControl.Blit(surf);
        }

        private void DrawTricks(PlayState playState)
        {
            for (int i = 0; i < playState.Trick.Count; i++)
            {
                int x = 0, y = 0;
                int temp = (playState.Leader + i) % 4;
                switch (temp)
                {
                    case 0:
                        x = 390; y = 260;
                        break;
                    case 1:
                        x = 350; y = 220;
                        break;
                    case 2:
                        x = 395; y = 200;
                        break;
                    case 3:
                        x = 430; y = 240;
                        break;
                }

                surf.Blit(drawer.DrawCardFaceUp(playState.Trick[i]), new Point(x, y));
            }
        }

        private void DrawCardFaceUp(Card card, int x, int y, bool clicked)
        {
            if (clicked)
            {
                surf.Blit(drawer.DrawCardFaceUp(card), new Point(x, y - 10));
            }
            else
            {
                surf.Blit(drawer.DrawCardFaceUp(card), new Point(x, y));
            }
        }

        private void DrawInverted(Card card, int x, int y)
        {
            surf.Blit(drawer.DrawCardInverted(card), new Point(x, y));
        }

        private void DrawCardFaceDown(int x, int y, int hand, bool passing, bool dot)
        {
            int px = x, py = y;
            switch (hand)
            {
                case 1:
                    px = x + CardWidth + 3;
                    py = y + 4;
                    break;
                case 2:
                    py = y + CardHeight + 3;
                    px = x + CardWidth - 8;
                    break;
                case 3:
                    px = x - 7;
                    py = y + CardHeight - 8;
                    break;
            }

            if (passing && dot)
            {
                surf.Blit(passDot, new Point(px, py));
            }
            surf.Blit(drawer.DrawCardFacedown(), new Point(x, y));
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
                if (buttonFont != null)
                {
                    buttonFont.Dispose();
                    buttonFont = null;
                }
                if (passButton != null)
                {
                    passButton.Dispose();
                    passButton = null;
                }
                if (passDot != null)
                {
                    passDot.Dispose();
                    passDot = null;
                }
                if (surf != null)
                {
                    surf.Dispose();
                    surf = null;
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
