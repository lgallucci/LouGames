using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using SdlDotNet.Input;
using GameOptions;
using SdlDotNet.Core;

namespace Minesweeper
{
    internal partial class Mines : Form, IDisposable
    {
        private Board board;
        private GraphicsEngine gEngine;
        private int prvMines, timerValue;
        private bool Clickable, ButtonPressed;
        MouseButton PressedButton;
        private Image smileyButton, suprisedButton, coolButton, deadButton, clickingButton, prevButton;
        Thread thread1, thread2;
        private UserName unForm;
        HighScores hs;

        private delegate void UpdateMeLabel(string _str, Color _fontcolor);
        private delegate void CheckHighScoresDel();

        /* Constructor */
        internal Mines()
        {
            InitializeComponent();

            gEngine = new GraphicsEngine(surfaceControl1);
            unForm = new UserName();
            smileyButton = new Bitmap(Properties.Resources.SmileyButton);
            suprisedButton = new Bitmap(Properties.Resources.SuprisedButton);
            coolButton = new Bitmap(Properties.Resources.CoolButton);
            deadButton = new Bitmap(Properties.Resources.DeadButton);
            clickingButton = new Bitmap(Properties.Resources.ClickingButton);
            hs = new HighScores();

            MineLabel.Font = new Font("Arial", 20F, FontStyle.Regular,
                                       GraphicsUnit.Point, ((byte)(0)));
            MineLabel.ForeColor = Color.Red;
            MineLabel.Text = "000";

            CounterLabel.Font = new Font("Arial", 20F, FontStyle.Regular,
                                          GraphicsUnit.Point, ((byte)(0)));
            CounterLabel.ForeColor = Color.Red;
            CounterLabel.Text = "000";

            LoadImagesForBorders();

            this.CenterToScreen();
        }

        private void LoadImagesForBorders()
        {
            pbTopBorder.BackgroundImage = Properties.Resources.TopBorder;
            pbTopLeft.BackgroundImage = Properties.Resources.TopLeftCorner;
            pbTopRight.BackgroundImage = Properties.Resources.TopRightCorner;
            pbRightBorder.BackgroundImage = Properties.Resources.RightBorder;
            pbLeftBorder.BackgroundImage = Properties.Resources.LeftBorder;
            pbBottomBorder.BackgroundImage = Properties.Resources.BottomBorder;
            pbBottomLeft.BackgroundImage = Properties.Resources.LowerLeftCorner;
            pbBottomRight.BackgroundImage = Properties.Resources.LowerRightCorner;

            pbTopBorderMines.BackgroundImage = Properties.Resources.TopBorder;
            pbTopBorderMines.Width = MineLabel.Width;
            pbTopLeftMines.BackgroundImage = Properties.Resources.TopLeftCorner;
            pbTopRightMines.BackgroundImage = Properties.Resources.TopRightCorner;
            pbRightBorderMines.BackgroundImage = Properties.Resources.RightBorder;
            pbRightBorderMines.Height = MineLabel.Height;
            pbLeftBorderMines.BackgroundImage = Properties.Resources.LeftBorder;
            pbLeftBorderMines.Height = MineLabel.Height;
            pbBottomBorderMines.BackgroundImage = Properties.Resources.BottomBorder;
            pbBottomBorderMines.Width = MineLabel.Width;
            pbBottomLeftMines.BackgroundImage = Properties.Resources.LowerLeftCorner;
            pbBottomRightMines.BackgroundImage = Properties.Resources.LowerRightCorner;

            pbTopBorderCounter.BackgroundImage = Properties.Resources.TopBorder;
            pbTopBorderCounter.Width = CounterLabel.Width;
            pbTopLeftCounter.BackgroundImage = Properties.Resources.TopLeftCorner;
            pbTopRightCounter.BackgroundImage = Properties.Resources.TopRightCorner;
            pbRightBorderCounter.BackgroundImage = Properties.Resources.RightBorder;
            pbRightBorderCounter.Height = CounterLabel.Height;
            pbLeftBorderCounter.BackgroundImage = Properties.Resources.LeftBorder;
            pbLeftBorderCounter.Height = CounterLabel.Height;
            pbBottomBorderCounter.BackgroundImage = Properties.Resources.BottomBorder;
            pbBottomBorderCounter.Width = CounterLabel.Width;
            pbBottomLeftCounter.BackgroundImage = Properties.Resources.LowerLeftCorner;
            pbBottomRightCounter.BackgroundImage = Properties.Resources.LowerRightCorner;

        }

