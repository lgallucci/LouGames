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

        public void UpdateScale(float scaleX, float scaleY, float screenWidth, float screenHeight)
        {
            this.X = screenWidth * PercentOfWidth;
            this.Y = screenHeight * PercentOfHeight;
            base.UpdateScale(scaleX, scaleY);
        }

        public override void UpdateScale(float scaleX, float scaleY)
        {
            throw new NotImplementedException();
        }

        public void Update(float elapsedTime, float screenWidth, float screenHeight)
        {
            base.Update(elapsedTime);

            this.PercentOfHeight = Y / screenHeight;
            this.PercentOfWidth = X / screenWidth;
        }

        public override void Update(float elapsedTime)
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
