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
        }

        public virtual void Update(float elapsedTime)
        {
            this.X += this.DX * elapsedTime;
            this.Y += this.DY * elapsedTime;
            this.Angle += this.DA * elapsedTime;
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
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color? color = null, Rectangle? sourceRectangle = null)
        {
            Vector2 spritePosition = new Vector2(this.X, this.Y);
            spriteBatch.Draw(Texture, spritePosition, sourceRectangle, color ?? Color.White, this.Angle, new Vector2(Width / 2, Height / 2), new Vector2(ScaleX, ScaleY), SpriteEffects.None, 0f);
        }

        public bool RectangleCollision(SpriteClass otherSprite)
        {
            if (this.X + Width * this.ScaleX * HITBOXSCALE / 2 < otherSprite.X - otherSprite.Texture.Width * otherSprite.ScaleX / 2) return false;
            if (this.Y + Height * this.ScaleY * HITBOXSCALE / 2 < otherSprite.Y - otherSprite.Texture.Height * otherSprite.ScaleY / 2) return false;
            if (this.X - Width * this.ScaleX * HITBOXSCALE / 2 > otherSprite.X + otherSprite.Texture.Width * otherSprite.ScaleX / 2) return false;
            if (this.Y - Height * this.ScaleY * HITBOXSCALE / 2 > otherSprite.Y + otherSprite.Texture.Height * otherSprite.ScaleY / 2) return false;
            return true;
        }

        public float Height
        {
            get
            {
                if (_height < 0)
                    return this.Texture.Height;
                else
                    return _height;
            }
            set { _height = value; }
        }

        public float Width
        {
            get
            {
                if (_width < 0)
                    return this.Texture.Height;
                else
                    return _width;
            }
            set { _width = value; }
        }

        const float HITBOXSCALE = .75f;

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
