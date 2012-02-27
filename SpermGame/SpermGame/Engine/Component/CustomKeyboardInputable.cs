using System;
using Microsoft.Xna.Framework.Input;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomKeyboardInputable : Core.Component, IKeyboardInputed {
        private readonly Action<Entity, KeyboardState> onUpdated;

        public CustomKeyboardInputable(Action<Entity, KeyboardState> onUpdated) {
            if (onUpdated == null) {
                throw new ArgumentNullException("onUpdated");
            }

            this.onUpdated = onUpdated;
        }

        public void Update(Entity e, KeyboardState s) {
            this.onUpdated(e, s);
        }
    }
}