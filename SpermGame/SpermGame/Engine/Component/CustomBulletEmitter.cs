using System;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomBulletEmitter : CustomComponent<Action<Entity, Entity>>, IBulletEmitter {
        public CustomBulletEmitter(Action<Entity, Entity> action) :
            base(action) {
        }

        public void Emit(Entity e, Entity owner) {
            this.Action(e, owner);
        }
    }
}