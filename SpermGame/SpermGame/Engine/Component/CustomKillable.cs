using System;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomKillable : CustomComponent<Action<Entity>>, IKillable {
        public CustomKillable(Action<Entity> action) :
            base(action) {
        }

        public void Kill(Entity e) {
            this.Action(e);
        }
    }
}