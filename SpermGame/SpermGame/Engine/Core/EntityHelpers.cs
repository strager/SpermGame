using System;
using System.Collections.Generic;

namespace SpermGame.Engine.Core {
    static class EntityHelpers {
        public static Entity Configure(this Entity e, Action<Entity> callback) {
            callback(e);
            return e;
        }

        public static Entity Configure<T>(this Entity e, Property<T> prop, T value) {
            e.Set(prop, value);
            return e;
        }

        public static Entity Configure(this Entity e, IEnumerable<IComponent> components) {
            e.Add(components);
            return e;
        }
    }
}
