using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class Weaponized : Core.Component {
        public static readonly Weaponized Instance = new Weaponized();

        public static readonly Property<WeaponConfiguration> WeaponConfiguration = new Property<WeaponConfiguration>();

        public void Emit(Entity e) {
            var weaponConfig = e.Get(WeaponConfiguration);
            if (weaponConfig != null) {
                foreach (var weapon in weaponConfig.Weapons) {
                    weapon.ForEach<IBulletEmitter>((c) => {
                        c.Emit(weapon, e);
                    });
                }
            }
        }
    }
}
