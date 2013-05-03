using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HardShadows.Engine;

namespace HardShadows.GamePlay
{
    class Player
    {
        private BoundingCircle boundingBody;
        public BoundingCircle BoundingBody
        {
            get
            {
                return boundingBody;
            }
        }
        
        private Vector2 position = Vector2.Zero;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private float scale;
        public int Size
        {
            get { return (int)(texture.Width/scale); }
            set { scale = texture.Width/value; }
        }

        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Player(Texture2D texture, Vector2 position, Color color, int size)
        {
            this.texture = texture;
            this.position = position;
            this.color = color;
            Size = size;

            boundingBody = new BoundingCircle(position, size / 2);
        }
    }
}
