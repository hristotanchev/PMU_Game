using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardShadows.Engine
{
    class BoundingRect
    {
        private Vector2 min;
        public Vector2 Min
        {
            get
            {
                return min;
            }

            set
            {
                min = value;
            }
        }

        private Vector2 max;
        public Vector2 Max
        {
            get
            {
                return max;
            }

            set
            {
                max = value;
            }
        }

        public BoundingRect()
        {
            min = Vector2.Zero;
            max = Vector2.Zero;
        }

        public BoundingRect(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
        }

        public BoundingRect(int minX, int minY, int maxX, int maxY)
        {
            min = new Vector2(minX, minY);
            max = new Vector2(maxX, maxY);
        }

        public bool Intersects(Vector2 p)
        {
            if (p.X > min.X && p.Y > min.Y && p.X < max.X && p.Y < max.Y)
            {
                return true;
            }

            return false;
        }

        public bool Intersects(BoundingCircle circle)
        {

            return circle.Intersects(this);
        }

        public bool Intersects(BoundingRect rect)
        {
            return false;
        }

        public void CreateFromPoints(IEnumerable<Vector2> points)
        {
            if (points.Count() < 1)
                return;

            float minX = points.ElementAt(0).X;
            float maxX = points.ElementAt(0).X;
            float minY = points.ElementAt(0).Y;
            float maxY = points.ElementAt(0).Y;

            foreach (Vector2 point in points)
            {
                minX = point.X < minX ? point.X : minX;
                maxX = point.X > maxX ? point.X : maxX;
                minY = point.Y < minY ? point.Y : minY;
                maxY = point.Y > maxY ? point.Y : maxY;
            }

            min.X = minX;
            min.Y = minY;
            max.X = maxX;
            max.Y = maxY;
        }
    }
}
