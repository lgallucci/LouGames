using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace StarShooter
{
    public class StarShooter : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        float screenWidth;
        float screenHeight;
        float shipBoundary;

        RollingSprite starsFront;
        RollingSprite starsBack;
        Ship ship;
        ShipCollection swarm;

        SpriteFont scoreFont;
        SpriteFont stateFont;
        Texture2D gameOverTexture;
        bool gameOver;

        bool spaceDown;
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
            base.Initialize();

            // TODO: Add your initialization logic here
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            screenHeight = (float)ApplicationView.GetForCurrentView().VisibleBounds.Height;
            screenWidth = (float)ApplicationView.GetForCurrentView().VisibleBounds.Width;
            shipBoundary = screenHeight * .75F;

            gameStarted = false;
            score = 0;
            random = new Random();
            shipSpeedX = ScaleToHighDPI(600f);
            shipSpeedY = ScaleToHighDPI(600f);
            projectileSpeed = ScaleToHighDPI(150f);
            starSpeed = ScaleToHighDPI(100f);
            gameOver = false;

            this.IsMouseVisible = true; // Hide the mouse within the app window
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
            ship = new Ship(GraphicsDevice, Content.Load<Texture2D>("ship"), ScaleToHighDPI(.8f));
            swarm = new ShipCollection(GraphicsDevice, Content.Load<Texture2D>("swarm"), ScaleToHighDPI(1f));
            starsFront = new RollingSprite(GraphicsDevice, Content.Load<Texture2D>("starsFront"), ScaleToHighDPI(1f));
            starsBack = new RollingSprite(GraphicsDevice, Content.Load<Texture2D>("starsBack"), ScaleToHighDPI(1f));

            scoreFont = Content.Load<SpriteFont>("Score");
            stateFont = Content.Load<SpriteFont>("GameState");
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
                swarm.Draw(spriteBatch);
                ship.Draw(spriteBatch, alpha, shipDirection);

                var scoreString = ((int)score).ToString("d5");
                Vector2 scoreSize = stateFont.MeasureString(scoreString);

                spriteBatch.DrawString(scoreFont, scoreString, new Vector2(10, 10), Color.White);
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
            starsBack.Update(elapsedTime * (starSpeed / 2));

            if (!gameStarted || gameOver)
                return;

            swarm.Update(elapsedTime, screenHeight);
            ship.Update(elapsedTime, screenWidth, screenHeight, shipBoundary);

            swarm.CheckCollisions(ship.Projectiles);
            ship.CheckCollisions(swarm.Projectiles);

            //score = (int)gameTime.TotalGameTime.TotalSeconds / 5;

            CreateProjectiles(screenWidth);

            if (ship.Health <= 0)
                gameOver = true;

            base.Update(gameTime);
        }

        private void CreateProjectiles(float screenWidth)
        {
            var gameTimeSpan = DateTime.Now - gameStart;
            int projectileCount = (int)gameTimeSpan.TotalSeconds / 10;

            if (swarm.Projectiles.Count < projectileCount + 1)
            {
                var projectile = new Projectile(GraphicsDevice, Content.Load<Texture2D>("lazer"), ScaleToHighDPI(.8f));

                int maxProjectileSpeed = (int)(projectileSpeed + (float)gameTimeSpan.TotalSeconds);

                projectile.Y = 0; projectile.DY = random.Next((int)projectileSpeed, maxProjectileSpeed > 2000 ? 2000 : maxProjectileSpeed);
                projectile.X = random.Next(10, (int)screenWidth - 10);
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
                    spaceDown = true;
                    gameOver = false;
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
                //TODO: MENU!
                //ship.Freeze();
                //swarm.Freeze();
                spaceDown = true;
            }
            else spaceDown = false;

            // Handle left and right
            if (state.IsKeyDown(Keys.A))
            {
                ship.DX = shipSpeedX * -1;
                shipDirection = ShipDirection.Left;
            }
            else if (state.IsKeyDown(Keys.D))
            {
                ship.DX = shipSpeedX;
                shipDirection = ShipDirection.Right;
            }
            else
            {
                ship.DX = 0;
                shipDirection = ShipDirection.Middle;
            }

            if (state.IsKeyDown(Keys.W))
                ship.DY = shipSpeedY * -1;
            else if (state.IsKeyDown(Keys.S))
                ship.DY = shipSpeedY;
            else
                ship.DY = 0;
        }

        public void StartGame()
        {
            score = 0;
            ship.X = screenWidth / 2;
            ship.Y = screenHeight * shipBoundary;
            ship.Health = 100;
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
