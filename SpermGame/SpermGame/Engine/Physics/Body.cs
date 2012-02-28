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
                // FIXME Not sure if this is correct for non-zero-centered bboxes
                return this.shapes.Aggregate(
                    new BoundingBox(),
                    (acc, x) => BoundingBox.CreateMerged(acc, x.Bounds)
                );
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