using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using System;
using System.Drawing;
using System.Globalization;

namespace Bejeweled
{
    class GraphicsEngine : IDisposable
    {
        #region Surface Variables
        private Surface VideoScreen;
        private Surface BoardImage;
        private Surface SideImage;
        private Surface ScoreFont;
        private Surface LevelFont;
        private Surface GameFont;
        private Surface GameFontShadow;
        private Surface GFXFont;
        private Surface GFXFontShadow;
        private Surface ClickedImage;
        private Surface HintImage;
        private Surface BarBorderImage;
        private Surface BarBackground;
        private Surface BarFillImage;
        private Surface ScoresWindow;
        private Surface ModeFont;

        internal Sprite SoundImage;
        internal Sprite SoundOffImage;

        private Surface BlankImage;
        private Surface Gem1;
        private Surface Gem2;
        private Surface Gem3;
        private Surface Gem4;
        private Surface Gem5;
        private Surface Gem6;
        private Surface Gem7;
        private Surface Sparkle;
        private Surface ButtonImage;
        private Surface ExitButton;

        private Point BarBorderImagePos;
        private Point BoardImagePos;
        private Point SideImagePos;
        private Point ScoreFontPos;
        private Point LevelFontPos;
        private Point ClickedImagePos;

        private SdlDotNet.Graphics.Font infoFont;
        private SdlDotNet.Graphics.Font gameFont;
        private SdlDotNet.Graphics.Font scoreFont;
        private SdlDotNet.Graphics.Font gfxFont;
        #endregion
        internal Button newGame, highScores, quitGame, switchMode, exitButton;

        /* Constructor */
        internal GraphicsEngine()
        {
            VideoScreen = Video.SetVideoMode(835, 600, 32, false, false, false, false, true);
            Video.WindowCaption = "Bejeweled";
            Video.WindowIcon(Properties.Resources.game);

            LoadImages();

            newGame = new Button();
            newGame.SetImage(new Surface(ButtonImage), new Point(8, 485));
            newGame.SetText("New Game");
            highScores = new Button();
            highScores.SetImage(new Surface(ButtonImage), new Point(8, 410));
            highScores.SetText("High Scores");
            quitGame = new Button();
            quitGame.SetImage(new Surface(ButtonImage), new Point(113, 485));
            quitGame.SetText("Retire Game");
            switchMode = new Button();
            switchMode.SetImage(new Surface(ButtonImage), new Point(113, 410));
            switchMode.SetText("Timed Mode");
            exitButton = new Button();
            exitButton.SetImage(ExitButton, new Point(625, 97));
        }

