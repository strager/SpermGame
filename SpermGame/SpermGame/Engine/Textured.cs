using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpermGame.Engine.Core;

namespace SpermGame.Engine {
    class Textured : Component {
        public static readonly Textured Instance = new Textured();

        public static readonly Property<Texture2D> Texture = new Property<Texture2D>();

        public void Draw(Entity e, SpriteBatch b) {
            // TODO Position
            b.Draw(e.Get(Texture), e.Get(Located.Location), Color.White);
        }
    }
}
