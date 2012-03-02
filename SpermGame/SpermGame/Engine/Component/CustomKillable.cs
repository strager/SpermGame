using System;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomKillable : CustomComponent<Action<Entity, Entity>>, IKillable {
        public CustomKillable(Action<Entity, Entity> action) :
            base(action) {
        }

        public void Kill(Entity e, Entity killer = null) {
            this.Action(e, killer);
        }
    }
}