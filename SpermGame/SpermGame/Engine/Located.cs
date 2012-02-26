using Microsoft.Xna.Framework;
using SpermGame.Engine.Core;

namespace SpermGame.Engine {
    class Located {
        public static readonly Property<Vector2> Position = new Property<Vector2>(Vector2.Zero);
        public static readonly Property<Vector2> Velocity = new Property<Vector2>(Vector2.Zero);
    }
}