        /* Game Operation Methods */
        private void NewGame()
        {
            if (thread2 != null)
                thread2.Abort();
            UpdateTimerLabel(timerValue.ToString(CultureInfo.CurrentCulture), Color.Red);
            for (int i = 0; i < board.SizeX; i++)
                for (int j = 0; j < board.SizeY; j++)
                    board.data[i, j].displayInfo = DisplayInfo.SquareUnclicked;

            board.IsEmpty = true;
            Clickable = true;
            SmileButton.Image = smileyButton;
            prvMines = board.NumberOfMines;
            UpdateMinesLabel(prvMines.ToString(CultureInfo.CurrentCulture), Color.Red);
            timerValue = 0;
            UpdateTimerLabel(timerValue.ToString(CultureInfo.CurrentCulture), Color.Red);

            gEngine.DrawScreen(board, surfaceControl1);
        }

        private void Run(int x, int y, int mines)
        {
            if (thread2 != null)
                thread2.Abort();
            timerValue = 000;
            UpdateTimerLabel(timerValue.ToString(CultureInfo.CurrentCulture), Color.Red);

            prvMines = mines;
            UpdateMinesLabel(prvMines.ToString(CultureInfo.CurrentCulture), Color.Red);

            board = new Board(x, y, mines, 24);

            DisplayBorders();

            Clickable = true;

            this.Size = new Size(pbRightBorder.Right + 16, pbBottomBorder.Bottom + 38);

            surfaceControl1.Width = x * board.squareSize;
            surfaceControl1.Height = y * board.squareSize;

            surfaceControl1.Left = pbLeftBorder.Width;
            surfaceControl1.Top = pbTopBorder.Height + pbTopBorder.Top;

            gEngine.ResizeSurface(surfaceControl1);

            SmileButton.Image = smileyButton;

            SmileButton.Left = ((surfaceControl1.Width + (pbLeftBorder.Width * 2)) / 2 - SmileButton.Width / 2) - 1;

            MineLabel.Left = pbLeftBorderMines.Right;

            CounterLabel.Left = pbLeftBorderCounter.Right;

            gEngine.DrawScreen(board, surfaceControl1);
        }

