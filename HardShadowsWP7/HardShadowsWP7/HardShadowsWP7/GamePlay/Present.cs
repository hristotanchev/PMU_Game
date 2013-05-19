using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardShadows.GamePlay
{
    class Present
    {
        public Vector2 position;
        public Texture2D texture;
    
        public Present(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;            
        }

    }
}
