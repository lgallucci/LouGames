using System;
using SdlDotNet;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Audio;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;
using GameOptions;

namespace Bejeweled
{
    class Swapped
    {
        internal int x1, x2, y1, y2;
        internal bool isSwappingBack;
    }

    class Cascader
    {
        internal bool startSparkles;
        internal bool endSparkles;
        internal int removeCounter;
        internal int[] xRemoves = { 0 };
        internal int[] yRemoves = { 0 };
    }

    class Engine : IDisposable
    {
        Board board;
        GraphicsEngine gEngine;
        GameInfo gameInfo;

        private BackgroundWorker bw;
        private Swapped swapHelper;
        private Cascader cascadeHelper;
        private static float moveLeftOver, offset, timerOffset, timerMoveLeftOver;
        private float timerOffsetPerSecond = 2.0f;
        private const int offsetPerSecond = 600;
        private int move_counter, cascadeRows, pauseTime;
        private bool isCurrentBoardFalling, isCurrentBoardFallingFinished,
                     isChangingName, isClickable, isSwapping, isCascading,
                     isApplyingGravity, isSwitchedMode;
        private Sound levelChange, match, applause;
        private string userName;

        internal Engine()
        {
            gameInfo = new GameInfo();
            board = new Board(7);
            gEngine = new GraphicsEngine();

            bw = new BackgroundWorker();
            bw.DoWork += CheckHints;

            gameInfo.GameMode = 1;

            swapHelper = new Swapped();
            cascadeHelper = new Cascader();

            UnmanagedMemoryStream lChange = Properties.Resources.ChangeLevel;
            byte[] blevelChange = new byte[lChange.Capacity];
            lChange.Read(blevelChange, 0, (int)lChange.Capacity);
            levelChange = new Sound(blevelChange);
            lChange.Dispose();

            UnmanagedMemoryStream uMatch = Properties.Resources.matched;
            byte[] matched = new byte[uMatch.Capacity];
            uMatch.Read(matched, 0, (int)uMatch.Capacity);
            match = new Sound(matched);
            uMatch.Dispose();

            UnmanagedMemoryStream uApplause = Properties.Resources.applause;
            byte[] bApplause = new byte[uApplause.Capacity];
            uApplause.Read(bApplause, 0, (int)uApplause.Capacity);
            applause = new Sound(bApplause);
            uApplause.Dispose();

            match.Volume = 128;
            levelChange.Volume = 20;
            applause.Volume = 50;
            gameInfo.Sound = true;
        }

        internal void Run()
        {
            gameInfo.IsRunning = true;

            string[] names = new string[20];
            int[] scores = new int[20];

            FileProcessing.LoadBejeweled(ref names, ref scores);

            gameInfo.TopTenNames = names;
            gameInfo.TopTenScores = scores;

            StartGame();
            SdlDotNet.Input.Keyboard.KeyRepeat = true;
            Events.TargetFps = 500;
            Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuit);
            Events.MouseButtonDown += new EventHandler<SdlDotNet.Input.MouseButtonEventArgs>(MouseButtonDownEvent);
            Events.KeyboardDown += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(KeyboardDownEvent);
            Events.Tick += new EventHandler<TickEventArgs>(TickEvent);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(Events_MouseMotion);
            Events.Run();
        }

        private void SwitchMode()
        {
            isSwitchedMode = true;

            RetireGame(true);

            if (gameInfo.GameMode == 1)
            {
                gameInfo.GameMode = 2;
                gameInfo.TimerValue = 600;
            }
            else
            {
                gameInfo.GameMode = 1;
                gameInfo.Score = 0;
                gameInfo.StartScore = 0;
                gameInfo.NextScore = 0;
            }
        }

        private void StartGame()
        {
            gameInfo.Score = 0;
            gameInfo.Level = 0;

            isClickable = true;

            bw.RunWorkerAsync("!");

            UpdateLevel();
        }

        private void NewGame()
        {
            gameInfo.Hint = -2;
            timerOffsetPerSecond = 2.0f;
            gameInfo.Score = 0;
            gameInfo.Level = 0;
            gameInfo.IsRetired = false;
            gameInfo.GameOver = false;

            if (gameInfo.GameMode == 2)
                gameInfo.TimerValue = 600;

            isClickable = true;
            gameInfo.LevelUp = true;
            UpdateLevel();
        }

