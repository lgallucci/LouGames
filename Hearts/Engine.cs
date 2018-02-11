using System;
using SdlDotNet;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Audio;
using SdlDotNet.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using CardGame;

namespace Hearts
{
    internal partial class Engine : Form
    {
        GraphicsEngine gEngine;
        Surface VideoScreen;
        private int pauseTime, numberOfClicked, round;
        private List<int> userScore;
        private int roundScore;
        private int[] passing;
        private Deck deck;
        private Hand userHand;
        private AI[] computerPlayers;
        private bool isPassing, isPlaying, isShowingPoints, isNotDealt, isSoundOn = true, showingScores;
        private PlayState playState;
        private Tricks userTricks;
        private Tricks[] tricks;
        private Sound glass, bawong;
        ScoreBox sb = new ScoreBox();
        Thread thread1;
        private delegate void ShowForm(ScoreBox frm);
        Options options;

        /* Constructor */
        internal Engine()
        {
            options = new Options();
            InitializeComponent();
            deck = new Deck(true);
            userHand = new Hand();
            passing = new int[3];
            computerPlayers = new AI[3];
            tricks = new Tricks[4];
            playState = new PlayState();
            userTricks = new Tricks();
            userScore = new List<int>();
            for (int i = 0; i < 3; i++)
                computerPlayers[i] = new AI();

            for (int i = 0; i < 3; i++)
                passing[i] = -1;

            for (int i = 0; i < 3; i++)
                tricks[i] = new Tricks();

            gEngine = new GraphicsEngine(surfaceControl1);

            options.LoadNames();

            computerPlayers[0].Name = options.comp1;
            computerPlayers[1].Name = options.comp2;
            computerPlayers[2].Name = options.comp3;

            playState.UserName = "Player";

            UnmanagedMemoryStream lGlass = Properties.Resources.glass;
            byte[] bGlass = new byte[lGlass.Capacity];
            lGlass.Read(bGlass, 0, (int)lGlass.Capacity);
            glass = new Sound(bGlass);

            UnmanagedMemoryStream lBawong = Properties.Resources.bawong;
            byte[] bBawong = new byte[lBawong.Capacity];
            lBawong.Read(bBawong, 0, (int)lBawong.Capacity);
            bawong = new Sound(bBawong);

            bawong.Volume = 20;
            glass.Volume = 20;

            userScore.Add(0);
            for (int i = 0; i < 3; i++)
                computerPlayers[i].CreateNewScore(0);

            sb.SetScores(userScore.AsReadOnly(), computerPlayers[0].Score, computerPlayers[1].Score, computerPlayers[2].Score,
             playState.UserName, computerPlayers[0].Name, computerPlayers[1].Name, computerPlayers[2].Name, false);

            SdlDotNet.Core.Events.TargetFps = 500;
            SdlDotNet.Core.Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuit);
            SdlDotNet.Core.Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(MouseButtonDownEvent);
            SdlDotNet.Core.Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(MouseButtonUpEvent);
            SdlDotNet.Core.Events.Tick += new EventHandler<TickEventArgs>(TickEvent);
        }

        /* Game Actions Methods */
        internal void Run()
        {
            if (playState.UserName == "Player")
            {
                options.Location = new Point(this.Left + (this.Width / 2 - options.Width / 2), this.Top + (this.Height / 2 - options.Height / 2));
                if (DialogResult.OK == options.ShowForm(true, "Player"))
                {
                    playState.UserName = options.player;
                }
            }

            Deal();

            isPassing = true; isPlaying = false;
        }