        /* Form Display Method */
        private void DisplayBorders()
        {
            pbTopLeftMines.Location = new Point(0, this.fileToolStripMenuItem.Height + 2);
            pbTopBorderMines.Location = new Point(pbTopLeftMines.Width, pbTopLeftMines.Top);
            pbLeftBorderMines.Location = new Point(0, pbTopLeftMines.Height + pbTopLeftMines.Top);
            pbTopRightMines.Location = new Point(pbTopBorderMines.Width + pbTopLeftMines.Width, pbTopBorderMines.Top);
            pbRightBorderMines.Location = new Point(pbTopBorderMines.Width + pbTopLeftMines.Width,
                                                    pbTopRightMines.Top + pbTopRightMines.Height);
            pbBottomLeftMines.Location = new Point(0, pbLeftBorderMines.Height + pbLeftBorderMines.Top);
            pbBottomBorderMines.Location = new Point(pbBottomLeftMines.Width, pbLeftBorderMines.Height + pbLeftBorderMines.Top);
            pbBottomRightMines.Location = new Point(pbBottomBorderMines.Width + pbBottomBorderMines.Left,
                                                    pbRightBorderMines.Height + pbRightBorderMines.Top);

            pbTopLeft.Location = new Point(0, pbBottomBorderMines.Bottom);
            pbTopBorder.Location = new Point(pbTopLeft.Width, pbTopLeft.Top);
            pbTopBorder.Width = board.SizeX * board.squareSize;
            pbLeftBorder.Location = new Point(0, pbTopLeft.Height + pbTopLeft.Top);
            pbLeftBorder.Height = board.SizeY * board.squareSize;
            pbTopRight.Location = new Point(pbTopBorder.Width + pbTopLeft.Width, pbTopBorder.Top);
            pbRightBorder.Location = new Point(pbTopBorder.Width + pbTopLeft.Width, pbTopRight.Top + pbTopRight.Height);
            pbRightBorder.Height = board.SizeY * board.squareSize;
            pbBottomLeft.Location = new Point(0, pbLeftBorder.Height + pbLeftBorder.Top);
            pbBottomBorder.Location = new Point(pbBottomLeft.Width, pbLeftBorder.Height + pbLeftBorder.Top);
            pbBottomBorder.Width = board.SizeX * board.squareSize;
            pbBottomRight.Location = new Point(pbBottomBorder.Width + pbBottomBorder.Left, pbRightBorder.Height + pbRightBorder.Top);

            pbTopRightCounter.Location = new Point(pbRightBorder.Left, this.fileToolStripMenuItem.Height + 2);
            pbRightBorderCounter.Location = new Point(pbTopRightCounter.Left, pbTopRightCounter.Bottom);
            pbTopBorderCounter.Location = new Point(pbTopRightCounter.Left - CounterLabel.Width, pbTopRightCounter.Top);
            pbTopLeftCounter.Location = new Point(pbTopBorderCounter.Left - pbTopLeftCounter.Width, pbTopBorderCounter.Top);
            pbLeftBorderCounter.Location = new Point(pbTopLeftCounter.Left, pbTopLeftCounter.Bottom);
            pbBottomLeftCounter.Location = new Point(pbLeftBorderCounter.Left, pbLeftBorderCounter.Bottom);
            pbBottomBorderCounter.Location = new Point(pbBottomLeftCounter.Right, pbBottomLeftCounter.Top);
            pbBottomRightCounter.Location = new Point(pbBottomBorderCounter.Right, pbBottomBorderCounter.Top);

            MineLabel.Top = pbTopBorderMines.Bottom + 4;
            CounterLabel.Top = pbTopBorderCounter.Bottom + 4;
            SmileButton.Top = pbTopBorderMines.Bottom;
        }

        /* Separate Thread / Delegates Methods */
        private void TimerThread()
        {
            while (true)
            {
                try
                {
                    System.Threading.Thread.Sleep(1000);
                    timerValue++;
                    UpdateTimerLabel(timerValue.ToString(CultureInfo.CurrentCulture), Color.Red);
                }
                catch (Exception) { }
            }
        }

        private void DisplayErrorThread()
        {
            System.Threading.Thread.Sleep(350);
            board.ErrorFlag[0] = -1; board.ErrorFlag[1] = -1;
            gEngine.DrawScreen(board, surfaceControl1);
        }

        private void UpdateTimerLabel(string str, Color fontColor)
        {
            str = String.Format(CultureInfo.CurrentCulture, "{0:000}", Convert.ToInt32(str, CultureInfo.CurrentCulture));
            if (this.CounterLabel.InvokeRequired)
            {
                UpdateMeLabel updaterdelegate = new UpdateMeLabel(UpdateTimerLabel);
                if (gEngine != null)
                {
                    this.Invoke(updaterdelegate, new object[] { str, fontColor });
                }
            }
            else
            {
                if (str == "1000")
                    CounterLabel.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold,
                                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                else if (str == "000")
                    CounterLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular,
                                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CounterLabel.Text = str;
                CounterLabel.ForeColor = fontColor;
            }
        }

        private void UpdateMinesLabel(string str, Color fontColor)
        {
            str = String.Format(CultureInfo.CurrentCulture, "{0:000}", Convert.ToInt32(str, CultureInfo.CurrentCulture));
            if (this.MineLabel.InvokeRequired)
            {
                UpdateMeLabel updaterdelegate = new UpdateMeLabel(UpdateMinesLabel);
                this.Invoke(updaterdelegate, new object[] { str, fontColor });
            }
            else
            {
                MineLabel.Text = str;
                MineLabel.ForeColor = fontColor;
            }
        }

