using System;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    interface IDamager : IComponent {
        void Damaged(Entity e, Entity damaged, float damageDealt);
    }

    class CustomDamager : CustomComponent<Action<Entity, Entity, float>>, IDamager {
        public CustomDamager(Action<Entity, Entity, float> action) :
            base(action) {
        }

        public void Damaged(Entity e, Entity damaged, float damageDealt) {
            this.Action(e, damaged, damageDealt);
        }
    }
}