        internal void Deal()
        {
            Hand[] hands = new Hand[4];
            for (int i = 0; i < 4; i++)
                hands[i] = new Hand();

            deck.Shuffle();
            deck.Shuffle();

            for (int i = 0; i < 52; i++)
            {
                hands[0].Add(deck[i]); i++;
                hands[1].Add(deck[i]); i++;
                hands[2].Add(deck[i]); i++;
                hands[3].Add(deck[i]);
            }
            hands[0].Sort();
            hands[1].Sort();
            hands[2].Sort();
            hands[3].Sort();

            userHand.SetHand(hands[0].Cards);
            computerPlayers[0].SetHand(hands[1]);
            computerPlayers[1].SetHand(hands[2]);
            computerPlayers[2].SetHand(hands[3]);

            gEngine.ClearHand();
            gEngine.UpdateHand(userHand.Cards.Count, passing);

            playState.SetPassing(0, computerPlayers[0].PassIndexes());
            playState.SetPassing(1, computerPlayers[1].PassIndexes());
            playState.SetPassing(2, computerPlayers[2].PassIndexes());

            for (int i = 0; i < 3; i++)
            {
                computerPlayers[i].PreferredClubs = true;
                computerPlayers[i].PreferredDiamonds = true;
                computerPlayers[i].PreferredHearts = true;
                computerPlayers[i].PreferredSpades = true;
            }

            if (round != 0)
            {
                userScore.Add(userScore[round - 1]);
                for (int i = 0; i < 3; i++)
                    computerPlayers[i].CreateNewScore(computerPlayers[i].Score[round - 1]);
            }

            if (userHand.Cards.Count != 13 || computerPlayers[0].AIHand.Cards.Count != 13 ||
                computerPlayers[1].AIHand.Cards.Count != 13 || computerPlayers[2].AIHand.Cards.Count != 13)
            {
                throw new InvalidOperationException("DEAL FAILED!");
            }
        }

        private void ResetGame()
        {
            isPassing = true; isPlaying = false;
            isShowingPoints = false;
            isNotDealt = false;

            pauseTime = 0;
            numberOfClicked = 0;
            round = 0;

            userScore.Clear();

            deck = new Deck(true);
            userHand = new Hand();
            passing = new int[3];
            tricks = new Tricks[4];
            userTricks = new Tricks();
            userScore = new List<int>();

            for (int i = 0; i < 3; i++)
                passing[i] = -1;

            for (int i = 0; i < 3; i++)
                tricks[i] = new Tricks();

            computerPlayers[0].Reset();
            computerPlayers[1].Reset();
            computerPlayers[2].Reset();
            playState.Reset();

            userScore.Add(0);
            for (int i = 0; i < 3; i++)
                computerPlayers[i].CreateNewScore(0);

            Deal();
        }

        private void ShowScoreForm(ScoreBox frm)
        {
            if (this.InvokeRequired)
            {
                ShowForm updaterdelegate = new ShowForm(ShowScoreForm);
                this.Invoke(updaterdelegate, new object[] { frm });
            }
            else
            {
                frm.Location = new Point(this.Left + (this.Width / 2 - frm.Width / 2), this.Top + (this.Height / 2 - frm.Height / 2));
                frm.SetScores(userScore.AsReadOnly(), computerPlayers[0].Score, computerPlayers[1].Score, computerPlayers[2].Score,
                              playState.UserName, computerPlayers[0].Name, computerPlayers[1].Name, computerPlayers[2].Name, true);
                showingScores = true;
                frm.ShowForm(this);
                showingScores = false;
            }
        }

        private void UserPlayCard(Card c)
        {
            if (c.CardSuit == Suit.Hearts && !playState.IsBroken)
            {
                playState.IsBroken = true;
                if (isSoundOn)
                    glass.Play();
            }
            if (c.CardValue == FaceValue.Queen && c.CardSuit == Suit.Spades && !playState.QueenHasBeenPlayed)
            {
                playState.QueenHasBeenPlayed = true;
                if (isSoundOn)
                    bawong.Play();
            }

            playState.AddTrick(c);
            userHand.Remove(c);

            gEngine.ClearHand();
            gEngine.UpdateHand(userHand.Cards.Count, passing);
        }

        private int GetLead()
        {
            if (userHand.Contains(Suit.Clubs, FaceValue.Two))
                return 0;
            else if (computerPlayers[0].HasTwoOfClubs())
                return 1;
            else if (computerPlayers[1].HasTwoOfClubs())
                return 2;
            else if (computerPlayers[2].HasTwoOfClubs())
                return 3;
            else
                throw new InvalidOperationException("Two of Clubs doesn't exist");
        }

        private Collection<Card> RemovePassedCardsFromUserHand()
        {
            Collection<Card> ret = new Collection<Card>();
            foreach (int index in passing)
            {
                ret.Add(userHand.Cards[index]);
            }
            foreach (Card c in ret)
            {
                userHand.Remove(c);
            }
            return ret;
        }

