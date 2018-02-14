using Microsoft.Xna.Framework.Graphics;

namespace StarShooter
{
    public class Projectile : MoveOnResizeSprite
    {
        public Projectile(Texture2D texture, float scaleX, float scaleY)
            : base(texture, scaleX, scaleY)
        {
            Strength = 100;
        }
        
        public float Strength
        {
            get;
            set;
        }
    }
}