﻿using Microsoft.Xna.Framework.Graphics;

namespace StarShooter
{
    public class Projectile : SpriteClass
    {
        public Projectile(GraphicsDevice graphicsDevice, Texture2D texture, float scale) 
            : base(graphicsDevice, texture, scale)
        {
        }
    }
}