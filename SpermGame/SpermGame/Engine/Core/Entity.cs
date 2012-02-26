using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpermGame.Engine.Core {
    class Entity : IEnumerable<IComponent> {
        public string Name { get; set; }

        private readonly IList<IComponent> components = new List<IComponent>();

        public IEnumerable<IComponent> AllComponents {
            get { return this.components; }
        }

        public T Component<T>() where T : IComponent {
            return this.components.OfType<T>().FirstOrDefault();
        }

        public IComponent Component(Type t) {
            return this.components.Where(t.IsInstanceOfType).FirstOrDefault();
        }

        public void Add(IComponent c) {
            if (c == null) {
                throw new ArgumentNullException("c");
            }

            if (this.components.Contains(c)) {
                // FIXME Is this the best type of exception to throw?
                throw new ArgumentOutOfRangeException("c", "Component already added to entity");
            }

            if (this.Component(c.GetType()) != null) {
                // FIXME Is this the best type of exception to throw?
                throw new ArgumentOutOfRangeException("c", string.Format(
                    "Component of type {0} already added to entity",
                    c.GetType()
                ));
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
