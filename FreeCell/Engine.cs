using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SdlDotNet.Core;
using SdlDotNet.Input;
using CardGame;
using System.Threading;

namespace FreeCell
{
    internal partial class Engine : Form, IDisposable
    {
        GraphicsEngine gEngine;
        Tableau tableau;
        Deck deck;
        bool isGameRunning;
        Cursor myCursor;
        Options options;
        Thread createFireworks, thread1;
        //private float timerOffsetPerSecond = 2.0f;
        private const int offsetPerSecond = 600;
        private int pauseTime;
        private bool isCascading = false;

        /* Delegates */
        private delegate void UpdateMeLabel(string _str);

        private delegate void UpdateMeLabel2(int _str);

        private delegate void UpdateMeLabel3(string _str);

        private delegate void UpdateMeCursor(Cursor crs);

        internal static Cursor CreateCursor(Bitmap bmp, int hotspotX, int hotspotY)
        {
            IconInfo tmp = new IconInfo();
            UnsafeNativeMethods.GetIconInfo(bmp.GetHicon(), ref tmp);
            tmp.hotspotX = hotspotX;
            tmp.hotspotY = hotspotY;
            tmp.fIcon = false;
            return new Cursor(UnsafeNativeMethods.CreateIconIndirect(ref tmp));
        }

        /* Constructor */
        internal Engine()
        {
            InitializeComponent();

            Bitmap bitmap = new Bitmap(Properties.Resources.DownArrow);
            myCursor = CreateCursor(bitmap, 9, 30);
            bitmap.Dispose();

            gEngine = new GraphicsEngine(surfaceControl1);
            tableau = new Tableau();
            deck = new Deck(false);
            options = new Options();

            SdlDotNet.Core.Events.TargetFps = 500;
            SdlDotNet.Core.Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuit);
            SdlDotNet.Core.Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(MouseButtonDownEvent);
            SdlDotNet.Core.Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(MouseMotionEvent);
            SdlDotNet.Core.Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(MouseButtonUpEvent);
            SdlDotNet.Core.Events.Tick += new EventHandler<TickEventArgs>(Events_Tick);
        }

        /* Game Begin Methods */

        internal void Deal()
        {
            tableau.FireworksLeft = -1;
            options.GamesPlayed++;

            options.SaveOptions();

            gEngine.KingState = 1;
            int pileIndex = 0;

            deck.Shuffle();
            deck.Shuffle();

            for (int i = 0; i < 52; i++)
            {
                tableau.Piles[pileIndex].Add(deck[i]);

                pileIndex++;
                if (pileIndex == 8)
                    pileIndex = 0;
            }
            isGameRunning = true;
            gEngine.GameOver = 0;

        }

        internal void DealInOrder()
        {
            deck = new Deck(false);

            tableau.FireworksLeft = -1;
            options.GamesPlayed++;
            options.SaveOptions();

            gEngine.KingState = 1;
            int pileIndex = 0;

            for (int i = 51; i >= 0; i--)
            {
                tableau.Piles[pileIndex].Add(deck[i]);

                if (tableau.Piles[pileIndex].Count == 13)
                    pileIndex++;
            }
            isGameRunning = true;
            gEngine.GameOver = 0;
        }

        /* Mouse Intersection Methods */
        internal static int GetPileIntersects(int mouseX, int mouseY)
        {
            int temp;
            for (int i = 0; i < 8; i++)
            {
                temp = GraphicsEngine.LeftOfPiles + (i * (GraphicsEngine.CardWidth + GraphicsEngine.PileSpacing));

                if (mouseX >= temp && mouseX < temp + GraphicsEngine.CardWidth)
                {
                    if (mouseY > GraphicsEngine.TopOfPiles)
                        return i;
                }
            }
            return -1;
        }

