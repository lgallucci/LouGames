using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarShooter.GameElements;
using StarShooter.Particles;
using StarShooter.Utils;
using System;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace StarShooter
{
    public class GameRoot : Game
    {
        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static ParticleManager<ParticleState> ParticleManager { get; private set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        float originalWidth;
        float originalHeight;

        float screenWidth;
        float screenHeight;
        float shipBoundary;

        RollingSprite starsFront;
        RollingSprite starsBack;
        Ship ship;
        ShipCollection swarm;
        LivesPanel lives;

        bool gameOver;

        bool spaceDown;
        bool paused;
        bool gameStarted;

        float shipSpeedX;
        float shipSpeedY;
        float starSpeed;
        int score;

        float swarmSize = .6f;
        float shipSize = .8f;

        Random random;

        public GameRoot()
        {
            Instance = this;
            ApplicationView.PreferredLaunchViewSize = new Size(1200, 900);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            ParticleManager = new ParticleManager<ParticleState>(1024 * 20, ParticleState.UpdateParticle);

            screenHeight = (float)ApplicationView.GetForCurrentView().VisibleBounds.Height;
            originalHeight = screenHeight;
            screenWidth = (float)ApplicationView.GetForCurrentView().VisibleBounds.Width;
            originalWidth = screenWidth;
            shipBoundary = screenHeight * .6F;

            gameStarted = false;
            score = 0;
            random = new Random();
            shipSpeedX = ScaleToHighDPI(400f);
            shipSpeedY = ScaleToHighDPI(400f);
            starSpeed = ScaleToHighDPI(40f);
            gameOver = false;

            this.IsMouseVisible = true; // Hide the mouse within the app window

            this.Window.ClientSizeChanged += Window_ClientSizeChanged;

            base.Initialize();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            screenHeight = (float)ApplicationView.GetForCurrentView().VisibleBounds.Height;
            screenWidth = (float)ApplicationView.GetForCurrentView().VisibleBounds.Width;
            shipBoundary = screenHeight * .6F;

            float scaleX = screenWidth / originalWidth;
            float scaleY = screenHeight / originalHeight;

            ship.UpdateScale(ScaleToHighDPI(shipSize * scaleX), ScaleToHighDPI(shipSize * scaleY), screenWidth, screenHeight);

            swarm.UpdateScale(ScaleToHighDPI(swarmSize * scaleX), ScaleToHighDPI(swarmSize * scaleY), screenWidth, screenHeight);            

            starsFront.UpdateScale(ScaleToHighDPI(1.1f * scaleX), ScaleToHighDPI(1f * scaleY), screenWidth, screenHeight);
            starsBack.UpdateScale(ScaleToHighDPI(1.1f * scaleX), ScaleToHighDPI(1f * scaleY), screenWidth, screenHeight);
            lives.UpdateScale(ScaleToHighDPI(.4f * scaleX), ScaleToHighDPI(.4f * scaleY));
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);

            // TODO: use this.Content to load your game content here
            ship = new Ship(Art.Ship, ScaleToHighDPI(shipSize), ScaleToHighDPI(shipSize));
            ship.Height = 125; ship.Width = 100;
                
            swarm = new ShipCollection(Art.Swarm, ScaleToHighDPI(swarmSize), ScaleToHighDPI(swarmSize));
            starsFront = new RollingSprite(Art.StarsFront, ScaleToHighDPI(1.1f), ScaleToHighDPI(1f), screenWidth, screenHeight);
            starsBack = new RollingSprite(Art.StarsBack, ScaleToHighDPI(1.1f), ScaleToHighDPI(1f), screenWidth, screenHeight);
            lives = new LivesPanel(Art.Ship, ScaleToHighDPI(.4f), ScaleToHighDPI(.4f));

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            var gamePad = Input.IsGamePadConnected();

            spriteBatch.Begin();

            starsFront.Draw(spriteBatch);
            starsBack.Draw(spriteBatch);

            if (!gameStarted)
            {
                string title = "Star    Shooter";
                string gamePadA = Input.IsGamePadConnected() ? " (A)" : string.Empty;
                string pressSpace = $"Press Space{gamePadA} to start";
                string leftThumbstick = Input.IsGamePadConnected() ? " or the left thumbstick" : string.Empty;
                string instructions = $"Use 'W', 'A', 'S' and 'D'{leftThumbstick} to move the ship";

                // Measure the size of text in the given font
                Vector2 titleSize = Art.StateFont.MeasureString(title);
                Vector2 pressSpaceSize = Art.ScoreFont.MeasureString(pressSpace);
                Vector2 instructionsSize = Art.ScoreFontSmall.MeasureString(instructions);

                // Draw the text horizontally centered
                spriteBatch.DrawString(Art.StateFont, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), Color.Fuchsia);
                spriteBatch.DrawString(Art.ScoreFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2, screenHeight / 2), Color.White * ((float)alpha / 255));
                spriteBatch.DrawString(Art.ScoreFontSmall, instructions, new Vector2(screenWidth / 2 - instructionsSize.X / 2, screenHeight / 1.2f), Color.White);
            }
            else
            {
                lives.Draw(spriteBatch, screenWidth);
                swarm.Draw(spriteBatch);
                ship.Draw(spriteBatch, alpha);
                
                ParticleManager.Draw(spriteBatch);

                var scoreString = ((int)score).ToString("d5");
                Vector2 scoreSize = Art.ScoreFont.MeasureString(scoreString);

                spriteBatch.DrawString(Art.ScoreFont, scoreString, new Vector2(10, 10), Color.White);
            }

            if (paused)
            {
                string title = "Paused";
                string gamePadA = Input.IsGamePadConnected() ? " (A)" : string.Empty;
                string pressSpace = $"Press Space{gamePadA} to resume";
                string leftThumbstick = Input.IsGamePadConnected() ? " or the left thumbstick" : string.Empty;
                string instructions = $"Use 'W', 'A', 'S' and 'D'{leftThumbstick} to move the ship";

                // Measure the size of text in the given font
                Vector2 titleSize = Art.StateFontMedium.MeasureString(title);
                Vector2 pressSpaceSize = Art.ScoreFont.MeasureString(pressSpace);
                Vector2 instructionsSize = Art.ScoreFontSmall.MeasureString(instructions);

                // Draw the text horizontally centered
                spriteBatch.DrawString(Art.StateFontMedium, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), Color.Fuchsia);
                spriteBatch.DrawString(Art.ScoreFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2, screenHeight / 2), Color.White * ((float)alpha / 255));
                spriteBatch.DrawString(Art.ScoreFontSmall, instructions, new Vector2(screenWidth / 2 - instructionsSize.X / 2, screenHeight / 1.2f), Color.White);
            }

            if (gameOver)
            {
                // Draw game over texture
                spriteBatch.Draw(Art.GameOver, new Vector2(screenWidth / 2 - Art.GameOver.Width / 2, screenHeight / 4 - Art.GameOver.Width / 2), Color.White);
                string gamePadStart = Input.IsGamePadConnected() ? " (start)" : string.Empty;
                String pressEnter = $"Press Enter{gamePadStart} to restart!";
                String yourScore = $"Your score: {score.ToString("d5")}";
                // Measure the size of text in the given font
                Vector2 pressEnterSize = Art.ScoreFont.MeasureString(pressEnter);
                Vector2 yourScoreSize = Art.ScoreFont.MeasureString(yourScore);
                // Draw the text horizontally centered
                spriteBatch.DrawString(Art.ScoreFont, pressEnter, new Vector2(screenWidth / 2 - pressEnterSize.X / 2, screenHeight - 200), Color.White * ((float)alpha / 255));
                spriteBatch.DrawString(Art.ScoreFont, yourScore, new Vector2(screenWidth / 2 - yourScoreSize.X / 2, screenHeight - 500), Color.White);
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            // Handle keyboard input
            KeyboardHandler(gameTime); 

            // Update animated SpriteClass objects based on their current rates of change

            UpdateColorBlink();

            starsFront.Update(gameTime, starSpeed);
            starsBack.Update(gameTime, (starSpeed *.6f));

            ParticleManager.Update();

            if (!gameStarted || gameOver || paused)
                return;

            score += swarm.Update(gameTime, screenWidth, screenHeight, gameTime.TotalGameTime.TotalSeconds - gameStart - pauseTime);
            ship.Update(gameTime, screenWidth, screenHeight, shipBoundary);
            lives.Update(gameTime);

            if (swarm.CheckCollisions(ship.Projectiles, gameTime)) score += 5;
            //ship.CheckCollisions(swarm.Projectiles);
            ship.CheckCollisions(swarm.Ships, gameTime);

            if (ship.Health <= 0)
            {
                lives.Lives -= 1;
                if (lives.Lives > 0)
                    ship.ResetHealth();
                else
                    gameOver = true;
            }

            ship.UpdateProjectiles(score);

            base.Update(gameTime);
        }

        int alphaDirection = 0;
        int alpha = 0;

        private void UpdateColorBlink()
        {
            switch (alphaDirection)
            {
                case 0:
                    alpha += 5;
                    if (alpha >= 255)
                    {
                        alpha = 255;
                        alphaDirection = 1;
                    }
                    break;
                case 1:
                    alpha -= 5;
                    if (alpha <= 0)
                    {
                        alpha = 0;
                        alphaDirection = 0;
                    }
                    break;
            }
        }

        double pauseStart = 0;
        double pauseTime = 0;
        void KeyboardHandler(GameTime gameTime)
        {
            // Quit the game if Escape is pressed.
            if (Input.WasKeyPressed(Keys.Escape))
            {
                Exit();
            }
            
            // Start the game if Space is pressed.
            if (!gameStarted)
            {
                if (Input.WasKeyPressed(Keys.Space) || Input.WasButtonPressed(Buttons.A))
                {
                    StartGame(gameTime);
                    gameStarted = true;
                    gameOver = false;
                    spaceDown = true;
                }
                return;
            }

            if (gameOver && (Input.WasKeyPressed(Keys.Enter) || Input.WasButtonPressed(Buttons.Start)))
            {
                StartGame(gameTime);
                gameOver = false;
            }

            if (gameOver)
                return;

            if (Input.WasKeyPressed(Keys.Space) || Input.WasButtonPressed(Buttons.A))
            {
                if (!spaceDown)
                {
                    spaceDown = true;
                    paused = !paused;
                    if (paused)
                        pauseStart = gameTime.TotalGameTime.TotalSeconds;
                    else
                        pauseTime = gameTime.TotalGameTime.TotalSeconds - pauseStart;
                }
            }
            else
                spaceDown = false;

            if (paused)
                return;
        }

        double gameStart = 0;
        public void StartGame(GameTime gameTime)
        {
            score = 0;
            ship.Position.X = screenWidth / 2;
            ship.Position.Y = screenHeight;

            ship.Health = 100;
            lives.Lives = 3;

            swarm.Projectiles.Clear();
            swarm.Ships.Clear();
            swarm.Explosions.Clear();
            
            ship.Projectiles.Clear();
            ship.UpdateProjectiles(score);
            gameStart = gameTime.TotalGameTime.TotalSeconds;
        }

        public void StartLevel()
        {
        }

        public static float ScaleToHighDPI(float f)
        {
            DisplayInformation d = DisplayInformation.GetForCurrentView();
            f *= (float)d.RawPixelsPerViewPixel;
            return f;
        }
    }
}
