using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarShooter.GameElements
{
    public enum ShipDirection
    {
        Left = 0,
        Middle = 1,
        Right = 2
    }

    public class Ship : MoveOnResizeSprite
    {
        float projectileSpeed;

        public Texture2D LazerTexture { get; private set; }
        public Texture2D ExplosionTexture { get; private set; }

        float projectileSize = .5f;

        private bool blinking;
        double blinkTime = 0;

        public Ship(Texture2D texture, Texture2D lazerTexture, Texture2D explosionTexture, float scaleX, float scaleY)
            : base(texture, scaleX, scaleY)
        {
            Projectiles = new List<Projectile>();

            projectileSpeed = StarShooter.ScaleToHighDPI(150f);
            LazerTexture = lazerTexture;
            ExplosionTexture = explosionTexture;
        }

        public void Draw(SpriteBatch spriteBatch, float alpha, ShipDirection direction)
        {
            Rectangle? sourceRectangle = null;

            if (!Enemy)
            {
                if (direction == ShipDirection.Left)
                {
                    sourceRectangle = new Rectangle(0, 0, (int)this.Width, (int)this.Height);
                }
                else if (direction == ShipDirection.Middle)
                {
                    sourceRectangle = new Rectangle((int)this.Width, 0, (int)this.Width, (int)this.Height);
                }
                else
                {
                    sourceRectangle = new Rectangle((int)this.Width * 2, 0, (int)this.Width, (int)this.Height);
                }
            }

            this.Draw(spriteBatch, blinking ? Color.White * (alpha / 255) : Color.White, sourceRectangle);

            foreach (var projectile in Projectiles)
                projectile.Draw(spriteBatch);
        }

        public override void UpdateScale(float scaleX, float scaleY, float screenWidth, float screenHeight)
        {
            foreach (var projectile in Projectiles)
                projectile.UpdateScale(StarShooter.ScaleToHighDPI(projectileSize * scaleX), StarShooter.ScaleToHighDPI(projectileSize * scaleY), screenWidth, screenHeight);

            base.UpdateScale(scaleX, scaleY, screenWidth, screenHeight);
        }

        public void Update(GameTime gameTime, float screenWidth, float screenHeight, float topBoundary)
        {
            base.Update(gameTime, screenWidth, screenHeight);

            if (this.X > screenWidth - this.Width / 2)
            {
                this.X = screenWidth - this.Width / 2;
                this.DX = 0;
            }

            // Set left edge
            if (this.X < 0 + this.Width / 2)
            {
                this.X = 0 + this.Width / 2;
                this.DX = 0;
            }

            if (this.Y < topBoundary)
            {
                this.Y = topBoundary;
                this.DY = 0;
            }

            if (this.Y > screenHeight - this.Height / 2)
            {
                this.Y = screenHeight - this.Height / 2;
                this.DY = 0;
            }

            CreateProjectiles(gameTime);

            for (int i = Projectiles.Count - 1; i >= 0; i--)
            {
                if (Projectiles[i].Y <= 0)
                    Projectiles.Remove(Projectiles[i]); //_projectiles[i].Active = false;
                else
                    Projectiles[i].Update(gameTime, screenWidth, screenHeight);
            }

            if (blinkTime > 0 && (gameTime.TotalGameTime.TotalSeconds - blinkTime) > 5)
            {
                blinkTime = 0;
                blinking = false;
            }
        }

        int projectileInterval = 500;
        double lastProjectile = 0;
        int projectileCount = 1;
        private void CreateProjectiles(GameTime gameTime)
        {
            if (Projectiles.Count < projectileCount && (lastProjectile + projectileInterval) < gameTime.TotalGameTime.TotalMilliseconds)
            {
                var projectile = new Projectile(this.LazerTexture, StarShooter.ScaleToHighDPI(projectileSize * ScaleX), StarShooter.ScaleToHighDPI(projectileSize * ScaleY));

                projectile.SetPosition(X, Y - (Height / 2), 0, -3 * projectileSpeed);
                Projectiles.Add(projectile);
                lastProjectile = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public bool CheckCollisions(List<Projectile> projectiles, GameTime gameTime)
        {
            if (blinking) return false;

            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                if (this.RectangleCollision(projectiles[i]))
                {
                    Health -= projectiles[i].Strength;
                    projectiles.Remove(projectiles[i]);

                    if (Health <= 0)
                    {
                        blinkTime = gameTime.TotalGameTime.TotalSeconds;
                        blinking = true;
                        return true;
                        //TODO: EXPLOSION!
                    }
                }
            }
            return false;
        }

        public bool CheckCollisions(List<Ship> ships, GameTime gameTime)
        {
            if (blinking) return false;

            for (int i = ships.Count - 1; i >= 0; i--)
            {
                if (this.RectangleCollision(ships[i]))
                {
                    Health -= 100;// projectiles[i].Strength;
                    ships.RemoveAt(i);

                    if (Health <= 0)
                    {
                        blinkTime = gameTime.TotalGameTime.TotalSeconds;
                        blinking = true;
                        return true;
                        //TODO: EXPLOSION!
                    }
                }
            }
            return false;
        }

        public void ResetHealth()
        {
            Health = 100;
        }

        public float Health
        {
            get;
            set;
        }

        public bool Enemy { get; set; }

        public List<Projectile> Projectiles { get; }

        internal void UpdateProjectiles(int score)
        {
            if (score < 250 && projectileCount != 1) projectileCount = 2; projectileInterval = (1000 / projectileCount);
            if (score > 250 && projectileCount == 1) projectileCount = 2; projectileInterval = (1000 / projectileCount);
            if (score > 1000 && projectileCount == 2) projectileCount = 3; projectileInterval = (1000 / projectileCount);
            if (score > 1500 && projectileCount == 3) projectileCount = 4; projectileInterval = (1500 / projectileCount);
            if (score > 2000 && projectileCount == 4) projectileCount = 5; projectileInterval = (1500 / projectileCount);
        }
    }
}