        private void PassCards()
        {
            Collection<Card> c1 = new Collection<Card>();
            Collection<Card> c2 = new Collection<Card>();
            Collection<Card> c3 = new Collection<Card>();
            Collection<Card> u = new Collection<Card>();

            if (playState.PassingMode == 0)
            {
                c1 = RemovePassedCardsFromUserHand();
                c2 = computerPlayers[0].Pass();
                c3 = computerPlayers[1].Pass();
                u = computerPlayers[2].Pass();
                playState.PassingMode++;
            }
            else if (playState.PassingMode == 1)
            {
                c1 = computerPlayers[2].Pass();
                c2 = RemovePassedCardsFromUserHand();
                c3 = computerPlayers[0].Pass();
                u = computerPlayers[1].Pass();
                playState.PassingMode++;
            }
            else if (playState.PassingMode == 2)
            {
                c1 = computerPlayers[1].Pass();
                c2 = computerPlayers[2].Pass();
                c3 = RemovePassedCardsFromUserHand();
                u = computerPlayers[0].Pass();
                playState.PassingMode++;
            }
            else if (playState.PassingMode == 3)
                playState.PassingMode = 0;

            if (playState.PassingMode != 0)
            {
                computerPlayers[0].Add(c1);
                computerPlayers[0].AIHand.Sort();
                computerPlayers[1].Add(c2);
                computerPlayers[0].AIHand.Sort();
                computerPlayers[2].Add(c3);
                computerPlayers[0].AIHand.Sort();
                foreach (Card card in u)
                {
                    userHand.Add(card);
                }
                userHand.Sort();

                int index = 0;
                for (int i = 0; i < userHand.Cards.Count; i++)
                {
                    if (userHand.Cards[i] == u[0] ||
                        userHand.Cards[i] == u[1] ||
                        userHand.Cards[i] == u[2])
                    {
                        passing[index] = i;
                        index++;
                    }
                }
            }
            numberOfClicked = 0;
        }

        /* SDL Events Functions */
        private void TickEvent(object sender, TickEventArgs e)
        {
            if (pauseTime > 0)
            {
                pauseTime--;
                return;
            }

            gEngine.DrawScreen(surfaceControl1, playState, userHand, computerPlayers, passing, isPassing, isPlaying, isShowingPoints);
            if (isShowingPoints)
            {
                //SHOOT THE MOON!
                if (roundScore == 26)
                {
                    for (int i = 0; i < 3; i++)
                        computerPlayers[i].RoundScore = 26;
                    roundScore = 0;
                }
                else
                {
                    int shootTheMoon = -1;
                    for (int i = 0; i < 3; i++)
                    {
                        if (computerPlayers[i].RoundScore == 26)
                            shootTheMoon = i;
                    }

                    if (shootTheMoon != -1)
                    {
                        roundScore = 26;

                        for (int i = 0; i < 3; i++)
                        {
                            if (shootTheMoon == i)
                                computerPlayers[i].RoundScore = 0;
                            else
                                computerPlayers[i].RoundScore = 26;
                        }
                    }
                }

                userScore[round] += roundScore;
                roundScore = 0;
                for (int i = 0; i < 3; i++)
                {
                    computerPlayers[i].AddToScore(round, computerPlayers[i].RoundScore);
                    computerPlayers[i].RoundScore = 0;
                }

                ShowScoreForm(sb);

                pauseForXTicks(200);
                isShowingPoints = false;

                if (GameOver())
                    ResetGame();
            }

            if (isNotDealt)
            {
                for (int i = 0; i < 3; i++)
                    tricks[i].Clear();
                userTricks.Clear();

                userHand.Clear();
                for (int i = 0; i < 3; i++)
                    computerPlayers[i].AIHand.Clear();
                round++;
                Deal();
                isNotDealt = false;
                isPassing = true;

                if (playState.PassingMode == 3)
                {
                    PassCards();
                    isPlaying = true;
                }
            }

            if (isPassing && isPlaying)
            {
                pauseForXTicks(400);
                isPassing = false;
                passing[0] = -1; passing[1] = -1; passing[2] = -1;
                playState.Leader = GetLead();
                playState.Turn = playState.Leader;
            }

            if (playState.Trick.Count == 4)
            {
                CalculateTrick();
                return;
            }

            if (isPlaying && playState.Turn != 0)
            {
                Card play = computerPlayers[playState.Turn - 1].Play(playState);

                if (play.CardSuit == Suit.Hearts && !playState.IsBroken)
                {
                    playState.IsBroken = true;
                    if (isSoundOn)
                        glass.Play();
                }
                if (play.CardSuit == Suit.Spades && play.CardValue == FaceValue.Queen && !playState.QueenHasBeenPlayed)
                {
                    playState.QueenHasBeenPlayed = true;
                    if (isSoundOn)
                        bawong.Play();
                }

                if (playState.Trick.Count == 0)
                {
                    playState.Lead = play;
                    playState.Leader = playState.Turn;
                }

                playState.AddTrick(play);

                if (playState.Turn != 3)
                {
                    playState.Turn++;
                    pauseForXTicks(050);
                }
                else
                {
                    playState.Turn = 0;
                    pauseForXTicks(050);
                }
            }
            return;
        }

