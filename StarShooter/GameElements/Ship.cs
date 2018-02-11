﻿using System.Collections.ObjectModel;
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

    public class Ship : SpriteClass
    {
        Collection<Projectile> _projectiles;

        public Ship(GraphicsDevice graphicsDevice, Texture2D texture, float scale)
            : base(graphicsDevice, texture, scale)
        {
            _projectiles = new Collection<Projectile>();
            Height = 125;
            Width = 100;
        }

        public void Draw(SpriteBatch spriteBatch, ShipDirection direction)
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
            this.Draw(spriteBatch, sourceRectangle);
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
        }

        public void CheckCollisions(Collection<Projectile> projectiles)
        {
            foreach (var projectile in projectiles)
                this.RectangleCollision(projectile);
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
