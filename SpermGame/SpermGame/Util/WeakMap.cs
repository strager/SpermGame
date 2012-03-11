using System;
using System.Collections.Generic;

namespace SpermGame.Util {
    class WeakMap<TKey, TValue> where TKey : class {
        private readonly IDictionary<object, TValue> dictionary = new Dictionary<object, TValue>(new WeakKeyComparer<TKey>());

        public TValue this[TKey key] {
            set {
                this.dictionary[WeakReference<TKey>.Create(key)] = value;
            }
        }

        public bool TryGetValue(TKey key, out TValue value) {
            return this.dictionary.TryGetValue(key, out value);
        }

        public bool ContainsKey(TKey key) {
            return this.dictionary.ContainsKey(key);
        }
    }

    class WeakKeyComparer<T> : IEqualityComparer<object> where T : class {
        private readonly IEqualityComparer<T> comparer;

        public WeakKeyComparer() {
            this.comparer = EqualityComparer<T>.Default;
        }

        public bool Equals(object a, object b) {
            T targetA, targetB;

            if (!TryGetTarget(a, out targetA)) {
                return false;
            }
            if (!TryGetTarget(b, out targetB)) {
                return false;
            }

            return this.comparer.Equals(targetA, targetB);
        }

        public int GetHashCode(object obj) {
            T target;
            if (!TryGetTarget(obj, out target)) {
                target = null;
            }

            return this.comparer.GetHashCode(target);
        }

        private static bool TryGetTarget<T>(object wrapper, out T target) where T : class {
            var weakRef = wrapper as WeakReference<T>;
            if (weakRef != null) {
                if (weakRef.IsAlive) {
                    target = weakRef.Target;
                    return true;
                } else {
                    target = null;
                    return false;
                }
            }

            // weakRef is an ordinary object
            target = (T) wrapper;
            return true;
        }
    }

    class WeakReference<T> where T : class {
        private readonly WeakReference reference;

        private WeakReference(T obj) {
            this.reference = new WeakReference(obj);
        }

        public static WeakReference<T> Create(T obj) {
            if (obj == null) {
                throw new ArgumentNullException("obj");
            }

            return new WeakReference<T>(obj);
        }

        public bool IsAlive {
            get { return this.reference.IsAlive; }
        }

        public T Target {
            get { return (T) this.reference.Target; }
        }
    }
}
