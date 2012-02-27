using System;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class Healthed : Core.Component {
        public static readonly Property<float> Health = new Property<float>(0.0f);
 
        public static void Damage(Entity e, float damageAmount) {
            float health = e.Get(Health) - damageAmount;
            float finalHealth = Math.Max(0, health);
            e.Set(Health, finalHealth);

            if (finalHealth == 0) {
                e.ForEach<IKillable>((c) => {
                    c.Kill(e);
                });
            }
        }
    }
}