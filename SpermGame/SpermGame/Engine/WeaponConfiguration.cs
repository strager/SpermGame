using System.Collections.Generic;
using System.Linq;
using SpermGame.Engine.Core;

namespace SpermGame.Engine {
    class WeaponConfiguration {
        private readonly IEnumerable<Entity> weapons;

        public IEnumerable<Entity> Weapons {
            get { return this.weapons; }
        }

        public WeaponConfiguration(IEnumerable<Entity> weapons) {
            this.weapons = weapons.ToList();
        }
    }
}