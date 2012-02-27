using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpermGame.Engine.Core {
    class EntityManager : IEnumerable<Entity> {
        private readonly IList<Entity> entities = new List<Entity>();

        private readonly IList<Entity> spawnQueue = new List<Entity>();
        private readonly IList<Entity> destroyQueue = new List<Entity>();
 
        public void Add(Entity e) {
            this.QueueSpawn(e);
        }

        public void QueueSpawn(Entity e) {
            this.spawnQueue.Add(e);
        }

        public void ForEach<T>(Action<Entity, T> callback) where T : IComponent {
            foreach (var e in this.entities) {
                e.ForEach<T>((c) => callback(e, c));
            }
        }

        public IEnumerable<Entity> EntitiesWith<T>() where T : IComponent {
            return this.entities.Where((e) => e.HasComponent<T>());
        }

        public IEnumerator<Entity> GetEnumerator() {
            return this.entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public void QueueDestroy(Entity e) {
            this.destroyQueue.Add(e);
        }

        public void Begin() {
            foreach (var e in this.spawnQueue) {
                this.entities.Add(e);
            }

            this.spawnQueue.Clear();
        }

        public void End() {
            foreach (var e in this.destroyQueue) {
                this.entities.Remove(e);
            }

            this.destroyQueue.Clear();
        }
    }
}
