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
        private Vector2 origin;
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        private BoundingCircle boundingBody;
        public BoundingCircle BoundingBody
        {
            get { return boundingBody; }
        }

        private BoundingCircle triggerBody;
        public BoundingCircle TriggerBody
        {
            get { return triggerBody; }
        }
        
        private Vector2 position = Vector2.Zero;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; boundingBody.Center = position; triggerBody.Center = position; }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public int Size
        {
            get { return (int)(texture.Width/scale); }
            set { scale = ((float)value/texture.Width); }
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
            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            boundingBody = new BoundingCircle(position, size);
            triggerBody = new BoundingCircle(position, size);
        }
    }
}