        /* Load Images */
        private void LoadImages()
        {
            BoardImage = (new Surface(Properties.Resources.bjw_board)).Convert(VideoScreen, false, true);
            SideImage = (new Surface(Properties.Resources.bjw_logo)).Convert(VideoScreen, false, true);
            BarBackground = (new Surface(Properties.Resources.level_bar)).Convert(VideoScreen, false, true);
            BarFillImage = (new Surface(Properties.Resources.level_bar_stretch)).Convert(VideoScreen, false, true);

            ScoresWindow = (new Surface(Properties.Resources.bjw_scores)).Convert(VideoScreen, false, true);
            ScoresWindow.Transparent = true;
            ScoresWindow.TransparentColor = Color.FromArgb(255, 000, 255);

            BarBorderImage = (new Surface(Properties.Resources.level_bar_border)).Convert(VideoScreen, false, true);
            BarBorderImage.Transparent = true;
            BarBorderImage.TransparentColor = Color.FromArgb(255, 000, 255);

            ExitButton = new Surface(Properties.Resources.exit_button).Convert(VideoScreen, false, true);
            ExitButton.Transparent = true;
            ExitButton.TransparentColor = Color.FromArgb(255, 000, 255);

            ButtonImage = new Surface(Properties.Resources.button).Convert(VideoScreen, false, true);
            ButtonImage.Transparent = true;
            ButtonImage.TransparentColor = Color.FromArgb(255, 000, 255);

            ClickedImage = (new Surface(Properties.Resources.clicked)).Convert(VideoScreen, false, true);
            ClickedImage.Transparent = true;
            ClickedImage.TransparentColor = Color.FromArgb(255, 000, 255);

            HintImage = (new Surface(Properties.Resources.clicked)).Convert(VideoScreen, false, true);
            HintImage.Transparent = true;
            HintImage.TransparentColor = Color.FromArgb(255, 000, 255);

            BlankImage = (new Surface(Properties.Resources.jewel0)).Convert(VideoScreen, false, true);
            BlankImage.Transparent = true;
            BlankImage.TransparentColor = Color.FromArgb(255, 000, 255);

            Gem1 = (new Surface(Properties.Resources.jewel1)).Convert(VideoScreen, false, true);
            Gem1.Transparent = true;
            Gem1.TransparentColor = Color.FromArgb(255, 000, 255);

            Gem2 = (new Surface(Properties.Resources.jewel2)).Convert(VideoScreen, false, true);
            Gem2.Transparent = true;
            Gem2.TransparentColor = Color.FromArgb(255, 000, 255);

            Gem3 = (new Surface(Properties.Resources.jewel3)).Convert(VideoScreen, false, true);
            Gem3.Transparent = true;
            Gem3.TransparentColor = Color.FromArgb(255, 000, 255);

            Gem4 = (new Surface(Properties.Resources.jewel4)).Convert(VideoScreen, false, true);
            Gem4.Transparent = true;
            Gem4.TransparentColor = Color.FromArgb(255, 000, 255);

            Gem5 = (new Surface(Properties.Resources.jewel5)).Convert(VideoScreen, false, true);
            Gem5.Transparent = true;
            Gem5.TransparentColor = Color.FromArgb(255, 000, 255);

            Gem6 = (new Surface(Properties.Resources.jewel6)).Convert(VideoScreen, false, true);
            Gem6.Transparent = true;
            Gem6.TransparentColor = Color.FromArgb(255, 000, 255);

            Gem7 = (new Surface(Properties.Resources.jewel7)).Convert(VideoScreen, false, true);
            Gem7.Transparent = true;
            Gem7.TransparentColor = Color.FromArgb(255, 000, 255);

            Sparkle = (new Surface(Properties.Resources.sparkle)).Convert(VideoScreen, false, true);
            Sparkle.Transparent = true;
            Sparkle.TransparentColor = Color.FromArgb(255, 000, 255);

            SoundImage = new Sprite(); SoundOffImage = new Sprite();

            SoundImage.Surface = (new Surface(Properties.Resources.sound)).Convert(VideoScreen, false, true);
            SoundImage.Transparent = true;
            SoundImage.TransparentColor = Color.FromArgb(255, 000, 255);

            SoundOffImage.Surface = (new Surface(Properties.Resources.sound_off)).Convert(VideoScreen, false, true);
            SoundOffImage.Transparent = true;
            SoundOffImage.TransparentColor = Color.FromArgb(255, 000, 255);

            SoundImage.X = 97;SoundImage.Y = 535;
            SoundOffImage.X = 97;SoundOffImage.Y = 535;

            BoardImagePos = new Point(235, 0);
            SideImagePos = new Point(0, 0);
            BarBorderImagePos = new Point(221, 0);

            try
            {
                infoFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\trebuc.ttf", 35);
                gameFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\trebuc.ttf", 60);
                scoreFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\trebuc.ttf", 16);
                gfxFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\trebuc.ttf", 32);
            }
            catch (SdlException)
            {
                infoFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\arial.ttf", 30);
                gameFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\arial.ttf", 55);
                scoreFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\arial.ttf", 14);
                gfxFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\arial.ttf", 30);
            }
        }