        internal static int GetCardIntersects(int mouseY, int pileCards)
        {
            int temp;
            int spacing = GraphicsEngine.CardSpacing;
            if (pileCards > 10)
            {
                spacing -= (int)((pileCards - 10) * 1.5);
            }

            for (int i = 0; i < pileCards; i++)
            {
                temp = GraphicsEngine.TopOfPiles + (i * spacing);
                if (mouseY >= temp && mouseY < temp + spacing)
                    return i;

                if (i == pileCards - 1)
                {
                    if (mouseY >= temp && mouseY < temp + GraphicsEngine.CardHeight)
                        return i;
                }
            }

            return -1;
        }

        internal static int GetCellIntersects(int mouseX, int mouseY)
        {
            int temp;
            for (int i = 0; i < 4; i++)
            {
                temp = GraphicsEngine.LeftOfCells + ((GraphicsEngine.CardWidth + 4) * i);
                if (mouseX >= temp && mouseX < temp + (GraphicsEngine.CardWidth + 4))
                    if (mouseY >= GraphicsEngine.TopOfCells && mouseY < GraphicsEngine.TopOfCells + (GraphicsEngine.CardHeight + 4))
                        return i;
            }
            return -1;
        }

        internal static int GetFoundationIntersects(int mouseX, int mouseY)
        {
            int temp;
            for (int i = 0; i < 4; i++)
            {
                temp = GraphicsEngine.LeftOfFoundation + ((GraphicsEngine.CardWidth + 4) * i);
                if (mouseX >= temp && mouseX < temp + (GraphicsEngine.CardWidth + 4))
                    if (mouseY >= GraphicsEngine.TopOfCells && mouseY < GraphicsEngine.TopOfCells + (GraphicsEngine.CardHeight + 4))
                        return i;
            }
            return -1;
        }

        /* Delegate Invoke Methods */
        private void UpdateStatusLabel(string str)
        {
            if (this.statusStrip1.InvokeRequired)
            {
                UpdateMeLabel labelupdaterdelegate = new UpdateMeLabel(UpdateStatusLabel);
                this.Invoke(labelupdaterdelegate, new object[] { str });
            }
            else
            {
                sbMessage.Text = str;
            }
        }

        private void UpdateStatusLabel2(int str)
        {
            if (this.statusStrip1.InvokeRequired)
            {
                UpdateMeLabel2 labelupdaterdelegate = new UpdateMeLabel2(UpdateStatusLabel2);
                this.Invoke(labelupdaterdelegate, new object[] { str });
            }
            else
            {
                sbMovableCards.Text = "Movable Cards: " + str;
            }
        }

        private void UpdateStatusLabel3(string str)
        {
            if (this.statusStrip1.InvokeRequired)
            {
                UpdateMeLabel3 labelupdaterdelegate = new UpdateMeLabel3(UpdateStatusLabel3);
                this.Invoke(labelupdaterdelegate, new object[] { str });
            }
            else
            {
                sbMaxFoundation.Text = "FaceValue." + str;
            }
        }

        private void UpdateCursor(Cursor crs)
        {
            if (this.InvokeRequired)
            {
                UpdateMeCursor cursorupdaterdelegate = new UpdateMeCursor(UpdateCursor);
                this.Invoke(cursorupdaterdelegate, new object[] { crs });
            }
            else
            {
                this.Cursor = crs;
            }
        }
        
        /* SDL Event Methods */
        private void Events_Tick(object sender, TickEventArgs e)
        {
            if (pauseTime > 0)
            {
                pauseTime--;
                return;
            }
            
            if (isCascading)
            {
                isCascading = CascadeToFoundation();
            }

            gEngine.DrawScreen(surfaceControl1, tableau);
        }

        private void pauseForXTicks(int ticks)
        {
            pauseTime = ticks;
        }