        private void MouseButtonDownEvent(object sender, SdlDotNet.Input.MouseButtonEventArgs mouse)
        {
            if (isPassing)
            {
                if (gEngine.GetPassButtonIntersects(mouse.X, mouse.Y))
                {
                    PassCards();
                    isPlaying = true;
                }

                bool found = false;
                int cardClicked = gEngine.GetCardIntersects(mouse.X, mouse.Y);
                if (cardClicked == -1)
                    return;
                for (int i = 0; i < 3; i++)
                {
                    if (found)
                    {
                        passing[i - 1] = passing[i];
                    }
                    if (passing[i] == cardClicked)
                        found = true;
                }
                if (found)
                {
                    passing[2] = -1;
                    numberOfClicked--;
                }
                if (numberOfClicked < 3 && !found)
                {
                    passing[numberOfClicked] = cardClicked;
                    numberOfClicked++;
                }
                gEngine.ClearHand();
                gEngine.UpdateHand(userHand.Cards.Count, passing);
            }
            else if (isPlaying)
            {
                if (playState.Turn == 0)
                {
                    if (playState.Trick.Count == 4)
                    {
                        CalculateTrick();
                        return;
                    }
                    int cardClicked = gEngine.GetCardIntersects(mouse.X, mouse.Y);

                    if (cardClicked == -1)
                        return;

                    if (playState.Trick.Count > 0)
                    {
                        if (UserHasSuit(playState.Lead.CardSuit))
                            if (userHand.Cards[cardClicked].CardSuit == playState.Lead.CardSuit)
                                UserPlayCard(userHand.Cards[cardClicked]);
                            else
                            {
                                playState.Clicked = cardClicked;
                                return;
                            }
                        else if ((userHand.Cards[cardClicked].CardSuit == Suit.Hearts ||
                            (userHand.Cards[cardClicked].CardSuit == Suit.Spades &&
                            userHand.Cards[cardClicked].CardValue == FaceValue.Queen)) &&
                            (playState.Lead.CardSuit == Suit.Clubs && playState.Lead.CardValue == FaceValue.Two))
                        {
                            playState.Clicked = cardClicked;
                            return;
                        }
                        else
                            UserPlayCard(userHand.Cards[cardClicked]);
                    }
                    else
                    {
                        if (userHand.Contains(Suit.Clubs, FaceValue.Two)
                            && !(userHand.Cards[cardClicked].CardSuit == Suit.Clubs
                            && userHand.Cards[cardClicked].CardValue == FaceValue.Two))
                        {
                            playState.Clicked = cardClicked;
                            return;
                        }
                        else if (!playState.IsBroken && userHand.Cards[cardClicked].CardSuit == Suit.Hearts)
                        {
                            playState.Clicked = cardClicked;

                            if (UserHasSuit(Suit.Clubs) || UserHasSuit(Suit.Diamonds) || UserHasSuit(Suit.Spades))
                                return;
                        }
                        playState.Lead = userHand.Cards[cardClicked];
                        UserPlayCard(userHand.Cards[cardClicked]);
                    }
                    if (userHand.Cards.Count == 12 && (playState.Lead.CardSuit != Suit.Clubs || playState.Lead.CardValue != FaceValue.Two))
                        throw new InvalidOperationException("ENGINE - FIRST CARD WASN'T TWO OF CLUBS!");

                    playState.Turn++;
                    pauseForXTicks(050);
                }
            }
        }

        private void MouseButtonUpEvent(object sender, SdlDotNet.Input.MouseButtonEventArgs mouse)
        {
            playState.Clicked = -1;
        }

        private void ApplicationQuit(object sender, QuitEventArgs args)
        {
            this.Close();
        }

        private void pauseForXTicks(int ticks)
        {
            pauseTime = ticks;
        }

