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

            foreach (var projectile in _projectiles)
                projectile.Draw(spriteBatch);
        }

        public void Update(float elapsedTime, float screenWidth, float screenHeight, float topBoundary)
        {
            base.Update(elapsedTime, screenWidth, screenHeight);

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

            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                if (_projectiles[i].Y <= 0)
                    _projectiles.Remove(_projectiles[i]);
                else
                    _projectiles[i].Update(elapsedTime, screenWidth, screenHeight);
            }

            if (blinkingStart > DateTime.MinValue && (DateTime.Now - blinkingStart).TotalSeconds > 5)
            {
                blinking = false;
                blinkingStart = DateTime.MinValue;
            }
        }

        public bool CheckCollisions(Collection<Projectile> projectiles)
        {
            if (blinking) return false;

            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                if (this.RectangleCollision(projectiles[i]))
                {
                    Health -= projectiles[i].Strength;
                    projectiles.RemoveAt(i);

                    if (Health <= 0)
                    {
                        blinking = true;
                        blinkingStart = DateTime.Now;
                        return true;
                        //TODO: EXPLOSION!
                    }
                }
            }
            return false;
        }

        public bool CheckCollisions(Collection<Ship> ships)
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
                        blinking = true;
                        blinkingStart = DateTime.Now;
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

        public Collection<Projectile> Projectiles
        {
            get { return _projectiles; }
        }
    }
}
