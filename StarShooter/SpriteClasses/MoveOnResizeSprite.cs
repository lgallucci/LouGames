using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarShooter
{
    public class MoveOnResizeSprite : SpriteClass
    {
        public MoveOnResizeSprite(Texture2D texture, float scaleX, float scaleY)
            : base(texture, scaleX, scaleY)
        {
        }

        public virtual void UpdateScale(float scaleX, float scaleY, float screenWidth, float screenHeight)
        {
            this.Position.X = screenWidth * PercentOfWidth;
            this.Position.Y = screenHeight * PercentOfHeight;
            base.UpdateScale(scaleX, scaleY);
        }

        public override void UpdateScale(float scaleX, float scaleY)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(GameTime gameTime, float screenWidth, float screenHeight)
        {
            this.PercentOfHeight = Position.Y / screenHeight;
            this.PercentOfWidth = Position.X / screenWidth;

            base.Update(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public float PercentOfWidth
        {
            get;
            set;
        }

        public float PercentOfHeight
        {
            get;
            set;
        }
    }
}
