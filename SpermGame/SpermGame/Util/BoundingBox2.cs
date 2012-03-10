using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;

namespace SpermGame.Util {
    struct BoundingBox2 {
        public readonly Vector2 Min;
        public readonly Vector2 Max;

        public Vector2 Center {
            [Pure]
            get {
                return this.Min + (this.Max - this.Min) / 2;
            }
        }

        public BoundingBox2(Vector2 a, Vector2 b) {
            this.Min = VectorMin(a, b);
            this.Max = VectorMax(a, b);
        }

        [Pure]
        private static Vector2 VectorMin(Vector2 a, Vector2 b) {
            return new Vector2(
                Math.Min(a.X, b.X),
                Math.Min(a.Y, b.Y)
            );
        }

        [Pure]
        private static Vector2 VectorMax(Vector2 a, Vector2 b) {
            return new Vector2(
                Math.Max(a.X, b.X),
                Math.Max(a.Y, b.Y)
            );
        }

        [Pure]
        public static BoundingBox2 CreateMerged(BoundingBox2 a, BoundingBox2 b) {
            return new BoundingBox2(
                VectorMin(a.Min, b.Min),
                VectorMax(a.Max, b.Max)
            );
        }

        [Pure]
        private BoundingBox ToBoundingBox3() {
            return new BoundingBox(
                new Vector3(this.Min, 0),
                new Vector3(this.Max, 0)
            );
        }

        [Pure]
        public bool Intersects(BoundingBox2 other) {
            return this.ToBoundingBox3().Intersects(other.ToBoundingBox3());
        }
    }
}