        private void CheckHighScore()
        {
            bool highScore = false;

            if (expertToolStripMenuItem.Checked)
            {
                if (timerValue < hs.times[2])
                {
                    unForm.ShowForm();
                    unForm.Focus();
                    hs.names[2] = unForm.userName;
                    hs.times[2] = timerValue;
                    highScore = true;
                }
            }
            else if (intermediateToolStripMenuItem.Checked)
            {
                if (timerValue < hs.times[1])
                {
                    unForm.ShowForm();
                    unForm.Focus();
                    hs.names[1] = unForm.userName;
                    hs.times[1] = timerValue;
                    highScore = true;
                }
            }
            else if (beginnerToolStripMenuItem.Checked)
            {
                if (timerValue < hs.times[0])
                {
                    unForm.ShowForm();
                    unForm.Focus();
                    hs.names[0] = unForm.userName;
                    hs.times[0] = timerValue;
                    highScore = true;
                }
            }
            if (highScore)
            {
                ShowHighScores();
            }

        }

        private void ShowHighScores()
        {
            if (this.InvokeRequired)
            {
                CheckHighScoresDel highscoredelegate = new CheckHighScoresDel(ShowHighScores);
                this.Invoke(highscoredelegate);
            }
            else
            {
                hs.showForm(this, false);
            }
        }

        /* SDL Event Methods */
        private void MouseButtonDownEvent(object sender, MouseButtonEventArgs mouse)
        {
            if (!Clickable)
                return;

            int x = (mouse.X / board.squareSize);
            int y = (mouse.Y / board.squareSize);

            if (x > board.SizeX * board.squareSize || x < 0 || y < 0 || y > board.SizeY * board.squareSize)
                return;

            ButtonPressed = true;
            SmileButton.Image = suprisedButton;

            if (mouse.Button == MouseButton.PrimaryButton)
            {
                if (ButtonPressed && PressedButton == MouseButton.SecondaryButton)
                {
                    ClickSquareWithBothButtons(x, y);
                }
                else
                {
                    if (board.data[x, y].displayInfo == DisplayInfo.SquareUnclicked)
                    {
                        board.clickedX = x;
                        board.clickedY = y;
                        gEngine.DrawScreen(board, surfaceControl1);
                    }
                    PressedButton = MouseButton.PrimaryButton;
                }
            }
            else if (mouse.Button == MouseButton.SecondaryButton)
            {
                if (ButtonPressed && PressedButton == MouseButton.PrimaryButton)
                {
                    ClickSquareWithBothButtons(x, y);
                }
                else
                {
                    PressedButton = MouseButton.SecondaryButton;
                }
            }
        }

        private void ClickSquareWithBothButtons(int x, int y)
        {
            if (board.data[x, y].displayInfo == DisplayInfo.SquareClicked)
            {
                board.doubleClicked = true;
                board.clickedX = x;
                board.clickedY = y;
                gEngine.DrawScreen(board, surfaceControl1);
            }
        }

