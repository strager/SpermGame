using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SpermGame.Engine.Core {
    class Entity : IEnumerable<IComponent> {
        private readonly Entity prototype = null;

        [Pure]
        private IEnumerable<Entity> PrototypeChain() {
            yield return this;

            if (this.prototype != null) {
                foreach (var e in this.prototype.PrototypeChain()) {
                    yield return e;
                }
            }
        }

        private readonly IList<IComponent> components = new List<IComponent>();

        private readonly string name;
        public string Name {
            [Pure]
            get { return this.name; }
        }

        public Entity(string name = null) {
            this.name = name;
        }

        private Entity(Entity prototype, string name = null) :
            this(name) {
            this.prototype = prototype;
        }

        [Pure]
        public override string ToString() {
            var pchain = this.PrototypeChain().ToList();
            var names = pchain.Select((p) => string.IsNullOrEmpty(p.Name) ? "(unnamed)" : p.Name);

            return "<Entity " + string.Join(" : ", names) + ">";
        }

        [Pure]
        public bool HasComponent<T>() where T : IComponent {
            return this.Components<T>().Any();
        }

        [Pure]
        private IEnumerable<T> Components<T>() where T : IComponent {
            foreach (var c in this.components.OfType<T>()) {
                yield return c;
            }

            if (this.prototype != null) {
                foreach (var c in this.prototype.Components<T>()) {
                    yield return c;
                }
            }
        }

        public void ForEach<T>(Action<T> callback) where T : IComponent {
            foreach (var c in this.Components<T>()) {
                callback(c);
            }
        }

        public void Add(IComponent c) {
            if (c == null) {
                throw new ArgumentNullException("c");
            }

            this.components.Add(c);
            c.Initialize(this);
        }

        public void Add(IEnumerable<IComponent> cs) {
            foreach (var c in cs) {
                this.Add(c);
            }
        }

        public void Add<T>(Property<T> prop, T value) {
            this.Set(prop, value);
        }

        [Pure]
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
    }
}
