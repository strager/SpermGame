using Microsoft.Xna.Framework;
using SpermGame.Engine.Core;

namespace SpermGame.Engine {
    interface IUpdated : IComponent {
        void Update(Entity e, GameTime t);
    }
}