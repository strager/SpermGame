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

        private readonly EntityManager entities = new EntityManager();

        public Game1() {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
        }

        void Init() {
            this.entities.Add(new Entity("player") {
                Textured.Instance,
                Order2Update.Instance,
                VelocityInputed.Instance,

                new CustomCollidable((e, other) => {
                    // Everything I touch dies!
                    entities.QueueDestroy(other);
                }),

                { Textured.Texture, this.Content.Load<Texture2D>("box") },
                { Located.Position, new Vector2(30, 30) },

                {
                    Collidable.Body,
                    new Body(new ShapePrimitive[] {
                        new CircleShape(Vector2.Zero, 32)
                    })
                },
            });

            this.entities.Add(new Entity("powerup") {
                Textured.Instance,

                { Textured.Texture, this.Content.Load<Texture2D>("powerup") },
                { Located.Position, new Vector2(200, 100) },

                {
                    Collidable.Body,
                    new Body(new ShapePrimitive[] {
                        new CircleShape(Vector2.Zero, 12)
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

            var kb = Keyboard.GetState(PlayerIndex.One);
            this.entities.ForEach<IKeyboardInputed>((e, c) => {
                c.Update(e, kb);
            });

            this.entities.ForEach<IUpdated>((e, c) => {
                c.Update(e, gameTime);
            });

            PhysicsModule.Update(this.entities);

            this.entities.End();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();

            this.entities.ForEach<Textured>((e, c) => {
                c.Draw(e, this.spriteBatch);
            });

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
