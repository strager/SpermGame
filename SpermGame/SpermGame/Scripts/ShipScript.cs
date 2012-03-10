using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpermGame.Engine.Component;
using SpermGame.Engine.Core;
using SpermGame.Engine.Physics;

namespace SpermGame.Scripts {
    [ScriptDepends(typeof(ShipBulletScript))]
    class ShipScript : ScriptBase {
        public static readonly ShipScript Instance = new ShipScript();

        private Texture2D shipTexture;

        public Entity Ship {
            get;
            private set;
        }

        private ShipScript() {
        }

        protected override void LoadContentImpl(ContentManager content) {
            base.LoadContentImpl(content);

            this.shipTexture = content.Load<Texture2D>("box");
        }

        protected override void ExecuteImpl(EntityCollection entities) {
            var shipBulletScript = ShipBulletScript.Instance;

            this.Ship = new Entity("ship proto") {
                Textured.Instance,
                Order2Update.Instance,
                VelocityInputed.Instance,
                Weaponized.Instance,

                new TimedEmitter((e) => {
                    e.ForEach<Weaponized>((c) => {
                        c.Emit(e);
                    });
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
        }
    }
}
