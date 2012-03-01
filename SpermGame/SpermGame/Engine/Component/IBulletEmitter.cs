using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    interface IBulletEmitter : IComponent {
        void Emit(Entity e, Entity owner);
    }
}