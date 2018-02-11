using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarShooter
{
    public abstract class SpriteClass
    {
        public SpriteClass(GraphicsDevice graphicsDevice, Texture2D texture, float scale)
        {
            this.Scale = scale;
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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spritePosition = new Vector2(this.X, this.Y);
            spriteBatch.Draw(Texture, spritePosition, null, Color.White, this.Angle, new Vector2(Texture.Width / 2, Texture.Height / 2), new Vector2(Scale, Scale), SpriteEffects.None, 0f);
        }

        public bool RectangleCollision(SpriteClass otherSprite)
        {
            if (this.X + this.Texture.Width * this.Scale * HITBOXSCALE / 2 < otherSprite.X - otherSprite.Texture.Width * otherSprite.Scale / 2) return false;
            if (this.Y + this.Texture.Height * this.Scale * HITBOXSCALE / 2 < otherSprite.Y - otherSprite.Texture.Height * otherSprite.Scale / 2) return false;
            if (this.X - this.Texture.Width * this.Scale * HITBOXSCALE / 2 > otherSprite.X + otherSprite.Texture.Width * otherSprite.Scale / 2) return false;
            if (this.Y - this.Texture.Height * this.Scale * HITBOXSCALE / 2 > otherSprite.Y + otherSprite.Texture.Height * otherSprite.Scale / 2) return false;
            return true;
        }

        public void Freeze()
        {
            throw new NotImplementedException();
        }

        const float HITBOXSCALE = .5f;

        public Texture2D Texture
        {
            get;
        }

        public float X
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

        public float Scale
        {
            get;
            set;
        }
    }
}
