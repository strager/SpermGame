using System;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomKiller : CustomComponent<Action<Entity, Entity>>, IKiller {
        public CustomKiller(Action<Entity, Entity> action) :
            base(action) {
        }

        public void Killed(Entity e, Entity killed) {
            this.Action(e, killed);
        }
    }
}