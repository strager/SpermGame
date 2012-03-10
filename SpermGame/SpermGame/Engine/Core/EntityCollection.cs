using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpermGame.Engine.Core {
    class EntityCollection : IEnumerable<Entity> {
        private readonly IList<Entity> entities = new List<Entity>();

        private readonly IList<Entity> spawnQueue = new List<Entity>();
        private readonly IList<Entity> destroyQueue = new List<Entity>();
 
        /// <summary>
        /// Enqueues entity for spawning when SpawnEntities is called.
        /// </summary>
        /// <param name="e"></param>
        public void Add(Entity e) {
            this.EnqueueSpawn(e);
        }

        public void EnqueueSpawn(Entity e) {
            this.spawnQueue.Add(e);
        }

        public void EnqueueDestroy(Entity e) {
            this.destroyQueue.Add(e);
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

        public void SpawnEntities() {
            foreach (var e in this.spawnQueue) {
                this.entities.Add(e);
            }

            this.spawnQueue.Clear();
        }

        public void DestroyEntities() {
            foreach (var e in this.destroyQueue) {
                this.entities.Remove(e);
            }

            this.destroyQueue.Clear();
        }
    }
}
