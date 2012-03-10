using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    interface ICollidable : IComponent {
        void Collided(Entity e, Entity other);
    }
}