        private void MouseButtonDownEvent(object sender, MouseButtonEventArgs e)
        {
            if (!isGameRunning || isCascading)
                return;

            bool success = false;

            if (e.Button == MouseButton.PrimaryButton)
            {
                int pile = GetPileIntersects(e.X, e.Y);

                if (pile != -1)
                {
                    success = HandlePileClicked(pile, e);
                }
                else
                {
                    pile = GetCellIntersects(e.X, e.Y);

                    if (pile != -1)
                    {
                        success = HandleCellClicked(pile);
                    }
                    else
                    {
                        pile = GetFoundationIntersects(e.X, e.Y);

                        if (pile != -1)
                        {
                            success = HandleFoundationClicked(pile);
                        }
                        else
                        {
                            tableau.Clicked[0] = -1;
                            tableau.Clicked[1] = -1;
                        }
                    }
                }
            }
            else if (e.Button == MouseButton.SecondaryButton && options.AllowRightClick)
            {
                int pile = GetPileIntersects(e.X, e.Y);
                int cardIndex;
                if (pile != -1)
                {
                    cardIndex = GetCardIntersects(e.Y, tableau.Piles[pile].Count);

                    if (cardIndex == tableau.Piles[pile].Count - 1 && cardIndex != -1)
                    {
                        success = tableau.TryToAddToFoundation(tableau.Piles[pile][cardIndex]);
                        if (success)
                        {
                            tableau.RemoveFromPile(pile, cardIndex);
                            tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
                        }
                        else
                        {
                            success = tableau.TryToMoveToList(tableau.Piles[pile][cardIndex]);
                            if (success)
                            {
                                tableau.RemoveFromPile(pile, cardIndex);
                                tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
                            }
                        }
                    }
                }
                else
                {
                    pile = GetCellIntersects(e.X, e.Y);
                    if (pile != -1)
                    {
                        success = tableau.TryToAddToFoundation(tableau.Cells[pile]);
                        if (success)
                        {
                            tableau.RemoveFromCell(pile);
                            tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
                        }
                        else
                        {
                            success = tableau.TryToMoveToList(tableau.Cells[pile]);
                            if (success)
                            {
                                tableau.RemoveFromCell(pile);
                                tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
                            }
                        }
                    }
                }
            }

            if (e.Button == MouseButton.SecondaryButton)
            {
                int pile = GetPileIntersects(e.X, e.Y);
                int cardIndex;
                if (pile != -1)
                {
                    cardIndex = GetCardIntersects(e.Y, tableau.Piles[pile].Count);

                    if (cardIndex != tableau.Piles[pile].Count - 1 && cardIndex != -1)
                    {
                        tableau.TopCard[0] = pile;
                        tableau.TopCard[1] = cardIndex;
                    }
                }
            }

            CheckEmptyPiles();
            if (success)
            {
                isCascading = true;
            }
            #if DEBUG
                UpdateStatusLabel2(tableau.CardsMovable + tableau.EmptyPiles);
                UpdateStatusLabel3("Red: " + tableau.MaxFoundationMoveRed.ToString() + ", Black:" + tableau.MaxFoundationMoveBlack.ToString());
            #endif
            CheckGameOver();
        }

        private void MouseButtonUpEvent(object sender, MouseButtonEventArgs e)
        {
            tableau.TopCard[0] = -1;
            tableau.TopCard[1] = -1;
        }

        private void MouseMotionEvent(object sender, MouseMotionEventArgs e)
        {
            if (gEngine.GameOver == 1)
                gEngine.KingState = -1;
            else if (e.X <= surfaceControl1.Width / 2)
                gEngine.KingState = 0;
            else
                gEngine.KingState = 1;

            if (!isMoveValid(e))
                UpdateCursor(Cursors.Default);
            else
                UpdateCursor(myCursor);
        }

        private void ApplicationQuit(object sender, QuitEventArgs e)
        {
            this.Close();
        }

        /* Game Action Process Methods */
        private bool HandleFoundationClicked(int pile)
        {
            bool success = true;
            if (tableau.Clicked[0] == 8)
            {
                if (!tableau.AddToFoundation(tableau.Cells[tableau.Clicked[1]], pile))
                {
                    tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
                }
                else
                {
                    tableau.RemoveFromCell(tableau.Clicked[1]);
                    tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
                    success = true;
                }

            }
            else if (tableau.Clicked[0] != -1)
            {
                IList<Card> cards = tableau.RemoveFromPile(tableau.Clicked[0], tableau.Clicked[1]);
                if (cards.Count == 1)
                    if (!tableau.AddToFoundation(cards[0], pile))
                        tableau.AddToPile(tableau.Clicked[0], cards);
                    else
                    {
                        tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
                    }
                else
                {
                    tableau.AddToPile(tableau.Clicked[0], cards);
                    tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
                    success = true;
                }
            }
            return success;
        }