        private void MouseButtonUpEvent(object sender, MouseButtonEventArgs mouse)
        {
            if (!Clickable)
                return;

            SmileButton.Image = smileyButton;

            ButtonPressed = false;
            PressedButton = MouseButton.None;

            int x = (mouse.X / board.squareSize);
            int y = (mouse.Y / board.squareSize);

            if (x >= board.SizeX || x < 0 || y < 0 || y >= board.SizeY)
            {
                board.doubleClicked = false;
                board.clickedX = -1;
                board.clickedY = -1;
                gEngine.DrawScreen(board, surfaceControl1);
                return;
            }

            if (mouse.Button == MouseButton.PrimaryButton)
            {
                board.clickedX = -1;
                board.clickedY = -1;
                if (board.data[x, y].displayInfo == DisplayInfo.SquareUnclicked
                    || board.data[x, y].displayInfo == DisplayInfo.QuestionFlag)
                {
                    if (board.IsEmpty)
                    {
                        board.CreateBoard(x, y);
                        thread2 = new Thread(new ThreadStart(TimerThread));
                        thread2.IsBackground = true;
                        thread2.Name = "TimerThread";
                        thread2.Priority = ThreadPriority.Normal;
                        thread2.Start();
                    }
                    ClickSquare(x, y);
                }
            }
            else if (mouse.Button == MouseButton.SecondaryButton)
            {
                if (board.data[x, y].displayInfo == DisplayInfo.SquareUnclicked)
                {
                    board.data[x, y].displayInfo = DisplayInfo.BombFlag;
                    prvMines--;
                }
                else if (board.data[x, y].displayInfo == DisplayInfo.BombFlag)
                {
                    board.data[x, y].displayInfo = DisplayInfo.QuestionFlag;
                    prvMines++;
                }
                else if (board.data[x, y].displayInfo == DisplayInfo.QuestionFlag)
                    board.data[x, y].displayInfo = DisplayInfo.SquareUnclicked;
            }

            if (board.data[x, y].displayInfo == DisplayInfo.SquareClicked && board.doubleClicked)
            {
                if (board.GetNumberOfAdjoiningFlags(x, y) == board.data[x, y].Value)
                {
                    ClickSurroundingSquares(x, y);
                }
                else
                {
                    if (board.ErrorFlag[0] == -1)
                    {
                        /*board.ErrorFlag[0] = x; board.ErrorFlag[1] = y;
                        if (thread3 == null)
                        {
                            thread3 = new Thread(new ThreadStart(DisplayErrorThread));
                            thread3.IsBackground = true;
                            thread3.Name = "ErrorThread";
                            thread3.Priority = ThreadPriority.Normal;
                            thread3.Start();
                        }
                        else if (!thread3.IsAlive)
                        {
                            thread3 = new Thread(new ThreadStart(DisplayErrorThread));
                            thread3.IsBackground = true;
                            thread3.Name = "ErrorThread";
                            thread3.Priority = ThreadPriority.Normal;
                            thread3.Start();
                        }*/

                    }
                }
            }

            board.doubleClicked = false;
            UpdateMinesLabel(prvMines.ToString(CultureInfo.CurrentCulture), Color.Red);

            gEngine.DrawScreen(board, surfaceControl1);
        }

        private void MouseMotionEvent(object sender, MouseMotionEventArgs move)
        {
            if (!ButtonPressed)
                return;

            if (PressedButton == MouseButton.PrimaryButton)
            {
                board.clickedX = (move.X / board.squareSize);
                board.clickedY = (move.Y / board.squareSize);
                gEngine.DrawScreen(board, surfaceControl1);
            }
        }

        private void ApplicationQuit(object sender, QuitEventArgs e)
        {
            thread2.Abort();
            SdlDotNet.Core.Events.QuitApplication();
        }

        /* Game Operation Methods */
        private void ClickSurroundingSquares(int x, int y)
        {
            if (y > 0)
            {
                if (x > 0)
                    if (board.data[x - 1, y - 1].displayInfo != DisplayInfo.BombFlag &&
                        board.data[x - 1, y - 1].displayInfo != DisplayInfo.IncorrectBombFlag)
                        ClickSquare(x - 1, y - 1);
                if (board.data[x, y - 1].displayInfo != DisplayInfo.BombFlag &&
                        board.data[x, y - 1].displayInfo != DisplayInfo.IncorrectBombFlag)
                    ClickSquare(x, y - 1);
                if (x < board.SizeX - 1)
                    if (board.data[x + 1, y - 1].displayInfo != DisplayInfo.BombFlag &&
                        board.data[x + 1, y - 1].displayInfo != DisplayInfo.IncorrectBombFlag)
                        ClickSquare(x + 1, y - 1);
            }
            if (x > 0)
                if (board.data[x - 1, y].displayInfo != DisplayInfo.BombFlag &&
                        board.data[x - 1, y].displayInfo != DisplayInfo.IncorrectBombFlag)
                    ClickSquare(x - 1, y);
            if (x < board.SizeX - 1)
                if (board.data[x + 1, y].displayInfo != DisplayInfo.BombFlag &&
                        board.data[x + 1, y].displayInfo != DisplayInfo.IncorrectBombFlag)
                    ClickSquare(x + 1, y);

            if (y < board.SizeY - 1)
            {
                if (x > 0)
                    if (board.data[x - 1, y + 1].displayInfo != DisplayInfo.BombFlag &&
                        board.data[x - 1, y + 1].displayInfo != DisplayInfo.IncorrectBombFlag)
                        ClickSquare(x - 1, y + 1);
                if (board.data[x, y + 1].displayInfo != DisplayInfo.BombFlag &&
                        board.data[x, y + 1].displayInfo != DisplayInfo.IncorrectBombFlag)
                    ClickSquare(x, y + 1);
                if (x < board.SizeX - 1)
                    if (board.data[x + 1, y + 1].displayInfo != DisplayInfo.BombFlag &&
                        board.data[x + 1, y + 1].displayInfo != DisplayInfo.IncorrectBombFlag)
                        ClickSquare(x + 1, y + 1);
            }
        }