        /* Various Drawing Functions */
        internal void DrawScreen(Board board, GameInfo gameInfo)
        {
            double height;
            VideoScreen.Fill(Color.Black);
            ScoreFontPos = new Point(95 - (10 * (gameInfo.Score.ToString(CultureInfo.CurrentCulture).Length - 1)), 210);
            LevelFontPos = new Point(95 - (10 * (gameInfo.Level.ToString(CultureInfo.CurrentCulture).Length - 1)), 335);
            ScoreFont = infoFont.Render(gameInfo.Score.ToString(CultureInfo.CurrentCulture), Color.Black);
            LevelFont = infoFont.Render(gameInfo.Level.ToString(CultureInfo.CurrentCulture), Color.Black);
            VideoScreen.Blit(BoardImage, BoardImagePos);
            VideoScreen.Blit(SideImage, SideImagePos);
            VideoScreen.Blit(ScoreFont, ScoreFontPos);
            VideoScreen.Blit(LevelFont, LevelFontPos);

            if (gameInfo.Sound)
                VideoScreen.Blit(SoundImage);
            else
                VideoScreen.Blit(SoundOffImage);

            VideoScreen.Blit(BarBackground, BarBorderImagePos);
            if (gameInfo.GameMode == 1)
                height = (((double)(gameInfo.Score - gameInfo.StartScore) /
                    (double)(gameInfo.NextScore - gameInfo.StartScore)) * 600);
            else
                height = gameInfo.TimerValue;

            if (height > 600)
                height = 600;
            VideoScreen.Blit(BarFillImage, new Point(221, 600 - (int)height));

            VideoScreen.Blit(BarBorderImage, BarBorderImagePos);

            GFXFont = gfxFont.Render(Convert.ToString(gameInfo.CascadeCount * 10, CultureInfo.CurrentCulture), Color.Red);
            GFXFontShadow = gfxFont.Render(Convert.ToString(gameInfo.CascadeCount * 10, CultureInfo.CurrentCulture), Color.DarkRed);
            ModeFont = scoreFont.Render("Game Mode: " + (gameInfo.GameMode == 1 ? "Simple" : "Timed"), Color.Black);

            VideoScreen.Blit(ModeFont, new Point(35, 570));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    DrawGem(board.data[i, j].Type, board.data[i, j].Position.X, board.data[i, j].Position.Y);

            VideoScreen.Blit(newGame);
            VideoScreen.Blit(quitGame);
            VideoScreen.Blit(switchMode);
            VideoScreen.Blit(highScores);

            if (board.clicked)
            {
                ClickedImagePos = new Point(235 + (board.sel_x * 75), board.sel_y * 75);
                VideoScreen.Blit(ClickedImage, ClickedImagePos);
            }
            switchMode = new Button();
            switchMode.SetImage(new Surface(ButtonImage), new Point(113, 410));
            if (gameInfo.GameMode == 1)
                switchMode.SetText("Timed Mode");
            else
                switchMode.SetText("Simple Mode");


            if (gameInfo.HighScores)
                DrawScores(gameInfo);

            if (gameInfo.GameOver)
                if (gameInfo.IsRetired)
                    DrawGameText("GAME RETIRED!", 300, 260);
                else
                    if (gameInfo.GameMode == 2)
                        DrawGameText("TIME UP!", 360, 260);
                    else
                        DrawGameText("NO MORE MOVES!", 285, 260);

            if (gameInfo.LevelUp && gameInfo.GameMode == 1)
                DrawGameText("LEVEL " + gameInfo.Level + "!", 400, 260);

            if (gameInfo.Hint >= 15 && gameInfo.IsRunning && gameInfo.ShowHint)
            {
                DrawHint(board.hint_x, board.hint_y);
            }

            VideoScreen.Update();
        }

