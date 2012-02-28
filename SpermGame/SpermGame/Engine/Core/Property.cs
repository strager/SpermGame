using System.Collections.Generic;

namespace SpermGame.Engine.Core {
    class Property<T> {
        // TODO Weak dictionary
        private readonly IDictionary<Entity, T> values = new Dictionary<Entity, T>();
        private readonly T defaultValue;

        public Property(T defaultValue = default(T)) {
            this.defaultValue = defaultValue;
        }

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

        public bool Has(Entity entity) {
            return this.values.ContainsKey(entity);
        }
    }
}