        private bool HandleCellClicked(int pile)
        {
            bool success = false;
            if (tableau.Clicked[0] == 8)
            {
                tableau.Clicked[0] = -1;
                tableau.Clicked[1] = -1;
                if (tableau.Cells[pile] == null)
                    return false;
            }
            else if (tableau.Clicked[0] != -1)
            {
                IList<Card> cards = tableau.RemoveFromPile(tableau.Clicked[0], tableau.Clicked[1]);
                if (cards.Count == 1)
                {
                    if (!tableau.AddToCell(cards[0], pile))
                        tableau.AddToPile(tableau.Clicked[0], cards);
                    else
                    {
                        tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
                        success = true;
                    }
                }
                else
                {
                    tableau.AddToPile(tableau.Clicked[0], cards);
                }
            }
            else
            {
                if (tableau.Cells[pile] == null)
                    return false;
                tableau.Clicked[0] = 8;
                tableau.Clicked[1] = pile;
            }

            return success;
        }

        private bool HandlePileClicked(int pile, MouseButtonEventArgs e)
        {
            bool success = false;

            int cardIndex = GetCardIntersects(e.Y, tableau.Piles[pile].Count);

            if ((cardIndex == -1 || (tableau.Clicked[1] == cardIndex && tableau.Clicked[0] == pile)) && tableau.Piles[pile].Count > 0)
            {
                tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
            }
            else
            {
                if (tableau.Clicked[0] != -1)
                {
                    if (tableau.Clicked[0] == 8)
                    {
                        if (tableau.Cells[tableau.Clicked[1]] == null)
                            return false;

                        if (tableau.Piles[pile].Count == 0)
                        {
                            tableau.Piles[pile].Add(tableau.Cells[tableau.Clicked[1]]);
                            tableau.RemoveFromCell(tableau.Clicked[1]);
                            success = true;

                        }
                        else if ((tableau.Cells[tableau.Clicked[1]].CardRank + 1 ==
                            tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardRank) &&
                            (tableau.Cells[tableau.Clicked[1]].CardColor !=
                            tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardColor))
                        {
                            tableau.Piles[pile].Add(tableau.Cells[tableau.Clicked[1]]);
                            tableau.RemoveFromCell(tableau.Clicked[1]);
                            success = true;
                        }
                        tableau.Clicked[0] = -1;
                        tableau.Clicked[1] = -1;
                    }
                    else
                    {
                        int moveCard = tableau.CanMoveCard(tableau.Clicked[0], tableau.Clicked[1], pile);
                        IList<Card> cards = tableau.RemoveFromPile(tableau.Clicked[0], tableau.Clicked[1]);

                        int empty = moveCard;

                        if (tableau.Piles[pile].Count == 0)
                        {
                            empty--;
                            if (moveCard == 0)
                            {
                                tableau.AddToPile(pile, cards);
                                if (tableau.Clicked[0] == 8)
                                {
                                    tableau.RemoveFromCell(tableau.Clicked[1]);
                                }
                                success = true;
                                UpdateStatusLabel("Good Move!");
                            }
                            else
                            {
                                tableau.AddToPile(tableau.Clicked[0], cards);
                                UpdateStatusLabel("Good Move!");
                                success = true;
                            }
                            tableau.Clicked[0] = -1;
                            tableau.Clicked[1] = -1;
                        }
                        else if ((tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardColor != cards[0].CardColor)
                            && (tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardRank == cards[0].CardRank + 1)
                            && moveCard == 0)
                        {
                            tableau.AddToPile(pile, cards);
                            UpdateStatusLabel("Good Move!");
                            success = true;
                        }
                        else
                        {
                            tableau.AddToPile(tableau.Clicked[0], cards);
                            UpdateStatusLabel("Good Move!");
                            success = true;
                        }

                        if (moveCard > 0)
                            UpdateStatusLabel("You only have " + empty + " move spaces available!");
                        else if (moveCard == -1)
                            UpdateStatusLabel("Move was invalid!");

                        tableau.Clicked[0] = -1; tableau.Clicked[1] = -1;
                    }
                }
                else
                {
                    if (cardIndex != -1)
                    {
                        tableau.Clicked[0] = pile;
                        tableau.Clicked[1] = cardIndex;
                    }
                    else
                    {
                        tableau.Clicked[0] = -1;
                        tableau.Clicked[1] = -1;
                    }
                }
            }
            return success;
        }

