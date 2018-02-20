﻿using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarShooter
{
    public class ShipCollection
    {
        private Collection<Ship> _shipCollection;
        private Collection<Projectile> _projectiles;

        public ShipCollection(Texture2D texture, float scaleX, float scaleY)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            if (Texture == null)
            {
                Texture = texture;
            }
            _shipCollection = new Collection<Ship>();
            _projectiles = new Collection<Projectile>();
        }

        public void Update(float elapsedTime, float screenHeight)
        {
            foreach (var ship in _shipCollection)
                ship.Update(elapsedTime);

            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                if (_projectiles[i].Y > screenHeight)
                    _projectiles.Remove(_projectiles[i]);
                else
                    _projectiles[i].Update(elapsedTime);
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
            foreach (var ship in _shipCollection)
                ship.CheckCollisions(projectiles);

            return true;
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

        public Texture2D Texture
        {
            get;
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
