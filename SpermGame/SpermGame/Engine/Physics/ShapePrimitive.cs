using System;
using Microsoft.Xna.Framework;

namespace SpermGame.Engine.Physics {
    abstract class ShapePrimitive {
        private static bool CantCollide(ShapePrimitive a, ShapePrimitive b) {
            throw new NotImplementedException(string.Format(
                "Collision testing between {0} and {1} not implemented",
                a.GetType(), b.GetType()
            ));
        }

        public static bool Intersects(ShapePrimitive a, ShapePrimitive b) {
            var aCircle = a as CircleShape;
            if (aCircle != null) { return Intersects(aCircle, b); }

            return CantCollide(a, b);
        }

        public static bool Intersects(CircleShape a, ShapePrimitive b) {
            var bCircle = b as CircleShape;
            if (bCircle != null) { return Intersects(a, bCircle); }

            return CantCollide(a, b);
        }

        public static bool Intersects(CircleShape a, CircleShape b) {
            float centerDist2 = (a.Center - b.Center).LengthSquared();
            float minDist = a.Radius + b.Radius;
            return centerDist2 <= minDist * minDist;
        }

        public abstract Vector2 Center { get; }
        public abstract BoundingBox Bounds { get; }

        public abstract ShapePrimitive Offset(Vector2 v);
    }
    
    sealed class CircleShape : ShapePrimitive {
        private readonly Vector2 center;
        public override Vector2 Center {
            get { return this.center; }
        }

        private readonly float radius;
        public float Radius {
            get {
                return this.radius;
            }
        }

        public CircleShape(Vector2 center, float radius) {
            this.center = center;
            this.radius = radius;
        }

        public override BoundingBox Bounds {
            get {
                float minX = this.center.X - this.radius;
                float minY = this.center.Y - this.radius;
                float maxX = this.center.X + this.radius;
                float maxY = this.center.Y + this.radius;

                return new BoundingBox(
                    new Vector3(minX, minY, 0),
                    new Vector3(maxX, maxY, 0)
                );
            }
        }

        public override ShapePrimitive Offset(Vector2 v) {
            return new CircleShape(this.center + v, radius);
        }
    }
}