        private bool CascadeToFoundation()
        {
            bool cardMoved = false;

            for (int i = 0; i < 4; i++)
            {
                if (tableau.Cells[i] != null)
                {
                    if (tableau.Cells[i].CardRank <= tableau.MaxFoundationMoveRed && tableau.Cells[i].CardColor == CardColor.Red)
                    {
                        if (tableau.TryToAddToFoundation(tableau.Cells[i]))
                        {
                            tableau.RemoveFromCell(i);
                            cardMoved = true;
                        }
                    }
                    else if (tableau.Cells[i].CardRank <= tableau.MaxFoundationMoveBlack && tableau.Cells[i].CardColor == CardColor.Black)
                    {
                        if (tableau.TryToAddToFoundation(tableau.Cells[i]))
                        {
                            tableau.RemoveFromCell(i);
                            cardMoved = true;
                        }
                    }
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (tableau.Piles[i].Count > 0)
                {
                    if (tableau.Piles[i][tableau.Piles[i].Count - 1].CardRank <= tableau.MaxFoundationMoveRed && tableau.Piles[i][tableau.Piles[i].Count - 1].CardColor == CardColor.Red)
                    {
                        if (tableau.TryToAddToFoundation(tableau.Piles[i][tableau.Piles[i].Count - 1]))
                        {
                            tableau.RemoveFromPile(i, tableau.Piles[i].Count - 1);
                            cardMoved = true;
                        }
                    }
                    else if (tableau.Piles[i][tableau.Piles[i].Count - 1].CardRank <= tableau.MaxFoundationMoveBlack && tableau.Piles[i][tableau.Piles[i].Count - 1].CardColor == CardColor.Black)
                    {
                        if (tableau.TryToAddToFoundation(tableau.Piles[i][tableau.Piles[i].Count - 1]))
                        {
                            tableau.RemoveFromPile(i, tableau.Piles[i].Count - 1);
                            cardMoved = true;
                        }
                    }
                }
            }

            if (cardMoved)
            {
                pauseForXTicks(50);
            }

            CheckGameOver();

            return cardMoved;
        }

        /* Game State Check Methods */
        private bool isMoveValid(MouseMotionEventArgs e)
        {
            if (tableau.Clicked[0] == -1 || tableau.Clicked[1] == -1)
                return false;

            int pile = GetPileIntersects(e.X, e.Y);

            if (pile == -1 || pile == tableau.Clicked[0])
            {
                pile = GetCellIntersects(e.X, e.Y);
                if (pile == -1)
                {
                    pile = GetFoundationIntersects(e.X, e.Y);

                    if (pile == -1)
                        return false;

                    if (tableau.Clicked[0] == 8)
                    {
                        if (tableau.Foundation[pile] == null && tableau.Cells[tableau.Clicked[1]].CardValue == FaceValue.Ace)
                            return true;

                        if (tableau.Foundation[pile] == null && tableau.Cells[tableau.Clicked[1]].CardValue != FaceValue.Ace)
                            return false;

                        if (tableau.Cells[tableau.Clicked[1]].CardSuit != tableau.Foundation[pile].CardSuit
                            || tableau.Cells[tableau.Clicked[1]].CardRank != tableau.Foundation[pile].CardRank + 1)
                            return false;
                    }
                    else
                    {
                        if (tableau.Foundation[pile] == null && tableau.Piles[tableau.Clicked[0]][tableau.Clicked[1]].CardValue == FaceValue.Ace)
                            return true;

                        if (tableau.Foundation[pile] == null && tableau.Piles[tableau.Clicked[0]][tableau.Clicked[1]].CardValue != FaceValue.Ace)
                            return false;

                        if (tableau.Piles[tableau.Clicked[0]][tableau.Clicked[1]].CardSuit != tableau.Foundation[pile].CardSuit
                            || tableau.Piles[tableau.Clicked[0]][tableau.Clicked[1]].CardRank != tableau.Foundation[pile].CardRank + 1)
                            return false;
                    }
                }
                else
                {
                    if (tableau.Clicked[0] == 8
                    || tableau.Cells[pile] != null
                    || tableau.Piles[tableau.Clicked[0]].Count - 1 != tableau.Clicked[1])
                        return false;
                }
            }
            else if (tableau.Clicked[0] != 8)
            {
                int empty = tableau.EmptyPiles;
                if (tableau.Piles[pile].Count == 0)
                    empty--;
                if (tableau.Piles[tableau.Clicked[0]].Count - tableau.Clicked[1] > tableau.CardsMovable + empty)
                    return false;

                int cardIndex = GetCardIntersects(e.Y, tableau.Piles[pile].Count);
                if (cardIndex == -1 && tableau.Piles[pile].Count != 0)
                    return false;
                else if (tableau.Piles[pile].Count != 0 &&
                         ((tableau.Piles[tableau.Clicked[0]][tableau.Clicked[1]].CardColor
                         == tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardColor)
                         || (tableau.Piles[tableau.Clicked[0]][tableau.Clicked[1]].CardRank
                         != tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardRank - 1)))
                    return false;
            }
            else
            {
                int cardIndex = GetCardIntersects(e.Y, tableau.Piles[pile].Count);
                if (cardIndex == -1 && tableau.Piles[pile].Count != 0)
                    return false;
                else if (tableau.Piles[pile].Count != 0 &&
                         ((tableau.Cells[tableau.Clicked[1]].CardColor
                         == tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardColor)
                         || (tableau.Cells[tableau.Clicked[1]].CardRank
                         != tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardRank - 1)))
                    return false;
            }
            return true;
        }

        private void CheckEmptyPiles()
        {
            tableau.EmptyPiles = 0;
            for (int i = 0; i < 8; i++)
            {
                if (tableau.Piles[i].Count == 0)
                {
                    tableau.EmptyPiles++;
                }
            }
        }

        private void CheckGameOver()
        {
            bool notwon = false;
            for (int i = 0; i < 4; i++)
            {
                if (tableau.Foundation[i] == null)
                    notwon = true;
            }

            if (!notwon && tableau.Foundation[0].CardValue == FaceValue.King
               && tableau.Foundation[1].CardValue == FaceValue.King
               && tableau.Foundation[2].CardValue == FaceValue.King
               && tableau.Foundation[3].CardValue == FaceValue.King)
            {
                gEngine.GameOver = 1;
                isGameRunning = false;
                gEngine.KingState = -1;
                options.GamesWon++;
                options.SaveOptions();
                StartFireworks(10);
            }

            bool lost = true;

            if (tableau.CardsMovable + tableau.EmptyPiles != 1)
                return;

            if (CheckMoveFromCell())
                lost = false;

            for (int i = 0; i < 8; i++)
            {
                if (tableau.Piles[i].Count == 0)
                    lost = false;

                if (lost && CheckMovePileToFoundation(i))
                    lost = false;

                if (lost && CheckMovePileToOtherPile(i))
                    lost = false;
            }

            if (lost)
            {
                gEngine.GameOver = -1;
                isGameRunning = false;
                options.GamesLost++;
                options.SaveOptions();
            }
        }

        private bool CheckMoveFromCell()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tableau.Piles[j].Count == 0)
                        return true;

