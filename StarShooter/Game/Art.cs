namespace StarShooter
{
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework;


    public static class Art
    {
        public static Texture2D Ship { get; private set; }        
        public static Texture2D StarsBack { get; private set; }
        public static Texture2D StarsFront { get; private set; }
        public static Texture2D Swarm { get; private set; }
        public static Texture2D Lazer { get; private set; }

        public static Texture2D BlackHole { get; private set; }

        public static Texture2D LineParticle { get; private set; }
        public static Texture2D Glow { get; private set; }
        public static Texture2D Pixel { get; private set; }     // a single white pixel

        public static Texture2D GameOver { get; private set; }

        public static SpriteFont ScoreFont { get; private set; }
        public static SpriteFont ScoreFontSmall { get; private set; }
        public static SpriteFont StateFont { get; private set; }
        public static SpriteFont StateFontMedium { get; private set; }

        public static void Load(ContentManager content)
        {
            ScoreFont = content.Load<SpriteFont>("Score");
            ScoreFontSmall = content.Load<SpriteFont>("ScoreSmall");
            StateFont = content.Load<SpriteFont>("GameState");
            StateFontMedium = content.Load<SpriteFont>("GameStateMedium");
            GameOver = content.Load<Texture2D>("game-over");
            Ship = content.Load<Texture2D>("ship");
            StarsBack = content.Load<Texture2D>("starsBack");
            StarsFront = content.Load<Texture2D>("starsFront");
            Swarm = content.Load<Texture2D>("swarm");
            Lazer = content.Load<Texture2D>("lazer");


            LineParticle = content.Load<Texture2D>("line");
            Glow = content.Load<Texture2D>("glow");

            Pixel = new Texture2D(Ship.GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }
    }
}
