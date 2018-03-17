using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarShooter.GameElements
{
    public class LivesPanel : SpriteClass
    {
        float defaultScale;

        public LivesPanel(Texture2D texture, float scaleX, float scaleY)
            : base(texture, scaleX, scaleY)
        {
            defaultScale = scaleX;
            Width = 100; Height = 125;
        }

        public void Draw(SpriteBatch spriteBatch, float screenWidth)
        {
            var sourceRectangle = new Rectangle((int)this.Width, 0, (int)this.Width, (int)this.Height);
            for (int i = 0; i < Lives - 1; i++)
            {
                Vector2 spritePosition = new Vector2(screenWidth - 25 - ((i) * 50 * (ScaleX / defaultScale)), this.Y + 25);
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
