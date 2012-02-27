using System;
using Microsoft.Xna.Framework;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomUpdated : Core.Component, IUpdated {
        private readonly Action<Entity, GameTime> onUpdated;

        public CustomUpdated(Action<Entity, GameTime> onUpdated) {
            if (onUpdated == null) {
                throw new ArgumentNullException("onUpdated");
            }

            this.onUpdated = onUpdated;
        }

        public void Update(Entity e, GameTime t) {
            this.onUpdated(e, t);
        }
    }
}