                    if (tableau.Piles[j][tableau.Piles[j].Count - 1].CardColor != tableau.Cells[i].CardColor)
                        if (tableau.Piles[j][tableau.Piles[j].Count - 1].CardRank == tableau.Cells[i].CardRank + 1)
                            return true;
                }

                for (int j = 0; j < 4; j++)
                {
                    if (tableau.Foundation[j] == null && tableau.Cells[i].CardValue != FaceValue.Ace)
                        continue;

                    if (tableau.Foundation[j].CardSuit == tableau.Cells[i].CardSuit)
                        if ((tableau.Foundation[j].CardValue == FaceValue.Ace
                            && tableau.Cells[i].CardValue == FaceValue.Two)
                            || tableau.Foundation[j].CardRank == tableau.Cells[i].CardRank - 1)
                            return true;
                }
            }
            return false;
        }

        private bool CheckMovePileToOtherPile(int pile)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pile == j || tableau.Piles[j].Count < 1)
                    continue;

                if (tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardColor
                    != tableau.Piles[j][tableau.Piles[j].Count - 1].CardColor)
                {
                    if ((tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardValue == FaceValue.Two
                        && tableau.Piles[j][tableau.Piles[j].Count - 1].CardValue == FaceValue.Ace)
                        || (tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardRank
                        == tableau.Piles[j][tableau.Piles[j].Count - 1].CardRank + 1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckMovePileToFoundation(int pile)
        {
            for (int j = 0; j < 4; j++)
            {
                if (tableau.Foundation[j] == null)
                {
                    if (tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardValue == FaceValue.Ace)
                    {
                        return true;
                    }
                }
                else
                {
                    if (tableau.Foundation[j].CardSuit == tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardSuit)
                    {
                        if ((tableau.Foundation[j].CardValue == FaceValue.Ace
                            && tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardValue == FaceValue.Two)
                            || (tableau.Foundation[j].CardRank == tableau.Piles[pile][tableau.Piles[pile].Count - 1].CardRank - 1))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /* Firework Methods */
        private void StartFireworks(int num)
        {
            tableau.SetFireworks(num);

            createFireworks = new Thread(new ThreadStart(CreateFireworks));
            createFireworks.Start();
        }

        private void CreateFireworks()
        {
            while (tableau.FireworksLeft > 0)
            {
                tableau.FireworksLeft--;
                gEngine.ExplodeFirework(tableau);
                System.Threading.Thread.Sleep(1500);
            }
            tableau.FireworksLeft--;
        }

        /* Form Event Methods */
        private void dealToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isGameRunning)
            {
                var result = MessageBox.Show("Dealing will forfiet the current game.\nAre you sure?",
                    "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes || result == DialogResult.OK)
                {
                    tableau = new Tableau();
                    Deal();
                    options.GamesForfiet++;
                    options.SaveOptions();
                }
            }
            else
            {
                tableau = new Tableau();
                Deal();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationQuit(sender, new QuitEventArgs());
        }

        private void Engine_Load(object sender, EventArgs e)
        {
            thread1 = new Thread(new ThreadStart(SdlDotNet.Core.Events.Run));
            thread1.IsBackground = true;
            thread1.Name = "SDL.NET";
            thread1.Priority = ThreadPriority.Normal;
            thread1.Start();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            aboutForm.Location = new Point(this.Left + (this.Width / 2 - aboutForm.Width / 2), this.Top + (this.Height / 2 - aboutForm.Height / 2));
            aboutForm.ShowDialog(this);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            options.Location = new Point(this.Left + (this.Width / 2 - options.Width / 2), this.Top + (this.Height / 2 - options.Height / 2));
            options.ShowForm(this);
        }

        private void Engine_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isGameRunning)
            {
                options.GamesForfiet++;
                options.SaveOptions();
            }
            if(createFireworks != null && createFireworks.IsAlive)
                createFireworks.Abort();

            thread1.Abort();
        }

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                if (gEngine != null)
                {
                    gEngine.Dispose();
                    gEngine = null;
                }
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}