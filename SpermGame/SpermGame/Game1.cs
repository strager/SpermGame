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
using SpermGame.Scripts;

namespace SpermGame {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private readonly EntityCollection entities = new EntityCollection();

        public Game1() {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
        }

        void Init() {
            var scripts = new ScriptBase[] {
                new PlayerScript(),
                new PowerupsScript(),
                new EnemiesScript(),
            };

            foreach (var script in scripts) {
                script.LoadContent(this.Content);
            }

            foreach (var script in scripts) {
                script.Execute(entities);
            }
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

            this.entities.SpawnEntities();

            var kb = Keyboard.GetState(PlayerIndex.One);
            this.entities.ForEach<IKeyboardInputed>((e, c) => {
                c.Update(e, kb);
            });

            this.entities.ForEach<IUpdated>((e, c) => {
                c.Update(e, gameTime);
            });

            PhysicsModule.Update(this.entities);

            this.entities.DestroyEntities();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();

            this.entities.ForEach<Textured>((e, c) => {
                c.Draw(e, this.spriteBatch);
            });

            this.entities.ForEach<Texted>((e, c) => {
                c.Draw(e, this.spriteBatch);
            });

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
