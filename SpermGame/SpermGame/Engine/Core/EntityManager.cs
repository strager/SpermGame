using System;
using System.Collections.Generic;

namespace SpermGame.Engine.Core {
    class EntityManager {
        private readonly IList<Entity> entities = new List<Entity>();
 
        public void Add(Entity e) {
            this.entities.Add(e);
        }

        public void ForEach<T>(Action<Entity, T> callback) where T : IComponent {
            foreach (var e in this.entities) {
                e.ForEach<T>((c) => callback(e, c));
            }
        }
    }
}
