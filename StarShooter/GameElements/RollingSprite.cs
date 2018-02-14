using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarShooter
{
    public class RollingSprite : SpriteClass
    {
        private Vector2 screenpos, origin, texturesize;
        private float _screenHeight;

        public RollingSprite(Texture2D texture, float scaleX, float scaleY, float screenWidth, float screenHeight)
            : base(texture, scaleX, scaleY)
        {
            _screenHeight = screenHeight;
            // Set the origin so that we're drawing from the 
            // center of the top edge.
            origin = new Vector2(Texture.Width / 2, 0);
            // Set the screen position to the center of the screen.
            screenpos = new Vector2(screenWidth / 2, _screenHeight / 2);
            // Offset to draw the second texture, when necessary.
            texturesize = new Vector2(0, Texture.Height);
        }

        public override void Update(float elapsedTime)
        {
            screenpos.Y += elapsedTime;
            screenpos.Y = screenpos.Y % Texture.Height;
        }

        public override void Draw(SpriteBatch batch, Color? color = null, Rectangle? sourceRectangle = null)
        {
            // Draw the texture, if it is still onscreen.
            if (screenpos.Y < _screenHeight)
            {
                batch.Draw(Texture, screenpos, null,
                     color ?? Color.White, 0, origin, new Vector2(this.ScaleX, this.ScaleY), SpriteEffects.None, 0f);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            batch.Draw(Texture, screenpos - texturesize, null,
                 color ?? Color.White, 0, origin, new Vector2(this.ScaleX, this.ScaleY), SpriteEffects.None, 0f);
        }

        public void UpdateScale(float scaleX, float scaleY, float screenWidth, float screenHeight)
        {
            _screenHeight = screenHeight;

            origin = new Vector2(Texture.Width / 2, 0);

            screenpos = new Vector2(screenWidth / 2, _screenHeight / 2);

            texturesize = new Vector2(0, Texture.Height);

            base.UpdateScale(scaleX, scaleY);
        }
    }
}