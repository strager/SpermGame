using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    abstract class Collidable : Core.Component {
        public static readonly Property<Body> Body = new Property<Body>();

        public abstract void Collided(Entity e, Entity other);
    }
}
