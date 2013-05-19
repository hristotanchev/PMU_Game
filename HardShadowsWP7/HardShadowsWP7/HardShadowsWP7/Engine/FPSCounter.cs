using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardShadows.Engine
{
    class FPSCounter
    {
        private SpriteFont font;
        public SpriteFont Font
        {
            get { return font; }
        }

        private int totalFrames;
        public int TotalFrames
        {
            get { return totalFrames; }
            set { totalFrames = value; }
        }

        public float elapsedTime;
        private int fps;

        public int FPS
        {
            get { return fps; }
        }

        public FPSCounter(SpriteFont FPSFont)
        {
            font = FPSFont;
            totalFrames = 0;
            elapsedTime = 70000;
            fps = 40;
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= 0)
                fps = (int)elapsedTime/1000;
        }
    }
}
