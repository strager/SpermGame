using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    interface IKillable : IComponent {
        void Kill(Entity e, Entity killer = null);
    }
}