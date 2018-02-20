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

        public void SetPosition(float x, float y, float dX, float dY, float screenWidth, float screenHeight)
        {
            PercentOfWidth = x / screenWidth;
            PercentOfHeight = y / screenHeight;
            base.SetPosition(x, y, dX, dY);
        }

        public override void SetPosition(float x, float y, float dX, float dY)
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
