using System.Collections.Generic;
using System.Linq;

namespace SpermGame.Engine.Core {
    class EntityManager {
        private readonly IList<Entity> entities = new List<Entity>();
 
        public void Add(Entity e) {
            this.entities.Add(e);
        }

        public IEnumerable<Entity> WithComponent<T>() where T : Component {
            return this.entities.Where((e) => e.Component<T>() != null);
        }
    }
}
