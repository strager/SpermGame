using Microsoft.Xna.Framework;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    interface IUpdated : IComponent {
        void Update(Entity e, GameTime t);
    }
}