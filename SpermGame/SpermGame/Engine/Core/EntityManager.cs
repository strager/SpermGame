using System;
using System.Collections.Generic;
using System.Linq;

namespace SpermGame.Engine.Core {
    class EntityManager {
        private readonly IList<Entity> entities = new List<Entity>();
 
        public void Add(Entity e) {
            this.entities.Add(e);
        }

        public IEnumerable<Entity> WithComponent<T>() where T : IComponent {
            return this.entities.Where((e) => e.Component<T>() != null);
        }

        public void ForEach<T>(Action<Entity, T> callback) where T : IComponent {
            foreach (var e in this.WithComponent<T>()) {
                callback(e, e.Component<T>());
            }
        }
    }
}
