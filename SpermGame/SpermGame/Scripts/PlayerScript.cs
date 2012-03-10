using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpermGame.Engine.Component;
using SpermGame.Engine.Core;
using SpermGame.Engine.Physics;

namespace SpermGame.Scripts {
    [ScriptDepends(typeof(ShipScript))]
    class PlayerScript : ScriptBase {
        public static readonly PlayerScript Instance = new PlayerScript();

        private SpriteFont font;

        protected override void LoadContentImpl(Microsoft.Xna.Framework.Content.ContentManager content) {
            base.LoadContentImpl(content);

            this.font = content.Load<SpriteFont>("myfont");
        }

        protected override void ExecuteImpl(EntityCollection entities) {
            var shipScript = ShipScript.Instance;

            var player = shipScript.Ship.Create("player")
                .Configure(new IComponent[] {
                    new CustomKeyboardInputable((e, s) => {
                        e.Set(TimedEmitter.IsEmitting, s.IsKeyDown(Keys.X));
                    }),

                    new CustomKiller((e, killed) => {
                        uint earnedPoints = killed.Get(Properties.Points);
                        e.Update(Properties.Score, (x) => earnedPoints + x);
                    }),
                })
                .Configure(
                    Collidable.Body,
                    new Body(new ShapePrimitive[] {
                        new CircleShape(Vector2.Zero, 32)
                    })
                )
                .Configure(Located.Position, new Vector2(50, 50));

            entities.EnqueueSpawn(player);

            entities.EnqueueSpawn(new Entity("ship score display") {
                Texted.Instance,

                new CustomUpdated((e, gt) => {
                    // I have a bad feeling about the UI being updated like
                    // other entities.  =\
                    e.Set(Texted.Text, player.Get(Properties.Score).ToString());
                }),

                { Texted.Font, font },
                { Texted.Text, "hello world" },
            });
        }
    }
}