        private void ClickSquare(int x, int y)
        {
            if (board.data[x, y].Value == 0)
            {
                EmptySquares(x, y);
                if (CheckGame())
                    EndGame(x, y, true);
            }
            else if (board.data[x, y].Value == -1)
            {
                EndGame(x, y, false);
            }
            else
            {
                board.data[x, y].displayInfo = DisplayInfo.SquareClicked;
                if (CheckGame())
                    EndGame(x, y, true);
            }
        }

        private void EmptySquares(int x, int y)
        {
            if (board.data[x, y].displayInfo == DisplayInfo.SquareClicked
                || board.data[x, y].displayInfo == DisplayInfo.BombFlag)
                return;

            board.data[x, y].displayInfo = DisplayInfo.SquareClicked;

            if (board.data[x, y].Value != 0)
                return;

            if (y > 0)
            {
                if (x > 0)
                    EmptySquares(x - 1, y - 1);
                EmptySquares(x, y - 1);
                if (x < board.SizeX - 1)
                    EmptySquares(x + 1, y - 1);
            }
            if (x > 0)
                EmptySquares(x - 1, y);
            if (x < board.SizeX - 1)
                EmptySquares(x + 1, y);

            if (y < board.SizeY - 1)
            {
                if (x > 0)
                    EmptySquares(x - 1, y + 1);
                EmptySquares(x, y + 1);
                if (x < board.SizeX - 1)
                    EmptySquares(x + 1, y + 1);
            }
        }

        private bool CheckGame()
        {
            for (int i = 0; i < board.SizeX; i++)
                for (int j = 0; j < board.SizeY; j++)
                {
                    if ((board.data[i, j].displayInfo == DisplayInfo.SquareUnclicked
                        || board.data[i, j].displayInfo == DisplayInfo.QuestionFlag
                        || board.data[i, j].displayInfo == DisplayInfo.BombFlag)
                        && board.data[i, j].Value != -1)
                        return false;
                }
            return true;
        }

        private void EndGame(int x, int y, bool win)
        {
            thread2.Abort();
            //thread3.Abort();
            if (win)
            {
                for (int i = 0; i < board.SizeX; i++)
                    for (int j = 0; j < board.SizeY; j++)
                    {
                        if (board.data[i, j].Value == -1)
                            board.data[i, j].displayInfo = DisplayInfo.BombFlag;
                        else
                            board.data[i, j].displayInfo = DisplayInfo.SquareClicked;
                    }
                SmileButton.Image = coolButton;
                prvMines = 0;
                UpdateMinesLabel(prvMines.ToString(CultureInfo.CurrentCulture), Color.Red);
                gEngine.DrawScreen(board, surfaceControl1);
                CheckHighScore();
            }
            else
            {
                board.data[x, y].displayInfo = DisplayInfo.BombExplode;
                for (int i = 0; i < board.SizeX; i++)
                    for (int j = 0; j < board.SizeY; j++)
                    {
                        if (i == x && j == y)
                        { }
                        else if (board.data[i, j].Value == -1
                            && board.data[i, j].displayInfo != DisplayInfo.BombFlag)
                            board.data[i, j].displayInfo = DisplayInfo.SquareClicked;
                        else if (board.data[i, j].displayInfo == DisplayInfo.BombFlag
                            && board.data[i, j].Value != -1)
                            board.data[i, j].displayInfo = DisplayInfo.IncorrectBombFlag;
                    }
                SmileButton.Image = deadButton;
            }

            Clickable = false;
        }

