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

        public ShipCollection(float scaleX, float scaleY)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;

            _shipCollection = new Collection<Ship>();
            _projectiles = new Collection<Projectile>();
        }

        public void Update(float elapsedTime, float screenWidth, float screenHeight)
        {
            //foreach (var ship in _shipCollection)
            //    ship.Update(elapsedTime, screenWidth, screenHeight);

            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                if (_projectiles[i].Y > screenHeight)
                    _projectiles.Remove(_projectiles[i]);
                else
                    _projectiles[i].Update(elapsedTime, screenWidth, screenHeight);
            }

            for (int i = _shipCollection.Count - 1; i >= 0; i--)
            {
                if (_shipCollection[i].Y > screenHeight)
                    _shipCollection.Remove(_shipCollection[i]);
                else
                    _shipCollection[i].Update(elapsedTime, screenWidth, screenHeight);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var ship in _shipCollection)
                ship.Draw(spriteBatch);

            foreach (var projectile in _projectiles)
                projectile.Draw(spriteBatch);
        }

        public bool CheckCollisions(Collection<Projectile> projectiles)
        {
            bool collision = false;
            for (int i = _shipCollection.Count - 1; i >= 0; i--)
                if (_shipCollection[i].CheckCollisions(projectiles))
                {
                    _shipCollection.Remove(_shipCollection[i]);
                    collision= true;
                }
            return collision;
        }

        public void UpdateScale(float scaleX, float scaleY, float screenWidth, float screenHeight)
        {
            foreach (var ship in _shipCollection)
                ship.UpdateScale(scaleX, scaleY, screenWidth, screenHeight);
        }

        const float HITBOXSCALE = .5f;

        public Collection<Projectile> Projectiles
        {
            get { return _projectiles; }
        }

        public Collection<Ship> Ships
        {
            get { return _shipCollection; }
        }

        public float ScaleX
        {
            get;
            set;
        }

        public float ScaleY
        {
            get;
            set;
        }
    }
}
