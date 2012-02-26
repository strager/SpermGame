using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpermGame.Engine.Core;

namespace SpermGame.Engine {
    class VelocityInputed : Component, IKeyboardInputed {
        public static readonly VelocityInputed Instance = new VelocityInputed();

        public static readonly Property<float> MovementSpeed = new Property<float>(1);
 
        public void Update(Entity e, KeyboardState s) {
            var v = Vector2.Zero;
            if (s.IsKeyDown(Keys.Left )) { v.X -= 1; }
            if (s.IsKeyDown(Keys.Right)) { v.X += 1; }
            if (s.IsKeyDown(Keys.Down )) { v.Y += 1; }
            if (s.IsKeyDown(Keys.Up   )) { v.Y -= 1; }

            if (v != Vector2.Zero) {
                v.Normalize();
            }

            e.Set(Located.Velocity, v * e.Get(MovementSpeed));
        }
    }
}
