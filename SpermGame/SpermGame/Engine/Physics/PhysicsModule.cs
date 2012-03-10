using System;
using System.Collections.Generic;
using System.Linq;
using SpermGame.Engine.Component;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Physics {
    static class PhysicsModule {
        public static void Update(EntityCollection entities) {
            // The state of collisions (positions, etc.) may change between
            // iterations of FindCollisions.  To make this clear, we've opted
            // for a callback function instead of an IEnumerable return value
            // to indicate collisions.
            //
            // For example, four balls are colliding.  Each ball, after being
            // notified of a collision, moves itself so that it does not
            // collide with any of the other balls.
            //
            // If we collected collisions before-hand, handling them later, we
            // would fire 4*3 = 12 collision events.
            //
            // If we collected and handled collisions as they were detected, we
            // would fire at only three collisions.

            FindCollisions(entities.EntitiesWith<Collidable>(), entities, Intersected);
        }

        private static void FindCollisions(
            IEnumerable<Entity> collidables,
            IEnumerable<Entity> all,
            Action<Entity, Entity> onCollision
        ) {
            var allList = all.ToList();
            foreach (var entityA in collidables) {
                var bodyA = entityA.Get(Collidable.Body);
                if (bodyA == null) {
                    // Collidable has no body.  Whatever... skip
                    continue;
                }

                var positionA = entityA.Get(Located.Position);

                foreach (var entityB in allList.Where((e) => e != entityA)) {
                    var bodyB = entityB.Get(Collidable.Body);
                    if (bodyB == null) {
                        // No body
                        continue;
                    }

                    var positionB = entityB.Get(Located.Position);

                    if (bodyA.Intersects(bodyB.Offset(positionA - positionB))) {
                        onCollision(entityA, entityB);
                    }
                }
            }
        }

        private static void Intersected(Entity a, Entity b) {
            a.ForEach<Collidable>((c) => {
                c.Collided(a, b);
            });

            b.ForEach<Collidable>((c) => {
                c.Collided(b, a);
            });
        }
    }
}
