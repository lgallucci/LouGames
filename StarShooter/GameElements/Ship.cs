using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarShooter
{
    public enum ShipDirection
    {
        Left = 0,
        Middle = 1,
        Right = 2
    }

    public class Ship : MoveOnResizeSprite
    {
        Collection<Projectile> _projectiles;
        private bool blinking;
        private DateTime blinkingStart = DateTime.MinValue;

        public Ship(Texture2D texture, float scaleX, float scaleY)
            : base(texture, scaleX, scaleY)
        {
            _projectiles = new Collection<Projectile>();
            Height = 125;
            Width = 100;
        }

        public void Draw(SpriteBatch spriteBatch, float alpha, ShipDirection direction)
        {
            Rectangle sourceRectangle;
            if (direction == ShipDirection.Left)
            {
                sourceRectangle = new Rectangle(0, 0, 100, 125);
            }
            else if (direction == ShipDirection.Middle)
            {
                sourceRectangle = new Rectangle(100, 0, 100, 125);
            }
            else
            {
                sourceRectangle = new Rectangle(200, 0, 100, 125);
            }
            this.Draw(spriteBatch, blinking ? Color.White * (alpha / 255) : Color.White, sourceRectangle);
        }

        public void Update(float elapsedTime, float screenWidth, float screenHeight, float topBoundary)
        {
            base.Update(elapsedTime);

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

            if (blinkingStart > DateTime.MinValue && (DateTime.Now - blinkingStart).TotalSeconds > 5)
            {
                blinking = false;
                blinkingStart = DateTime.MinValue;
            }
        }

        public void UpdateScale(float scaleX, float scaleY, int screenWidth)
        {
            foreach (var projectile in _projectiles)
                projectile.UpdateScale(scaleX, scaleY, screenWidth);

            base.UpdateScale(scaleX, scaleY, screenWidth);
        }

        public void CheckCollisions(Collection<Projectile> projectiles)
        {
            if (blinking) return;

            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                if (this.RectangleCollision(projectiles[i]))
                {
                    Health -= projectiles[i].Strength;
                    if (Health <= 0)
                    {
                        projectiles.RemoveAt(i);
                        blinking = true;
                        blinkingStart = DateTime.Now;
                    }
                    //TODO: EXPLOSION!
                }
            }
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

        public Collection<Projectile> Projectiles
        {
            get { return _projectiles; }
        }
    }
}
