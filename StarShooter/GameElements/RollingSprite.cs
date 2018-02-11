using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarShooter
{
    public class RollingSprite : SpriteClass
    {
        private Vector2 screenpos, origin, texturesize;
        private int screenheight;

        public RollingSprite(GraphicsDevice graphicsDevice, Texture2D texture, float scale) 
            : base(graphicsDevice, texture, scale)
        {
            screenheight = graphicsDevice.Viewport.Height;
            int screenwidth = graphicsDevice.Viewport.Width;
            // Set the origin so that we're drawing from the 
            // center of the top edge.
            origin = new Vector2(Texture.Width / 2, 0);
            // Set the screen position to the center of the screen.
            screenpos = new Vector2(screenwidth / 2, screenheight / 2);
            // Offset to draw the second texture, when necessary.
            texturesize = new Vector2(0, Texture.Height);
        }
        
        // ScrollingBackground.Update
        public override void Update(float elapsedTime)
        {
            screenpos.Y += elapsedTime;
            screenpos.Y = screenpos.Y % Texture.Height;
        }

        // ScrollingBackground.Draw
        public override void Draw(SpriteBatch batch)
        {
            // Draw the texture, if it is still onscreen.
            if (screenpos.Y < screenheight)
            {
                batch.Draw(Texture, screenpos, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            batch.Draw(Texture, screenpos - texturesize, null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}