        #region Gem Movement Handler Methods
        private void HandleSwap() 
        {
            isClickable = false;
            float swap_offset = -1f;
            move_counter += (int)offset;

            if (swapHelper.x1 == swapHelper.x2)
            {
                if (swapHelper.y1 > swapHelper.y2)
                    swap_offset = -1f;
                else
                    swap_offset = 1f;

                board.data[swapHelper.x1, swapHelper.y1].Position.Y -= (int)(swap_offset * offset);
                board.data[swapHelper.x2, swapHelper.y2].Position.Y += (int)(swap_offset * offset);

            }
            else if (swapHelper.y1 == swapHelper.y2)
            {
                if (swapHelper.x1 > swapHelper.x2)
                    swap_offset = -1f;
                else
                    swap_offset = 1f;

                board.data[swapHelper.x1, swapHelper.y1].Position.X -= (int)(swap_offset * offset);
                board.data[swapHelper.x2, swapHelper.y2].Position.X += (int)(swap_offset * offset);
            }

            if (move_counter >= 75)
            {
                isSwapping = false;

                if (!gameInfo.IsRunning && gameInfo.GameMode == 2)
                    gameInfo.IsRunning = true;

                board.data[swapHelper.x1, swapHelper.y1].Position.X = (235 + (swapHelper.x1 * 75));
                board.data[swapHelper.x2, swapHelper.y2].Position.X = (235 + (swapHelper.x2 * 75));
                board.data[swapHelper.x1, swapHelper.y1].Position.Y = (swapHelper.y1 * 75);
                board.data[swapHelper.x2, swapHelper.y2].Position.Y = (swapHelper.y2 * 75);

                move_counter = 0;

                if (swapHelper.isSwappingBack)
                {
                    isClickable = true;
                    swapHelper.isSwappingBack = false;
                }
                else if (!board.CheckMoves(swapHelper.x1, swapHelper.y1) &&
                    !board.CheckMoves(swapHelper.x2, swapHelper.y2))
                {
                    pauseForXTicks(200);
                    swapHelper.isSwappingBack = true;
                    board.SwapPieces(swapHelper.x1, swapHelper.y1, swapHelper.x2, swapHelper.y2);
                    isSwapping = true;
                }
                else
                {
                    StartCascade();
                }
            }
        }

