using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    interface IKiller : IComponent {
        void Killed(Entity e, Entity killed);
    }
}
