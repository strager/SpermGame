using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpermGame.Engine.Core;

namespace SpermGame.Engine {
    class TextureComponent : Component {
        public static readonly TextureComponent Instance = new TextureComponent();

        public static readonly Property<Texture2D> Texture = new Property<Texture2D>();

        public void Draw(Entity e, SpriteBatch b) {
            // TODO Position
            b.Draw(e.Get(Texture), Vector2.Zero, Color.White);
        }
    }
}
