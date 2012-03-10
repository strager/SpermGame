using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpermGame.Engine;
using SpermGame.Engine.Component;
using SpermGame.Engine.Core;
using SpermGame.Engine.Physics;

namespace SpermGame {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private static readonly EntityCollection Entities = new EntityCollection();

        public Game1() {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
        }

        void Init() {
            var texturePowerup = this.Content.Load<Texture2D>("powerup");
            var textureBox = this.Content.Load<Texture2D>("box");
            var textureBullet = this.Content.Load<Texture2D>("bullet");
            var textureEnemy = this.Content.Load<Texture2D>("enemy");

            var font = this.Content.Load<SpriteFont>("myfont");

            var destroysEnemies = new Property<bool>(false);
            var isPowerup = new Property<bool>(false);
            var damage = new Property<float>(1.0f);
            var score = new Property<uint>(0);
            var points = new Property<uint>(0);
            var owner = new Property<Entity>();

            var bulletP = new Entity("bullet") {
                Textured.Instance,
                Order2Update.Instance,

                new CustomOnEscape(new BoundingBox(
                    new Vector3(0, 0, 0),
                    new Vector3(640, 480, 0)
                ), (e) => {
                    Entities.EnqueueDestroy(e);
                }),

                { Textured.Texture, textureBullet },
                { destroysEnemies, true },
                { damage, 10.0f },

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
                    bullet.Set(owner, weaponOwner);
                    Entities.EnqueueSpawn(bullet);
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

            var weaponConfigs = new[] {
                new[] { weapons[2], },
                new[] { weapons[0], weapons[1], },
                new[] { weapons[0], weapons[1], weapons[2], },
            };

            var player = new Entity("player") {
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
                    if (other.Get(isPowerup)) {
                        Entities.EnqueueDestroy(other);

                        var weaponConfig = e.Get(Weaponized.WeaponConfiguration);
                        var nextWeaponConfig = weaponConfigs
                            .SkipWhile((wc) => wc != weaponConfig)
                            .Skip(1)
                            .FirstOrDefault();

                        if (nextWeaponConfig != null) {
                            e.Set(Weaponized.WeaponConfiguration, nextWeaponConfig);
                        }
                    }
                }),

                new CustomKiller((e, killed) => {
                    uint earnedPoints = killed.Get(points);
                    e.Update(score, (x) => earnedPoints + x);
                }),

                { Textured.Texture, textureBox },
                { Located.Position, new Vector2(30, 30) },
                { score, 0U },
                { Weaponized.WeaponConfiguration, weaponConfigs[0] },

                {
                    Collidable.Body,
                    new Body(new ShapePrimitive[] {
                        new CircleShape(Vector2.Zero, 32)
                    })
                },
            };

            Entities.EnqueueSpawn(player);

            Entities.EnqueueSpawn(new Entity("player score display") {
                Texted.Instance,

                new CustomUpdated((e, gt) => {
                    // I have a bad feeling about the UI being updated like
                    // other entities.  =\
                    e.Set(Texted.Text, player.Get(score).ToString());
                }),

                { Texted.Font, font },
                { Texted.Text, "hello world" },
            });

            var powerupP = new Entity("powerup") {
                Textured.Instance,

                { Textured.Texture, texturePowerup },
                { isPowerup, true },

                {
                    Collidable.Body,
                    new Body(new ShapePrimitive[] {
                        new CircleShape(Vector2.Zero, 12)
                    })
                },
            };

            Entities.EnqueueSpawn(powerupP.Create().Configure((e) => {
                e.Set(Located.Position, new Vector2(200, 100));
            }));

            Entities.EnqueueSpawn(powerupP.Create().Configure((e) => {
                e.Set(Located.Position, new Vector2(300, 300));
            }));

            var ms = new Property<uint>();

            Entities.EnqueueSpawn(new Entity("enemy") {
                Textured.Instance,

                new CustomUpdated((e, gt) => {
                    e.Update(ms, (t) => t + (uint) gt.ElapsedGameTime.TotalMilliseconds);

                    float d = (float) Math.Sin(e.Get(ms) / 1000.0f) * 50;
                    e.Set(Located.Position, new Vector2(0, d) + new Vector2(500, 100));
                }),

                new CustomCollidable((e, other) => {
                    if (other.Get(destroysEnemies)) {
                        Entities.EnqueueDestroy(other);

                        var ownerE = other.Get(owner);
                        Healthed.Damage(e, other.Get(damage), ownerE);
                    }
                }),

                new CustomKillable((e, killer) => {
                    Entities.EnqueueDestroy(e);
                }),

                { Textured.Texture, textureEnemy },
                { Located.Position, new Vector2(500, 100) },
                { Healthed.Health, 30.0f },
                { points, 50U },

                {
                    Collidable.Body,
                    new Body(new ShapePrimitive[] {
                        new CircleShape(Vector2.Zero, 16)
                    })
                },
            });
        }

        protected override void LoadContent() {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.Init();
        }

        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                this.Exit();
            }

            Entities.SpawnEntities();

            var kb = Keyboard.GetState(PlayerIndex.One);
            Entities.ForEach<IKeyboardInputed>((e, c) => {
                c.Update(e, kb);
            });

            Entities.ForEach<IUpdated>((e, c) => {
                c.Update(e, gameTime);
            });

            PhysicsModule.Update(Entities);

            Entities.DestroyEntities();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();

            Entities.ForEach<Textured>((e, c) => {
                c.Draw(e, this.spriteBatch);
            });

            Entities.ForEach<Texted>((e, c) => {
                c.Draw(e, this.spriteBatch);
            });

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