        private void DrawGem(int type, int x, int y)
        {
            switch (type)
            {
                case 0:
                    VideoScreen.Blit(BlankImage, new Point(x, y));
                    break;
                case 1:
                    VideoScreen.Blit(Gem1, new Point(x, y));
                    break;
                case 2:
                    VideoScreen.Blit(Gem2, new Point(x, y));
                    break;
                case 3:
                    VideoScreen.Blit(Gem3, new Point(x, y));
                    break;
                case 4:
                    VideoScreen.Blit(Gem4, new Point(x, y));
                    break;
                case 5:
                    VideoScreen.Blit(Gem5, new Point(x, y));
                    break;
                case 6:
                    VideoScreen.Blit(Gem6, new Point(x, y));
                    break;
                case 7:
                    VideoScreen.Blit(Gem7, new Point(x, y));
                    break;
                case 8:
                    VideoScreen.Blit(Sparkle, new Point(x, y), new Rectangle(new Point(0, 0), new Size(75, 75)));
                    VideoScreen.Blit(GFXFontShadow, new Point(x + 41, y + 41));
                    VideoScreen.Blit(GFXFont, new Point(x + 40, y + 40));
                    break;
                case 9:
                    VideoScreen.Blit(Sparkle, new Point(x, y), new Rectangle(new Point(75, 0), new Size(75, 75)));
                    VideoScreen.Blit(GFXFontShadow, new Point(x + 41, y + 35));
                    VideoScreen.Blit(GFXFont, new Point(x + 40, y + 34));
                    break;
            }
        }

        private void DrawScores(GameInfo gameInfo)
        {
            VideoScreen.Blit(ScoresWindow, new Point(300, 75));
            VideoScreen.Blit(exitButton);
            Surface HighScoresFont;
            int adjustment = gameInfo.GameMode * 10 - 10;
            for (int i = 0; i < 10; i++)
            {
                string leadingSpace = "  ";
                if (i == 9)
                    leadingSpace = "";
                string scoreText = leadingSpace + (i + 1) + ": " + gameInfo.TopTenNames[i + adjustment] + " - " + gameInfo.TopTenScores[i + adjustment];
                if (i == gameInfo.Place)
                    HighScoresFont = scoreFont.Render(scoreText, Color.Red);
                else
                    HighScoresFont = scoreFont.Render(scoreText, Color.Blue);
                VideoScreen.Blit(HighScoresFont, new Point(365, 160 + (i * 18)));
            }
        }

        private void DrawGameText(string gameText, int x, int y)
        {
            GameFont = gameFont.Render(gameText, Color.Red);
            GameFontShadow = gameFont.Render(gameText, Color.DarkRed);
            VideoScreen.Blit(GameFontShadow, new Point(x + 2, y + 2));
            VideoScreen.Blit(GameFont, new Point(x, y));
        }

        private void DrawHint(int x, int y)
        {
            ClickedImagePos = new Point(235 + (x * 75), y * 75);
            VideoScreen.Blit(HintImage, ClickedImagePos);
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
                if (infoFont != null)
                {
                    infoFont.Dispose();
                    infoFont = null;
                }
                if (gameFont != null)
                {
                    gameFont.Dispose();
                    gameFont = null;
                }
                if (scoreFont != null)
                {
                    scoreFont.Dispose();
                    scoreFont = null;
                }
                if (gfxFont != null)
                {
                    gfxFont.Dispose();
                    gfxFont = null;
                }
                if (newGame != null)
                {
                    newGame.Dispose();
                    newGame = null;
                }
                if (exitButton != null)
                {
                    exitButton.Dispose();
                    exitButton = null;
                }
                if (highScores != null)
                {
                    highScores.Dispose();
                    highScores = null;
                }
                if (quitGame != null)
                {
                    quitGame.Dispose();
                    quitGame = null;
                }
                if (switchMode != null)
                {
                    switchMode.Dispose();
                    switchMode = null;
                }
                if (SoundImage != null)
                {
                    SoundImage.Dispose();
                    SoundImage = null;
                }
                if (SoundOffImage != null)
                {
                    SoundOffImage.Dispose();
                    SoundOffImage = null;
                }
            }
        }

        #endregion
    }
}
