using System;
using Microsoft.Xna.Framework;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class TimedEmitter : Core.Component, IUpdated {
        public static readonly Property<bool> IsEmitting = new Property<bool>(false);
        public static readonly Property<uint> EmitTime = new Property<uint>(100);

        private static readonly Property<uint> TimeToEmission = new Property<uint>(0);

        private readonly Action<Entity> onEmit;

        public TimedEmitter(Action<Entity> onEmit) {
            if (onEmit == null) {
                throw new ArgumentNullException("onEmit");
            }

            this.onEmit = onEmit;
        }

        public void Update(Entity e, GameTime t) {
            // We burn through just enough time for emission to occur.  We
            // update the state, then perform an emission.  Then we pick up
            // where we left off (when the loop loops).
            uint dt = (uint) t.ElapsedGameTime.TotalMilliseconds;
            while (dt > 0) {
                uint emitTime = e.Get(EmitTime);

                uint timeToEmission = e.Get(TimeToEmission);
                if (timeToEmission == 0) {
                    timeToEmission = emitTime;
                }

                uint burntTime = Math.Min(timeToEmission, dt);
                timeToEmission -= burntTime;
                dt -= burntTime;

                e.Set(TimeToEmission, timeToEmission);

                if (timeToEmission == 0 && e.Get(IsEmitting)) {
                    this.onEmit(e);
                }
            }
        }
    }
}