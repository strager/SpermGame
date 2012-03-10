using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpermGame.Engine.Component;
using SpermGame.Engine.Core;
using SpermGame.Engine.Physics;

namespace SpermGame.Scripts {
    class ShipScript : ScriptBase {
        public static readonly ShipScript Instance = new ShipScript();

        private Texture2D shipTexture;
        private SpriteFont font;

        private ShipScript() {
        }

        protected override void LoadContentImpl(ContentManager content) {
            base.LoadContentImpl(content);

            this.shipTexture = content.Load<Texture2D>("box");
            this.font = content.Load<SpriteFont>("myfont");
        }

        protected override void ExecuteImpl(EntityCollection entities) {
            var shipBulletScript = ShipBulletScript.Instance;
            shipBulletScript.Execute(entities);

            var ship = new Entity("ship") {
                Textured.Instance,
                Order2Update.Instance,
                VelocityInputed.Instance,
                Weaponized.Instance,

                new TimedEmitter((e) => {
                    e.ForEach<Weaponized>((c) => {
                        c.Emit(e);
                    });
                }),

                new CustomKeyboardInputable((e, s) => {
                    e.Set(TimedEmitter.IsEmitting, s.IsKeyDown(Keys.X));
                }),

                new CustomCollidable((e, other) => {
                    if (other.Get(Properties.IsPowerup)) {
                        entities.EnqueueDestroy(other);

                        var weaponConfig = e.Get(Weaponized.WeaponConfiguration);
                        var nextWeaponConfig = shipBulletScript.WeaponConfigs
                            .SkipWhile((wc) => wc != weaponConfig)
                            .Skip(1)
                            .FirstOrDefault();

                        if (nextWeaponConfig != null) {
                            e.Set(Weaponized.WeaponConfiguration, nextWeaponConfig);
                        }
                    }
                }),

                new CustomKiller((e, killed) => {
                    uint earnedPoints = killed.Get(Properties.Points);
                    e.Update(Properties.Score, (x) => earnedPoints + x);
                }),

                { Textured.Texture, this.shipTexture },
                { Located.Position, new Vector2(30, 30) },
                { Properties.Score, 0U },
                { Weaponized.WeaponConfiguration, shipBulletScript.WeaponConfigs[0] },

                {
                    Collidable.Body,
                    new Body(new ShapePrimitive[] {
                        new CircleShape(Vector2.Zero, 32)
                    })
                },
            };

            entities.EnqueueSpawn(ship);

            entities.EnqueueSpawn(new Entity("ship score display") {
                Texted.Instance,

                new CustomUpdated((e, gt) => {
                    // I have a bad feeling about the UI being updated like
                    // other entities.  =\
                    e.Set(Texted.Text, ship.Get(Properties.Score).ToString());
                }),

                { Texted.Font, font },
                { Texted.Text, "hello world" },
            });

        }
    }
}
