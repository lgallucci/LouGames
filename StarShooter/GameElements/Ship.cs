

namespace StarShooter.GameElements
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using StarShooter.Particles;
    using StarShooter.Utils;

    public enum ShipDirection
    {
        Left = 0,
        Middle = 1,
        Right = 2
    }

    public class Ship : MoveOnResizeSprite
    {
        float projectileSpeed = GameRoot.ScaleToHighDPI(150f);

        float projectileSize = .5f;
        float shipSpeed = GameRoot.ScaleToHighDPI(400f);
        ShipDirection _direction = ShipDirection.Middle;

        private bool blinking;
        double blinkTime = 0;
        double explosionTime = 0;
        Random rand = new Random();

        public Ship(Texture2D texture, float scaleX, float scaleY)
            : base(texture, scaleX, scaleY)
        {
            Projectiles = new List<Projectile>();
        }

        public void Draw(SpriteBatch spriteBatch, float alpha)
        {
            Rectangle? sourceRectangle = null;

            if (!Enemy)
            {
                if (_direction == ShipDirection.Left)
                {
                    sourceRectangle = new Rectangle(0, 0, (int)this.Width, (int)this.Height);
                }
                else if (_direction == ShipDirection.Middle)
                {
                    sourceRectangle = new Rectangle((int)this.Width, 0, (int)this.Width, (int)this.Height);
                }
                else
                {
                    sourceRectangle = new Rectangle((int)this.Width * 2, 0, (int)this.Width, (int)this.Height);
                }
            }

            if (explosionTime == 0)
                this.Draw(spriteBatch, blinking ? Color.White * (alpha / 255) : Color.White, sourceRectangle);

            foreach (var projectile in Projectiles)
                projectile.Draw(spriteBatch);

        }

        public override void UpdateScale(float scaleX, float scaleY, float screenWidth, float screenHeight)
        {
            foreach (var projectile in Projectiles)
                projectile.UpdateScale(GameRoot.ScaleToHighDPI(projectileSize * scaleX), GameRoot.ScaleToHighDPI(projectileSize * scaleY), screenWidth, screenHeight);

            base.UpdateScale(scaleX, scaleY, screenWidth, screenHeight);
        }

        public override void Update(GameTime gameTime, float screenWidth, float screenHeight)
        {
            if (this.Position.X > screenWidth)
            {
                this.Position.X = 0;
            }

            if (this.Position.X < 0)
            {
                this.Position.X = screenWidth;
            }

            base.Update(gameTime, screenWidth, screenHeight);
        }

        private void MakeExhaustFire(GameTime gameTime)
        {
            double t = gameTime.TotalGameTime.TotalSeconds;

            Color sideColor = new Color(200, 38, 9);    // deep red
            Color midColor = new Color(255, 187, 30);   // orange-yellow
            Vector2 pos = new Vector2(Position.X, Position.Y + (Height / 3f));  // position of the ship's exhaust pipe.

            Vector2 baseVel = new Vector2(0, 3f);
            Vector2 velMid = baseVel + rand.NextVector2(0, 1);

            GameRoot.ParticleManager.CreateParticle(Art.Glow, pos, midColor, 5f, new Vector2(0.5f, 1),
                new ParticleState(velMid, ParticleType.Enemy));

            GameRoot.ParticleManager.CreateParticle(Art.Glow, pos + rand.NextVector2(0, 5), sideColor, 5f, new Vector2(0.5f, 1),
                new ParticleState(baseVel, ParticleType.Enemy));
            GameRoot.ParticleManager.CreateParticle(Art.Glow, pos - rand.NextVector2(0, 5), sideColor, 5f, new Vector2(0.5f, 1),
                new ParticleState(baseVel, ParticleType.Enemy));
        }

        public void Update(GameTime gameTime, float screenWidth, float screenHeight, float topBoundary)
        {
            Velocity += shipSpeed * Input.GetMovementDirection();

            base.Update(gameTime, screenWidth, screenHeight);

            Position = Vector2.Clamp(Position, new Vector2(0, (GameRoot.ScreenSize.Y / 2 + Size.Y / 2)), new Vector2(GameRoot.ScreenSize.X, (GameRoot.ScreenSize.Y - Size.Y / 2)));

            _direction = Velocity.X < 0 ? ShipDirection.Left : Velocity.X > 0 ? ShipDirection.Right : ShipDirection.Middle;

            if (explosionTime == 0)
            {
                CreateProjectiles(gameTime);

                MakeExhaustFire(gameTime);
            }

            for (int i = Projectiles.Count - 1; i >= 0; i--)
            {
                if (Projectiles[i].Position.Y <= 0)
                    Projectiles.Remove(Projectiles[i]); //_projectiles[i].Active = false;
                else
                    Projectiles[i].Update(gameTime, screenWidth, screenHeight);
            }

            if (explosionTime > 0 && (gameTime.TotalGameTime.TotalSeconds - explosionTime) > 2)
            {
                explosionTime = 0;
                Position.X = screenWidth / 2;
                Position.Y = screenHeight;
                blinkTime = gameTime.TotalGameTime.TotalSeconds;
            }

            if (blinkTime > 0 && (gameTime.TotalGameTime.TotalSeconds - blinkTime) > 5)
            {
                blinkTime = 0;
                blinking = false;
            }

            Velocity = Vector2.Zero;
        }

        int projectileInterval = 500;
        double lastProjectile = 0;
        int projectileCount = 1;
        private void CreateProjectiles(GameTime gameTime)
        {
            if (Projectiles.Count < projectileCount && (lastProjectile + projectileInterval) < gameTime.TotalGameTime.TotalMilliseconds)
            {
                var projectile = new Projectile(Art.Lazer, GameRoot.ScaleToHighDPI(projectileSize * ScaleX), GameRoot.ScaleToHighDPI(projectileSize * ScaleY));

                projectile.SetPosition(new Vector2(Position.X, Position.Y - (Height / 2)), new Vector2(0, -3 * projectileSpeed));
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
                        explosionTime = gameTime.TotalGameTime.TotalSeconds;
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
                        explosionTime = gameTime.TotalGameTime.TotalSeconds;
                        blinking = true;
                        CreateExplosion(Position.X, Position.Y, Color.White);
                        return true;
                    }
                }
            }
            return false;
        }


        private void CreateExplosion(float x, float y, Color color1)
        {
            //float hue1 = rand.NextFloat(0, 6);
            //float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;

            //Color color1 = new Color(0, 51, 255);//ColorUtil.HSVToColor(hue1, 0.5f, 1);
            Color color2 = new Color(255, 50, 0);// ColorUtil.HSVToColor(hue2, 0.5f, 1);

            for (int i = 0; i < 120; i++)
            {
                float speed = 16f * (1f - 1 / rand.NextFloat(1, 10));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.Enemy,
                    LengthMultiplier = 1
                };

                Color color = Color.Lerp(color1, color2, (1f - speed / 10f));
                GameRoot.ParticleManager.CreateParticle(Art.LineParticle, new Vector2(x, y), color, 80, 2f, state);
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

        public bool Enemy { get; set; }

        public List<Projectile> Projectiles { get; }

        internal void UpdateProjectiles(int score)
        {
            if (score < 250 && projectileCount != 1) projectileCount = 1; projectileInterval = (1000 / projectileCount);
            if (score > 250 && projectileCount == 1) projectileCount = 2; projectileInterval = (1000 / projectileCount);
            if (score > 1000 && projectileCount == 2) projectileCount = 3; projectileInterval = (1000 / projectileCount);
            if (score > 1500 && projectileCount == 3) projectileCount = 4; projectileInterval = (1500 / projectileCount);
            if (score > 2000 && projectileCount == 4) projectileCount = 5; projectileInterval = (1500 / projectileCount);
        }
    }
}
