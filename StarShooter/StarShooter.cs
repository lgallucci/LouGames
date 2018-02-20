using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace StarShooter
{
    public class StarShooter : Game
    {
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

        SpriteFont scoreFont;
        SpriteFont stateFont;
        SpriteFont stateFontMedium;
        Texture2D gameOverTexture;
        bool gameOver;

        bool spaceDown;
        bool paused;
        ShipDirection shipDirection;
        bool gameStarted;

        float projectileSpeed;
        float shipSpeedX;
        float shipSpeedY;
        float starSpeed;
        int score;

        Random random;

        public StarShooter()
        {
            ApplicationView.PreferredLaunchViewSize = new Size(1200, 900);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            projectileSpeed = ScaleToHighDPI(150f);
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

            ship.UpdateScale(ScaleToHighDPI(.8f * scaleX), ScaleToHighDPI(.8f * scaleY), screenWidth, screenHeight);

            foreach (var projectile in ship.Projectiles)
                projectile.UpdateScale(ScaleToHighDPI(.5f * scaleX), ScaleToHighDPI(.5f * scaleY), screenWidth, screenHeight);

            swarm.UpdateScale(ScaleToHighDPI(1f * scaleX), ScaleToHighDPI(1f * scaleY), screenWidth, screenHeight);
            
            foreach (var projectile in swarm.Projectiles)
                projectile.UpdateScale(ScaleToHighDPI(.5f * scaleX), ScaleToHighDPI(.5f * scaleY), screenWidth, screenHeight);

            starsFront.UpdateScale(ScaleToHighDPI(1.1f * scaleX), ScaleToHighDPI(1f * scaleY), screenWidth, screenHeight);
            starsBack.UpdateScale(ScaleToHighDPI(1.1f * scaleX), ScaleToHighDPI(1f * scaleY), screenWidth, screenHeight);
            lives.UpdateScale(ScaleToHighDPI(.4f * scaleX), ScaleToHighDPI(.4f * scaleY));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ship = new Ship(Content.Load<Texture2D>("ship"), ScaleToHighDPI(.8f), ScaleToHighDPI(.8f));
            swarm = new ShipCollection(Content.Load<Texture2D>("swarm"), ScaleToHighDPI(1f), ScaleToHighDPI(1f));
            starsFront = new RollingSprite(Content.Load<Texture2D>("starsFront"), ScaleToHighDPI(1.1f), ScaleToHighDPI(1f), screenWidth, screenHeight);
            starsBack = new RollingSprite(Content.Load<Texture2D>("starsBack"), ScaleToHighDPI(1.1f), ScaleToHighDPI(1f), screenWidth, screenHeight);
            lives = new LivesPanel(Content.Load<Texture2D>("ship"), ScaleToHighDPI(.4f), ScaleToHighDPI(.4f));
            scoreFont = Content.Load<SpriteFont>("Score");
            stateFont = Content.Load<SpriteFont>("GameState");
            stateFontMedium = Content.Load<SpriteFont>("GameStateMedium");
            gameOverTexture = Content.Load<Texture2D>("game-over");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            starsFront.Draw(spriteBatch);
            starsBack.Draw(spriteBatch);

            if (!gameStarted)
            {
                String title = "Star    Shooter";
                String pressSpace = "Press Space to start";

                // Measure the size of text in the given font
                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = scoreFont.MeasureString(pressSpace);

                // Draw the text horizontally centered
                spriteBatch.DrawString(stateFont, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), Color.Fuchsia);
                spriteBatch.DrawString(scoreFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2, screenHeight / 2), Color.White * ((float)alpha / 255));
            }
            else
            {
                lives.Draw(spriteBatch, new Rectangle(100, 0, 100, 125), screenWidth);
                swarm.Draw(spriteBatch);
                ship.Draw(spriteBatch, alpha, shipDirection);

                var scoreString = ((int)score).ToString("d5");
                Vector2 scoreSize = scoreFont.MeasureString(scoreString);

                spriteBatch.DrawString(scoreFont, scoreString, new Vector2(10, 10), Color.White);
            }

            if (paused)
            {
                String title = "Paused";
                String pressSpace = "Press Space to resume";

                // Measure the size of text in the given font
                Vector2 titleSize = stateFontMedium.MeasureString(title);
                Vector2 pressSpaceSize = scoreFont.MeasureString(pressSpace);

                // Draw the text horizontally centered
                spriteBatch.DrawString(stateFontMedium, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), Color.Fuchsia);
                spriteBatch.DrawString(scoreFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2, screenHeight / 2), Color.White * ((float)alpha / 255));
            }

            if (gameOver)
            {
                // Draw game over texture
                spriteBatch.Draw(gameOverTexture, new Vector2(screenWidth / 2 - gameOverTexture.Width / 2, screenHeight / 4 - gameOverTexture.Width / 2), Color.White);
                String pressEnter = "Press Enter to restart!";
                String yourScore = $"Your score: {score.ToString("d5")}";
                // Measure the size of text in the given font
                Vector2 pressEnterSize = scoreFont.MeasureString(pressEnter);
                Vector2 yourScoreSize = scoreFont.MeasureString(yourScore);
                // Draw the text horizontally centered
                spriteBatch.DrawString(scoreFont, pressEnter, new Vector2(screenWidth / 2 - pressEnterSize.X / 2, screenHeight - 200), Color.White * ((float)alpha / 255));
                spriteBatch.DrawString(scoreFont, yourScore, new Vector2(screenWidth / 2 - yourScoreSize.X / 2, screenHeight - 500), Color.White);
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            KeyboardHandler(); // Handle keyboard input
                                          // Update animated SpriteClass objects based on their current rates of change

            UpdateColorBlink();

            starsFront.Update(elapsedTime * starSpeed);
            starsBack.Update(elapsedTime * (starSpeed *.6f));

            if (!gameStarted || gameOver || paused)
                return;

            swarm.Update(elapsedTime, screenHeight);
            ship.Update(elapsedTime, screenWidth, screenHeight, shipBoundary);
            lives.Update(elapsedTime);
            swarm.CheckCollisions(ship.Projectiles);
            ship.CheckCollisions(swarm.Projectiles);

            if (ship.Health <= 0)
            {
                lives.Lives -= 1;
                if (lives.Lives > 0)
                    ship.ResetHealth();
                else
                    gameOver = true;
            }

            CreateProjectiles(screenWidth);

            base.Update(gameTime);
        }

        private void CreateProjectiles(float screenWidth)
        {
            var gameTimeSpan = DateTime.Now - gameStart;

            float scaleX = screenWidth / originalWidth;
            float scaleY = screenHeight / originalHeight;

            int projectileCount = ((int)gameTimeSpan.TotalSeconds / 5) > 100 ? 100 : ((int)gameTimeSpan.TotalSeconds / 5);

            if (swarm.Projectiles.Count < projectileCount + 1)
            {
                var projectile = new Projectile(Content.Load<Texture2D>("lazer"), ScaleToHighDPI(.5f * scaleX), ScaleToHighDPI(.5f * scaleY));

                int maxProjectileSpeed = (int)(projectileSpeed + (float)gameTimeSpan.TotalSeconds);

                var projectileX = random.Next(10, (int)screenWidth - 10);
                var projectileY = random.Next((int)projectileSpeed, maxProjectileSpeed > 2000 ? 2000 : maxProjectileSpeed);
                projectile.SetPosition(projectileX, 0, 0, projectileY, screenWidth, screenHeight);
                swarm.Projectiles.Add(projectile);
                score++;
            }
        }

        int alphaDirection = 0;
        int alpha = 0;
        private DateTime gameStart;

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

        void KeyboardHandler()
        {
            KeyboardState state = Keyboard.GetState();

            // Quit the game if Escape is pressed.
            if (state.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            
            // Start the game if Space is pressed.
            if (!gameStarted)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    StartGame();
                    gameStarted = true;
                    gameOver = false;
                    spaceDown = true;
                }
                return;
            }

            if (gameOver && state.IsKeyDown(Keys.Enter))
            {
                StartGame();
                gameOver = false;
            }

            if (gameOver)
                return;

            // Jump if Space is pressed
            if (state.IsKeyDown(Keys.Space))
            {
                if (!spaceDown)
                {
                    spaceDown = true;
                    paused = !paused;
                }
            }
            else
                spaceDown = false;

            if (paused)
                return;

            float directionX, directionY;
            // Handle left and right
            if (state.IsKeyDown(Keys.A))
            {
                directionX = shipSpeedX * -1;
                shipDirection = ShipDirection.Left;
            }
            else if (state.IsKeyDown(Keys.D))
            {
                directionX = shipSpeedX;
                shipDirection = ShipDirection.Right;
            }
            else
            {
                directionX = 0;
                shipDirection = ShipDirection.Middle;
            }

            if (state.IsKeyDown(Keys.W))
                directionY = shipSpeedY * -1;
            else if (state.IsKeyDown(Keys.S))
                directionY = shipSpeedY;
            else
                directionY = 0;

            ship.SetPosition(ship.X, ship.Y, directionX, directionY, screenWidth, screenHeight);
        }

        public void StartGame()
        {
            score = 0;
            ship.X = screenWidth / 2;
            ship.Y = screenHeight * shipBoundary;

            ship.Health = 100;
            lives.Lives = 3;

            swarm.Projectiles.Clear();
            gameStart = DateTime.Now;
        }

        public void StartLevel()
        {
        }

        public float ScaleToHighDPI(float f)
        {
            DisplayInformation d = DisplayInformation.GetForCurrentView();
            f *= (float)d.RawPixelsPerViewPixel;
            return f;
        }
    }
}
