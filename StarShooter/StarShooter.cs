using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarShooter.GameElements;
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

        float shipSpeedX;
        float shipSpeedY;
        float starSpeed;
        int score;

        float swarmSize = .6f;
        float shipSize = .8f;

        Random random;

        public StarShooter()
        {
            ApplicationView.PreferredLaunchViewSize = new Size(1200, 900);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

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

            // TODO: use this.Content to load your game content here
            ship = new Ship(Content.Load<Texture2D>("ship"), Content.Load<Texture2D>("lazer"), Content.Load<Texture2D>("swarm_explode"), ScaleToHighDPI(shipSize), ScaleToHighDPI(shipSize));
            ship.Height = 125; ship.Width = 100;
                
            swarm = new ShipCollection(Content.Load<Texture2D>("swarm"), Content.Load<Texture2D>("lazer"), Content.Load<Texture2D>("swarm_explode"), ScaleToHighDPI(swarmSize), ScaleToHighDPI(swarmSize));
            starsFront = new RollingSprite(Content.Load<Texture2D>("starsFront"), ScaleToHighDPI(1.1f), ScaleToHighDPI(1f), screenWidth, screenHeight);
            starsBack = new RollingSprite(Content.Load<Texture2D>("starsBack"), ScaleToHighDPI(1.1f), ScaleToHighDPI(1f), screenWidth, screenHeight);
            lives = new LivesPanel(Content.Load<Texture2D>("ship"), ScaleToHighDPI(.4f), ScaleToHighDPI(.4f));
            scoreFont = Content.Load<SpriteFont>("Score");
            stateFont = Content.Load<SpriteFont>("GameState");
            stateFontMedium = Content.Load<SpriteFont>("GameStateMedium");
            gameOverTexture = Content.Load<Texture2D>("game-over");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

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
                lives.Draw(spriteBatch, screenWidth);
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

        protected override void Update(GameTime gameTime)
        {     
            // Handle keyboard input
            KeyboardHandler(gameTime); 

            // Update animated SpriteClass objects based on their current rates of change

            UpdateColorBlink();

            starsFront.Update(gameTime, starSpeed);
            starsBack.Update(gameTime, (starSpeed *.6f));

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
                    StartGame(gameTime);
                    gameStarted = true;
                    gameOver = false;
                    spaceDown = true;
                }
                return;
            }

            if (gameOver && state.IsKeyDown(Keys.Enter))
            {
                StartGame(gameTime);
                gameOver = false;
            }

            if (gameOver)
                return;

            if (state.IsKeyDown(Keys.Space))
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

            ship.SetPosition(ship.X, ship.Y, directionX, directionY);
        }
        double gameStart = 0;
        public void StartGame(GameTime gameTime)
        {
            score = 0;
            ship.X = screenWidth / 2;
            ship.Y = screenHeight * shipBoundary;

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
