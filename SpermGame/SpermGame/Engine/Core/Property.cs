using System.Diagnostics.Contracts;
using SpermGame.Util;

namespace SpermGame.Engine.Core {
    class Property<T> {
        private readonly WeakMap<Entity, T> values = new WeakMap<Entity, T>();
        private readonly T defaultValue;

        public Property(T defaultValue = default(T)) {
            this.defaultValue = defaultValue;
        }

        [Pure]
        internal T Get(Entity entity) {
            T value;
            if (this.values.TryGetValue(entity, out value)) {
                return value;
            } else {
                return this.defaultValue;
            }
        }

        internal void Set(Entity entity, T value) {
            this.values[entity] = value;
        }

        [Pure]
        internal bool Has(Entity entity) {
            return this.values.ContainsKey(entity);
        }
    }
}