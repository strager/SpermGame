using System;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomCollidable : CustomComponent<Action<Entity, Entity>>, ICollidable {
        public CustomCollidable(Action<Entity, Entity> action) :
            base(action) {
        }

        public void Collided(Entity e, Entity other) {
            this.Action(e, other);
        }
    }
}
