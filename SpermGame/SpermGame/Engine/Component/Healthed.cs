using System;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class Healthed : Core.Component {
        public static readonly Property<float> Health = new Property<float>(0.0f);
 
        public static float Damage(Entity e, float damageAmount) {
            float originalHealth = e.Get(Health);
            float finalHealth = Math.Max(0, originalHealth - damageAmount);
            e.Set(Health, finalHealth);

            if (finalHealth == 0) {
                e.ForEach<IKillable>((c) => {
                    c.Kill(e);
                });
            }

            return originalHealth - finalHealth;
        }
    }
}