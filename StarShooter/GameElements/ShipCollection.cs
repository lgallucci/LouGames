using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarShooter.GameElements
{
    public class ShipCollection
    {
        Random random;
        float projectileSize = .35f;

        public ShipCollection(Texture2D texture, Texture2D lazerTexture, Texture2D explosionTexture, float scaleX, float scaleY)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            this.Texture = texture;
            this.LazerTexture = lazerTexture;
            this.ExplosionTexture = explosionTexture;

            Ships = new List<Ship>();
            Projectiles = new List<Projectile>();
            Explosions = new List<Animation>();
            random = new Random();
        }

        public int Update(GameTime gameTime, float screenWidth, float screenHeight, double totalGameTime)
        {
            //foreach (var ship in _shipCollection)
            //    ship.Update(elapsedTime, screenWidth, screenHeight);

            //for (int i = _projectiles.Count - 1; i >= 0; i--)
            //{
            //    if (_projectiles[i].Y > screenHeight)
            //        _projectiles.Remove(_projectiles[i]);
            //    else
            //        _projectiles[i].Update(elapsedTime, screenWidth, screenHeight);
            //}

            var newShips = CreateShips(gameTime, screenWidth, totalGameTime);

            for (int i = Ships.Count - 1; i >= 0; i--)
            {
                if (Ships[i].Y > screenHeight)
                    Ships.Remove(Ships[i]);
                else
                    Ships[i].Update(gameTime, screenWidth, screenHeight);
            }

            for (int i = Explosions.Count - 1; i >= 0; i--)
            {
                if (!Explosions[i].Active)
                    Explosions.Remove(Explosions[i]);
                else 
                    Explosions[i].Update(gameTime);
            }

            return newShips;
        }

        float shipSpeed = StarShooter.ScaleToHighDPI(150f);

        private int CreateShips(GameTime gameTime, float screenWidth, double totalGameTime)
        {
            int score = 0;
            var enemyTicker = ((int)totalGameTime / 5);
            int enemyCount = enemyTicker > 100 ? 100 : enemyTicker;

            if (Ships.Count < enemyCount + 1)
            {
                var enemy = new Ship(this.Texture, this.LazerTexture, this.ExplosionTexture, StarShooter.ScaleToHighDPI(this.ScaleX), StarShooter.ScaleToHighDPI(this.ScaleY));
                enemy.Enemy = true; enemy.HITBOXSCALE = .9f;

                int maxEnemySpeed = (int)(shipSpeed + (float)totalGameTime);

                var enemyX = random.Next(10, (int)screenWidth - 10);
                var enemyY = random.Next((int)shipSpeed, maxEnemySpeed > 2000 ? 2000 : maxEnemySpeed);
                enemy.SetPosition(enemyX, 0, 0, enemyY);
                Ships.Add(enemy);
                score++;
            }

            return score;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var ship in Ships)
                ship.Draw(spriteBatch);

            foreach(var explosion in Explosions)
                explosion.Draw(spriteBatch);

            //foreach (var projectile in _projectiles)
            //    projectile.Draw(spriteBatch);
        }

        public bool CheckCollisions(List<Projectile> projectiles, GameTime gameTime)
        {
            bool collision = false;
            for (int i = Ships.Count - 1; i >= 0; i--)
                if (Ships[i].CheckCollisions(projectiles, gameTime))
                {
                    CreateExplosion(Ships[i].X - (Ships[i].Width / 4), Ships[i].Y - (Ships[i].Height / 4));
                    Ships.Remove(Ships[i]);
                    collision = true;
                }
            return collision;
        }

        private void CreateExplosion(float x, float y)
        {
            var explosion = new Animation();
            Explosions.Add(explosion);

            explosion.Initialize(ExplosionTexture,
                new Vector2(x, y),
                100,
                75,
                6,
                100,
                Color.White,
                this.ScaleX, 
                this.ScaleY,
                false);
        }

        public void UpdateScale(float scaleX, float scaleY, float screenWidth, float screenHeight)
        {
            foreach (var ship in Ships)
                ship.UpdateScale(scaleX, scaleY, screenWidth, screenHeight);
            
            foreach (var projectile in Projectiles)
                projectile.UpdateScale(StarShooter.ScaleToHighDPI(projectileSize * scaleX), StarShooter.ScaleToHighDPI(projectileSize * scaleY), screenWidth, screenHeight);
        }

        const float HITBOXSCALE = .5f;

        public List<Projectile> Projectiles { get; }

        public List<Ship> Ships { get; }

        public List<Animation> Explosions { get; }

        public Texture2D Texture
        {
            get;
            set;
        }

        public Texture2D ExplosionTexture
        {
            get;
            set;
        }

        public float ScaleX
        {
            get;
            set;
        }

        public float ScaleY
        {
            get;
            set;
        }
        public Texture2D LazerTexture { get; private set; }
    }
}
