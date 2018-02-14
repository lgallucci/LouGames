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

        public void UpdateScale(float scaleX, float scaleY, float screenWidth)
        {
            this.X = screenWidth * PercentOfWidth;
            base.UpdateScale(scaleX, scaleY);
        }

        public override void UpdateScale(float scaleX, float scaleY)
        {
            throw new NotImplementedException();
        }

        public void SetPosition(float x, float y, float dX, float dY, float screenWidth)
        {
            PercentOfWidth = x / screenWidth;
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
    }
}
