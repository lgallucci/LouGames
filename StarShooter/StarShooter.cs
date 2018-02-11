﻿using Microsoft.Xna.Framework;
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
        bool gameStarted;

        float projectileSpeed;
        float shipSpeedX;
        float shipSpeedY;
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
            projectileSpeed = ScaleToHighDPI(30f);
            gameOver = false;

            base.Initialize();
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
                String title = "Star   Shooter";
                String pressSpace = "Press Space to start";

                // Measure the size of text in the given font
                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = scoreFont.MeasureString(pressSpace);

                // Draw the text horizontally centered
                spriteBatch.DrawString(stateFont, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), Color.Fuchsia);
                spriteBatch.DrawString(scoreFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2, screenHeight / 2), new Color(255, 255, 255, alpha));
            }
            else
            {
                swarm.Draw(spriteBatch);

                ship.Draw(spriteBatch);

                var scoreString = ((int)score).ToString("d5");
                Vector2 scoreSize = stateFont.MeasureString(scoreString);

                spriteBatch.DrawString(scoreFont, scoreString, new Vector2(10, 10), Color.White);
            }

            if (gameOver)
            {
                // Draw game over texture
                spriteBatch.Draw(gameOverTexture, new Vector2(screenWidth / 2 - gameOverTexture.Width / 2, screenHeight / 4 - gameOverTexture.Width / 2), Color.White);

                String pressEnter = "Press Enter to restart!";

                // Measure the size of text in the given font
                Vector2 pressEnterSize = stateFont.MeasureString(pressEnter);

                // Draw the text horizontally centered
                spriteBatch.DrawString(stateFont, pressEnter, new Vector2(screenWidth / 2 - pressEnterSize.X / 2, screenHeight - 200), Color.White);
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

            if (!gameStarted)
                return;
                               
            if (gameOver)
            {
                ship.Freeze();
                swarm.Freeze();
            }

            swarm.Update(elapsedTime);
            ship.Update(elapsedTime, screenWidth, screenHeight, shipBoundary);
            starsFront.Update(elapsedTime * 100);
            starsBack.Update(elapsedTime * 50);

            swarm.CheckCollisions(ship.Projectiles);
            ship.CheckCollisions(swarm.Projectiles);

            score = (int)gameTime.TotalGameTime.TotalSeconds / 5;

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

            // Jump if Space is pressed
            if (state.IsKeyDown(Keys.Space))
            {
                //TODO: PAUSE!
                spaceDown = true;
            }
            else spaceDown = false;

            // Handle left and right
            if (state.IsKeyDown(Keys.A))
                ship.DX = shipSpeedX * -1;
            else if (state.IsKeyDown(Keys.D))
                ship.DX = shipSpeedX;
            else
                ship.DX = 0;

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
