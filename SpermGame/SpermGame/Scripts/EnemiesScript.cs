using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpermGame.Engine.Component;
using SpermGame.Engine.Core;
using SpermGame.Engine.Physics;

namespace SpermGame.Scripts {
    class EnemiesScript : ScriptBase {
        public static readonly EnemiesScript Instance = new EnemiesScript();

        private Texture2D enemyTexture;

        private EnemiesScript() {
        }

        protected override void LoadContentImpl(ContentManager content) {
            base.LoadContentImpl(content);

            this.enemyTexture = content.Load<Texture2D>("enemy");
        }

        protected override void ExecuteImpl(EntityCollection entities) {
            entities.EnqueueSpawn(new Entity("enemy") {
                Textured.Instance,

                new CustomUpdated((e, gt) => {
                    e.Update(Properties.Milliseconds, (t) => t + (uint) gt.ElapsedGameTime.TotalMilliseconds);

                    float d = (float) Math.Sin(e.Get(Properties.Milliseconds) / 1000.0f) * 50;
                    e.Set(Located.Position, new Vector2(0, d) + new Vector2(500, 100));
                }),

                new CustomCollidable((e, other) => {
                    if (other.Get(Properties.DestroysEnemies)) {
                        entities.EnqueueDestroy(other);

                        var ownerE = other.Get(Properties.Owner);
                        Healthed.Damage(e, other.Get(Properties.Damage), ownerE);
                    }
                }),

                new CustomKillable((e, killer) => {
                    entities.EnqueueDestroy(e);
                }),

                { Textured.Texture, this.enemyTexture },
                { Located.Position, new Vector2(500, 100) },
                { Healthed.Health, 30.0f },
                { Properties.Points, 50U },

                {
                    Collidable.Body,
                    new Body(new ShapePrimitive[] {
                        new CircleShape(Vector2.Zero, 16)
                    })
                },
            });
        }
    }
}