        /* Windows Form Event Methods */
        private void beginnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            beginnerToolStripMenuItem.CheckState = CheckState.Checked;
            intermediateToolStripMenuItem.CheckState = CheckState.Unchecked;
            expertToolStripMenuItem.CheckState = CheckState.Unchecked;
            customToolStripMenuItem.CheckState = CheckState.Unchecked;

            Run(9, 9, 10);
        }

        private void intermediateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            beginnerToolStripMenuItem.CheckState = CheckState.Unchecked;
            intermediateToolStripMenuItem.CheckState = CheckState.Checked;
            expertToolStripMenuItem.CheckState = CheckState.Unchecked;
            customToolStripMenuItem.CheckState = CheckState.Unchecked;

            Run(16, 16, 40);
        }

        private void expertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            beginnerToolStripMenuItem.CheckState = CheckState.Unchecked;
            intermediateToolStripMenuItem.CheckState = CheckState.Unchecked;
            expertToolStripMenuItem.CheckState = CheckState.Checked;
            customToolStripMenuItem.CheckState = CheckState.Unchecked;

            Run(30, 16, 99);
        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomBoard custom = new CustomBoard(board.SizeX, board.SizeY, board.NumberOfMines);
            DialogResult dr = custom.ShowDialog();

            if (dr == DialogResult.OK)
            {
                beginnerToolStripMenuItem.CheckState = CheckState.Unchecked;
                intermediateToolStripMenuItem.CheckState = CheckState.Unchecked;
                expertToolStripMenuItem.CheckState = CheckState.Unchecked;
                customToolStripMenuItem.CheckState = CheckState.Checked;

                Run(custom.BWidth, custom.BHeight, custom.BMines);
            }
        }

        private void highScoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hs.showForm(this, true);
        }

        private void SmileButton_Click(object sender, EventArgs e)
        {
            NewGame();
            prevButton = smileyButton;
        }

        private void SmileButton_MouseDown(object sender, MouseEventArgs e)
        {
            SmileButton.Image = clickingButton;
        }

        private void SmileButton_MouseLeave(object sender, EventArgs e)
        {
            SmileButton.Image = prevButton;
        }

        private void SmileButton_MouseEnter(object sender, EventArgs e)
        {
            prevButton = SmileButton.Image;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void Minesweeper_Load(object sender, EventArgs e)
        {
            var minesweeperScores = FileProcessing.LoadMinesweeper();
            hs.names = minesweeperScores.Item1;
            hs.times = minesweeperScores.Item2;            

            surfaceControl1.Width = 144;
            surfaceControl1.Height = 144;
            
            Run(9, 9, 10);

            SdlDotNet.Core.Events.TargetFps = 500;
            SdlDotNet.Core.Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuit);
            SdlDotNet.Core.Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(MouseButtonDownEvent);
            SdlDotNet.Core.Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(MouseButtonUpEvent);
            SdlDotNet.Core.Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(MouseMotionEvent);

            thread1 = new Thread(new ThreadStart(SdlDotNet.Core.Events.Run));
            thread1.IsBackground = true;
            thread1.Name = "SDL.NET";
            thread1.Priority = ThreadPriority.Normal;
            thread1.Start();
        }

        private void Mines_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileProcessing.SaveMinesweeper(hs.names, hs.times);
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
                if (coolButton != null)
                {
                    coolButton.Dispose();
                    coolButton = null;
                }
                if (smileyButton != null)
                {
                    smileyButton.Dispose();
                    smileyButton = null;
                }
                if (suprisedButton != null)
                {
                    suprisedButton.Dispose();
                    suprisedButton = null;
                }
                if (deadButton != null)
                {
                    deadButton.Dispose();
                    deadButton = null;
                }

                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}