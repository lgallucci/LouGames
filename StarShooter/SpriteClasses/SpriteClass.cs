using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarShooter
{
    public abstract class SpriteClass
    {
        float _height, _width;

        public bool Active { get; set; }

        public float Height
        {
            get
            {
                if (_height <= 0)
                    return this.Texture.Height;
                else
                    return _height;
            }
            set { _height = value; SetHitbox(); }
        }

        public float Width
        {
            get
            {
                if (_width <= 0)
                    return this.Texture.Width;
                else
                    return _width;
            }
            set { _width = value; SetHitbox(); }
        }

        public float HITBOXSCALE { get; set; } = .75f;

        private Rectangle _hitbox = new Rectangle();
        public Rectangle Hitbox { get { return _hitbox; } }

        public Color Color { get; set; } = Color.White;
        
        public Texture2D Texture
        {
            get;
        }

        public Vector2 Size
        {
            get
            {
                return Texture == null ? Vector2.Zero : new Vector2(Texture.Width, Texture.Height);
            }
        }

        public Vector2 Position, Velocity;

        public float Angle
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

        public SpriteClass(Texture2D texture, float scaleX, float scaleY)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            if (Texture == null)
            {
                Texture = texture;
            }
            SetHitbox();
        }

        private void SetHitbox()
        {
            _hitbox.Width = (int)(Width * this.ScaleX * HITBOXSCALE);
            _hitbox.Height = (int)(Height * this.ScaleY * HITBOXSCALE);
        }

        public virtual void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity * elapsedTime;

            _hitbox.X = (int)this.Position.X - (_hitbox.Width / 2);
            _hitbox.Y = (int)this.Position.Y - (_hitbox.Height / 2);
        }

        public virtual void SetPosition(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public virtual void UpdateScale(float scaleX, float scaleY)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            SetHitbox();
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color? color = null, Rectangle? sourceRectangle = null)
        {
            spriteBatch.Draw(Texture, this.Position, sourceRectangle, color ?? this.Color, this.Angle, new Vector2(Width / 2, Height / 2), new Vector2(ScaleX, ScaleY), SpriteEffects.None, 0f);

#if DEBUG
            //Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //pixel.SetData(new[] { Color.White });
            //spriteBatch.Draw(pixel, _hitbox, Color.LightGreen);
#endif
        }

        public bool RectangleCollision(SpriteClass otherSprite)
        {
            if (this.Hitbox.Intersects(otherSprite.Hitbox))
                return true;
            return false;
        }
    }
}
