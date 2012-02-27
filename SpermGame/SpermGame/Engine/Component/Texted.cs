using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class Texted : Core.Component {
        public static readonly Texted Instance = new Texted();

        public static readonly Property<SpriteFont> Font = new Property<SpriteFont>();
        public static readonly Property<string> Text = new Property<string>();

        public void Draw(Entity e, SpriteBatch b) {
            b.DrawString(e.Get(Font), e.Get(Text), e.Get(Located.Position), Color.White);
        }
    }
}