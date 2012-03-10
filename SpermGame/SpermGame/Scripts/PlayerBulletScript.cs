using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpermGame.Engine.Component;
using SpermGame.Engine.Core;
using SpermGame.Engine.Physics;

namespace SpermGame.Scripts {
    class PlayerBulletScript : ScriptBase {
        private Texture2D bulletTexture;

        private Entity[][] weaponConfigs;
        public Entity[][] WeaponConfigs {
            get { return this.weaponConfigs; }
        }

        protected override void LoadContentImpl(ContentManager content) {
            base.LoadContentImpl(content);

            this.bulletTexture = content.Load<Texture2D>("bullet");
        }

        public override void Execute(EntityCollection entities) {
            var bulletP = new Entity("bullet") {
                Textured.Instance,
                Order2Update.Instance,

                new CustomOnEscape(new BoundingBox(
                    new Vector3(0, 0, 0),
                    new Vector3(640, 480, 0)
                ), (e) => {
                    entities.EnqueueDestroy(e);
                }),

                { Textured.Texture, this.bulletTexture },
                { Properties.DestroysEnemies, true },
                { Properties.Damage, 10.0f },

                {
                    Collidable.Body,
                    new Body(new ShapePrimitive[] {
                        new CircleShape(Vector2.Zero, 8)
                    })
                },
            };

            var basicWeapon = new Entity {
                new CustomBulletEmitter((e, weaponOwner) => {
                    var bullet = bulletP.Create();
                    bullet.Set(Located.Position, e.Get(Located.Position) + weaponOwner.Get(Located.Position));
                    bullet.Set(Located.Velocity, e.Get(Located.Velocity));
                    bullet.Set(Properties.Owner, weaponOwner);
                    entities.EnqueueSpawn(bullet);
                })
            };

            var weapons = new[] {
                basicWeapon.Create("top").Configure((e) => {
                    e.Set(Located.Position, new Vector2(0, 0));
                    e.Set(Located.Velocity, new Vector2(7, 0));
                }),

                basicWeapon.Create("bottom").Configure((e) => {
                    e.Set(Located.Position, new Vector2(0, 30));
                    e.Set(Located.Velocity, new Vector2(7, 0));
                }),

                basicWeapon.Create("center").Configure((e) => {
                    e.Set(Located.Position, new Vector2(0, 15));
                    e.Set(Located.Velocity, new Vector2(7, 0));
                }),
            };

            this.weaponConfigs = new[] {
                new[] { weapons[2], },
                new[] { weapons[0], weapons[1], },
                new[] { weapons[0], weapons[1], weapons[2], },
            };
        }
    }
}
