using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarShooter.Particles;
using StarShooter.Utils;

namespace StarShooter.GameElements
{
    public class ShipCollection
    {
        Random rand;
        float projectileSize = .35f;
        float shipSpeed = GameRoot.ScaleToHighDPI(150f);

        public ShipCollection(Texture2D texture, float scaleX, float scaleY)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            this.Texture = texture;

            Ships = new List<Ship>();
            Projectiles = new List<Projectile>();
            Explosions = new List<Animation>();
            rand = new Random();
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
                if (Ships[i].Position.Y > screenHeight)
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

        private int CreateShips(GameTime gameTime, float screenWidth, double totalGameTime)
        {
            int score = 0;
            var enemyTicker = ((int)totalGameTime / 10);
            int enemyCount = enemyTicker > 100 ? 100 : enemyTicker;

            Color color1 = new Color(0, 50, 255);

            if (Ships.Count < enemyCount + 1)
            {
                var enemy = new Ship(this.Texture, GameRoot.ScaleToHighDPI(this.ScaleX), GameRoot.ScaleToHighDPI(this.ScaleY));
                enemy.Enemy = true; enemy.HITBOXSCALE = .9f;

                int maxEnemySpeed = (int)(shipSpeed + (float)totalGameTime);

                float enemyX = rand.Next(10, (int)screenWidth - 10);
                float enemyDY = rand.Next((int)shipSpeed, maxEnemySpeed > 500 ? 500 : maxEnemySpeed);
                float enemyDX = 0f;

                if (rand.Next(1, 10) == 1)
                {
                    if (enemyDY > 300)
                    {
                        enemy.Color = new Color(255, 100, 0);
                        enemyDY = 300f;
                        enemyDX = GetPositiveOrNegative() * rand.NextFloat(40, 80);
                    }
                    else
                    {
                        enemy.Color = new Color(255, 255, 50);
                        enemyDY = shipSpeed;
                        enemyDX = GetPositiveOrNegative() * rand.NextFloat(20, 40);
                    }
                    enemy.SetPosition(new Vector2(enemyX, 0), new Vector2(enemyDX, enemyDY));
                }
                else
                {
                    enemy.Color = GetEnemyColor(enemyDY);
                    enemy.SetPosition(new Vector2(enemyX, 0), new Vector2(0, enemyDY));
                }
                Ships.Add(enemy);
                score++;
            }

            return score;
        }

        private float GetPositiveOrNegative()
        {
            return rand.Next(1, 3) == 1 ? 1 : -1;
        }

        private Color GetEnemyColor(float enemySpeed)
        {
            if (enemySpeed <= 200) return new Color(0, 50, 255);
            else if (enemySpeed <= 300) return new Color(255, 100, 255);
            else if (enemySpeed <= 400) return new Color(50, 255, 50);
            else if (enemySpeed <= 500) return new Color(0, 255, 255);
            return new Color(0, 50, 255);
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
                    CreateExplosion(Ships[i].Position.X , Ships[i].Position.Y, Ships[i].Color);
                    Ships.Remove(Ships[i]);
                    collision = true;
                }
            return collision;
        }

        private void CreateExplosion(float x, float y, Color color1)
        {
            //float hue1 = rand.NextFloat(0, 6);
            //float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;

            //Color color1 = new Color(0, 51, 255);//ColorUtil.HSVToColor(hue1, 0.5f, 1);
            Color color2 = new Color(255, 50, 0);// ColorUtil.HSVToColor(hue2, 0.5f, 1);

            for (int i = 0; i < 30; i++)
            {
                float speed = 8f * (1f - 1 / rand.NextFloat(1, 10));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.Enemy,
                    LengthMultiplier = 1
                };

                Color color = Color.Lerp(color1, color2, (1f - speed / 10f));
                GameRoot.ParticleManager.CreateParticle(Art.LineParticle, new Vector2(x, y), color, 80, 3f, state);
            }
        }

        public void UpdateScale(float scaleX, float scaleY, float screenWidth, float screenHeight)
        {
            foreach (var ship in Ships)
                ship.UpdateScale(scaleX, scaleY, screenWidth, screenHeight);
            
            foreach (var projectile in Projectiles)
                projectile.UpdateScale(GameRoot.ScaleToHighDPI(projectileSize * scaleX), GameRoot.ScaleToHighDPI(projectileSize * scaleY), screenWidth, screenHeight);
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
    }
}
