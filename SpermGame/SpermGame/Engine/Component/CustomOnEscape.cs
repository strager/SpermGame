using System;
using Microsoft.Xna.Framework;
using SpermGame.Engine.Core;

namespace SpermGame.Engine.Component {
    class CustomOnEscape : CustomComponent<Action<Entity>>, IUpdated {
        private static readonly Property<bool> IsEscaped = new Property<bool>(false);

        private readonly BoundingBox bbox;

        public CustomOnEscape(BoundingBox bbox, Action<Entity> action) :
            base(action) {
            this.bbox = bbox;
        }

        public void Update(Entity e, GameTime t) {
            var body = Collidable.GetEffectiveBody(e);
            if (body == null) {
                this.SetEscaped(e, true);
                return;
            }

            // FIXME Not accurate at all
            bool isEscaped = !body.Bounds.Intersects(this.bbox);
            this.SetEscaped(e, isEscaped);
        }

        // Should probably abstract this out to something reusable.
        private void SetEscaped(Entity e, bool isEscaped) {
            e.Update(IsEscaped, (curIsEscaped) => {
                if (isEscaped != curIsEscaped) {
                    this.Action(e);
                }

                return isEscaped;
            });
        }
    }
}