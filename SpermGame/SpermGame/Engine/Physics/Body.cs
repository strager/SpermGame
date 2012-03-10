using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Xna.Framework;
using SpermGame.Util;

namespace SpermGame.Engine.Physics {
    class Body {
        private readonly ReadOnlyCollection<ShapePrimitive> shapes;

        // Bounds cache
        private BoundingBox2? bounds = null;

        public IEnumerable<ShapePrimitive> Shapes {
            [Pure]
            get { return this.shapes; }
        }

        public BoundingBox2 Bounds {
            get {
                if (this.bounds.HasValue) {
                    return this.bounds.Value;
                } else {
                    var bounds = CalculateBounds(this.shapes);
                    this.bounds = bounds;
                    return bounds;
                }
            }
        }

        [Pure]
        private static BoundingBox2 CalculateBounds(IEnumerable<ShapePrimitive> shapes) {
            var shapeBounds = shapes.Select((s) => s.Bounds).ToList();
            if (!shapeBounds.Any()) {
                return new BoundingBox2();
            }

            // We have a special first case because
            // CreateMerged does not work as expected
            // when merging (0,0,0 => 0,0,0).
            // (wtb foldl1)
            var head = shapeBounds.First();
            var tail = shapeBounds.Skip(1);
            return tail.Aggregate(head, BoundingBox2.CreateMerged);
        }

        public Body(IEnumerable<ShapePrimitive> shapes) {
            this.shapes = new ReadOnlyCollection<ShapePrimitive>(shapes.ToList());
        }

        [Pure]
        public Body Offset(Vector2 v) {
            return new Body(this.shapes.Select((s) => s.Offset(v)));
        }

        [Pure]
        public bool Intersects(Body other) {
            return Intersects(this, other);
        }

        [Pure]
        public static bool Intersects(Body a, Body b) {
            return a.Shapes.Any(
                (sa) => b.Shapes.Any(
                    (sb) => ShapePrimitive.Intersects(sa, sb)
                )
            );
        }
    }
}