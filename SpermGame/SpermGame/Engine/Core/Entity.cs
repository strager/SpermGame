using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpermGame.Engine.Core {
    class Entity : IEnumerable<IComponent> {
        public string Name { get; set; }

        private readonly IList<IComponent> components = new List<IComponent>();

        public void ForEach<T>(Action<T> callback) where T : IComponent {
            foreach (var c in this.components.OfType<T>()) {
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

        public void Add<T>(Property<T> prop, T value) {
            this.Set(prop, value);
        }

        public T Get<T>(Property<T> prop) {
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
    }
}
