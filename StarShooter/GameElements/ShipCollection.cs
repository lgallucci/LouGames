using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarShooter
{
    public class ShipCollection
    {
        private Collection<Ship> _shipCollection;
        private Collection<Projectile> _projectiles;

        public ShipCollection(GraphicsDevice graphicsDevice, Texture2D texture, float scale)
        {
            this.Scale = scale;
            if (Texture == null)
            {
                Texture = texture;
            }
            _shipCollection = new Collection<Ship>();
            _projectiles = new Collection<Projectile>();
        }

        public void Update(float elapsedTime)
        {
            foreach (var ship in _shipCollection)
                ship.Update(elapsedTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var ship in _shipCollection)
                ship.Draw(spriteBatch);
        }

        public bool CheckCollisions(Collection<Projectile> projectiles)
        {
            foreach (var ship in _shipCollection)
                ship.CheckCollisions(projectiles);

            return true;
        }

        public void Freeze()
        {
            foreach (var ship in _shipCollection)
                ship.Freeze();
        }

        const float HITBOXSCALE = .5f;

        public Collection<Projectile> Projectiles
        {
            get { return _projectiles; }
        }

        public Texture2D Texture
        {
            get;
        }

        public float Scale
        {
            get;
            set;
        }
    }
}
