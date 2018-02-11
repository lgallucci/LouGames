using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Graphics;

namespace StarShooter
{
    public class Ship : SpriteClass
    {
        Collection<Projectile> _projectiles;

        

        public Ship(GraphicsDevice graphicsDevice, Texture2D texture, float scale)
            : base(graphicsDevice, texture, scale)
        {
            _projectiles = new Collection<Projectile>();
        }

        public void Update(float elapsedTime, float screenWidth, float screenHeight, float topBoundary)
        {
            base.Update(elapsedTime);

            if (this.X > screenWidth - this.Texture.Width / 2)
            {
                this.X = screenWidth - this.Texture.Width / 2;
                this.DX = 0;
            }

            // Set left edge
            if (this.X < 0 + this.Texture.Width / 2)
            {
                this.X = 0 + this.Texture.Width / 2;
                this.DX = 0;
            }

            if (this.Y < topBoundary)
            {
                this.Y = topBoundary;
                this.DY = 0;
            }

            if (this.Y > screenHeight - this.Texture.Height / 2)
            {
                this.Y = screenHeight - this.Texture.Height / 2;
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
