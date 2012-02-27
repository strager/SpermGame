using Microsoft.Xna.Framework;
using SpermGame.Engine.Core;

namespace SpermGame.Engine {
    class Order2Update : Component, IUpdated {
        public static readonly Order2Update Instance = new Order2Update();

        public void Update(Entity e, GameTime t) {
            var v = e.Get(Located.Velocity);
            e.Update(Located.Position, (p) => p + v);
        }
    }
}
