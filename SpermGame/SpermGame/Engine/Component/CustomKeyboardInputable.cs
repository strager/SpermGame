using System;
using Microsoft.Xna.Framework.Input;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomKeyboardInputable : CustomComponent<Action<Entity, KeyboardState>>, IKeyboardInputed {
        public CustomKeyboardInputable(Action<Entity, KeyboardState> action)
            : base(action) {
        }

        public void Update(Entity e, KeyboardState s) {
            this.Action(e, s);
        }
    }
}