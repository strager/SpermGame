using System;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomCollidable : Collidable {
        private readonly Action<Entity, Entity> onCollided;

        public CustomCollidable(Action<Entity, Entity> onCollided) {
            if (onCollided == null) {
                throw new ArgumentNullException("onCollided");
            }

            this.onCollided = onCollided;
        }

        public override void Collided(Entity e, Entity other) {
            this.onCollided(e, other);
        }
    }
}
