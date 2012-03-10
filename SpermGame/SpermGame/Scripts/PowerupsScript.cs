using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpermGame.Engine.Component;
using SpermGame.Engine.Core;
using SpermGame.Engine.Physics;

namespace SpermGame.Scripts {
    class PowerupsScript : ScriptBase {
        public static readonly PowerupsScript Instance = new PowerupsScript();

        private Texture2D powerupTexture;

        private PowerupsScript() {
        }

        protected override void LoadContentImpl(ContentManager content) {
            base.LoadContentImpl(content);

            this.powerupTexture = content.Load<Texture2D>("powerup");
        }

        protected override void ExecuteImpl(EntityCollection entities) {
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
