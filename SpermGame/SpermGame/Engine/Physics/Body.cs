using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SpermGame.Engine.Physics {
    class Body {
        private readonly IList<ShapePrimitive> shapes;

        public IEnumerable<ShapePrimitive> Shapes {
            get { return this.shapes; }
        }

        public BoundingBox Bounds {
            get {
                // We have a special first case because
                // CreateMerged does not work as expected
                // when merging (0,0,0 => 0,0,0).
                var bounds = this.shapes.Select((s) => s.Bounds);
                if (!bounds.Any()) {
                    return new BoundingBox();
                }

                var head = bounds.First();
                var tail = bounds.Skip(1);
                return tail.Aggregate(head, BoundingBox.CreateMerged);
            }
        }

        public Body(IEnumerable<ShapePrimitive> shapes) {
            this.shapes = shapes.ToList();
        }

        public Body Offset(Vector2 v) {
            return new Body(this.shapes.Select((s) => s.Offset(v)));
        }

        public bool Intersects(Body other) {
            return Intersects(this, other);
        }

        public static bool Intersects(Body a, Body b) {
            return a.Shapes.Any(
                (sa) => b.Shapes.Any(
                    (sb) => ShapePrimitive.Intersects(sa, sb)
                )
            );
        }
    }
}