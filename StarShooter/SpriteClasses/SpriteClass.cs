using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarShooter
{
    public abstract class SpriteClass
    {
        float _height, _width;

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

            this.X += this.DX * elapsedTime;
            this.Y += this.DY * elapsedTime;
            this.Angle += this.DA * elapsedTime;

            _hitbox.X = (int)this.X - (_hitbox.Width / 2);
            _hitbox.Y = (int)this.Y - (_hitbox.Height / 2);
        }

        public virtual void SetPosition(float x, float y, float dX, float dY)
        {
            this.X = x;
            this.Y = y;
            this.DX = dX;
            this.DY = dY;
        }

        public virtual void UpdateScale(float scaleX, float scaleY)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            SetHitbox();
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color? color = null, Rectangle? sourceRectangle = null)
        {
            Vector2 spritePosition = new Vector2(this.X, this.Y);
            spriteBatch.Draw(Texture, spritePosition, sourceRectangle, color ?? Color.White, this.Angle, new Vector2(Width / 2, Height / 2), new Vector2(ScaleX, ScaleY), SpriteEffects.None, 0f);

#if DEBUG
            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
            spriteBatch.Draw(pixel, _hitbox, Color.LightGreen);
#endif
        }

        public bool RectangleCollision(SpriteClass otherSprite)
        {
            if (this.Hitbox.Intersects(otherSprite.Hitbox))
                return true;
            return false;
        }

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


        public Texture2D Texture
        {
            get;
        }

        public virtual float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Angle
        {
            get;
            set;
        }

        public float DX
        {
            get;
            set;
        }

        public float DY
        {
            get;
            set;
        }

        public float DA
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
