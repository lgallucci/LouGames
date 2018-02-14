using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarShooter
{
    public class LivesPanel : SpriteClass
    {
        float defaultScale;

        public LivesPanel(Texture2D texture, float scaleX, float scaleY)
            : base(texture, scaleX, scaleY)
        {
            defaultScale = scaleX;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle sourceRectangle, float screenWidth)
        {
            for (int i = 0; i < Lives - 1; i++)
            {
                Vector2 spritePosition = new Vector2(screenWidth - 10 - ((i + 1) * 50 * (ScaleX / defaultScale)), this.Y + 10);
                spriteBatch.Draw(Texture, spritePosition, sourceRectangle, Color.White, this.Angle, new Vector2(Width / 2, Height / 2), new Vector2(ScaleX, ScaleY), SpriteEffects.None, 0f);
            }
        }

        public int Lives
        {
            get;
            set;
        }
    }
}
