using Microsoft.Xna.Framework.Input;
using SpermGame.Engine.Core;

namespace SpermGame.Engine {
    interface IKeyboardInputed : IComponent {
        void Update(Entity e, KeyboardState s);
    }
}
