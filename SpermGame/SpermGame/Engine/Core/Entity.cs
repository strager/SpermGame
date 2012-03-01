using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpermGame.Engine.Core {
    class Entity : IEnumerable<IComponent> {
        private readonly Entity prototype = null;

        private readonly IList<IComponent> components = new List<IComponent>();

        private readonly string name;
        public string Name {
            get { return this.name; }
        }

        public Entity(string name = null) {
            this.name = name;
        }

        private Entity(Entity prototype, string name = null) :
            this(name) {
            this.prototype = prototype;
        }

        public bool HasComponent<T>() {
            return this.components.OfType<T>().Any();
        }

        public void ForEach<T>(Action<T> callback) where T : IComponent {
            foreach (var c in this.components.OfType<T>()) {
                callback(c);
            }

            if (this.prototype != null) {
                this.prototype.ForEach(callback);
            }
        }

        public void Add(IComponent c) {
            if (c == null) {
                throw new ArgumentNullException("c");
            }

            this.components.Add(c);
            c.Initialize(this);
        }

        public void Add<T>(Property<T> prop, T value) {
            this.Set(prop, value);
        }

        public T Get<T>(Property<T> prop) {
            if (this.prototype != null) {
                if (!prop.Has(this)) {
                    // Walk up the prototype chain
                    return this.prototype.Get(prop);
                }
            }

            return prop.Get(this);
        }

        public void Set<T>(Property<T> prop, T value) {
            prop.Set(this, value);
        }

        public void Update<T>(Property<T> prop, Func<T, T> update) {
            this.Set(prop, update(this.Get(prop)));
        }

        // C# is a superdouche and doesn't allow generic indexers!
        /*
        public T this[Property<T> prop] {
            get { return prop.Get(this); }
            set { prop.Set(this, value); }
        }
        */

        public IEnumerator<IComponent> GetEnumerator() {
            return this.components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public Entity Create(string name = null) {
            return new Entity(this, name);
        }

        public Entity Configure(Action<Entity> callback) {
            callback(this);
            return this;
        }
    }
}
