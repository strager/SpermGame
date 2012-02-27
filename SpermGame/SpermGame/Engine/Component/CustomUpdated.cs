using System;
using Microsoft.Xna.Framework;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomUpdated : CustomComponent<Action<Entity, GameTime>>, IUpdated {
        public CustomUpdated(Action<Entity, GameTime> action)
            : base(action) {
        }

        public void Update(Entity e, GameTime t) {
            this.Action(e, t);
        }
    }
}