        private void HandleCascade()
        {
            isClickable = false;

            if (gameInfo.CascadeCount > 5)
                gameInfo.CascadeCount = 5;

            if (isApplyingGravity)
            {
                board.ApplyGravity();
                isApplyingGravity = false;
            }

            if (move_counter >= (cascadeRows * 75))
            {
                move_counter = 0;

                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        if (board.data[i, j].Position.Y != j * 75)
                        {
                            board.data[i, j].Position.Y = j * 75;
                        }
                    }

                cascadeRows = RemoveMatches();
                isApplyingGravity = true;
                gameInfo.CascadeCount++;
            }
            else
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        if (board.data[i, j].Position.Y != j * 75)
                        {
                            board.data[i, j].Position.Y += (int)(offset + (1.5 * gameInfo.CascadeCount));
                            if (board.data[i, j].Position.Y > j * 75)
                                board.data[i, j].Position.Y = j * 75;
                        }
                    }
            }
            move_counter += (int)(offset + (1.5 * gameInfo.CascadeCount));

            if (cascadeRows == 0)
            {
                isClickable = true;
                isCascading = false;
                if (gameInfo.Score >= gameInfo.NextScore && gameInfo.GameMode == 1)
                    UpdateLevel();
                else
                    FindMoves();
            }
        }

        private void BoardFallAway()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (board.data[i, j].Position.Y != (j * 75 + 600))
                    {
                        board.data[i, j].Position.Y += (int)(offset + (i * 1.1));
                        if (board.data[i, j].Position.Y > (j * 75 + 600))
                            board.data[i, j].Position.Y = (j * 75 + 600);
                    }
                }
        }

        private void HandleLevelChange()
        {
            if (isCurrentBoardFalling)
            {
                if (!isCurrentBoardFallingFinished && ((gameInfo.Level > 1) || !board.IsEmpty))
                {
                    BoardFallAway();

                    move_counter += (int)offset;
                    if (move_counter >= 600)
                    {
                        move_counter = 0;
                        isCurrentBoardFallingFinished = true;
                    }
                }
                else
                {
                    pauseForXTicks(200);
                    board.CreateBoard();
                    board.FindMoves();
                    isCurrentBoardFalling = false;
                    isCurrentBoardFallingFinished = false;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        if (board.data[i, j].Position.Y != j * 75)
                        {
                            board.data[i, j].Position.Y += (int)(offset + (i * 1.1));
                            if (board.data[i, j].Position.Y > j * 75)
                                board.data[i, j].Position.Y = j * 75;
                        }
                    }

                move_counter += (int)offset;
                if (move_counter >= 600)
                {
                    move_counter = 0;
                    gameInfo.LevelUp = false;
                    isClickable = true;
                    if (gameInfo.GameMode == 1)
                        gameInfo.IsRunning = true;
                }
            }
        }
        #endregion

        #region High Score Functions
        private bool CheckScore()
        {
            if (gameInfo.Score > gameInfo.TopTenScores[9 + (gameInfo.GameMode * 10 - 10)])
                return true;
            return false;
        }

        private void AddToTopTen(int score, String name)
        {
            for (int i = gameInfo.GameMode * 10 - 1; i >= (gameInfo.GameMode * 10 - 10); i--)
            {
                if (i != (gameInfo.GameMode * 10 - 10) && score > gameInfo.TopTenScores[i - 1])
                {
                    gameInfo.TopTenScores[i] = gameInfo.TopTenScores[i - 1];
                    gameInfo.TopTenNames[i] = gameInfo.TopTenNames[i - 1];
                }
                else
                {
                    gameInfo.TopTenScores[i] = score;
                    gameInfo.TopTenNames[i] = name;
                    gameInfo.Place = i - (gameInfo.GameMode * 10 - 10);
                    break;
                }
            }
            isChangingName = true;
            gameInfo.HighScores = true;
        }
        #endregion

        #region SDL Events Functions
        private void TickEvent(object sender, TickEventArgs e)
        {
            if (pauseTime > 0)
            {
                pauseTime--;
                return;
            }

            if (gameInfo.TimerValue <= 0 && gameInfo.GameMode == 2 && gameInfo.IsRunning)
                RetireGame(false);

            offset = 1.0f / 1000 * e.TicksElapsed * offsetPerSecond;
            timerOffset = 1.0f / 1000 * e.TicksElapsed * timerOffsetPerSecond;

            if (gameInfo.IsRunning && gameInfo.GameMode == 2)
                timerOffsetPerSecond += 0.0003f;
            if (timerOffsetPerSecond >= 12.0f)
                timerOffsetPerSecond = 12.0f;

            if (moveLeftOver < 1)
            {
                offset += moveLeftOver;
                moveLeftOver = 0;
            }

            if (timerMoveLeftOver < 1)
            {
                timerOffset += timerMoveLeftOver;
                timerMoveLeftOver = 0;
            }

            if (timerOffset < 1)
            {
                timerMoveLeftOver += timerOffset;
                timerOffset = 0;
            }

            if (offset < 1)
            {
                moveLeftOver += offset;
                offset = 0;
            }

            if (gameInfo.GameMode == 2 && gameInfo.IsRunning && !gameInfo.HighScores && !isCurrentBoardFalling)
            {
                gameInfo.TimerValue -= (int)timerOffset;

            }

            if (isSwitchedMode)
            {
                BoardFallAway();

                move_counter += (int)offset;
                if (move_counter >= 600)
                {
                    move_counter = 0;
                    isSwitchedMode = false;
                    board.IsEmpty = true;
                }
            }

            if (isSwapping)
                HandleSwap();

            if (isCascading && !cascadeHelper.startSparkles && !cascadeHelper.endSparkles)
                HandleCascade();

            gEngine.DrawScreen(board, gameInfo);

            if (cascadeHelper.endSparkles)
            {
                for (int i = 0; i < cascadeHelper.removeCounter; i++)
                    board.EmptySquare(cascadeHelper.xRemoves[i], cascadeHelper.yRemoves[i]);
                cascadeHelper.endSparkles = false;
                pauseForXTicks(75);
            }
            if (cascadeHelper.startSparkles)
            {
                for (int i = 0; i < cascadeHelper.removeCounter; i++)
                    board.EndSparkles(cascadeHelper.xRemoves[i], cascadeHelper.yRemoves[i]);

                match.Play();

                cascadeHelper.endSparkles = true;
                cascadeHelper.startSparkles = false;
                pauseForXTicks(75);
            }

            if (gameInfo.LevelUp)
            {
                HandleLevelChange();
            }
            if (gameInfo.GameOver && !isSwitchedMode)
            {
                gameInfo.GameOver = false;
                pauseForXTicks(500);

                if (!gameInfo.IsRetired)
                {
                    gameInfo.HighScores = true;
                }
                gameInfo.IsRetired = false;

                if (CheckScore())
                {

                    userName += "_";
                    AddToTopTen((int)gameInfo.Score, userName);
                    gameInfo.HighScores = true;
                    applause.Play();
                }
            }

        }

        private void Events_MouseMotion(object sender, MouseMotionEventArgs e)
        {
            if (gEngine.highScores.IntersectsWith(new Point(e.X, e.Y)) ||
                gEngine.newGame.IntersectsWith(new Point(e.X, e.Y)) ||
                gEngine.quitGame.IntersectsWith(new Point(e.X, e.Y)) ||
                gEngine.exitButton.IntersectsWith(new Point(e.X, e.Y)) ||
                gEngine.switchMode.IntersectsWith(new Point(e.X, e.Y)))
                Cursor.Current = Cursors.Hand;
            else
                Cursor.Current = Cursors.Default;
        }

        private void MouseButtonDownEvent(object sender, SdlDotNet.Input.MouseButtonEventArgs mouse)
        {
            if (mouse.Button != MouseButton.PrimaryButton)
                return;

            if (gameInfo.HighScores)
            {
                if (gEngine.exitButton.IntersectsWith(new Point(mouse.X, mouse.Y)))
                {
                    if (isChangingName)
                    {
                        userName = userName.Remove(userName.Length - 1);
                        gameInfo.TopTenNames[gameInfo.Place] = userName;
                        isChangingName = false;
                        gameInfo.Place = -1;
                    }
                    gameInfo.HighScores = false;
                    return;
                }
                return;
            }

            if (gEngine.highScores.IntersectsWith(new Point(mouse.X, mouse.Y)))
            {
                gameInfo.HighScores = true;
                return;
            }

            if (gEngine.newGame.IntersectsWith(new Point(mouse.X, mouse.Y)))
            {
                NewGame();
                return;
            }

            if (gEngine.quitGame.IntersectsWith(new Point(mouse.X, mouse.Y)))
            {
                RetireGame(true);
                return;
            }

            if (gEngine.switchMode.IntersectsWith(new Point(mouse.X, mouse.Y)))
            {
                SwitchMode();
                return;
            }

            if (gEngine.SoundImage.IntersectsWith(new Point(mouse.X, mouse.Y)) ||
                gEngine.SoundOffImage.IntersectsWith(new Point(mouse.X, mouse.Y)))
            {
                if (gameInfo.Sound)
                {
                    gameInfo.Sound = false;
                    match.Volume = 0;
                    levelChange.Volume = 0;
                    applause.Volume = 0;
                }
                else
                {
                    gameInfo.Sound = true;
                    match.Volume = 128;
                    levelChange.Volume = 20;
                    applause.Volume = 50;
                }
                return;
            }

            if (mouse.X < 235)
                return;

            if (!isClickable)
                return;

            int x = (mouse.X - 235) / 75;
            int y = mouse.Y / 75;

            gameInfo.Hint = 0;
            if (!board.clicked)
            {
                board.clicked = true;
                board.sel_x = x;
                board.sel_y = y;
            }
            else
            {
                if ((x == (board.sel_x + 1) && (y == board.sel_y)) ||
                   (x == (board.sel_x - 1) && (y == board.sel_y)) ||
                   (y == (board.sel_y + 1) && (x == board.sel_x)) ||
                   (y == (board.sel_y - 1) && (x == board.sel_x)))
                {
                    board.clicked = false;
                    board.SwapPieces(x, y, board.sel_x, board.sel_y);
                    swapHelper.x1 = x; swapHelper.y1 = y; swapHelper.x2 = board.sel_x; swapHelper.y2 = board.sel_y;
                    isSwapping = true;
                }
                board.clicked = false;
            }
        }

        private void KeyboardDownEvent(object sender, SdlDotNet.Input.KeyboardEventArgs keyboard)
        {
            if (isChangingName)
            {
                if (keyboard.Key == Key.Return)
                {
                    userName = userName.Remove(userName.Length - 1);
                    isChangingName = false;
                    gameInfo.TopTenNames[gameInfo.Place + (gameInfo.GameMode * 10 - 10)] = userName;
                    gameInfo.Place = -1;
                    return;
                }
                else if (keyboard.Key == Key.Backspace)
                {
                    if (userName.Length < 2)
                        return;
                    userName = userName.Remove(userName.Length - 2);
                    userName += "_";
                }
                else if (keyboard.Key == Key.Space && userName.Length <= 20)
                {
                    userName = userName.Insert(userName.Length - 1, " ");
                }
                else if ((keyboard.Key.ToString().Length == 1) &&
                    (Char.IsLetter(keyboard.Key.ToString()[0])) &&
                    userName.Length <= 20)
                {
                    if ((keyboard.Mod & ModifierKeys.RightShift) != 0 || (keyboard.Mod & ModifierKeys.LeftShift) != 0)
                        userName = userName.Insert(userName.Length - 1,
                            keyboard.KeyboardCharacter.ToUpper(CultureInfo.CurrentCulture));
                    else
                        userName = userName.Insert(userName.Length - 1, keyboard.KeyboardCharacter);
                }
                gameInfo.TopTenNames[gameInfo.Place + (gameInfo.GameMode * 10 - 10)] = userName;
            }
        }

        private void ApplicationQuit(object sender, QuitEventArgs args)
        {
            //gameInfo.IsRunning = false;
            FileProcessing.SaveBejeweled(gameInfo.TopTenNames, gameInfo.TopTenScores);
            Events.QuitApplication();
        }

        private void pauseForXTicks(int ticks)
        {
            pauseTime = ticks;
        }
        #endregion

        #region Matching Gem Functions
        private void FindMoves()
        {
            if (board.FindMoves())
                return;

            if (gameInfo.GameMode == 1)
                RetireGame(false);
            else
            {
                gameInfo.LevelUp = true;
                isClickable = false;
                isCurrentBoardFalling = true;
                HandleLevelChange();
                levelChange.Play();
                gameInfo.TimerValue -= (gameInfo.TimerValue / 4);
            }
        }

        private void StartCascade()
        {
            isCascading = true;
            gameInfo.CascadeCount = 0;
            cascadeRows = 0;
        }

        private int RemoveMatches()
        {
            int ret = 0;
            int[] xRemoves = new int[64];
            int[] yRemoves = new int[64];
            int counter = 0;

            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                {
                    if (board.CheckMoves(x, y))
                    {
                        xRemoves[counter] = x;
                        yRemoves[counter] = y;

                        if (counter == 0 || yRemoves[counter - 1] < yRemoves[counter])
                            ret++;

                        counter++;
                    }
                }
            if (counter != 0)
            {
                for (int i = 0; i < counter; i++)
                    board.StartSparkles(xRemoves[i], yRemoves[i]);

                cascadeHelper.xRemoves = xRemoves;
                cascadeHelper.yRemoves = yRemoves;
                cascadeHelper.removeCounter = counter;
                cascadeHelper.startSparkles = true;

                UpdateScore(counter, gameInfo.CascadeCount + 1);
            }

            return ret;
        }
        #endregion

        #region Game Progression Functions
        private void UpdateScore(int numOfBlocks, int mult)
        {
            gameInfo.Score += numOfBlocks * 10 * mult;

            if (gameInfo.GameMode == 2)
            {
                gameInfo.TimerValue += numOfBlocks * mult;

                if (gameInfo.TimerValue > 600)
                    gameInfo.TimerValue = 600;
            }
        }

        private void UpdateLevel()
        {
            gameInfo.Hint = 0;
            isClickable = false;

            gameInfo.LevelUp = true;
            isCurrentBoardFalling = true;

            gameInfo.StartScore = (int)gameInfo.Score;
            gameInfo.Level++;
            gameInfo.NextScore = (int)gameInfo.Score + (gameInfo.Level * 1000);

            if (gameInfo.Level > 1)
                levelChange.Play();
        }

        private void RetireGame(bool clicked)
        {
            if (!gameInfo.IsRunning)
                return;

            gameInfo.IsRunning = false;
            if (!isSwitchedMode)
                gameInfo.GameOver = true;
            if (clicked)
                gameInfo.IsRetired = true;

            isClickable = false;
        }
        #endregion

        #region Background worker that shows hints
        private void CheckHints(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (gameInfo.Hint < 15 && !gameInfo.HighScores)
                {
                    System.Threading.Thread.Sleep(1000);
                    gameInfo.Hint++;
                }
                else
                {
                    gameInfo.ShowHint = true;
                    System.Threading.Thread.Sleep(250);
                    gameInfo.ShowHint = false;
                    System.Threading.Thread.Sleep(250);
                    gameInfo.ShowHint = true;
                    System.Threading.Thread.Sleep(250);
                    gameInfo.ShowHint = false;
                    System.Threading.Thread.Sleep(250);
                    gameInfo.ShowHint = true;
                    System.Threading.Thread.Sleep(250);
                    gameInfo.Hint = 0;
                }
            }
        }
        #endregion

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
                if (gEngine != null)
                {
                    gEngine.Dispose();
                    gEngine = null;
                }
                if (levelChange != null)
                {
                    levelChange.Dispose();
                    levelChange = null;
                }
                if (match != null)
                {
                    match.Dispose();
                    match = null;
                }
                if (applause != null)
                {
                    applause.Dispose();
                    applause = null;
                }
                if (bw != null)
                {
                    bw.Dispose();
                    bw = null;
                }
            }
        }

        #endregion
    }
}