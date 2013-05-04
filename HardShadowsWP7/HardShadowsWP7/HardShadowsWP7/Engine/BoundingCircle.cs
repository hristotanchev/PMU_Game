using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardShadows.Engine
{
    class BoundingCircle
    {
        private Vector2 center;
        public Vector2 Center
        {
            get
            {
                return center;
            }

            set
            {
                center = value;
            }
        }

        private float radius;
        public float Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }

        public BoundingCircle(Vector2 position, float radius)
        {
            this.center = position;
            this.radius = radius;
        }

        public BoundingCircle()
        {
            center = Vector2.Zero;
            radius = 0;
        }

        public bool Intersects(Vector2 p)
        {
            float deltaX = p.X - center.X;
            float deltaY = p.Y - center.Y;

            if (deltaX * deltaX + deltaY * deltaY < radius * radius)
            {
                return true;
            }

            return false;
        }

        public bool Intersects(BoundingCircle circle)
        {
            float centerDistanceSquared = (circle.Center - center).LengthSquared();
            float totalRadiusLength = circle.Radius + radius;

            if (centerDistanceSquared < totalRadiusLength * totalRadiusLength)
            {
                return true;
            }

            return false;
        }

        public bool Intersects(BoundingRect rect)
        {
            Vector2 circleDistance = Vector2.Zero;

            Vector2 rectSize = rect.Max - rect.Min;

            circleDistance.X = Math.Abs(center.X - (rect.Min.X + rectSize.X/2));
            circleDistance.Y = Math.Abs(center.Y - (rect.Min.Y + rectSize.Y / 2));

            if (circleDistance.X > rectSize.X / 2 + radius) return false;
            if (circleDistance.Y > rectSize.Y / 2 + radius) return false;

            if (circleDistance.X <= rectSize.X / 2) return true;
            if (circleDistance.Y <= rectSize.Y / 2) return true;

            float deltaX = circleDistance.X - rectSize.X / 2;
            float deltaY = circleDistance.Y - rectSize.Y / 2;

            float cornerDistanceSquared = deltaX * deltaX + deltaY * deltaY;

            return cornerDistanceSquared <= radius * radius;
        }
    }
}