        /* Game State Check Methods */
        private void CalculateTrick()
        {
            Card highCard = playState.Lead;
            int winner = playState.Leader;
            int score = 0;

            for (int i = 0; i < playState.Trick.Count; i++)
            {
                if (playState.Trick[i].CardRank > highCard.CardRank && playState.Trick[i].CardSuit == playState.Lead.CardSuit)
                {
                    highCard = playState.Trick[i];
                    winner = (playState.Leader + i) % 4;
                }

                if (playState.Trick[i].CardSuit == Suit.Hearts)
                    score++;
                else if (playState.Trick[i].CardSuit == Suit.Spades && playState.Trick[i].CardValue == FaceValue.Queen)
                    score += 13;
            }

            if (winner == 0)
            {

                roundScore += score;
                for (int i = 0; i < playState.Trick.Count; i++)
                    if (playState.Trick[i].CardSuit == Suit.Hearts ||
                        (playState.Trick[i].CardSuit == Suit.Spades && playState.Trick[i].CardValue == FaceValue.Queen))
                        userTricks.Add(playState.Trick[i]);
            }
            else
            {
                computerPlayers[winner - 1].RoundScore += score;

                for (int i = 0; i < playState.Trick.Count; i++)
                    if (playState.Trick[i].CardSuit == Suit.Hearts ||
                        (playState.Trick[i].CardSuit == Suit.Spades && playState.Trick[i].CardValue == FaceValue.Queen))
                        tricks[winner - 1].Add(playState.Trick[i]);

                if (winner == playState.Leader)
                {
                    bool followed = false;
                    for (int i = 1; i < playState.Trick.Count; i++)
                    {
                        if (playState.Trick[i].CardSuit == playState.Lead.CardSuit)
                            followed = true;
                    }
                    if (!followed)
                        computerPlayers[winner - 1].RemoveSuitPreference(playState.Lead.CardSuit);
                }

            }

            if (userHand.Cards.Count == 0)
            {
                isPlaying = false;
                isShowingPoints = true;
                playState.QueenHasBeenPlayed = false;
                userHand.SetHand(userTricks.Cards);
                for (int i = 0; i < 3; i++)
                    computerPlayers[i].AIHand.SetHand(tricks[i].Cards);
                playState.ClearTrick();

                isNotDealt = true;
                playState.IsBroken = false;
                return;
            }

            playState.ClearTrick();
            playState.Turn = winner;
            playState.Leader = winner;
            pauseForXTicks(400);
        }

        private bool GameOver()
        {
            for (int i = 0; i < 3; i++)
                if (computerPlayers[i].Score[round] >= 100)
                    return true;

            if (userScore[round] >= 100)
                return true;

            return false;
        }

        private bool UserHasSuit(Suit suit)
        {
            foreach (Card c in userHand.Cards)
            {
                if (c.CardSuit == suit)
                    return true;
            }
            return false;
        }

        /* Game Form Methods */
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationQuit(sender, new QuitEventArgs());
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isPlaying && !isPassing)
                Run();
            else
                ResetGame();
        }

        private void viewScoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!showingScores)
            {
                sb.SetNames(playState.UserName, computerPlayers[0].Name, computerPlayers[1].Name, computerPlayers[2].Name, false);
                sb.ShowForm(this);
            }
        }

        private void soundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (soundToolStripMenuItem.Checked)
            {
                isSoundOn = false;
                soundToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
            else
            {
                isSoundOn = true;
                soundToolStripMenuItem.CheckState = CheckState.Checked;
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            options.Location = new Point(this.Left + (this.Width / 2 - options.Width / 2), this.Top + (this.Height / 2 - options.Height / 2));
            if (options.ShowForm(false, playState.UserName) == DialogResult.OK)
            {
                playState.UserName = options.player;
                computerPlayers[0].Name = options.comp1;
                computerPlayers[1].Name = options.comp2;
                computerPlayers[2].Name = options.comp3;
            }
        }

        private void Engine_Load(object sender, EventArgs e)
        {
            thread1 = new Thread(new ThreadStart(SdlDotNet.Core.Events.Run));
            thread1.IsBackground = true;
            thread1.Name = "SDL.NET";
            thread1.Priority = ThreadPriority.Normal;
            thread1.Start();
        }

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (gEngine != null)
                {
                    gEngine.Dispose();
                    gEngine = null;
                }
                if (glass != null)
                {
                    glass.Dispose();
                    glass = null;
                }
                if (sb != null)
                {
                    sb.Dispose();
                    sb = null;
                }
                if (bawong != null)
                {
                    bawong.Dispose();
                    bawong = null;
                }
                if (VideoScreen != null)
                {
                    VideoScreen.Dispose();
                    VideoScreen = null;
                }
            }
        }

        #endregion
    }
}