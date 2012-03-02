using System.Collections.Generic;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class Weaponized : Core.Component {
        public static readonly Weaponized Instance = new Weaponized();

        public static readonly Property<IEnumerable<Entity>> WeaponConfiguration = new Property<IEnumerable<Entity>>();

        public void Emit(Entity e) {
            var weaponConfig = e.Get(WeaponConfiguration);
            if (weaponConfig != null) {
                foreach (var weapon in weaponConfig) {
                    weapon.ForEach<IBulletEmitter>((c) => {
                        c.Emit(weapon, e);
                    });
                }
            }
        }
    }
}
