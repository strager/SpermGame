using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpermGame.Engine.Component;
using SpermGame.Engine.Core;
using SpermGame.Engine.Physics;

namespace SpermGame.Scripts {
    class PowerupsScript : ScriptBase {
        private Texture2D powerupTexture;

        protected override void LoadContentImpl(ContentManager content) {
            base.LoadContentImpl(content);

            this.powerupTexture = content.Load<Texture2D>("powerup");
        }

        public override void Execute(EntityCollection entities) {
            var powerupP = new Entity("powerup") {
                Textured.Instance,

                { Textured.Texture, this.powerupTexture },
                { Properties.IsPowerup, true },

                {
                    Collidable.Body,
                    new Body(new ShapePrimitive[] {
                        new CircleShape(Vector2.Zero, 12)
                    })
                },
            };

            entities.EnqueueSpawn(powerupP.Create().Configure((e) => {
                e.Set(Located.Position, new Vector2(200, 100));
            }));

            entities.EnqueueSpawn(powerupP.Create().Configure((e) => {
                e.Set(Located.Position, new Vector2(300, 300));
            }));

        }
    }
}
